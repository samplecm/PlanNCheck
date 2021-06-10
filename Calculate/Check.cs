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
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;




namespace Plan_n_Check.Calculate
{
    public static class Check
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

        public static void RunReport(ScriptContext context, HNPlan hnplan, string path, List<List<Structure>> matchingStructures, List<List<Structure>> optimizedStructures, List<List<string>> updateLog, List<Tuple<ROI, int, int, int, int>> DVH_ReportStructures)
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
                Tuple<ROI, int, int, int, int> dvhTuple = DVH_ReportStructures[0]; //Declare first so that the variable saves after loop
                for (int r = 0; r< DVH_ReportStructures.Count; r++)
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
                        string imageSaveLocation = path.Substring(0,indexPeriod) + roi.Name + "_" + d + ".png";
                        pngExporter.ExportToFile(pv.Model, imageSaveLocation);
                        //Now add this html:
                        outputFile += "<img src=\"" + imageSaveLocation + "/>";
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

        public static PlotView DVH_Maker(ScriptContext context, Structure structure, int dlb, int dub, int vlb, int vub) //dose lower bound, dose upperbound... etc
        {
            PlotView pv = new OxyPlot.WindowsForms.PlotView();
            pv.Location = new System.Drawing.Point(30, 60);
            pv.Size = new System.Drawing.Size(450, 300);
            pv.Model = new OxyPlot.PlotModel { Title = "DVH" };
            pv.Model.Axes.Add(new LinearAxis
            {
                Title = "Dose (cGy)",
                Position = AxisPosition.Bottom,
                Minimum = dlb,
                Maximum = dub
            } ) ;
            pv.Model.Axes.Add(new LinearAxis
            {
                Title = "Volume (cc)",
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
