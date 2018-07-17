namespace VelocityMap
{
    partial class Form1
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Field = new System.Windows.Forms.TabPage();
            this.mainField = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Data = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DistancePlot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.VelocityPlot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.controlPoints = new System.Windows.Forms.DataGridView();
            this.x = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Direction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.maxVelocity = new System.Windows.Forms.TextBox();
            this.timeSample = new System.Windows.Forms.TextBox();
            this.trackWidth = new System.Windows.Forms.TextBox();
            this.AccelRate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.ClearCP = new System.Windows.Forms.Button();
            this.CTRE = new System.Windows.Forms.CheckBox();
            this.wheel = new System.Windows.Forms.TextBox();
            this.tolerence = new System.Windows.Forms.TextBox();
            this.offset = new System.Windows.Forms.TextBox();
            this.cpLoad = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.isntaVel = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SpeedLimit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TurnCheck = new System.Windows.Forms.CheckBox();
            this.smoothness = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.degrees = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.Field.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainField)).BeginInit();
            this.Data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DistancePlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelocityPlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.controlPoints)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Field);
            this.tabControl1.Controls.Add(this.Data);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(577, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1029, 961);
            this.tabControl1.TabIndex = 1;
            // 
            // Field
            // 
            this.Field.Controls.Add(this.mainField);
            this.Field.Location = new System.Drawing.Point(4, 22);
            this.Field.Margin = new System.Windows.Forms.Padding(1);
            this.Field.Name = "Field";
            this.Field.Padding = new System.Windows.Forms.Padding(1);
            this.Field.Size = new System.Drawing.Size(1021, 935);
            this.Field.TabIndex = 0;
            this.Field.Text = "Field";
            this.Field.UseVisualStyleBackColor = true;
            // 
            // mainField
            // 
            chartArea1.Name = "ChartArea1";
            this.mainField.ChartAreas.Add(chartArea1);
            this.mainField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainField.Location = new System.Drawing.Point(1, 1);
            this.mainField.Name = "mainField";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.mainField.Series.Add(series1);
            this.mainField.Size = new System.Drawing.Size(1019, 933);
            this.mainField.TabIndex = 4;
            this.mainField.Text = "chart2";
            this.mainField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mainField_MouseClick);
            this.mainField.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainField_MouseDown);
            this.mainField.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainField_MouseMove);
            // 
            // Data
            // 
            this.Data.Controls.Add(this.splitContainer1);
            this.Data.Location = new System.Drawing.Point(4, 22);
            this.Data.Margin = new System.Windows.Forms.Padding(1);
            this.Data.Name = "Data";
            this.Data.Padding = new System.Windows.Forms.Padding(1);
            this.Data.Size = new System.Drawing.Size(1021, 935);
            this.Data.TabIndex = 1;
            this.Data.Text = "Data";
            this.Data.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(1, 1);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.DistancePlot);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.VelocityPlot);
            this.splitContainer1.Size = new System.Drawing.Size(1019, 933);
            this.splitContainer1.SplitterDistance = 456;
            this.splitContainer1.TabIndex = 3;
            // 
            // DistancePlot
            // 
            chartArea2.Name = "ChartArea1";
            this.DistancePlot.ChartAreas.Add(chartArea2);
            this.DistancePlot.Dock = System.Windows.Forms.DockStyle.Top;
            this.DistancePlot.Location = new System.Drawing.Point(0, 0);
            this.DistancePlot.Name = "DistancePlot";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.DistancePlot.Series.Add(series2);
            this.DistancePlot.Size = new System.Drawing.Size(1019, 235);
            this.DistancePlot.TabIndex = 2;
            this.DistancePlot.Text = "chart2";
            // 
            // VelocityPlot
            // 
            chartArea3.Name = "ChartArea1";
            this.VelocityPlot.ChartAreas.Add(chartArea3);
            this.VelocityPlot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.VelocityPlot.Location = new System.Drawing.Point(0, 275);
            this.VelocityPlot.Name = "VelocityPlot";
            this.VelocityPlot.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            series3.ChartArea = "ChartArea1";
            series3.Name = "Series1";
            this.VelocityPlot.Series.Add(series3);
            this.VelocityPlot.Size = new System.Drawing.Size(1019, 198);
            this.VelocityPlot.TabIndex = 1;
            this.VelocityPlot.Text = "chart2";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // controlPoints
            // 
            this.controlPoints.AllowUserToResizeColumns = false;
            this.controlPoints.AllowUserToResizeRows = false;
            this.controlPoints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.controlPoints.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.controlPoints.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.controlPoints.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.controlPoints.ColumnHeadersHeight = 20;
            this.controlPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.controlPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.x,
            this.y,
            this.Direction});
            this.controlPoints.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.controlPoints.Location = new System.Drawing.Point(19, 295);
            this.controlPoints.Margin = new System.Windows.Forms.Padding(1);
            this.controlPoints.MultiSelect = false;
            this.controlPoints.Name = "controlPoints";
            this.controlPoints.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.controlPoints.RowHeadersWidth = 20;
            this.controlPoints.RowTemplate.Height = 40;
            this.controlPoints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.controlPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.controlPoints.Size = new System.Drawing.Size(237, 666);
            this.controlPoints.TabIndex = 2;
            this.controlPoints.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.controlPoints_CellEndEdit);
            this.controlPoints.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.controlPoints_CellMouseUp);
            this.controlPoints.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.controlPoints_CellValidating);
            // 
            // x
            // 
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.x.DefaultCellStyle = dataGridViewCellStyle1;
            this.x.Frozen = true;
            this.x.HeaderText = "X";
            this.x.Name = "x";
            this.x.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.x.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.x.Width = 58;
            // 
            // y
            // 
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.y.DefaultCellStyle = dataGridViewCellStyle2;
            this.y.Frozen = true;
            this.y.HeaderText = "Y";
            this.y.Name = "y";
            this.y.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.y.Width = 58;
            // 
            // Direction
            // 
            this.Direction.Frozen = true;
            this.Direction.HeaderText = "Vel (+/-)";
            this.Direction.MaxInputLength = 1;
            this.Direction.Name = "Direction";
            this.Direction.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 243);
            this.button1.Margin = new System.Windows.Forms.Padding(1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 21);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Save_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 201);
            this.button2.Margin = new System.Windows.Forms.Padding(1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(231, 34);
            this.button2.TabIndex = 3;
            this.button2.Text = "Apply";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Apply_Click);
            // 
            // maxVelocity
            // 
            this.maxVelocity.Location = new System.Drawing.Point(179, 17);
            this.maxVelocity.Margin = new System.Windows.Forms.Padding(1);
            this.maxVelocity.Name = "maxVelocity";
            this.maxVelocity.Size = new System.Drawing.Size(71, 20);
            this.maxVelocity.TabIndex = 4;
            this.maxVelocity.Text = "2500";
            this.maxVelocity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timeSample
            // 
            this.timeSample.Location = new System.Drawing.Point(179, 35);
            this.timeSample.Margin = new System.Windows.Forms.Padding(1);
            this.timeSample.Name = "timeSample";
            this.timeSample.Size = new System.Drawing.Size(71, 20);
            this.timeSample.TabIndex = 4;
            this.timeSample.Text = "10";
            this.timeSample.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // trackWidth
            // 
            this.trackWidth.Location = new System.Drawing.Point(179, 54);
            this.trackWidth.Margin = new System.Windows.Forms.Padding(1);
            this.trackWidth.Name = "trackWidth";
            this.trackWidth.Size = new System.Drawing.Size(71, 20);
            this.trackWidth.TabIndex = 4;
            this.trackWidth.Text = "680";
            this.trackWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AccelRate
            // 
            this.AccelRate.Location = new System.Drawing.Point(179, 73);
            this.AccelRate.Margin = new System.Windows.Forms.Padding(1);
            this.AccelRate.Name = "AccelRate";
            this.AccelRate.Size = new System.Drawing.Size(71, 20);
            this.AccelRate.TabIndex = 4;
            this.AccelRate.Text = "80";
            this.AccelRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Max Velocity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Time (ms)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 54);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Track Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 73);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Accel Filter (FL1)";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(53, 312);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(1);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Limit Max Speed";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // ClearCP
            // 
            this.ClearCP.Location = new System.Drawing.Point(164, 266);
            this.ClearCP.Margin = new System.Windows.Forms.Padding(1);
            this.ClearCP.Name = "ClearCP";
            this.ClearCP.Size = new System.Drawing.Size(86, 21);
            this.ClearCP.TabIndex = 3;
            this.ClearCP.Text = "Clear";
            this.ClearCP.UseVisualStyleBackColor = true;
            this.ClearCP.Click += new System.EventHandler(this.ClearCP_Click);
            // 
            // CTRE
            // 
            this.CTRE.AutoSize = true;
            this.CTRE.Checked = true;
            this.CTRE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CTRE.Location = new System.Drawing.Point(21, 161);
            this.CTRE.Margin = new System.Windows.Forms.Padding(1);
            this.CTRE.Name = "CTRE";
            this.CTRE.Size = new System.Drawing.Size(88, 17);
            this.CTRE.TabIndex = 6;
            this.CTRE.Text = "CTRE output";
            this.CTRE.UseVisualStyleBackColor = true;
            this.CTRE.CheckedChanged += new System.EventHandler(this.CTRE_CheckedChanged);
            // 
            // wheel
            // 
            this.wheel.Location = new System.Drawing.Point(205, 159);
            this.wheel.Margin = new System.Windows.Forms.Padding(1);
            this.wheel.Name = "wheel";
            this.wheel.Size = new System.Drawing.Size(45, 20);
            this.wheel.TabIndex = 4;
            this.wheel.Text = "6";
            this.wheel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tolerence
            // 
            this.tolerence.Enabled = false;
            this.tolerence.Location = new System.Drawing.Point(138, 331);
            this.tolerence.Margin = new System.Windows.Forms.Padding(1);
            this.tolerence.Name = "tolerence";
            this.tolerence.Size = new System.Drawing.Size(71, 20);
            this.tolerence.TabIndex = 7;
            this.tolerence.Text = "660";
            this.tolerence.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tolerence.Visible = false;
            // 
            // offset
            // 
            this.offset.Enabled = false;
            this.offset.Location = new System.Drawing.Point(138, 350);
            this.offset.Margin = new System.Windows.Forms.Padding(1);
            this.offset.Name = "offset";
            this.offset.Size = new System.Drawing.Size(71, 20);
            this.offset.TabIndex = 8;
            this.offset.Text = "50";
            this.offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.offset.Visible = false;
            // 
            // cpLoad
            // 
            this.cpLoad.Location = new System.Drawing.Point(164, 243);
            this.cpLoad.Margin = new System.Windows.Forms.Padding(1);
            this.cpLoad.Name = "cpLoad";
            this.cpLoad.Size = new System.Drawing.Size(86, 21);
            this.cpLoad.TabIndex = 3;
            this.cpLoad.Text = "Load";
            this.cpLoad.UseVisualStyleBackColor = true;
            this.cpLoad.Click += new System.EventHandler(this.Load_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(18, 333);
            this.label5.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Velocity Offset";
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(18, 352);
            this.label6.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Vel Tolerence";
            this.label6.Visible = false;
            // 
            // isntaVel
            // 
            this.isntaVel.Location = new System.Drawing.Point(58, 372);
            this.isntaVel.Margin = new System.Windows.Forms.Padding(1);
            this.isntaVel.Name = "isntaVel";
            this.isntaVel.Size = new System.Drawing.Size(99, 21);
            this.isntaVel.TabIndex = 6;
            this.isntaVel.Text = "Inst Velocity";
            this.isntaVel.UseVisualStyleBackColor = true;
            this.isntaVel.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(21, 266);
            this.button3.Margin = new System.Windows.Forms.Padding(1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 21);
            this.button3.TabIndex = 3;
            this.button3.Text = "Invert";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Invert_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Delete,
            this.insertToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(141, 48);
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(140, 22);
            this.Delete.Text = "Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.insertToolStripMenuItem.Text = "Insert Above";
            this.insertToolStripMenuItem.Click += new System.EventHandler(this.insertToolStripMenuItem_Click);
            // 
            // SpeedLimit
            // 
            this.SpeedLimit.Location = new System.Drawing.Point(179, 92);
            this.SpeedLimit.Margin = new System.Windows.Forms.Padding(1);
            this.SpeedLimit.Name = "SpeedLimit";
            this.SpeedLimit.Size = new System.Drawing.Size(71, 20);
            this.SpeedLimit.TabIndex = 4;
            this.SpeedLimit.Text = "2.5";
            this.SpeedLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 92);
            this.label7.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Speed Limit Factor";
            // 
            // TurnCheck
            // 
            this.TurnCheck.AutoSize = true;
            this.TurnCheck.Location = new System.Drawing.Point(21, 138);
            this.TurnCheck.Margin = new System.Windows.Forms.Padding(1);
            this.TurnCheck.Name = "TurnCheck";
            this.TurnCheck.Size = new System.Drawing.Size(48, 17);
            this.TurnCheck.TabIndex = 6;
            this.TurnCheck.Text = "Turn";
            this.TurnCheck.UseVisualStyleBackColor = true;
            this.TurnCheck.CheckedChanged += new System.EventHandler(this.CalCheck_CheckedChanged);
            // 
            // smoothness
            // 
            this.smoothness.Location = new System.Drawing.Point(205, 115);
            this.smoothness.Margin = new System.Windows.Forms.Padding(1);
            this.smoothness.Name = "smoothness";
            this.smoothness.Size = new System.Drawing.Size(45, 20);
            this.smoothness.TabIndex = 8;
            this.smoothness.Text = "10";
            this.smoothness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.smoothness.TextChanged += new System.EventHandler(this.Rotations_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 118);
            this.label8.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Smoothing";
            // 
            // degrees
            // 
            this.degrees.Location = new System.Drawing.Point(205, 138);
            this.degrees.Margin = new System.Windows.Forms.Padding(1);
            this.degrees.Name = "degrees";
            this.degrees.Size = new System.Drawing.Size(45, 20);
            this.degrees.TabIndex = 8;
            this.degrees.Tag = "Degrees";
            this.degrees.Text = "90";
            this.degrees.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(136, 161);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(67, 13);
            this.Label9.TabIndex = 9;
            this.Label9.Text = "Wheel Size :";
            this.Label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(150, 143);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Degrees :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1606, 961);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.smoothness);
            this.Controls.Add(this.degrees);
            this.Controls.Add(this.offset);
            this.Controls.Add(this.tolerence);
            this.Controls.Add(this.isntaVel);
            this.Controls.Add(this.TurnCheck);
            this.Controls.Add(this.CTRE);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wheel);
            this.Controls.Add(this.SpeedLimit);
            this.Controls.Add(this.AccelRate);
            this.Controls.Add(this.trackWidth);
            this.Controls.Add(this.timeSample);
            this.Controls.Add(this.maxVelocity);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ClearCP);
            this.Controls.Add(this.cpLoad);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.controlPoints);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Byting Bulldogs (3539) - Motion Profiler Creator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tabControl1.ResumeLayout(false);
            this.Field.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainField)).EndInit();
            this.Data.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DistancePlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelocityPlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.controlPoints)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Field;
        private System.Windows.Forms.TabPage Data;
        private System.Windows.Forms.DataVisualization.Charting.Chart VelocityPlot;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView controlPoints;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataVisualization.Charting.Chart mainField;
        private System.Windows.Forms.TextBox maxVelocity;
        private System.Windows.Forms.TextBox timeSample;
        private System.Windows.Forms.TextBox trackWidth;
        private System.Windows.Forms.TextBox AccelRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button ClearCP;
        private System.Windows.Forms.CheckBox CTRE;
        private System.Windows.Forms.TextBox wheel;
        private System.Windows.Forms.DataVisualization.Charting.Chart DistancePlot;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tolerence;
        private System.Windows.Forms.TextBox offset;
        private System.Windows.Forms.Button cpLoad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox isntaVel;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.TextBox SpeedLimit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox TurnCheck;
        private System.Windows.Forms.TextBox smoothness;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox degrees;
        private System.Windows.Forms.DataGridViewTextBoxColumn x;
        private System.Windows.Forms.DataGridViewTextBoxColumn y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Direction;
        private System.Windows.Forms.Label Label9;
        private System.Windows.Forms.Label label10;
    }
}

