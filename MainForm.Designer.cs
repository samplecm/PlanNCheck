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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
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
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.ConstraintGridView = new System.Windows.Forms.DataGridView();
            this.Structure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subscript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Relation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Format = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.groupBox1.SuspendLayout();
            this.AssigningPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssignStructGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.conStructGridView)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConstraintGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.PanelSpecialFeatures.SuspendLayout();
            this.panel7.SuspendLayout();
            this.LatexPanel.SuspendLayout();
            this.PlotPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CustomizeButton
            // 
            CustomizeButton.BackColor = System.Drawing.Color.DarkTurquoise;
            CustomizeButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CustomizeButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            CustomizeButton.Location = new System.Drawing.Point(271, 325);
            CustomizeButton.Name = "CustomizeButton";
            CustomizeButton.Size = new System.Drawing.Size(162, 34);
            CustomizeButton.TabIndex = 11;
            CustomizeButton.Text = "Customize";
            CustomizeButton.UseVisualStyleBackColor = false;
            CustomizeButton.Click += new System.EventHandler(this.CustomizeButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.DarkTurquoise;
            this.label2.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(249, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 42);
            this.label2.TabIndex = 2;
            this.label2.Text = "Plan";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.checkBox19);
            this.groupBox1.Controls.Add(this.checkBox18);
            this.groupBox1.Controls.Add(this.checkBox17);
            this.groupBox1.Controls.Add(this.checkBox16);
            this.groupBox1.Controls.Add(this.checkBox15);
            this.groupBox1.Controls.Add(this.checkBox14);
            this.groupBox1.Controls.Add(this.checkBox13);
            this.groupBox1.Controls.Add(this.checkBox12);
            this.groupBox1.Controls.Add(this.checkBox11);
            this.groupBox1.Controls.Add(this.checkBox10);
            this.groupBox1.Controls.Add(this.checkBox9);
            this.groupBox1.Controls.Add(this.checkBox8);
            this.groupBox1.Controls.Add(this.checkBox7);
            this.groupBox1.Controls.Add(this.checkBox6);
            this.groupBox1.Controls.Add(this.checkBox5);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.groupBox1.Location = new System.Drawing.Point(43, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 287);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox19.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox19.Location = new System.Drawing.Point(304, 228);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(312, 27);
            this.checkBox19.TabIndex = 18;
            this.checkBox19.Text = "Sarcoma Retroperitoneal and Pelvis";
            this.checkBox19.UseVisualStyleBackColor = true;
            this.checkBox19.CheckedChanged += new System.EventHandler(this.checkBox19_CheckedChanged);
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox18.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox18.Location = new System.Drawing.Point(304, 202);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(138, 27);
            this.checkBox18.TabIndex = 17;
            this.checkBox18.Text = "Sarcoma Limb";
            this.checkBox18.UseVisualStyleBackColor = true;
            this.checkBox18.CheckedChanged += new System.EventHandler(this.checkBox18_CheckedChanged);
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox17.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox17.Location = new System.Drawing.Point(304, 176);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(97, 27);
            this.checkBox17.TabIndex = 16;
            this.checkBox17.Text = "Prostate";
            this.checkBox17.UseVisualStyleBackColor = true;
            this.checkBox17.CheckedChanged += new System.EventHandler(this.checkBox17_CheckedChanged);
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox16.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox16.Location = new System.Drawing.Point(304, 150);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(342, 27);
            this.checkBox16.TabIndex = 15;
            this.checkBox16.Text = "P1 Prostate/pelvis (plus P2 LDR Brachy)";
            this.checkBox16.UseVisualStyleBackColor = true;
            this.checkBox16.CheckedChanged += new System.EventHandler(this.checkBox16_CheckedChanged);
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox15.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox15.Location = new System.Drawing.Point(304, 124);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(252, 27);
            this.checkBox15.TabIndex = 14;
            this.checkBox15.Text = "Post-operative prostate bed";
            this.checkBox15.UseVisualStyleBackColor = true;
            this.checkBox15.CheckedChanged += new System.EventHandler(this.checkBox15_CheckedChanged);
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox14.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox14.Location = new System.Drawing.Point(304, 98);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(96, 27);
            this.checkBox14.TabIndex = 13;
            this.checkBox14.Text = "Orator 2";
            this.checkBox14.UseVisualStyleBackColor = true;
            this.checkBox14.CheckedChanged += new System.EventHandler(this.checkBox14_CheckedChanged);
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox13.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox13.Location = new System.Drawing.Point(304, 72);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(172, 27);
            this.checkBox13.TabIndex = 12;
            this.checkBox13.Text = "Lymphoma Thorax";
            this.checkBox13.UseVisualStyleBackColor = true;
            this.checkBox13.CheckedChanged += new System.EventHandler(this.checkBox13_CheckedChanged);
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox12.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox12.Location = new System.Drawing.Point(304, 46);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(165, 27);
            this.checkBox12.TabIndex = 11;
            this.checkBox12.Text = "Lymphoma Pelvis";
            this.checkBox12.UseVisualStyleBackColor = true;
            this.checkBox12.CheckedChanged += new System.EventHandler(this.checkBox12_CheckedChanged);
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox11.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox11.Location = new System.Drawing.Point(304, 20);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(143, 27);
            this.checkBox11.TabIndex = 10;
            this.checkBox11.Text = "Lymphoma H&N";
            this.checkBox11.UseVisualStyleBackColor = true;
            this.checkBox11.CheckedChanged += new System.EventHandler(this.checkBox11_CheckedChanged);
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox10.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox10.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox10.Location = new System.Drawing.Point(19, 254);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(66, 27);
            this.checkBox10.TabIndex = 9;
            this.checkBox10.Text = "Lung";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox9.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox9.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox9.Location = new System.Drawing.Point(19, 228);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(231, 27);
            this.checkBox9.TabIndex = 8;
            this.checkBox9.Text = "Hepatocellular carcinoma";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox8.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox8.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox8.Location = new System.Drawing.Point(19, 202);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(147, 27);
            this.checkBox8.TabIndex = 7;
            this.checkBox8.Text = "Head and Neck";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox7.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox7.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox7.Location = new System.Drawing.Point(19, 176);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(79, 27);
            this.checkBox7.TabIndex = 6;
            this.checkBox7.Text = "Gynae";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox6.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox6.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox6.Location = new System.Drawing.Point(19, 150);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(85, 27);
            this.checkBox6.TabIndex = 5;
            this.checkBox6.Text = "Glioma";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox5.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox5.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox5.Location = new System.Drawing.Point(19, 124);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(215, 27);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "Thoracic/ GE Esophagus";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox4.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox4.Location = new System.Drawing.Point(19, 98);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(175, 27);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "Breast / Chestwall";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox3.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox3.Location = new System.Drawing.Point(19, 72);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(131, 27);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "Brain Glioma";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox2.Location = new System.Drawing.Point(19, 46);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(168, 27);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Brain Metastases";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.checkBox1.Location = new System.Drawing.Point(19, 20);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(112, 27);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Anal Canal";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
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
            this.AddStructureButton.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.removeAssignedButton.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.AssigningLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.AssigningLabel.Location = new System.Drawing.Point(313, 18);
            this.AssigningLabel.Name = "AssigningLabel";
            this.AssigningLabel.Size = new System.Drawing.Size(424, 51);
            this.AssigningLabel.TabIndex = 4;
            this.AssigningLabel.Text = "Click on a constrained structure to view / update assigned dicom structures.";
            // 
            // AssignStructGridView
            // 
            this.AssignStructGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.AssignStructGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AssignStructGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.AssignStructGridView.GridColor = System.Drawing.SystemColors.HotTrack;
            this.AssignStructGridView.Location = new System.Drawing.Point(373, 132);
            this.AssignStructGridView.Name = "AssignStructGridView";
            this.AssignStructGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AssignStructGridView.Size = new System.Drawing.Size(307, 106);
            this.AssignStructGridView.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn2.HeaderText = "Assigned Structure";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // conStructGridView
            // 
            this.conStructGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.conStructGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.conStructGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.conStructGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.conStructGridView.GridColor = System.Drawing.SystemColors.HotTrack;
            this.conStructGridView.Location = new System.Drawing.Point(35, 26);
            this.conStructGridView.Name = "conStructGridView";
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
            this.FinishedEdtingButton.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.panel2.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.label24.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.label3.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.panel5.Controls.Add(CustomizeButton);
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Controls.Add(this.ErrorLabel);
            this.panel5.Location = new System.Drawing.Point(0, 65);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(737, 386);
            this.panel5.TabIndex = 21;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.ErrorLabel.Location = new System.Drawing.Point(75, 361);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(520, 15);
            this.ErrorLabel.TabIndex = 13;
            this.ErrorLabel.Text = "Only one plan can be checked at a time. Please run the program separately for eac" +
    "h plan type.";
            this.ErrorLabel.Visible = false;
            this.ErrorLabel.Click += new System.EventHandler(this.ErrorLabel_Click);
            // 
            // ConstraintGridView
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConstraintGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.ConstraintGridView.BackgroundColor = System.Drawing.Color.DarkTurquoise;
            this.ConstraintGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ConstraintGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConstraintGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Structure,
            this.Type,
            this.Subscript,
            this.Relation,
            this.Value,
            this.Format});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DarkTurquoise;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConstraintGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.ConstraintGridView.GridColor = System.Drawing.Color.DarkTurquoise;
            this.ConstraintGridView.Location = new System.Drawing.Point(26, 12);
            this.ConstraintGridView.Name = "ConstraintGridView";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConstraintGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
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
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // ValueTB
            // 
            this.ValueTB.Location = new System.Drawing.Point(662, 279);
            this.ValueTB.Name = "ValueTB";
            this.ValueTB.Size = new System.Drawing.Size(56, 20);
            this.ValueTB.TabIndex = 9;
            // 
            // SubscriptTB
            // 
            this.SubscriptTB.Location = new System.Drawing.Point(443, 305);
            this.SubscriptTB.Name = "SubscriptTB";
            this.SubscriptTB.Size = new System.Drawing.Size(58, 20);
            this.SubscriptTB.TabIndex = 7;
            // 
            // StructureTB
            // 
            this.StructureTB.Location = new System.Drawing.Point(443, 251);
            this.StructureTB.Name = "StructureTB";
            this.StructureTB.Size = new System.Drawing.Size(124, 20);
            this.StructureTB.TabIndex = 5;
            // 
            // PlotFormButton
            // 
            this.PlotFormButton.BackColor = System.Drawing.Color.DarkTurquoise;
            this.PlotFormButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlotFormButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.PlotFormButton.Location = new System.Drawing.Point(8, 263);
            this.PlotFormButton.Name = "PlotFormButton";
            this.PlotFormButton.Size = new System.Drawing.Size(157, 27);
            this.PlotFormButton.TabIndex = 25;
            this.PlotFormButton.Text = "Plot DVH";
            this.PlotFormButton.UseVisualStyleBackColor = false;
            this.PlotFormButton.Click += new System.EventHandler(this.PlotFormButton_Click);
            // 
            // helpButtonConstraints
            // 
            this.helpButtonConstraints.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.Combobox_Format.Location = new System.Drawing.Point(662, 303);
            this.Combobox_Format.Name = "Combobox_Format";
            this.Combobox_Format.Size = new System.Drawing.Size(56, 21);
            this.Combobox_Format.TabIndex = 23;
            // 
            // Combobox_Relation
            // 
            this.Combobox_Relation.FormattingEnabled = true;
            this.Combobox_Relation.Location = new System.Drawing.Point(662, 250);
            this.Combobox_Relation.Name = "Combobox_Relation";
            this.Combobox_Relation.Size = new System.Drawing.Size(56, 21);
            this.Combobox_Relation.TabIndex = 22;
            // 
            // Combobox_Type
            // 
            this.Combobox_Type.FormattingEnabled = true;
            this.Combobox_Type.Location = new System.Drawing.Point(443, 278);
            this.Combobox_Type.Name = "Combobox_Type";
            this.Combobox_Type.Size = new System.Drawing.Size(58, 21);
            this.Combobox_Type.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label5.Location = new System.Drawing.Point(359, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 19);
            this.label5.TabIndex = 20;
            this.label5.Text = "Structure";
            // 
            // LoadFeaturesButton
            // 
            this.LoadFeaturesButton.BackColor = System.Drawing.Color.DarkTurquoise;
            this.LoadFeaturesButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadFeaturesButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.LoadFeaturesButton.Location = new System.Drawing.Point(8, 293);
            this.LoadFeaturesButton.Name = "LoadFeaturesButton";
            this.LoadFeaturesButton.Size = new System.Drawing.Size(157, 27);
            this.LoadFeaturesButton.TabIndex = 19;
            this.LoadFeaturesButton.Text = "Special Features";
            this.LoadFeaturesButton.UseVisualStyleBackColor = false;
            this.LoadFeaturesButton.Click += new System.EventHandler(this.LoadFeaturesButton_Click);
            // 
            // EditAssignedButton
            // 
            this.EditAssignedButton.BackColor = System.Drawing.Color.DarkTurquoise;
            this.EditAssignedButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditAssignedButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.EditAssignedButton.Location = new System.Drawing.Point(6, 324);
            this.EditAssignedButton.Name = "EditAssignedButton";
            this.EditAssignedButton.Size = new System.Drawing.Size(159, 27);
            this.EditAssignedButton.TabIndex = 18;
            this.EditAssignedButton.Text = "Assigned Structures";
            this.EditAssignedButton.UseVisualStyleBackColor = false;
            this.EditAssignedButton.Click += new System.EventHandler(this.EditAssignedButton_Click);
            // 
            // AddLabel
            // 
            this.AddLabel.AutoSize = true;
            this.AddLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
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
            this.FormatLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.FormatLabel.Location = new System.Drawing.Point(586, 304);
            this.FormatLabel.Name = "FormatLabel";
            this.FormatLabel.Size = new System.Drawing.Size(58, 19);
            this.FormatLabel.TabIndex = 15;
            this.FormatLabel.Text = "Format";
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValueLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.ValueLabel.Location = new System.Drawing.Point(586, 278);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(46, 19);
            this.ValueLabel.TabIndex = 14;
            this.ValueLabel.Text = "Value";
            // 
            // RelationLabel
            // 
            this.RelationLabel.AutoSize = true;
            this.RelationLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RelationLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.RelationLabel.Location = new System.Drawing.Point(586, 252);
            this.RelationLabel.Name = "RelationLabel";
            this.RelationLabel.Size = new System.Drawing.Size(65, 19);
            this.RelationLabel.TabIndex = 13;
            this.RelationLabel.Text = "Relation";
            // 
            // SubscriptLabel
            // 
            this.SubscriptLabel.AutoSize = true;
            this.SubscriptLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubscriptLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.SubscriptLabel.Location = new System.Drawing.Point(360, 302);
            this.SubscriptLabel.Name = "SubscriptLabel";
            this.SubscriptLabel.Size = new System.Drawing.Size(73, 19);
            this.SubscriptLabel.TabIndex = 12;
            this.SubscriptLabel.Text = "Subscript";
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.TypeLabel.Location = new System.Drawing.Point(362, 276);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(42, 19);
            this.TypeLabel.TabIndex = 11;
            this.TypeLabel.Text = "Type";
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
            this.button3.BackColor = System.Drawing.Color.DarkTurquoise;
            this.button3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button3.Location = new System.Drawing.Point(380, 354);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(140, 30);
            this.button3.TabIndex = 3;
            this.button3.Text = "Add Constraint";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.BackColor = System.Drawing.Color.DarkTurquoise;
            this.DeleteButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.DeleteButton.Location = new System.Drawing.Point(527, 354);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(140, 30);
            this.DeleteButton.TabIndex = 2;
            this.DeleteButton.Text = "Delete Constraint";
            this.DeleteButton.UseVisualStyleBackColor = false;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.BackColor = System.Drawing.Color.DarkTurquoise;
            this.OkButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.OkButton.Location = new System.Drawing.Point(6, 354);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(159, 28);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "Plan Type";
            this.OkButton.UseVisualStyleBackColor = false;
            this.OkButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // PanelSpecialFeatures
            // 
            this.PanelSpecialFeatures.BackColor = System.Drawing.Color.White;
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
            // LabelSpecialFeatures
            // 
            this.LabelSpecialFeatures.AutoSize = true;
            this.LabelSpecialFeatures.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelSpecialFeatures.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.LabelSpecialFeatures.Location = new System.Drawing.Point(30, 35);
            this.LabelSpecialFeatures.Name = "LabelSpecialFeatures";
            this.LabelSpecialFeatures.Size = new System.Drawing.Size(197, 33);
            this.LabelSpecialFeatures.TabIndex = 0;
            this.LabelSpecialFeatures.Text = "Special Features";
            // 
            // ButtonDoneFeatures
            // 
            this.ButtonDoneFeatures.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.CheckBox_ChopParotid.BackColor = System.Drawing.Color.DarkTurquoise;
            this.CheckBox_ChopParotid.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBox_ChopParotid.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.CheckBox_ChopParotid.Location = new System.Drawing.Point(199, 132);
            this.CheckBox_ChopParotid.Name = "CheckBox_ChopParotid";
            this.CheckBox_ChopParotid.Size = new System.Drawing.Size(302, 27);
            this.CheckBox_ChopParotid.TabIndex = 1;
            this.CheckBox_ChopParotid.Text = "Sub-parotid structure optimization";
            this.CheckBox_ChopParotid.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.textBox1.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.textBox1.Location = new System.Drawing.Point(111, 498);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(174, 22);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "PatientReport";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // BrowseButton
            // 
            this.BrowseButton.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.StartButton.BackColor = System.Drawing.Color.DarkTurquoise;
            this.StartButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.StartButton.Location = new System.Drawing.Point(579, 519);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 31);
            this.StartButton.TabIndex = 7;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DarkTurquoise;
            this.button2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Location = new System.Drawing.Point(655, 519);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 31);
            this.button2.TabIndex = 8;
            this.button2.Text = "Quit";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Snow;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkTurquoise;
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
            this.StartErrorLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.StartErrorLabel.Location = new System.Drawing.Point(365, 526);
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
            this.IterationsLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.IterationsLabel.Location = new System.Drawing.Point(538, 471);
            this.IterationsLabel.Name = "IterationsLabel";
            this.IterationsLabel.Size = new System.Drawing.Size(162, 19);
            this.IterationsLabel.TabIndex = 19;
            this.IterationsLabel.Text = "Optimization Iterations:";
            // 
            // IterationsTextBox
            // 
            this.IterationsTextBox.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IterationsTextBox.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.IterationsTextBox.Location = new System.Drawing.Point(706, 468);
            this.IterationsTextBox.Name = "IterationsTextBox";
            this.IterationsTextBox.Size = new System.Drawing.Size(16, 27);
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
            this.LabelSaveCheck.ForeColor = System.Drawing.Color.DarkTurquoise;
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
            this.LocationLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
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
            this.LatexPanel.Location = new System.Drawing.Point(0, 0);
            this.LatexPanel.Name = "LatexPanel";
            this.LatexPanel.Size = new System.Drawing.Size(734, 591);
            this.LatexPanel.TabIndex = 25;
            this.LatexPanel.Visible = false;
            // 
            // DoneLatexButton
            // 
            this.DoneLatexButton.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.DVHLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.DVHLabel.Location = new System.Drawing.Point(12, 25);
            this.DVHLabel.Name = "DVHLabel";
            this.DVHLabel.Size = new System.Drawing.Size(173, 39);
            this.DVHLabel.TabIndex = 11;
            this.DVHLabel.Text = "DVH Plotter";
            // 
            // PlotButton
            // 
            this.PlotButton.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.PlotCombobox.FormattingEnabled = true;
            this.PlotCombobox.Location = new System.Drawing.Point(129, 442);
            this.PlotCombobox.Name = "PlotCombobox";
            this.PlotCombobox.Size = new System.Drawing.Size(156, 21);
            this.PlotCombobox.TabIndex = 9;
            // 
            // PlotDoneButton
            // 
            this.PlotDoneButton.BackColor = System.Drawing.Color.DarkTurquoise;
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
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AssigningPanel);
            this.Controls.Add(this.PanelSpecialFeatures);
            this.Controls.Add(this.LocationLabel);
            this.Controls.Add(this.LabelSaveCheck);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.IterationsTextBox);
            this.Controls.Add(this.IterationsLabel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.StartErrorLabel);
            this.Controls.Add(this.SaveCheck);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.PlotPanel);
            this.Controls.Add(this.LatexPanel);
            this.Controls.Add(this.panel5);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Plan N Check";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.LatexPanel.ResumeLayout(false);
            this.PlotPanel.ResumeLayout(false);
            this.PlotPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox19;
        private System.Windows.Forms.CheckBox checkBox18;
        private System.Windows.Forms.CheckBox checkBox17;
        private System.Windows.Forms.CheckBox checkBox16;
        private System.Windows.Forms.CheckBox checkBox15;
        private System.Windows.Forms.CheckBox checkBox14;
        private System.Windows.Forms.CheckBox checkBox13;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
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
    }
}