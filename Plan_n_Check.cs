using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using Plan_n_Check;
using Plan_n_Check.Plans;
// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: ESAPIScript(IsWriteable = true)]
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]


// TODO: Uncomment the following line if the script requires write access.
// [assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
  public class Script
  {
    public Script()
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Execute(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
    {
            //Assembly assembly = Assembly.LoadFrom(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319");
            //Type type = assembly.GetType(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319.DialogBox");


            //Is patient loaded? 
            if (context.Patient == null)
            {
                throw new System.ArgumentException("Please load a patient and try again.");
            }
            //Is there a structure set?
            if (context.StructureSet == null)
            {
                throw new System.ArgumentException("Failed to find a structure set for patient " + context.Patient.Name);
            }
            //Is there a course?
            if (context.Course == null)
            {
                throw new System.ArgumentException("Please check that a course has been selected for " + context.Patient.Name);
            }
            StructureSet ss = context.StructureSet;
            PlanSetup ps = context.PlanSetup;
            Patient p = context.Patient;
            MainForm mainForm = new MainForm(ref context);
            System.Windows.Forms.Application.Run(mainForm);
            //System.Windows.System.Windows.MessageBox.Show("Hello", "input", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information, System.Windows.MessageBoxResult.OK);
    }
        public static List<List<Structure>> StartOptimizer(ScriptContext context, HNPlan hnPlan, List<List<Structure>> matchingStructures) //Returns list of matching structures
        {
            if (context.Patient == null)
            {
               System.Windows.MessageBox.Show("Please load a patient and try again");
                return new List<List<Structure>>();
            }
            // Check for patient plan loaded
            ExternalPlanSetup plan = context.ExternalPlanSetup;
            if (plan == null)
            {
                System.Windows.MessageBox.Show("Please load a plan and try again.");
                return new List<List<Structure>>();
            }
            if (context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Could not find a structure set. Terminating.");
            }

            Patient patient = context.Patient;
            StructureSet ss = context.StructureSet;
            Course course = context.Course;
            Image image3d = context.Image;

            patient.BeginModifications();


            ExternalBeamMachineParameters ebmp = new ExternalBeamMachineParameters("VaUnit6TB", "6X", 600, "ARC", null);
            //Create two VMAT beams
            BeamMaker(ref plan, ss, plan.TotalDose.Dose);

            plan.SetPrescription(35, new DoseValue(200, "cGy"), 1);
            double presDose = plan.TotalDose.Dose;

            
            //matchingStructures is the same length as hnPlan.ROIs.count
            //Now set optimization constraints
            SetConstraints(ref plan, hnPlan, matchingStructures);
            ParotidChop(ref plan, hnPlan, matchingStructures, ss, context);

            return matchingStructures;

        }
        public static void ParotidChop(ref ExternalPlanSetup plan, HNPlan hnPlan, List<List<Structure>> matchingStructures, StructureSet ss, ScriptContext context)
        {
            /* 
             1. Find contralateral parotid (one with least overlap, largest sum of distance from PTVs), get contours
             2.) split this up into its separate contours
             3. make new structure for each
             4. make constraint for each based on importance
             */

            //1.
            Structure contraPar = FindContraPar(plan, ss, hnPlan, matchingStructures, context);
            //Now get contours for it
            // GetContours function will return the list of contours, as well as a list of all z-planes which contours were taken from, in a tuple
            var tuple = GetContours(contraPar, context);
            List<double[,]> contours = tuple.Item1;
            List<double[]> planes = tuple.Item2;


            //2. Now the parotid segmentation!
            int numCutsZ = 2;
            int numCutsX = 2;
            int numCutsY = 1;
            List<List<double[,]>> choppedContours = Chop(contours, numCutsX, numCutsY, numCutsZ, contraPar.Name);

            //3. Make the structures, 4. make constraints for each:    (All done in Optimize function)     
            //Now run the VMAT optimization.
            Optimize(choppedContours, planes, ref plan, ref ss, hnPlan, context, matchingStructures, contraPar.Name);



        }
        public static void Optimize(List<List<double[,]>> choppedContours, List<double[]>
            planes, ref ExternalPlanSetup plan, ref StructureSet ss, HNPlan hnPlan, ScriptContext context, List<List<Structure>> matchingStructures, string contraName, int numIterations = 2)
        {
            MakeParotidStructures(choppedContours, planes, ref plan, ref ss, hnPlan, context, matchingStructures, contraName, 1.1);
            //Now run the first VMAT optimization. 
            plan.SetCalculationModel(CalculationType.PhotonVMATOptimization, "PO_13623");
            plan.SetCalculationModel(CalculationType.DVHEstimation, "DVH Estimation Algorithm [15.6.06]");
            plan.SetCalculationModel(CalculationType.PhotonVolumeDose, "AAA_13623");
            plan.OptimizationSetup.AddNormalTissueObjective(100, 3, 95, 50, 0.2);
            plan.OptimizeVMAT();
            plan.CalculateDose();


            for (int iter = 0; iter < numIterations; iter++)
            {
                string mlcID = plan.Beams.FirstOrDefault<Beam>().MLC.Id;
                OptimizationOptionsVMAT oov = new OptimizationOptionsVMAT(1, mlcID);
                plan.OptimizeVMAT(oov);
                plan.CalculateDose();

                var dose = plan.Dose;
                //Now need to perform a plan check and iteratively adjust constraints based on whether they passed or failed, and whether they passed with flying colours or failed miserably.
                //Going to find the percentage by which the constraint failed or passed, and adjust both the priority and dose constraint based on this. 
                UpdateConstraints(choppedContours, planes, ref plan, ref ss, hnPlan, context, matchingStructures, contraName);
                MakeParotidStructures(choppedContours, planes, ref plan, ref ss, hnPlan, context, matchingStructures, contraName, 1.1);



            }



        }
        public static string CheckConstraints(List<List<double[,]>> choppedContours, List<double[]>
            planes, ref ExternalPlanSetup plan, ref StructureSet ss, HNPlan hnPlan, ScriptContext context, List<List<Structure>> matchingStructures, string contraName)
        {

            //Go through all plan types and check all constraints.
            PlanSetup p = context.PlanSetup;
            string returnString = "";
            p.DoseValuePresentation = DoseValuePresentation.Absolute;
            double prescriptionDose = p.TotalDose.Dose;
            List<ROI> ROIs = hnPlan.ROIs;
            for (int s = 0; s < hnPlan.ROIs.Count; s++) //Gothrough all the different constrained structures
            {
                for (int match = 0; match < matchingStructures[s].Count; match++)  //FOr each structure in ss that matches the hnplan structure 
                {
                    for (int i = 0; i < ROIs[s].Constraints.Count; i++)                //first go one by one through the constraints.
                    {
                        //Get DVH data for structure: 
                        DVHData dvhData = p.GetDVHCumulativeData(matchingStructures[s][match], DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.001);

                        //Get type of constraint
                        string type = ROIs[s].Constraints[i].Type;
                        string subscript = ROIs[s].Constraints[i].Subscript;
                        string relation = ROIs[s].Constraints[i].EqualityType;
                        double value = ROIs[s].Constraints[i].Value;
                        string format = ROIs[s].Constraints[i].Format;


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
                                    value = value * prescriptionDose / 100;

                                    double doseQuant = p.GetDoseAtVolume(matchingStructures[s][match], sub, vp, dp).Dose;
                                    if (p.GetDoseAtVolume(matchingStructures[s][match], sub, vp, dp).Unit == DoseValue.DoseUnit.cGy) //convert to gy if necessary
                                    {
                                        doseQuant /= 100;
                                    }
                                    //now check the inequality: 
                                    if (relation == "<")
                                    {
                                        if (doseQuant < value)
                                        {
                                            returnString += "Constraint SATISFIED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + "\n";
                                        }
                                        else
                                        {
                                            returnString += "Constraint FAILED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + "\n";
                                        }
                                    }
                                    else if (relation == ">")
                                    {
                                        if (doseQuant > value)
                                        {
                                            returnString += "Constraint SATISFIED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + "\n";

                                        }
                                        else
                                        {
                                            returnString += "Constraint FAILED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + "\n";
                                        }
                                    }
                                    else
                                    {
                                        returnString += "Could not understand the relation given in the constraint. \n";
                                    }
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
                                    returnString += "Failed to interpret subscript given for this constraint. \n";
                                    break;
                                }
                                if (dvhData.MeanDose.Unit == DoseValue.DoseUnit.cGy) //convert to gy if necessary
                                {
                                    dose /= 100;
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
                                        returnString += "Constraint SATISFIED. D = " + string.Format("{0:0.0}", dose) + "Gy \n";
                                    }
                                    else
                                    {
                                        returnString += "Constraint FAILED. D = " + string.Format("{0:0.0}", dose) + "Gy \n";
                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (dose > value)
                                    {
                                        returnString += "Constraint SATISFIED. D = " + string.Format("{0:0.0}", dose) + "Gy \n";

                                    }
                                    else
                                    {
                                        returnString += "Constraint FAILED. D = " + string.Format("{0:0.0}", dose) + "Gy \n";
                                    }
                                }
                                else
                                {
                                    returnString += "Could not understand the relation given in the constraint. \n";
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
                                bool isPTV = false;

                                if (format.ToLower() == "abs")
                                {
                                    value = (value / volume) * 100;
                                    sub = Convert.ToDouble(subscript);
                                }
                                //But if PTV, then this is not relative to the prescription dose.
                                if (ROIs[s].Name.ToLower().Contains("ptv"))
                                {
                                    isPTV = true;
                                    frac = FindPTVNumber(ROIs[s].Name.ToLower());
                                    sub = Convert.ToDouble(subscript) * frac / 100;
                                    vp = VolumePresentation.Relative;
                                }

                                DoseValue dose = new DoseValue(sub * 100, "cGy");
                                //Need dose in cGy for calculation:
                                double volumeQuant = p.GetVolumeAtDose(matchingStructures[s][match], dose, vp);
                                //Now check the inequality: 
                                if (relation == "<")
                                {
                                    if (volumeQuant < value)
                                    {
                                        if (isPTV)
                                        {
                                            returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "Gy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. \n";
                                        }
                                        else
                                        {
                                            returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "Gy was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. \n";
                                        }
                                    }
                                    else
                                    {
                                        if (isPTV)
                                        {
                                            returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "Gy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. \n";
                                        }
                                        else
                                        {
                                            returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "Gy was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. \n";
                                        }
                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (volumeQuant > value)
                                    {
                                        if (isPTV)
                                        {
                                            returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "Gy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. \n";
                                        }
                                        else
                                        {
                                            returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "Gy was  " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. \n";
                                        }
                                    }
                                    else
                                    {
                                        if (isPTV)
                                        {
                                            returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "Gy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. \n";
                                        }
                                        else
                                        {
                                            returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "Gy was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. \n";
                                        }
                                    }
                                }
                                else
                                {
                                    returnString += "Could not understand the relation given in the constraint. \n";
                                }
                            }
                            catch
                            {
                                returnString += "Failed to interpret subscript given for this constraint. \n";
                            }
                        }
                    }
                }
            }

            return returnString;


        }
        public static void UpdateConstraints(List<List<double[,]>> choppedContours, List<double[]>
            planes, ref ExternalPlanSetup plan, ref StructureSet ss, HNPlan hnPlan, ScriptContext context, List<List<Structure>> matchingStructures, string contraName)
        {

            //Go through all plan types and check all constraints.
            PlanSetup p = context.PlanSetup;
            string returnString = "";
            p.DoseValuePresentation = DoseValuePresentation.Absolute;
            double prescriptionDose = p.TotalDose.Dose;
            List<ROI> ROIs = hnPlan.ROIs;
            for (int s = 0; s < hnPlan.ROIs.Count; s++) //Gothrough all the different constrained structures
            {
                for (int match = 0; match < matchingStructures[s].Count; match++)  //FOr each structure in ss that matches the hnplan structure 
                {
                    for (int i = 0; i < ROIs[s].Constraints.Count; i++)                //first go one by one through the constraints.
                    {
                        //Get DVH data for structure: 
                        DVHData dvhData = p.GetDVHCumulativeData(matchingStructures[s][match], DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.001);

                        //Get type of constraint
                        string type = ROIs[s].Constraints[i].Type;
                        string subscript = ROIs[s].Constraints[i].Subscript;
                        string relation = ROIs[s].Constraints[i].EqualityType;
                        double value = ROIs[s].Constraints[i].Value;
                        string format = ROIs[s].Constraints[i].Format;
                        int priority = ROIs[s].Constraints[i].Priority;


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
                                    value = value * prescriptionDose;

                                    double doseQuant = p.GetDoseAtVolume(matchingStructures[s][match], sub, vp, dp).Dose;
                                    //now check the inequality: 
                                    if (relation == "<")
                                    {
                                        if (doseQuant < value)
                                        {
                                            returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED.";
                                        }
                                        else
                                        {
                                            returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from" + string.Format("{0}", priority) + " to " + string.Format("{0}", (int)(priority * 1.1));
                                            hnPlan.ROIs[s].Constraints[i].Priority = (int)(priority * 1.1);
                                        }
                                    }
                                    else if (relation == ">")
                                    {
                                        if (doseQuant > value)
                                        {
                                            returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED.";

                                        }
                                        else
                                        {
                                            returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from" + string.Format("{0}", priority) + " to " + string.Format("{0}", (int)(priority * 1.1));
                                            hnPlan.ROIs[s].Constraints[i].Priority = (int)(priority * 1.1);
                                        }
                                    }
                                    else
                                    {
                                        returnString += "Could not understand the relation given in the constraint. \n";
                                    }
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
                                    returnString += "Failed to interpret subscript given for this constraint. \n";
                                    break;
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
                                        returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED.";
                                    }
                                    else
                                    {
                                        returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from" + string.Format("{0}", priority) + " to " + string.Format("{0}", (int)(priority * 1.1));
                                        hnPlan.ROIs[s].Constraints[i].Priority = (int)(priority * 1.1);
                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (dose > value)
                                    {
                                        returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED.";

                                    }
                                    else
                                    {
                                        returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from" + string.Format("{0}", priority) + " to " + string.Format("{0}", (int)(priority * 1.1));
                                        hnPlan.ROIs[s].Constraints[i].Priority = (int)(priority * 1.1);
                                    }
                                }
                                else
                                {
                                    returnString += "Could not understand the relation given in the constraint. \n";
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

                                    frac = FindPTVNumber(ROIs[s].Name.ToLower());
                                    sub = Convert.ToDouble(subscript) * frac; //frac in Gy and subscript percentage, so factor of 100 cancels out
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
                                        returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED.";

                                    }
                                    else
                                    {

                                        returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from" + string.Format("{0}", priority) + " to " + string.Format("{0}", (int)(priority * 1.1));
                                        hnPlan.ROIs[s].Constraints[i].Priority = (int)(priority * 1.1);

                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (volumeQuant > value)
                                    {
                                        returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " SATISFIED.";
                                    }
                                    else
                                    {
                                        returnString += ROIs[s].Name + ": Constraint " + (i + 1).ToString() + " Failed. Updating priority from" + string.Format("{0}", priority) + " to " + string.Format("{0}", (int)(priority * 1.1));
                                        hnPlan.ROIs[s].Constraints[i].Priority = (int)(priority * 1.1);
                                    }
                                }
                                else
                                {
                                    returnString += "Could not understand the relation given in the constraint. \n";
                                }
                            }
                            catch
                            {
                                returnString += "Failed to interpret subscript given for this constraint. \n";
                            }
                        }
                    }
                }
            }

            System.Windows.MessageBox.Show(returnString);

        }
        public static void MakeParotidStructures(List<List<double[,]>> choppedContours, List<double[]>
            planes, ref ExternalPlanSetup plan, ref StructureSet ss, HNPlan hnPlan, ScriptContext context, List<List<Structure>> matchingStructures, string contraName, double priorityRatio)
        {
            Image image = context.Image;
            //First off, in case this script is re-run, we need to remove any subsegment structures already made. 
            foreach (Structure structure in ss.Structures.ToList())
            {
                if (structure.Name.ToLower().Contains("subsegment"))
                {
                    ss.RemoveStructure(structure);
                }
            }


            int numSections = choppedContours.Count;
            double[] importanceValues = new double[18] { 0.751310670731707,  0.526618902439024,   0.386310975609756,
                1,   0.937500000000000,   0.169969512195122,   0.538871951219512 ,  0.318064024390244,   0.167751524390244,
                0.348320884146341,   0.00611608231707317, 0.0636128048780488,  0.764222560975610,   0.0481192835365854,  0.166463414634146,
                0.272984146341463,   0.0484897103658537,  0.0354939024390244, };
            //Now need to chop up the parotid, and make new constraints
            List<Structure> subsegments = new List<Structure>();
            string name;

            for (int subsegment = 0; subsegment < numSections; subsegment++) //loop over all subsegments
            {
                name = "Subsegment " + (subsegment + 1).ToString();
                if (ss.CanAddStructure("CONTROL", name))
                {
                    subsegments.Add(ss.AddStructure("CONTROL", name));
                    for (int zPlane = 0; zPlane < choppedContours[subsegment].Count; zPlane++) //loop over all contours for current subsegment, and add contour to structure
                    {
                        int planeIndex = 0;
                        for (int z = 0; z < planes.Count; z++)//Find the index of the plane corresponding to the current z value of the contour (saved in the planes list)
                        {
                            if (choppedContours[subsegment][zPlane][0, 2] == planes[z][1])
                            {
                                planeIndex = (int)planes[z][0];
                            }
                        }
                        VVector[] currentContour = ArrayToVVector(choppedContours[subsegment][zPlane]);
                        //Before adding this contour, need to convert the coordinates from dicom back to the user coordinate system. 
                        for (int i = 0; i < currentContour.Length; i++)
                        {
                            currentContour[i] = image.DicomToUser(currentContour[i], plan);
                        }
                        subsegments[subsegment].AddContourOnImagePlane(currentContour, planeIndex);
                        //Now also need to set an optimization constraint based on the importance, and the constraint set on the whole contralateral parotid.    

                    }
                    //Find out constraint on contrapar
                    int ROI_Index = 0;
                    double doseConstraint = 0;
                    double priority = 0;
                    for (int i = 0; i < matchingStructures.Count; i++)
                    {
                        for (int j = 0; j < matchingStructures[i].Count; j++)
                        {
                            if (contraName == matchingStructures[i][j].Name)
                            {
                                ROI_Index = i;
                            }

                        }

                    }
                    bool overlap = DoesOverlapWithPTV(choppedContours[subsegment], ss);
                    if (!overlap) //only set a constraint for the subsegment if it does not overlap with a ptv. 
                    {
                        try
                        {
                            doseConstraint = 1500;//hnPlan.ROIs[ROI_Index].Constraints[0].Value;
                            priority = hnPlan.ROIs[ROI_Index].Constraints[0].Priority;
                            priority *= priorityRatio * importanceValues[subsegment];
                            plan.OptimizationSetup.AddMeanDoseObjective(subsegments[subsegment], new DoseValue(doseConstraint, "cGy"), priority);
                        }
                        catch { }
                    }
                    else
                    {
                        overlap = false; //reset the value
                    }



                }
                else
                {
                    System.Windows.MessageBox.Show("Could not create new subsegment structures.");
                }
            }

        }
        public static bool DoesOverlapWithPTV(List<double[,]> contours, StructureSet ss)
        {
            //right now consider 3 or more points for a given subsegment inside a ptv means it is overlapping.
            double x, y, z;
            VVector point;
            int overlapNum = 0;

            //get list of ptvs
            List<Structure> ptvs = new List<Structure>();
            foreach (Structure structure in ss.Structures)
            {
                if (structure.Name.ToLower().Contains("ptv"))
                {
                    ptvs.Add(structure);
                }
            }
            for (int j = 0; j < contours.Count; j++) //loop over axial plane contours
            {
                for (int k = 0; k < contours[j].GetLength(0); k++)
                {
                    x = contours[j][k, 0];
                    y = contours[j][k, 1];
                    z = contours[j][k, 2];
                    point = new VVector(x, y, z);

                    for (int ptv = 0; ptv < ptvs.Count; ptv++)
                    {
                        if (ptvs[ptv].IsPointInsideSegment(point))
                        {
                            overlapNum++;
                        }
                    }

                }
            }
            if (overlapNum >= 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static VVector[] ArrayToVVector(double[,] contourArray)
        {
            VVector[] contour = new VVector[contourArray.GetLength(0)];
            for (int i = 0; i < contourArray.GetLength(0); i++)
            {
                contour[i] = new VVector(contourArray[i, 0], contourArray[i, 1], contourArray[i, 2]);
            }

            return contour;
        }

        public static Structure FindContraPar(ExternalPlanSetup plan, StructureSet ss, HNPlan hnPlan, List<List<Structure>> matchingStructures, ScriptContext context)
        {
            //find all the ptvs

            List<Structure> ptvs = new List<Structure>();
            foreach (Structure structure in ss.Structures)
            {
                if (structure.Name.ToLower().Contains("ptv"))
                {
                    ptvs.Add(structure);
                }
            }
            //Now find the parotids, by first getting index of parotids in the head and neck plan list, and taking the assigned structure for each,
            List<Structure> parotids = new List<Structure>();
            List<int> overlappingPars = new List<int>();
            for (int s = 0; s < hnPlan.ROIs.Count; s++)
            {
                if (hnPlan.ROIs[s].Name.ToLower().Contains("parotid"))
                {
                    parotids.Add(matchingStructures[s][0]);
                }
            }
            List<List<double[,]>> contours = new List<List<double[,]>>();
            //now need to find which structure in parotids is closest to PTVs. 
            //first check if one overlaps and the other doesn't. In this case, make non-overlapping one the contralateral.
            for (int i = 0; i < parotids.Count; i++)
            {
                contours.Add(StructureContours(parotids[i], plan, context, ss));
            }
            bool overlap;
            double x, y, z;
            VVector point;
            for (int i = 0; i < parotids.Count; i++)
            {
                overlap = false;
                for (int j = 0; j < contours[i].Count; j++) //loop over axial plane contours
                {
                    if (overlap)
                    {
                        overlappingPars.Add(i);
                        break;
                    }
                    for (int k = 0; k < contours[i][j].GetLength(0); k++)
                    {
                        x = contours[i][j][k, 0];
                        y = contours[i][j][k, 1];
                        z = contours[i][j][k, 2];
                        point = new VVector(x, y, z);

                        for (int ptv = 0; ptv < ptvs.Count; ptv++)
                        {
                            if (ptvs[ptv].IsPointInsideSegment(point))
                            {
                                overlap = true;
                                break;
                            }
                        }
                        if (overlap)
                        {
                            break;
                        }
                    }
                }
            }
            //Now find out if only one overlapped: 
            int numOverlapping = overlappingPars.Count;

            if (numOverlapping == 1)
            {
                return parotids[overlappingPars[0]];
            }

            //Or else need to find the distance between parotid and ptvs. 
            double dist;
            double smallestDist = 0;
            Structure contraPar = parotids[1]; //placeHolder
            VVector parCentre, ptvCentre;
            for (int i = 0; i < parotids.Count; i++)
            {
                dist = 0;
                parCentre = parotids[i].CenterPoint;

                for (int ptv = 0; ptv < ptvs.Count; ptv++)
                {
                    ptvCentre = ptvs[ptv].CenterPoint;
                    dist += Math.Pow(parCentre.x - ptvCentre.x, 2) + Math.Pow(parCentre.y - ptvCentre.y, 2) + Math.Pow(parCentre.x - ptvCentre.x, 2);
                }
                if (dist > smallestDist)
                {

                    contraPar = parotids[i];

                }

            }
            //Now return the one with the largest distance

            return contraPar;
        }

        public static void SetConstraints(ref ExternalPlanSetup plan, HNPlan presetPlan, List<List<Structure>> matchingStructures)
        {
            //Need to set all optimization constraints now. First clear all the current constraints
            foreach (var objective in plan.OptimizationSetup.Objectives.OfType<OptimizationPointObjective>())
            {

                plan.OptimizationSetup.RemoveObjective(objective);
            }
            foreach (var objective in plan.OptimizationSetup.Objectives.OfType<OptimizationMeanDoseObjective>())
            {
                plan.OptimizationSetup.RemoveObjective(objective);

            }

            //Now loop over all constraints and set them
            for (int i = 0; i < presetPlan.ROIs.Count; i++)    //Loop over all structures
            {
                for (int match = 0; match < matchingStructures[i].Count; match++)
                {
                    for (int j = 0; j < presetPlan.ROIs[i].Constraints.Count; j++)    //Loop over all constraints for the current structure
                    {
                        plan.OptimizationSetup.AddNormalTissueObjective(80.0f, 0.0f, 100.0f, 40.0f, 0.05f);

                        string type = presetPlan.ROIs[i].Constraints[j].Type;
                        string subscript = presetPlan.ROIs[i].Constraints[j].Subscript;
                        string relation = presetPlan.ROIs[i].Constraints[j].EqualityType;
                        double value = presetPlan.ROIs[i].Constraints[j].Value;
                        string format = presetPlan.ROIs[i].Constraints[j].Format;
                        double volume = matchingStructures[i][match].Volume;

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
                                plan.OptimizationSetup.AddPointObjective(matchingStructures[i][match],
                                    constraintType, new DoseValue(value, "cGy"), volume, presetPlan.ROIs[i].Constraints[j].Priority);
                            }
                            catch //is a mean or max restriction
                            {
                                if (subscript.ToLower() == "mean")
                                {
                                    double dose = value * 0.95; //take 90 percent of mean constraint dose to start
                                    plan.OptimizationSetup.AddMeanDoseObjective(matchingStructures[i][match],
                                        new DoseValue(dose, "cGy"), presetPlan.ROIs[i].Constraints[j].Priority);

                                }
                                else if (subscript.ToLower() == "max")
                                {
                                    double dose = value * 0.95; //take 90 percent of constraint dose to start
                                    plan.OptimizationSetup.AddPointObjective(matchingStructures[i][match], constraintType,
                                        new DoseValue(dose, "cGy"), 0, presetPlan.ROIs[i].Constraints[j].Priority);

                                }
                                else if (subscript.ToLower() == "min")
                                {
                                    double dose = value * 1.05; //take 110% percent of constraint dose to start
                                    plan.OptimizationSetup.AddPointObjective(matchingStructures[i][match], constraintType,
                                        new DoseValue(dose, "cGy"), 100, presetPlan.ROIs[i].Constraints[j].Priority);
                                }
                            }
                        }
                        else if (type.ToLower() == "v")
                        {
                            try //in case can't parse the subscript into Int32
                            {
                                if (format.ToLower() == "abs")
                                {
                                    double dose = Convert.ToInt32(subscript) * 100; //convert to cGy
                                    //Need to convert to relative volume. 
                                    volume = value / volume * 100;
                                    plan.OptimizationSetup.AddPointObjective(matchingStructures[i][match], constraintType,
                                        new DoseValue(dose, "cGy"), volume, presetPlan.ROIs[i].Constraints[j].Priority);

                                }
                                else  //if relative, will belong to a PTV
                                {
                                    double dose = FindPTVNumber(matchingStructures[i][match].Name.ToLower()) * 100;
                                    volume = value;
                                    plan.OptimizationSetup.AddPointObjective(matchingStructures[i][match], constraintType,
                                       new DoseValue(dose, "cGy"), volume, presetPlan.ROIs[i].Constraints[j].Priority);
                                }
                            }
                            catch
                            {

                            }
                        }



                    }
                }


            }
        }
        public static List<double[,]> StructureContours(Structure structure, ExternalPlanSetup plan, ScriptContext context, StructureSet ss)
        {
            var image = context.Image;

            List<VVector[]> contoursTemp = new List<VVector[]>();
            //ROI is now a list with one structure; the one of interest.
            int zSlices = ss.Image.ZSize;

            for (int z = 0; z < zSlices; z++)
            {
                VVector[][] contoursOnPlane = structure.GetContoursOnImagePlane(z);
                //If length > 1, there could be an island.
                if (contoursOnPlane.GetLength(0) > 0)
                {
                    // will check for the one with the most points, and keep that one.
                    /*int keeper = 0;
                    int numPoints = 0;
                    for (int cont = 0; cont < contoursOnPlane.GetLength(0); cont++)
                    {
                        if (contoursOnPlane[cont].GetLength(0) > numPoints)
                        {
                            keeper = cont;
                        }
                    }*/
                    contoursTemp.Add(contoursOnPlane[0]);
                }
            }
            //System.Windows.MessageBox.Show(contoursTemp[0][0].z.ToString());
            //Now convert this into a double[,] array list
            List<double[,]> contours = new List<double[,]>();
            for (int i = 0; i < contoursTemp.Count; i++)
            {
                contours.Add(new double[contoursTemp[i].GetLength(0), 3]);
                for (int j = 0; j < contoursTemp[i].GetLength(0); j++)
                {
                    VVector point = image.UserToDicom(contoursTemp[i][j], plan);
                    contours[i][j, 0] = (point.x);
                    contours[i][j, 1] = (point.y);
                    contours[i][j, 2] = (point.z);
                }
            }
            contours = ClosedLooper(contours);
            contours = IslandRemover(contours);
            return contours;
        }
        public static Tuple<List<double[,]>, List<double[]>> GetContours(Structure parotid, ScriptContext context)
        {
            //First element of List<double[]>> array is the plane index, and the second is its correspoinding cartesian z coordinate
            ExternalPlanSetup plan = context.ExternalPlanSetup;
            Image image = context.Image;
            StructureSet ss = context.StructureSet;

            List<VVector[]> contoursTemp = new List<VVector[]>();
            List<double[]> planes = new List<double[]>();
            //ROI is now a list with one structure; the one of interest.
            int zSlices = ss.Image.ZSize;

            for (int z = 0; z < zSlices; z++)
            {
                VVector[][] contoursOnPlane = parotid.GetContoursOnImagePlane(z);
                //If length > 1, there could be an island.
                if (contoursOnPlane.GetLength(0) > 0)
                {
                    // will check for the one with the most points, and keep that one. (islands usually smaller)
                    /*int keeper = 0;
                    int numPoints = 0;
                    for (int cont = 0; cont < contoursOnPlane.GetLength(0); cont++)
                    {
                        if (contoursOnPlane[cont].GetLength(0) > numPoints)
                        {
                            keeper = cont;
                        }
                    }*/
                    VVector point = image.UserToDicom(contoursOnPlane[0][0], plan);
                    contoursTemp.Add(contoursOnPlane[0]);
                    planes.Add(new double[2] { z, point.z });
                }
            }
            //System.Windows.MessageBox.Show(contoursTemp[0][0].z.ToString());
            //Now convert this into a double[,] array list
            List<double[,]> contours = new List<double[,]>();
            for (int i = 0; i < contoursTemp.Count; i++)
            {
                contours.Add(new double[contoursTemp[i].GetLength(0), 3]);
                for (int j = 0; j < contoursTemp[i].GetLength(0); j++)
                {
                    VVector point = image.UserToDicom(contoursTemp[i][j], plan);
                    contours[i][j, 0] = (point.x);
                    contours[i][j, 1] = (point.y);
                    contours[i][j, 2] = (point.z);
                }
            }
            contours = ClosedLooper(contours);
            contours = IslandRemover(contours);
            return Tuple.Create(contours, planes);
        }

        public static List<List<double[,]>> GetContoursPTV(StructureSet structureSet, PlanSetup plan1, Image image)
        {

            List<Structure> ROI = new List<Structure>();    //Saving in a list because I only have read access.
            string organName = "";
            List<VVector[]> contoursTemp = new List<VVector[]>();
            foreach (Structure structure in structureSet.Structures)
            {
                organName = structure.Name;
                if (organName.ToLower().Contains("ptv"))
                {
                    ROI.Add(structure);

                }
            }
            List<List<double[,]>> contours = new List<List<double[,]>>();
            for (int l = 0; l < ROI.Count; l++)
            {
                contours.Add(new List<double[,]>());
            }
            for (int contourCount = 0; contourCount < ROI.Count; contourCount++)
            {
                contoursTemp.Clear();
                //ROI is now a list with one structure; the one of interest.
                int zSlices = structureSet.Image.ZSize;

                for (int z = 0; z < zSlices; z++)
                {
                    VVector[][] contoursOnPlane = ROI[contourCount].GetContoursOnImagePlane(z);
                    //If length > 1, there could be an island.
                    if (contoursOnPlane.GetLength(0) > 0)
                    {
                        contoursTemp.Add(contoursOnPlane[0]);
                    }
                }

                //Now convert this into a double[,] array list
                if (contoursTemp.Count != 0)
                {
                    for (int i = 0; i < contoursTemp.Count; i++)
                    {
                        contours[contourCount].Add(new double[contoursTemp[i].GetLength(0), 3]);

                        for (int j = 0; j < contoursTemp[i].GetLength(0); j++)
                        {
                            VVector point = image.UserToDicom(contoursTemp[i][j], plan1);
                            contours[contourCount][i][j, 0] = (point.x);
                            contours[contourCount][i][j, 1] = (point.y);
                            contours[contourCount][i][j, 2] = (point.z);
                        }
                    }
                    contours[contourCount] = ClosedLooper(contours[contourCount]);
                }
            }
            return contours;

        }
        public static void BeamMaker(ref ExternalPlanSetup plan, StructureSet ss, double prescriptionDose)
        {
            //need to create two arc beams, and make sure they fit to PTVs.
            ExternalBeamMachineParameters ebmp = new ExternalBeamMachineParameters("VaUnit6TB", "6X", 600, "ARC", null);
            //First need to find the isocentre, which will be in the main PTV

            //find all the ptvs
            List<Structure> ptvs = new List<Structure>();
            List<Structure> mainPTVs = new List<Structure>();
            foreach (Structure structure in ss.Structures)
            {
                if (structure.Name.ToLower().Contains("ptv"))
                {
                    ptvs.Add(structure);
                    if (FindPTVNumber(structure.Name) == prescriptionDose / 100)
                    //Check if receiving prescription dose
                    {
                        mainPTVs.Add(structure);
                    }

                }
            }
            //Check if it's receiving the prescription dose. If so, set isocentre here. If there is more than 
            //One receiving the prescription dose, set at the average between the two.
            VVector isocentre;
            if (mainPTVs.Count > 0)
            {
                double x = 0;
                double y = 0;
                double z = 0;
                //Find average x,y,z
                for (int i = 0; i < mainPTVs.Count; i++)
                {
                    x += Math.Round(mainPTVs[i].CenterPoint.x / 10.0f) * 10.0f / mainPTVs.Count;
                    y += Math.Round(mainPTVs[i].CenterPoint.y / 10.0f) * 10.0f / mainPTVs.Count;
                    z += Math.Round(mainPTVs[i].CenterPoint.z / 10.0f) * 10.0f / mainPTVs.Count;
                }
                isocentre = new VVector(x, y, z);
            }
            else
            {
                isocentre = new VVector(0, 0, 0);
            }
            //Create two VMAT beams

            //First get the right jaw dimensions: 
            VRect<double> jaws1 = FitJawsToTarget(isocentre, plan, ptvs, 30, 0);
            VRect<double> jaws2 = FitJawsToTarget(isocentre, plan, ptvs, 330, 0);
            Beam vmat1 = plan.AddArcBeam(ebmp, jaws1, 30, 181, 179, GantryDirection.Clockwise, 0, isocentre);
            Beam vmat2 = plan.AddArcBeam(ebmp, jaws2, 330, 179, 181, GantryDirection.CounterClockwise, 0, isocentre);
            vmat1.Id = "vmat1";
            vmat2.Id = "vmat2";


        }
        public static VRect<double> FitJawsToTarget(VVector isocentre, ExternalPlanSetup plan, List<Structure> ptvs, double collimatorAngleInDeg, double margin)
        {
            var collimatorAngleInRad = DegToRad(collimatorAngleInDeg);
            double xMin = isocentre.x;
            double yMin = isocentre.y;
            double xMax = isocentre.x;
            double yMax = isocentre.y;

            for (int gantryRotationInDeg = 0; gantryRotationInDeg < 360; gantryRotationInDeg += 30)
            {
                double gantryRotationInRad = DegToRad(gantryRotationInDeg);


                var nPlanes = plan.StructureSet.Image.ZSize;
                //Need to approximate the rotating gantry as finite number of static fields. approximate it as 6 static fields
                for (int i = 0; i < ptvs.Count; i++)
                {
                    for (int z = 0; z < nPlanes; z++)
                    {
                        var contoursOnImagePlane = ptvs[i].GetContoursOnImagePlane(z);
                        if (contoursOnImagePlane != null && contoursOnImagePlane.Length > 0)
                        {
                            foreach (var contour in contoursOnImagePlane)
                            {
                                AdjustJawSizeForContour(ref xMin, ref xMax, ref yMin, ref yMax, isocentre, contour, gantryRotationInRad, collimatorAngleInRad);
                            }
                        }
                    }
                }
            }
            return new VRect<double>(xMin - margin, yMin - margin, xMax + margin, yMax + margin);
        }

        private static void AdjustJawSizeForContour(ref double xMin, ref double xMax, ref double yMin, ref double yMax, VVector isocentre, IEnumerable<VVector> contour, double gantryRtnInRad, double collRtnInRad, bool exactFit = false)
        {
            foreach (var point in contour)
            {
                var projection = ProjectToBeamEyeView(point, isocentre, gantryRtnInRad, collRtnInRad);
                var xCoord = projection.Item1;
                var yCoord = projection.Item2;

                // Update the coordinates for jaw positions.
                if (xCoord < xMin)
                {
                    xMin = xCoord;
                }

                if (xCoord > xMax)
                {
                    xMax = xCoord;
                }

                if (yCoord < yMin)
                {
                    yMin = yCoord;
                }

                if (yCoord > yMax)
                {
                    yMax = yCoord;
                }
            }
        }


        /// <summary>
        /// Project a given point to beam's eye view. Assumes head first supine treatment orientation.
        /// </summary>
        private static Tuple<double, double> ProjectToBeamEyeView(VVector point, VVector isocentre, double gantryRtnInRad, double collRtnInRad)
        {
            // Calculate coordinates with respect to isocentre location.
            var p = point - isocentre;

            // Calculate the components of a vector corresponding to beam direction (from isocentre toward source).
            var nx = Math.Cos(gantryRtnInRad - Math.PI / 2.0);
            var ny = Math.Sin(gantryRtnInRad - Math.PI / 2.0);

            // Calculate the projection of a contour point p on the plane orthogonal to beam direction such that collimator rotation is taken into account.
            var cosCollRtn = Math.Cos(collRtnInRad);
            var sinCollRtn = Math.Sin(collRtnInRad);
            var xCoord = cosCollRtn * (nx * p.y - ny * p.x) + sinCollRtn * p.z;
            var yCoord = sinCollRtn * (ny * p.x - nx * p.y) + cosCollRtn * p.z;

            return new Tuple<double, double>(xCoord, yCoord);
        }


        

       
        public static List<Structure> AssignStructure(StructureSet ss, ROI roi) //return a list containing a list of structures matching the ROI for each constrained ROI
        {
            string name = roi.Name;
            List<Structure> dicomStructures = new List<Structure>();
            //Now need to find out which structure matches with the constraint ROI.
            //Now compare all structures with ROI using Damershau Lamershtein test for similarity. For PTVs, have to be more careful though since there can be variable amounts of 
            //different kinds. 

            if (!name.ToLower().Contains("ptv")) //If not PTV 
            {
                int closeness = 0; //structure giving smallest will be the match.
                int closest = 100;
                foreach (Structure structure in ss.Structures)
                {

                    string structureName = structure.Name; //get name of structure in eclipse

                    closeness = StringDistance(structureName, name);
                    if ((closeness < closest) && (LongestSubstring(structureName, name) >= 3) && (AllowedToMatch(structureName, name))) //must be closest, and also have a substring of at least 3 in common with other. 
                    {
                        dicomStructures.Clear();
                        dicomStructures.Add(structure);
                    }
                    else if (closeness == closest)  //add it to the list if its equal in closeness to other. 
                    {
                        dicomStructures.Add(structure);
                    }

                }

            }
            else //It is a PTV:
            {
                //Get the type of PTV 
                int PTV_Type = FindPTVNumber(name);
                if (PTV_Type == 111)
                {
                    //foreach (Structure structure in ss.Structures)
                    //{

                    //    if (structure.Name.ToLower().Contains("ptv"))
                    //    {
                    //        dicomStructures.Add(structure);
                    //    }

                    //}
                }
                else  //if PTV has a number
                {
                    foreach (Structure structure in ss.Structures)
                    {

                        if ((structure.Name.ToLower().Contains(PTV_Type.ToString())))
                        {
                            dicomStructures.Add(structure);
                        }

                    }
                }

            }
            return dicomStructures;
        }
        public static bool AllowedToMatch(string s1, string s2) //disqualify structures from matching if they don't have special words in common. 
        {
            bool allowed = true;
            List<string> keywords = new List<string>();
            keywords.Add("prv");
            keywords.Add("ptv");
            keywords.Add("stem");
            keywords.Add("cord");
            keywords.Add("chi");
            keywords.Add("opt");
            keywords.Add("ner");
            keywords.Add("par");
            keywords.Add("globe");
            keywords.Add("lips");
            keywords.Add("cav");
            keywords.Add("sub");
            keywords.Add("test");
            int num;
            for (int i = 0; i < keywords.Count; i++)
            {
                num = 0;
                if (s1.ToLower().Contains(keywords[i]))
                {
                    num += 1;
                }
                if (s2.ToLower().Contains(keywords[i]))
                {
                    num += 1;
                }
                if (num == 1)
                {
                    allowed = false;
                }
            }
            //Also can't have left and no L in another. Or right and no R.
            if ((s1.ToLower().Contains("left")) || (s2.ToLower().Contains("left")))
            {
                num = 0;
                if (s1.ToLower().Contains("l"))
                {
                    num += 1;
                }
                if (s2.ToLower().Contains("l"))
                {
                    num += 1;
                }
                if (num == 1)
                {
                    allowed = false;
                }
            }
            if ((s1.ToLower().Contains("right")) || (s2.ToLower().Contains("right")))
            {
                num = 0;
                if (s1.ToLower().Contains("r"))
                {
                    num += 1;
                }
                if (s2.ToLower().Contains("r"))
                {
                    num += 1;
                }
                if (num == 1)
                {
                    allowed = false;
                }
            }
            //also an issue if has _L_ or " L " or " L-" or "left" in only one. 
            if ((s1.ToLower().Contains("_l_")) || (s1.ToLower().Contains(" l ")) || (s1.ToLower().Contains(" l-")) || (s1.ToLower().Contains("-l-")) || (s1.ToLower().Contains(" l_")) || (s1.ToLower().Contains("_l ")) || (s1.ToLower().Contains("-l ")) || (s1.ToLower().Contains("left")) || (s1.ToLower().StartsWith("l ")))
            {
                if (!((s2.ToLower().Contains("_l_")) || (s2.ToLower().Contains(" l ")) || (s2.ToLower().Contains(" l-")) || (s2.ToLower().Contains("-l-")) || (s2.ToLower().Contains(" l_")) || (s2.ToLower().Contains("_l ")) || (s2.ToLower().Contains("-l ")) || (s2.ToLower().Contains("left")) || (s2.ToLower().StartsWith("l "))))
                {
                    allowed = false;
                }
            }
            if ((s1.ToLower().Contains("_r_")) || (s1.ToLower().Contains(" r ")) || (s1.ToLower().Contains(" r-")) || (s1.ToLower().Contains("-r-")) || (s1.ToLower().Contains(" r_")) || (s1.ToLower().Contains("_r ")) || (s1.ToLower().Contains("-r ")) || (s1.ToLower().Contains("right")) || (s1.ToLower().StartsWith("r ")))
            {
                if (!((s2.ToLower().Contains("_r_")) || (s2.ToLower().Contains(" r ")) || (s2.ToLower().Contains(" r-")) || (s2.ToLower().Contains("-r-")) || (s2.ToLower().Contains(" r_")) || (s2.ToLower().Contains("_r ")) || (s2.ToLower().Contains("-r ")) || (s2.ToLower().Contains("right")) || (s2.ToLower().StartsWith("r "))))
                {
                    allowed = false;
                }
            }
            //Also seems to happen for lt and rt
            if ((s1.ToLower().Contains("_lt_")) || (s1.ToLower().Contains(" lt ")) || (s1.ToLower().Contains(" lt-")) || (s1.ToLower().Contains("-lt-")) || (s1.ToLower().Contains(" lt_")) || (s1.ToLower().Contains("_lt ")) || (s1.ToLower().Contains("-lt ")) || (s1.ToLower().Contains("left")) || (s1.ToLower().StartsWith("lt ")) || (s1.ToLower().StartsWith("lt ")))
            {
                if (!((s2.ToLower().Contains("_lt_")) || (s2.ToLower().Contains(" lt ")) || (s2.ToLower().Contains(" lt-")) || (s2.ToLower().Contains("-lt-")) || (s2.ToLower().Contains(" lt_")) || (s2.ToLower().Contains("_lt ")) || (s2.ToLower().Contains("-lt ")) || (s2.ToLower().Contains("left")) || (s2.ToLower().StartsWith("lt ")) || (s2.ToLower().StartsWith("lt "))))
                {
                    allowed = false;
                }
            }
            if ((s1.ToLower().Contains("_rt_")) || (s1.ToLower().Contains(" rt ")) || (s1.ToLower().Contains(" rt-")) || (s1.ToLower().Contains("-rt-")) || (s1.ToLower().Contains(" rt_")) || (s1.ToLower().Contains("_rt ")) || (s1.ToLower().Contains("-rt ")) || (s1.ToLower().Contains("right")) || (s1.ToLower().StartsWith("rt ")) || (s1.ToLower().StartsWith("rt ")))
            {
                if (!((s2.ToLower().Contains("_rt_")) || (s2.ToLower().Contains(" rt ")) || (s2.ToLower().Contains(" rt-")) || (s2.ToLower().Contains("-rt-")) || (s2.ToLower().Contains(" rt_")) || (s2.ToLower().Contains("_rt ")) || (s2.ToLower().Contains("-rt ")) || (s2.ToLower().Contains("right")) || (s2.ToLower().StartsWith("rt ")) || (s2.ToLower().StartsWith("rt "))))
                {
                    allowed = false;
                }
            }

            //parotids are tricky, so basically if it has par, and an L its left, and if no L its right.
            if ((s1.ToLower().Contains("par")) && (s2.ToLower().Contains("par")))
            {
                if (((s1.ToLower().Contains("l")) && !(s2.ToLower().Contains("l"))) || ((s2.ToLower().Contains("l")) && !(s1.ToLower().Contains("l"))))
                {
                    allowed = false;
                }
            }
            return allowed;
        }
        public static int FindPTVNumber(string name)
        {
            //sometimes there is the PTV number (2 digits) and then also a number for listing structures, so need to separate these and take the larger
            List<string> number = new List<string>();
            string num = "";
            bool isDigit = false;
            int index = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (Char.IsDigit(name[i]))
                {
                    num += name[i];
                    isDigit = true;
                }
                else
                {
                    if (isDigit == true)
                    {
                        number.Add(num);
                        isDigit = false;
                        num = "";
                        index++;
                    }
                }
                if (num != "")
                {
                    number.Add(num);
                }

            }//now find the largest number
            int longest = 0;
            if (number.Count > 0)
            {
                for (int ind = 0; ind < number.Count; ind++)
                {
                    if (Convert.ToInt32(number[ind]) > longest)
                    {
                        longest = Convert.ToInt32(number[ind]);
                    }
                }

                if (longest > 1000)
                {
                    longest /= 100;
                }
                return longest;

            }
            else
            {

                return 111; //error 111 means that no number was found with the ptv name.
            }
        }

        public static int StringDistance(string firstText, string secondText) //Finding out how close strings are (Damerau Levenshtein)
        {
            var n = firstText.Length + 1;
            var m = secondText.Length + 1;
            var arrayD = new int[n, m];

            for (var i = 0; i < n; i++)
            {
                arrayD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                arrayD[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var cost = firstText[i - 1] == secondText[j - 1] ? 0 : 1;

                    arrayD[i, j] = Minimum(arrayD[i - 1, j] + 1, // delete
                                                            arrayD[i, j - 1] + 1, // insert
                                                            arrayD[i - 1, j - 1] + cost); // replacement

                    if (i > 1 && j > 1
                       && firstText[i - 1] == secondText[j - 2]
                       && firstText[i - 2] == secondText[j - 1])
                    {
                        arrayD[i, j] = Minimum(arrayD[i, j],
                        arrayD[i - 2, j - 2] + cost); // permutation
                    }
                }
            }

            return arrayD[n - 1, m - 1];
        }
        static int Minimum(int a, int b)
        {
            if (a < b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        static int Minimum(int a, int b, int c)
        {
            if ((a < b) && (a < c))
            {
                return a;
            }
            else if (b < c)
            {
                return b;
            }
            else
            {
                return c;
            }
        }
        public static int LongestSubstring(string X, string Y) //Longest common substring
        {
            int m = X.Length;
            int n = Y.Length;
            // Create an array to store lengths of  
            // longest common suffixes of substrings. 

            int[,] LCStuff = new int[m + 1, n + 1];

            // To store length of the longest common 
            // substring 
            int result = 0;

            // Following steps build LCSuff[m+1][n+1]  
            // in bottom up fashion 
            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (i == 0 || j == 0)
                        LCStuff[i, j] = 0;
                    else if (X[i - 1] == Y[j - 1])
                    {
                        LCStuff[i, j] =
                                LCStuff[i - 1, j - 1] + 1;

                        result = Math.Max(result,
                                          LCStuff[i, j]);
                    }
                    else
                        LCStuff[i, j] = 0;
                }
            }

            return result;
        }
        private static double DegToRad(double angle)
        {
            const double degToRad = Math.PI / 180.0D;
            return angle * degToRad;
        }
        public static List<double[,]> ClosedLooper(List<double[,]> contours)
        //Here we ensure that each contour forms a closed loop. 
        {

            for (int i = 0; i < contours.Count; i++)
            {
                int numRows = contours[i].Length / 3;
                double x1 = contours[i][0, 0];
                double x2 = contours[i][numRows - 1, 0];
                double y1 = contours[i][0, 1];
                double y2 = contours[i][numRows - 1, 1];
                if ((x1 != x2) || (y1 != y2))
                {
                    contours[i] = FirstToLast(contours[i]);
                }
            }
            return contours;
        }
        public static double[,] ClosedLooper(double[,] contours)
        //Here we ensure that each contour forms a closed loop. 
        {
            int numRows = contours.Length / 3;
            double x1 = contours[0, 0];
            double x2 = contours[numRows - 1, 0];
            double y1 = contours[0, 1];
            double y2 = contours[numRows - 1, 1];
            if ((x1 != x2) || (y1 != y2))
            {
                contours = FirstToLast(contours);
            }


            return contours;
        }
        public static List<double[]> ClosedLooper(List<double[]> contours)
        //Here we ensure that each contour forms a closed loop. 
        {
            double x1 = contours[0][0];
            double x2 = contours[contours.Count - 1][0];
            double y1 = contours[0][1];
            double y2 = contours[contours.Count - 1][1];
            if ((x1 != x2) || (y1 != y2))
            {
                contours = FirstToLast(contours);
            }
            return contours;
        }

        public static double[,] FirstToLast(double[,] a)
        //Add the first row to the end of an array. (close loops)
        {
            double[,] b = new double[a.Length / 3 + 1, 3];
            int row = 0;
            for (int j = 0; j < a.Length; j++)
            {
                int column = j % 3;

                b[row, column] = a[row, column];

                if (column == 2)
                {
                    row++;
                }
            }
            b[a.Length / 3, 0] = a[0, 0];
            b[a.Length / 3, 1] = a[0, 1];
            b[a.Length / 3, 2] = a[0, 2];
            return b;
        }
        public static List<double[]> FirstToLast(List<double[]> a)
        //Add the first row to the end of an array. (close loops)
        {
            a.Add(a[0]);
            return a;
        }
        public static double[,] AddPoint(double[,] a, int index, double[] point)
        //Add the first row to the end of an array. (close loops)
        {
            double[,] b = new double[a.Length / 3 + 1, 3];
            int row = 0;
            if (index != 0)
            {
                for (int j = 0; j < index * 3; j++)
                {
                    int column = j % 3;

                    b[row, column] = a[row, column];

                    if (column == 2)
                    {
                        row++;
                    }
                }
            }
            b[index, 0] = point[0];
            b[index, 1] = point[1];
            b[index, 2] = point[2];
            if (index != a.Length / 3)
            {
                row = index + 1;
                for (int j = index * 3; j < b.Length - 3; j++)
                {
                    int column = j % 3;

                    b[row, column] = a[row - 1, column];

                    if (column == 2)
                    {
                        row++;
                    }
                }
            }

            return b;
        }
        public static List<double[,]> IslandRemover(List<double[,]> contours)
        //This function will remove ROI contour islands if they exist.
        //Basic idea is to search through contour points, and if there is a large
        //variation in x or y from one contour to the next, then remove the contour which is 
        //furthest from the mean.
        {
            int numIslands = 0;
            int numContours = contours.Count;
            double meanX = 0;
            double meanY = 0;
            int maxSep = 30; //island cutoff criteria (mm), difference in means between adjacent contours (X,y)
            List<double> means = new List<double>();


            //first get the mean x,y,z for the whole ROI:
            meanX = SliceMean(0, contours);
            meanY = SliceMean(1, contours);
            means.Add(meanX);
            means.Add(meanY);


            //Now go through and check for large variation between adjacent contours: 
            //Currently using a difference of 2cm means between adjacent contours to flag an island
            for (int i = 0; i < numContours - 2; i++)
            {
                //Another for loop to check both x and y columns:
                for (int col = 0; col < 2; col++)
                {
                    double mean1 = SliceMean(col, contours[i]);
                    double mean2 = SliceMean(col, contours[i + 1]);
                    if (Math.Abs(mean1 - mean2) > maxSep)
                    {
                        numIslands++;
                        //Check which one is furthest from the ROI mean.
                        double dif1 = mean1 - means[col];
                        double dif2 = mean2 - means[col];

                        //remove the one furthest from the mean.
                        if (dif1 > dif2)    //remove contours[i]
                        {
                            contours.RemoveAt(i);
                        }
                        else     //Remove contours[i+1]
                        {
                            contours.RemoveAt(i + 1);
                        }
                    }
                }
            }
            if (numIslands == 0)
            {

            }
            else if (numIslands == 1)
            {
                System.Windows.MessageBox.Show(numIslands + " island detected and removed");
            }
            else
            {
                System.Windows.MessageBox.Show(numIslands + " islands detected and removed");
            }
            return contours;
        }
        public static double[] ReverseArray(double[] array)
        {
            double[] newArray = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[array.Length - 1 - i];
            }
            return newArray;
        }
        public static int[] ReverseArray(int[] array)
        {
            int[] newArray = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[array.Length - 1 - i];
            }
            return newArray;
        }
        public static double SliceMean(int dimension, double[,] points)
        {
            double totalSum = 0;
            double numPoints = 0;
            for (int i = 0; i < points.Length / 3; i++)
            {
                totalSum += points[i, dimension];
                numPoints++;
            }
            return totalSum / numPoints;
        }
        public static double SliceMean(int dimension, List<double[,]> points)
        {
            double totalSum = 0;
            double numPoints = 0;
            for (int j = 0; j < points.Count; j++)
            {
                for (int i = 0; i < points[j].Length / 3; i++)
                {
                    totalSum += points[j][i, dimension];
                    numPoints++;
                }
            }
            return totalSum / numPoints;
        }
        public static List<List<double[,]>> Chop(List<double[,]> contoursTemp, int numCutsX, int numCutsY, int numCutsZ, string organName)
        {

            //Now make the axial cuts first: 
            double[] zCuts = new double[numCutsZ];
            zCuts = BestCutZ(contoursTemp, numCutsZ);    //find out where to cut the structure along z
            List<List<double[,]>> axialDivs;
            if (numCutsZ != 0)
            {
                axialDivs = ZChop(contoursTemp, zCuts); //Perform the z cuts //holds the different sections after just the z-cuts.
            }
            else
            {
                axialDivs = new List<List<double[,]>>();
                axialDivs.Add(contoursTemp);
            }
            //For each z chunk, need to recursively chop into y bits, and each y bit into x bits. 

            List<List<double[,]>> contoursY = new List<List<double[,]>>(); //to hold all chopped contours after y cuts
            if (numCutsY != 0)
            {
                for (int i = 0; i < axialDivs.Count; i++)
                {
                    List<List<double[,]>> temp = YChop(axialDivs[i], numCutsY);
                    for (int j = 0; j < temp.Count; j++)
                    {
                        contoursY.Add(temp[j].GetRange(0, temp[j].Count));
                    }
                }
            }
            else
            {
                contoursY = axialDivs;
            }

            //Now do the x chops to finish up!

            List<List<double[,]>> contours = new List<List<double[,]>>(); //to hold all chopped contours after all cuts
            if (numCutsX != 0)
            {
                for (int i = 0; i < contoursY.Count; i++)
                {
                    List<List<double[,]>> temp = XChop(contoursY[i], numCutsX);
                    for (int j = 0; j < temp.Count; j++)
                    {
                        contours.Add(temp[j].GetRange(0, temp[j].Count));
                    }
                }

            }
            else
            {
                contours = contoursY;
            }

            contours = ReOrder(contours, organName, numCutsX, numCutsY, numCutsZ);
            return contours;



        }
        public static List<List<double[,]>> ReOrder(List<List<double[,]>> contours, string organName, int numCutsX, int numCutsY, int numCutsZ)
        {
            //Reorder the organ from inferior --> superior, medial --> lateral, anterior --> posterior,
            //make 2D array which holds the mean x,y,z for each contour.
            int j = 0;
            List<List<double[,]>> finalContours = new List<List<double[,]>>();
            if (organName.ToLower().Contains("l"))    //if the left (medial --> lateral is increasing x.
            {
                for (int i = 0; i < contours.Count; i++)
                {
                    if ((i % (numCutsZ + 1) == 0) && (i != 0))
                    {
                        j += 1;
                    }
                    int index = j % ((numCutsX + 1) * (numCutsY + 1) * (numCutsZ + 1));
                    finalContours.Add(contours[index]);
                    j += (numCutsX + 1) * (numCutsY + 1);
                }
            }
            else
            {
                j = numCutsX;
                for (int i = 0; i < contours.Count; i++)
                {
                    if (j >= (numCutsX + 1) * (numCutsY + 1) * (numCutsZ + 1))
                    {
                        j -= (numCutsX + 1) * (numCutsY + 1) * (numCutsZ + 1) + 1;
                        if (j == -1)
                        {
                            j += (numCutsX + 1) * (numCutsY + 1);
                        }
                    }

                    finalContours.Add(contours[j]);
                    j += (numCutsX + 1) * (numCutsY + 1);

                }
            }
            return finalContours;

        }
        public static List<List<double[,]>> XChop(List<double[,]> contours, int numCutsX)
        {
            double[] xCuts = BestCutX(contours, numCutsX, 0.0001);
            // add intersection points
            for (int i = 0; i < contours.Count; i++)
            {
                contours[i] = AddIntersectionsX(contours[i], xCuts);
                contours[i] = ClosedLooper(contours[i]);
            }

            //////////////////////////////////////
            //Now divide into separate parts.
            ///////////////////////////////////////
            List<List<double[,]>> finalContours = new List<List<double[,]>>();
            //make a list for each x division for the current contour.
            List<List<double[]>> divisions = new List<List<double[]>>();

            //Make the list the correct size so that there is an item for each x division.
            for (int div = 0; div <= xCuts.Length; div++)
            {
                divisions.Add(new List<double[]>());
                finalContours.Add(new List<double[,]>());
            }
            for (int i = 0; i < contours.Count; i++)    //for all of the contours
            {
                divisions.Clear();
                for (int div = 0; div <= xCuts.Length; div++)
                {
                    divisions.Add(new List<double[]>());
                }
                for (int x = 0; x <= xCuts.Length; x++) //a section for every cut, + 1
                {
                    for (int j = 0; j < contours[i].Length / 3; j++)    //loop through all points
                    {

                        if (x == 0)
                        {
                            if (contours[i][j, 0] <= xCuts[x])
                            {
                                divisions[x].Add(new double[] { contours[i][j, 0], contours[i][j, 1], contours[i][j, 2] });

                            }
                        }
                        else if (x == xCuts.Length)
                        {


                            if (contours[i][j, 0] >= xCuts[x - 1])
                            {
                                divisions[x].Add(new double[] { contours[i][j, 0], contours[i][j, 1], contours[i][j, 2] });
                            }
                        }
                        else
                        {
                            if ((contours[i][j, 0] >= xCuts[x - 1]) && (contours[i][j, 0] <= xCuts[x]))
                            {
                                divisions[x].Add(new double[] { contours[i][j, 0], contours[i][j, 1], contours[i][j, 2] });
                            }
                        }
                    }
                }
                //at this point divisions has a list item holding a list of array points for each cut.
                //Need to now make double arrays for each of these and add them to new final list.
                double[,] temp;
                for (int x = 0; x <= xCuts.Length; x++) //a section for every cut, + 1
                {
                    temp = new double[divisions[x].Count, 3];
                    for (int row = 0; row < temp.Length / 3; row++)
                    {
                        temp[row, 0] = divisions[x][row][0];
                        temp[row, 1] = divisions[x][row][1];
                        temp[row, 2] = divisions[x][row][2];
                    }
                    if (temp.Length != 0)
                    {
                        temp = ClosedLooper(temp);
                        finalContours[x].Add(temp);
                    }
                }
            }
            return finalContours;
        }
        public static double[] BestCutX(List<double[,]> contours, int numCuts, double errorTolerance)
        {
            double[] xCuts = new double[numCuts];
            double area = 0;
            double maxX = -1000;
            double minX = 1000;
            double error, xCut, newArea;

            //Get total area of contours:
            for (int i = 0; i < contours.Count; i++)
            {
                if (contours[i].Length != 0)
                {
                    area += Area(contours[i]);
                }
            }
            for (int cut = 0; cut < numCuts; cut++)
            {
                double areaGoal = (double)(cut + 1) / (numCuts + 1); // fractional area goal for each cut.
                //Now get the max and min Y for the structure
                for (int j = 0; j < contours.Count; j++)
                {
                    if (contours[j].Length != 0)
                    {
                        for (int row = 0; row < contours[j].Length / 3; row++)
                        //get the maximum, minimum y
                        {
                            if (contours[j][row, 0] > maxX)
                            {
                                maxX = contours[j][row, 0];
                            }
                            if (contours[j][row, 0] < minX)
                            {
                                minX = contours[j][row, 0];
                            }

                        }
                    }
                }
                //Now iteratively get equal volumes to the error allowance set
                List<double[,]> tempContours = new List<double[,]>();
                List<double[]> cutContours = new List<double[]>(); //store contour as a list to append easily.
                do
                {

                    tempContours.Clear();
                    xCut = (minX + maxX) / 2;
                    newArea = 0;
                    //First add the intersection points: 
                    for (int i = 0; i < contours.Count; i++)
                    {
                        cutContours.Clear();
                        if (contours[i].Length != 0)
                        {
                            tempContours.Add(AddIntersectionsX(contours[i], xCut));
                            tempContours[i] = ClosedLooper(tempContours[i]);
                            //now make a new contour with points below yCut.
                            for (int j = 0; j < tempContours[i].Length / 3; j++)
                            {
                                if (tempContours[i][j, 0] <= xCut)
                                {
                                    cutContours.Add(new double[] { tempContours[i][j, 0], tempContours[i][j, 1], tempContours[i][j, 2] });
                                }
                            }
                            if (cutContours.Count != 0)
                            {
                                cutContours = ClosedLooper(cutContours);
                                newArea += Area(cutContours);
                            }
                        }
                    }
                    //Now compare areas:
                    if (newArea / area < areaGoal)
                    {
                        minX = xCut;
                    }
                    else if (newArea / area > areaGoal)
                    {
                        maxX = xCut;
                    }
                    error = Math.Abs((newArea / area) - areaGoal);

                } while (error > errorTolerance);

                xCuts[cut] = xCut;
            }
            return xCuts;
        }
        public static double[,] AddIntersectionsX(double[,] contours, double xCut)
        {
            double m;
            double yNew;
            double[,] finalContours = contours;

            int numAdded = 1; //start at one, increment after adding each point, to keep track of where to add next additional point (add to index)
            //index 0 outside of loop:
            if ((contours[0, 0] > xCut) & (contours[contours.Length / 3 - 1, 0] < xCut))
            {
                m = (contours[0, 0] - contours[contours.Length / 3 - 1, 0]) / (contours[0, 1] - contours[contours.Length / 3 - 1, 1]);
                yNew = ((xCut - contours[contours.Length / 3 - 1, 0]) / m) + contours[contours.Length / 3 - 1, 1];
                finalContours = AddPoint(finalContours, 0, new double[] { xCut, yNew, contours[0, 2] });
                numAdded++;
            }
            if ((contours[0, 0] < xCut) & (contours[contours.Length / 3 - 1, 0] > xCut))
            {
                m = (contours[contours.Length / 3 - 1, 0] - contours[0, 0]) / (contours[contours.Length / 3 - 1, 1] - contours[0, 1]);
                yNew = ((xCut - contours[0, 0]) / m) + contours[0, 1];
                finalContours = AddPoint(finalContours, contours.Length / 3, new double[] { xCut, yNew, contours[0, 2] });
            }

            for (int i = 0; i < contours.Length / 3 - 1; i++)    //for all points, except last one will be out of loop
            {
                if ((contours[i, 0] < xCut) & (contours[i + 1, 0] > xCut)) //if x is below the cut
                {
                    m = (contours[i + 1, 0] - contours[i, 0]) / (contours[i + 1, 1] - contours[i, 1]);
                    yNew = ((xCut - contours[i, 0]) / m) + contours[i, 1];
                    finalContours = AddPoint(finalContours, i + numAdded, new double[] { xCut, yNew, contours[0, 2] });
                    numAdded++;

                }
                else if ((contours[i, 0] > xCut) & (contours[i + 1, 0] < xCut))
                {
                    m = (contours[i + 1, 0] - contours[i, 0]) / (contours[i + 1, 1] - contours[i, 1]);
                    yNew = ((xCut - contours[i, 0]) / m) + contours[i, 1];
                    finalContours = AddPoint(finalContours, i + numAdded, new double[] { xCut, yNew, contours[0, 2] });
                    numAdded++;
                }
            }
            return finalContours;

        }
        public static double[,] AddIntersectionsX(double[,] structures, double[] xCut)
        {
            double m;
            double yNew;
            double[,] finalContours = structures;
            for (int x = 0; x < xCut.Length; x++)
            {
                double[,] contours = finalContours;
                int numConts = finalContours.Length / 3;
                int numAdded = 1; //start at one, increment after adding each point, to keep track of where to add next additional point (add to index)
                                  //index 0 outside of loop:
                if ((contours[0, 0] > xCut[x]) & (contours[contours.Length / 3 - 1, 0] < xCut[x]))
                {
                    m = (contours[0, 0] - contours[contours.Length / 3 - 1, 0]) / (contours[0, 1] - contours[contours.Length / 3 - 1, 1]);
                    yNew = ((xCut[x] - contours[contours.Length / 3 - 1, 0]) / m) + contours[contours.Length / 3 - 1, 1];
                    finalContours = AddPoint(finalContours, 0, new double[] { xCut[x], yNew, contours[0, 2] });
                    numAdded++;
                }
                if ((contours[0, 0] < xCut[x]) & (contours[contours.Length / 3 - 1, 0] > xCut[x]))
                {
                    m = (contours[contours.Length / 3 - 1, 0] - contours[0, 0]) / (contours[contours.Length / 3 - 1, 1] - contours[0, 1]);
                    yNew = ((xCut[x] - contours[0, 0]) / m) + contours[0, 1];
                    finalContours = AddPoint(finalContours, contours.Length / 3, new double[] { xCut[x], yNew, contours[0, 2] });
                }

                for (int i = 0; i < contours.Length / 3 - 1; i++)    //for all points, except last one will be out of loop
                {
                    if ((contours[i, 0] < xCut[x]) & (contours[i + 1, 0] > xCut[x])) //if y is below the cut
                    {
                        m = (contours[i + 1, 0] - contours[i, 0]) / (contours[i + 1, 1] - contours[i, 1]);
                        yNew = ((xCut[x] - contours[i, 0]) / m) + contours[i, 1];
                        finalContours = AddPoint(finalContours, i + numAdded, new double[] { xCut[x], yNew, contours[0, 2] });
                        numAdded++;

                    }
                    else if ((contours[i, 0] > xCut[x]) & (contours[i + 1, 0] < xCut[x]))
                    {
                        m = (contours[i + 1, 0] - contours[i, 0]) / (contours[i + 1, 1] - contours[i, 1]);
                        yNew = ((xCut[x] - contours[i, 0]) / m) + contours[i, 1];
                        finalContours = AddPoint(finalContours, i + numAdded, new double[] { xCut[x], yNew, contours[0, 2] });
                        numAdded++;
                    }
                }
            }
            return finalContours;

        }
        public static List<List<double[,]>> YChop(List<double[,]> contours, int numCutsY)
        {

            double[] yCuts = BestCutY(contours, numCutsY, 0.0001);    //Contains y-values for cut locations

            // add intersection points
            for (int i = 0; i < contours.Count; i++)
            {
                contours[i] = AddIntersectionsY(contours[i], yCuts);
                contours[i] = ClosedLooper(contours[i]);
            }

            //////////////////////////////////////
            //Now divide into separate parts.
            ///////////////////////////////////////
            List<List<double[,]>> finalContours = new List<List<double[,]>>();
            //make a list for each y division for the current contour.
            List<List<double[]>> divisions = new List<List<double[]>>();

            //Make the list the correct size so that there is an item for each y division.
            for (int div = 0; div <= yCuts.Length; div++)
            {
                finalContours.Add(new List<double[,]>());
            }


            for (int i = 0; i < contours.Count; i++)    //for all of the contours
            {
                divisions.Clear();
                //Make the list the correct size so that there is an item for each y division.
                for (int div = 0; div <= yCuts.Length; div++)
                {
                    divisions.Add(new List<double[]>());
                }
                for (int y = 0; y <= yCuts.Length; y++) //a section for every cut, + 1
                {
                    for (int j = 0; j < contours[i].Length / 3; j++)    //loop through all points
                    {
                        if (y == 0)
                        {
                            if (contours[i][j, 1] <= yCuts[y])
                            {
                                divisions[y].Add(new double[] { contours[i][j, 0], contours[i][j, 1], contours[i][j, 2] });

                            }
                        }
                        else if (y == yCuts.Length)
                        {


                            if (contours[i][j, 1] >= yCuts[y - 1])
                            {
                                divisions[y].Add(new double[] { contours[i][j, 0], contours[i][j, 1], contours[i][j, 2] });
                            }
                        }
                        else
                        {
                            if ((contours[i][j, 1] >= yCuts[y - 1]) && (contours[i][j, 1] <= yCuts[y]))
                            {
                                divisions[y].Add(new double[] { contours[i][j, 0], contours[i][j, 1], contours[i][j, 2] });
                            }
                        }
                    }
                }
                //at this point divisions has a list item holding a list of array points for each cut.
                //Need to now make double arrays for each of these and add them to new final list.
                double[,] temp;
                for (int y = 0; y <= yCuts.Length; y++) //a section for every cut, + 1
                {
                    temp = new double[divisions[y].Count, 3];
                    for (int row = 0; row < temp.Length / 3; row++)
                    {
                        temp[row, 0] = divisions[y][row][0];
                        temp[row, 1] = divisions[y][row][1];
                        temp[row, 2] = divisions[y][row][2];
                    }

                    if (temp.Length != 0)
                    {
                        temp = ClosedLooper(temp);
                        finalContours[y].Add(temp);
                    }

                }
            }

            return finalContours;
        }
        public static double[] BestCutY(List<double[,]> contours, int numCuts, double errorTolerance)
        {
            double[] yCuts = new double[numCuts];
            double area = 0;
            double maxY = -1000;
            double minY = 1000;
            double error, yCut, newArea;

            //Get the total area of contours: 
            for (int i = 0; i < contours.Count; i++)
            {
                if (contours[i].Length != 0)
                {
                    area += Area(contours[i]);
                }

            }
            for (int cut = 0; cut < numCuts; cut++)
            {
                double areaGoal = (double)(cut + 1) / (numCuts + 1); // fractional area goal for each cut.
                //Now get the max and min Y for the structure
                for (int j = 0; j < contours.Count; j++)
                {
                    if (contours[j].Length != 0)
                    {
                        for (int row = 0; row < contours[j].Length / 3; row++)
                        //get the maximum, minimum y
                        {
                            if (contours[j][row, 1] > maxY)
                            {
                                maxY = contours[j][row, 1];
                            }
                            if (contours[j][row, 1] < minY)
                            {
                                minY = contours[j][row, 1];
                            }

                        }
                    }
                }
                //Now iteratively get equal volumes to the error allowance set
                List<double[,]> tempContours = new List<double[,]>();
                List<double[]> cutContours = new List<double[]>(); //store contour as a list to append easily.
                do
                {

                    tempContours.Clear();
                    yCut = (minY + maxY) / 2;
                    newArea = 0;
                    //First add the intersection points: 
                    for (int i = 0; i < contours.Count; i++)
                    {
                        cutContours.Clear();
                        if (contours[i].Length != 0)
                        {
                            tempContours.Add(AddIntersectionsY(contours[i], yCut));
                            tempContours[i] = ClosedLooper(tempContours[i]);
                            //now make a new contour with points below yCut.
                            for (int j = 0; j < tempContours[i].Length / 3; j++)
                            {
                                if (tempContours[i][j, 1] <= yCut)
                                {
                                    cutContours.Add(new double[] { tempContours[i][j, 0], tempContours[i][j, 1], tempContours[i][j, 2] });
                                }
                            }
                            if (cutContours.Count != 0)
                            {
                                cutContours = ClosedLooper(cutContours);
                                newArea += Area(cutContours);
                            }
                        }
                    }
                    //Now compare areas:
                    if (newArea / area < areaGoal)
                    {
                        minY = yCut;
                    }
                    else if (newArea / area > areaGoal)
                    {
                        maxY = yCut;
                    }
                    error = Math.Abs((newArea / area) - areaGoal);

                } while (error > errorTolerance);

                yCuts[cut] = yCut;
            }
            return yCuts;

        }
        public static double[,] AddIntersectionsY(double[,] contours, double yCut)
        {
            double m;
            double xNew;
            double[,] finalContours = contours;

            int numAdded = 1; //start at one, increment after adding each point, to keep track of where to add next additional point (add to index)
            //index 0 outside of loop:
            if ((contours[0, 1] > yCut) & (contours[contours.Length / 3 - 1, 1] < yCut))
            {
                m = (contours[0, 1] - contours[contours.Length / 3 - 1, 1]) / (contours[0, 0] - contours[contours.Length / 3 - 1, 0]);
                xNew = ((yCut - contours[contours.Length / 3 - 1, 1]) / m) + contours[contours.Length / 3 - 1, 0];
                finalContours = AddPoint(finalContours, 0, new double[] { xNew, yCut, contours[0, 2] });
                numAdded++;
            }
            if ((contours[0, 1] < yCut) & (contours[contours.Length / 3 - 1, 1] > yCut))
            {
                m = (contours[contours.Length / 3 - 1, 1] - contours[0, 1]) / (contours[contours.Length / 3 - 1, 0] - contours[0, 0]);
                xNew = ((yCut - contours[0, 1]) / m) + contours[0, 0];
                finalContours = AddPoint(finalContours, contours.Length / 3, new double[] { xNew, yCut, contours[0, 2] });
            }

            for (int i = 1; i < contours.Length / 3 - 1; i++)    //for all points, except last one will be out of loop
            {
                if ((contours[i, 1] < yCut) & (contours[i + 1, 1] > yCut)) //if y is below the cut
                {
                    m = (contours[i + 1, 1] - contours[i, 1]) / (contours[i + 1, 0] - contours[i, 0]);
                    xNew = ((yCut - contours[i, 1]) / m) + contours[i, 0];
                    finalContours = AddPoint(finalContours, i + numAdded, new double[] { xNew, yCut, contours[0, 2] });
                    numAdded++;

                }
                else if ((contours[i, 1] > yCut) & (contours[i + 1, 1] < yCut))
                {
                    m = (contours[i + 1, 1] - contours[i, 1]) / (contours[i + 1, 0] - contours[i, 0]);
                    xNew = ((yCut - contours[i, 1]) / m) + contours[i, 0];
                    finalContours = AddPoint(finalContours, i + numAdded, new double[] { xNew, yCut, contours[0, 2] });
                    numAdded++;
                }
            }
            return finalContours;

        }
        public static double[,] AddIntersectionsY(double[,] structure, double[] yCut)
        {
            double m;
            double xNew;
            double[,] finalContours = structure;
            for (int y = 0; y < yCut.Length; y++)

            {
                double[,] contours = finalContours;
                int numConts = finalContours.Length / 3;
                int numAdded = 1; //start at one, increment after adding each point, to keep track of where to add next additional point (add to index)
                                  //index 0 outside of loop:
                if ((contours[0, 1] > yCut[y]) & (contours[contours.Length / 3 - 1, 1] < yCut[y]))
                {
                    m = (contours[0, 1] - contours[contours.Length / 3 - 1, 1]) / (contours[0, 0] - contours[contours.Length / 3 - 1, 0]);
                    xNew = ((yCut[y] - contours[contours.Length / 3 - 1, 1]) / m) + contours[contours.Length / 3 - 1, 0];
                    finalContours = AddPoint(finalContours, 0, new double[] { xNew, yCut[y], contours[0, 2] });
                    numAdded++;
                }
                if ((contours[0, 1] < yCut[y]) & (contours[contours.Length / 3 - 1, 1] > yCut[y]))
                {
                    m = (contours[contours.Length / 3 - 1, 1] - contours[0, 1]) / (contours[contours.Length / 3 - 1, 0] - contours[0, 0]);
                    xNew = ((yCut[y] - contours[0, 1]) / m) + contours[0, 0];
                    finalContours = AddPoint(finalContours, contours.Length / 3, new double[] { xNew, yCut[y], contours[0, 2] });
                }
                for (int i = 1; i < numConts - 1; i++)    //for all points, except last one will be out of loop
                {
                    if ((contours[i, 1] < yCut[y]) & (contours[i + 1, 1] > yCut[y])) //if y is below the cut
                    {
                        m = (contours[i + 1, 1] - contours[i, 1]) / (contours[i + 1, 0] - contours[i, 0]);
                        xNew = ((yCut[y] - contours[i, 1]) / m) + contours[i, 0];
                        finalContours = AddPoint(finalContours, i + numAdded, new double[] { xNew, yCut[y], contours[0, 2] });
                        numAdded++;
                    }
                    else if ((contours[i, 1] > yCut[y]) & (contours[i + 1, 1] < yCut[y]))
                    {
                        m = (contours[i + 1, 1] - contours[i, 1]) / (contours[i + 1, 0] - contours[i, 0]);
                        xNew = ((yCut[y] - contours[i, 1]) / m) + contours[i, 0];
                        finalContours = AddPoint(finalContours, i + numAdded, new double[] { xNew, yCut[y], contours[0, 2] });
                        numAdded++;
                    }
                }
            }
            return finalContours;
        }
        public static double[] BestCutZ(List<double[,]> contours, int numCuts)
        {
            double[] zCuts = new double[numCuts];    //to hold z value of cut locations
            int numConts = contours.Count;
            double totalVolume = 0;
            double deltaZ = Math.Abs(contours[0][0, 2] - contours[1][0, 2]);    //distance between adjacent contours
            double[] contourAreas = new double[numConts];

            for (int i = 0; i < numConts; i++) //right now using area of every contour but last... should last be included? 
            {
                contourAreas[i] = Area(contours[i]);
                if (i != numConts - 1)
                {
                    totalVolume += contourAreas[i] * deltaZ;
                }
            }

            //Now find the right cut spots:
            //
            int contIndex;
            double subVolume;
            for (int i = 1; i <= numCuts; i++)
            {
                double volumeGoal = (double)i / (numCuts + 1);
                contIndex = 0;
                subVolume = 0;//eg. if 2 cuts, volume goal before first cut = 1/3, goal before second = 2/3.

                while (subVolume < volumeGoal * totalVolume)
                {
                    subVolume += contourAreas[contIndex] * deltaZ;
                    contIndex++;
                }
                //The cut will be between indices contIndex-1 and contIndex.
                //Now determine the volume below and ontop of the cut.
                double volumeBelow = 0;
                double avgArea;
                for (int j = 0; j < contIndex - 1; j++)
                {
                    //first get average area between two contours, used to approximate volume between the two.
                    avgArea = 0.5 * (contourAreas[j] + contourAreas[j + 1]);
                    volumeBelow += avgArea * deltaZ;

                }

                //Now get the average area for the slicing region:
                avgArea = 0.5 * (contourAreas[contIndex - 1] + contourAreas[contIndex]);


                zCuts[i - 1] = contours[contIndex - 1][0, 2] + (volumeGoal * totalVolume - volumeBelow) / avgArea;
            }

            return zCuts;


        }
        public static List<List<double[,]>> ZChop(List<double[,]> contours, double[] zCuts)
        {
            int[] sliceIndices = new int[zCuts.Length];
            double[,] newContour;
            int[] contoursZ;
            int numPoints;
            for (int i = 0; i < zCuts.Length; i++)    //for each cut
            {
                //get the closest 2 contours for the cut
                contoursZ = ClosestContourZ(zCuts[i], contours);
                numPoints = contours[contoursZ[0]].Length / 3;
                newContour = new double[numPoints, 3];

                for (int j = 0; j < numPoints; j++)
                {
                    double x = contours[contoursZ[0]][j, 0];
                    double y = contours[contoursZ[0]][j, 1];
                    double z = contours[contoursZ[0]][j, 2];
                    //Now get idx, the row index in the second closest contour. Then interpolate between the two.
                    int idx = ClosestPoint(x, y, contours[contoursZ[1]]);
                    double[] point1 = { x, y, z };
                    double[] point2 = { contours[contoursZ[1]][idx, 0], contours[contoursZ[1]][idx, 1], contours[contoursZ[1]][idx, 2] };
                    double[] newPoint = InterpolateXY(point1, point2, zCuts[i]);
                    //now add newPoint to the newContour:
                    newContour[j, 0] = newPoint[0];
                    newContour[j, 1] = newPoint[1];
                    newContour[j, 2] = newPoint[2];
                }
                //Add this new contour to the list.
                sliceIndices[i] = Math.Max(contoursZ[0], contoursZ[1]);
                contours.Insert(sliceIndices[i], newContour);
                //at this point new axial division contours have been added to original set. 
            }

            List<List<double[,]>> finalContours = new List<List<double[,]>>();
            List<double[,]> tempList = new List<double[,]>();

            for (int cut = 0; cut <= zCuts.Length; cut++)
            {
                tempList.Clear();

                if (cut == 0)
                {
                    for (int i = 0; i <= sliceIndices[cut]; i++)
                    {
                        tempList.Add(contours[i]);
                    }
                }
                else if (cut > 0 & cut < zCuts.Length)
                {
                    for (int i = sliceIndices[cut - 1]; i <= sliceIndices[cut]; i++)
                    {
                        tempList.Add(contours[i]);
                    }

                }
                else
                {
                    for (int i = sliceIndices[cut - 1]; i <= contours.Count - 1; i++)
                    {
                        tempList.Add(contours[i]);
                    }
                }
                finalContours.Add(tempList.GetRange(0, tempList.Count));
                //finalContours[cut] = ContourFixing.ClosedLooper(finalContours[cut]);
            }
            //now form closed loops: 

            return finalContours;
        }
        public static int[] ClosestContourZ(double zCut, List<double[,]> contours)
        {
            double temp = 1000;
            int[] closestContours = new int[2];

            //first find the closest contour to the zCut: 
            for (int i = 0; i < contours.Count; i++)
            {
                double contourDistance = Math.Abs(contours[i][0, 2] - zCut);
                if (contourDistance < temp)
                {
                    closestContours[0] = i;
                    temp = contourDistance;
                }
            }
            //Now get the second closest contour: 
            temp = 1000;
            for (int i = 0; i < contours.Count; i++)
            {
                if (i == closestContours[0])
                {
                    continue;
                }
                double contourDistance = Math.Abs(contours[i][0, 2] - zCut);
                if (contourDistance < temp)
                {
                    closestContours[1] = i;
                    temp = contourDistance;
                }
            }
            return closestContours;
        }
        public static int ClosestPoint(double x, double y, double[,] points)
        {
            double m = 1000;
            int closestPoint = 1000;
            for (int i = 0; i < points.Length / 3; i++)
            {
                double diff = Math.Sqrt(Math.Pow((x - points[i, 0]), 2) + Math.Pow((y - points[i, 1]), 2));    //difference between points in xy plane
                if (diff < m)
                {
                    closestPoint = i;
                    m = diff;
                }
            }
            if (closestPoint == 1000)
            {
                System.Windows.MessageBox.Show("Closest Point not found, terminating.");

            }
            return closestPoint;
        }
        public static double[] InterpolateXY(double[] point1, double[] point2, double z)
        {
            if (point1[2] == z)
            {
                return point1;
            }
            if (point2[2] == z)
            {
                return point2;
            }
            double xSlope = (point2[0] - point1[0]) / (point2[2] - point1[2]);
            double ySlope = (point2[1] - point1[1]) / (point2[2] - point1[2]);

            double newX = point1[0] + xSlope * (z - point1[2]);
            double newY = point1[1] + ySlope * (z - point1[2]);

            double[] newPoint = new double[3] { newX, newY, z };
            return newPoint;

        }
        public static double Area(double[,] pointSet)
        {
            int d = pointSet.Length / 3;
            double area = 0;
            for (int i = 0; i < d - 1; i++)
            {
                area += pointSet[i, 0] * pointSet[i + 1, 1] - pointSet[i + 1, 0] * pointSet[i, 1];
            }
            area += pointSet[d - 1, 0] * pointSet[0, 1] - pointSet[0, 0] * pointSet[d - 1, 1];
            return 0.5 * Math.Abs(area);

        }
        public static double Area(List<double[]> pointSet)
        {
            int d = pointSet.Count;
            double area = 0;
            for (int i = 0; i < d - 1; i++)
            {
                area += (pointSet[i][0] * pointSet[i + 1][1]) - (pointSet[i + 1][0] * pointSet[i][1]);
            }
            area += (pointSet[d - 1][0] * pointSet[0][1]) - (pointSet[0][0] * pointSet[d - 1][1]);
            return 0.5 * Math.Abs(area);

        }

        public static int[] LowestValIndices(double[,] a, int dim, int numberPoints)
        //returns a list of indices corresponding to the lowest values within a list at a certain dimension.
        {
            double[] min = { a[0, dim], 0 };    //first entry min, second corresponding index
            List<int> used = new List<int>();
            int[] smalls = new int[numberPoints];

            for (int i = 0; i < numberPoints; i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                {
                    if ((a[j, dim] < min[0]) && (!used.Contains(j)))
                    {
                        min[0] = a[j, dim];
                        min[1] = j;
                    }
                }
                smalls[i] = (int)min[1];
                used.Add(smalls[i]);
                min[0] = 1000;
                min[1] = 0;
            }
            smalls = Sort(smalls);
            smalls = ReverseArray(smalls);
            return smalls;
        }
        public static int[] Sort(int[] a)
        {
            int[] b = new int[a.Length];
            int temp;
            for (int i = 0; i < a.Length - 1; i++)
            {
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (a[i] > a[j])
                    {
                        temp = a[i];
                        a[i] = a[j];
                        a[j] = temp;
                    }
                }
            }
            return a;
        }
        public static double[,] RemovePoint(double[,] a, int index)
        //Add the first row to the end of an array. (close loops)
        {
            double[,] b = new double[a.Length / 3 - 1, 3];
            int row = 0;
            if (index != 0)
            {
                for (int j = 0; j < index * 3; j++)
                {
                    int column = j % 3;

                    b[row, column] = a[row, column];

                    if (column == 2)
                    {
                        row++;
                    }
                }
            }
            if (index != a.Length / 3)
            {
                row = index;
                for (int j = index * 3; j < b.Length - 3; j++)
                {
                    int column = j % 3;

                    b[row, column] = a[row + 1, column];

                    if (column == 2)
                    {
                        row++;
                    }
                }
            }

            return b;
        }
        public static int[,] RemovePoint(int[,] a, int index)
        //Add the first row to the end of an array. (close loops)
        {
            int[,] b = new int[a.Length / 3 - 1, 3];
            int row = 0;
            if (index != 0)
            {
                for (int j = 0; j < index * 3; j++)
                {
                    int column = j % 3;

                    b[row, column] = a[row, column];

                    if (column == 2)
                    {
                        row++;
                    }
                }
            }
            if (index != a.Length / 3)
            {
                row = index + 1;
                for (int j = index * 3; j < b.Length - 3; j++)
                {
                    int column = j % 3;

                    b[row, column] = a[row + 1, column];

                    if (column == 2)
                    {
                        row++;
                    }
                }
            }

            return b;
        }
        public static int[] RemovePoint(int[] a, int index)
        //Add the first row to the end of an array. (close loops)
        {
            int[] b = new int[a.Length - 1];
            if (index != 0)
            {
                for (int j = 0; j < index; j++)
                {
                    b[j] = a[j];
                }
            }
            if (index != a.Length)
            {
                for (int j = index; j < b.Length; j++)
                {
                    b[j] = a[j + 1];
                }
            }

            return b;
        }
        public static double min(double[,] a, int dim)
        {
            double min = 1000;
            for (int point = 0; point < a.GetLength(0); point++)
            {
                if (a[point, dim] < min)
                {
                    min = a[point, dim];
                }
            }
            return min;
        }
        public static double max(double[,] a, int dim)
        {
            double max = -1000;
            for (int point = 0; point < a.GetLength(0); point++)
            {
                if (a[point, dim] > max)
                {
                    max = a[point, dim];
                }
            }
            return max;
        }

        public static void PlanChecker(StructureSet ss, PlanSetup p, Patient patient, string path)
        {
            //Export to a text file
            //make txt for meandoses
            string fileName = p.Id + "_PlanCheck.txt";

            //Get the current date and time:
            var culture = new CultureInfo("en-US");
            DateTime localDate = DateTime.Now;

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, fileName)))
            {
                outputFile.WriteLine("Treatment Plan Verification Report \n");
                outputFile.WriteLine("Date of Report: " + localDate.ToString(culture));
                outputFile.WriteLine("Patient:");
                outputFile.WriteLine("last name: " + patient.LastName.ToString());
                outputFile.WriteLine("last name: " + patient.FirstName.ToString());
                outputFile.WriteLine("\n \n");

                List<string> regionOutput = new List<string>();
                List<string> violated = new List<string>();   //hold names of violated regions

                double prescDose = p.TotalDose.Dose;
                foreach (Structure s in ss.Structures)
                {
                    string organName = s.Name;
                    if ((organName.ToLower().Contains("st")) && !(organName.ToLower().Contains("prv")))    //Brain stem
                    {
                        bool viol = false;
                        string organOut = "---------------------\n";
                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        var a = p.GetDoseAtVolume(s, 95, VolumePresentation.Relative, DoseValuePresentation.Absolute);
                        DoseValue fiftyGy = new DoseValue(5000, "cGy");
                        double V50 = p.GetVolumeAtDose(s, fiftyGy, VolumePresentation.Relative);
                        DoseValue fortyGy = new DoseValue(4000, "cGy");
                        double V40 = p.GetVolumeAtDose(s, fiftyGy, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((maxDose > 5000) || (V50 > 1) || (V40 > 10))
                        {
                            violated.Add(organName);
                            viol = true;
                        }
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";
                        organOut += "V50: " + string.Format("{0:0.00}", V50) + "% \n";
                        organOut += "V40: " + string.Format("{0:0.00}", V40) + "% \n";
                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                    }
                    else if ((organName.ToLower().Contains("st")) && (organName.ToLower().Contains("prv"))) //brain prv
                    {
                        bool viol = false;
                        string organOut = "---------------------\n";
                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        var a = p.GetDoseAtVolume(s, 95, VolumePresentation.Relative, DoseValuePresentation.Absolute);
                        DoseValue fiftyGy = new DoseValue(5000, "cGy");
                        double V50 = p.GetVolumeAtDose(s, fiftyGy, VolumePresentation.Relative);
                        DoseValue fortyGy = new DoseValue(4000, "cGy");
                        double V40 = p.GetVolumeAtDose(s, fiftyGy, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((maxDose > 5000) || (V50 > 1) || (V40 > 10))
                        {
                            violated.Add(organName);
                            viol = true;
                        }
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";
                        organOut += "V50: " + string.Format("{0:0.00}", V50) + "% \n";
                        organOut += "V40: " + string.Format("{0:0.00}", V40) + "% \n";
                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);

                    }
                    else if ((organName.ToLower().Contains("cord")) && !(organName.ToLower().Contains("prv")))
                    {
                        bool viol = false;
                        string organOut = "---------------------\n";
                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        var a = p.GetDoseAtVolume(s, 95, VolumePresentation.Relative, DoseValuePresentation.Absolute);
                        DoseValue fiftyGy = new DoseValue(5000, "cGy");
                        double V50 = p.GetVolumeAtDose(s, fiftyGy, VolumePresentation.Relative);
                        DoseValue fortyGy = new DoseValue(4000, "cGy");
                        double V40 = p.GetVolumeAtDose(s, fiftyGy, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((maxDose > 5000) || (V50 > 1) || (V40 > 10))
                        {
                            violated.Add(organName);
                            viol = true;
                        }
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";
                        organOut += "V50: " + string.Format("{0:0.00}", V50) + "% \n";
                        organOut += "V40: " + string.Format("{0:0.00}", V40) + "% \n";
                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("cord")) && (organName.ToLower().Contains("prv")))
                    {
                        bool viol = false;
                        string organOut = "---------------------\n";
                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        var a = p.GetDoseAtVolume(s, 95, VolumePresentation.Relative, DoseValuePresentation.Absolute);
                        DoseValue fiftyGy = new DoseValue(5000, "cGy");
                        double V50 = p.GetVolumeAtDose(s, fiftyGy, VolumePresentation.Relative);
                        DoseValue fortyGy = new DoseValue(4000, "cGy");
                        double V40 = p.GetVolumeAtDose(s, fiftyGy, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((maxDose > 5000) || (V50 > 1) || (V40 > 10))
                        {
                            violated.Add(organName);
                            viol = true;
                        }
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";
                        organOut += "V50: " + string.Format("{0:0.00}", V50) + "% \n";
                        organOut += "V40: " + string.Format("{0:0.00}", V40) + "% \n";
                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("ptv")))
                    {
                        //Get what type of PTV:
                        string typetemp = "";
                        for (int n = 0; n < organName.Length; n++)
                        {
                            if (Char.IsDigit(organName[n]))
                                typetemp += organName[n];
                        }
                        double type = Convert.ToDouble(typetemp + "00");
                        bool viol = false;
                        string organOut = "---------------------\n";
                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        DoseValue v98 = new DoseValue(type * 0.95, "cGy");
                        double V98 = p.GetVolumeAtDose(s, v98, VolumePresentation.Relative);

                        organOut += organName + "\n\n";

                        if ((maxDose > prescDose * 1.1) || (V98 < 0.98))
                        {
                            violated.Add(organName);
                            viol = true;
                        }
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";
                        organOut += "V98: " + string.Format("{0:0.00}", V98) + "% \n";
                        organOut += "Dmean: " + string.Format("{0:0.00}", dvh.MeanDose.Dose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("par")) && !(organName.ToLower().Contains("opt")) && !(organName.ToLower().Contains("l")))
                    {//Right parotid

                        bool viol = false;
                        string organOut = "---------------------\n";

                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        double meanDose = dvh.MeanDose.Dose;
                        DoseValue v20 = new DoseValue(2000, "cGy");
                        double V20 = p.GetVolumeAtDose(s, v20, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((meanDose > 2000))
                        {
                            violated.Add(organName);
                            viol = true;
                        }

                        organOut += "Dmean: " + string.Format("{0:0.00}", meanDose) + "cGy \n";
                        organOut += "V_20Gy: " + string.Format("{0:0.00}", V20) + "% \n";
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("par")) && (organName.ToLower().Contains("l")) && !(organName.ToLower().Contains("opt")))
                    { //Left parotid
                        bool viol = false;
                        string organOut = "---------------------\n";

                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        double meanDose = dvh.MeanDose.Dose;
                        DoseValue v20 = new DoseValue(2000, "cGy");
                        double V20 = p.GetVolumeAtDose(s, v20, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((meanDose > 2000))
                        {
                            violated.Add(organName);
                            viol = true;
                        }

                        organOut += "Dmean: " + string.Format("{0:0.00}", meanDose) + "cGy \n";
                        organOut += "V_20Gy: " + string.Format("{0:0.00}", V20) + "% \n";
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("par")) && (organName.ToLower().Contains("opt")) && !(organName.ToLower().Contains("l")))
                    {//Right opti parotid

                        bool viol = false;
                        string organOut = "---------------------\n";

                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        double meanDose = dvh.MeanDose.Dose;
                        DoseValue v20 = new DoseValue(2000, "cGy");
                        double V20 = p.GetVolumeAtDose(s, v20, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((meanDose > 2000))
                        {
                            violated.Add(organName);
                            viol = true;
                        }

                        organOut += "Dmean: " + string.Format("{0:0.00}", meanDose) + "cGy \n";
                        organOut += "V_20Gy: " + string.Format("{0:0.00}", V20) + "% \n";
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("par")) && (organName.ToLower().Contains("opt")))
                    { //left opti parotid
                        bool viol = false;
                        string organOut = "---------------------\n";

                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        double meanDose = dvh.MeanDose.Dose;
                        DoseValue v20 = new DoseValue(2000, "cGy");
                        double V20 = p.GetVolumeAtDose(s, v20, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((meanDose > 2000))
                        {
                            violated.Add(organName);
                            viol = true;
                        }

                        organOut += "Dmean: " + string.Format("{0:0.00}", meanDose) + "cGy \n";
                        organOut += "V_20Gy: " + string.Format("{0:0.00}", V20) + "% \n";
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("subm")) && (organName.ToLower().Contains("r")) && !(organName.ToLower().Contains("opt")))
                    {
                        bool viol = false;
                        string organOut = "---------------------\n";

                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        double meanDose = dvh.MeanDose.Dose;
                        DoseValue v20 = new DoseValue(2000, "cGy");
                        double V20 = p.GetVolumeAtDose(s, v20, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((meanDose > 2000))
                        {
                            violated.Add(organName);
                            viol = true;
                        }

                        organOut += "Dmean: " + string.Format("{0:0.00}", meanDose) + "cGy \n";
                        organOut += "V_20Gy: " + string.Format("{0:0.00}", V20) + "% \n";
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("subm")) && (organName.ToLower().Contains("l")) && !(organName.ToLower().Contains("opt")))
                    {
                        bool viol = false;
                        string organOut = "---------------------\n";

                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        double meanDose = dvh.MeanDose.Dose;
                        DoseValue v20 = new DoseValue(2000, "cGy");
                        double V20 = p.GetVolumeAtDose(s, v20, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((meanDose > 2000))
                        {
                            violated.Add(organName);
                            viol = true;
                        }

                        organOut += "Dmean: " + string.Format("{0:0.00}", meanDose) + "cGy \n";
                        organOut += "V_20Gy: " + string.Format("{0:0.00}", V20) + "% \n";
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("cav")) && (organName.ToLower().Contains("o")))
                    { //oral cavity
                        bool viol = false;
                        string organOut = "---------------------\n";

                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        double meanDose = dvh.MeanDose.Dose;
                        DoseValue v50 = new DoseValue(5000, "cGy");
                        double V50 = p.GetVolumeAtDose(s, v50, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((meanDose > 5000))
                        {
                            violated.Add(organName);
                            viol = true;
                        }

                        organOut += "Dmean: " + string.Format("{0:0.00}", meanDose) + "cGy \n";
                        organOut += "V_50Gy: " + string.Format("{0:0.00}", V50) + "% \n";
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                    else if ((organName.ToLower().Contains("lar")))
                    { //laryngopharynx
                        bool viol = false;
                        string organOut = "---------------------\n";

                        DVHData dvh = p.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.001);
                        double maxDose = dvh.MaxDose.Dose;
                        double meanDose = dvh.MeanDose.Dose;
                        DoseValue v45 = new DoseValue(4500, "cGy");
                        double V45 = p.GetVolumeAtDose(s, v45, VolumePresentation.Relative);
                        organOut += organName + "\n\n";

                        if ((meanDose > 4500))
                        {
                            violated.Add(organName);
                            viol = true;
                        }

                        organOut += "Dmean: " + string.Format("{0:0.00}", meanDose) + "cGy \n";
                        organOut += "V_45Gy: " + string.Format("{0:0.00}", V45) + "% \n";
                        organOut += "Dmax: " + string.Format("{0:0.00}", maxDose) + "cGy \n";

                        if (viol)
                        {
                            organOut += "VIOLATED";
                        }
                        else
                        {
                            organOut += "PASSED";
                        }
                        regionOutput.Add(organOut);
                    }
                }
                outputFile.WriteLine("Violated Structures: ");
                outputFile.WriteLine("---------------------");
                for (int i = 0; i < violated.Count; i++)
                {
                    outputFile.WriteLine(violated[i]);
                }
                outputFile.WriteLine("---------------------");
                for (int i = 0; i < regionOutput.Count; i++)
                {
                    outputFile.WriteLine(regionOutput[i]);
                }

            }
        }
    }
}
