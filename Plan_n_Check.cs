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
using Plan_n_Check.Optimization;
using Plan_n_Check.Calculate;
using Plan_n_Check.Features;
using TheArtOfDev.HtmlRenderer.PdfSharp;
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
    public void Execute(ScriptContext context)
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
            
            PlanSetup ps = context.PlanSetup;
            Patient p = context.Patient;
            p.BeginModifications();
            StructureSet ss = context.StructureSet;
            foreach (Structure structure in ss.Structures.ToList())
            {
                if (structure.Name.ToLower().Contains("subsegment"))
                {
                    ss.RemoveStructure(structure);
                }
            }
            MainForm mainForm = new MainForm(ref context);
            System.Windows.Forms.Application.Run(mainForm);
            //System.Windows.System.Windows.MessageBox.Show("Hello", "input", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information, System.Windows.MessageBoxResult.OK);
    }
        public static Tuple<List<List<Structure>>, List<List<Structure>>, List<List<string>>> StartOptimizer(ScriptContext context, HNPlan hnPlan, List<List<Structure>> matchingStructures, int numIterations, List<Tuple<bool, double[], string>> features) //Returns list of matching structures
        {
         
            // Check for patient plan loaded
            ExternalPlanSetup plan = context.ExternalPlanSetup;
            

            Patient patient = context.Patient;
            StructureSet ss = context.StructureSet;
            Course course = context.Course;
            Image image3d = context.Image;


            ExternalBeamMachineParameters ebmp = new ExternalBeamMachineParameters("VaUnit6TB", "6X", 600, "ARC", null);
           
            //Create two VMAT beams
            BeamMaker(ref plan, ss, plan.TotalDose.Dose);
            plan.SetPrescription(35, new DoseValue(200, "cGy"), 1);

            
            //matchingStructures is the same length as hnPlan.ROIs.count
            //Now set optimization constraints
            List<List<Structure>> optimizedStructures = OptObjectivesEditing.SetConstraints(ref plan, hnPlan, matchingStructures, true); //true to check for opti structures, returns new matching list of structures
            List<List<double[,]>> choppedContours;
            List<double[]> planes;
            string contraParName;
              
            if (features[0].Item1 == true)
            {
                Tuple<List<List<double[,]>>, string, List<double[]>>  choppedAndName = ParotidChop(ref plan, hnPlan, matchingStructures, ss, context);
                choppedContours = choppedAndName.Item1;
                contraParName = choppedAndName.Item2;
                planes = choppedAndName.Item3;
            }
            else
            {
                choppedContours = new List<List<double[,]>>();
                contraParName = "";
                planes = new List<double[]>();
            }
            
            List<List<string>> updatesLog = Optimize(choppedContours, planes, ref plan, ref ss, hnPlan, context, optimizedStructures,matchingStructures, contraParName, numIterations, features);
            return Tuple.Create(optimizedStructures, matchingStructures, updatesLog);

        }
        
        public static Tuple<List<List<double[,]>>, string, List<double[]>> ParotidChop(ref ExternalPlanSetup plan, HNPlan hnPlan, List<List<Structure>> matchingStructures, StructureSet ss, ScriptContext context)
        {
            /* 
             1. Find contralateral parotid (one with least overlap, largest sum of distance from PTVs), get contours
             2.) split this up into its separate contours
             3. make new structure for each
             4. make constraint for each based on importance
             */

            //1.
            Structure contraPar = Segmentation.FindContraPar(plan, ss, hnPlan, matchingStructures, context);
            //Now get contours for it
            // GetContours function will return the list of contours, as well as a list of all z-planes which contours were taken from, in a tuple
            var tuple = Segmentation.GetContours(contraPar, context);
            List<double[,]> contours = tuple.Item1;
            List<double[]> planes = tuple.Item2;


            //2. Now the parotid segmentation!
            int numCutsZ = 2;
            int numCutsX = 2;
            int numCutsY = 1;
            List<List<double[,]>> choppedContours = Segmentation.Chop(contours, numCutsX, numCutsY, numCutsZ, contraPar.Name);

            return Tuple.Create(choppedContours, contraPar.Name, planes);
            



        }
        public static List<List<string>> Optimize(List<List<double[,]>> choppedContours, List<double[]>
            planes, ref ExternalPlanSetup plan, ref StructureSet ss, HNPlan hnPlan, ScriptContext context, List<List<Structure>> optimizedStructures, List<List<Structure>> matchingStructures, string contraName, int numIterations, List<Tuple<bool, double[], string>> features)
        //return a list of strings which is the log of constraint updates during optimization. 
        {
            //Only make parotid structures if that feature has been selected
            
            if (features[0].Item1 == true)
            {
                double priorityRatio = features[0].Item2[0];
                Segmentation.MakeParotidStructures(choppedContours, planes, ref plan, ref ss, hnPlan, context, matchingStructures, contraName, priorityRatio);
            }
            else
            {
                //remove previously segmented structures if there
                foreach (Structure structure in ss.Structures.ToList())
                {
                    if (structure.Name.ToLower().Contains("cpg_subseg"))
                    {
                        ss.RemoveStructure(structure);
                    }
                }
            }
            
            //Now run the first VMAT optimization. 
            plan.SetCalculationModel(CalculationType.PhotonVMATOptimization, "PO_13623");
            plan.SetCalculationModel(CalculationType.DVHEstimation, "DVH Estimation Algorithm [15.6.06]");
            plan.SetCalculationModel(CalculationType.PhotonVolumeDose, "AAA_13623");
            plan.OptimizationSetup.AddNormalTissueObjective(100, 3, 95, 50, 0.2);
            plan.OptimizeVMAT();
            plan.CalculateDose();

            string mlcID;
            OptimizationOptionsVMAT oov;
            ;
            List<List<string>> updateLog = new List<List<string>>();
            for (int iter = 0; iter < numIterations; iter++)
            {
                mlcID = plan.Beams.FirstOrDefault<Beam>().MLC.Id;
                oov = new OptimizationOptionsVMAT(1, mlcID);
                plan.OptimizeVMAT(oov);
                plan.CalculateDose();

                //Now need to perform a plan check and iteratively adjust constraints based on whether they passed or failed, and whether they passed with flying colours or failed miserably.
                //Going to find the percentage by which the constraint failed or passed, and adjust both the priority and dose constraint based on this. 
                updateLog.Add(OptObjectivesEditing.UpdateConstraints(ref plan, ref ss, ref hnPlan, context, optimizedStructures, matchingStructures));
                if (features[0].Item1 == true)
                {
                    Segmentation.MakeParotidStructures(choppedContours, planes, ref plan, ref ss, hnPlan, context, matchingStructures, contraName, features[0].Item2[0]);
                }
            }
            //The final iteration does 4 VMAT cycles
            mlcID = plan.Beams.FirstOrDefault<Beam>().MLC.Id; //both MLCs have same Id
            oov = new OptimizationOptionsVMAT(4, mlcID);
            plan.OptimizeVMAT(oov);
            plan.CalculateDose();

            return updateLog;
            //Now need to perform a plan check and iteratively adjust constraints based on whether they passed or failed, and whether they passed with flying colours or failed miserably.
            //Going to find the percentage by which the constraint failed or passed, and adjust both the priority and dose constraint based on this. 
           



        }
        

        
       
        public static void BeamMaker(ref ExternalPlanSetup plan, StructureSet ss, double prescriptionDose)
        {
            //First check if beams already exist
            foreach (Beam beam in plan.Beams)
            {
                if (beam.Id == "vmat1")
                {
                    return;
                } 
            }

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
                    if (StringOperations.FindPTVNumber(structure.Name) == prescriptionDose / 100)
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
            xMin = Math.Max(-149, xMin);
            yMin = Math.Max(-149, yMin);
            xMax = Math.Min(149, xMax);
            yMax = Math.Min(149, yMax);
            //Issues if exceed a field size of 22cm, so need to trim if necessary:
            if (xMax-xMin > 220)
            {
                //trim the largest one down to meet the size restriction:
                if (Math.Abs(xMin) > xMax)
                {
                    double extraLength = (xMax - xMin) - 220;
                    xMin += extraLength + 5;
                }else if (xMax > Math.Abs(xMin))
                {
                    double extraLength = (xMax - xMin) - 220;
                    xMax -= extraLength + 5;
                }
            }
            if (yMax - yMin > 220)
            {
                //trim the largest one down to meet the size restriction:
                if (Math.Abs(yMin) > yMax)
                {
                    double extraLength = (yMax - yMin) - 220;
                    yMin += extraLength + 5;
                }
                else if (yMax > Math.Abs(yMin))
                {
                    double extraLength = (yMax - yMin) - 220;
                    yMax -= extraLength + 5;
                }
            }
            return new VRect<double>(xMin, yMin, xMax, yMax);
        }

        private static void AdjustJawSizeForContour(ref double xMin, ref double xMax, ref double yMin, ref double yMax, VVector isocentre, IEnumerable<VVector> contour, double gantryRtnInRad, double collRtnInRad, bool exactFit = false)
        {
            foreach (var point in contour)
            {
                Tuple<double,double> projection = ProjectToBeamEyeView(point, isocentre, gantryRtnInRad, collRtnInRad);
                double x = projection.Item1;
                double y = projection.Item2;

                if (x < xMin)
                {
                    xMin = x;
                }
                else if (x > xMax)
                {
                    xMax = x;
                }

                if (y < yMin)
                {
                    yMin = y;
                }
                else if (y > yMax)
                {
                    yMax = y;
                }
            }
        }

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


        

       
        
        
        
        private static double DegToRad(double angle)
        {
            const double degToRad = Math.PI / 180.0D;
            return angle * degToRad;
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
