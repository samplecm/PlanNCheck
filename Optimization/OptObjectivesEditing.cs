using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plan_n_Check.Plans;
using Plan_n_Check.Calculate;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Plan_n_Check.Optimization
{
    public static class OptObjectivesEditing
    {
        public static List<List<Structure>> SetConstraints(ref ExternalPlanSetup plan_setup, Plan plan_obj, List<List<Structure>> matchingStructures, bool checkOptis = false)
        {
            StructureSet ss = plan_setup.StructureSet;
            //Will be checking if opti structures need to be made. If so, they will be created and used for optimization. So we need a new matching list
            //which contains structures (opti or not) that are actually used during optimization.
            List<List<Structure>> optimizedStructures = new List<List<Structure>>();
            if (!checkOptis)
            {
                optimizedStructures = new List<List<Structure>>(matchingStructures);
                for (int i = 0; i < plan_obj.ROIs.Count; i++)
                {
                    plan_obj.ROIs[i].OptimizationStructures = new List<Structure>(plan_obj.ROIs[i].MatchingStructures);
                }
                return optimizedStructures;
            }


            //Need to set all optimization constraints now. First clear all the current constraints
            foreach (var objective in plan_setup.OptimizationSetup.Objectives.OfType<OptimizationPointObjective>())
            {

                plan_setup.OptimizationSetup.RemoveObjective(objective);
            }
            foreach (var objective in plan_setup.OptimizationSetup.Objectives.OfType<OptimizationMeanDoseObjective>())
            {
                plan_setup.OptimizationSetup.RemoveObjective(objective);


            }
            //Now loop over all constraints and set them
            for (int i = 0; i < plan_obj.ROIs.Count; i++)    //Loop over all structures
            {
 
                plan_obj.ROIs[i].OptimizationStructures = new List<Structure>(); 
                optimizedStructures.Add(new List<Structure>());
                

                for (int match = 0; match < matchingStructures[i].Count; match++)
                {

                    //Here I want to make sure that the matched structure does not overlap with PTVs, and if it does I want to make an opti structure. 

                    
                    if (!plan_obj.ROIs[i].Critical)
                    {
                        Structure opti = CheckOverlap_OptiMaker(matchingStructures[i][match], ss, false);
                        optimizedStructures[i].Add(opti);
                        plan_obj.ROIs[i].OptimizationStructures.Add(opti);

                    }
                    else
                    {
                        Structure opti = CheckOverlap_OptiMaker(matchingStructures[i][match], ss, true);                            
                        optimizedStructures[i].Add(opti);
                        plan_obj.ROIs[i].OptimizationStructures.Add(opti);


                    }
                    
                    if (plan_obj.ROIs[i].OptimizationStructures[match].Volume < 0.5)
                    {
                        continue;
                    }


                    for (int j = 0; j < plan_obj.ROIs[i].Constraints.Count; j++)    //Loop over all constraints for the current structure
                    {

                        string type = plan_obj.ROIs[i].Constraints[j].Type;
                        string subscript = plan_obj.ROIs[i].Constraints[j].Subscript;
                        string relation = plan_obj.ROIs[i].Constraints[j].EqualityType;
                        double value = plan_obj.ROIs[i].Constraints[j].Value;
                        string format = plan_obj.ROIs[i].Constraints[j].Format;
                        double volume = plan_obj.ROIs[i].OptimizationStructures[match].Volume;

                        OptimizationObjectiveOperator constraintType;
                        if (relation == "<")
                        {
                            constraintType = OptimizationObjectiveOperator.Upper;
                        }
                        else
                        {
                            constraintType = OptimizationObjectiveOperator.Lower;
                        }
                        if (type.ToLower() == "d") //if a dose constraint
                        {
                            try //subscript is a number
                            {

                                volume = Convert.ToInt32(subscript); //need to analyze DVH data if a number.

                                //convert to cGy (90% of maximum for optimization)
                                plan_setup.OptimizationSetup.AddPointObjective(plan_obj.ROIs[i].OptimizationStructures[match],
                                    constraintType, new DoseValue(value, "cGy"), volume, plan_obj.ROIs[i].Constraints[j].Priority);
                            }
                            catch //is a mean or max restriction
                            {
                                if (subscript.ToLower() == "mean")
                                {
                                    double dose = value * 0.9; //take 90 percent of dose constraint dose to start
                                    OptimizationObjective objective = plan_setup.OptimizationSetup.AddMeanDoseObjective(plan_obj.ROIs[i].OptimizationStructures[match],
                                        new DoseValue(dose, "cGy"), plan_obj.ROIs[i].Constraints[j].Priority);
                                    //update subsegment constraints if organ is subsegmented
                                    if (plan_obj.ROIs[i].HasSubsegments)
                                    {
                                        //replace whole organ constraint with subsegment constraints

                                        //plan.OptimizationSetup.RemoveObjective(objective); //uncomment if don't also want the whole organ constraint
                                        foreach (Structure structure in ss.Structures)
                                        {
                                            if ((structure.Id.Contains(plan_obj.ROIs[i].OptimizationStructures[match].Id.Substring(0, 5))) && (structure.Id.ToLower().Contains("subseg")))
                                            {
                                                plan_setup.OptimizationSetup.AddMeanDoseObjective(structure,
                                        new DoseValue(dose, "cGy"), plan_obj.ROIs[i].Constraints[j].Priority);
                                            }
                                        }
                                    }

                                }
                                else if (subscript.ToLower() == "max")
                                {   
                                    double dose = value * 0.9; //take 90 percent of constraint dose to start

                                    if (plan_obj.ROIs[i].IsPTV) //if ptv, go halfway between the max dose and prescription dose 
                                    {
                                        dose = (value + (double)plan_obj.ROIs[i].PTVDose) * 0.5;
                                    }
                                    OptimizationObjective objective = plan_setup.OptimizationSetup.AddPointObjective(plan_obj.ROIs[i].OptimizationStructures[match], constraintType,
                                        new DoseValue(dose, "cGy"), 0, plan_obj.ROIs[i].Constraints[j].Priority);
                                    if (plan_obj.ROIs[i].HasSubsegments)
                                    {
                                        //replace whole organ constraint with subsegment constraints
                                        //plan.OptimizationSetup.RemoveObjective(objective); //uncomment if don't also want the whole organ constraint
                                        foreach (Structure structure in ss.Structures)
                                        {
                                            if ((structure.Id.Contains(plan_obj.ROIs[i].OptimizationStructures[match].Id.Substring(0, 5))) && (structure.Id.Contains("subseg")))
                                            {
                                                plan_setup.OptimizationSetup.AddPointObjective(plan_obj.ROIs[i].OptimizationStructures[match], constraintType,
                                                new DoseValue(dose, "cGy"), 0, plan_obj.ROIs[i].Constraints[j].Priority);
                                            }
                                        }
                                    }

                                }
                                else if (subscript.ToLower() == "min")
                                {
                                    double dose = value * 1.05; //take 110% percent of constraint dose to start
                                    OptimizationObjective objective = plan_setup.OptimizationSetup.AddPointObjective(plan_obj.ROIs[i].OptimizationStructures[match], constraintType,
                                        new DoseValue(dose, "cGy"), 100, plan_obj.ROIs[i].Constraints[j].Priority);
                                    if (plan_obj.ROIs[i].HasSubsegments)
                                    {
                                        //replace whole organ constraint with subsegment constraints
                                        //plan.OptimizationSetup.RemoveObjective(objective); //uncomment if don't also want the whole organ constraint
                                        foreach (Structure structure in ss.Structures)
                                        {
                                            if ((structure.Id.Contains(plan_obj.ROIs[i].OptimizationStructures[match].Id.Substring(0, 5))) && (structure.Id.Contains("subseg")))
                                            {
                                                plan_setup.OptimizationSetup.AddPointObjective(plan_obj.ROIs[i].OptimizationStructures[match], constraintType,
                                                new DoseValue(dose, "cGy"), 100, plan_obj.ROIs[i].Constraints[j].Priority);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        else if (type.ToLower() == "v")
                        {
                            try //in case can't parse the subscript into Int32
                            {
                                if (format.ToLower() == "abs") //I assume here that an absolute volume constraint will always be an upper bound OAR consatraint
                                {
                                    double dose = Convert.ToInt32(subscript); //convert to cGy
                                    //Need to convert to relative volume. 
                                    volume = value / volume * 100;
                                    volume *= 0.9; //Make it an even harsher constraint
                                    if (volume <= 0.2)
                                    {
                                        volume = 0;
                                    }
                                    plan_setup.OptimizationSetup.AddPointObjective(plan_obj.ROIs[i].OptimizationStructures[match], constraintType,
                                        new DoseValue(dose * 0.95, "cGy"), volume, plan_obj.ROIs[i].Constraints[j].Priority);

                                }
                                else  //if relative, will belong to a PTV
                                {
                                    double dose = StringOperations.FindPTVNumber(plan_obj.ROIs[i].OptimizationStructures[match].Id.ToLower()) * 100;
                                    volume = value;
                                    volume = 100; //set to 100 to push the optimizer
                                    plan_setup.OptimizationSetup.AddPointObjective(plan_obj.ROIs[i].OptimizationStructures[match], constraintType,
                                       new DoseValue(dose, "cGy"), volume, plan_obj.ROIs[i].Constraints[j].Priority);
                                }
                            }
                            catch
                            {

                            }
                        }



                    }
                }
            }
            return optimizedStructures;
        }
        public static Structure CheckOverlap_OptiMaker(Structure structure, StructureSet ss, bool isCritical)
        {

            if (structure.Id.ToLower().Contains("ptv") || structure.Id.ToLower().Contains("body")) //don't check if it is a ptv already or body contours...
            {
                return structure;
            }
            else if (isCritical)
            {
                return structure;
            }

            int nameLen = Math.Min(structure.Id.Length, 7);
            string name = "PC_opti_" + structure.Id.Substring(0, nameLen);

            
            foreach (Structure struc in ss.Structures.ToList())
            {
                if (struc.Id == name)
                {
                    ss.RemoveStructure(struc);
                }
            }
            Structure opti;
            try
            {
                opti= ss.AddStructure("CONTROL", name);
            }
            catch
            {
                name = name.Substring(0, 15) + "2";
                opti = ss.AddStructure("CONTROL", name);
            }
           

            if (structure.IsHighResolution) //fix geometry error when resolutions are diff
            {
                var mesh = structure.MeshGeometry.Bounds;
                var mesh_low = _GetSlice(mesh.Z, ss);
                var mesh_high = _GetSlice(mesh.Z + mesh.SizeZ, ss) + 1;
                for (int j = mesh_low; j <= mesh_high; j++)
                {
                    var contours = structure.GetContoursOnImagePlane(j);
                    if (contours.Length > 0)
                    {
                        opti.AddContourOnImagePlane(contours[0], j);
                    }
                }
            }
            else
            {
                opti.SegmentVolume = structure.Margin(0);
            }

            
            //Make an opti structure, but delete it if its exactly the same as the original structure

            //Loop through and subtract all PTV volumes:
            foreach (Structure s in ss.Structures)
            {
                if (s.Id.ToLower().Contains("ptv"))
                {

                    opti.SegmentVolume = opti.Sub(s);


                    

                } 
            }
            //Check if it changed (if it did overlap, need to make a new opti structure)
            if (opti.Volume == 0)
            {
                ss.RemoveStructure(opti);
                return structure;
            }
            //So this is an imperfect function and the volumes will be different no matter what... so make sure they are different by more than 2%
            if ((structure.Volume - opti.Volume) / structure.Volume > 0.02)
            {
                return opti;
            }
            else
            {
                ss.RemoveStructure(opti);
                return structure;
            }

        }

        public static int _GetSlice(double z, StructureSet SS)
        {
            var imageRes = SS.Image.ZRes;

            return Convert.ToInt32((z) - SS.Image.Origin.z / imageRes);
        }
        public static List<string> UpdateConstraints(ref ExternalPlanSetup plan, ref StructureSet ss, ref Plan plan_obj, ScriptContext context, List<List<Structure>> optimizedStructures, List<List<Structure>> matchingStructures, int numCycles)
        {
            List<string> log = new List<string>();
            log.Add("<p>");
            if (numCycles == 1)
            {
                log.Add("Preliminary Optimization Iteration Update Log: <br>");
            }
            else {
                log.Add("Full VMAT Optimization Update: <br>");
            }


            //Go through all plan types and check all constraints.

            PlanSetup p = context.PlanSetup;
            p.DoseValuePresentation = DoseValuePresentation.Absolute;
            double prescriptionDose = p.TotalDose.Dose;
            List<ROI> ROIs = plan_obj.ROIs;
            for (int s = 0; s < plan_obj.ROIs.Count; s++) //Gothrough all the different constrained structures
            {
                for (int match = 0; match < matchingStructures[s].Count; match++)  //FOr each structure in ss that matches the hnplan structure 
                {
                    for (int i = 0; i < ROIs[s].Constraints.Count; i++)                //first go one by one through the constraints.
                    {
                        //Get DVH data for structure: 
                        DVHData dvhData = p.GetDVHCumulativeData(matchingStructures[s][match], DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.01);

                        //Get type of constraint
                        string type = ROIs[s].Constraints[i].Type;
                        string subscript = ROIs[s].Constraints[i].Subscript;
                        string relation = ROIs[s].Constraints[i].EqualityType;
                        double value = ROIs[s].Constraints[i].Value;
                        string format = ROIs[s].Constraints[i].Format;
                        int priority = ROIs[s].Constraints[i].Priority;
                        int priorityLower = ROIs[s].Constraints[i].PriorityRange[0];
                        int priorityUpper = ROIs[s].Constraints[i].PriorityRange[1];


                        if (type.ToLower() == "d")//now subscript can be mean, max, min, median? 
                        {
                            try
                            {
                                double sub = Convert.ToInt32(subscript); //need to analyze DVH data if a number.
                                VolumePresentation vp = VolumePresentation.AbsoluteCm3;
                                DoseValuePresentation dp = new DoseValuePresentation();
                                dp = DoseValuePresentation.Absolute;
                                double volume = matchingStructures[s][match].Volume;
                                if (format.ToLower() == "rel")
                                {
                                    sub = volume * sub / 100;
                                    value *= prescriptionDose / 100;
                                }
                                double doseQuant = p.GetDoseAtVolume(matchingStructures[s][match], sub, vp, dp).Dose;
                                if (p.GetDoseAtVolume(matchingStructures[s][match], sub, vp, dp).Unit == DoseValue.DoseUnit.Gy) //convert to cGy if necessary
                                {
                                    doseQuant *= 100;
                                }
                                //now check the inequality: 
                                if (relation == "<")
                                {
                                    if (doseQuant < value)
                                    {

                                        //If an OAR, loosen priority if its satisfied by over 7Gy
                                        if ((ROIs[s].Type == "OAR") && (value - doseQuant > 1000))
                                        {
                                            priority = (int)(priority * 0.8);                                           
                                            //Make sure the priority is within the priority range:
                                            priority = Math.Min(priorityUpper, priority);
                                            priority = Math.Max(priorityLower, priority);
                                            plan_obj.ROIs[s].Constraints[i].Priority = priority;
                                            log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED. Dose (" + doseQuant.ToString() + "cGy) not within 10Gy of constraint - adjusted priority from " + string.Format("{0}", priority) + " to " + ((int)(priority * 0.8)).ToString() + ".");
                                        }
                                        else
                                        {
                                            log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED. Dose (" + doseQuant.ToString() + "cGy). Unadjusted.");
                                        }
                                    }
                                    else
                                    {
                                        //By what percentage is the constraint being missed by (as ratio)? 
                                        double percentageOff = 1 + (doseQuant - value) / value;

                                        int newPriority = Math.Max((int)(priority * percentageOff), priority + 10);

                                        newPriority = Math.Min(priorityUpper, newPriority);
                                        newPriority = Math.Max(priorityLower, newPriority);
                                        plan_obj.ROIs[s].Constraints[i].Priority = newPriority;
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " FAILED- Updating priority from " + string.Format("{0}", priority) + " to " + string.Format("{0}", (int)(newPriority)));
                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (doseQuant > value)
                                    {
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED Dose (" + doseQuant.ToString() + "cGy). Unadjusted.");

                                    }
                                    else
                                    {

                                        //By what percentage is the constraint being missed by (as ratio)? 
                                        double percentageOff = 1 - (doseQuant - value) / value; //minus because its negative numerator

                                        int newPriority = Math.Max((int)(priority * percentageOff), priority + 10);

                                        newPriority = Math.Min(priorityUpper, newPriority);
                                        newPriority = Math.Max(priorityLower, newPriority);
                                        plan_obj.ROIs[s].Constraints[i].Priority = newPriority;
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " FAILED. Updating priority from " + string.Format("{0}", priority) + " to " + string.Format("{0}", newPriority));

                                    }
                                }
                                else
                                {
                                    log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + "- Could not understand the relation given in the constraint.");
                                }

                            }
                            catch //mean median...
                            {
                                double dose = 0;
                                if (subscript.ToLower() == "mean")
                                {
                                    dose = dvhData.MeanDose.Dose;
                                }
                                else if (subscript.ToLower() == "max")
                                {
                                    dose = dvhData.MaxDose.Dose;
                                }
                                else if (subscript.ToLower() == "median")
                                {
                                    dose = dvhData.MedianDose.Dose;
                                }
                                else if (subscript.ToLower() == "min")
                                {
                                    dose = dvhData.MinDose.Dose;
                                }
                                else
                                {
                                    log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + "- Could not understand the relation given in the constraint. ");
                                    break;
                                }
                                if (dvhData.MeanDose.Unit == DoseValue.DoseUnit.Gy) //convert to gy if necessary
                                {
                                    dose *= 100;
                                }
                                if (format.ToLower() == "rel")
                                {
                                    value *= prescriptionDose / 100;
                                }
                                //Now check the relation
                                if (relation == "<")
                                {
                                    if (dose < value)
                                    {

                                        if ((ROIs[s].Type == "OAR") && (value - dose > 1000))
                                        {
                                            log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED by over 10Gy. Decreasing priority to " + string.Format("{0}", (int)(priority * 0.8)));
                                            priority = (int)(priority * 0.8);
                                            priority = Math.Min(priorityUpper, priority);
                                            priority = Math.Max(priorityLower, priority);
                                            plan_obj.ROIs[s].Constraints[i].Priority = priority;
                                        }
                                        else
                                        {
                                            log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED. Unadjusted");
                                        }
                                    }
                                    else
                                    {
                                        //By what percentage is the constraint being missed by (as ratio)? 
                                        double percentageOff = 1 + (dose - value) / value;
                                        int newPriority = Math.Max((int)(priority * percentageOff), priority + 10);
                                        //Also don't want to increase priority by more than 20 at a time: 
                                        newPriority = Math.Min(newPriority, priority + 20);
                                        if ((ROIs[s].Type == "OAR") && (ROIs[s].Critical == false))
                                        {
                                            newPriority = Math.Min(newPriority, 70);

                                        }
                                        if (dose - value > 2000) //if constraint is failing miserably, stop trying
                                        {
                                            newPriority = 0;
                                            log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed by more than 20Gy. Deleting constraint.");
                                            plan_obj.ROIs[s].Constraints[i].Priority = newPriority;
                                        }
                                        else
                                        {
                                            log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from " + string.Format("{0}", priority) + " to " + string.Format("{0}", (int)(priority * 1.1)));
                                            newPriority = Math.Min(priorityUpper, newPriority);
                                            newPriority = Math.Max(priorityLower, newPriority);
                                            plan_obj.ROIs[s].Constraints[i].Priority = newPriority;
                                        }

                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (dose > value)
                                    {
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED. Unadjusted.");

                                    }
                                    else
                                    {
                                        //By what percentage is the constraint being missed by (as ratio)? 
                                        double percentageOff = 1 - (dose - value) / value; //negative because negative numerator

                                        int newPriority = Math.Max((int)(priority * percentageOff), priority + 10);
                                        newPriority = Math.Min(priorityUpper, newPriority);
                                        newPriority = Math.Max(priorityLower, newPriority);
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from " + string.Format("{0}", priority) + " to " + string.Format("{0}", newPriority));
                                        plan_obj.ROIs[s].Constraints[i].Priority = newPriority;
                                    }
                                }
                                else
                                {
                                    log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + "-Could not understand the relation given in the constraint.");
                                }
                            }
                        }
                        else if (type == "V")
                        {

                            try
                            {
                                double sub = Convert.ToDouble(subscript); //need to analyze DVH data if a number.
                                double frac = prescriptionDose; //ad hoc fix for ptv string output
                                VolumePresentation vp = VolumePresentation.Relative;
                                double volume = matchingStructures[s][match].Volume;

                                if (format.ToLower() == "abs")
                                {
                                    value = (value / volume) * 100;
                                    sub = Convert.ToDouble(subscript);
                                }
                                //But if PTV, then this is not relative to the prescription dose.
                                if (ROIs[s].Name.ToLower().Contains("ptv"))
                                {

                                    frac = StringOperations.FindPTVNumber(ROIs[s].Name.ToLower());
                                    sub = Convert.ToDouble(subscript) * frac / 100; //frac in Gy and subscript percentage, so factor of 100 cancels out
                                    vp = VolumePresentation.Relative;
                                }

                                DoseValue dose = new DoseValue(sub, "cGy");
                                //Need dose in cGy for calculation:
                                double volumeQuant = p.GetVolumeAtDose(matchingStructures[s][match], dose, vp);
                                //Now check the inequality: 
                                if (relation == "<")
                                {
                                    if (volumeQuant < value)
                                    {
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED.");

                                    }
                                    else
                                    {
                                        //By what percentage is the constraint being missed by (as ratio)? 
                                        double percentageOff = 1 + (volumeQuant - value) / value;

                                        int newPriority = Math.Max((int)(priority * percentageOff), priority + 10);
                                        //Also don't want to adjust any constraint by more than 20 at a time:
                                        newPriority = Math.Min(newPriority, priority + 20);
                                        newPriority = Math.Min(priorityUpper, newPriority);
                                        newPriority = Math.Max(priorityLower, newPriority);
                                        plan_obj.ROIs[s].Constraints[i].Priority = newPriority;
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from" + string.Format("{0}", priority) + " to " + string.Format("{0}", newPriority));

                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (volumeQuant > value)
                                    {
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED.");
                                    }
                                    else
                                    {
                                        double percentageOff = 1 - (volumeQuant - value) / value;

                                        int newPriority = Math.Max((int)(priority * percentageOff), priority + 10);
                                        newPriority = Math.Min(priorityUpper, newPriority);
                                        newPriority = Math.Max(priorityLower, newPriority);
                                        plan_obj.ROIs[s].Constraints[i].Priority = newPriority;
                                        log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from" + string.Format("{0}", priority) + " to " + string.Format("{0}", newPriority));
                                    }
                                }
                                else
                                {
                                    log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + "-Could not understand the relation given in the constraint.");
                                }
                            }
                            catch
                            {
                                log.Add(ROIs[s].Name + ": Constraint " + (i + 1).ToString() + "-Could not understand the relation given in the constraint.");
                            }
                        }
                    }
                }
                log.Add("</p>");
            }
            //Now need to delete and reset all constraints
            SetConstraints(ref plan, plan_obj, optimizedStructures);
            return log;

        }
    }
}
