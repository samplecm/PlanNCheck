using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Plan_n_Check.Plans;
using VMS.TPS.Common.Model.API;
using Microsoft.ML.OnnxRuntime;
using System.Numerics.Tensors;

namespace Plan_n_Check.Dose_Model
{
    class Model
    {
        public static List<double> Predict_Score(HNPlan plan, ScriptContext context)
        {
            List<string> roi_list = new List<string>()
            {
                "Brain Stem",
                "Laryngo-pharynx",
                "Mandible",
                "Oral Cavity",
                "Left Parotid",
                "Right Parotid",
                "Spinal Cord",
                "Right Submandibular",
                "Left Submandibular"
            };
            List<string> ptv_list = new List<string>()
            {
                "PTV 70",
                "PTV 63",
                "PTV 56"
            };

            List<double> scores = new List<double>();

            List<List<double>> distances = ROI_Distances.Get_ROI_Distances(plan, roi_list, ptv_list, context);
            //Call the python model program
            //DataHandling.Run_Model_Exe(Path.Combine(Global_Variables.dose_model_dir,"main.exe"));
            string path = Path.Combine(Global_Variables.dose_model_dir, "Model.onnx");
            Run_Model(path, distances);
            return scores;
        }
        public static void Run_Model(string path, List<List<double>> distances)
        {
            var session = new InferenceSession(path);
            System.Windows.MessageBox.Show("Created a new inference session");
        }
    }
}
