using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using VMS.TPS;

namespace Plan_n_Check.Plans
{
    public class HN_NP_Plan : Plan
    {
        List<int> ptv_Types = new List<int>() { 56, 63, 70 };
        string name = "Head and Neck - Nasopharynx";
        List<ROI> rois = new List<ROI>();
        IDictionary<string, ROI> roi_dict = new Dictionary<string, ROI>();
        double prescriptionDose = 7000; //in cGy
        int fractions = 35;
        public HN_NP_Plan(double presDose, int numFractions)
        {
            this.prescriptionDose = presDose;
            this.fractions = numFractions;


            //Get all constraints set

            //Brain stem constraint (0)
            ROI Brainstem = new ROI();
            Brainstem.Name = "Brain Stem";
            Brainstem.Constraints.Add(new Constraint("D", "max", "<", 5400, "abs", 100, new List<int> { 80, 120 }));
            Brainstem.Type = "OAR";
            Brainstem.Critical = true;
            Brainstem.Weight = 100;
            this.rois.Add(Brainstem); //Add to ROI list
            this.roi_dict.Add(Brainstem.Name, Brainstem);

            //Brainstem PRV: (1)
            ROI BrainStemPRV = new ROI();
            BrainStemPRV.Name = "Brain Stem PRV";
            BrainStemPRV.Constraints.Add(new Constraint("D", "max", "<", 5900, "abs", 100, new List<int> { 80, 120 }));
            BrainStemPRV.Type = "OAR";
            BrainStemPRV.Critical = true;
            BrainStemPRV.Weight = 100;
            this.rois.Add(BrainStemPRV);
            this.roi_dict.Add(BrainStemPRV.Name, BrainStemPRV);

            //Spinal Cord: (2)
            ROI SpinalCord = new ROI();
            SpinalCord.Name = "Spinal Cord";
            SpinalCord.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 100, new List<int> { 80, 120 }));
            SpinalCord.Type = "OAR";
            SpinalCord.Critical = true;
            SpinalCord.Weight = 100;
            this.rois.Add(SpinalCord);
            this.roi_dict.Add(SpinalCord.Name, SpinalCord);

            //Spinal Cord PRV: (3)
            ROI SpinalCordPRV = new ROI();
            SpinalCordPRV.Name = "Spinal Cord PRV";
            SpinalCordPRV.Constraints.Add(new Constraint("D", "max", "<", 5000, "abs", 100, new List<int> { 80, 120 }));
            SpinalCordPRV.Type = "OAR";
            SpinalCordPRV.Critical = true;
            SpinalCordPRV.Weight = 100;
            this.rois.Add(SpinalCordPRV);
            this.roi_dict.Add(SpinalCordPRV.Name, SpinalCordPRV);

            //PTV70: (4)
            ROI PTV70 = new ROI();
            PTV70.Name = "PTV 70";
            PTV70.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 110, new List<int> { 80, 120 }));
            PTV70.Constraints.Add(new Constraint("D", "max", "<", 1.1 * this.PrescriptionDose, "abs", 100, new List<int> { 80, 120 }));
            PTV70.IsPTV = true;
            PTV70.PTVDose = 7000;
            PTV70.Weight = 100;
            this.rois.Add(PTV70);
            this.roi_dict.Add(PTV70.Name, PTV70);

            //PTV63 (5)
            ROI PTV63 = new ROI();
            PTV63.Name = "PTV 63";
            PTV63.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 110, new List<int> { 80, 120 }));
            PTV63.Constraints.Add(new Constraint("D", "max", "<", 1.1 * this.PrescriptionDose, "abs", 100, new List<int> { 80, 120 }));
            PTV63.IsPTV = true;
            PTV63.PTVDose = 6300;
            PTV63.Weight = 80;
            this.rois.Add(PTV63);
            this.roi_dict.Add(PTV63.Name, PTV63);

            //PTV56 (6)
            ROI PTV56 = new ROI();
            PTV56.Name = "PTV 56";
            PTV56.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 120, new List<int> { 80, 130 }));
            PTV56.Constraints.Add(new Constraint("D", "max", "<", 1.1 * this.PrescriptionDose, "abs", 100, new List<int> { 80, 120 }));
            PTV56.IsPTV = true;
            PTV56.PTVDose = 5600;
            PTV56.Weight = 80;
            this.rois.Add(PTV56);
            this.roi_dict.Add(PTV56.Name, PTV56);

            //Brain (7)
            ROI Brain = new ROI();
            Brain.Name = "Brain";
            Brain.Constraints.Add(new Constraint("V", "6000", "<", 0.1, "abs", 100, new List<int> { 80, 120 }));
            Brain.Type = "OAR";
            Brain.Critical = true;
            Brain.Weight = 80;
            this.rois.Add(Brain);
            this.roi_dict.Add(Brain.Name, Brain);


            //Chiasm  (8)
            ROI Chiasm = new ROI();
            Chiasm.Name = "Chiasm";
            Chiasm.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20, new List<int> { 0, 30 }));
            Chiasm.Type = "OAR";
            Chiasm.Weight = 80;
            this.rois.Add(Chiasm);
            this.roi_dict.Add(Chiasm.Name, Chiasm);


            //Chiasm PRV (9)
            ROI ChiasmPRV = new ROI();
            ChiasmPRV.Name = "Chiasm PRV";
            ChiasmPRV.Constraints.Add(new Constraint("D", "max", "<", 5000, "abs", 20, new List<int> { 0, 30 }));
            ChiasmPRV.Type = "OAR";
            ChiasmPRV.Weight = 80;
            this.rois.Add(ChiasmPRV);
            this.roi_dict.Add(ChiasmPRV.Name, ChiasmPRV);


            //Right Optic Nerve (10)
            ROI ROpticNerve = new ROI();
            ROpticNerve.Name = "Right Optic Nerve";
            ROpticNerve.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20, new List<int> { 0, 40 }));
            ROpticNerve.Type = "OAR";
            ROpticNerve.Critical = true;
            ROpticNerve.Weight = 20;
            this.rois.Add(ROpticNerve);
            this.roi_dict.Add(ROpticNerve.Name, ROpticNerve);


            //Left Optic Nerve (11)
            ROI LOpticNerve = new ROI();
            LOpticNerve.Name = "Left Optic Nerve";
            LOpticNerve.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20, new List<int> { 0, 40 }));
            LOpticNerve.Type = "OAR";
            LOpticNerve.Critical = true;
            LOpticNerve.Weight = 20;
            this.rois.Add(LOpticNerve);
            this.roi_dict.Add(LOpticNerve.Name, LOpticNerve);



            //Optic Nerve PRV (12)
            ROI OpticNervePRV = new ROI();
            OpticNervePRV.Name = "Optic Nerve PRV";
            OpticNervePRV.Constraints.Add(new Constraint("D", "max", "<", 5000, "abs", 20, new List<int> { 0, 40 }));
            OpticNervePRV.Type = "OAR";
            OpticNervePRV.Weight = 20;
            this.rois.Add(OpticNervePRV);
            this.roi_dict.Add(OpticNervePRV.Name, OpticNervePRV);


            //Right Globe (13)
            ROI RGlobe = new ROI();
            RGlobe.Name = "Right Globe";
            RGlobe.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20, new List<int> { 0, 30 }));
            RGlobe.Type = "OAR";
            RGlobe.Weight = 20;
            this.rois.Add(RGlobe);
            this.roi_dict.Add(RGlobe.Name, RGlobe);


            //Left Globe (14)
            ROI LGlobe = new ROI();
            LGlobe.Name = "Left Globe";
            LGlobe.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20, new List<int> { 0, 30 }));
            LGlobe.Type = "OAR";
            LGlobe.Weight = 20;
            this.rois.Add(LGlobe);
            this.roi_dict.Add(LGlobe.Name, LGlobe);


            //Right Parotid (15)
            ROI RParotid = new ROI();
            RParotid.Name = "Right Parotid";
            RParotid.Constraints.Add(new Constraint("D", "mean", "<", 2000, "abs", 50, new List<int> { 0, 70 }));
            RParotid.Type = "OAR";
            RParotid.Weight = 50;
            this.rois.Add(RParotid);
            this.roi_dict.Add(RParotid.Name, RParotid);


            //Left Parotid (16)
            ROI LParotid = new ROI();
            LParotid.Name = "Left Parotid";
            LParotid.Constraints.Add(new Constraint("D", "mean", "<", 2000, "abs", 50, new List<int> { 0, 70 }));
            LParotid.Type = "OAR";
            LParotid.Weight = 50;
            this.rois.Add(LParotid);
            this.roi_dict.Add(LParotid.Name, LParotid);


            //Right Submandibular (17)
            ROI RSubmandibular = new ROI();
            RSubmandibular.Name = "Right Submandibular";
            RSubmandibular.Constraints.Add(new Constraint("D", "mean", "<", 2000, "abs", 30, new List<int> { 0, 50 }));
            RSubmandibular.Type = "OAR";
            RSubmandibular.Weight = 30;
            this.rois.Add(RSubmandibular);
            this.roi_dict.Add(RSubmandibular.Name, RSubmandibular);


            //Left Submandibular (18)
            ROI LSubmandibular = new ROI();
            LSubmandibular.Name = "Left Submandibular";
            LSubmandibular.Constraints.Add(new Constraint("D", "mean", "<", 2000, "abs", 30, new List<int> { 0, 50 }));
            LSubmandibular.Type = "OAR";
            LSubmandibular.Weight = 30;
            this.rois.Add(LSubmandibular);
            this.roi_dict.Add(LSubmandibular.Name, LSubmandibular);

            //Right Lens (19)
            ROI RLens = new ROI();
            RLens.Name = "Right Lens";
            RLens.Constraints.Add(new Constraint("D", "max", "<", 1000, "abs", 20, new List<int> { 0, 40 }));
            RLens.Type = "OAR";
            RLens.Weight = 20;
            this.rois.Add(RLens);
            this.roi_dict.Add(RLens.Name, RLens);

            //Left Lens (20)
            ROI LLens = new ROI();
            LLens.Name = "Left Lens";
            LLens.Constraints.Add(new Constraint("D", "max", "<", 1000, "abs", 20, new List<int> { 0, 40 }));
            LLens.Type = "OAR";
            LLens.Weight = 20;
            this.rois.Add(LLens);
            this.roi_dict.Add(LLens.Name, LLens);

            //Right Eye (21)
            ROI R_Eye = new ROI();
            R_Eye.Name = "Right Eye";
            R_Eye.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20, new List<int> { 0, 40 }));
            R_Eye.Type = "OAR";
            R_Eye.Weight = 20;
            this.rois.Add(R_Eye);
            this.roi_dict.Add(R_Eye.Name, R_Eye);

            //Left Eye (22)
            ROI L_Eye = new ROI();
            L_Eye.Name = "Left Eye";
            L_Eye.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20, new List<int> { 0, 40 }));
            L_Eye.Type = "OAR";
            L_Eye.Weight = 20;
            this.rois.Add(L_Eye);
            this.roi_dict.Add(L_Eye.Name, L_Eye);

            //Right Lacrimal (23)
            ROI R_Lacrimal = new ROI();
            R_Lacrimal.Name = "Right Lacrimal";
            R_Lacrimal.Constraints.Add(new Constraint("D", "max", "<", 2000, "abs", 20, new List<int> { 0, 40 }));
            R_Lacrimal.Type = "OAR";
            R_Lacrimal.Weight = 20;
            this.rois.Add(R_Lacrimal);
            this.roi_dict.Add(R_Lacrimal.Name, R_Lacrimal);

            //Left Lacrimal (24)
            ROI L_Lacrimal = new ROI();
            L_Lacrimal.Name = "Left Lacrimal";
            L_Lacrimal.Constraints.Add(new Constraint("D", "max", "<", 2000, "abs", 20, new List<int> { 0, 40 }));
            L_Lacrimal.Type = "OAR";
            L_Lacrimal.Weight = 20;
            this.rois.Add(L_Lacrimal);
            this.roi_dict.Add(L_Lacrimal.Name, L_Lacrimal);

            //Right Cochlea (25)
            ROI R_Cochlea = new ROI();
            R_Cochlea.Name = "Right Cochlea";
            R_Cochlea.Constraints.Add(new Constraint("D", "max", "<", 2000, "abs", 20, new List<int> { 0, 40 }));
            R_Cochlea.Type = "OAR";
            R_Cochlea.Weight = 20;
            this.rois.Add(R_Cochlea);
            this.roi_dict.Add(R_Cochlea.Name, R_Cochlea);

            //Left Cochlea (26)
            ROI L_Cochlea = new ROI();
            L_Cochlea.Name = "Left Cochlea";
            L_Cochlea.Constraints.Add(new Constraint("D", "max", "<", 2000, "abs", 20, new List<int> { 0, 40 }));
            L_Cochlea.Type = "OAR";
            L_Cochlea.Weight = 20;
            this.rois.Add(L_Cochlea);
            this.roi_dict.Add(L_Cochlea.Name, L_Cochlea);

            //Oral Cavity (27)
            ROI OralCavity = new ROI();
            OralCavity.Name = "Oral Cavity";
            OralCavity.Constraints.Add(new Constraint("D", "mean", "<", 4500, "abs", 40, new List<int> { 0, 70 }));
            OralCavity.Type = "OAR";
            OralCavity.Weight = 50;
            this.rois.Add(OralCavity);
            this.roi_dict.Add(OralCavity.Name, OralCavity);

            //Laryngo-pharynx (28)
            ROI LarPhar = new ROI();
            LarPhar.Name = "Laryngo-pharynx";
            LarPhar.Constraints.Add(new Constraint("D", "mean", "<", 4500, "abs", 40, new List<int> { 0, 70 }));
            LarPhar.Type = "OAR";
            LarPhar.Weight = 50;
            this.rois.Add(LarPhar);
            this.roi_dict.Add(LarPhar.Name, LarPhar);

            //Lips (29)
            ROI Lips = new ROI();
            Lips.Name = "Lips";
            Lips.Constraints.Add(new Constraint("D", "mean", "<", 2500, "abs", 10, new List<int> { 0, 30 }));
            Lips.Type = "OAR";
            Lips.Weight = 10;
            this.rois.Add(Lips);
            this.roi_dict.Add(Lips.Name, Lips);

            //Mandible (30)
            ROI Mandible = new ROI();
            Mandible.Name = "Mandible";
            Mandible.Constraints.Add(new Constraint("D", "max", "<", 7000, "abs", 10, new List<int> { 0, 30 }));
            Mandible.Type = "OAR";
            Mandible.Weight = 30;
            this.rois.Add(Mandible);
            this.roi_dict.Add(Mandible.Name, Mandible);

            //Body (31)
            ROI Body = new ROI();
            Body.Name = "Body";
            Body.Constraints.Add(new Constraint("D", "max", "<", 1.1 * this.PrescriptionDose, "abs", 100, new List<int> { 80, 130 }));
            Body.Weight = 100;
            this.rois.Add(Body);
            this.roi_dict.Add(Body.Name, Body);
        }

        public override string Name
        {
            get { return name; }
            set { name = value; }
        }
        public override List<ROI> ROIs
        {
            get { return rois; }
            set { rois = value; }
        }
        public override double PrescriptionDose
        {
            get { return prescriptionDose; }
            set { prescriptionDose = value; }
        }
        public override List<int> PTV_Types
        {
            get { return ptv_Types; }
            set { ptv_Types = value; }
        }
        public override int Fractions
        {
            get { return fractions; }
            set { fractions = value; }
        }
        
        public override IDictionary<string, ROI> ROI_Dict
        {
            get { return roi_dict; }
            set { roi_dict = value; }
        }
 



    }
}
