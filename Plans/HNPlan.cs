using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_n_Check.Plans
{
    public class HNPlan : Plan
    {

        string name = "Head and Neck";
        List<ROI> rois = new List<ROI>();
        double prescriptionDose;
        public HNPlan(double presDose)
        {
            this.prescriptionDose = presDose;


            //Get all constraints set

            //Brain stem constraint (0)
            ROI Brainstem = new ROI();
            Brainstem.Name = "Brain Stem";
            Brainstem.Constraints.Add(new Constraint("D", "max", "<", 5400, "abs", 100));
            Brainstem.Constraints.Add(new Constraint("V", "5000", "<", 0.1, "abs", 100));
            Brainstem.Type = "OAR";
            Brainstem.Critical = true;
            this.rois.Add(Brainstem); //Add to ROI list

            //Brainstem PRV: (1)
            ROI BrainStemPRV = new ROI();
            BrainStemPRV.Name = "Brain Stem PRV";
            BrainStemPRV.Constraints.Add(new Constraint("V", "6000", "<", 0.1, "abs", 100));
            BrainStemPRV.Type = "OAR";
            BrainStemPRV.Critical = true;
            this.rois.Add(BrainStemPRV);

            //Spinal Cord: (2)
            ROI SpinalCord = new ROI();
            SpinalCord.Name = "Spinal Cord";
            SpinalCord.Constraints.Add(new Constraint("D", "max", "<", 4800, "abs", 100));
            SpinalCord.Constraints.Add(new Constraint("V", "4500", "<", 0.1, "abs", 100));
            SpinalCord.Type = "OAR";
            SpinalCord.Critical = true;
            this.rois.Add(SpinalCord);

            //Spinal Cord PRV: (3)
            ROI SpinalCordPRV = new ROI();
            SpinalCordPRV.Name = "Spinal Cord PRV";
            SpinalCordPRV.Constraints.Add(new Constraint("V", "5200", "<", 0.1, "abs", 100));
            SpinalCordPRV.Type = "OAR";
            SpinalCordPRV.Critical = true;
            this.rois.Add(SpinalCordPRV);

            //PTV70: (4)
            ROI PTV70 = new ROI();
            PTV70.Name = "PTV 70";
            PTV70.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 110));
            PTV70.Constraints.Add(new Constraint("D", "max", "<", 1.1 * prescriptionDose, "abs", 100));
            this.rois.Add(PTV70);

            //PTV63 (5)
            ROI PTV63 = new ROI();
            PTV63.Name = "PTV 63";
            PTV63.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 110));
            PTV63.Constraints.Add(new Constraint("D", "max", "<", 1.1 * prescriptionDose, "abs", 100));
            this.rois.Add(PTV63);

            //PTV56 (6)
            ROI PTV56 = new ROI();
            PTV56.Name = "PTV 56";
            PTV56.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 120));
            PTV56.Constraints.Add(new Constraint("D", "max", "<", 1.1 * prescriptionDose, "abs", 100));
            this.rois.Add(PTV56);

            //Brain (7)
            ROI Brain = new ROI();
            Brain.Name = "Brain";
            Brain.Constraints.Add(new Constraint("V", "6000", "<", 0.1, "abs", 100));
            Brain.Type = "OAR";
            Brain.Critical = true;
            this.rois.Add(Brain);
            

            //Chiasm  (8)
            ROI Chiasm = new ROI();
            Chiasm.Name = "Chiasm";
            Chiasm.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20));
            Chiasm.Type = "OAR";
            this.rois.Add(Chiasm);
            

            //Chiasm PRV (9)
            ROI ChiasmPRV = new ROI();
            ChiasmPRV.Name = "Chiasm PRV";
            ChiasmPRV.Constraints.Add(new Constraint("D", "max", "<", 5000, "abs", 20));
            ChiasmPRV.Type = "OAR";
            this.rois.Add(ChiasmPRV);
            

            //Right Optic Nerve (10)
            ROI ROpticNerve = new ROI();
            ROpticNerve.Name = "Right Optic Nerve";
            ROpticNerve.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20));
            ROpticNerve.Type = "OAR";
            ROpticNerve.Critical = true;
            this.rois.Add(ROpticNerve);
            

            //Left Optic Nerve (11)
            ROI LOpticNerve = new ROI();
            LOpticNerve.Name = "Left Optic Nerve";
            LOpticNerve.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20));
            LOpticNerve.Type = "OAR";
            LOpticNerve.Critical = true;
            this.rois.Add(LOpticNerve);
            

            //Optic Nerve PRV (12)
            ROI OpticNervePRV = new ROI();
            OpticNervePRV.Name = "Optic Nerve PRV";
            OpticNervePRV.Constraints.Add(new Constraint("D", "max", "<", 5000, "abs", 20));
            OpticNervePRV.Type = "OAR";
            this.rois.Add(OpticNervePRV);
            

            //Right Globe (14)
            ROI RGlobe = new ROI();
            RGlobe.Name = "Right Globe";
            RGlobe.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20));
            RGlobe.Type = "OAR";
            this.rois.Add(RGlobe);
            

            //Left Globe (15)
            ROI LGlobe = new ROI();
            LGlobe.Name = "Left Globe";
            LGlobe.Constraints.Add(new Constraint("D", "max", "<", 4500, "abs", 20));
            LGlobe.Type = "OAR";
            this.rois.Add(LGlobe);
           

            //Right Parotid (16)
            ROI RParotid = new ROI();
            RParotid.Name = "Right Parotid";
            RParotid.Constraints.Add(new Constraint("D", "mean", "<", 2000, "abs", 50));
            RParotid.Type = "OAR";
            this.rois.Add(RParotid);
            

            //Left Parotid (17)
            ROI LParotid = new ROI();
            LParotid.Name = "Left Parotid";
            LParotid.Constraints.Add(new Constraint("D", "mean", "<", 2000, "abs", 50));
            LParotid.Type = "OAR";
            this.rois.Add(LParotid);
            

            //Right Submandibular (18)
            ROI RSubmandibular = new ROI();
            RSubmandibular.Name = "Right Submandibular";
            RSubmandibular.Constraints.Add(new Constraint("D", "mean", "<", 2000, "abs", 30));
            RSubmandibular.Type = "OAR";
            this.rois.Add(RSubmandibular);
            

            //Left Submandibular (19)
            ROI LSubmandibular = new ROI();
            LSubmandibular.Name = "Left Submandibular";
            LSubmandibular.Constraints.Add(new Constraint("D", "mean", "<", 2000, "abs", 30));
            LSubmandibular.Type = "OAR";
            this.rois.Add(LSubmandibular);


            //Right Lens (20)
            ROI RLens = new ROI();
            RLens.Name = "Right Lens";
            RLens.Constraints.Add(new Constraint("D", "max", "<", 1000, "abs", 20));
            RLens.Type = "OAR";
            this.rois.Add(RLens);

            //Left Lens (21)
            ROI LLens = new ROI();
            LLens.Name = "Left Lens";
            LLens.Constraints.Add(new Constraint("D", "max", "<", 1000, "abs", 20));
            LLens.Type = "OAR";
            this.rois.Add(LLens);

            //Oral Cavity (22)
            ROI OralCavity = new ROI();
            OralCavity.Name = "Oral Cavity";
            OralCavity.Constraints.Add(new Constraint("D", "mean", "<", 5000, "abs", 40));
            OralCavity.Type = "OAR";
            this.rois.Add(OralCavity);

            //Laryngo-pharynx (23)
            ROI LarPhar = new ROI();
            LarPhar.Name = "Laryngo-pharynx";
            LarPhar.Constraints.Add(new Constraint("D", "mean", "<", 4500, "abs", 40));
            LarPhar.Type = "OAR";
            this.rois.Add(LarPhar);

            //Lips (24)
            ROI Lips = new ROI();
            Lips.Name = "Lips";
            Lips.Constraints.Add(new Constraint("D", "mean", "<", 2500, "abs", 10));
            Lips.Type = "OAR";
            this.rois.Add(Lips);

            //Mandible (25)
            ROI Mandible = new ROI();
            Mandible.Name = "Mandible";
            Mandible.Constraints.Add(new Constraint("D", "max", "<", 7000, "abs", 10));
            Mandible.Type = "OAR";
            this.rois.Add(Mandible);

            //Body (26)
            ROI Body = new ROI();
            Body.Name = "Body";
            Body.Constraints.Add(new Constraint("D", "max", "<", 7700, "abs", 100));
            this.rois.Add(Body);
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

    }
}
