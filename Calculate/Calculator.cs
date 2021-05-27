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
using TheArtOfDev.HtmlRenderer.PdfSharp;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;




namespace Plan_n_Check.Calculate
{
    public static class Calculator
    {       
        
        public static string CheckConstraints(ScriptContext context, ROI ROI, List<Structure> dicomStructure, bool isParotid = false)
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
                    string htmlSubscript = "&#60;";
                    
                    string relation = ROI.Constraints[i].EqualityType;
                    if (relation == ">")
                    {
                        htmlSubscript = "&#62;";
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
                                    }
                                    else
                                    {
                                        returnString += "Constraint FAILED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + " cGy<br>";
                                    }
                                }
                                else if (relation == ">")
                                {
                                    if (doseQuant > value)
                                    {
                                        returnString += "Constraint SATISFIED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + " cGy <br>";

                                    }
                                    else
                                    {
                                        returnString += "Constraint FAILED. The dose covering " + string.Format("{0:0.0}", (sub * 100 / volume)) + "% of the structure was " + string.Format("{0:00}", doseQuant) + " cGy<br>";
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
                                    }

                                }
                            }
                            else if (relation == ">")
                            {
                                if (dose > value)
                                {
                                    returnString += "Constraint SATISFIED. D = " + string.Format("{0:0.0}", dose) + "cGy <br>";

                                }
                                else
                                {
                                    returnString += "Constraint FAILED. D = " + string.Format("{0:0.0}", dose) + "cGy <br>";
                                }
                            }
                            else
                            {
                                returnString += "Could not understand the relation given in the constraint. <br>";
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
                                frac = FindPTVNumber(ROI.Name.ToLower()) * 100;
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
                                        returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub ) + "cGy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                    }
                                    else
                                    {
                                        returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                    }
                                }
                                else
                                {
                                    if (isPTV)
                                    {
                                        returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                    }
                                    else
                                    {
                                        returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub ) + "cGy was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                    }
                                }
                            }
                            else if (relation == ">")
                            {
                                if (volumeQuant > value)
                                {
                                    if (isPTV)
                                    {
                                        returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                    }
                                    else
                                    {
                                        returnString += "Constraint SATISFIED. The volume receiving at least " + string.Format("{0:0.0}", sub ) + "cGy was  " + string.Format("{0:0.0}", (volumeQuant )) + "% of the structure's total volume. <br>";
                                    }
                                }
                                else
                                {
                                    if (isPTV)
                                    {
                                        returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy in the PTV was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                    }
                                    else
                                    {
                                        returnString += "Constraint VIOLATED. The volume receiving at least " + string.Format("{0:0.0}", sub) + "cGy was " + string.Format("{0:0.0}", (volumeQuant)) + "% of the structure's total volume. <br>";
                                    }
                                }
                            }
                            else
                            {
                                returnString += "Could not understand the relation given in the constraint. <br>";
                            }
                        }
                        catch
                        {
                            returnString += "Failed to interpret subscript given for this constraint. <br>";
                        }
                    }
                }
            }
            returnString += "</p>";
            return returnString;
        }

        public static void RunReport(ScriptContext context, HNPlan hnplan, string path, List<List<Structure>> matchingStructures, List<List<Structure>> optimizedStructures, List<List<string>> updateLog, List<ROI> DVH_ReportStructures)
        {
            //Need to go one by one and check constraints. 

            //First need to check what DICOM structures correspond to which constraint structures.
            Patient patient = context.Patient;
            StructureSet ss = context.StructureSet;
            
            //Now need to check the constrains on these structures.
            List<string> ReportStrings = new List<string>(); //report for each constraint.
            for (int i = 0; i < hnplan.ROIs.Count; i++)
            {
                if (matchingStructures[i].Count > 0)
                {
                    string report = CheckConstraints(context, hnplan.ROIs[i], matchingStructures[i]);
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
            string outputFile = "<!DOCTYPE html> <html> <body>";
            outputFile += "<h1>Treatment Plan Verification Report </h1>";
            outputFile += "<h2>Plan n Check </h2>";
            outputFile += "<h3>Date of Report: " + localDate.ToString(culture) + "</h3>";
            
            outputFile += "<p><h4>Patient:</h4><br>";
            outputFile += "Last name: " + patient.LastName.ToString() + "<br>";
            outputFile += "First name: " + patient.FirstName.ToString() + "<br>";


            outputFile += "<br><br></p>";
            for (int i = 0; i < hnplan.ROIs.Count; i++)
            {
                ROI roi = hnplan.ROIs[i];

                outputFile += "<p><h4>Structure: " + roi.Name + "</h4>";
                if (matchingStructures[i].Count == 0)
                {
                    outputFile += "No matching dicom structures found. <br>";
                }
                else
                {
                    outputFile += "<h5>DICOM Structure(s): " + matchingStructures[i].Count + " matching</h5>";
               
                        for (int ms = 0; ms < matchingStructures[i].Count; ms++)
                    {
                        outputFile += matchingStructures[i][ms].Name + "<br>";
                    }
                
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
                foreach (ROI dvhROI in DVH_ReportStructures)
                {
                    if (roi.Name == dvhROI.Name)
                    {
                        includeDVH = true;
                        break;
                    }
                }
                if (includeDVH)
                {
                    for (int d = 0; d < matchingStructures[i].Count; d++)
                    {
                        PlotView pv = DVH_Maker(context, matchingStructures[i][d]);
                        //Now need to add this as an image to the report. 
                        
                        var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
                        int indexPeriod = path.LastIndexOf(".");
                        string imageSaveLocation = path.Substring(0,indexPeriod) + roi.Name + "_" + d + ".png";
                        pngExporter.ExportToFile(pv.Model, imageSaveLocation);
                        //Now add this html:
                        outputFile += "<img src=\"" + imageSaveLocation + " />";
                    }
                    
                }
                
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
            pdf.Save(path);
                
            
        }

        public static PlotView DVH_Maker(ScriptContext context, Structure structure)
        {
            PlotView pv = new OxyPlot.WindowsForms.PlotView();
            pv.Location = new System.Drawing.Point(30, 60);
            pv.Size = new System.Drawing.Size(550, 340);
            pv.Model = new OxyPlot.PlotModel { Title = "DVH" };
            pv.Model.Axes.Add(new LinearAxis
            {
                Title = "Dose (cGy)",
                Position = AxisPosition.Bottom
            }
                );
            pv.Model.Axes.Add(new LinearAxis
            {
                Title = "Volume (cc)",
                Position = AxisPosition.Left
            } );
            var dvh = CalculateDVH(context.PlanSetup, structure);
            var series = CreateDVHSeries(dvh);
            pv.Model.Series.Add(series);
            return pv;
        }
        public static DVHData CalculateDVH(PlanSetup plan, Structure structure)
        {
            return plan.GetDVHCumulativeData(structure, DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.01);
        }
        public static OxyPlot.Series.Series CreateDVHSeries(DVHData dvh)
        {
            var series = new LineSeries();
            var points = CreateDataPoints(dvh);
            series.Points.AddRange(points);
            return series;
        }

        public static List<DataPoint> CreateDataPoints(DVHData dvh)
        {
            var points = new List<DataPoint>();
            foreach (var dvhPoint in dvh.CurveData)
            {
                var point = CreateDataPoint(dvhPoint);
                points.Add(point);
            }
            return points;
        }
        public static DataPoint CreateDataPoint(DVHPoint dvhPoint)
        {
            return new DataPoint(dvhPoint.DoseValue.Dose, dvhPoint.Volume);
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
                    if ((closeness < closest)&&(LongestSubstring(structureName, name) >= 3)&&(AllowedToMatch(structureName, name))) //must be closest, and also have a substring of at least 3 in common with other. 
                    {
                        dicomStructures.Clear();
                        dicomStructures.Add(structure);
                    }else if (closeness == closest)  //add it to the list if its equal in closeness to other. 
                    {
                        dicomStructures.Add(structure);
                    }                  

                }

            }
            else //It is a PTV:
            {
                //Get the type of PTV 
                int PTV_Type = FindPTVNumber(name);
                if (PTV_Type == 111) //if the ptv has no number with it in the name, then add every ptv to the list. 
                {
                    foreach (Structure structure in ss.Structures)
                    {

                        if (structure.Name.ToLower().Contains("ptv"))
                        {
                            dicomStructures.Add(structure);
                        }

                }
                }
                else  //if PTV has a number
                {
                    foreach (Structure structure in ss.Structures)
                    {

                        if ((structure.Name.ToLower().Contains("ptv"))&& (structure.Name.ToLower().Contains(PTV_Type.ToString())))
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
            for (int i =0; i < keywords.Count; i++)
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
            if ((s1.ToLower().Contains("left"))|| (s2.ToLower().Contains("left")))
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
        public static void DVHPlot(Structure structure, ScriptContext scriptContext)
        {
            var model = new OxyPlot.PlotModel { Title = $"DVH Plot" };
        }
    }
}
