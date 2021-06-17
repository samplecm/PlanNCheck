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
using Plan_n_Check.Features;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TheArtOfDev.HtmlRenderer.Core;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;




namespace Plan_n_Check.Calculate
{
    public static class Check
    {       
        
        public static string CheckConstraints_Report(ScriptContext context, ROI ROI, List<Structure> dicomStructure, bool isParotid = false)
            //This returns the HTML string for the full check report, as well as a list of boolean values corresponding to whether or not constraints were met. The list will be in the same order as the list of dicom structures supplied. 
        {

            string returnString = "<p>";
            //Go through all plan types and check all constraints for report. 
            PlanSetup p = context.PlanSetup;
            p.DoseValuePresentation = DoseValuePresentation.Absolute;
            
            
            double prescriptionDose = p.TotalDose.Dose;
            if (p.TotalDose.Unit == DoseValue.DoseUnit.Gy)
            {
                prescriptionDose *= 100; //Convert to Gy
                
            }
            for (int match = 0; match < dicomStructure.Count; match++)
            {
                returnString += "<h6>Matching Structure " + string.Format("{0}", match + 1) + ": </h6>";
                //first go one by one through the constraints.
                for (int i = 0; i < ROI.Constraints.Count; i++)
                {                  
                    //Get DVH data for structure: 
                    DVHData dvhData = p.GetDVHCumulativeData(dicomStructure[match], DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.01);
              
                    //Get type of constraint
                    string type = ROI.Constraints[i].Type;
                    string subscript = ROI.Constraints[i].Subscript;
                    string htmlSubscript = "&#60;"; // The < sign
                    
                    string relation = ROI.Constraints[i].EqualityType;
                    if (relation == ">")
                    {
                        htmlSubscript = "&#62;"; //The > sign
                    }
                    double value = ROI.Constraints[i].Value;
                    string format = ROI.Constraints[i].Format;
                    returnString += "Constraint " + string.Format("{0}", i + 1) + ": " + type + "<sub>" +  subscript.ToString() + "</sub>" + htmlSubscript + value.ToString();
                    if (type.ToLower() == "d")
                    {
                        if (format.ToLower() == "abs")
                        {
                            returnString += "cGy <br>";
                        }
                        else
                        {
                            returnString += "% <br>";
                        }
                    }else
                    {
                        if (format.ToLower() == "abs")
                        {
                            returnString += "cc <br>";
                        }
                        else
                        {
                            returnString += "% <br>";
                        }
                    }
                    

                    if (type == "D")//now subscript can be mean, max, min, median? 
                    {
                        try
                        {
                            double sub = Convert.ToInt32(subscript); //need to analyze DVH data if a number.
                            VolumePresentation vp = VolumePresentation.AbsoluteCm3;
                            DoseValuePresentation dp = new DoseValuePresentation();
                            dp = DoseValuePresentation.Absolute;
                            double volume = dicomStructure[match].Volume;
                            if (format.ToLower() == "rel")
                            {
                                sub = volume * sub / 100;
                                value = value * prescriptionDose / 100;

                                double doseQuant = p.GetDoseAtVolume(dicomStructure[match], sub, vp, dp).Dose;
                                //if (p.GetDoseAtVolume(dicomStructure[match], sub, vp, dp).Unit == DoseValue.DoseUnit.cGy) //convert to gy if necessary
                                //{
                                //    doseQuant /= 100;
                                //}
                                //now check the inequality: 
                                if (relation == "<")
                                {
                                    if (doseQuant < value)
                                    {
                                        returnString += "Constraint SATISFIED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + " cGy <br>";
                                        ROI.Constraints[i].Status.Add(true);
                                    }
                                    else
                                    {
                                        returnString += "Constraint FAILED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + " cGy<br>";
                                        ROI.Constraints[i].Status.Add(false);
                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (doseQuant > value)
                                    {
                                        returnString += "Constraint SATISFIED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + " cGy <br>";
                                        ROI.Constraints[i].Status.Add(true);

                                    }
                                    else
                                    {
                                        returnString += "Constraint FAILED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + " cGy<br>";
                                        ROI.Constraints[i].Status.Add(false);
                                    }
                                }
                                else
                                {
                                    returnString += "Could not understand the relation given in the constraint. \n";
                                    ROI.Constraints[i].Status.Add(null);
                                }
                            }
                        }
                        catch //mean median...
                        {
                            double dose;
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
                                returnString += "Failed to interpret subscript given for this constraint. <br>";
                                ROI.Constraints[i].Status.Add(null);
                                break;
                            }
                            if (dvhData.MeanDose.Unit == DoseValue.DoseUnit.Gy) //convert to cgy if necessary
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
                                    returnString += "Constraint SATISFIED. D = " + string.Format("{0:0.0}", dose) + "cGy <br>";
                                    ROI.Constraints[i].Status.Add(true);
                                }
                                
                                else
                                {
                                    if ((ROI.Name.ToLower().Contains("paro"))&&(dose < 2500))
                                    {
                                        returnString += "D = " + string.Format("{0:0.0}", dose) + "cGy. Possibly acceptable. <br>";
                                    }
                                    else
                                    {
                                        returnString += "Constraint FAILED. D = " + string.Format("{0:0.0}", dose) + "cGy <br>";
                                        ROI.Constraints[i].Status.Add(false);
                                    }

                                }
                            }
                            else if (relation == ">")
                            {
                                if (dose > value)
                                {
                                    returnString += "Constraint SATISFIED. D = " + string.Format("{0:0.0}", dose) + "cGy <br>";
                                    ROI.Constraints[i].Status.Add(true);

                                }
                                else
                                {
                                    returnString += "Constraint FAILED. D = " + string.Format("{0:0.0}", dose) + "cGy <br>";
                                    ROI.Constraints[i].Status.Add(false);
                                }
                            }
                            else
                            {
                                returnString += "Could not understand the relation given in the constraint. <br>";
                                ROI.Constraints[i].Status.Add(null);
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
                            double volume = dicomStructure[match].Volume;
                            bool isPTV = false; 
                          
                            if (format.ToLower() == "abs")
                            {                              
                                value = (value / volume) * 100;
                                sub = Convert.ToDouble(subscript);
                            }
                            //But if PTV, then this is not relative to the prescription dose.
                            if (ROI.Name.ToLower().Contains("ptv"))
                            {
                                isPTV = true;
                                frac = StringOperations.FindPTVNumber(ROI.Name.ToLower()) * 100;
                                sub = Convert.ToDouble(subscript)*frac / 100;
                                vp = VolumePresentation.Relative;                    
                            }
                            
                            DoseValue dose = new DoseValue(sub, "cGy");
                            //Need dose in cGy for calculation:
                            double volumeQuant = p.GetVolumeAtDose(dicomStructure[match], dose, vp);
                            //Now check the inequality: 
                            if (relation == "<")
                            {
                                if (volumeQuant < value)
                                {
                                    if (isPTV)
                                    {
                                        returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub ) + 
                                            "cGy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                        ROI.Constraints[i].Status.Add(true);
                                    }
                                    else
                                    {
                                        returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy was " + 
                                            string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                        ROI.Constraints[i].Status.Add(true);
                                    }
                                }
                                else
                                {
                                    if (isPTV)
                                    {
                                        returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy in the PTV was " + 
                                            string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                        ROI.Constraints[i].Status.Add(false);
                                    }
                                    else
                                    {
                                        returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub ) + "cGy was " + 
                                            string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                        ROI.Constraints[i].Status.Add(false);
                                    }
                                }
                            }
                            else if (relation == ">")
                            {
                                if (volumeQuant > value)
                                {
                                    if (isPTV)
                                    {
                                        returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy in the PTV was " + 
                                            string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                        ROI.Constraints[i].Status.Add(true);
                                    }
                                    else
                                    {
                                        returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub ) + "cGy was  " + 
                                            string.Format("{0:0.0}", (volumeQuant )) + "% of the structure's total volume. <br>";
                                        ROI.Constraints[i].Status.Add(true);
                                    }
                                }
                                else
                                {
                                    if (isPTV)
                                    {
                                        returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy in the PTV was " + 
                                            string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                        ROI.Constraints[i].Status.Add(false);
                                    }
                                    else
                                    {
                                        returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy was " + 
                                            string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                        ROI.Constraints[i].Status.Add(false);
                                    }
                                }
                            }
                            else
                            {
                                returnString += "Could not understand the relation given in the constraint. <br>";
                                ROI.Constraints[i].Status.Add(null);
                            }
                        }
                        catch
                        {
                            returnString += "Failed to interpret subscript given for this constraint. <br>";
                            ROI.Constraints[i].Status.Add(null);
                        }
                    }
                }
            }
            returnString += "</p>";
            return returnString;
        }

        public static Tuple<bool, List<List<double>>> CheckConstraints(ScriptContext context, ROI ROI, List<Structure> dicomStructure)
        //This returns the HTML string for the full check report, as well as a list of boolean values corresponding to whether or not constraints were met. The list will be in the same order as the list of dicom structures supplied. 
        {
            bool passed = true; //if any constraints are failed this will be changed to false
            List<List<double>> constraintValues = new List<List<double>>(); //list is for each matching structure, then each constraint

            //Go through all plan types and check all constraints for report. 
            PlanSetup p = context.PlanSetup;
            p.DoseValuePresentation = DoseValuePresentation.Absolute;


            double prescriptionDose = p.TotalDose.Dose;
            if (p.TotalDose.Unit == DoseValue.DoseUnit.Gy)
            {
                prescriptionDose *= 100; //Convert to Gy

            }
            for (int match = 0; match < dicomStructure.Count; match++)
            {
                constraintValues.Add(new List<double>());
                //first go one by one through the constraints.
                for (int i = 0; i < ROI.Constraints.Count; i++)
                {
                    //Get DVH data for structure: 
                    DVHData dvhData = p.GetDVHCumulativeData(dicomStructure[match], DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.01);

                    //Get type of constraint
                    string type = ROI.Constraints[i].Type;
                    string subscript = ROI.Constraints[i].Subscript;
                    string relation = ROI.Constraints[i].EqualityType;
                    double value = ROI.Constraints[i].Value;
                    string format = ROI.Constraints[i].Format;                  

                    if (type == "D")//now subscript can be mean, max, min, median? 
                    {
                        try
                        {
                            double sub = Convert.ToInt32(subscript); //need to analyze DVH data if a number.
                            VolumePresentation vp = VolumePresentation.AbsoluteCm3;
                            DoseValuePresentation dp = new DoseValuePresentation();
                            dp = DoseValuePresentation.Absolute;
                            double volume = dicomStructure[match].Volume;
                            if (format.ToLower() == "rel")
                            {
                                sub = volume * sub / 100;
                                value = value * prescriptionDose / 100;

                                double doseQuant = p.GetDoseAtVolume(dicomStructure[match], sub, vp, dp).Dose;
                                //if (p.GetDoseAtVolume(dicomStructure[match], sub, vp, dp).Unit == DoseValue.DoseUnit.cGy) //convert to gy if necessary
                                //{
                                //    doseQuant /= 100;
                                //}
                                //now check the inequality: 
                                if (relation == "<")
                                {
                                    if (doseQuant < value)
                                    {

                                        constraintValues[constraintValues.Count - 1].Add(doseQuant);
                                    }
                                    else
                                    {
                                        passed = false;
                                        constraintValues[constraintValues.Count - 1].Add(doseQuant);
                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (doseQuant > value)
                                    {
                                        constraintValues[constraintValues.Count - 1].Add(doseQuant);
                                    }
                                    else
                                    {

                                        constraintValues[constraintValues.Count - 1].Add(doseQuant);
                                        passed = false;
                                    }
                                }
                                else //constraint not understood
                                {

                                    constraintValues[constraintValues.Count - 1].Add(0);
                                    passed = false;
                                }
                            }
                        }
                        catch //mean median...
                        {
                            double dose;
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
                            else //not understood
                            {
                                constraintValues[constraintValues.Count - 1].Add(0);
                                passed = false;
                                break;
                            }
                            if (dvhData.MeanDose.Unit == DoseValue.DoseUnit.Gy) //convert to cgy if necessary
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
                                    constraintValues[constraintValues.Count-1].Add(dose);
                                }

                                else
                                {

                                    constraintValues[constraintValues.Count - 1].Add(dose);
                                    passed = false;

                                }
                            }
                            else if (relation == ">")
                            {
                                if (dose > value)
                                {

                                    constraintValues[constraintValues.Count - 1].Add(dose);

                                }
                                else
                                {
                                    constraintValues[constraintValues.Count - 1].Add(dose);
                                    passed = false;
                                }
                            }
                            else //not understood
                            {
                                constraintValues[constraintValues.Count - 1].Add(0);
                                passed = false;
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
                            double volume = dicomStructure[match].Volume;
     
          
                            if (format.ToLower() == "abs")
                            {
                                value = (value / volume) * 100;
                                sub = Convert.ToDouble(subscript);
                            }
                            //But if PTV, then this is not relative to the prescription dose.
                            if (ROI.Name.ToLower().Contains("ptv"))
                            {

                                frac = StringOperations.FindPTVNumber(ROI.Name.ToLower()) * 100;
                                sub = Convert.ToDouble(subscript) * frac / 100;
                                vp = VolumePresentation.Relative;
                            }

                            DoseValue dose = new DoseValue(sub, "cGy");
                            //Need dose in cGy for calculation:
                            double volumeQuant = p.GetVolumeAtDose(dicomStructure[match], dose, vp);
                            //Now check the inequality: 
                            if (relation == "<")
                            {
                                if (volumeQuant < value)
                                {

                                      constraintValues[constraintValues.Count - 1].Add(volumeQuant);


                                }
                                else
                                {
                                    constraintValues[constraintValues.Count - 1].Add(volumeQuant);
                                    passed = false;
                                }
                            }
                            else if (relation == ">")
                            {
                                if (volumeQuant > value)
                                {
                                    constraintValues[constraintValues.Count - 1].Add(volumeQuant);
                                }
                                else
                                {
                                    constraintValues[constraintValues.Count - 1].Add(volumeQuant);
                                }
                            }
                            else //not understood
                            {
                                constraintValues[constraintValues.Count - 1].Add(0);
                                passed = false;
                            }
                        }
                        catch //not understood 
                        {
                            constraintValues[constraintValues.Count - 1].Add(0);
                            passed = false;
                        }
                    }
                }
            }
            return Tuple.Create(passed, constraintValues);
        }

        public static Tuple<bool, List<bool>, List<List<List<double>>>> EvaluatePlan(ScriptContext context, HNPlan plan, List<List<Structure>> matchingStructures, List<List<Structure>> optimizedStructures)
        {
            //This function iterates over all the constraints and checks if they are met or not. It saves all the doses/volumes corresponding to each constraint in List<List<double>> constraintValues. Each ROI must have each constraint satisfied for each
            //matching structure for the ROI to be "passed". The pass/fail status of each ROI is saved in list<bool> ROI_results. Then the final bool Passed is true if all constraints are met when at least one matching structure exists. Saliva glands
            //must be grouped to see whether they pass as a whole or not. 
            List<bool> ROI_results = new List<bool>();
            List<List<List<double>>> constraintValues = new List<List<List<double>>>();
            for (int i = 0; i < plan.ROIs.Count; i++)
            {
                if (matchingStructures[i].Count > 0)
                {

                    Tuple<bool, List<List<double>>> checkData = CheckConstraints(context, plan.ROIs[i], optimizedStructures[i]);
                    ROI_results.Add(checkData.Item1);
                    constraintValues.Add(checkData.Item2);

                }
                else
                {
                    ROI_results.Add(true);
                    constraintValues.Add(new List<List<double>>());
                }
            }
            //Now need to check if the plan passed or not: 
            bool planPassed = VerifyPlan(ROI_results, constraintValues);
            return Tuple.Create(planPassed, ROI_results, constraintValues);
        }
        public static bool VerifyPlan(List<bool> ROI_Results, List<List<List<double>>> constraintValues)
        {
            //Every constraint besides saliva glands must be passed:
            for (int i =0; i < 16; i++)
            {
                if (ROI_Results[i] == false)
                {
                    return false; //plan failed
                }
            }
            for (int i = 10; i < ROI_Results.Count; i++)
            {
                if (ROI_Results[i] == false)
                {
                    return false; //fail plan
                }
            }
            //For the saliva glands, the plan passes if one parotid is below 20Gy or both are below 25Gy.
            if ((constraintValues[16][0][0] > 2500) && (constraintValues[17][0][0] > 2500)) //index 16, 17 is right, left parotid
            {
                return false; //fail plan
            }else if (!(constraintValues[16][0][0] < 2000) || (constraintValues[17][0][0] > 2500)) //index 16, 17 is right, left parotid
            {
                return false; //fail plan
            }else if ((constraintValues[16][0][0] > 2500) || !(constraintValues[17][0][0] < 2000)) //index 16, 17 is right, left parotid
            {
                return false; //fail plan
            }

            return true;
        }

        public static void RunReport(ScriptContext context, HNPlan hnplan, string path, List<List<Structure>> matchingStructures, List<List<Structure>> optimizedStructures, List<List<string>> updateLog, List<Tuple<ROI, int, int, int, int>> DVH_ReportStructures)
        {
            //Need to go one by one and check constraints. 

           
            Patient patient = context.Patient;
            StructureSet ss = context.StructureSet;
            
            //Now need to check the constraints on these structures.
            List<string> ReportStrings = new List<string>(); //report for each constraint.           
            for (int i = 0; i < hnplan.ROIs.Count; i++)
            {
                if (matchingStructures[i].Count > 0)
                {

                    string report = CheckConstraints_Report(context, hnplan.ROIs[i], matchingStructures[i]);
                    ReportStrings.Add(report);
                }
                else
                {
                    ReportStrings.Add("");
                    
                }
                
            }
            //Get the current date and time:
            var culture = new CultureInfo("en-US");
            DateTime localDate = DateTime.Now;

            //Get html code for green, red checkmarks:
            string greenCheck = @"<div style=""color:Green;""><p>Passed   </p></div>";
            string redCheck = @"<div style=""color:Red;""><p>Failed   </p></div>";
            string questionMark = @"<div style=""color:Orange;""><p>Constraint Issue   </p></div>";

            string outputFile = "<!DOCTYPE html> <html> <body>";

            //add table style
            outputFile += "<style>";
            outputFile += "table {font-family: arial, sans-serif; border-collapse: collapse; width: 100%; page-break-inside: avoid;}";
            outputFile += "td, th { border: 1px solid #dddddd; text-align: left; passing: 8px; page-break-inside: avoid;}";
            outputFile += "tr:nth-child(even) {background-color: #dddddd; page-break-inside: avoid;}";
            outputFile += "h2 {page-break-inside: avoid;}";
            outputFile += "h3 {page-break-inside: avoid;}";
            outputFile += "h4 {page-break-inside: avoid;}";
            outputFile += "hr {page-break-inside: avoid;}";
            outputFile += "p {page-break-inside: avoid;}";
            outputFile += "</style>";



            outputFile += "<h2>Treatment Plan Verification Report </h2>";        
            outputFile += "<h2>Plan n Check </h2>";
            outputFile += "<hr>";
            outputFile += "<h3>Date of Report: " + localDate.ToString(culture) + "</h3>";
            
            outputFile += "<p><h4>Patient:</h4>";
            outputFile += "Last name: " + patient.LastName.ToString() + "<br>";
            outputFile += "First name: " + patient.FirstName.ToString() + "<br>";
            outputFile += "<hr>";
             
            //Now have a checkbox summary table for constraints: 
            outputFile += "<table>";
            outputFile += "<tr>";//Headers
            outputFile += "<th>Region of Interest</th>";
            outputFile += "<th>DICOM Structure</th>";
            outputFile += "<th>Constraints Status</th>";
            outputFile += "</tr>";
            //Now add a row for each ROI:
            for (int i =0; i < hnplan.ROIs.Count; i++)
            {
                outputFile += "<tr>";
                outputFile += "<td>" + hnplan.ROIs[i].Name + "</td>";
                int numMatches = matchingStructures[i].Count;
                int constraint_idx = 0;
                
                if (matchingStructures[i].Count == 0)
                {
                    outputFile += "<td>No matching DICOM structures</td>";
                    outputFile += "<td> </td>";
                    outputFile += "</tr>";
                }
                else
                {
                    for (int d = 0; d < matchingStructures[i].Count; d++)
                    {
                        outputFile += "<td>" + matchingStructures[i][d].Name + "</td>";
                        outputFile += "<td>";
                        for (int c = constraint_idx; c < hnplan.ROIs[i].Constraints.Count; c++)
                        {
                            if (hnplan.ROIs[i].Constraints[c].Status[d] == true)
                            {
                                outputFile += greenCheck;
                            }
                            else if (hnplan.ROIs[i].Constraints[c].Status[d] == false)
                            {
                                outputFile += redCheck;
                            }
                            else
                            {
                                outputFile += questionMark;
                            }
                            constraint_idx = c;
                        }
                        
                        
                        outputFile += "</td>";
                        outputFile += "</tr>";
                        //Now need to add a blank first column to extend table for another matching structure if there is another match.
                        if (matchingStructures[i].Count - d > 0)
                        {
                            outputFile += "<tr><td> </td>";
                        }
                        
                    }
                    outputFile += "</tr>";
                }

            }


            outputFile += "</table>";


            outputFile += "<br><br></p>";
            for (int i = 0; i < hnplan.ROIs.Count; i++)
            {
                ROI roi = hnplan.ROIs[i];

                outputFile += "<hr><p><h4>" + roi.Name + "</h4>";
                if (matchingStructures[i].Count == 0)
                {
                    outputFile += "No matching dicom structures found. <br>";
                }
                else
                {
                    outputFile += "<h5>DICOM Structure(s): " + matchingStructures[i].Count + " matching</h5>";
               
                    //    for (int ms = 0; ms < matchingStructures[i].Count; ms++)
                    //{
                    //    outputFile += matchingStructures[i][ms].Name + "<br>";
                    //}
                
                for (int j = 0; j < matchingStructures[i].Count; j++)
                    {
                        if (matchingStructures[i][j].Name != optimizedStructures[i][j].Name)
                        {
                            outputFile += "Optimization Structure " + optimizedStructures[i][j].Name + " created for " + matchingStructures[i][j].Name + "<br>";
                        }
                    }
                outputFile += ReportStrings[i];
                }
                //Now need to add the DVH plot if wanted:
                bool includeDVH = false;
                try
                {
                    Tuple<ROI, int, int, int, int> dvhTuple = DVH_ReportStructures[0]; //Declare first so that the variable saves after loop
                    for (int r = 0; r < DVH_ReportStructures.Count; r++)
                    {
                        dvhTuple = DVH_ReportStructures[r];
                        if (roi.Name == dvhTuple.Item1.Name)
                        {
                            includeDVH = true;
                            break;
                        }
                    }
                    if (includeDVH)
                    {
                        for (int d = 0; d < matchingStructures[i].Count; d++)
                        {
                            PlotView pv = DVH_Maker(context, matchingStructures[i][d], dvhTuple.Item2, dvhTuple.Item3, dvhTuple.Item4, dvhTuple.Item5);
                            //Now need to add this as an image to the report. 

                            var pngExporter = new PngExporter { Width = 450, Height = 300, Background = OxyColors.White };
                            int indexPeriod = path.LastIndexOf(".");
                            string imageSaveLocation = path.Substring(0, indexPeriod) + roi.Name + "_" + d + ".png";
                            pngExporter.ExportToFile(pv.Model, imageSaveLocation);
                            //Now add this html:
                            outputFile += "<img src=\"" + imageSaveLocation + "/>";
                        }

                    }
                }
                catch { }
                
                
                outputFile += "</p>";
            }
           

            if (updateLog.Count != 0)
            {
                outputFile += "<h3>Update Log</h3>";

                int iteration = 1;
                foreach (List<string> log in updateLog)
                {
                    outputFile += "<h4>Iteration " + iteration.ToString() + " of constraint updates:</h4>";
                    iteration++;
                    outputFile += "<p>";
                    foreach (string constraintLog in log)
                    {
                        outputFile += constraintLog + "<br>";

                    }
                    outputFile += "</p>";

                }
                outputFile += "</body></html>";
            }
            

            //Now convert this html to a pdf
           
            
            var pdf = PdfGenerator.GeneratePdf(outputFile, PdfSharp.PageSize.Letter);
            try
            {
                pdf.Save(path);
            }
            catch
            {
                System.Windows.MessageBox.Show("Failed to save report");
            }
            
                
            
        }
        //public static PdfSharp.Pdf.PdfDocument GetPDF(string html, PdfSharp.PageSize pageSize)
        //{
        //    var config = new PdfGenerateConfig();
        //    config.PageSize = pageSize;
        //    config.SetMargins(20);
        //    var document = new PdfSharp.Pdf.PdfDocument();
        //    document.Info.Author = "Plan n Check";
            

        //    PdfSharp.Drawing.XSize orgPageSize;
        //    if (config.PageSize != PdfSharp.PageSize.Undefined)
        //        orgPageSize = PdfSharp.PageSizeConverter.ToSize(config.PageSize);
        //    else
        //        orgPageSize = config.ManualPageSize;
        //    var finalSize = new PdfSharp.Drawing.XSize(orgPageSize.Width - config.MarginLeft - config.MarginRight, orgPageSize.Height - config.MarginTop - config.MarginBottom);
        //    if (!string.IsNullOrEmpty(html))
        //    {
        //        using (var container = new HtmlContainer())
        //        {
        //            container.Location = new PdfSharp.Drawing.XPoint(config.MarginLeft, config.MarginTop);
        //            container.MaxSize = new PdfSharp.Drawing.XSize(finalSize.Width, 0);
        //            container.SetHtml(html, null);
                    
        //            //container.PageSize = finalSize;
        //            //container.MarginBottom = config.MarginBottom;
        //            //container.MarginLeft = config.MarginLeft;
        //            //container.MarginRight = config.MarginRight;
        //            //container.MarginTop = config.MarginTop;

        //            // layout the HTML with the page width restriction to know how many pages are required
        //            using (var measure = PdfSharp.Drawing.XGraphics.CreateMeasureContext(finalSize, PdfSharp.Drawing.XGraphicsUnit.Point, PdfSharp.Drawing.XPageDirection.Downwards))
        //            {
        //                container.PerformLayout(measure);
        //            }

        //            // while there is un-rendered HTML, create another PDF page and render with proper offset for the next page
        //            double scrollOffset = 0;
        //            while (scrollOffset > -container.ActualSize.Height)
        //            {
        //                var page = document.AddPage();
        //                page.Height = orgPageSize.Height;
        //                page.Width = orgPageSize.Width;
                        

        //                using (var g = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
        //                {
        //                    //g.IntersectClip(new XRect(config.MarginLeft, config.MarginTop, pageSize.Width, pageSize.Height));
        //                    g.IntersectClip(new PdfSharp.Drawing.XRect(0, 0, page.Width, page.Height));

        //                    container.ScrollOffset = new PdfSharp.Drawing.XPoint(0, scrollOffset);
        //                    container.PerformPaint(g);
        //                }
        //                scrollOffset -= finalSize.Height;
        //            }

        //            // add web links and anchors
        //            HandleLinks(document, container, orgPageSize, finalSize);
        //        }
        //    }

        //        return document;
        //}
        //private static void HandleLinks(PdfSharp.Pdf.PdfDocument document, HtmlContainer container, PdfSharp.Drawing.XSize orgPageSize, PdfSharp.Drawing.XSize pageSize)
        //{
        //    foreach (var link in container.GetLinks())
        //    {
        //        int i = (int)(link.Rectangle.Top / pageSize.Height);
        //        for (; i < document.Pages.Count && pageSize.Height * i < link.Rectangle.Bottom; i++)
        //        {
        //            var offset = pageSize.Height * i;

        //            // fucking position is from the bottom of the page
        //            var xRect = new PdfSharp.Drawing.XRect(link.Rectangle.Left, orgPageSize.Height - (link.Rectangle.Height + link.Rectangle.Top - offset), link.Rectangle.Width, link.Rectangle.Height);

        //            if (link.IsAnchor)
        //            {
        //                // create link to another page in the document
        //                var anchorRect = container.GetElementRectangle(link.AnchorId);
        //                if (anchorRect.HasValue)
        //                {
        //                    // document links to the same page as the link is not allowed
        //                    int anchorPageIdx = (int)(anchorRect.Value.Top / pageSize.Height);
        //                    if (i != anchorPageIdx)
        //                        document.Pages[i].AddDocumentLink(new PdfSharp.Pdf.PdfRectangle(xRect), anchorPageIdx);
        //                }
        //            }
        //            else
        //            {
        //                // create link to URL
        //                document.Pages[i].AddWebLink(new PdfSharp.Pdf.PdfRectangle(xRect), link.Href);
        //            }
        //        }
        //    }
        //}

        public static PlotView DVH_Maker(ScriptContext context, Structure structure, int dlb, int dub, int vlb, int vub) //dose lower bound, dose upperbound... etc
        {
            PlotView pv = new OxyPlot.WindowsForms.PlotView();
            pv.Location = new System.Drawing.Point(30, 60);
            pv.Size = new System.Drawing.Size(450, 300);
            pv.Model = new OxyPlot.PlotModel { Title = "DVH" };
            pv.Model.Axes.Add(new LinearAxis
            {
                Title = "Dose (%)",
                Position = AxisPosition.Bottom,
                Minimum = dlb,
                Maximum = dub
            } ) ;
            pv.Model.Axes.Add(new LinearAxis
            {
                Title = "Volume (%)",
                Position = AxisPosition.Left,
                Minimum = vlb,
                Maximum = vub
            } );
            var dvh = DVHMaker.CalculateDVH(context.PlanSetup, structure);
            var series = DVHMaker.CreateDVHSeries(dvh);
            pv.Model.Series.Add(series);
            return pv;
        }
       
        
        public static Tuple<double, int> RatioOverlapWithPTV(List<double[,]> contours, StructureSet ss)
        {
            //returns a tuple with the overlap ratio and also the PTV dose in Gy found to overlap with it (not necessarily the only one that overlaps though)
            //right now consider 3 or more points for a given subsegment inside a ptv means it is overlapping.
            double x, y, z;
            VVector point;
            int overlapNum = 0;
            int totalPoints = 0;
            int ptvDose = 0;

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
                    totalPoints++;
                    point = new VVector(x, y, z);

                    for (int ptv = 0; ptv < ptvs.Count; ptv++)
                    {
                        if (ptvs[ptv].IsPointInsideSegment(point))
                        {
                            overlapNum++;
                            if (overlapNum < 2)
                            { //only do once
                                ptvDose = StringOperations.FindPTVNumber(ptvs[ptv].Name) * 100; //Just taking the first ptv found to overlap with it
                            }

                            break;
                        }
                    }

                }
            }
            double overlapRatio = (double)overlapNum / (double)totalPoints;

            return Tuple.Create(overlapRatio, ptvDose);
        }
    }
}
