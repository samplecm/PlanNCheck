using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plan_n_Check.Plans;
using Plan_n_Check.Calculate;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Plan_n_Check.Features
{
    public static class Segmentation
    {

        public static void MakeParotidStructures(List<List<double[,]>> choppedContours, List<double[]>
            planes, ref ExternalPlanSetup plan_setup, ref StructureSet ss, Plan plan_obj, ScriptContext context, List<List<Structure>> matchingStructures, string contraName, double priorityRatio)
        {
            Image image = context.Image;
            //First off, in case this script is re-run, we need to remove any subsegment structures already made. 
            foreach (Structure structure in ss.Structures.ToList())
            {
                if (structure.Name.ToLower().Contains("cpg_subseg"))
                {
                    ss.RemoveStructure(structure);
                }
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
            //Base constraints on the d50 parameter in the saliva model for the subregion in Gy
            double[] D50 = new double[18] { 21.7, 20.4, 14.3, 21.6, 14.8, 13.5, 15, 11.6, 7.8, 20.7, 13.5, 1000, 21.7, 1000, 1000, 15.3, 1000, 1000 };
            int numSections = choppedContours.Count;
            double[] importanceValues = new double[18] { 0.751310670731707,  0.526618902439024,   0.386310975609756,
                1,   0.937500000000000,   0.169969512195122,   0.538871951219512 ,  0.318064024390244,   0.167751524390244,
                0.348320884146341,   0.00611608231707317, 0.0636128048780488,  0.764222560975610,   0.0481192835365854,  0.166463414634146,
                0.272984146341463,   0.0484897103658537,  0.0354939024390244 };
            //Now need to chop up the parotid, and make new constraints
            List<Structure> subsegments = new List<Structure>();
            string name;

            for (int subsegment = 0; subsegment < numSections; subsegment++) //loop over all subsegments
            {
                name = "CPG_subseg " + (subsegment + 1).ToString();
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
                            currentContour[i] = image.DicomToUser(currentContour[i], plan_setup);
                        }
                        subsegments[subsegment].AddContourOnImagePlane(currentContour, planeIndex);
                        //Now also need to set an optimization constraint based on the importance, and the constraint set on the whole contralateral parotid.    

                    }

                    Tuple<double, int> overlapData = Check.RatioOverlapWithPTV(choppedContours[subsegment], ss);
                    //First element is overlap ratio, second is prescription dose of overlapping ptv
                    double overlapRatio = overlapData.Item1;

                    int overlapPTVDose = overlapData.Item2;



                    doseConstraint = D50[subsegment];//plan_obj.ROIs[ROI_Index].Constraints[0].Value;
                    priority = plan_obj.ROIs[ROI_Index].Constraints[0].Priority;
                    priority *= priorityRatio * importanceValues[subsegment];

                    if ((priority > 10) && (doseConstraint != 1000))
                    {
                        //Furthermore, want to temper priority and dose constraint. Take weighted average of dose constraint with the ptv it overlaps with
                        doseConstraint = (1 - overlapRatio) * (doseConstraint * 100) + overlapRatio * overlapPTVDose;
                        priority = priority * (1 - overlapRatio);
                        plan_setup.OptimizationSetup.AddMeanDoseObjective(subsegments[subsegment], new DoseValue(doseConstraint, "cGy"), priority);
                    }


                }
                else
                {
                    System.Windows.MessageBox.Show("Could not create new subsegment structures.");
                }
            }

        }
        public static Tuple<List<List<double[,]>>, List<double[]>> OrganChop(Structure organ, int[] numCuts, StructureSet ss, ScriptContext context)
        {
            /* 
             
             1.) split this up into its separate contours
             2. make new structure for each
\
             */


            //Now get contours for it
            // GetContours function will return the list of contours, as well as a list of all z-planes which contours were taken from, in a tuple
            var tuple = GetContours(organ, context);
            List<double[,]> contours = tuple.Item1;
            List<double[]> planes = tuple.Item2;


            //2. Now the parotid segmentation!
            int numCutsZ = numCuts[0];
            int numCutsX = numCuts[2];
            int numCutsY = numCuts[1];
            List<List<double[,]>> choppedContours = Chop(contours, numCutsX, numCutsY, numCutsZ, organ.Name);

            return Tuple.Create(choppedContours, planes);




        }
        public static void MakeSubsegmentStructures(Structure roi, double[] numCutsDouble, ref ExternalPlanSetup plan_setup, ref StructureSet ss, ScriptContext context, bool applyConstraints, List<List<Structure>> matchingStructures, ref Plan plan)
        {
            //Convert numCuts to int array
            int[] numCuts = numCutsDouble.Select(d => (int)d).ToArray();
            //Chop up the contours
            Tuple<List<List<double[,]>>, List<double[]>> choppedTuple = OrganChop(roi, numCuts, ss, context);
            List<List<double[,]>> choppedContours = choppedTuple.Item1;
            List<double[]> planes = choppedTuple.Item2;
            Image image = context.Image;
            int numSections = choppedContours.Count;
            //get constraint on original structure, if applyconstraints is true:
            int ROI_Index = 0;
            DoseValue doseConstraint = new DoseValue(0, "cGy");
            string constraintType = "";
            double priority = 0;

            if (applyConstraints)
            {

                for (int i = 0; i < matchingStructures.Count; i++)
                {
                    for (int j = 0; j < matchingStructures[i].Count; j++)
                    {
                        if (roi.Name == matchingStructures[i][j].Name)
                        {
                            ROI_Index = i;
                            //assume its a dose constraint
                            plan.ROIs[i].HasSubsegments = true;
                            doseConstraint = new DoseValue(plan.ROIs[i].Constraints[0].Value, "cGy");
                            constraintType = plan.ROIs[i].Constraints[0].Subscript;
                            priority = plan.ROIs[i].Constraints[0].Priority;

                        }

                    }

                }
            }

            List<Structure> subsegments = new List<Structure>();
            string name;

            for (int subsegment = 0; subsegment < numSections; subsegment++) //loop over all subsegments
            {
                int min = Math.Min(6, roi.Name.Length - 1);
                name = roi.Name.Substring(0, min) + "_subseg" + (subsegment + 1).ToString();
                //Delete structures if already there:
                foreach (Structure s2 in ss.Structures.ToList())
                {
                    if (s2.Name == (name))
                    {
                        ss.RemoveStructure(s2);
                    }
                }
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
                            currentContour[i] = image.DicomToUser(currentContour[i], plan_setup);
                        }
                        subsegments[subsegment].AddContourOnImagePlane(currentContour, planeIndex);
                        //Now also need to set an optimization constraint based on the importance, and the constraint set on the whole contralateral parotid.    

                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("Could not create new subsegment structures.");
                }
                //Now need to apply constraint
                if (applyConstraints)
                {
                    if (constraintType.ToLower() == "mean")
                    {
                        plan_setup.OptimizationSetup.AddMeanDoseObjective(subsegments[subsegment], 0.9 * doseConstraint, priority);
                    }
                    else if (constraintType.ToLower() == "min")
                    {
                        plan_setup.OptimizationSetup.AddPointObjective(subsegments[subsegment], OptimizationObjectiveOperator.Lower, 1.05 * doseConstraint, 100, priority);
                    }
                    else if (constraintType.ToLower() == "max")
                    {
                        plan_setup.OptimizationSetup.AddPointObjective(subsegments[subsegment], OptimizationObjectiveOperator.Upper, 0.9 * doseConstraint, 0, priority);
                    }
                }


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


        public static Structure FindContraPar(ExternalPlanSetup plan_setup, StructureSet ss, Plan plan, List<List<Structure>> matchingStructures, ScriptContext context)
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
            List<double> overlapRatios = new List<double>();
            for (int s = 0; s < plan.ROIs.Count; s++)
            {
                if (plan.ROIs[s].Name.ToLower().Contains("parotid"))
                {
                    try { parotids.Add(matchingStructures[s][0]); }
                    catch
                    {
                        System.Windows.MessageBox.Show("Could not find matching parotid substructure in FindContraPar");
                    }

                }
            }
            if (parotids.Count < 2)
            {
                return parotids[0];
            }
            List<List<double[,]>> contours = new List<List<double[,]>>();
            //now need to find which structure in parotids is closest to PTVs. 
            //first check if one has larger overlap ratio than the other. In this case, make non-overlapping one the contralateral.
            double overlap;
            for (int i = 0; i < parotids.Count; i++)//go through all parotids
            {
                contours.Add(StructureContours(parotids[i], plan_setup, context, ss));
                //get the overlap ratio for the parotid 
                overlap = Check.RatioOverlapWithPTV(contours[contours.Count - 1], ss).Item1;
                overlapRatios.Add(overlap);
            }

            //Now find out if one overlaps more
            if (overlapRatios[0] > overlapRatios[1])
            {
                return parotids[1];
            }
            else if (overlapRatios[1] > overlapRatios[0])
            {
                return parotids[0];
            }

            //Or else need to find the average distance between each parotid and ptvs. 
            double avgDist;
            double smallestDist = 0;
            Structure contraPar = parotids[0]; //placeHolder
            VVector parCentre, ptvCentre;
            for (int i = 0; i < parotids.Count; i++)
            {
                avgDist = 0;
                parCentre = parotids[i].CenterPoint;

                for (int ptv = 0; ptv < ptvs.Count; ptv++)
                {
                    ptvCentre = ptvs[ptv].CenterPoint;
                    avgDist += Math.Sqrt(Math.Pow(parCentre.x - ptvCentre.x, 2) + Math.Pow(parCentre.y - ptvCentre.y, 2) + Math.Pow(parCentre.z - ptvCentre.z, 2));
                }
                avgDist /= ptvs.Count; //get the average distance between this parotid and all ptvs
                if (avgDist > smallestDist)
                {

                    contraPar = parotids[i];

                }

            }
            //Now return the one with the largest distance

            return contraPar;
        }


        public static List<double[,]> StructureContours(Structure structure, ExternalPlanSetup plan_setup, ScriptContext context, StructureSet ss)
        {
            //returns list of contours for a structure
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
                    VVector point = image.UserToDicom(contoursTemp[i][j], plan_setup);
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

        public static List<List<double[,]>> GetContoursPTV(StructureSet structureSet, PlanSetup plan_setup, Image image)
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
                            VVector point = image.UserToDicom(contoursTemp[i][j], plan_setup);
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
    }
}
