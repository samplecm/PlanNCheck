using System;
using System.Linq;
using System.Data;
using System.Text;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using VMS.TPS;
using Plan_n_Check;
using Plan_n_Check.Plans;
using Plan_n_Check.Calculate;

namespace Plan_n_Check
{
    public partial class MainForm : Form
    {
        public string SavePath { get; set; }
        public List<Plan> Plans { get; set;}
        public HNPlan HnPlan { get; set; }
        public List<List<Structure>> MatchingStructures { get; set; }
        public ScriptContext context { get; set; }

        public List<Structure> DicomStructures { get; set; }
        public bool[] Features { get; set; }
        public MainForm(ref ScriptContext contextIn)
        {
            InitializeComponent();
            this.context = contextIn;
            this.Plans = new List<Plan>();
            this.MatchingStructures = new List<List<Structure>>();
            this.DicomStructures = new List<Structure>();
            this.Features = new bool[1];
            
        }

        private string GetPath(String name)
        {
            var sfd = new SaveFileDialog
            {
                Title = "Choose Save Location",
                Filter = "TXT Files (*.txt)|*.txt",
                FileName = name
            };

            DialogResult dialogResult = sfd.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                textBox1.Text = sfd.FileName;
                return sfd.FileName;
            }
            else
            {
                return null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            String path = GetPath(textBox1.Text);
            if (!String.IsNullOrEmpty(path))
            {
                LocationLabel.Text = path;
                this.SavePath = path;
                StartButton.Enabled = true;
            }        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SaveLocationLabel_Click(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            //Call method for starting report. 
            if (this.SavePath == "")
            {
                StartErrorLabel.Text = "Please select a save destination.";
                StartErrorLabel.Visible = true;
            }
            else if (this.Plans.Count == 0)
            {
                StartErrorLabel.Text = "Please select a type of plan to report on.";
            }
            else if ((!int.TryParse(this.IterationsTextBox.Text, out int value))||(this.IterationsTextBox.Text == "0")) //if numints is not an integer or 0
            {
                StartErrorLabel.Text = "Number of iterations must be an integer greater than zero.";
            }
            else
            {
                int iterations = Convert.ToInt32(this.IterationsTextBox.Text);
                //Check which special optimization features to include
                if (CheckBox_ChopParotid.Checked)
                {
                    this.Features[0] = true;
                }
                this.StartErrorLabel.Text = "In progress";
                this.StartErrorLabel.Visible = true;               
                var structureLists = VMS.TPS.Script.StartOptimizer(this.context, this.HnPlan, this.MatchingStructures, iterations, this.Features);
                List<List<Structure>> optimizedStructures = structureLists.Item1;
                List<List<Structure>> matchingStructures = structureLists.Item2;
                if (SaveCheck.Checked)
                {
                    Calculator.RunReport(this.context, this.HnPlan, this.SavePath, optimizedStructures, this.MatchingStructures);
                }
               
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
                     
        }

        private void LocationLabel_Click(object sender, EventArgs e)
        {
        }
        private void CustomizeButton_Click(object sender, EventArgs e)
        {
            //First make sure only one checkbox is checked. 
            
            int checkCount = 0;
            foreach (CheckBox checkBox in groupBox1.Controls.OfType<CheckBox>())
            {
                if (checkBox.CheckState == CheckState.Checked)
                {
                    checkCount++;
                }
            }
            if (checkCount >= 2)
            {
                ErrorLabel.Text = "Please select only one plan type. Re-run the program for each plan type required.";
                ErrorLabel.Visible = true;

            }
            else if (checkCount == 0)
            {
                ErrorLabel.Text = "Please select a plan to customize first.";
                ErrorLabel.Visible = true;
            }
            else //if one plan is checked
            {
                //Need to keep track of amount of times clicked. Only create a clean plan on first click. Otherwise return to default must be pressed.
                ErrorLabel.Visible = false;
                panel1.Visible = true;
                panel1.BringToFront();
                PopulateGrid();
            }
        
            

        }



        

        private void OKButton_Click(object sender, EventArgs e)
        {
            
            this.panel1.Visible = false;
            this.panel1.SendToBack();
            
            this.MatchingStructures.Clear();
            for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, this.HnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.HnPlan = hnPlan;
            this.Plans = plans;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //add constraint
        {
            if ((String.IsNullOrEmpty(StructureTB.Text)) || (String.IsNullOrEmpty(TypeTB.Text)) || (String.IsNullOrEmpty(SubscriptTB.Text)) || 
                (String.IsNullOrEmpty(RelationTB.Text)) || (String.IsNullOrEmpty(ValueTB.Text)) || (String.IsNullOrEmpty(FormatTB.Text))) 
            {               
                this.AddLabel.Visible = true;

            }else if ((TypeTB.Text.ToLower() != "d") && (TypeTB.Text.ToLower() != "v"))
            {
                this.AddLabel.Text = "Invalid Type.";
                this.AddLabel.Visible = true;
            }
            else if ((RelationTB.Text.ToLower() != "<") && (RelationTB.Text.ToLower() != ">"))
            {
                this.AddLabel.Text = "Invalid Relation. Can either be \"<\" or \">\".";
                this.AddLabel.Visible = true;
            }
            else if ((FormatTB.Text.ToLower() != "rel") && (FormatTB.Text.ToLower() != "abs"))
            {
                this.AddLabel.Text = "Invalid Format. Can either be \"rel\" or \"abs\". ";
                this.AddLabel.Visible = true;
            }
            else if (!Double.TryParse(ValueTB.Text, out double _))
            {
                this.AddLabel.Text = "Invalid value, must be a number.";
                this.AddLabel.Visible = true;
                
            }
            else if ((SubscriptTB.Text.ToLower() != "max")&& (SubscriptTB.Text.ToLower() != "min")&& (SubscriptTB.Text.ToLower() != "mean") && 
                !(Double.TryParse(SubscriptTB.Text, out double _)))
            {
                this.AddLabel.Text = "Invalid subscript.";
                this.AddLabel.Visible = true;
            }
            else
            {
                
                this.AddLabel.Text = "Constraint Added";
                this.AddLabel.Visible = true;
                
                //First check if structure exists: 
                bool structExists = false;
                for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
                {
                    if (this.HnPlan.ROIs[i].Name == StructureTB.Text.ToString()) //if structure already exists
                    {
                        structExists = true;
                        Constraint newConstraint = new Constraint(TypeTB.Text.ToString(), SubscriptTB.Text.ToString(), RelationTB.Text.ToString(), Convert.ToDouble(ValueTB.Text.ToString()), FormatTB.Text.ToString());
                        this.HnPlan.ROIs[i].Constraints.Add(newConstraint);
                        break;
                    }
                }
                if (structExists == false)
                {
                    ROI NewROI = new ROI();
                    NewROI.Name = StructureTB.Text.ToString();
                    NewROI.Constraints.Add(new Constraint(TypeTB.Text.ToString(), SubscriptTB.Text.ToString(), RelationTB.Text.ToString(), Convert.ToDouble(ValueTB.Text.ToString()), FormatTB.Text.ToString()));

                    this.HnPlan.ROIs.Add(NewROI);
                    //Need to also add assigned structure for this
                    List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, NewROI); //find structures that match the constraint structure
                    this.MatchingStructures.Add(assignedStructures);
                }


            }
            PopulateGrid();
            this.TypeTB.Text = "";
            this.ValueTB.Text = "";
            this.RelationTB.Text = "";
            this.FormatTB.Text = "";
            this.SubscriptTB.Text = "";
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow item in this.ConstraintGridView.SelectedRows)
            {
                //Stop error of deleting last blank row
                try
                {
                    string test = item.Cells[0].Value.ToString();
                }catch
                {
                    break;
                }

               
                bool removeROI = false;
                int ROI_Index = 0;
                int constraint_index = 0;

            //Remove each constraint from the list of ROIs/constraints in plan.
            for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
                {
                    

                    if (this.HnPlan.ROIs[i].Name == item.Cells[0].Value.ToString()) //if the right structure
                    {

                        for (int j = 0; j < this.HnPlan.ROIs[i].Constraints.Count; j++) //Look to get the right constraint
                        {

                            if ((this.HnPlan.ROIs[i].Constraints[j].Type == item.Cells[1].Value.ToString()) && (this.HnPlan.ROIs[i].Constraints[j].Subscript == item.Cells[2].Value.ToString()) && 
                                (this.HnPlan.ROIs[i].Constraints[j].EqualityType == item.Cells[3].Value.ToString()))
                            {                             
                                int numConstraints = this.HnPlan.ROIs[i].Constraints.Count;
                                //Remove the constraint
                                ROI_Index = i;
                                constraint_index = j;
                                //Also remove structure if was the only constraint for it:
                                if (numConstraints == 1)
                                {
                                    removeROI = true;

                                }
                                break;
                            }
                        }
                        
                    }
                }
                this.HnPlan.ROIs[ROI_Index].Constraints.RemoveAt(constraint_index);
                if (removeROI == true)
                {
                    this.HnPlan.ROIs.RemoveAt(ROI_Index);
                    
                }               
            }
            //Now need to update the grid: 
            
            PopulateGrid();
        }
        public void PopulateGrid()
        {

            //Delete all rows: 
            int numRows = ConstraintGridView.Rows.Count;
            if (numRows > 1)
            {
                for (int i = numRows - 1; i > 0; i--)
                {
                    try
                    {
                        ConstraintGridView.Rows.RemoveAt(i);
                    }
                    catch
                    {
                    }

                }
                ConstraintGridView.Rows.Clear();
            }
            ConstraintGridView.ForeColor = System.Drawing.Color.Black;
            DataTable dt = new DataTable();
            dt.Columns.Add("structure");
            dt.Columns.Add("type");
            dt.Columns.Add("subscript");
            dt.Columns.Add("relation");
            dt.Columns.Add("value");
            dt.Columns.Add("format");
            //Get all the constraints and make a row for each

            for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
            {
                for (int j = 0; j < this.HnPlan.ROIs[i].Constraints.Count; j++)
                {
                    DataRow row = dt.NewRow();
                    row["Structure"] = this.HnPlan.ROIs[i].Name;
                    row["Type"] = this.HnPlan.ROIs[i].Constraints[j].Type;
                    row["Subscript"] = this.HnPlan.ROIs[i].Constraints[j].Subscript;
                    row["Relation"] = this.HnPlan.ROIs[i].Constraints[j].EqualityType;
                    row["Value"] = this.HnPlan.ROIs[i].Constraints[j].Value;
                    row["Format"] = this.HnPlan.ROIs[i].Constraints[j].Format;
                    dt.Rows.Add(row);
                }
            }

            //Add these rows to the view.
            foreach (DataRow DRow in dt.Rows)
            {
                int num = ConstraintGridView.Rows.Add();
                ConstraintGridView.Rows[num].Cells[0].Value = DRow["Structure"].ToString();
                ConstraintGridView.Rows[num].Cells[1].Value = DRow["Type"].ToString();
                ConstraintGridView.Rows[num].Cells[2].Value = DRow["Subscript"].ToString();
                ConstraintGridView.Rows[num].Cells[3].Value = DRow["Relation"].ToString();
                ConstraintGridView.Rows[num].Cells[4].Value = DRow["Value"].ToString();
                ConstraintGridView.Rows[num].Cells[5].Value = DRow["Format"].ToString();
            }
        }

        private void ErrorLabel_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }

        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
            this.HnPlan = hnPlan;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void EditAssignedButton_Click(object sender, EventArgs e)
        {
            //This is the button that brings the panel for editing assigned structures up.
            this.panel1.SendToBack();
            this.panel1.Visible = false;
            this.AssigningPanel.Visible = true;
            this.AssigningPanel.BringToFront();
            //First populate the drop down lists and combo box.

            //Constraint Structure List: 
            //Delete all rows: 
            this.conStructGridView.Rows.Clear();
  
            this.conStructGridView.ForeColor = System.Drawing.Color.Black;
            DataTable dt = new DataTable();
            dt.Columns.Add("Constrained Structure");
            
            //Get all the constraints and make a row for each

            for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
            {

                DataRow row = dt.NewRow();
                row["Constrained Structure"] = this.HnPlan.ROIs[i].Name;               
                dt.Rows.Add(row);               
            }

            //Add these rows to the view.
            foreach (DataRow DRow in dt.Rows)
            {
                int num = conStructGridView.Rows.Add();
                this.conStructGridView.Rows[num].Cells[0].Value = DRow["Constrained Structure"].ToString();
            }
            this.conStructGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            //System.Windows.MessageBox.Show(this.MatchingStructures.Count.ToString());

            //Populate the drop down list with all the dicom structures in the structure set. 
            List<string> StructureNames = new List<string>();

            this.DicomStructures.Clear();
            foreach (Structure structure in this.context.StructureSet.Structures)
            {
                this.DicomStructures.Add(structure);
                StructureNames.Add(structure.Name);
            }
    
            this.DicomComboBox.DataSource = StructureNames;
            
            

            
            
        }

        private void conStructGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void FinishedEdtingButton_Click(object sender, EventArgs e)
        {
            AssigningPanel.Visible = false;
            AssigningPanel.SendToBack();
            panel1.Visible = true;
            panel1.BringToFront();
        }

        private void conStructGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex != -1)&&(e.RowIndex < this.HnPlan.ROIs.Count)) //if not the header or footer
            { 
                this.AssignStructGridView.Rows.Clear();

                this.StructureLabel.Text = e.RowIndex.ToString(); //save which structure is selected for editing
                this.AssignStructGridView.ForeColor = System.Drawing.Color.Black;
                DataTable dt = new DataTable();
                dt.Columns.Add("Assigned Structures");
                for (int i = 0; i < this.MatchingStructures[e.RowIndex].Count; i++)
                {
                    DataRow row = dt.NewRow();
                    row["Assigned Structures"] = this.MatchingStructures[e.RowIndex][i].Name;
                    dt.Rows.Add(row);
                }
                foreach (DataRow DRow in dt.Rows)
                {
                    int num = this.AssignStructGridView.Rows.Add();
                    this.AssignStructGridView.Rows[num].Cells[0].Value = DRow["Assigned Structures"].ToString();
                }
            }
        }

        private void AddStructureButton_Click(object sender, EventArgs e)
        {
            int contourIndex = Convert.ToInt32(this.StructureLabel.Text);
            //Now add this to the assigned structures list. 
            bool alreadyAssigned = false;
            for (int i = 0; i < this.MatchingStructures[contourIndex].Count; i++)
            {
                if (this.MatchingStructures[contourIndex][i].Name.ToLower() == this.DicomStructures[DicomComboBox.SelectedIndex].Name.ToLower())
                {
                    alreadyAssigned = true;
                }
            }
            if (!alreadyAssigned)
            {
                this.MatchingStructures[contourIndex].Add(this.DicomStructures[DicomComboBox.SelectedIndex]);

                //Now repopulate current list. 
                this.AssignStructGridView.Rows.Clear();
                this.StructureLabel.Text = contourIndex.ToString(); //save which structure is selected for editing
                this.AssignStructGridView.ForeColor = System.Drawing.Color.Black;
                DataTable dt = new DataTable();
                dt.Columns.Add("Assigned Structures");
                for (int i = 0; i < this.MatchingStructures[contourIndex].Count; i++)
                {
                    DataRow row = dt.NewRow();
                    row["Assigned Structures"] = this.MatchingStructures[contourIndex][i].Name;
                    dt.Rows.Add(row);
                }
                foreach (DataRow DRow in dt.Rows)
                {
                    int num = this.AssignStructGridView.Rows.Add();
                    this.AssignStructGridView.Rows[num].Cells[0].Value = DRow["Assigned Structures"].ToString();
                }
            }
           

        }

        private void removeAssignedButton_Click(object sender, EventArgs e)
        {
            int contourIndex = Convert.ToInt32(this.StructureLabel.Text);
            int selectedRowCount =AssignStructGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                int selectedIndex = AssignStructGridView.SelectedRows[0].Index;
                this.MatchingStructures[contourIndex].RemoveAt(selectedIndex);
            }
                

            //Now update the lists

            this.AssignStructGridView.Rows.Clear();
          
            this.AssignStructGridView.ForeColor = System.Drawing.Color.Black;
            DataTable dt = new DataTable();
            dt.Columns.Add("Assigned Structures");
            for (int i = 0; i < this.MatchingStructures[contourIndex].Count; i++)
            {
                DataRow row = dt.NewRow();
                row["Assigned Structures"] = this.MatchingStructures[contourIndex][i].Name;
                dt.Rows.Add(row);
            }
            foreach (DataRow DRow in dt.Rows)
            {
                int num = this.AssignStructGridView.Rows.Add();
                this.AssignStructGridView.Rows[num].Cells[0].Value = DRow["Assigned Structures"].ToString();
            }
           
        }

        private void LoadFeaturesButton_Click(object sender, EventArgs e)
        {
            this.PanelSpecialFeatures.Visible = true;
            this.PanelSpecialFeatures.BringToFront();
            this.panel1.Visible = false;
            this.panel1.SendToBack();
        }

        private void ButtonDoneFeatures_Click(object sender, EventArgs e)
        {
            this.PanelSpecialFeatures.Visible = false;
            this.PanelSpecialFeatures.SendToBack();
            this.panel1.Visible = true;
            this.panel1.BringToFront();
        }
    }
}
