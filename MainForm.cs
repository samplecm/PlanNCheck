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
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

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
        public List<Tuple<bool, double[], string>> Features { get; set; }

        public OxyPlot.WindowsForms.PlotView PV { get; set; }
        public Stopwatch TotalTime { get; set; }
        public Stopwatch OptimTime { get; set; }




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
            if (this.SavePath == "")
            {
                StartErrorLabel.Text = "Please select a save destination.";
                StartErrorLabel.Visible = true;
            }
            else if (this.Plans.Count == 0)
            {
                StartErrorLabel.Text = "Please select a type of plan to report on.";
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
                
                this.StartErrorLabel.Text = "In progress";
                this.StartErrorLabel.Visible = true;
                this.OptimTime.Start();
                var structureLists = VMS.TPS.Script.StartOptimizer(this.context, this.HnPlan, this.MatchingStructures, iterations, this.Features);
                List<List<Structure>> optimizedStructures = structureLists.Item1;
                List<List<Structure>> matchingStructures = structureLists.Item2;
                List<List<string>> updateLog = structureLists.Item3;
                if (SaveCheck.Checked)
                {
                    Calculator.RunReport(this.context, this.HnPlan, this.SavePath, optimizedStructures, this.MatchingStructures, updateLog);
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
                System.Windows.MessageBox.Show("Optimization time elapsed: " + optimSeconds.ToString() + " seconds");

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
                //Also check for special prescription PTVs (not 56, 63, or 70 for HNPlan) and find maximum PTV prescription, which if less than the prescription dose
                //will be used to adjust all PTV maximum dose constraints, and maximum body constraint.
                int ptvType;
                int maxPTV_Dose = 0;
                foreach (Structure structure in context.StructureSet.Structures)
                {
                    if (structure.Name.ToLower().Contains("ptv"))
                    {
                        ptvType = VMS.TPS.Script.FindPTVNumber(structure.Name.ToLower());
                        if (ptvType > maxPTV_Dose)
                        {
                            maxPTV_Dose = ptvType;
                        }
                        //Check if standard prescription dose type;
                        bool isStandard = this.HnPlan.PTV_Types.IndexOf(ptvType) != -1;
                        if ((!isStandard)&&(ptvType != 0))
                        {
                            //Make a new constraint
                            string Name = "PTV" + maxPTV_Dose.ToString();
                            ROI newPTV = new ROI();
                            newPTV.Name = Name;
                            newPTV.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 110));
                            newPTV.Constraints.Add(new Constraint("D", "max", "<", ptvType * 100, "abs", 100));
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
            
            this.MatchingStructures.Clear();
            for (int i = 0; i < this.HnPlan.ROIs.Count; i++)
            {
                List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, this.HnPlan.ROIs[i]); //find structures that match the constraint structure
                this.MatchingStructures.Add(assignedStructures);
            }
            //Also check for special prescription PTVs (not 56, 63, or 70 for HNPlan)
            int ptvType;
            foreach (Structure structure in context.StructureSet.Structures)
            {
                if (structure.Name.ToLower().Contains("ptv"))
                {
                    ptvType = VMS.TPS.Script.FindPTVNumber(structure.Name.ToLower());
                    //Check if standard prescription dose type;
                    bool isStandard = this.HnPlan.PTV_Types.IndexOf(ptvType) != -1;
                    if (!isStandard)
                    {
                        //Make a new constraint
                        string Name = "PTV" + ptvType.ToString();
                        ROI newPTV = new ROI();
                        newPTV.Name = Name;
                        newPTV.Constraints.Add(new Constraint("V", "95", ">", 98, "rel", 110));
                        newPTV.Constraints.Add(new Constraint("D", "max", "<", 1.1 * this.HnPlan.PrescriptionDose, "abs", 100));
                        this.HnPlan.ROIs.Add(newPTV);
                        this.MatchingStructures.Add(new List<Structure>() { structure });
                    }
                }
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
            if ((String.IsNullOrEmpty(this.StructureTB.Text)) || (String.IsNullOrEmpty(this.SubscriptTB.Text)) || 
                (String.IsNullOrEmpty(this.ValueTB.Text))) 
            {               
                this.AddLabel.Visible = true;

            }else if ((this.Combobox_Type.SelectedIndex == -1)||(this.Combobox_Relation.SelectedIndex == -1) ||(this.Combobox_Format.SelectedIndex == -1))
            {

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
                        Constraint newConstraint = new Constraint(this.Combobox_Type.SelectedItem.ToString(), SubscriptTB.Text.ToString(), this.Combobox_Relation.SelectedItem.ToString(), Convert.ToDouble(ValueTB.Text.ToString()), this.Combobox_Format.SelectedItem.ToString());
                        this.HnPlan.ROIs[i].Constraints.Add(newConstraint);
                        break;
                    }
                }
                if (structExists == false)
                {
                    ROI NewROI = new ROI();
                    NewROI.Name = StructureTB.Text.ToString();
                    NewROI.Constraints.Add(new Constraint(this.Combobox_Type.SelectedItem.ToString(), SubscriptTB.Text.ToString(), this.Combobox_Relation.SelectedItem.ToString(), Convert.ToDouble(ValueTB.Text.ToString()), this.Combobox_Format.SelectedItem.ToString()));

                    this.HnPlan.ROIs.Add(NewROI);
                    //Need to also add assigned structure for this
                    List<Structure> assignedStructures = VMS.TPS.Script.AssignStructure(context.StructureSet, NewROI); //find structures that match the constraint structure
                    this.MatchingStructures.Add(assignedStructures);
                }


            }
            PopulateGrid();
            this.Combobox_Type.SelectedIndex = -1;
            this.ValueTB.Text = "";
            this.Combobox_Relation.SelectedIndex = -1;
            this.Combobox_Format.SelectedIndex = -1;
            this.Combobox_Type.SelectedIndex = -1;
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
        }

        private void ButtonDoneFeatures_Click(object sender, EventArgs e)
        {
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
            var dvh = CalculateDVH(this.context.PlanSetup, plotStructure);
            var series = CreateDVHSeries(dvh);
            this.PV.Model.Series.Add(series);
            this.Controls.Add(this.PV);
            this.PlotPanel.Controls.Add(this.PV);

            

            


        }
        
        private DVHData CalculateDVH(PlanSetup plan, Structure structure)
        {
            return plan.GetDVHCumulativeData(structure, DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.01);
        }
        private OxyPlot.Series.Series CreateDVHSeries(DVHData dvh)
        {
            var series = new LineSeries();
            var points = CreateDataPoints(dvh);
            series.Points.AddRange(points);
            return series;
        }

        private List<DataPoint> CreateDataPoints(DVHData dvh)
        {
            var points = new List<DataPoint>();
            foreach (var dvhPoint in dvh.CurveData)
            {
                var point = CreateDataPoint(dvhPoint);
                points.Add(point);
            }
            return points;
        }
        private DataPoint CreateDataPoint(DVHPoint dvhPoint)
        {
            return new DataPoint(dvhPoint.DoseValue.Dose, dvhPoint.Volume);
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
            VMS.TPS.Script.MakeSubsegmentStructures(organ, organSegOptions, ref plan, ref ss, this.context, applyConstraints, this.MatchingStructures, ref hnPlan);
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
    }
}
