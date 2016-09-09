namespace BrainPackDataAnalyzer
{
    partial class mainForm
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.cb_serialPorts = new System.Windows.Forms.ComboBox();
            this.l_COM_Port = new System.Windows.Forms.Label();
            this.bnt_Connect = new System.Windows.Forms.Button();
            this.btn_SendCmd = new System.Windows.Forms.Button();
            this.btn_Analyze = new System.Windows.Forms.Button();
            this.ofd_AnalyzeFile = new System.Windows.Forms.OpenFileDialog();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tb_Console = new System.Windows.Forms.TextBox();
            this.sfd_ConvertedFile = new System.Windows.Forms.SaveFileDialog();
            this.btn_EncryptSettings = new System.Windows.Forms.Button();
            this.btn_CreateFwBin = new System.Windows.Forms.Button();
            this.btn_disconnect = new System.Windows.Forms.Button();
            this.tb_stretchData = new System.Windows.Forms.TextBox();
            this.lb_stretchdata = new System.Windows.Forms.Label();
            this.btn_record = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_getState = new System.Windows.Forms.Button();
            this.btn_clearStats = new System.Windows.Forms.Button();
            this.btn_setTime = new System.Windows.Forms.Button();
            this.cb_saveRecordEntries = new System.Windows.Forms.CheckBox();
            this.tb_saveLocation = new System.Windows.Forms.TextBox();
            this.btn_setSaveLocation = new System.Windows.Forms.Button();
            this.cb_serialPassT = new System.Windows.Forms.ComboBox();
            this.cb_serialPassEn = new System.Windows.Forms.CheckBox();
            this.serialPortPassThrough = new System.IO.Ports.SerialPort(this.components);
            this.bgw_AnalysisBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.btn_EnterBootloader = new System.Windows.Forms.Button();
            this.btn_exitBootloader = new System.Windows.Forms.Button();
            this.btn_Convert = new System.Windows.Forms.Button();
            this.btn_CheckVersion = new System.Windows.Forms.Button();
            this.cb_commands = new System.Windows.Forms.ComboBox();
            this.cb_Baud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_SelectedImu = new System.Windows.Forms.NumericUpDown();
            this.chrt_dataChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pb_processingProgress = new System.Windows.Forms.ProgressBar();
            this.dgv_SensorStats = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.cb_protobuf = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btn_disconnectSock = new System.Windows.Forms.Button();
            this.cb_udpListenner = new System.Windows.Forms.CheckBox();
            this.mtb_NetAddress = new System.Windows.Forms.MaskedTextBox();
            this.mtb_netPort = new System.Windows.Forms.MaskedTextBox();
            this.btn_connectSocket = new System.Windows.Forms.Button();
            this.mtb_udpPort = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SelectedImu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrt_dataChart)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SensorStats)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPort
            // 
            this.serialPort.BaudRate = 115200;
            this.serialPort.ReadTimeout = 500;
            this.serialPort.WriteTimeout = 500;
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // 
            // cb_serialPorts
            // 
            this.cb_serialPorts.FormattingEnabled = true;
            this.cb_serialPorts.Location = new System.Drawing.Point(115, 255);
            this.cb_serialPorts.Name = "cb_serialPorts";
            this.cb_serialPorts.Size = new System.Drawing.Size(161, 21);
            this.cb_serialPorts.TabIndex = 1;
            this.cb_serialPorts.SelectedIndexChanged += new System.EventHandler(this.cb_serialPorts_SelectedIndexChanged);
            this.cb_serialPorts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cb_serialPorts_MouseDoubleClick);
            // 
            // l_COM_Port
            // 
            this.l_COM_Port.AutoSize = true;
            this.l_COM_Port.Location = new System.Drawing.Point(56, 263);
            this.l_COM_Port.Name = "l_COM_Port";
            this.l_COM_Port.Size = new System.Drawing.Size(53, 13);
            this.l_COM_Port.TabIndex = 2;
            this.l_COM_Port.Text = "COM Port";
            // 
            // bnt_Connect
            // 
            this.bnt_Connect.Location = new System.Drawing.Point(299, 253);
            this.bnt_Connect.Name = "bnt_Connect";
            this.bnt_Connect.Size = new System.Drawing.Size(73, 23);
            this.bnt_Connect.TabIndex = 3;
            this.bnt_Connect.Text = "Connect";
            this.bnt_Connect.UseVisualStyleBackColor = true;
            this.bnt_Connect.Click += new System.EventHandler(this.bnt_Connect_Click);
            // 
            // btn_SendCmd
            // 
            this.btn_SendCmd.Location = new System.Drawing.Point(299, 310);
            this.btn_SendCmd.Name = "btn_SendCmd";
            this.btn_SendCmd.Size = new System.Drawing.Size(117, 23);
            this.btn_SendCmd.TabIndex = 4;
            this.btn_SendCmd.Text = "Send Command";
            this.btn_SendCmd.UseVisualStyleBackColor = true;
            this.btn_SendCmd.Click += new System.EventHandler(this.btn_SendCmd_Click);
            // 
            // btn_Analyze
            // 
            this.btn_Analyze.Location = new System.Drawing.Point(571, 29);
            this.btn_Analyze.Name = "btn_Analyze";
            this.btn_Analyze.Size = new System.Drawing.Size(117, 23);
            this.btn_Analyze.TabIndex = 6;
            this.btn_Analyze.Text = "Analyze File";
            this.btn_Analyze.UseVisualStyleBackColor = true;
            this.btn_Analyze.Click += new System.EventHandler(this.btn_Analyze_Click);
            // 
            // ofd_AnalyzeFile
            // 
            this.ofd_AnalyzeFile.DefaultExt = "csv";
            this.ofd_AnalyzeFile.FileName = "movementData.csv";
            // 
            // tb_Console
            // 
            this.tb_Console.Location = new System.Drawing.Point(65, 29);
            this.tb_Console.Multiline = true;
            this.tb_Console.Name = "tb_Console";
            this.tb_Console.ReadOnly = true;
            this.tb_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Console.Size = new System.Drawing.Size(455, 218);
            this.tb_Console.TabIndex = 7;
            // 
            // btn_EncryptSettings
            // 
            this.btn_EncryptSettings.Location = new System.Drawing.Point(571, 87);
            this.btn_EncryptSettings.Name = "btn_EncryptSettings";
            this.btn_EncryptSettings.Size = new System.Drawing.Size(117, 23);
            this.btn_EncryptSettings.TabIndex = 12;
            this.btn_EncryptSettings.Text = "Encrypt Settings";
            this.btn_EncryptSettings.UseVisualStyleBackColor = true;
            this.btn_EncryptSettings.Click += new System.EventHandler(this.btn_EncryptSettings_Click);
            // 
            // btn_CreateFwBin
            // 
            this.btn_CreateFwBin.Location = new System.Drawing.Point(571, 116);
            this.btn_CreateFwBin.Name = "btn_CreateFwBin";
            this.btn_CreateFwBin.Size = new System.Drawing.Size(117, 23);
            this.btn_CreateFwBin.TabIndex = 13;
            this.btn_CreateFwBin.Text = "Create Firmware Binary";
            this.btn_CreateFwBin.UseVisualStyleBackColor = true;
            this.btn_CreateFwBin.Click += new System.EventHandler(this.btn_CreateFwBin_Click);
            // 
            // btn_disconnect
            // 
            this.btn_disconnect.Location = new System.Drawing.Point(299, 282);
            this.btn_disconnect.Name = "btn_disconnect";
            this.btn_disconnect.Size = new System.Drawing.Size(73, 23);
            this.btn_disconnect.TabIndex = 15;
            this.btn_disconnect.Text = "Disconnect";
            this.btn_disconnect.UseVisualStyleBackColor = true;
            this.btn_disconnect.Click += new System.EventHandler(this.btn_disconnect_Click);
            // 
            // tb_stretchData
            // 
            this.tb_stretchData.Location = new System.Drawing.Point(717, 312);
            this.tb_stretchData.Name = "tb_stretchData";
            this.tb_stretchData.Size = new System.Drawing.Size(202, 20);
            this.tb_stretchData.TabIndex = 16;
            // 
            // lb_stretchdata
            // 
            this.lb_stretchdata.AutoSize = true;
            this.lb_stretchdata.Location = new System.Drawing.Point(644, 317);
            this.lb_stretchdata.Name = "lb_stretchdata";
            this.lb_stretchdata.Size = new System.Drawing.Size(67, 13);
            this.lb_stretchdata.TabIndex = 17;
            this.lb_stretchdata.Text = "Stretch Data";
            // 
            // btn_record
            // 
            this.btn_record.Location = new System.Drawing.Point(567, 209);
            this.btn_record.Name = "btn_record";
            this.btn_record.Size = new System.Drawing.Size(73, 23);
            this.btn_record.TabIndex = 19;
            this.btn_record.Text = "Record";
            this.btn_record.UseVisualStyleBackColor = true;
            this.btn_record.Click += new System.EventHandler(this.btn_record_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(664, 209);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(73, 23);
            this.btn_reset.TabIndex = 20;
            this.btn_reset.Text = "Reset";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_getState
            // 
            this.btn_getState.Location = new System.Drawing.Point(567, 169);
            this.btn_getState.Name = "btn_getState";
            this.btn_getState.Size = new System.Drawing.Size(73, 23);
            this.btn_getState.TabIndex = 21;
            this.btn_getState.Text = "Get State";
            this.btn_getState.UseVisualStyleBackColor = true;
            this.btn_getState.Click += new System.EventHandler(this.btn_getState_Click);
            // 
            // btn_clearStats
            // 
            this.btn_clearStats.Location = new System.Drawing.Point(447, 309);
            this.btn_clearStats.Name = "btn_clearStats";
            this.btn_clearStats.Size = new System.Drawing.Size(73, 23);
            this.btn_clearStats.TabIndex = 22;
            this.btn_clearStats.Text = "Clear Stats";
            this.btn_clearStats.UseVisualStyleBackColor = true;
            this.btn_clearStats.Click += new System.EventHandler(this.btn_clearStats_Click);
            // 
            // btn_setTime
            // 
            this.btn_setTime.Location = new System.Drawing.Point(664, 169);
            this.btn_setTime.Name = "btn_setTime";
            this.btn_setTime.Size = new System.Drawing.Size(73, 23);
            this.btn_setTime.TabIndex = 23;
            this.btn_setTime.Text = "Set Time";
            this.btn_setTime.UseVisualStyleBackColor = true;
            this.btn_setTime.Click += new System.EventHandler(this.btn_setTime_Click);
            // 
            // cb_saveRecordEntries
            // 
            this.cb_saveRecordEntries.AutoSize = true;
            this.cb_saveRecordEntries.Location = new System.Drawing.Point(567, 253);
            this.cb_saveRecordEntries.Name = "cb_saveRecordEntries";
            this.cb_saveRecordEntries.Size = new System.Drawing.Size(112, 17);
            this.cb_saveRecordEntries.TabIndex = 24;
            this.cb_saveRecordEntries.Text = "Save Data To File";
            this.cb_saveRecordEntries.UseVisualStyleBackColor = true;
            this.cb_saveRecordEntries.CheckedChanged += new System.EventHandler(this.cb_saveRecordEntries_CheckedChanged);
            // 
            // tb_saveLocation
            // 
            this.tb_saveLocation.Location = new System.Drawing.Point(567, 271);
            this.tb_saveLocation.Name = "tb_saveLocation";
            this.tb_saveLocation.Size = new System.Drawing.Size(232, 20);
            this.tb_saveLocation.TabIndex = 25;
            // 
            // btn_setSaveLocation
            // 
            this.btn_setSaveLocation.Location = new System.Drawing.Point(805, 271);
            this.btn_setSaveLocation.Name = "btn_setSaveLocation";
            this.btn_setSaveLocation.Size = new System.Drawing.Size(116, 23);
            this.btn_setSaveLocation.TabIndex = 26;
            this.btn_setSaveLocation.Text = "Set File Location";
            this.btn_setSaveLocation.UseVisualStyleBackColor = true;
            this.btn_setSaveLocation.Click += new System.EventHandler(this.btn_setSaveLocation_Click);
            // 
            // cb_serialPassT
            // 
            this.cb_serialPassT.FormattingEnabled = true;
            this.cb_serialPassT.Location = new System.Drawing.Point(761, 211);
            this.cb_serialPassT.Name = "cb_serialPassT";
            this.cb_serialPassT.Size = new System.Drawing.Size(115, 21);
            this.cb_serialPassT.TabIndex = 27;
            // 
            // cb_serialPassEn
            // 
            this.cb_serialPassEn.AutoSize = true;
            this.cb_serialPassEn.Location = new System.Drawing.Point(761, 188);
            this.cb_serialPassEn.Name = "cb_serialPassEn";
            this.cb_serialPassEn.Size = new System.Drawing.Size(128, 17);
            this.cb_serialPassEn.TabIndex = 28;
            this.cb_serialPassEn.Text = "Enable  Pass Though";
            this.cb_serialPassEn.UseVisualStyleBackColor = true;
            this.cb_serialPassEn.CheckedChanged += new System.EventHandler(this.cb_serialPassEn_CheckedChanged);
            // 
            // serialPortPassThrough
            // 
            this.serialPortPassThrough.BaudRate = 115200;
            this.serialPortPassThrough.ReadTimeout = 10;
            this.serialPortPassThrough.WriteTimeout = 5;
            this.serialPortPassThrough.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortPassThrough_DataReceived);
            // 
            // bgw_AnalysisBackgroundWorker
            // 
            this.bgw_AnalysisBackgroundWorker.WorkerReportsProgress = true;
            this.bgw_AnalysisBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_AnalysisBackgroundWorker_DoWork);
            this.bgw_AnalysisBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgw_AnalysisBackgroundWorker_ProgressChanged);
            this.bgw_AnalysisBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_AnalysisBackgroundWorker_RunWorkerCompleted);
            // 
            // btn_EnterBootloader
            // 
            this.btn_EnterBootloader.Location = new System.Drawing.Point(742, 29);
            this.btn_EnterBootloader.Name = "btn_EnterBootloader";
            this.btn_EnterBootloader.Size = new System.Drawing.Size(134, 23);
            this.btn_EnterBootloader.TabIndex = 31;
            this.btn_EnterBootloader.Text = "Enter Bootloader";
            this.btn_EnterBootloader.UseVisualStyleBackColor = true;
            this.btn_EnterBootloader.Click += new System.EventHandler(this.btn_EnterBootloader_Click);
            // 
            // btn_exitBootloader
            // 
            this.btn_exitBootloader.Location = new System.Drawing.Point(742, 58);
            this.btn_exitBootloader.Name = "btn_exitBootloader";
            this.btn_exitBootloader.Size = new System.Drawing.Size(134, 23);
            this.btn_exitBootloader.TabIndex = 32;
            this.btn_exitBootloader.Text = "Exit Bootloader";
            this.btn_exitBootloader.UseVisualStyleBackColor = true;
            this.btn_exitBootloader.Click += new System.EventHandler(this.btn_exitBootloader_Click);
            // 
            // btn_Convert
            // 
            this.btn_Convert.Location = new System.Drawing.Point(571, 58);
            this.btn_Convert.Name = "btn_Convert";
            this.btn_Convert.Size = new System.Drawing.Size(117, 23);
            this.btn_Convert.TabIndex = 33;
            this.btn_Convert.Text = "Convert File";
            this.btn_Convert.UseVisualStyleBackColor = true;
            this.btn_Convert.Click += new System.EventHandler(this.btn_Convert_Click);
            // 
            // btn_CheckVersion
            // 
            this.btn_CheckVersion.Location = new System.Drawing.Point(742, 87);
            this.btn_CheckVersion.Name = "btn_CheckVersion";
            this.btn_CheckVersion.Size = new System.Drawing.Size(134, 23);
            this.btn_CheckVersion.TabIndex = 34;
            this.btn_CheckVersion.Text = "Check Version";
            this.btn_CheckVersion.UseVisualStyleBackColor = true;
            this.btn_CheckVersion.Click += new System.EventHandler(this.btn_CheckVersion_Click);
            // 
            // cb_commands
            // 
            this.cb_commands.FormattingEnabled = true;
            this.cb_commands.Items.AddRange(new object[] {
            "?",
            "getCharge",
            "getRawCharge",
            "Power"});
            this.cb_commands.Location = new System.Drawing.Point(115, 311);
            this.cb_commands.Name = "cb_commands";
            this.cb_commands.Size = new System.Drawing.Size(161, 21);
            this.cb_commands.TabIndex = 35;
            // 
            // cb_Baud
            // 
            this.cb_Baud.FormattingEnabled = true;
            this.cb_Baud.Location = new System.Drawing.Point(115, 282);
            this.cb_Baud.Name = "cb_Baud";
            this.cb_Baud.Size = new System.Drawing.Size(161, 21);
            this.cb_Baud.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 282);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Baud";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.nud_SelectedImu);
            this.tabPage2.Controls.Add(this.chrt_dataChart);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1004, 406);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Graph";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Selected IMU";
            // 
            // nud_SelectedImu
            // 
            this.nud_SelectedImu.Location = new System.Drawing.Point(20, 80);
            this.nud_SelectedImu.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nud_SelectedImu.Name = "nud_SelectedImu";
            this.nud_SelectedImu.Size = new System.Drawing.Size(120, 20);
            this.nud_SelectedImu.TabIndex = 30;
            // 
            // chrt_dataChart
            // 
            chartArea1.AxisY.Maximum = 3.15D;
            chartArea1.AxisY.Minimum = -3.15D;
            chartArea1.Name = "ChartArea1";
            this.chrt_dataChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chrt_dataChart.Legends.Add(legend1);
            this.chrt_dataChart.Location = new System.Drawing.Point(152, 15);
            this.chrt_dataChart.Name = "chrt_dataChart";
            this.chrt_dataChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.BorderWidth = 5;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Roll";
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series2.BorderWidth = 5;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Pitch";
            series3.BorderWidth = 5;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Yaw";
            this.chrt_dataChart.Series.Add(series1);
            this.chrt_dataChart.Series.Add(series2);
            this.chrt_dataChart.Series.Add(series3);
            this.chrt_dataChart.Size = new System.Drawing.Size(713, 377);
            this.chrt_dataChart.TabIndex = 29;
            this.chrt_dataChart.Text = "chart1";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pb_processingProgress);
            this.tabPage1.Controls.Add(this.dgv_SensorStats);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1004, 406);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Table";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pb_processingProgress
            // 
            this.pb_processingProgress.Location = new System.Drawing.Point(20, 348);
            this.pb_processingProgress.Name = "pb_processingProgress";
            this.pb_processingProgress.Size = new System.Drawing.Size(965, 23);
            this.pb_processingProgress.Step = 1;
            this.pb_processingProgress.TabIndex = 15;
            this.pb_processingProgress.Visible = false;
            // 
            // dgv_SensorStats
            // 
            this.dgv_SensorStats.AllowUserToAddRows = false;
            this.dgv_SensorStats.AllowUserToDeleteRows = false;
            this.dgv_SensorStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SensorStats.Location = new System.Drawing.Point(20, 22);
            this.dgv_SensorStats.Name = "dgv_SensorStats";
            this.dgv_SensorStats.ReadOnly = true;
            this.dgv_SensorStats.Size = new System.Drawing.Size(965, 302);
            this.dgv_SensorStats.TabIndex = 14;
            this.dgv_SensorStats.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_SensorStats_DataError);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(26, 412);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1012, 432);
            this.tabControl1.TabIndex = 29;
            // 
            // cb_protobuf
            // 
            this.cb_protobuf.AutoSize = true;
            this.cb_protobuf.Location = new System.Drawing.Point(388, 259);
            this.cb_protobuf.Name = "cb_protobuf";
            this.cb_protobuf.Size = new System.Drawing.Size(132, 17);
            this.cb_protobuf.TabIndex = 38;
            this.cb_protobuf.Text = "Parse Protobuf Stream";
            this.cb_protobuf.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.checkBox1.Location = new System.Drawing.Point(689, 35);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(47, 16);
            this.checkBox1.TabIndex = 39;
            this.checkBox1.Text = "batch";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btn_disconnectSock
            // 
            this.btn_disconnectSock.Location = new System.Drawing.Point(299, 377);
            this.btn_disconnectSock.Name = "btn_disconnectSock";
            this.btn_disconnectSock.Size = new System.Drawing.Size(105, 23);
            this.btn_disconnectSock.TabIndex = 47;
            this.btn_disconnectSock.Text = "Disconnect Sock";
            this.btn_disconnectSock.UseVisualStyleBackColor = true;
            // 
            // cb_udpListenner
            // 
            this.cb_udpListenner.AutoSize = true;
            this.cb_udpListenner.Location = new System.Drawing.Point(591, 356);
            this.cb_udpListenner.Name = "cb_udpListenner";
            this.cb_udpListenner.Size = new System.Drawing.Size(125, 17);
            this.cb_udpListenner.TabIndex = 48;
            this.cb_udpListenner.Text = "Enable UDP Listener";
            this.cb_udpListenner.UseVisualStyleBackColor = true;
            this.cb_udpListenner.CheckedChanged += new System.EventHandler(this.cb_udpListenner_CheckedChanged);
            // 
            // mtb_NetAddress
            // 
            this.mtb_NetAddress.Location = new System.Drawing.Point(182, 352);
            this.mtb_NetAddress.Name = "mtb_NetAddress";
            this.mtb_NetAddress.Size = new System.Drawing.Size(100, 20);
            this.mtb_NetAddress.TabIndex = 44;
            this.mtb_NetAddress.Text = "192.168.2.1";
            // 
            // mtb_netPort
            // 
            this.mtb_netPort.Location = new System.Drawing.Point(182, 381);
            this.mtb_netPort.Name = "mtb_netPort";
            this.mtb_netPort.Size = new System.Drawing.Size(100, 20);
            this.mtb_netPort.TabIndex = 45;
            this.mtb_netPort.Text = "6666";
            // 
            // btn_connectSocket
            // 
            this.btn_connectSocket.Location = new System.Drawing.Point(299, 350);
            this.btn_connectSocket.Name = "btn_connectSocket";
            this.btn_connectSocket.Size = new System.Drawing.Size(105, 23);
            this.btn_connectSocket.TabIndex = 46;
            this.btn_connectSocket.Text = "Connect Socket";
            this.btn_connectSocket.UseVisualStyleBackColor = true;
            // 
            // mtb_udpPort
            // 
            this.mtb_udpPort.Location = new System.Drawing.Point(485, 356);
            this.mtb_udpPort.Name = "mtb_udpPort";
            this.mtb_udpPort.Size = new System.Drawing.Size(100, 20);
            this.mtb_udpPort.TabIndex = 49;
            this.mtb_udpPort.Text = "6666";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 856);
            this.Controls.Add(this.mtb_udpPort);
            this.Controls.Add(this.btn_disconnectSock);
            this.Controls.Add(this.cb_udpListenner);
            this.Controls.Add(this.mtb_NetAddress);
            this.Controls.Add(this.mtb_netPort);
            this.Controls.Add(this.btn_connectSocket);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cb_protobuf);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_Baud);
            this.Controls.Add(this.cb_commands);
            this.Controls.Add(this.btn_CheckVersion);
            this.Controls.Add(this.btn_Convert);
            this.Controls.Add(this.btn_exitBootloader);
            this.Controls.Add(this.btn_EnterBootloader);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cb_serialPassEn);
            this.Controls.Add(this.cb_serialPassT);
            this.Controls.Add(this.btn_setSaveLocation);
            this.Controls.Add(this.tb_saveLocation);
            this.Controls.Add(this.cb_saveRecordEntries);
            this.Controls.Add(this.btn_setTime);
            this.Controls.Add(this.btn_clearStats);
            this.Controls.Add(this.btn_getState);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.btn_record);
            this.Controls.Add(this.lb_stretchdata);
            this.Controls.Add(this.tb_stretchData);
            this.Controls.Add(this.btn_disconnect);
            this.Controls.Add(this.btn_CreateFwBin);
            this.Controls.Add(this.btn_EncryptSettings);
            this.Controls.Add(this.tb_Console);
            this.Controls.Add(this.btn_Analyze);
            this.Controls.Add(this.btn_SendCmd);
            this.Controls.Add(this.bnt_Connect);
            this.Controls.Add(this.l_COM_Port);
            this.Controls.Add(this.cb_serialPorts);
            this.Name = "mainForm";
            this.Text = "Brain Data Analyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.DoubleClick += new System.EventHandler(this.mainForm_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SelectedImu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrt_dataChart)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SensorStats)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.ComboBox cb_serialPorts;
        private System.Windows.Forms.Label l_COM_Port;
        private System.Windows.Forms.Button bnt_Connect;
        private System.Windows.Forms.Button btn_SendCmd;
        private System.Windows.Forms.Button btn_Analyze;
        private System.Windows.Forms.OpenFileDialog ofd_AnalyzeFile;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TextBox tb_Console;
        private System.Windows.Forms.SaveFileDialog sfd_ConvertedFile;
        private System.Windows.Forms.Button btn_EncryptSettings;
        private System.Windows.Forms.Button btn_CreateFwBin;
        private System.Windows.Forms.Button btn_disconnect;
        private System.Windows.Forms.TextBox tb_stretchData;
        private System.Windows.Forms.Label lb_stretchdata;
        private System.Windows.Forms.Button btn_record;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Button btn_getState;
        private System.Windows.Forms.Button btn_clearStats;
        private System.Windows.Forms.Button btn_setTime;
        private System.Windows.Forms.CheckBox cb_saveRecordEntries;
        private System.Windows.Forms.TextBox tb_saveLocation;
        private System.Windows.Forms.Button btn_setSaveLocation;
        private System.Windows.Forms.ComboBox cb_serialPassT;
        private System.Windows.Forms.CheckBox cb_serialPassEn;
        private System.IO.Ports.SerialPort serialPortPassThrough;
        private System.ComponentModel.BackgroundWorker bgw_AnalysisBackgroundWorker;
        private System.Windows.Forms.Button btn_EnterBootloader;
        private System.Windows.Forms.Button btn_exitBootloader;
        private System.Windows.Forms.Button btn_Convert;
        private System.Windows.Forms.Button btn_CheckVersion;
        private System.Windows.Forms.ComboBox cb_commands;
        private System.Windows.Forms.ComboBox cb_Baud;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_SelectedImu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrt_dataChart;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ProgressBar pb_processingProgress;
        private System.Windows.Forms.DataGridView dgv_SensorStats;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox cb_protobuf;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btn_disconnectSock;
        private System.Windows.Forms.CheckBox cb_udpListenner;
        private System.Windows.Forms.MaskedTextBox mtb_NetAddress;
        private System.Windows.Forms.MaskedTextBox mtb_netPort;
        private System.Windows.Forms.Button btn_connectSocket;
        private System.Windows.Forms.MaskedTextBox mtb_udpPort;
    }
}

