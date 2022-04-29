using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace Plan_n_Check
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 



        private void InitializeComponent()
        {
            System.Windows.Forms.Button CustomizeButton;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label2 = new System.Windows.Forms.Label();
            this.ComboBox_PlanType = new System.Windows.Forms.ComboBox();
            this.AssigningPanel = new System.Windows.Forms.Panel();
            this.assignedLabel = new System.Windows.Forms.Label();
            this.StructLabel = new System.Windows.Forms.Label();
            this.DicomComboBox = new System.Windows.Forms.ComboBox();
            this.AddStructureButton = new System.Windows.Forms.Button();
            this.removeAssignedButton = new System.Windows.Forms.Button();
            this.AssigningLabel = new System.Windows.Forms.Label();
            this.AssignStructGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.conStructGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FinishedEdtingButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label24 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.Label_Fracs = new System.Windows.Forms.Label();
            this.ComboBox_TreatmentArea = new System.Windows.Forms.ComboBox();
            this.Label_TreatmentArea = new System.Windows.Forms.Label();
            this.ComboBox_TreatmentCenter = new System.Windows.Forms.ComboBox();
            this.Label_TreatmentCenter = new System.Windows.Forms.Label();
            this.Label_Fractions = new System.Windows.Forms.Label();
            this.Label_Presc = new System.Windows.Forms.Label();
            this.Label_PrescDose = new System.Windows.Forms.Label();
            this.Label_PlanType = new System.Windows.Forms.Label();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.checkBoxJawTracking = new System.Windows.Forms.CheckBox();
            this.ConstraintGridView = new System.Windows.Forms.DataGridView();
            this.Structure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subscript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Relation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Format = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox_OAR = new System.Windows.Forms.CheckBox();
            this.ValueTB = new System.Windows.Forms.TextBox();
            this.SubscriptTB = new System.Windows.Forms.TextBox();
            this.StructureTB = new System.Windows.Forms.TextBox();
            this.PlotFormButton = new System.Windows.Forms.Button();
            this.helpButtonConstraints = new System.Windows.Forms.Button();
            this.Combobox_Format = new System.Windows.Forms.ComboBox();
            this.Combobox_Relation = new System.Windows.Forms.ComboBox();
            this.Combobox_Type = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LoadFeaturesButton = new System.Windows.Forms.Button();
            this.EditAssignedButton = new System.Windows.Forms.Button();
            this.AddLabel = new System.Windows.Forms.Label();
            this.OkButtonLabel = new System.Windows.Forms.Label();
            this.FormatLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.RelationLabel = new System.Windows.Forms.Label();
            this.SubscriptLabel = new System.Windows.Forms.Label();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.StructureLabel = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.PanelSpecialFeatures = new System.Windows.Forms.Panel();
            this.Button_ParamOptStart = new System.Windows.Forms.Button();
            this.ML_Label = new System.Windows.Forms.Label();
            this.TextBox_Volume_ub = new System.Windows.Forms.TextBox();
            this.TextBox_Volume_lb = new System.Windows.Forms.TextBox();
            this.TextBox_Dose_ub = new System.Windows.Forms.TextBox();
            this.TextBox_Dose_lb = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Label_DVH_VolumeBound = new System.Windows.Forms.Label();
            this.Label_DVHDoseBound = new System.Windows.Forms.Label();
            this.SpecialFeatures_Clicked = new System.Windows.Forms.CheckBox();
            this.Combobox_dvhReport = new System.Windows.Forms.ComboBox();
            this.DVH_gridView = new System.Windows.Forms.DataGridView();
            this.DVHcolumn_Structure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DVHcolumn_VolumeBounds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DVHcolumn_DoseBounds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Button_AddDVH_Report = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ButtonDeleteParotidSub = new System.Windows.Forms.Button();
            this.CheckboxSegmentConstraints = new System.Windows.Forms.CheckBox();
            this.Button_DeleteSubsegments = new System.Windows.Forms.Button();
            this.Button_StartSegmentation = new System.Windows.Forms.Button();
            this.Label_Sagittal = new System.Windows.Forms.Label();
            this.Sagittal_Combobox = new System.Windows.Forms.ComboBox();
            this.Label_Coronal = new System.Windows.Forms.Label();
            this.Coronal_Combobox = new System.Windows.Forms.ComboBox();
            this.Label_AxialSlice = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Axial_Combobox = new System.Windows.Forms.ComboBox();
            this.OrganSeg_OrgansCombo = new System.Windows.Forms.ComboBox();
            this.PriorityLabel = new System.Windows.Forms.Label();
            this.PriorityRatio_TextBox = new System.Windows.Forms.TextBox();
            this.LabelSpecialFeatures = new System.Windows.Forms.Label();
            this.ButtonDoneFeatures = new System.Windows.Forms.Button();
            this.CheckBox_ChopParotid = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.StartErrorLabel = new System.Windows.Forms.Label();
            this.IterationsLabel = new System.Windows.Forms.Label();
            this.IterationsTextBox = new System.Windows.Forms.TextBox();
            this.SaveCheck = new System.Windows.Forms.CheckBox();
            this.LabelSaveCheck = new System.Windows.Forms.Label();
            this.LocationLabel = new System.Windows.Forms.Label();
            this.LatexPanel = new System.Windows.Forms.Panel();
            this.DoneLatexButton = new System.Windows.Forms.Button();
            this.PlotPanel = new System.Windows.Forms.Panel();
            this.DVHLabel = new System.Windows.Forms.Label();
            this.PlotButton = new System.Windows.Forms.Button();
            this.PlotCombobox = new System.Windows.Forms.ComboBox();
            this.PlotDoneButton = new System.Windows.Forms.Button();
            CustomizeButton = new System.Windows.Forms.Button();
            this.AssigningPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssignStructGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.conStructGridView)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConstraintGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.PanelSpecialFeatures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DVH_gridView)).BeginInit();
            this.panel7.SuspendLayout();
            this.LatexPanel.SuspendLayout();
            this.PlotPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CustomizeButton
            // 
            CustomizeButton.BackColor = System.Drawing.Color.SteelBlue;
            CustomizeButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CustomizeButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            CustomizeButton.Location = new System.Drawing.Point(271, 325);
            CustomizeButton.Name = "CustomizeButton";
            CustomizeButton.Size = new System.Drawing.Size(162, 34);
            CustomizeButton.TabIndex = 11;
            CustomizeButton.Text = "Continue";
            CustomizeButton.UseVisualStyleBackColor = false;
            CustomizeButton.Click += new System.EventHandler(this.CustomizeButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.SteelBlue;
            this.label2.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(249, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 42);
            this.label2.TabIndex = 2;
            this.label2.Text = "Plan";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ComboBox_PlanType
            // 
            this.ComboBox_PlanType.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_PlanType.FormattingEnabled = true;
            this.ComboBox_PlanType.Location = new System.Drawing.Point(392, 52);
            this.ComboBox_PlanType.Name = "ComboBox_PlanType";
            this.ComboBox_PlanType.Size = new System.Drawing.Size(156, 31);
            this.ComboBox_PlanType.TabIndex = 14;
            // 
            // AssigningPanel
            // 
            this.AssigningPanel.BackColor = System.Drawing.Color.White;
            this.AssigningPanel.Controls.Add(this.assignedLabel);
            this.AssigningPanel.Controls.Add(this.StructLabel);
            this.AssigningPanel.Controls.Add(this.DicomComboBox);
            this.AssigningPanel.Controls.Add(this.AddStructureButton);
            this.AssigningPanel.Controls.Add(this.removeAssignedButton);
            this.AssigningPanel.Controls.Add(this.AssigningLabel);
            this.AssigningPanel.Controls.Add(this.AssignStructGridView);
            this.AssigningPanel.Controls.Add(this.conStructGridView);
            this.AssigningPanel.Controls.Add(this.FinishedEdtingButton);
            this.AssigningPanel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssigningPanel.Location = new System.Drawing.Point(0, 65);
            this.AssigningPanel.Name = "AssigningPanel";
            this.AssigningPanel.Size = new System.Drawing.Size(737, 386);
            this.AssigningPanel.TabIndex = 18;
            this.AssigningPanel.Visible = false;
            // 
            // assignedLabel
            // 
            this.assignedLabel.AutoSize = true;
            this.assignedLabel.Location = new System.Drawing.Point(368, 192);
            this.assignedLabel.Name = "assignedLabel";
            this.assignedLabel.Size = new System.Drawing.Size(0, 19);
            this.assignedLabel.TabIndex = 9;
            this.assignedLabel.Visible = false;
            // 
            // StructLabel
            // 
            this.StructLabel.AutoSize = true;
            this.StructLabel.Location = new System.Drawing.Point(371, 12);
            this.StructLabel.Name = "StructLabel";
            this.StructLabel.Size = new System.Drawing.Size(0, 19);
            this.StructLabel.TabIndex = 8;
            this.StructLabel.Visible = false;
            // 
            // DicomComboBox
            // 
            this.DicomComboBox.FormattingEnabled = true;
            this.DicomComboBox.Location = new System.Drawing.Point(496, 290);
            this.DicomComboBox.Name = "DicomComboBox";
            this.DicomComboBox.Size = new System.Drawing.Size(184, 27);
            this.DicomComboBox.TabIndex = 7;
            this.DicomComboBox.SelectedIndexChanged += new System.EventHandler(this.DicomComboBox_SelectedIndexChanged);
            // 
            // AddStructureButton
            // 
            this.AddStructureButton.BackColor = System.Drawing.Color.SteelBlue;
            this.AddStructureButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddStructureButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.AddStructureButton.Location = new System.Drawing.Point(373, 289);
            this.AddStructureButton.Name = "AddStructureButton";
            this.AddStructureButton.Size = new System.Drawing.Size(112, 28);
            this.AddStructureButton.TabIndex = 6;
            this.AddStructureButton.Text = "Add";
            this.AddStructureButton.UseVisualStyleBackColor = false;
            this.AddStructureButton.Click += new System.EventHandler(this.AddStructureButton_Click);
            // 
            // removeAssignedButton
            // 
            this.removeAssignedButton.BackColor = System.Drawing.Color.SteelBlue;
            this.removeAssignedButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeAssignedButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.removeAssignedButton.Location = new System.Drawing.Point(373, 244);
            this.removeAssignedButton.Name = "removeAssignedButton";
            this.removeAssignedButton.Size = new System.Drawing.Size(112, 30);
            this.removeAssignedButton.TabIndex = 5;
            this.removeAssignedButton.Text = "Remove";
            this.removeAssignedButton.UseVisualStyleBackColor = false;
            this.removeAssignedButton.Click += new System.EventHandler(this.removeAssignedButton_Click);
            // 
            // AssigningLabel
            // 
            this.AssigningLabel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssigningLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.AssigningLabel.Location = new System.Drawing.Point(313, 18);
            this.AssigningLabel.Name = "AssigningLabel";
            this.AssigningLabel.Size = new System.Drawing.Size(424, 51);
            this.AssigningLabel.TabIndex = 4;
            this.AssigningLabel.Text = "Click on a constrained structure to view / update assigned dicom structures.";
            // 
            // AssignStructGridView
            // 
            this.AssignStructGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AssignStructGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.AssignStructGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AssignStructGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.AssignStructGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.AssignStructGridView.GridColor = System.Drawing.Color.DodgerBlue;
            this.AssignStructGridView.Location = new System.Drawing.Point(373, 132);
            this.AssignStructGridView.Name = "AssignStructGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AssignStructGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.AssignStructGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AssignStructGridView.Size = new System.Drawing.Size(307, 106);
            this.AssignStructGridView.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn2.HeaderText = "Assigned Structure";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // conStructGridView
            // 
            this.conStructGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.conStructGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.conStructGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.conStructGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.conStructGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.conStructGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.conStructGridView.GridColor = System.Drawing.Color.DodgerBlue;
            this.conStructGridView.Location = new System.Drawing.Point(35, 26);
            this.conStructGridView.Name = "conStructGridView";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.conStructGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.conStructGridView.Size = new System.Drawing.Size(266, 345);
            this.conStructGridView.TabIndex = 2;
            this.conStructGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.conStructGridView_CellClick);
            this.conStructGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.conStructGridView_CellContentClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Constrained Structure";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // FinishedEdtingButton
            // 
            this.FinishedEdtingButton.BackColor = System.Drawing.Color.SteelBlue;
            this.FinishedEdtingButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FinishedEdtingButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FinishedEdtingButton.Location = new System.Drawing.Point(601, 339);
            this.FinishedEdtingButton.Name = "FinishedEdtingButton";
            this.FinishedEdtingButton.Size = new System.Drawing.Size(121, 33);
            this.FinishedEdtingButton.TabIndex = 10;
            this.FinishedEdtingButton.Text = "Finished";
            this.FinishedEdtingButton.UseVisualStyleBackColor = false;
            this.FinishedEdtingButton.Click += new System.EventHandler(this.FinishedEdtingButton_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SteelBlue;
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(737, 70);
            this.panel2.TabIndex = 13;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.SteelBlue;
            this.label24.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label24.Location = new System.Drawing.Point(340, 20);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(25, 25);
            this.label24.TabIndex = 4;
            this.label24.Text = "n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.SteelBlue;
            this.label3.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(362, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 42);
            this.label3.TabIndex = 3;
            this.label3.Text = "Check";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.Label_Fracs);
            this.panel5.Controls.Add(this.ComboBox_TreatmentArea);
            this.panel5.Controls.Add(this.Label_TreatmentArea);
            this.panel5.Controls.Add(this.ComboBox_TreatmentCenter);
            this.panel5.Controls.Add(this.Label_TreatmentCenter);
            this.panel5.Controls.Add(this.Label_Fractions);
            this.panel5.Controls.Add(this.Label_Presc);
            this.panel5.Controls.Add(this.Label_PrescDose);
            this.panel5.Controls.Add(this.Label_PlanType);
            this.panel5.Controls.Add(this.ComboBox_PlanType);
            this.panel5.Controls.Add(CustomizeButton);
            this.panel5.Controls.Add(this.ErrorLabel);
            this.panel5.Controls.Add(this.checkBoxJawTracking);
            this.panel5.Location = new System.Drawing.Point(0, 68);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(737, 386);
            this.panel5.TabIndex = 21;
            // 
            // Label_Fracs
            // 
            this.Label_Fracs.AutoSize = true;
            this.Label_Fracs.BackColor = System.Drawing.Color.Transparent;
            this.Label_Fracs.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Fracs.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_Fracs.Location = new System.Drawing.Point(388, 132);
            this.Label_Fracs.Name = "Label_Fracs";
            this.Label_Fracs.Size = new System.Drawing.Size(45, 23);
            this.Label_Fracs.TabIndex = 26;
            this.Label_Fracs.Text = "cGy";
            // 
            // ComboBox_TreatmentArea
            // 
            this.ComboBox_TreatmentArea.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_TreatmentArea.FormattingEnabled = true;
            this.ComboBox_TreatmentArea.Location = new System.Drawing.Point(394, 237);
            this.ComboBox_TreatmentArea.Name = "ComboBox_TreatmentArea";
            this.ComboBox_TreatmentArea.Size = new System.Drawing.Size(128, 31);
            this.ComboBox_TreatmentArea.TabIndex = 25;
            // 
            // Label_TreatmentArea
            // 
            this.Label_TreatmentArea.AutoSize = true;
            this.Label_TreatmentArea.BackColor = System.Drawing.Color.Transparent;
            this.Label_TreatmentArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_TreatmentArea.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_TreatmentArea.Location = new System.Drawing.Point(157, 244);
            this.Label_TreatmentArea.Name = "Label_TreatmentArea";
            this.Label_TreatmentArea.Size = new System.Drawing.Size(160, 25);
            this.Label_TreatmentArea.TabIndex = 24;
            this.Label_TreatmentArea.Text = "Treatment Area";
            // 
            // ComboBox_TreatmentCenter
            // 
            this.ComboBox_TreatmentCenter.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_TreatmentCenter.FormattingEnabled = true;
            this.ComboBox_TreatmentCenter.Location = new System.Drawing.Point(394, 201);
            this.ComboBox_TreatmentCenter.Name = "ComboBox_TreatmentCenter";
            this.ComboBox_TreatmentCenter.Size = new System.Drawing.Size(196, 31);
            this.ComboBox_TreatmentCenter.TabIndex = 23;
            this.ComboBox_TreatmentCenter.SelectedIndexChanged += new System.EventHandler(this.ComboBox_TreatmentCenter_SelectedIndexChanged);
            // 
            // Label_TreatmentCenter
            // 
            this.Label_TreatmentCenter.AutoSize = true;
            this.Label_TreatmentCenter.BackColor = System.Drawing.Color.Transparent;
            this.Label_TreatmentCenter.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_TreatmentCenter.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_TreatmentCenter.Location = new System.Drawing.Point(138, 200);
            this.Label_TreatmentCenter.Name = "Label_TreatmentCenter";
            this.Label_TreatmentCenter.Size = new System.Drawing.Size(179, 25);
            this.Label_TreatmentCenter.TabIndex = 22;
            this.Label_TreatmentCenter.Text = "Treatment Center";
            // 
            // Label_Fractions
            // 
            this.Label_Fractions.AutoSize = true;
            this.Label_Fractions.BackColor = System.Drawing.Color.Transparent;
            this.Label_Fractions.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Fractions.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_Fractions.Location = new System.Drawing.Point(216, 132);
            this.Label_Fractions.Name = "Label_Fractions";
            this.Label_Fractions.Size = new System.Drawing.Size(101, 25);
            this.Label_Fractions.TabIndex = 18;
            this.Label_Fractions.Text = "Fractions";
            // 
            // Label_Presc
            // 
            this.Label_Presc.AutoSize = true;
            this.Label_Presc.BackColor = System.Drawing.Color.Transparent;
            this.Label_Presc.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Presc.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_Presc.Location = new System.Drawing.Point(390, 93);
            this.Label_Presc.Name = "Label_Presc";
            this.Label_Presc.Size = new System.Drawing.Size(45, 23);
            this.Label_Presc.TabIndex = 17;
            this.Label_Presc.Text = "cGy";
            // 
            // Label_PrescDose
            // 
            this.Label_PrescDose.AutoSize = true;
            this.Label_PrescDose.BackColor = System.Drawing.Color.Transparent;
            this.Label_PrescDose.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_PrescDose.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_PrescDose.Location = new System.Drawing.Point(135, 93);
            this.Label_PrescDose.Name = "Label_PrescDose";
            this.Label_PrescDose.Size = new System.Drawing.Size(182, 25);
            this.Label_PrescDose.TabIndex = 15;
            this.Label_PrescDose.Text = "Prescription Dose";
            // 
            // Label_PlanType
            // 
            this.Label_PlanType.AutoSize = true;
            this.Label_PlanType.BackColor = System.Drawing.Color.Transparent;
            this.Label_PlanType.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_PlanType.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_PlanType.Location = new System.Drawing.Point(208, 52);
            this.Label_PlanType.Name = "Label_PlanType";
            this.Label_PlanType.Size = new System.Drawing.Size(109, 25);
            this.Label_PlanType.TabIndex = 5;
            this.Label_PlanType.Text = "Plan Type";
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.ErrorLabel.Location = new System.Drawing.Point(75, 361);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(520, 15);
            this.ErrorLabel.TabIndex = 13;
            this.ErrorLabel.Text = "Only one plan can be checked at a time. Please run the program separately for eac" +
    "h plan type.";
            this.ErrorLabel.Visible = false;
            this.ErrorLabel.Click += new System.EventHandler(this.ErrorLabel_Click);
            // 
            // checkBoxJawTracking
            // 
            this.checkBoxJawTracking.AutoSize = true;
            this.checkBoxJawTracking.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxJawTracking.ForeColor = System.Drawing.Color.SteelBlue;
            this.checkBoxJawTracking.Location = new System.Drawing.Point(567, 56);
            this.checkBoxJawTracking.Name = "checkBoxJawTracking";
            this.checkBoxJawTracking.Size = new System.Drawing.Size(114, 23);
            this.checkBoxJawTracking.TabIndex = 21;
            this.checkBoxJawTracking.Text = "Jaw Tracking";
            this.checkBoxJawTracking.UseVisualStyleBackColor = true;
            // 
            // ConstraintGridView
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConstraintGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.ConstraintGridView.BackgroundColor = System.Drawing.Color.SteelBlue;
            this.ConstraintGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConstraintGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.ConstraintGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConstraintGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Structure,
            this.Type,
            this.Subscript,
            this.Relation,
            this.Value,
            this.Format});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConstraintGridView.DefaultCellStyle = dataGridViewCellStyle10;
            this.ConstraintGridView.GridColor = System.Drawing.Color.SteelBlue;
            this.ConstraintGridView.Location = new System.Drawing.Point(26, 12);
            this.ConstraintGridView.Name = "ConstraintGridView";
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConstraintGridView.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.ConstraintGridView.Size = new System.Drawing.Size(683, 231);
            this.ConstraintGridView.TabIndex = 1;
            // 
            // Structure
            // 
            this.Structure.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Structure.HeaderText = "Structure";
            this.Structure.Name = "Structure";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // Subscript
            // 
            this.Subscript.HeaderText = "Subscript";
            this.Subscript.Name = "Subscript";
            // 
            // Relation
            // 
            this.Relation.HeaderText = "Relation";
            this.Relation.Name = "Relation";
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // Format
            // 
            this.Format.HeaderText = "Format";
            this.Format.Name = "Format";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.checkBox_OAR);
            this.panel1.Controls.Add(this.ValueTB);
            this.panel1.Controls.Add(this.SubscriptTB);
            this.panel1.Controls.Add(this.StructureTB);
            this.panel1.Controls.Add(this.PlotFormButton);
            this.panel1.Controls.Add(this.helpButtonConstraints);
            this.panel1.Controls.Add(this.Combobox_Format);
            this.panel1.Controls.Add(this.Combobox_Relation);
            this.panel1.Controls.Add(this.Combobox_Type);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.LoadFeaturesButton);
            this.panel1.Controls.Add(this.EditAssignedButton);
            this.panel1.Controls.Add(this.AddLabel);
            this.panel1.Controls.Add(this.OkButtonLabel);
            this.panel1.Controls.Add(this.FormatLabel);
            this.panel1.Controls.Add(this.ValueLabel);
            this.panel1.Controls.Add(this.RelationLabel);
            this.panel1.Controls.Add(this.SubscriptLabel);
            this.panel1.Controls.Add(this.TypeLabel);
            this.panel1.Controls.Add(this.StructureLabel);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.DeleteButton);
            this.panel1.Controls.Add(this.OkButton);
            this.panel1.Controls.Add(this.ConstraintGridView);
            this.panel1.Location = new System.Drawing.Point(0, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(737, 389);
            this.panel1.TabIndex = 12;
            this.panel1.Visible = false;
            // 
            // checkBox_OAR
            // 
            this.checkBox_OAR.AutoSize = true;
            this.checkBox_OAR.Checked = true;
            this.checkBox_OAR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_OAR.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_OAR.ForeColor = System.Drawing.Color.SteelBlue;
            this.checkBox_OAR.Location = new System.Drawing.Point(596, 252);
            this.checkBox_OAR.Name = "checkBox_OAR";
            this.checkBox_OAR.Size = new System.Drawing.Size(58, 23);
            this.checkBox_OAR.TabIndex = 27;
            this.checkBox_OAR.Text = "OAR";
            this.checkBox_OAR.UseVisualStyleBackColor = true;
            this.checkBox_OAR.Visible = false;
            // 
            // ValueTB
            // 
            this.ValueTB.Location = new System.Drawing.Point(505, 280);
            this.ValueTB.Name = "ValueTB";
            this.ValueTB.Size = new System.Drawing.Size(56, 20);
            this.ValueTB.TabIndex = 9;
            this.ValueTB.Visible = false;
            // 
            // SubscriptTB
            // 
            this.SubscriptTB.Location = new System.Drawing.Point(286, 306);
            this.SubscriptTB.Name = "SubscriptTB";
            this.SubscriptTB.Size = new System.Drawing.Size(58, 20);
            this.SubscriptTB.TabIndex = 7;
            this.SubscriptTB.Visible = false;
            // 
            // StructureTB
            // 
            this.StructureTB.Location = new System.Drawing.Point(286, 252);
            this.StructureTB.Name = "StructureTB";
            this.StructureTB.Size = new System.Drawing.Size(124, 20);
            this.StructureTB.TabIndex = 5;
            this.StructureTB.Visible = false;
            // 
            // PlotFormButton
            // 
            this.PlotFormButton.BackColor = System.Drawing.Color.SteelBlue;
            this.PlotFormButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlotFormButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.PlotFormButton.Location = new System.Drawing.Point(6, 359);
            this.PlotFormButton.Name = "PlotFormButton";
            this.PlotFormButton.Size = new System.Drawing.Size(157, 27);
            this.PlotFormButton.TabIndex = 25;
            this.PlotFormButton.Text = "Plot DVH";
            this.PlotFormButton.UseVisualStyleBackColor = false;
            this.PlotFormButton.Click += new System.EventHandler(this.PlotFormButton_Click);
            // 
            // helpButtonConstraints
            // 
            this.helpButtonConstraints.BackColor = System.Drawing.Color.SteelBlue;
            this.helpButtonConstraints.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButtonConstraints.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.helpButtonConstraints.Location = new System.Drawing.Point(674, 355);
            this.helpButtonConstraints.Name = "helpButtonConstraints";
            this.helpButtonConstraints.Size = new System.Drawing.Size(52, 30);
            this.helpButtonConstraints.TabIndex = 24;
            this.helpButtonConstraints.Text = "Help";
            this.helpButtonConstraints.UseVisualStyleBackColor = false;
            this.helpButtonConstraints.Click += new System.EventHandler(this.helpButtonConstraints_Click);
            // 
            // Combobox_Format
            // 
            this.Combobox_Format.FormattingEnabled = true;
            this.Combobox_Format.Location = new System.Drawing.Point(505, 304);
            this.Combobox_Format.Name = "Combobox_Format";
            this.Combobox_Format.Size = new System.Drawing.Size(56, 21);
            this.Combobox_Format.TabIndex = 23;
            this.Combobox_Format.Visible = false;
            // 
            // Combobox_Relation
            // 
            this.Combobox_Relation.FormattingEnabled = true;
            this.Combobox_Relation.Location = new System.Drawing.Point(505, 251);
            this.Combobox_Relation.Name = "Combobox_Relation";
            this.Combobox_Relation.Size = new System.Drawing.Size(56, 21);
            this.Combobox_Relation.TabIndex = 22;
            this.Combobox_Relation.Visible = false;
            // 
            // Combobox_Type
            // 
            this.Combobox_Type.FormattingEnabled = true;
            this.Combobox_Type.Location = new System.Drawing.Point(286, 279);
            this.Combobox_Type.Name = "Combobox_Type";
            this.Combobox_Type.Size = new System.Drawing.Size(58, 21);
            this.Combobox_Type.TabIndex = 21;
            this.Combobox_Type.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SteelBlue;
            this.label5.Location = new System.Drawing.Point(202, 252);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 19);
            this.label5.TabIndex = 20;
            this.label5.Text = "Structure";
            this.label5.Visible = false;
            // 
            // LoadFeaturesButton
            // 
            this.LoadFeaturesButton.BackColor = System.Drawing.Color.SteelBlue;
            this.LoadFeaturesButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadFeaturesButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.LoadFeaturesButton.Location = new System.Drawing.Point(6, 333);
            this.LoadFeaturesButton.Name = "LoadFeaturesButton";
            this.LoadFeaturesButton.Size = new System.Drawing.Size(157, 27);
            this.LoadFeaturesButton.TabIndex = 19;
            this.LoadFeaturesButton.Text = "Special Features";
            this.LoadFeaturesButton.UseVisualStyleBackColor = false;
            this.LoadFeaturesButton.Click += new System.EventHandler(this.LoadFeaturesButton_Click);
            // 
            // EditAssignedButton
            // 
            this.EditAssignedButton.BackColor = System.Drawing.Color.SteelBlue;
            this.EditAssignedButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditAssignedButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.EditAssignedButton.Location = new System.Drawing.Point(6, 306);
            this.EditAssignedButton.Name = "EditAssignedButton";
            this.EditAssignedButton.Size = new System.Drawing.Size(159, 27);
            this.EditAssignedButton.TabIndex = 18;
            this.EditAssignedButton.Text = "Structure Matching";
            this.EditAssignedButton.UseVisualStyleBackColor = false;
            this.EditAssignedButton.Click += new System.EventHandler(this.EditAssignedButton_Click);
            // 
            // AddLabel
            // 
            this.AddLabel.AutoSize = true;
            this.AddLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.AddLabel.Location = new System.Drawing.Point(390, 334);
            this.AddLabel.Name = "AddLabel";
            this.AddLabel.Size = new System.Drawing.Size(336, 14);
            this.AddLabel.TabIndex = 17;
            this.AddLabel.Text = "Please ensure all fields are filled before adding a constraint";
            this.AddLabel.Visible = false;
            // 
            // OkButtonLabel
            // 
            this.OkButtonLabel.AutoSize = true;
            this.OkButtonLabel.Location = new System.Drawing.Point(165, 368);
            this.OkButtonLabel.Name = "OkButtonLabel";
            this.OkButtonLabel.Size = new System.Drawing.Size(0, 13);
            this.OkButtonLabel.TabIndex = 16;
            this.OkButtonLabel.Visible = false;
            // 
            // FormatLabel
            // 
            this.FormatLabel.AutoSize = true;
            this.FormatLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormatLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.FormatLabel.Location = new System.Drawing.Point(429, 305);
            this.FormatLabel.Name = "FormatLabel";
            this.FormatLabel.Size = new System.Drawing.Size(58, 19);
            this.FormatLabel.TabIndex = 15;
            this.FormatLabel.Text = "Format";
            this.FormatLabel.Visible = false;
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValueLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.ValueLabel.Location = new System.Drawing.Point(429, 279);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(46, 19);
            this.ValueLabel.TabIndex = 14;
            this.ValueLabel.Text = "Value";
            this.ValueLabel.Visible = false;
            // 
            // RelationLabel
            // 
            this.RelationLabel.AutoSize = true;
            this.RelationLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RelationLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.RelationLabel.Location = new System.Drawing.Point(429, 253);
            this.RelationLabel.Name = "RelationLabel";
            this.RelationLabel.Size = new System.Drawing.Size(65, 19);
            this.RelationLabel.TabIndex = 13;
            this.RelationLabel.Text = "Relation";
            this.RelationLabel.Visible = false;
            // 
            // SubscriptLabel
            // 
            this.SubscriptLabel.AutoSize = true;
            this.SubscriptLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubscriptLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.SubscriptLabel.Location = new System.Drawing.Point(203, 303);
            this.SubscriptLabel.Name = "SubscriptLabel";
            this.SubscriptLabel.Size = new System.Drawing.Size(73, 19);
            this.SubscriptLabel.TabIndex = 12;
            this.SubscriptLabel.Text = "Subscript";
            this.SubscriptLabel.Visible = false;
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.TypeLabel.Location = new System.Drawing.Point(205, 277);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(42, 19);
            this.TypeLabel.TabIndex = 11;
            this.TypeLabel.Text = "Type";
            this.TypeLabel.Visible = false;
            // 
            // StructureLabel
            // 
            this.StructureLabel.AutoSize = true;
            this.StructureLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StructureLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.StructureLabel.Location = new System.Drawing.Point(253, 361);
            this.StructureLabel.Name = "StructureLabel";
            this.StructureLabel.Size = new System.Drawing.Size(65, 18);
            this.StructureLabel.TabIndex = 10;
            this.StructureLabel.Text = "Structure";
            this.StructureLabel.Visible = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.SteelBlue;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.No;
            this.button3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button3.Location = new System.Drawing.Point(380, 354);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(140, 30);
            this.button3.TabIndex = 3;
            this.button3.Text = "New Constraint";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.BackColor = System.Drawing.Color.SteelBlue;
            this.DeleteButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.DeleteButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.DeleteButton.Location = new System.Drawing.Point(527, 354);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(140, 30);
            this.DeleteButton.TabIndex = 2;
            this.DeleteButton.Text = "Delete Selection";
            this.DeleteButton.UseVisualStyleBackColor = false;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.BackColor = System.Drawing.Color.SteelBlue;
            this.OkButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.OkButton.Location = new System.Drawing.Point(6, 277);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(159, 28);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "Change Plan Type";
            this.OkButton.UseVisualStyleBackColor = false;
            this.OkButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // PanelSpecialFeatures
            // 
            this.PanelSpecialFeatures.BackColor = System.Drawing.Color.White;
            this.PanelSpecialFeatures.Controls.Add(this.Button_ParamOptStart);
            this.PanelSpecialFeatures.Controls.Add(this.ML_Label);
            this.PanelSpecialFeatures.Controls.Add(this.TextBox_Volume_ub);
            this.PanelSpecialFeatures.Controls.Add(this.TextBox_Volume_lb);
            this.PanelSpecialFeatures.Controls.Add(this.TextBox_Dose_ub);
            this.PanelSpecialFeatures.Controls.Add(this.TextBox_Dose_lb);
            this.PanelSpecialFeatures.Controls.Add(this.label13);
            this.PanelSpecialFeatures.Controls.Add(this.label12);
            this.PanelSpecialFeatures.Controls.Add(this.label11);
            this.PanelSpecialFeatures.Controls.Add(this.label10);
            this.PanelSpecialFeatures.Controls.Add(this.Label_DVH_VolumeBound);
            this.PanelSpecialFeatures.Controls.Add(this.Label_DVHDoseBound);
            this.PanelSpecialFeatures.Controls.Add(this.SpecialFeatures_Clicked);
            this.PanelSpecialFeatures.Controls.Add(this.Combobox_dvhReport);
            this.PanelSpecialFeatures.Controls.Add(this.DVH_gridView);
            this.PanelSpecialFeatures.Controls.Add(this.Button_AddDVH_Report);
            this.PanelSpecialFeatures.Controls.Add(this.label9);
            this.PanelSpecialFeatures.Controls.Add(this.label8);
            this.PanelSpecialFeatures.Controls.Add(this.label7);
            this.PanelSpecialFeatures.Controls.Add(this.ButtonDeleteParotidSub);
            this.PanelSpecialFeatures.Controls.Add(this.CheckboxSegmentConstraints);
            this.PanelSpecialFeatures.Controls.Add(this.Button_DeleteSubsegments);
            this.PanelSpecialFeatures.Controls.Add(this.Button_StartSegmentation);
            this.PanelSpecialFeatures.Controls.Add(this.Label_Sagittal);
            this.PanelSpecialFeatures.Controls.Add(this.Sagittal_Combobox);
            this.PanelSpecialFeatures.Controls.Add(this.Label_Coronal);
            this.PanelSpecialFeatures.Controls.Add(this.Coronal_Combobox);
            this.PanelSpecialFeatures.Controls.Add(this.Label_AxialSlice);
            this.PanelSpecialFeatures.Controls.Add(this.label1);
            this.PanelSpecialFeatures.Controls.Add(this.Axial_Combobox);
            this.PanelSpecialFeatures.Controls.Add(this.OrganSeg_OrgansCombo);
            this.PanelSpecialFeatures.Controls.Add(this.PriorityLabel);
            this.PanelSpecialFeatures.Controls.Add(this.PriorityRatio_TextBox);
            this.PanelSpecialFeatures.Controls.Add(this.LabelSpecialFeatures);
            this.PanelSpecialFeatures.Controls.Add(this.ButtonDoneFeatures);
            this.PanelSpecialFeatures.Controls.Add(this.CheckBox_ChopParotid);
            this.PanelSpecialFeatures.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PanelSpecialFeatures.Location = new System.Drawing.Point(0, 65);
            this.PanelSpecialFeatures.Name = "PanelSpecialFeatures";
            this.PanelSpecialFeatures.Size = new System.Drawing.Size(737, 386);
            this.PanelSpecialFeatures.TabIndex = 19;
            this.PanelSpecialFeatures.Visible = false;
            // 
            // Button_ParamOptStart
            // 
            this.Button_ParamOptStart.BackColor = System.Drawing.Color.SteelBlue;
            this.Button_ParamOptStart.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_ParamOptStart.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Button_ParamOptStart.Location = new System.Drawing.Point(394, 336);
            this.Button_ParamOptStart.Name = "Button_ParamOptStart";
            this.Button_ParamOptStart.Size = new System.Drawing.Size(56, 22);
            this.Button_ParamOptStart.TabIndex = 54;
            this.Button_ParamOptStart.Text = "Start";
            this.Button_ParamOptStart.UseVisualStyleBackColor = false;
            this.Button_ParamOptStart.Click += new System.EventHandler(this.Button_ParamOptStart_Click);
            // 
            // ML_Label
            // 
            this.ML_Label.AutoSize = true;
            this.ML_Label.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ML_Label.ForeColor = System.Drawing.Color.SteelBlue;
            this.ML_Label.Location = new System.Drawing.Point(376, 313);
            this.ML_Label.Name = "ML_Label";
            this.ML_Label.Size = new System.Drawing.Size(327, 21);
            this.ML_Label.TabIndex = 53;
            this.ML_Label.Text = "4. Run Optimization Parameter Optimization";
            // 
            // TextBox_Volume_ub
            // 
            this.TextBox_Volume_ub.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox_Volume_ub.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Volume_ub.Location = new System.Drawing.Point(607, 277);
            this.TextBox_Volume_ub.Name = "TextBox_Volume_ub";
            this.TextBox_Volume_ub.Size = new System.Drawing.Size(28, 23);
            this.TextBox_Volume_ub.TabIndex = 52;
            this.TextBox_Volume_ub.Text = "100";
            // 
            // TextBox_Volume_lb
            // 
            this.TextBox_Volume_lb.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox_Volume_lb.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Volume_lb.Location = new System.Drawing.Point(527, 277);
            this.TextBox_Volume_lb.Name = "TextBox_Volume_lb";
            this.TextBox_Volume_lb.Size = new System.Drawing.Size(28, 23);
            this.TextBox_Volume_lb.TabIndex = 51;
            this.TextBox_Volume_lb.Text = "0";
            // 
            // TextBox_Dose_ub
            // 
            this.TextBox_Dose_ub.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox_Dose_ub.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Dose_ub.Location = new System.Drawing.Point(443, 277);
            this.TextBox_Dose_ub.Name = "TextBox_Dose_ub";
            this.TextBox_Dose_ub.Size = new System.Drawing.Size(28, 23);
            this.TextBox_Dose_ub.TabIndex = 50;
            this.TextBox_Dose_ub.Text = "100";
            // 
            // TextBox_Dose_lb
            // 
            this.TextBox_Dose_lb.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox_Dose_lb.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Dose_lb.Location = new System.Drawing.Point(376, 277);
            this.TextBox_Dose_lb.Name = "TextBox_Dose_lb";
            this.TextBox_Dose_lb.Size = new System.Drawing.Size(28, 23);
            this.TextBox_Dose_lb.TabIndex = 49;
            this.TextBox_Dose_lb.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.SteelBlue;
            this.label13.Location = new System.Drawing.Point(603, 259);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 15);
            this.label13.TabIndex = 48;
            this.label13.Text = "Upper";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.SteelBlue;
            this.label12.Location = new System.Drawing.Point(443, 259);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 15);
            this.label12.TabIndex = 47;
            this.label12.Text = "Upper";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.SteelBlue;
            this.label11.Location = new System.Drawing.Point(524, 259);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 15);
            this.label11.TabIndex = 46;
            this.label11.Text = "Lower";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.SteelBlue;
            this.label10.Location = new System.Drawing.Point(373, 259);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 15);
            this.label10.TabIndex = 45;
            this.label10.Text = "Lower";
            // 
            // Label_DVH_VolumeBound
            // 
            this.Label_DVH_VolumeBound.AutoSize = true;
            this.Label_DVH_VolumeBound.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_DVH_VolumeBound.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_DVH_VolumeBound.Location = new System.Drawing.Point(524, 240);
            this.Label_DVH_VolumeBound.Name = "Label_DVH_VolumeBound";
            this.Label_DVH_VolumeBound.Size = new System.Drawing.Size(92, 15);
            this.Label_DVH_VolumeBound.TabIndex = 44;
            this.Label_DVH_VolumeBound.Text = "Volume Bounds";
            // 
            // Label_DVHDoseBound
            // 
            this.Label_DVHDoseBound.AutoSize = true;
            this.Label_DVHDoseBound.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_DVHDoseBound.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_DVHDoseBound.Location = new System.Drawing.Point(373, 240);
            this.Label_DVHDoseBound.Name = "Label_DVHDoseBound";
            this.Label_DVHDoseBound.Size = new System.Drawing.Size(77, 15);
            this.Label_DVHDoseBound.TabIndex = 43;
            this.Label_DVHDoseBound.Text = "Dose Bounds";
            // 
            // SpecialFeatures_Clicked
            // 
            this.SpecialFeatures_Clicked.AutoSize = true;
            this.SpecialFeatures_Clicked.Location = new System.Drawing.Point(619, 64);
            this.SpecialFeatures_Clicked.Name = "SpecialFeatures_Clicked";
            this.SpecialFeatures_Clicked.Size = new System.Drawing.Size(104, 23);
            this.SpecialFeatures_Clicked.TabIndex = 42;
            this.SpecialFeatures_Clicked.Text = "beenclicked";
            this.SpecialFeatures_Clicked.UseVisualStyleBackColor = true;
            this.SpecialFeatures_Clicked.Visible = false;
            // 
            // Combobox_dvhReport
            // 
            this.Combobox_dvhReport.FormattingEnabled = true;
            this.Combobox_dvhReport.Location = new System.Drawing.Point(375, 205);
            this.Combobox_dvhReport.Name = "Combobox_dvhReport";
            this.Combobox_dvhReport.Size = new System.Drawing.Size(121, 27);
            this.Combobox_dvhReport.TabIndex = 41;
            // 
            // DVH_gridView
            // 
            this.DVH_gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DVH_gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DVHcolumn_Structure,
            this.DVHcolumn_VolumeBounds,
            this.DVHcolumn_DoseBounds});
            this.DVH_gridView.Location = new System.Drawing.Point(376, 72);
            this.DVH_gridView.Name = "DVH_gridView";
            this.DVH_gridView.Size = new System.Drawing.Size(333, 124);
            this.DVH_gridView.TabIndex = 40;
            // 
            // DVHcolumn_Structure
            // 
            this.DVHcolumn_Structure.HeaderText = "Structure";
            this.DVHcolumn_Structure.Name = "DVHcolumn_Structure";
            // 
            // DVHcolumn_VolumeBounds
            // 
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DVHcolumn_VolumeBounds.DefaultCellStyle = dataGridViewCellStyle12;
            this.DVHcolumn_VolumeBounds.HeaderText = "Volume Bounds";
            this.DVHcolumn_VolumeBounds.Name = "DVHcolumn_VolumeBounds";
            // 
            // DVHcolumn_DoseBounds
            // 
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
            this.DVHcolumn_DoseBounds.DefaultCellStyle = dataGridViewCellStyle13;
            this.DVHcolumn_DoseBounds.HeaderText = "Dose bounds";
            this.DVHcolumn_DoseBounds.Name = "DVHcolumn_DoseBounds";
            // 
            // Button_AddDVH_Report
            // 
            this.Button_AddDVH_Report.BackColor = System.Drawing.Color.SteelBlue;
            this.Button_AddDVH_Report.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_AddDVH_Report.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Button_AddDVH_Report.Location = new System.Drawing.Point(505, 205);
            this.Button_AddDVH_Report.Name = "Button_AddDVH_Report";
            this.Button_AddDVH_Report.Size = new System.Drawing.Size(83, 22);
            this.Button_AddDVH_Report.TabIndex = 39;
            this.Button_AddDVH_Report.Text = "Include DVH";
            this.Button_AddDVH_Report.UseVisualStyleBackColor = false;
            this.Button_AddDVH_Report.Click += new System.EventHandler(this.Button_AddDVH_Report_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.SteelBlue;
            this.label9.Location = new System.Drawing.Point(376, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(219, 21);
            this.label9.TabIndex = 38;
            this.label9.Text = "3. Save DVH images to report";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.SteelBlue;
            this.label8.Location = new System.Drawing.Point(15, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(252, 21);
            this.label8.TabIndex = 37;
            this.label8.Text = "2. Organ sub-structure constraints";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.SteelBlue;
            this.label7.Location = new System.Drawing.Point(12, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(275, 21);
            this.label7.TabIndex = 36;
            this.label7.Text = "1. Heterogeneous parotid constraints";
            // 
            // ButtonDeleteParotidSub
            // 
            this.ButtonDeleteParotidSub.BackColor = System.Drawing.Color.SteelBlue;
            this.ButtonDeleteParotidSub.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonDeleteParotidSub.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ButtonDeleteParotidSub.Location = new System.Drawing.Point(203, 71);
            this.ButtonDeleteParotidSub.Name = "ButtonDeleteParotidSub";
            this.ButtonDeleteParotidSub.Size = new System.Drawing.Size(135, 22);
            this.ButtonDeleteParotidSub.TabIndex = 35;
            this.ButtonDeleteParotidSub.Text = "Delete sub-structures";
            this.ButtonDeleteParotidSub.UseVisualStyleBackColor = false;
            this.ButtonDeleteParotidSub.Click += new System.EventHandler(this.ButtonDeleteParotidSub_Click);
            // 
            // CheckboxSegmentConstraints
            // 
            this.CheckboxSegmentConstraints.AutoSize = true;
            this.CheckboxSegmentConstraints.BackColor = System.Drawing.Color.SteelBlue;
            this.CheckboxSegmentConstraints.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckboxSegmentConstraints.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.CheckboxSegmentConstraints.Location = new System.Drawing.Point(41, 187);
            this.CheckboxSegmentConstraints.Name = "CheckboxSegmentConstraints";
            this.CheckboxSegmentConstraints.Size = new System.Drawing.Size(263, 19);
            this.CheckboxSegmentConstraints.TabIndex = 34;
            this.CheckboxSegmentConstraints.Text = "Apply whole-organ constraints to segments";
            this.CheckboxSegmentConstraints.UseVisualStyleBackColor = false;
            // 
            // Button_DeleteSubsegments
            // 
            this.Button_DeleteSubsegments.BackColor = System.Drawing.Color.SteelBlue;
            this.Button_DeleteSubsegments.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_DeleteSubsegments.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Button_DeleteSubsegments.Location = new System.Drawing.Point(106, 210);
            this.Button_DeleteSubsegments.Name = "Button_DeleteSubsegments";
            this.Button_DeleteSubsegments.Size = new System.Drawing.Size(75, 22);
            this.Button_DeleteSubsegments.TabIndex = 33;
            this.Button_DeleteSubsegments.Text = "Delete";
            this.Button_DeleteSubsegments.UseVisualStyleBackColor = false;
            this.Button_DeleteSubsegments.Click += new System.EventHandler(this.Button_DeleteSubsegments_Click);
            // 
            // Button_StartSegmentation
            // 
            this.Button_StartSegmentation.BackColor = System.Drawing.Color.SteelBlue;
            this.Button_StartSegmentation.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_StartSegmentation.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Button_StartSegmentation.Location = new System.Drawing.Point(41, 210);
            this.Button_StartSegmentation.Name = "Button_StartSegmentation";
            this.Button_StartSegmentation.Size = new System.Drawing.Size(56, 22);
            this.Button_StartSegmentation.TabIndex = 32;
            this.Button_StartSegmentation.Text = "Create";
            this.Button_StartSegmentation.UseVisualStyleBackColor = false;
            this.Button_StartSegmentation.Click += new System.EventHandler(this.Button_StartSegmentation_Click);
            // 
            // Label_Sagittal
            // 
            this.Label_Sagittal.AutoSize = true;
            this.Label_Sagittal.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Sagittal.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_Sagittal.Location = new System.Drawing.Point(9, 331);
            this.Label_Sagittal.Name = "Label_Sagittal";
            this.Label_Sagittal.Size = new System.Drawing.Size(78, 15);
            this.Label_Sagittal.TabIndex = 31;
            this.Label_Sagittal.Text = "Sagittal Slices";
            // 
            // Sagittal_Combobox
            // 
            this.Sagittal_Combobox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sagittal_Combobox.FormattingEnabled = true;
            this.Sagittal_Combobox.Location = new System.Drawing.Point(96, 330);
            this.Sagittal_Combobox.Name = "Sagittal_Combobox";
            this.Sagittal_Combobox.Size = new System.Drawing.Size(56, 23);
            this.Sagittal_Combobox.TabIndex = 30;
            // 
            // Label_Coronal
            // 
            this.Label_Coronal.AutoSize = true;
            this.Label_Coronal.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Coronal.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_Coronal.Location = new System.Drawing.Point(3, 303);
            this.Label_Coronal.Name = "Label_Coronal";
            this.Label_Coronal.Size = new System.Drawing.Size(81, 15);
            this.Label_Coronal.TabIndex = 29;
            this.Label_Coronal.Text = "Coronal Slices";
            // 
            // Coronal_Combobox
            // 
            this.Coronal_Combobox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Coronal_Combobox.FormattingEnabled = true;
            this.Coronal_Combobox.Location = new System.Drawing.Point(96, 302);
            this.Coronal_Combobox.Name = "Coronal_Combobox";
            this.Coronal_Combobox.Size = new System.Drawing.Size(56, 23);
            this.Coronal_Combobox.TabIndex = 28;
            // 
            // Label_AxialSlice
            // 
            this.Label_AxialSlice.AutoSize = true;
            this.Label_AxialSlice.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_AxialSlice.ForeColor = System.Drawing.Color.SteelBlue;
            this.Label_AxialSlice.Location = new System.Drawing.Point(25, 273);
            this.Label_AxialSlice.Name = "Label_AxialSlice";
            this.Label_AxialSlice.Size = new System.Drawing.Size(65, 15);
            this.Label_AxialSlice.TabIndex = 27;
            this.Label_AxialSlice.Text = "Axial Slices";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(50, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 15);
            this.label1.TabIndex = 26;
            this.label1.Text = "Organ";
            // 
            // Axial_Combobox
            // 
            this.Axial_Combobox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Axial_Combobox.FormattingEnabled = true;
            this.Axial_Combobox.Location = new System.Drawing.Point(96, 273);
            this.Axial_Combobox.Name = "Axial_Combobox";
            this.Axial_Combobox.Size = new System.Drawing.Size(56, 23);
            this.Axial_Combobox.TabIndex = 25;
            // 
            // OrganSeg_OrgansCombo
            // 
            this.OrganSeg_OrgansCombo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrganSeg_OrgansCombo.FormattingEnabled = true;
            this.OrganSeg_OrgansCombo.Location = new System.Drawing.Point(96, 245);
            this.OrganSeg_OrgansCombo.Name = "OrganSeg_OrgansCombo";
            this.OrganSeg_OrgansCombo.Size = new System.Drawing.Size(142, 23);
            this.OrganSeg_OrgansCombo.TabIndex = 24;
            // 
            // PriorityLabel
            // 
            this.PriorityLabel.AutoSize = true;
            this.PriorityLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PriorityLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.PriorityLabel.Location = new System.Drawing.Point(50, 103);
            this.PriorityLabel.Name = "PriorityLabel";
            this.PriorityLabel.Size = new System.Drawing.Size(79, 15);
            this.PriorityLabel.TabIndex = 22;
            this.PriorityLabel.Text = "Priority Ratio";
            // 
            // PriorityRatio_TextBox
            // 
            this.PriorityRatio_TextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PriorityRatio_TextBox.ForeColor = System.Drawing.Color.SteelBlue;
            this.PriorityRatio_TextBox.Location = new System.Drawing.Point(135, 98);
            this.PriorityRatio_TextBox.Name = "PriorityRatio_TextBox";
            this.PriorityRatio_TextBox.Size = new System.Drawing.Size(25, 23);
            this.PriorityRatio_TextBox.TabIndex = 21;
            this.PriorityRatio_TextBox.Text = "0.5";
            // 
            // LabelSpecialFeatures
            // 
            this.LabelSpecialFeatures.AutoSize = true;
            this.LabelSpecialFeatures.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelSpecialFeatures.ForeColor = System.Drawing.Color.SteelBlue;
            this.LabelSpecialFeatures.Location = new System.Drawing.Point(250, 1);
            this.LabelSpecialFeatures.Name = "LabelSpecialFeatures";
            this.LabelSpecialFeatures.Size = new System.Drawing.Size(197, 33);
            this.LabelSpecialFeatures.TabIndex = 0;
            this.LabelSpecialFeatures.Text = "Special Features";
            // 
            // ButtonDoneFeatures
            // 
            this.ButtonDoneFeatures.BackColor = System.Drawing.Color.SteelBlue;
            this.ButtonDoneFeatures.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonDoneFeatures.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ButtonDoneFeatures.Location = new System.Drawing.Point(645, 355);
            this.ButtonDoneFeatures.Name = "ButtonDoneFeatures";
            this.ButtonDoneFeatures.Size = new System.Drawing.Size(75, 32);
            this.ButtonDoneFeatures.TabIndex = 2;
            this.ButtonDoneFeatures.Text = "Done";
            this.ButtonDoneFeatures.UseVisualStyleBackColor = false;
            this.ButtonDoneFeatures.Click += new System.EventHandler(this.ButtonDoneFeatures_Click);
            // 
            // CheckBox_ChopParotid
            // 
            this.CheckBox_ChopParotid.AutoSize = true;
            this.CheckBox_ChopParotid.BackColor = System.Drawing.Color.SteelBlue;
            this.CheckBox_ChopParotid.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBox_ChopParotid.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.CheckBox_ChopParotid.Location = new System.Drawing.Point(35, 73);
            this.CheckBox_ChopParotid.Name = "CheckBox_ChopParotid";
            this.CheckBox_ChopParotid.Size = new System.Drawing.Size(162, 19);
            this.CheckBox_ChopParotid.TabIndex = 1;
            this.CheckBox_ChopParotid.Text = "Sub-parotid optimization";
            this.CheckBox_ChopParotid.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.SteelBlue;
            this.panel3.Location = new System.Drawing.Point(0, 553);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(737, 39);
            this.panel3.TabIndex = 14;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.White;
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.button8);
            this.panel7.Controls.Add(this.button9);
            this.panel7.Controls.Add(this.label16);
            this.panel7.Controls.Add(this.label17);
            this.panel7.Controls.Add(this.label18);
            this.panel7.Controls.Add(this.label19);
            this.panel7.Controls.Add(this.label20);
            this.panel7.Controls.Add(this.label21);
            this.panel7.Controls.Add(this.label22);
            this.panel7.Controls.Add(this.label23);
            this.panel7.Controls.Add(this.textBox8);
            this.panel7.Controls.Add(this.textBox9);
            this.panel7.Controls.Add(this.textBox10);
            this.panel7.Controls.Add(this.textBox11);
            this.panel7.Controls.Add(this.textBox12);
            this.panel7.Controls.Add(this.textBox13);
            this.panel7.Controls.Add(this.button10);
            this.panel7.Controls.Add(this.button11);
            this.panel7.Controls.Add(this.button12);
            this.panel7.Location = new System.Drawing.Point(606, 26);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(737, 389);
            this.panel7.TabIndex = 12;
            this.panel7.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(62, 272);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 18);
            this.label6.TabIndex = 20;
            this.label6.Text = "Structure";
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Maroon;
            this.button8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(8, 293);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(185, 27);
            this.button8.TabIndex = 19;
            this.button8.Text = "Select Special Features";
            this.button8.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Maroon;
            this.button9.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(6, 324);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(187, 27);
            this.button9.TabIndex = 18;
            this.button9.Text = "View / Edit Assigned Structures";
            this.button9.UseVisualStyleBackColor = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label16.Location = new System.Drawing.Point(390, 338);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(336, 14);
            this.label16.TabIndex = 17;
            this.label16.Text = "Please ensure all fields are filled before adding a constraint";
            this.label16.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(165, 368);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(0, 13);
            this.label17.TabIndex = 16;
            this.label17.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label18.Location = new System.Drawing.Point(615, 273);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(52, 18);
            this.label18.TabIndex = 15;
            this.label18.Text = "Format";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label19.Location = new System.Drawing.Point(507, 273);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 18);
            this.label19.TabIndex = 14;
            this.label19.Text = "Value";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label20.Location = new System.Drawing.Point(408, 273);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(59, 18);
            this.label20.TabIndex = 13;
            this.label20.Text = "Relation";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label21.Location = new System.Drawing.Point(305, 272);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 18);
            this.label21.TabIndex = 12;
            this.label21.Text = "Subscript";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label22.Location = new System.Drawing.Point(203, 272);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(37, 18);
            this.label22.TabIndex = 11;
            this.label22.Text = "Type";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label23.Location = new System.Drawing.Point(279, 354);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(65, 18);
            this.label23.TabIndex = 10;
            this.label23.Text = "Structure";
            this.label23.Visible = false;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(618, 250);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(91, 20);
            this.textBox8.TabIndex = 10;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(411, 250);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(93, 20);
            this.textBox9.TabIndex = 8;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(308, 250);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(97, 20);
            this.textBox10.TabIndex = 7;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(206, 250);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(96, 20);
            this.textBox11.TabIndex = 6;
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(65, 250);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(135, 20);
            this.textBox12.TabIndex = 5;
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(510, 250);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(102, 20);
            this.textBox13.TabIndex = 9;
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Maroon;
            this.button10.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(420, 303);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(140, 30);
            this.button10.TabIndex = 3;
            this.button10.Text = "Add Constraint";
            this.button10.UseVisualStyleBackColor = false;
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button11.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(566, 302);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(152, 30);
            this.button11.TabIndex = 2;
            this.button11.Text = "Delete Constraint";
            this.button11.UseVisualStyleBackColor = false;
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.Maroon;
            this.button12.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.Location = new System.Drawing.Point(6, 354);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(187, 28);
            this.button12.TabIndex = 0;
            this.button12.Text = "Change Plan Type";
            this.button12.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.SteelBlue;
            this.textBox1.Location = new System.Drawing.Point(111, 498);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(174, 22);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "PatientReport";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // BrowseButton
            // 
            this.BrowseButton.BackColor = System.Drawing.Color.SteelBlue;
            this.BrowseButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BrowseButton.Location = new System.Drawing.Point(111, 524);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(70, 26);
            this.BrowseButton.TabIndex = 5;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = false;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.MediumPurple;
            this.StartButton.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.StartButton.Location = new System.Drawing.Point(589, 496);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(110, 27);
            this.StartButton.TabIndex = 7;
            this.StartButton.Text = "Create Plan";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.MediumPurple;
            this.button2.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Location = new System.Drawing.Point(589, 525);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 27);
            this.button2.TabIndex = 8;
            this.button2.Text = "Create Report";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Snow;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(0, 499);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 19);
            this.label4.TabIndex = 9;
            this.label4.Text = "Report Name:";
            // 
            // StartErrorLabel
            // 
            this.StartErrorLabel.AutoSize = true;
            this.StartErrorLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartErrorLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.StartErrorLabel.Location = new System.Drawing.Point(324, 472);
            this.StartErrorLabel.Name = "StartErrorLabel";
            this.StartErrorLabel.Size = new System.Drawing.Size(208, 19);
            this.StartErrorLabel.TabIndex = 14;
            this.StartErrorLabel.Text = "Please specify a save location.";
            this.StartErrorLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.StartErrorLabel.Visible = false;
            // 
            // IterationsLabel
            // 
            this.IterationsLabel.AutoSize = true;
            this.IterationsLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IterationsLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.IterationsLabel.Location = new System.Drawing.Point(425, 529);
            this.IterationsLabel.Name = "IterationsLabel";
            this.IterationsLabel.Size = new System.Drawing.Size(158, 19);
            this.IterationsLabel.TabIndex = 19;
            this.IterationsLabel.Text = "Optimization Iterations";
            // 
            // IterationsTextBox
            // 
            this.IterationsTextBox.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IterationsTextBox.ForeColor = System.Drawing.Color.SteelBlue;
            this.IterationsTextBox.Location = new System.Drawing.Point(399, 524);
            this.IterationsTextBox.Name = "IterationsTextBox";
            this.IterationsTextBox.Size = new System.Drawing.Size(20, 27);
            this.IterationsTextBox.TabIndex = 20;
            this.IterationsTextBox.Text = "2";
            // 
            // SaveCheck
            // 
            this.SaveCheck.AutoSize = true;
            this.SaveCheck.Checked = true;
            this.SaveCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SaveCheck.Location = new System.Drawing.Point(113, 472);
            this.SaveCheck.Name = "SaveCheck";
            this.SaveCheck.Size = new System.Drawing.Size(15, 14);
            this.SaveCheck.TabIndex = 21;
            this.SaveCheck.UseVisualStyleBackColor = true;
            // 
            // LabelSaveCheck
            // 
            this.LabelSaveCheck.AutoSize = true;
            this.LabelSaveCheck.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelSaveCheck.ForeColor = System.Drawing.Color.SteelBlue;
            this.LabelSaveCheck.Location = new System.Drawing.Point(8, 469);
            this.LabelSaveCheck.Name = "LabelSaveCheck";
            this.LabelSaveCheck.Size = new System.Drawing.Size(96, 19);
            this.LabelSaveCheck.TabIndex = 22;
            this.LabelSaveCheck.Text = "Save Report:";
            // 
            // LocationLabel
            // 
            this.LocationLabel.AutoSize = true;
            this.LocationLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LocationLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.LocationLabel.Location = new System.Drawing.Point(185, 531);
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new System.Drawing.Size(174, 15);
            this.LocationLabel.TabIndex = 23;
            this.LocationLabel.Text = "Please specify a save location.";
            this.LocationLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.LocationLabel.Visible = false;
            // 
            // LatexPanel
            // 
            this.LatexPanel.BackColor = System.Drawing.Color.White;
            this.LatexPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LatexPanel.BackgroundImage")));
            this.LatexPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.LatexPanel.Controls.Add(this.DoneLatexButton);
            this.LatexPanel.Controls.Add(this.StartErrorLabel);
            this.LatexPanel.Location = new System.Drawing.Point(0, 0);
            this.LatexPanel.Name = "LatexPanel";
            this.LatexPanel.Size = new System.Drawing.Size(734, 591);
            this.LatexPanel.TabIndex = 25;
            this.LatexPanel.Visible = false;
            // 
            // DoneLatexButton
            // 
            this.DoneLatexButton.BackColor = System.Drawing.Color.SteelBlue;
            this.DoneLatexButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoneLatexButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.DoneLatexButton.Location = new System.Drawing.Point(624, 544);
            this.DoneLatexButton.Name = "DoneLatexButton";
            this.DoneLatexButton.Size = new System.Drawing.Size(85, 35);
            this.DoneLatexButton.TabIndex = 0;
            this.DoneLatexButton.Text = "Done";
            this.DoneLatexButton.UseVisualStyleBackColor = false;
            this.DoneLatexButton.Click += new System.EventHandler(this.DoneLatexButton_Click);
            // 
            // PlotPanel
            // 
            this.PlotPanel.BackColor = System.Drawing.Color.White;
            this.PlotPanel.Controls.Add(this.DVHLabel);
            this.PlotPanel.Controls.Add(this.PlotButton);
            this.PlotPanel.Controls.Add(this.PlotCombobox);
            this.PlotPanel.Controls.Add(this.PlotDoneButton);
            this.PlotPanel.Location = new System.Drawing.Point(0, 65);
            this.PlotPanel.Name = "PlotPanel";
            this.PlotPanel.Size = new System.Drawing.Size(734, 494);
            this.PlotPanel.TabIndex = 25;
            this.PlotPanel.Visible = false;
            // 
            // DVHLabel
            // 
            this.DVHLabel.AutoSize = true;
            this.DVHLabel.BackColor = System.Drawing.Color.Snow;
            this.DVHLabel.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DVHLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.DVHLabel.Location = new System.Drawing.Point(12, 25);
            this.DVHLabel.Name = "DVHLabel";
            this.DVHLabel.Size = new System.Drawing.Size(173, 39);
            this.DVHLabel.TabIndex = 11;
            this.DVHLabel.Text = "DVH Plotter";
            // 
            // PlotButton
            // 
            this.PlotButton.BackColor = System.Drawing.Color.SteelBlue;
            this.PlotButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlotButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.PlotButton.Location = new System.Drawing.Point(8, 437);
            this.PlotButton.Name = "PlotButton";
            this.PlotButton.Size = new System.Drawing.Size(109, 31);
            this.PlotButton.TabIndex = 10;
            this.PlotButton.Text = "Plot";
            this.PlotButton.UseVisualStyleBackColor = false;
            this.PlotButton.Click += new System.EventHandler(this.PlotButton_Click);
            // 
            // PlotCombobox
            // 
            this.PlotCombobox.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlotCombobox.FormattingEnabled = true;
            this.PlotCombobox.Location = new System.Drawing.Point(129, 442);
            this.PlotCombobox.Name = "PlotCombobox";
            this.PlotCombobox.Size = new System.Drawing.Size(156, 21);
            this.PlotCombobox.TabIndex = 9;
            // 
            // PlotDoneButton
            // 
            this.PlotDoneButton.BackColor = System.Drawing.Color.SteelBlue;
            this.PlotDoneButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlotDoneButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.PlotDoneButton.Location = new System.Drawing.Point(549, 437);
            this.PlotDoneButton.Name = "PlotDoneButton";
            this.PlotDoneButton.Size = new System.Drawing.Size(171, 31);
            this.PlotDoneButton.TabIndex = 8;
            this.PlotDoneButton.Text = "Finish Plotting";
            this.PlotDoneButton.UseVisualStyleBackColor = false;
            this.PlotDoneButton.Click += new System.EventHandler(this.PlotDoneButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(734, 591);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.LocationLabel);
            this.Controls.Add(this.LabelSaveCheck);
            this.Controls.Add(this.SaveCheck);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.IterationsLabel);
            this.Controls.Add(this.IterationsTextBox);
            this.Controls.Add(this.AssigningPanel);
            this.Controls.Add(this.PanelSpecialFeatures);
            this.Controls.Add(this.PlotPanel);
            this.Controls.Add(this.LatexPanel);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Plan N Check";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.AssigningPanel.ResumeLayout(false);
            this.AssigningPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssignStructGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.conStructGridView)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConstraintGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PanelSpecialFeatures.ResumeLayout(false);
            this.PanelSpecialFeatures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DVH_gridView)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.LatexPanel.ResumeLayout(false);
            this.LatexPanel.PerformLayout();
            this.PlotPanel.ResumeLayout(false);
            this.PlotPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView ConstraintGridView;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Structure;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subscript;
        private System.Windows.Forms.DataGridViewTextBoxColumn Relation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Format;
        private System.Windows.Forms.Label FormatLabel;
        private System.Windows.Forms.Label ValueLabel;
        private System.Windows.Forms.Label RelationLabel;
        private System.Windows.Forms.Label SubscriptLabel;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.Label StructureLabel;
        private System.Windows.Forms.TextBox SubscriptTB;
        private System.Windows.Forms.TextBox StructureTB;
        private System.Windows.Forms.TextBox ValueTB;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Label OkButtonLabel;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Label AddLabel;
        private System.Windows.Forms.Panel AssigningPanel;
        private System.Windows.Forms.ComboBox DicomComboBox;
        private System.Windows.Forms.Button AddStructureButton;
        private System.Windows.Forms.Button removeAssignedButton;
        private System.Windows.Forms.Label AssigningLabel;
        private System.Windows.Forms.DataGridView AssignStructGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridView conStructGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button EditAssignedButton;
        private System.Windows.Forms.Label assignedLabel;
        private System.Windows.Forms.Label StructLabel;
        private System.Windows.Forms.Button FinishedEdtingButton;
        private System.Windows.Forms.Panel PanelSpecialFeatures;
        private System.Windows.Forms.CheckBox CheckBox_ChopParotid;
        private System.Windows.Forms.Label LabelSpecialFeatures;
        private System.Windows.Forms.Button LoadFeaturesButton;
        private System.Windows.Forms.Button ButtonDoneFeatures;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label StartErrorLabel;
        private System.Windows.Forms.Label IterationsLabel;
        private System.Windows.Forms.TextBox IterationsTextBox;
        private System.Windows.Forms.CheckBox SaveCheck;
        private System.Windows.Forms.Label LabelSaveCheck;
        private System.Windows.Forms.Label LocationLabel;
        private ComboBox Combobox_Format;
        private ComboBox Combobox_Relation;
        private ComboBox Combobox_Type;
        private Label label24;
        private Label label3;
        private Button helpButtonConstraints;
        private Panel LatexPanel;
        private Button DoneLatexButton;
        private Panel PlotPanel;
        private Label DVHLabel;
        private Button PlotButton;
        private ComboBox PlotCombobox;
        private Button PlotDoneButton;
        private Button PlotFormButton;
        private Label PriorityLabel;
        private TextBox PriorityRatio_TextBox;
        private Label Label_Sagittal;
        private ComboBox Sagittal_Combobox;
        private Label Label_Coronal;
        private ComboBox Coronal_Combobox;
        private Label Label_AxialSlice;
        private Label label1;
        private ComboBox Axial_Combobox;
        private ComboBox OrganSeg_OrgansCombo;
        private Button Button_StartSegmentation;
        private Button Button_DeleteSubsegments;
        private CheckBox CheckboxSegmentConstraints;
        private Button ButtonDeleteParotidSub;
        private Label label8;
        private Label label7;
        private Button Button_AddDVH_Report;
        private Label label9;
        private ComboBox Combobox_dvhReport;
        private DataGridView DVH_gridView;
        private CheckBox SpecialFeatures_Clicked;
        private DataGridViewTextBoxColumn DVHcolumn_Structure;
        private DataGridViewTextBoxColumn DVHcolumn_VolumeBounds;
        private DataGridViewTextBoxColumn DVHcolumn_DoseBounds;
        private TextBox TextBox_Volume_ub;
        private TextBox TextBox_Volume_lb;
        private TextBox TextBox_Dose_ub;
        private TextBox TextBox_Dose_lb;
        private Label label13;
        private Label label12;
        private Label label11;
        private Label label10;
        private Label Label_DVH_VolumeBound;
        private Label Label_DVHDoseBound;
        private CheckBox checkBox_OAR;
        private Button Button_ParamOptStart;
        private Label ML_Label;
        private CheckBox checkBoxJawTracking;
        private ComboBox ComboBox_PlanType;
        private Label Label_Fractions;
        private Label Label_Presc;
        private Label Label_PrescDose;
        private Label Label_PlanType;
        private ComboBox ComboBox_TreatmentArea;
        private Label Label_TreatmentArea;
        private ComboBox ComboBox_TreatmentCenter;
        private Label Label_TreatmentCenter;
        private Label Label_Fracs;
    }
}