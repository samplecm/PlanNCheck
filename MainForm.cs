using System;
using System.Linq;
using System.Data;
using System.Text;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using VMS.TPS;
using Plan_n_Check;
using Plan_n_Check.Plans;
using Plan_n_Check.Calculate;
using Plan_n_Check.Features;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

namespace Plan_n_Check
{
    public partial class MainForm : Form
    {
        public string SavePath { get; set; }
        public List<Plan> Plans { get; set; }
        public HNPlan HnPlan { get; set; }
        public List<List<Structure>> MatchingStructures { get; set; }
        public ScriptContext context { get; set; }

        public List<Structure> DicomStructures { get; set; }
        public List<Tuple<bool, double[], string>> Features { get; set; }

        public OxyPlot.WindowsForms.PlotView PV { get; set; }
        public Stopwatch TotalTime { get; set; }
        public Stopwatch OptimTime { get; set; }

        public List<Tuple<ROI, int, int, int, int>> DVH_ReportStructures {get; set;} //Structure, lowerDoseBound, upperDoseBound, lowerVolumeBound, upperVolumeBound 




        public MainForm(ref ScriptContext contextIn)
        {
            InitializeComponent();
            this.context = contextIn;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Location = new System.Drawing.Point(0, 0);
            this.Plans = new List<Plan>();
            this.MatchingStructures = new List<List<Structure>>();
            this.DicomStructures = new List<Structure>();
            this.Features = new List<Tuple<bool, double[], string>>();
            this.TotalTime = new Stopwatch();
            this.TotalTime.Start();
            this.OptimTime = new Stopwatch();
            this.DVH_ReportStructures = new List<Tuple<ROI, int, int, int, int>>();
            


            
            
        }

        private string GetPath(String name)
        {
            var sfd = new SaveFileDialog
            {
                Title = "Choose Save Location",
                Filter = "PDF Files (*.pdf)|*.pdf",
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
            OxyPlot.WindowsForms.PlotView pv = new OxyPlot.WindowsForms.PlotView();
            pv.Location = new System.Drawing.Point(30, 60);
            pv.Size = new System.Drawing.Size(650, 380);
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
            }
                );
            this.PV = pv;
            this.Controls.Add(this.PV);
            this.PlotPanel.Controls.Add(this.PV);

            List<string> listStructures = new List<string>();
            foreach (Structure structure in this.context.StructureSet.Structures)
            {
                listStructures.Add(structure.Name);

            }

            this.PlotCombobox.DataSource = listStructures;

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
            if ((this.SavePath == "")&&(this.SaveCheck.Checked))
            {
                StartErrorLabel.Text = "Please select a save destination.";
                StartErrorLabel.Visible = true;
            }
            else if (this.Plans.Count == 0)
            {
                StartErrorLabel.Text = "Please select a type of plan to create.";
            }
            //else if ((!int.TryParse(this.IterationsTextBox.Text, out int value))||(this.IterationsTextBox.Text == "0")) //if numints is not an integer or 0
            //{
            //    StartErrorLabel.Text = "Number of iterations must be an integer greater than zero.";
            //}
            else
            {
                int iterations = Convert.ToInt32(this.IterationsTextBox.Text);
                //Check which special optimization features to include

                //Parotid Seg:
                if (CheckBox_ChopParotid.Checked)
                {
                    this.Features.Add(Tuple.Create(true, new double[1] { Convert.ToDouble(this.PriorityRatio_TextBox.Text) }, ""));
                }else
                {
                    this.Features.Add(Tuple.Create(false, new double[1] { Convert.ToDouble(this.PriorityRatio_TextBox.Text) }, ""));
                }
                //check if want jaw tracking
                bool jawTracking;
                if (this.checkBoxJawTracking.Checked)
                {
                    jawTracking = true;
                }
                else
                { 
                    jawTracking = false; 
                }

                
                this.StartErrorLabel.Text = "In progress";
                this.StartErrorLabel.Visible = true;
                this.OptimTime.Start();
                var optimData = VMS.TPS.Script.StartOptimizer(this.context, this.HnPlan, this.MatchingStructures, iterations, this.Features, jawTracking);
                List<List<Structure>> optimizedStructures = optimData.Item1;
                List<List<Structure>> matchingStructures = optimData.Item2;
                List<List<string>> updateLog = optimData.Item3;
                bool isPassed = optimData.Item4;
                if (SaveCheck.Checked)
                {
                    Check.RunReport(this.context, this.HnPlan, this.SavePath, this.MatchingStructures, optimizedStructures, updateLog, this.DVH_ReportStructures, isPassed);
                }
                this.StartErrorLabel.Visible = false;
                this.TotalTime.Stop();
                this.OptimTime.Stop();

                this.PlotPanel.Visible = true;
                this.PlotPanel.BringToFront();
                double totalMins = this.TotalTime.Elapsed.TotalMinutes;
                double totalSeconds = this.TotalTime.Elapsed.TotalSeconds;
                double optimMins = this.OptimTime.Elapsed.TotalMinutes;
                double optimSeconds = this.OptimTime.Elapsed.TotalSeconds;
                System.Windows.MessageBox.Show("Total time elapsed: " + totalSeconds.ToString() + " seconds");
                //System.Windows.MessageBox.Show("Optimization time elapsed: " + optimSeconds.ToString() + " seconds");

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
               
                List<Plan> plans = new List<Plan>();
                double prescriptionDose = (double)context.PlanSetup.TotalDose.Dose;
                int numFractions = (int)context.PlanSetup.NumberOfFractions;
                HNPlan hnPlan = new HNPlan(prescriptionDose, numFractions);
                plans.Add(hnPlan);
                this.Plans = plans;
                this.HnPlan = hnPlan;
                this.MatchingStructures.Clear();
                for (int i = 0; i < hnPlan.ROIs.Count; i++)
                {
                    List<Structure> assignedStructures = StringOperations.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                    this.MatchingStructures.Add(assignedStructures);
                }
                //Also check for special prescription PTVs (not 56, 63, or 70 for HNPlan) and find maximum PTV prescription, which if less than the prescription dose
                //will be used to adjust all PTV maximum dose constraints, and maximum body constraint.
                //First get max ptv does for upper ptv constraint
                int ptvType;
                int maxPTV_Dose = 0;
                foreach (Structure structure in context.StructureSet.Structures)
                {
                    if (structure.Name.ToLower().Contains("ptv"))
                    {
                        ptvType = StringOperations.FindPTVNumber(structure.Name.ToLower());
                        if (ptvType > maxPTV_Dose)
                        {
                            maxPTV_Dose = ptvType;
                        }
                    }
                }

                
                foreach (Structure structure in context.StructureSet.Structures)
                {
                    if (structure.Name.ToLower().Contains("ptv"))
                    {
                        ptvType = StringOperations.FindPTVNumber(structure.Name.ToLower());
                        
                        //Check if standard prescription dose type;
                        bool isStandard = this.HnPlan.PTV_Types.IndexOf(ptvType) != -1;
                        if ((!isStandard)&&(ptvType != 0))
                        {
                            //Make a new constraint
                            string Name = "PTV" + ptvType.ToString();
                            ROI newPTV = new ROI();
                            newPTV.Name = Name;
                            newPTV.IsPTV = true;
                            newPTV.PTVDose = ptvType * 100;
                            newPTV.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 110, new List<int> {80, 120}));
                            newPTV.Constraints.Add(new Constraint("D", "max", "<", 1.1* maxPTV_Dose * 100, "abs", 100, new List<int> { 80, 120 }));
                            this.HnPlan.ROIs.Add(newPTV);
                            this.MatchingStructures.Add(new List<Structure>() { structure }); //Constraints will be updated according to this
                        }
                    }
                }
                List<string> TypeList = new List<string>();
                TypeList.Add("D");
                TypeList.Add("V");
                this.Combobox_Type.DataSource = TypeList;

                List<string> RelationList = new List<string>();
                RelationList.Add("<");
                RelationList.Add(">");
                this.Combobox_Relation.DataSource = RelationList;

                List<string> FormatList = new List<string>();
                FormatList.Add("Abs");
                FormatList.Add("Rel");
                this.Combobox_Format.DataSource = FormatList;






                this.panel5.Visible = false;
                this.panel5.SendToBack();
                this.panel1.BringToFront();
                this.ErrorLabel.Visible = false;
                this.panel1.Visible = true;
                PopulateGrid();
            }
        
            

        }



        

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.panel5.BringToFront();
            this.panel5.Visible = true;
            this.panel1.Visible = false;
            this.panel1.SendToBack();
                       
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            List<Plan> plans = new List<Plan>();
            double prescriptionDose = (double)context.PlanSetup.TotalDose.Dose;
            int numfractions = (int)context.PlanSetup.NumberOfFractions;
            HNPlan hnPlan = new HNPlan(prescriptionDose, numfractions);
            plans.Add(hnPlan);
            this.HnPlan = hnPlan;
            this.Plans = plans;
            this.MatchingStructures.Clear();
            for (int i = 0; i < hnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = StringOperations.AssignStructure(context.StructureSet, hnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //add constraint. First click (if dialog result no) brings up constraint parameter boxes, second click (dialog result yes) adds the constraint
        {

            if (this.button3.DialogResult == DialogResult.No)
            {
                //add all parameter labels/entry ports
                this.label5.Visible = true;
                this.TypeLabel.Visible = true;
                this.SubscriptLabel.Visible = true;
                this.RelationLabel.Visible = true;
                this.FormatLabel.Visible = true;
                this.ValueLabel.Visible = true;
                this.StructureTB.Visible = true;
                this.Combobox_Type.Visible = true;
                this.Combobox_Relation.Visible = true;
                this.Combobox_Format.Visible = true;
                this.SubscriptTB.Visible = true;
                this.checkBox_OAR.Visible = true;
                //update the add button
                this.button3.BackColor = System.Drawing.Color.MediumSeaGreen;
                this.button3.Text = "Add";
                this.button3.DialogResult = DialogResult.Yes;
                //update the cancel button
                this.DeleteButton.Text = "Cancel";
                this.DeleteButton.BackColor = System.Drawing.Color.DarkRed;
                this.DeleteButton.DialogResult = DialogResult.Yes;
                return;
            }
            else
            {

                if ((String.IsNullOrEmpty(this.StructureTB.Text)) || (String.IsNullOrEmpty(this.SubscriptTB.Text)) ||
                    (String.IsNullOrEmpty(this.ValueTB.Text)))
                {
                    this.AddLabel.Visible = true;
                    return;

                }
                else if ((this.Combobox_Type.SelectedIndex == -1) || (this.Combobox_Relation.SelectedIndex == -1) || (this.Combobox_Format.SelectedIndex == -1))
                {
                    return;
                }
                else if (!Double.TryParse(ValueTB.Text, out double _))
                {
                    this.AddLabel.Text = "Invalid value, must be a number.";
                    this.AddLabel.Visible = true;
                    return;

                }
                else if ((SubscriptTB.Text.ToLower() != "max") && (SubscriptTB.Text.ToLower() != "min") && (SubscriptTB.Text.ToLower() != "mean") &&
                    !(Double.TryParse(SubscriptTB.Text, out double _)))
                {
                    this.AddLabel.Text = "Invalid subscript.";
                    this.AddLabel.Visible = true;
                    return;
                }
                else
                {


                    this.AddLabel.Text = "Constraint Added";
                    this.AddLabel.Visible = true;

                    bool isOAR = this.checkBox_OAR.Checked;
                    int priority = 50; //start with a priority of 50 for OARs and shift it to 100 if it is a target volume
                    List<int> priorityRange = new List<int>() { 0, 70 };
                    if (!isOAR)
                    {
                        priority = 100;
                        priorityRange = new List<int>() { 70, 120 };
                    }
                    var type = this.Combobox_Type.SelectedItem.ToString();
                    var subscript = SubscriptTB.Text.ToString();
                    var relation = this.Combobox_Relation.SelectedItem.ToString();
                    var value = Convert.ToDouble(ValueTB.Text.ToString());
                    var format = this.Combobox_Format.SelectedItem.ToString();

                    //First check if structure exists: 
                    bool structExists = false;
                    for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
                    {
                        if (this.HnPlan.ROIs[i].Name == StructureTB.Text.ToString()) //if structure already exists
                        {
                            structExists = true;
                            Constraint newConstraint = new Constraint(type, subscript, relation, value, format, priority, priorityRange);
                            this.HnPlan.ROIs[i].Constraints.Add(newConstraint);
                            break;
                        }
                    }
                    if (structExists == false)
                    {
                        ROI NewROI = new ROI();
                        NewROI.Name = StructureTB.Text.ToString();
                        NewROI.Constraints.Add(new Constraint(type, subscript, relation, value, format, priority, priorityRange));

                        this.HnPlan.ROIs.Add(NewROI);
                        //Need to also add assigned structure for this
                        List<Structure> assignedStructures = StringOperations.AssignStructure(context.StructureSet, NewROI); //find structures that match the constraint structure
                        this.MatchingStructures.Add(assignedStructures);
                    }


                }
                PopulateGrid();
                this.Combobox_Type.SelectedIndex = -1;
                this.ValueTB.Text = "";
                this.Combobox_Relation.SelectedIndex = -1;
                this.Combobox_Format.SelectedIndex = -1;
                this.Combobox_Type.SelectedIndex = -1;

                //update the add button
                this.button3.BackColor = System.Drawing.Color.SteelBlue;
                this.button3.Text = "New Constraint";
                this.button3.DialogResult = DialogResult.No;
                //update the cancel button
                this.DeleteButton.Text = "Delete Selection";
                this.DeleteButton.BackColor = System.Drawing.Color.SteelBlue;
                this.DeleteButton.DialogResult = DialogResult.No;

                this.label5.Visible = false;
                this.TypeLabel.Visible = false;
                this.SubscriptLabel.Visible = false;
                this.RelationLabel.Visible = false;
                this.FormatLabel.Visible = false;
                this.ValueLabel.Visible = false;
                this.StructureTB.Visible = false;
                this.Combobox_Type.Visible = false;
                this.Combobox_Relation.Visible = false;
                this.Combobox_Format.Visible = false;
                this.SubscriptTB.Visible = false;
                this.checkBox_OAR.Visible = false;
            }


        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (this.DeleteButton.DialogResult == DialogResult.Yes)
            {
                //update the add button
                this.button3.BackColor = System.Drawing.Color.SteelBlue;
                this.button3.Text = "New Constraint";
                this.button3.DialogResult = DialogResult.No;
                //update the cancel button
                this.DeleteButton.Text = "Delete Selection";
                this.DeleteButton.BackColor = System.Drawing.Color.SteelBlue;
                this.DeleteButton.DialogResult = DialogResult.No;

                this.label5.Visible = false;
                this.TypeLabel.Visible = false;
                this.SubscriptLabel.Visible = false;
                this.RelationLabel.Visible = false;
                this.FormatLabel.Visible = false;
                this.ValueLabel.Visible = false;
                this.StructureTB.Visible = false;
                this.Combobox_Type.Visible = false;
                this.Combobox_Relation.Visible = false;
                this.Combobox_Format.Visible = false;
                this.SubscriptTB.Visible = false;
                this.checkBox_OAR.Visible = false;
                return;
            }

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
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            

        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Call method for starting report. 
            if ((this.SavePath == "") && (this.SaveCheck.Checked))
            {
                StartErrorLabel.Text = "Please select a save destination.";
                StartErrorLabel.Visible = true;
            }
            else if (this.Plans.Count == 0)
            {
                StartErrorLabel.Text = "Please select what type of plan you wish to create a report for.";
            }
            else
            {
                //Check which special optimization features to include

                //Parotid Seg:
                if (CheckBox_ChopParotid.Checked)
                {
                    this.Features.Add(Tuple.Create(true, new double[1] { Convert.ToDouble(this.PriorityRatio_TextBox.Text) }, ""));
                }
                else
                {
                    this.Features.Add(Tuple.Create(false, new double[1] { Convert.ToDouble(this.PriorityRatio_TextBox.Text) }, ""));
                }

                this.StartErrorLabel.Text = "In progress";
                this.StartErrorLabel.Visible = true;
                this.OptimTime.Start();
                var structureLists = VMS.TPS.Script.PrepareCheck(this.context, this.HnPlan, this.MatchingStructures, this.Features);
                List<List<Structure>> optimizedStructures = structureLists.Item1;
                List<List<Structure>> matchingStructures = structureLists.Item2;
                List<List<string>> updateLog = structureLists.Item3;
                if (SaveCheck.Checked)
                {
                    Check.RunReport(this.context, this.HnPlan, this.SavePath, this.MatchingStructures, optimizedStructures, updateLog, this.DVH_ReportStructures , true);
                }
                this.StartErrorLabel.Visible = false;
                this.TotalTime.Stop();
                this.OptimTime.Stop();

                this.PlotPanel.Visible = true;
                this.PlotPanel.BringToFront();
                System.Windows.MessageBox.Show("Report Saved");
               
            }
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
            //First see if this is first time that special features panel has been opened:
            if (!SpecialFeatures_Clicked.Checked) //If it hasn't been selected
            {
                SpecialFeatures_Clicked.CheckState = CheckState.Checked;
                //populate the combo boxes
                this.OrganSeg_OrgansCombo.DataSource = new List<string>();
                List<string> organNames = new List<string>();
                foreach (Structure s in this.context.StructureSet.Structures)
                {
                    organNames.Add(s.Name);
                }
                this.OrganSeg_OrgansCombo.DataSource = organNames;

                List<int> axial_sliceNums = new List<int>();
                List<int> coronal_sliceNums = new List<int>();
                List<int> sagittal_sliceNums = new List<int>();
                //Get options 0 through 10
                for (int i = 0; i <= 10; i++)
                {
                    axial_sliceNums.Add(i);
                    coronal_sliceNums.Add(i);
                    sagittal_sliceNums.Add(i);
                }
                this.Axial_Combobox.DataSource = axial_sliceNums;
                this.Sagittal_Combobox.DataSource = sagittal_sliceNums;
                this.Coronal_Combobox.DataSource = coronal_sliceNums;
                this.panel1.Visible = false;
                this.panel1.SendToBack();

                //Populate the dvh grid and combobox




                //Delete all grid rows and add the headers: 
                int numRows = this.DVH_gridView.Rows.Count;
                if (numRows > 1)
                {
                    for (int i = numRows - 1; i > 0; i--)
                    {
                        try
                        {
                            this.DVH_gridView.Rows.RemoveAt(i);
                        }
                        catch
                        {
                        }

                    }
                    this.DVH_gridView.Rows.Clear();
                }
                this.DVH_gridView.ForeColor = System.Drawing.Color.Black;
                DataTable dt = new DataTable();
                dt.Columns.Add("Structure");
                dt.Columns.Add("Volume Bounds");
                dt.Columns.Add("Dose Bounds");
                //By default include the PTV DVHs in the report:

                for (int j = 0; j < this.HnPlan.ROIs.Count; j++)
                {
                    string name = this.HnPlan.ROIs[j].Name;
                    if (name.ToLower().Contains("ptv"))
                    {
                        DataRow row = dt.NewRow();
                        row["Structure"] = name;
                        row["Volume Bounds"] = "[0,115]";
                        row["Dose Bounds"] = "[0,115]";
                        dt.Rows.Add(row);
                    }                  
                }

                //Add these rows to the view.
                foreach (DataRow DRow in dt.Rows)
                {
                    int num = this.DVH_gridView.Rows.Add();
                    this.DVH_gridView.Rows[num].Cells[0].Value = DRow["Structure"].ToString();
                    this.DVH_gridView.Rows[num].Cells[1].Value = DRow["Volume Bounds"].ToString();
                    this.DVH_gridView.Rows[num].Cells[2].Value = DRow["Dose bounds"].ToString();
                    
                }

                //Now make a list of all planning structures and then add to combobox
                List<string> tempList = new List<string>();
                for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
                {
                    tempList.Add(this.HnPlan.ROIs[i].Name);

                }
                this.Combobox_dvhReport.DataSource = tempList;
                this.Combobox_dvhReport.SelectedIndex = -1;

              
            }
            foreach (DataGridViewRow row in this.DVH_gridView.Rows) //Add the default PTV DVHs to the DVH list
            {
                if (row.Cells[0].Value == null)
                {
                    break;
                }
                string structureName = row.Cells[0].Value.ToString().ToLower();
                for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
                {
                    string tempName = this.HnPlan.ROIs[i].Name.ToLower();
                    if (tempName == structureName)
                    {
                        this.DVH_ReportStructures.Add(Tuple.Create(this.HnPlan.ROIs[i], 0, 115, 0, 115));
                    }
                }


            }
        
        }

        private void ButtonDoneFeatures_Click(object sender, EventArgs e)
        {
            //Check the DVH report list and add all desired DVHs to list
            
            this.PanelSpecialFeatures.Visible = false;
            this.PanelSpecialFeatures.SendToBack();
            this.panel1.Visible = true;
            this.panel1.BringToFront();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void RelationTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void DicomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void helpButtonConstraints_Click(object sender, EventArgs e)
        {
            this.LatexPanel.BringToFront();
            this.LatexPanel.Visible = true;

        }

        private void DoneLatexButton_Click(object sender, EventArgs e)
        {
            this.LatexPanel.SendToBack();
            this.LatexPanel.Visible = false;
        }

        private void PlotDoneButton_Click(object sender, EventArgs e)
        {
            this.PlotPanel.Visible = false;
            this.PlotPanel.SendToBack();
        }

        private void PlotButton_Click(object sender, EventArgs e)
        {
            this.PlotPanel.Controls.Remove(this.PV);
            this.Controls.Remove(this.PV);
            this.Refresh();

            OxyPlot.WindowsForms.PlotView pv = new OxyPlot.WindowsForms.PlotView();
            pv.Location = new System.Drawing.Point(30, 60);
            pv.Size = new System.Drawing.Size(650, 380);
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
            }
                );
            this.PV = pv;
            this.Controls.Add(this.PV);
            this.PlotPanel.Controls.Add(this.PV);

            MakePlot();
            

        }

        private void PlotFormButton_Click(object sender, EventArgs e)
        {
            this.PlotPanel.Visible = true;
            this.PlotPanel.BringToFront();
            this.PlotPanel.Controls.Remove(this.PV);
            this.Controls.Remove(this.PV);
            this.Refresh();

            OxyPlot.WindowsForms.PlotView pv = new OxyPlot.WindowsForms.PlotView();
            pv.Location = new System.Drawing.Point(30, 60);
            pv.Size = new System.Drawing.Size(650, 380);
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
            }
                );
            this.PV = pv;
            this.Controls.Add(this.PV);
            this.PlotPanel.Controls.Add(this.PV);
            MakePlot();
        }
        private void MakePlot()
        {
            if (this.PlotCombobox.SelectedIndex == -1)
            {
                return;
            }
            string structureName = this.PlotCombobox.SelectedItem.ToString();
            Structure plotStructure = this.context.StructureSet.Structures.First();
            foreach (Structure structure in this.context.StructureSet.Structures)
            {
                if (structure.Name == structureName) {
                    plotStructure = structure;
                }
            }
        
            var dvh = DVHMaker.CalculateDVH(this.context.PlanSetup, plotStructure);
            
            var series = DVHMaker.CreateDVHSeries(dvh);
            
            this.PV.Model.Series.Add(series);
            this.Controls.Add(this.PV);
            this.PlotPanel.Controls.Add(this.PV);

            

            


        }
        
        private void Button_StartSegmentation_Click(object sender, EventArgs e)
        {
            ExternalPlanSetup plan = this.context.ExternalPlanSetup;
            StructureSet ss = this.context.StructureSet;
            //Organ Seg:
            string organName = (string)this.OrganSeg_OrgansCombo.SelectedItem;
            Structure organ = this.context.StructureSet.Structures.First();
            bool applyConstraints = false;
            if (this.CheckboxSegmentConstraints.Checked)
            {
                applyConstraints = true;

            }
            foreach (Structure s in this.context.StructureSet.Structures)
            {
                if (organName == s.Name)
                {
                    organ = s;
                }
            }
            double[] organSegOptions = new double[] { Convert.ToDouble(this.Axial_Combobox.SelectedItem), Convert.ToDouble(this.Coronal_Combobox.SelectedItem), Convert.ToDouble(this.Sagittal_Combobox.SelectedItem) };
            this.Features.Add(Tuple.Create(true, organSegOptions, organName));
            //Need to update the hnplan structure so it knows that the structure has subsegments:
            HNPlan hnPlan = this.HnPlan;
            Segmentation.MakeSubsegmentStructures(organ, organSegOptions, ref plan, ref ss, this.context, applyConstraints, this.MatchingStructures, ref hnPlan);
            //update the hnplan property of the class
            this.HnPlan = hnPlan;

        }

        private void Button_DeleteSubsegments_Click(object sender, EventArgs e)
        {
            int numDeleted = 0;
            Structure organ = this.context.StructureSet.Structures.First();
            foreach (Structure s in this.context.StructureSet.Structures)
            {
                if ((string)this.OrganSeg_OrgansCombo.SelectedItem == s.Name)
                {
                    organ = s;
                }
            }
            foreach (Structure s2 in this.context.StructureSet.Structures.ToList())
            {
                int min = Math.Min(6, organ.Name.Length-1);
                string subName = organ.Name.Substring(0, min);
                if (s2.Name.Contains(subName + "_subseg"))
                {
                    this.context.StructureSet.RemoveStructure(s2);
                    numDeleted++;
                }
            }
            if (numDeleted > 0)
            {
                System.Windows.MessageBox.Show("Deleted " + numDeleted + " subsegment structures");
            }
        }

        private void ButtonDeleteParotidSub_Click(object sender, EventArgs e)
        {
            int numDeleted = 0;
          
            foreach (Structure s in this.context.StructureSet.Structures.ToList())
            {
                if (s.Name.ToLower().Contains("cpg_subseg"))
                {
                    this.context.StructureSet.RemoveStructure(s);
                    numDeleted++;
                }
            }
            if (numDeleted > 0)
            {
                System.Windows.MessageBox.Show("Deleted " + numDeleted + " subsegment structures");
            }
        }

        private void Button_AddDVH_Report_Click(object sender, EventArgs e)
        {
            string newDVH = this.Combobox_dvhReport.SelectedItem.ToString().ToLower();
            //Now get all the DVH bounds
            int doseLowerBound;
            int doseUpperBound;
            int volumeUpperBound;
            int volumeLowerBound;
            if (!int.TryParse(this.TextBox_Dose_lb.Text, out doseLowerBound))
            {
                System.Windows.MessageBox.Show("Please ensure that all DVH bounds are integers");
                return;
            }
            if (!int.TryParse(this.TextBox_Dose_ub.Text, out doseUpperBound))
            {
                System.Windows.MessageBox.Show("Please ensure that all DVH bounds are integers");
                return;
            }
            if (!int.TryParse(this.TextBox_Volume_lb.Text, out volumeLowerBound))
            {
                System.Windows.MessageBox.Show("Please ensure that all DVH bounds are integers");
                return;
            }
            if (!int.TryParse(this.TextBox_Volume_ub.Text, out volumeUpperBound))
            {
                System.Windows.MessageBox.Show("Please ensure that all DVH bounds are integers");
                return;
            }


            //Make sure upper bounds greater than lower bounds: 
            if ((doseLowerBound > doseUpperBound)||(volumeLowerBound > volumeUpperBound))
            {
                System.Windows.MessageBox.Show("Please ensure that upper DVH bounds exceed lower bounds");
            }

            //First check if it's already in the list: 
            foreach (DataGridViewRow r in this.DVH_gridView.Rows)
            {
                if (r.Cells[0].Value == null) //avoid error of checking last blank cell 
                {
                    break;
                }
                string tempName = r.Cells[0].Value.ToString().ToLower();
                if (newDVH.ToLower() == tempName)
                {
                    System.Windows.MessageBox.Show("DVH has already been included in the list.");
                    return;
                }
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("Structure");
            dt.Columns.Add("Volume bounds");
            dt.Columns.Add("Dose Bounds");
            DataRow row = dt.NewRow();
            row["Structure"] = newDVH;
            row["Volume Bounds"] = "[" + volumeLowerBound + "," + volumeUpperBound + "]";
            row["Dose Bounds"] = "[" + doseLowerBound + "," + doseUpperBound + "]";
            dt.Rows.Add(row);

            //Add to the gridview
            int num = this.DVH_gridView.Rows.Add();
            this.DVH_gridView.Rows[num].Cells[0].Value = dt.Rows[0]["Structure"].ToString();
            this.DVH_gridView.Rows[num].Cells[1].Value = dt.Rows[0]["Volume Bounds"].ToString();
            this.DVH_gridView.Rows[num].Cells[2].Value = dt.Rows[0]["Dose Bounds"].ToString();


            ////Add these rows to the view.
            //foreach (DataRow DRow in dt.Rows)
            //{
            //    int num = ConstraintGridView.Rows.Add();
            //    ConstraintGridView.Rows[num].Cells[0].Value = DRow["Structure"].ToString();
            //    ConstraintGridView.Rows[num].Cells[1].Value = DRow["Type"].ToString();
            //    ConstraintGridView.Rows[num].Cells[2].Value = DRow["Subscript"].ToString();
            //    ConstraintGridView.Rows[num].Cells[3].Value = DRow["Relation"].ToString();
            //    ConstraintGridView.Rows[num].Cells[4].Value = DRow["Value"].ToString();
            //    ConstraintGridView.Rows[num].Cells[5].Value = DRow["Format"].ToString();
            //}
            for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
            {
                string tempName = this.HnPlan.ROIs[i].Name.ToLower();
                if (tempName == newDVH)
                {
                    this.DVH_ReportStructures.Add(Tuple.Create(this.HnPlan.ROIs[i], doseLowerBound, doseUpperBound, volumeLowerBound, volumeUpperBound));
                }
            }
            //Reset the bounds
            this.TextBox_Dose_lb.Text = "0";
            this.TextBox_Dose_ub.Text = "100";
            this.TextBox_Volume_lb.Text = "0";
            this.TextBox_Volume_ub.Text = "100";


        }

        private void Button_ParamOptStart_Click(object sender, EventArgs e)
        //This function produces a csv containing information of which optimization parameters are best for different plan outcome priorities. 
        {
            //first need to open a save dialog to choose where to save the csv 
            String path = GetOptParamsCSVPath();
            if (String.IsNullOrEmpty(path))
            {
                System.Windows.MessageBox.Show("Invalid path chosen");
                return;
            }
            this.TotalTime.Start();
            //Parotid Seg:
            if (CheckBox_ChopParotid.Checked)
            {
                this.Features.Add(Tuple.Create(true, new double[1] { Convert.ToDouble(this.PriorityRatio_TextBox.Text) }, ""));
            }
            else
            {
                this.Features.Add(Tuple.Create(false, new double[1] { Convert.ToDouble(this.PriorityRatio_TextBox.Text) }, ""));
            }

            /*The ROIs that we will be iterating priorities on are:
             * 
             * PTV 70 (80, 100, 120)
             * 
             * (Group):
             * PTV 63 (80, 100, 120)
             * PTV 56 (80, 100, 120)
             * 
             * (Group):
             * Spinal/Spinal PRV (90, 105, 120)
             * Brainstem/ Spinal PRV (90, 105, 120)
             */
            //
            List<int> PTV70Priorities = new List<int>() { 100 };
            List<int> PTVOtherPriorities = new List<int>() { 100 };
            List<int> nervousPriorities = new List<int>() { 100 };
            //List<int> PTV70Priorities = new List<int>() { 80, 100, 120 };
            //List<int> PTVOtherPriorities = new List<int>() { 80, 100, 120 };
            //List<int> nervousPriorities = new List<int>() { 90, 105, 120 };
            int numIterations = PTV70Priorities.Count * PTVOtherPriorities.Count * nervousPriorities.Count;

            //Create a list of tuples containing the planning data
            List<Tuple<bool, List<bool>, List<List<List<double>>>>> CombinedPlanData = new List<Tuple<bool, List<bool>, List<List<List<double>>>>>();

            List<Tuple<int, Dictionary<string, double>>> passedPlans = new List<Tuple<int, Dictionary<string, double>>>(); //to hold passed plans, with their plan number, and dictionary containing ROI dose measures used for choosing best plan
            int currentIter = 1;
            for (int ptv70_idx = 0; ptv70_idx < PTV70Priorities.Count; ptv70_idx++)
            {
                for (int ptvOther_idx = 0; ptvOther_idx < PTVOtherPriorities.Count; ptvOther_idx++)
                {
                    for (int nervous_idx = 0; nervous_idx < nervousPriorities.Count; nervous_idx++)
                    {
                        currentIter++;
                        //create a plan 
                        HNPlan plan = new HNPlan(7000, 35);

                        //Set the priorities
                        for (int i = 0; i < 4; i++)//set nervous priorities
                        {
                            for (int j = 0; j < plan.ROIs[i].Constraints.Count; j++)
                            {
                                plan.ROIs[i].Constraints[j].Priority = nervousPriorities[nervous_idx];
                            }
                        }

                        for (int i = 0; i < 2; i++)//set ptV70 priority
                        {
                            plan.ROIs[4].Constraints[i].Priority = PTV70Priorities[ptv70_idx];
                        }

                        for (int i = 5; i < 7; i++)//set the ptv other priorities
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                plan.ROIs[i].Constraints[j].Priority = PTVOtherPriorities[ptv70_idx];
                            }
                        }
                        bool jawTracking;
                        if (this.checkBoxJawTracking.Checked)
                        {
                            jawTracking = true;
                        }
                        else
                        {
                            jawTracking = false;
                        }
                        this.OptimTime.Start();
                        var structureLists = VMS.TPS.Script.StartOptimizer(this.context, plan, this.MatchingStructures, 1, this.Features, jawTracking); //still doing 1 opt iteration for each plan to increase quality
                        List<List<Structure>> optimizedStructures = structureLists.Item1;
                        List<List<Structure>> matchingStructures = structureLists.Item2;
                        List<List<string>> updateLog = structureLists.Item3;

                        Tuple<bool, List<bool>, List<List<List<double>>>> planData = Check.EvaluatePlan(this.context, plan, this.MatchingStructures, optimizedStructures);

                        CombinedPlanData.Add(planData);

                    }
                }
            }
            //Now need to construct the CSV file. 
            List<string> csvString = new List<string>();
            //First row contains plan info
            csvString.Add("LAST NAME," + this.context.Patient.LastName);
            csvString.Add("FIRST NAME," + this.context.Patient.FirstName);
            csvString.Add("PLAN TYPE," + this.context.PlanSetup.PlanType.ToString());
            csvString.Add("SEX," + this.context.Patient.Sex);
            csvString.Add("AGE," + this.context.Patient.DateOfBirth.Value.Year.ToString());
            //Get the PTV sizes: 
            double volume = 0;
            for (int i = 0; i < this.MatchingStructures[4].Count; i++)//PTV70
            {
                volume += this.MatchingStructures[4][i].Volume;
            }
            csvString.Add("PTV 70 TOTAL VOLUME," + volume.ToString());

            volume = 0;
            for (int i = 0; i < this.MatchingStructures[5].Count; i++)//PTV63
            {
                volume += this.MatchingStructures[5][i].Volume;
            }
            csvString.Add("PTV 63 TOTAL VOLUME," + volume.ToString());

            volume = 0;
            for (int i = 0; i < this.MatchingStructures[6].Count; i++)//PTV56
            {
                volume += this.MatchingStructures[6][i].Volume;
            }
            csvString.Add("PTV 56 TOTAL VOLUME," + volume.ToString());


            //Now make a header for each ROI
            csvString.Add("");
            csvString.Add("");
            csvString.Add("PLAN#, PASSED, BRAINSTEM, BRAINSTEM PRV, SPINAL CORD, SPINAL CORD PRV, PTV 70, PTV 63, PTV 56, BRAIN, CHIASM, CHIASM PRV, RIGHT OPTIC NERVE, LEFT OPTIC NERVE, OPTIC NERVE PRV, R GLOBE, L GLOBE, R PAROTID, L PAROTID, R SUB, L SUB, R LENS, L LENS, ORAL CAVITY, LARYNX, LIPS, MANDIBLE, BODY");

            string csvLine = "";
            List<Dictionary<string, double>> DoseDictionaries = new List<Dictionary<string, double>>();

            for (int iter = 0; iter < numIterations; iter++)
            {
                csvLine = "";
                csvLine += (iter + 1).ToString() + ",";
                csvLine += CombinedPlanData[iter].Item1 + ",";
                for (int roi_idx = 0; roi_idx < CombinedPlanData[iter].Item3.Count; roi_idx++) //go through all ROIs
                {
                    if (CombinedPlanData[iter].Item3[roi_idx].Count == 0)
                    {
                        csvLine += ",";
                    } else
                    {
                        double value = 0;
                        for (int match = 0; match < CombinedPlanData[iter].Item3[roi_idx].Count; match++)
                        {
                            //Get the average value for the 1st constraint applied to the ROI
                            value += CombinedPlanData[iter].Item3[roi_idx][match][0];
                        }
                        value /= CombinedPlanData[iter].Item3[roi_idx].Count;

                        csvLine += value.ToString() + ",";
                    }

                }
                csvString.Add(csvLine);
                csvLine = ",,";
                //Now do another row for any ROIs that have a second constraint
                for (int roi_idx = 0; roi_idx < 26; roi_idx++) //go through all ROIs
                {
                    if (CombinedPlanData[iter].Item3[roi_idx].Count == 0)
                    {
                        csvLine += ",";
                    }
                    else if (CombinedPlanData[iter].Item3[roi_idx][0].Count == 1)
                    {
                        csvLine += ",";
                    } else
                    {
                        double value = 0;
                        double totalVolume = 0;
                        for (int match = 0; match < CombinedPlanData[iter].Item3[roi_idx].Count; match++)
                        {

                            //Get the volume-weighted average value for the 1st constraint applied to the ROI
                            value += CombinedPlanData[iter].Item3[roi_idx][match][1] * MatchingStructures[roi_idx][match].Volume;
                            totalVolume += MatchingStructures[roi_idx][match].Volume;



                        }
                        value /= totalVolume;

                        csvLine += value.ToString() + ",";
                    }

                }
                csvString.Add(csvLine);
                //Now need to add a final row indicating which plan was best for different measures. Get the different objectives now, including volume-weighted averages if there is more than one matching dicom structure for an roi
                Dictionary<string, double> doseVals = new Dictionary<string, double>();
                double temp;
                double totalVol;
                //PTV70 V95 weighed avg
                if (CombinedPlanData[iter].Item3[4].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[4].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[4][match][0] * MatchingStructures[4][match].Volume;
                        totalVol += MatchingStructures[4][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("PTV70_V95", temp);

                }
                else
                {
                    doseVals.Add("PTV70_V95", 11111);
                }
                //PTV63 V95 weighed avg
                if (CombinedPlanData[iter].Item3[5].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[5].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[5][match][0] * MatchingStructures[5][match].Volume;
                        totalVol += MatchingStructures[5][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("PTV63_V95", temp);

                }
                else
                {
                    doseVals.Add("PTV63_V95", 11111);
                }
                //PTV56 V95 weighed avg
                if (CombinedPlanData[iter].Item3[6].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[6].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[6][match][0] * MatchingStructures[6][match].Volume;
                        totalVol += MatchingStructures[6][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("PTV56_V95", temp);
                }
                else
                {
                    doseVals.Add("PTV56_V95", 11111);
                }
                //Brainstem Dmax:
                if (CombinedPlanData[iter].Item3[0].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[0].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[0][match][0] * MatchingStructures[0][match].Volume;
                        totalVol += MatchingStructures[0][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Brainstem_Dmax", temp);
                }
                else
                {
                    doseVals.Add("Brainstem_Dmax", 11111);
                }
                //Spinal dmax:
                if (CombinedPlanData[iter].Item3[0].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[2].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[2][match][0] * MatchingStructures[2][match].Volume;
                        totalVol += MatchingStructures[2][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Cord_Dmax", temp);
                }
                else
                {
                    doseVals.Add("Cord_Dmax", 11111);
                }
                //Dmax to the optic nerves:
                //R optic Dmax:
                if (CombinedPlanData[iter].Item3[10].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[10].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[10][match][0] * MatchingStructures[10][match].Volume;
                        totalVol += MatchingStructures[10][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Right_Optic_Dmax", temp);
                }
                else
                {
                    doseVals.Add("Right_Optic_Dmax", 11111);
                }
                //Left optic Dmax
                if (CombinedPlanData[iter].Item3[11].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[11].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[11][match][0] * MatchingStructures[11][match].Volume;
                        totalVol += MatchingStructures[11][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Left_Optic_Dmax", temp);
                }
                else
                {
                    doseVals.Add("Left_Optic_Dmax", 11111);
                }
                // R parotid Dmean
                if (CombinedPlanData[iter].Item3[16].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[16].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[16][match][0] * MatchingStructures[16][match].Volume;
                        totalVol += MatchingStructures[16][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Right_Paro_Dmean", temp);
                }
                else
                {
                    doseVals.Add("Right_Paro_Dmean", 11111);
                }
                // L parotid Dmean
                if (CombinedPlanData[iter].Item3[17].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[17].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[17][match][0] * MatchingStructures[17][match].Volume;
                        totalVol += MatchingStructures[17][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Left_Paro_Dmean", temp);
                }
                else
                {
                    doseVals.Add("Left_Paro_Dmean", 11111);
                }
                // R sub Dmean
                if (CombinedPlanData[iter].Item3[18].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[18].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[18][match][0] * MatchingStructures[18][match].Volume;
                        totalVol += MatchingStructures[18][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Right_Sub_Dmean", temp);
                }
                else
                {
                    doseVals.Add("Right_Sub_Dmean", 11111);
                }
                // L parotid Dmean
                if (CombinedPlanData[iter].Item3[19].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[19].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[19][match][0] * MatchingStructures[19][match].Volume;
                        totalVol += MatchingStructures[19][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Left_Sub_Dmean", temp);
                }
                else
                {
                    doseVals.Add("Left_Sub_Dmean", 11111);
                }
                //Oral cav dmean
                if (CombinedPlanData[iter].Item3[22].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[22].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[22][match][0] * MatchingStructures[22][match].Volume;
                        totalVol += MatchingStructures[22][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("OC_Dmean", temp);
                }
                else
                {
                    doseVals.Add("OC_Dmean", 11111);
                }
                //laryn Dmean
                if (CombinedPlanData[iter].Item3[23].Count > 0)
                {
                    temp = 0;
                    totalVol = 0;
                    for (int match = 0; match < CombinedPlanData[iter].Item3[23].Count; match++)
                    {
                        temp += CombinedPlanData[iter].Item3[23][match][0] * MatchingStructures[23][match].Volume;
                        totalVol += MatchingStructures[23][match].Volume;
                    }
                    temp /= totalVol;
                    doseVals.Add("Larynx_Dmean", temp);
                }
                else
                {
                    doseVals.Add("Larynx_Dmean", 11111);
                }
                DoseDictionaries.Add(doseVals);
            }
            //Now for the final rows we can go through the list of dose dictionaries for each plan and determine which plan worked the best for various objectives.
            csvString.Add("");
            csvString.Add("Best Plans");
            //Best plan for highest PTV70 V95:
            double max = -10000;
            double min = 10000;
            int currentIdx = 0;
            bool noPlans = true;
            if (DoseDictionaries[0]["PTV70_V95"] == 11111)
            {
                csvString.Add("Highest PTV70 V95, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["PTV70_V95"] > max) && (CombinedPlanData[i].Item1 == true)) //has to be a passing plan
                    {
                        currentIdx = i;
                        max = DoseDictionaries[i]["PTV70_V95"];
                        noPlans = false;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Highest PTV70 V95, None Passed");
                }
                else
                {
                    csvString.Add("Highest PTV70 V95," + (currentIdx + 1).ToString());
                }

                max = -10000;
                currentIdx = 0;
                
            }



            //Best for highest PTV63
            noPlans = true;
            if (DoseDictionaries[0]["PTV63_V95"] == 11111)
            {
                csvString.Add("Highest PTV63 V95, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["PTV63_V95"] > max) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        max = DoseDictionaries[i]["PTV63_V95"];
                        noPlans = false;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Highest PTV63 None Passed");
                }
                else
                {
                    csvString.Add("Highest PTV63 V95," + (currentIdx + 1).ToString());
                }
                max = -10000;
                currentIdx = 0;
            }


            //Best for highest PTV56
            noPlans = true;
            if (DoseDictionaries[0]["PTV56_V95"] == 11111)
            {
                csvString.Add("Highest PTV56 V95, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["PTV56_V95"] > max) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        max = DoseDictionaries[i]["PTV56_V95"];
                        noPlans = true;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Highest PTV56 V95, None Passed");
                }
                else
                {
                    csvString.Add("Highest PTV56 V95," + (currentIdx + 1).ToString());
                }
                max = -10000;
                currentIdx = 0;
            }

            //Best for lowest Brainstem dmax:
            noPlans = true;
            if (DoseDictionaries[0]["Brainstem_Dmax"] == 11111)
            {
                csvString.Add("Lowest Brainstem Dmax, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["Brainstem_Dmax"] < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = DoseDictionaries[i]["Brainstem_Dmax"];
                        noPlans = false;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Lowest Brainstem Dmax, None Passed");
                }
                else
                {
                    csvString.Add("Lowest Brainstem Dmax," + (currentIdx + 1).ToString());
                }
                min = 10000;
                currentIdx = 0;
            }
            //Best for lowest cord dmax:
            noPlans = true;
            if (DoseDictionaries[0]["Cord_Dmax"] == 11111)
            { 
                csvString.Add("Lowest Cord Dmax, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["Cord_Dmax"] < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = DoseDictionaries[i]["Cord_Dmax"];
                        noPlans = false;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Lowest Cord Dmax, None Passed");
                }
                else
                {
                    csvString.Add("Lowest Cord Dmax," + (currentIdx + 1).ToString());
                }
                min = 10000;
                currentIdx = 0;
            }
            //Best for lowest optic nerve dmax:
            noPlans = true;
            if ((DoseDictionaries[0]["Right_Optic_Dmax"] == 11111)|| (DoseDictionaries[0]["Left_Optic_Dmax"] == 11111))
            {
                csvString.Add("Lowest optic nerve Dmax, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((Math.Max(DoseDictionaries[i]["Right_Optic_Dmax"], DoseDictionaries[i]["Left_Optic_Dmax"]) < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = Math.Max(DoseDictionaries[i]["Right_Optic_Dmax"], DoseDictionaries[i]["Left_Optic_Dmax"]);
                        noPlans = true;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Lowest optic nerve Dmax, None Passed");
                }
                else
                {
                    csvString.Add("Lowest optic nerve Dmax," + (currentIdx + 1).ToString());
                }
                min = 10000;
                currentIdx = 0;
            }
            //Now to minimize one parotid most: 
            noPlans = true;
            if ((DoseDictionaries[0]["Right_Paro_Dmean"] == 11111) && (DoseDictionaries[0]["Left_Paro_Dmean"] == 11111))
            {
                csvString.Add("Minimize one parotid, N/A");
            }
            else if (DoseDictionaries[0]["Right_Paro_Dmean"] == 11111)
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["Left_Paro_Dmean"] < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = DoseDictionaries[i]["Left_Paro_Dmean"];
                        noPlans = false;
                    }
                }

            }
            else if (DoseDictionaries[0]["Left_Paro_Dmean"] == 11111)
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["Right_Paro_Dmean"] < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = DoseDictionaries[i]["Right_Paro_Dmean"];
                        noPlans = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((Math.Min(DoseDictionaries[i]["Right_Paro_Dmean"], DoseDictionaries[i]["Left_Paro_Dmean"]) < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = Math.Min(DoseDictionaries[i]["Right_Paro_Dmean"], DoseDictionaries[i]["Left_Paro_Dmean"]);
                        noPlans = false;
                    }
                }
            }
            if (noPlans)
            {
                csvString.Add("Minimize one parotid, None Passed");
            }
            else
            {
                csvString.Add("Minimize one parotid," + (currentIdx + 1).ToString());
                min = 10000;
                currentIdx = 0;
            }

            //Now to minimize both parotids: 
            noPlans = true;
            if ((DoseDictionaries[0]["Right_Paro_Dmean"] == 11111) && (DoseDictionaries[0]["Left_Paro_Dmean"] == 11111))
            {
                csvString.Add("Minimize both parotids, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((Math.Max(DoseDictionaries[i]["Right_Paro_Dmean"], DoseDictionaries[i]["Left_Paro_Dmean"]) < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = Math.Max(DoseDictionaries[i]["Right_Paro_Dmean"], DoseDictionaries[i]["Left_Paro_Dmean"]);
                        noPlans = false;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Minimize both parotids, None Passed");
                }
                else
                {
                    csvString.Add("Minimize both parotids," + (currentIdx + 1).ToString());
                }
                min = 10000;
                currentIdx = 0;
            }

            //Now to minimize one sub most: 
            noPlans = true;
            if ((DoseDictionaries[0]["Right_Sub_Dmean"] == 11111) && (DoseDictionaries[0]["Left_Sub_Dmean"] == 11111))
            {
                csvString.Add("Minimize one Submandibular, N/A");
            }
            else if (DoseDictionaries[0]["Right_Sub_Dmean"] == 11111)
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["Left_Sub_Dmean"] < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = DoseDictionaries[i]["Left_Sub_Dmean"];
                        noPlans = false;
                    }
                }

            }
            else if (DoseDictionaries[0]["Left_Sub_Dmean"] == 11111)
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["Right_Sub_Dmean"] < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = DoseDictionaries[i]["Right_Sub_Dmean"];
                        noPlans = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((Math.Min(DoseDictionaries[i]["Right_Sub_Dmean"], DoseDictionaries[i]["Left_Sub_Dmean"]) < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = Math.Min(DoseDictionaries[i]["Right_Sub_Dmean"], DoseDictionaries[i]["Left_Sub_Dmean"]);
                        noPlans = false;
                    }
                }
            }
            if (noPlans)
            {
                csvString.Add("Minimize one submandibular, None Passed");
            }
            else
            {
                csvString.Add("Minimize one submandibular," + (currentIdx + 1).ToString());
            }
            min = 10000;
            currentIdx = 0;

            //Now to minimize both subs: 
            noPlans = true;
            if ((DoseDictionaries[0]["Right_Sub_Dmean"] == 11111) && (DoseDictionaries[0]["Left_Sub_Dmean"] == 11111))
            {
                csvString.Add("Minimize both Submandibulars, N/A");
            }else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((Math.Max(DoseDictionaries[i]["Right_Sub_Dmean"], DoseDictionaries[i]["Left_Sub_Dmean"]) < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = Math.Max(DoseDictionaries[i]["Right_Sub_Dmean"], DoseDictionaries[i]["Left_Sub_Dmean"]);
                        noPlans = false;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Minimize both Submandibulars, None Passed");
                }
                else
                {
                    csvString.Add("Minimize both Submandibulars," + (currentIdx + 1).ToString());
                    min = 10000;
                    currentIdx = 0;
                }
            }


            //Best for lowest OC dmean:
            noPlans = true;
            if (DoseDictionaries[0]["OC_Dmean"] == 11111)
            {
                csvString.Add("Lowest OC Dmean, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["OC_Dmean"] < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = DoseDictionaries[i]["OC_Dmean"];
                        noPlans = false;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Lowest OC Dmean, None Passed");
                }
                else
                {
                    csvString.Add("Lowest OC Dmean," + (currentIdx + 1).ToString());
                }
                min = 10000;
                currentIdx = 0;
            }

            //Best for lowest larynx dmean:
            noPlans = true;
            if (DoseDictionaries[0]["Larynx_Dmean"] == 11111)
            {
                csvString.Add("Lowest Larynx Dmean, N/A");
            }
            else
            {
                for (int i = 0; i < DoseDictionaries.Count; i++)
                {
                    if ((DoseDictionaries[i]["Larynx_Dmean"] < min) && (CombinedPlanData[i].Item1 == true))
                    {
                        currentIdx = i;
                        min = DoseDictionaries[i]["Larynx_Dmean"];
                        noPlans = false;
                    }
                }
                if (noPlans)
                {
                    csvString.Add("Lowest Larynx Dmean, None Passed");
                }
                else
                {
                    csvString.Add("Lowest Larynx Dmean," + (currentIdx + 1).ToString());
                }
                min = 10000;
                currentIdx = 0;

            }




            using (TextWriter sw = new StreamWriter(path))
            {
                for (int i =0; i < csvString.Count; i++)
                {
                    sw.WriteLine(csvString[i]);
                }

            
                
            }

            this.TotalTime.Stop();
            double totalMins = this.TotalTime.Elapsed.TotalMinutes;
            double totalSeconds = this.TotalTime.Elapsed.TotalSeconds;

            System.Windows.MessageBox.Show("Total time elapsed: " + totalSeconds.ToString() + " seconds");


        }
        private string GetOptParamsCSVPath()
        {
            var sfd = new SaveFileDialog
            {
                Title = "Choose Save Location",
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = "OptimalParameters_" + this.context.Patient.Name
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
    }
}
