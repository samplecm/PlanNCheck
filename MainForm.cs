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
        public ScriptContext context { get; set; }
        public MainForm(ref ScriptContext contextIn)
        {
            InitializeComponent();
            this.context = contextIn;
            this.Plans = new List<Plan>();
            
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
            else
            {
                VMS.TPS.Script.StartOptimizer(this.context);
                Calculator.RunReport(this.context, this.Plans, this.SavePath);


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
            }
        
            panel1.Visible = true;
            PopulateGrid();

        }



        

        private void OKButton_Click(object sender, EventArgs e)
        {
            
            panel1.Visible = false;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //add constraint
        {
            if ((String.IsNullOrEmpty(StructureTB.Text)) || (String.IsNullOrEmpty(TypeTB.Text)) || (String.IsNullOrEmpty(SubscriptTB.Text)) || (String.IsNullOrEmpty(RelationTB.Text)) || (String.IsNullOrEmpty(ValueTB.Text)) || (String.IsNullOrEmpty(FormatTB.Text))) 
            {               
                AddLabel.Visible = true;
            }
            else
            {
                AddLabel.Text = "Constraint Added";
                AddLabel.Visible = true;
                
                //First check if structure exists: 
                bool structExists = false;
                for (int i = 0; i < this.Plans[0].ROIs.Count; i++)
                {
                    if (this.Plans[0].ROIs[i].Name == StructureTB.Text.ToString()) //if structure already exists
                    {
                        structExists = true;
                        Constraint newConstraint = new Constraint(TypeTB.Text.ToString(), SubscriptTB.Text.ToString(), RelationTB.Text.ToString(), Convert.ToDouble(ValueTB.Text.ToString()), FormatTB.Text.ToString());
                        this.Plans[0].ROIs[i].Constraints.Add(newConstraint);
                        break;
                    }
                }
                if (structExists == false)
                {
                    ROI NewROI = new ROI();
                    NewROI.Name = StructureTB.Text.ToString();
                    NewROI.Constraints.Add(new Constraint(TypeTB.Text.ToString(), SubscriptTB.Text.ToString(), RelationTB.Text.ToString(), Convert.ToDouble(ValueTB.Text.ToString()), FormatTB.Text.ToString()));
                    
                    this.Plans[0].ROIs.Add(NewROI);
                }
            }
            PopulateGrid();
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
            for (int i = 0; i < this.Plans[0].ROIs.Count; i++)
                {
                    

                    if (this.Plans[0].ROIs[i].Name == item.Cells[0].Value.ToString()) //if the right structure
                    {

                        for (int j = 0; j < Plans[0].ROIs[i].Constraints.Count; j++) //Look to get the right constraint
                        {

                            if ((this.Plans[0].ROIs[i].Constraints[j].Type == item.Cells[1].Value.ToString()) && (this.Plans[0].ROIs[i].Constraints[j].Subscript == item.Cells[2].Value.ToString()) && (this.Plans[0].ROIs[i].Constraints[j].EqualityType == item.Cells[3].Value.ToString()))
                            {                             
                                int numConstraints = this.Plans[0].ROIs[i].Constraints.Count;
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
                this.Plans[0].ROIs[ROI_Index].Constraints.RemoveAt(constraint_index);
                if (removeROI == true)
                {
                    this.Plans[0].ROIs.RemoveAt(ROI_Index);
                    
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
                for (int i = numRows - 2; i > 0; i--)
                {
                    try
                    {
                        ConstraintGridView.Rows.RemoveAt(i);
                    }
                    catch
                    {
                    }

                }
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

            for (int i = 0; i < this.Plans[0].ROIs.Count; i++)
            {
                for (int j = 0; j < this.Plans[0].ROIs[i].Constraints.Count; j++)
                {
                    DataRow row = dt.NewRow();
                    row["Structure"] = this.Plans[0].ROIs[i].Name;
                    row["Type"] = this.Plans[0].ROIs[i].Constraints[j].Type;
                    row["Subscript"] = this.Plans[0].ROIs[i].Constraints[j].Subscript;
                    row["Relation"] = this.Plans[0].ROIs[i].Constraints[j].EqualityType;
                    row["Value"] = this.Plans[0].ROIs[i].Constraints[j].Value;
                    row["Format"] = this.Plans[0].ROIs[i].Constraints[j].Format;
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
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            HNPlan hnPlan = new HNPlan(context.PlanSetup.TotalDose.Dose);
            plans.Add(hnPlan);
            this.Plans = plans;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
