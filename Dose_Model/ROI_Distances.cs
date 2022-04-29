using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plan_n_Check.Plans;
using VMS.TPS.Common.Model.API;
using System.IO;

namespace Plan_n_Check.Dose_Model
{
    class ROI_Distances
    {

        public static List<List<double>> Get_ROI_Distances(HNPlan plan, List<string> roi_list, List<string> ptv_list, ScriptContext context)
        {
            List<List<double>> distance_list = new List<List<double>>();
            //This list is to contain a list for each roi, each having a distance value from each ptv in ptv_list. (minimum distance)
            ROI roi;
            ROI ptv;
            for (int r = 0; r < roi_list.Count; r++)
            {
                string roi_name = roi_list[r];
                distance_list.Add(new List<double>());
                for (int p = 0; p < ptv_list.Count; p++)
                {
                    string ptv_name = ptv_list[p];
                    
                    roi = plan.ROI_Dict[roi_name];
                    ptv = plan.ROI_Dict[ptv_name];

                    if ((roi.MatchingStructures.Count == 0)||(ptv.MatchingStructures.Count == 0)) //If the plan doesn't have either one of the roi or ptv
                    {
                        distance_list[r].Add((double)1000);
                        continue;
                    }
                    //Now get distance between the roi and ptv (minimum) and add this value to the list
                    double distance = Structure_Distance(roi, ptv, context);
                    distance_list[r].Add(distance);
            
                }
            }
            DataHandling.SaveList(Path.Combine(Global_Variables.temp_directory, "distance_list.txt"), distance_list);
            DataHandling.List_To_CSV(Path.Combine(Global_Variables.temp_directory, "distances.csv"), distance_list);
            return distance_list;
        }
        public static double Structure_Distance(ROI roi, ROI ptv, ScriptContext context)
        {
            //Get the minimum distance between two rois.
            //System.Windows.MessageBox.Show("Looking for " + roi.Name + " and " + ptv.Name);
            double min_distance = 1000;
            
   
            List<Structure> roi_structures = roi.MatchingStructures;
            List<Structure> ptv_structures = ptv.MatchingStructures;
            for (int r = 0; r < roi_structures.Count; r++)
            {
                //System.Windows.MessageBox.Show(roi_structures[r].Name);
                for (int p = 0; p < ptv_structures.Count; p++)
                {

                    double total_distance = 0;
                    Structure roi_structure = roi_structures[r];
                    Structure ptv_structure = ptv_structures[p];
                    //System.Windows.MessageBox.Show(roi_structure.Name + " and " + ptv_structure.Name);

                    //First see if they overlap, and if they do, min distance = 0.
                    //Now keep margining the ptv and finding out when an intersection occurs (to get the minimum distance). This minimum distance will be the margin amount. 
                    double margin = 0;
                    SegmentVolume margin_seg_vol = ptv_structure.Margin(margin);
                    SegmentVolume overlap_volume = margin_seg_vol.And(roi_structure);
                    Structure overlap_structure = context.StructureSet.AddStructure("CONTROL", "overlap_struc");
                    Structure margin_structure = context.StructureSet.AddStructure("CONTROL", "margin_struct");
                    overlap_structure.SegmentVolume = overlap_volume;
                    margin_structure.SegmentVolume = margin_seg_vol;
                    double overlap = overlap_structure.Volume;
                    if (overlap > 0)
                    {
                        //System.Windows.MessageBox.Show(roi_structure.Name + " overlaps with " + ptv_structure.Name);
                        context.StructureSet.RemoveStructure(overlap_structure);
                        context.StructureSet.RemoveStructure(margin_structure);
                        return 0;
                    }
                    

                    double last_no;
                    double last_yes = 1000;
                    //these doubles are the last margins used for which there was not an overlap, and for which there was an overlap
                    int num_iters = 0;
                    while (true)
                    {
                        margin = 10;
                        
                        margin_seg_vol = margin_structure.Margin(margin);
                        

                        overlap_volume = margin_seg_vol.And(roi_structure);
                        overlap_structure.SegmentVolume = overlap_volume;
                        
                        overlap = overlap_structure.Volume;
                        if (overlap > 0)
                        {
                            //Now get the last_no margin ptv again to start the final iteration.
                     
                            break;
                        }
                        num_iters++;
                        margin_structure.SegmentVolume = margin_seg_vol;
                        total_distance += 10;
                        if (num_iters > 30)
                        {
                            System.Windows.MessageBox.Show("Got stuck looking for initial overlap at total distance " + total_distance.ToString());
                            System.Windows.Forms.Application.Exit();
                        }

                    }
                    
                    
                    last_yes = 10;
                    last_no = 0;

                    //Do a maximum of 30 iterations
                    int loop_count = 0;

                    do
                    {
                        loop_count++;
                        margin = 0.5 * (last_yes + last_no);                       
            
                        margin_seg_vol = margin_structure.Margin(margin);
                        overlap_volume = margin_seg_vol.And(roi_structure);
                        overlap_structure.SegmentVolume = overlap_volume;
                        
                        overlap = overlap_structure.Volume;
                        //System.Windows.MessageBox.Show("Trying margin: " + margin.ToString());

                        if (overlap > 0)
                        {
                            last_yes = margin;
                        }
                        else
                        {
                            last_no = margin;
                        }                      
                       
                        if (loop_count > 30)
                        {
                            System.Windows.MessageBox.Show("Maxed out with 30 iterations of overlap checking for " + roi.Name);
                            System.Windows.Forms.Application.Exit();
                            break;
                        }


                    } while (last_yes - last_no > 2); //look until within 2mm of distance

                    total_distance += margin;
                    if (total_distance < min_distance)
                    {
                        min_distance = margin;
                    }
                    
                    context.StructureSet.RemoveStructure(overlap_structure);
                    context.StructureSet.RemoveStructure(margin_structure);
                }
            }
            //System.Windows.MessageBox.Show(roi.Name + " distance: " + min_distance);
            return min_distance;

        }
       

        
    }
}
