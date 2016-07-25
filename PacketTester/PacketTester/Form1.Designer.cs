using System;
using System.ComponentModel;

namespace PacketTester
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
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btn_disconnect = new System.Windows.Forms.Button();
            this.tb_Console = new System.Windows.Forms.TextBox();
            this.bnt_Connect = new System.Windows.Forms.Button();
            this.cb_serialPorts = new System.Windows.Forms.ComboBox();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.cb_BaudRate = new System.Windows.Forms.ComboBox();
            this.chrt_dataChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tb_y_max = new System.Windows.Forms.TextBox();
            this.tb_y_min = new System.Windows.Forms.TextBox();
            this.lb_y_max = new System.Windows.Forms.Label();
            this.lb_y_min = new System.Windows.Forms.Label();
            this.btn_setAxis = new System.Windows.Forms.Button();
            this.btn_clearScreen = new System.Windows.Forms.Button();
            this.forwardSerialPort = new System.IO.Ports.SerialPort(this.components);
            this.sfd_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btn_disconnectSock = new System.Windows.Forms.Button();
            this.cb_udpSelected = new System.Windows.Forms.CheckBox();
            this.mtb_NetAddress = new System.Windows.Forms.MaskedTextBox();
            this.mtb_netPort = new System.Windows.Forms.MaskedTextBox();
            this.btn_connectSocket = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tb_dataLogLocation = new System.Windows.Forms.TextBox();
            this.cb_saveSensorData = new System.Windows.Forms.CheckBox();
            this.lb_updateRate = new System.Windows.Forms.Label();
            this.nud_updateRate = new System.Windows.Forms.NumericUpDown();
            this.cb_ypr = new System.Windows.Forms.CheckBox();
            this.lb_rates = new System.Windows.Forms.Label();
            this.btn_setRate = new System.Windows.Forms.Button();
            this.nud_gyroRate = new System.Windows.Forms.NumericUpDown();
            this.nud_accelRate = new System.Windows.Forms.NumericUpDown();
            this.nud_magRate = new System.Windows.Forms.NumericUpDown();
            this.cb_dataType = new System.Windows.Forms.ComboBox();
            this.btn_getStatus = new System.Windows.Forms.Button();
            this.btn_getFrame = new System.Windows.Forms.Button();
            this.btn_SetupMode = new System.Windows.Forms.Button();
            this.cb_SetupModeEn = new System.Windows.Forms.CheckBox();
            this.btn_sendUpdateCmd = new System.Windows.Forms.Button();
            this.cb_enableStream = new System.Windows.Forms.CheckBox();
            this.nud_SelectedImu = new System.Windows.Forms.NumericUpDown();
            this.cb_logErrors = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cb_fpBaudRate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_forwardPort = new System.Windows.Forms.Label();
            this.cb_OutputFormat = new System.Windows.Forms.ComboBox();
            this.cb_EnableFowardPort = new System.Windows.Forms.CheckBox();
            this.btn_StopStreaming = new System.Windows.Forms.Button();
            this.cb_forwardPorts = new System.Windows.Forms.ComboBox();
            this.btn_startStream = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btb_arm_playRecording = new System.Windows.Forms.Button();
            this.btn_arm_recMovement = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chb_arm_easeMovement = new System.Windows.Forms.CheckBox();
            this.btn_arm_startStreaming = new System.Windows.Forms.Button();
            this.btn_arm_stopStreaming = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btn_arm_togglePort = new System.Windows.Forms.Button();
            this.chb_arm_EnableTracing = new System.Windows.Forms.CheckBox();
            this.lb_loopsExecuted = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.countDelaySel = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.btn_arm_browseFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cb_robotPort = new System.Windows.Forms.ComboBox();
            this.btn_refreshComPorts = new System.Windows.Forms.Button();
            this.robotArmPort = new System.IO.Ports.SerialPort(this.components);
            this.bgw_uarmFileWorker = new System.ComponentModel.BackgroundWorker();
            this.ofd_openUarmFile = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lbl_data1 = new System.Windows.Forms.Label();
            this.lbl_data2 = new System.Windows.Forms.Label();
            this.lbl_data3 = new System.Windows.Forms.Label();
            this.lbl_data4 = new System.Windows.Forms.Label();
            this.lbl_valueData1 = new System.Windows.Forms.Label();
            this.lbl_valueData2 = new System.Windows.Forms.Label();
            this.lbl_valueData3 = new System.Windows.Forms.Label();
            this.lbl_valueData4 = new System.Windows.Forms.Label();
            this.fbd_folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.chrt_dataChart)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_updateRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_gyroRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_accelRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_magRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SelectedImu)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_disconnect
            // 
            this.btn_disconnect.Location = new System.Drawing.Point(222, 319);
            this.btn_disconnect.Name = "btn_disconnect";
            this.btn_disconnect.Size = new System.Drawing.Size(73, 23);
            this.btn_disconnect.TabIndex = 21;
            this.btn_disconnect.Text = "Disconnect";
            this.btn_disconnect.UseVisualStyleBackColor = true;
            this.btn_disconnect.Click += new System.EventHandler(this.btn_disconnect_Click);
            // 
            // tb_Console
            // 
            this.tb_Console.Location = new System.Drawing.Point(40, 30);
            this.tb_Console.Multiline = true;
            this.tb_Console.Name = "tb_Console";
            this.tb_Console.ReadOnly = true;
            this.tb_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Console.Size = new System.Drawing.Size(455, 218);
            this.tb_Console.TabIndex = 20;
            // 
            // bnt_Connect
            // 
            this.bnt_Connect.Location = new System.Drawing.Point(222, 290);
            this.bnt_Connect.Name = "bnt_Connect";
            this.bnt_Connect.Size = new System.Drawing.Size(73, 23);
            this.bnt_Connect.TabIndex = 17;
            this.bnt_Connect.Text = "Connect";
            this.bnt_Connect.UseVisualStyleBackColor = true;
            this.bnt_Connect.Click += new System.EventHandler(this.bnt_Connect_Click);
            // 
            // cb_serialPorts
            // 
            this.cb_serialPorts.FormattingEnabled = true;
            this.cb_serialPorts.Location = new System.Drawing.Point(37, 290);
            this.cb_serialPorts.Name = "cb_serialPorts";
            this.cb_serialPorts.Size = new System.Drawing.Size(161, 21);
            this.cb_serialPorts.TabIndex = 16;
            this.cb_serialPorts.SelectedIndexChanged += new System.EventHandler(this.cb_serialPorts_SelectedIndexChanged);
            // 
            // serialPort
            // 
            this.serialPort.BaudRate = 115200;
            this.serialPort.ReadTimeout = 500;
            this.serialPort.WriteTimeout = 500;
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // 
            // cb_BaudRate
            // 
            this.cb_BaudRate.FormattingEnabled = true;
            this.cb_BaudRate.Location = new System.Drawing.Point(313, 290);
            this.cb_BaudRate.Name = "cb_BaudRate";
            this.cb_BaudRate.Size = new System.Drawing.Size(156, 21);
            this.cb_BaudRate.TabIndex = 22;
            // 
            // chrt_dataChart
            // 
            chartArea1.AxisY.Maximum = 1.1D;
            chartArea1.AxisY.Minimum = -1.1D;
            chartArea1.Name = "ChartArea1";
            this.chrt_dataChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chrt_dataChart.Legends.Add(legend1);
            this.chrt_dataChart.Location = new System.Drawing.Point(541, 30);
            this.chrt_dataChart.Name = "chrt_dataChart";
            this.chrt_dataChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.BorderWidth = 5;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.MarkerSize = 1;
            series1.Name = "Qx";
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series2.BorderWidth = 5;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.MarkerSize = 1;
            series2.Name = "Qy";
            series3.BorderWidth = 5;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.MarkerSize = 1;
            series3.Name = "Qz";
            series4.BorderWidth = 5;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.MarkerSize = 1;
            series4.Name = "Qw";
            this.chrt_dataChart.Series.Add(series1);
            this.chrt_dataChart.Series.Add(series2);
            this.chrt_dataChart.Series.Add(series3);
            this.chrt_dataChart.Series.Add(series4);
            this.chrt_dataChart.Size = new System.Drawing.Size(589, 377);
            this.chrt_dataChart.TabIndex = 28;
            this.chrt_dataChart.Text = "chart1";
            this.chrt_dataChart.Click += new System.EventHandler(this.chrt_dataChart_Click);
            // 
            // tb_y_max
            // 
            this.tb_y_max.Location = new System.Drawing.Point(575, 435);
            this.tb_y_max.Name = "tb_y_max";
            this.tb_y_max.Size = new System.Drawing.Size(100, 20);
            this.tb_y_max.TabIndex = 29;
            this.tb_y_max.Text = "1.1";
            this.tb_y_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_y_max.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_y_max_KeyPress);
            // 
            // tb_y_min
            // 
            this.tb_y_min.Location = new System.Drawing.Point(575, 465);
            this.tb_y_min.Name = "tb_y_min";
            this.tb_y_min.Size = new System.Drawing.Size(100, 20);
            this.tb_y_min.TabIndex = 30;
            this.tb_y_min.Text = "-1.1";
            this.tb_y_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_y_min.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_y_min_KeyPress);
            // 
            // lb_y_max
            // 
            this.lb_y_max.AutoSize = true;
            this.lb_y_max.Location = new System.Drawing.Point(534, 442);
            this.lb_y_max.Name = "lb_y_max";
            this.lb_y_max.Size = new System.Drawing.Size(37, 13);
            this.lb_y_max.TabIndex = 31;
            this.lb_y_max.Text = "Y-Max";
            // 
            // lb_y_min
            // 
            this.lb_y_min.AutoSize = true;
            this.lb_y_min.Location = new System.Drawing.Point(534, 472);
            this.lb_y_min.Name = "lb_y_min";
            this.lb_y_min.Size = new System.Drawing.Size(34, 13);
            this.lb_y_min.TabIndex = 32;
            this.lb_y_min.Text = "Y-Min";
            // 
            // btn_setAxis
            // 
            this.btn_setAxis.Location = new System.Drawing.Point(575, 501);
            this.btn_setAxis.Name = "btn_setAxis";
            this.btn_setAxis.Size = new System.Drawing.Size(100, 23);
            this.btn_setAxis.TabIndex = 33;
            this.btn_setAxis.Text = "Set Y Axis";
            this.btn_setAxis.UseVisualStyleBackColor = true;
            this.btn_setAxis.Click += new System.EventHandler(this.btn_setAxis_Click);
            // 
            // btn_clearScreen
            // 
            this.btn_clearScreen.Location = new System.Drawing.Point(37, 254);
            this.btn_clearScreen.Name = "btn_clearScreen";
            this.btn_clearScreen.Size = new System.Drawing.Size(95, 23);
            this.btn_clearScreen.TabIndex = 35;
            this.btn_clearScreen.Text = "Clear Screen";
            this.btn_clearScreen.UseVisualStyleBackColor = true;
            this.btn_clearScreen.Click += new System.EventHandler(this.btn_clearScreen_Click);
            // 
            // forwardSerialPort
            // 
            this.forwardSerialPort.BaudRate = 115200;
            // 
            // sfd_saveFileDialog
            // 
            this.sfd_saveFileDialog.Filter = "Comma Separated Files (*.csv)|*.csv|All Files (*.*)|*.*";
            this.sfd_saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.sfd_saveFileDialog_FileOk);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btn_disconnectSock);
            this.tabPage3.Controls.Add(this.cb_udpSelected);
            this.tabPage3.Controls.Add(this.mtb_NetAddress);
            this.tabPage3.Controls.Add(this.mtb_netPort);
            this.tabPage3.Controls.Add(this.btn_connectSocket);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(468, 229);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "TCP IP Debug";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btn_disconnectSock
            // 
            this.btn_disconnectSock.Location = new System.Drawing.Point(130, 43);
            this.btn_disconnectSock.Name = "btn_disconnectSock";
            this.btn_disconnectSock.Size = new System.Drawing.Size(105, 23);
            this.btn_disconnectSock.TabIndex = 42;
            this.btn_disconnectSock.Text = "Disconnect Sock";
            this.btn_disconnectSock.UseVisualStyleBackColor = true;
            this.btn_disconnectSock.Click += new System.EventHandler(this.btn_disconnectSock_Click);
            // 
            // cb_udpSelected
            // 
            this.cb_udpSelected.AutoSize = true;
            this.cb_udpSelected.Location = new System.Drawing.Point(256, 21);
            this.cb_udpSelected.Name = "cb_udpSelected";
            this.cb_udpSelected.Size = new System.Drawing.Size(49, 17);
            this.cb_udpSelected.TabIndex = 43;
            this.cb_udpSelected.Text = "UDP";
            this.cb_udpSelected.UseVisualStyleBackColor = true;
            // 
            // mtb_NetAddress
            // 
            this.mtb_NetAddress.Location = new System.Drawing.Point(13, 18);
            this.mtb_NetAddress.Name = "mtb_NetAddress";
            this.mtb_NetAddress.Size = new System.Drawing.Size(100, 20);
            this.mtb_NetAddress.TabIndex = 39;
            this.mtb_NetAddress.Text = "192.168.2.1";
            // 
            // mtb_netPort
            // 
            this.mtb_netPort.Location = new System.Drawing.Point(13, 47);
            this.mtb_netPort.Name = "mtb_netPort";
            this.mtb_netPort.Size = new System.Drawing.Size(100, 20);
            this.mtb_netPort.TabIndex = 40;
            this.mtb_netPort.Text = "6666";
            // 
            // btn_connectSocket
            // 
            this.btn_connectSocket.Location = new System.Drawing.Point(130, 16);
            this.btn_connectSocket.Name = "btn_connectSocket";
            this.btn_connectSocket.Size = new System.Drawing.Size(105, 23);
            this.btn_connectSocket.TabIndex = 41;
            this.btn_connectSocket.Text = "Connect Socket";
            this.btn_connectSocket.UseVisualStyleBackColor = true;
            this.btn_connectSocket.Click += new System.EventHandler(this.btn_connectSocket_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tb_dataLogLocation);
            this.tabPage2.Controls.Add(this.cb_saveSensorData);
            this.tabPage2.Controls.Add(this.lb_updateRate);
            this.tabPage2.Controls.Add(this.nud_updateRate);
            this.tabPage2.Controls.Add(this.cb_ypr);
            this.tabPage2.Controls.Add(this.lb_rates);
            this.tabPage2.Controls.Add(this.btn_setRate);
            this.tabPage2.Controls.Add(this.nud_gyroRate);
            this.tabPage2.Controls.Add(this.nud_accelRate);
            this.tabPage2.Controls.Add(this.nud_magRate);
            this.tabPage2.Controls.Add(this.cb_dataType);
            this.tabPage2.Controls.Add(this.btn_getStatus);
            this.tabPage2.Controls.Add(this.btn_getFrame);
            this.tabPage2.Controls.Add(this.btn_SetupMode);
            this.tabPage2.Controls.Add(this.cb_SetupModeEn);
            this.tabPage2.Controls.Add(this.btn_sendUpdateCmd);
            this.tabPage2.Controls.Add(this.cb_enableStream);
            this.tabPage2.Controls.Add(this.nud_SelectedImu);
            this.tabPage2.Controls.Add(this.cb_logErrors);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(468, 229);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Debug 485 Sensors";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tb_dataLogLocation
            // 
            this.tb_dataLogLocation.Location = new System.Drawing.Point(152, 175);
            this.tb_dataLogLocation.Name = "tb_dataLogLocation";
            this.tb_dataLogLocation.Size = new System.Drawing.Size(301, 20);
            this.tb_dataLogLocation.TabIndex = 49;
            // 
            // cb_saveSensorData
            // 
            this.cb_saveSensorData.AutoSize = true;
            this.cb_saveSensorData.Location = new System.Drawing.Point(36, 178);
            this.cb_saveSensorData.Name = "cb_saveSensorData";
            this.cb_saveSensorData.Size = new System.Drawing.Size(108, 17);
            this.cb_saveSensorData.TabIndex = 48;
            this.cb_saveSensorData.Text = "Save Data to File";
            this.cb_saveSensorData.UseVisualStyleBackColor = true;
            this.cb_saveSensorData.CheckedChanged += new System.EventHandler(this.cb_saveSensorData_CheckedChanged);
            // 
            // lb_updateRate
            // 
            this.lb_updateRate.AutoSize = true;
            this.lb_updateRate.Location = new System.Drawing.Point(329, 18);
            this.lb_updateRate.Name = "lb_updateRate";
            this.lb_updateRate.Size = new System.Drawing.Size(87, 13);
            this.lb_updateRate.TabIndex = 47;
            this.lb_updateRate.Text = "Update Rate(ms)";
            // 
            // nud_updateRate
            // 
            this.nud_updateRate.Location = new System.Drawing.Point(332, 40);
            this.nud_updateRate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_updateRate.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nud_updateRate.Name = "nud_updateRate";
            this.nud_updateRate.Size = new System.Drawing.Size(56, 20);
            this.nud_updateRate.TabIndex = 46;
            this.nud_updateRate.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // cb_ypr
            // 
            this.cb_ypr.AutoSize = true;
            this.cb_ypr.Location = new System.Drawing.Point(174, 18);
            this.cb_ypr.Name = "cb_ypr";
            this.cb_ypr.Size = new System.Drawing.Size(137, 17);
            this.cb_ypr.TabIndex = 45;
            this.cb_ypr.Text = "Yaw-Pitch-Roll Enabled";
            this.cb_ypr.UseVisualStyleBackColor = true;
            this.cb_ypr.CheckedChanged += new System.EventHandler(this.cb_ypr_CheckedChanged);
            // 
            // lb_rates
            // 
            this.lb_rates.AutoSize = true;
            this.lb_rates.Location = new System.Drawing.Point(272, 78);
            this.lb_rates.Name = "lb_rates";
            this.lb_rates.Size = new System.Drawing.Size(176, 13);
            this.lb_rates.TabIndex = 44;
            this.lb_rates.Text = "Mag Rate    Accel Rate   Gyro Rate";
            // 
            // btn_setRate
            // 
            this.btn_setRate.Location = new System.Drawing.Point(274, 127);
            this.btn_setRate.Name = "btn_setRate";
            this.btn_setRate.Size = new System.Drawing.Size(102, 23);
            this.btn_setRate.TabIndex = 43;
            this.btn_setRate.Text = "Set Rates";
            this.btn_setRate.UseVisualStyleBackColor = true;
            this.btn_setRate.Click += new System.EventHandler(this.btn_setRate_Click);
            // 
            // nud_gyroRate
            // 
            this.nud_gyroRate.Location = new System.Drawing.Point(399, 96);
            this.nud_gyroRate.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_gyroRate.Name = "nud_gyroRate";
            this.nud_gyroRate.Size = new System.Drawing.Size(39, 20);
            this.nud_gyroRate.TabIndex = 42;
            this.nud_gyroRate.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // nud_accelRate
            // 
            this.nud_accelRate.Location = new System.Drawing.Point(337, 96);
            this.nud_accelRate.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_accelRate.Name = "nud_accelRate";
            this.nud_accelRate.Size = new System.Drawing.Size(39, 20);
            this.nud_accelRate.TabIndex = 41;
            this.nud_accelRate.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nud_magRate
            // 
            this.nud_magRate.Location = new System.Drawing.Point(274, 96);
            this.nud_magRate.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nud_magRate.Name = "nud_magRate";
            this.nud_magRate.Size = new System.Drawing.Size(39, 20);
            this.nud_magRate.TabIndex = 40;
            this.nud_magRate.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // cb_dataType
            // 
            this.cb_dataType.FormattingEnabled = true;
            this.cb_dataType.Location = new System.Drawing.Point(36, 43);
            this.cb_dataType.Name = "cb_dataType";
            this.cb_dataType.Size = new System.Drawing.Size(121, 21);
            this.cb_dataType.TabIndex = 39;
            this.cb_dataType.SelectedIndexChanged += new System.EventHandler(this.cb_dataType_SelectedIndexChanged);
            // 
            // btn_getStatus
            // 
            this.btn_getStatus.Location = new System.Drawing.Point(126, 127);
            this.btn_getStatus.Name = "btn_getStatus";
            this.btn_getStatus.Size = new System.Drawing.Size(75, 23);
            this.btn_getStatus.TabIndex = 34;
            this.btn_getStatus.Text = "Get Status";
            this.btn_getStatus.UseVisualStyleBackColor = true;
            this.btn_getStatus.Click += new System.EventHandler(this.btn_getStatus_Click);
            // 
            // btn_getFrame
            // 
            this.btn_getFrame.Location = new System.Drawing.Point(36, 68);
            this.btn_getFrame.Name = "btn_getFrame";
            this.btn_getFrame.Size = new System.Drawing.Size(75, 23);
            this.btn_getFrame.TabIndex = 23;
            this.btn_getFrame.Text = "Get Frame";
            this.btn_getFrame.UseVisualStyleBackColor = true;
            this.btn_getFrame.Click += new System.EventHandler(this.btn_getFrame_Click);
            // 
            // btn_SetupMode
            // 
            this.btn_SetupMode.Location = new System.Drawing.Point(36, 97);
            this.btn_SetupMode.Name = "btn_SetupMode";
            this.btn_SetupMode.Size = new System.Drawing.Size(103, 23);
            this.btn_SetupMode.TabIndex = 24;
            this.btn_SetupMode.Text = "Send Setup Mode";
            this.btn_SetupMode.UseVisualStyleBackColor = true;
            this.btn_SetupMode.Click += new System.EventHandler(this.btn_SetupMode_Click);
            // 
            // cb_SetupModeEn
            // 
            this.cb_SetupModeEn.AutoSize = true;
            this.cb_SetupModeEn.Location = new System.Drawing.Point(152, 101);
            this.cb_SetupModeEn.Name = "cb_SetupModeEn";
            this.cb_SetupModeEn.Size = new System.Drawing.Size(65, 17);
            this.cb_SetupModeEn.TabIndex = 25;
            this.cb_SetupModeEn.Text = "Enabled";
            this.cb_SetupModeEn.UseVisualStyleBackColor = true;
            // 
            // btn_sendUpdateCmd
            // 
            this.btn_sendUpdateCmd.Location = new System.Drawing.Point(36, 127);
            this.btn_sendUpdateCmd.Name = "btn_sendUpdateCmd";
            this.btn_sendUpdateCmd.Size = new System.Drawing.Size(75, 23);
            this.btn_sendUpdateCmd.TabIndex = 26;
            this.btn_sendUpdateCmd.Text = "Update";
            this.btn_sendUpdateCmd.UseVisualStyleBackColor = true;
            this.btn_sendUpdateCmd.Click += new System.EventHandler(this.btn_sendUpdateCmd_Click);
            // 
            // cb_enableStream
            // 
            this.cb_enableStream.AutoSize = true;
            this.cb_enableStream.Location = new System.Drawing.Point(174, 43);
            this.cb_enableStream.Name = "cb_enableStream";
            this.cb_enableStream.Size = new System.Drawing.Size(59, 17);
            this.cb_enableStream.TabIndex = 27;
            this.cb_enableStream.Text = "Stream";
            this.cb_enableStream.UseVisualStyleBackColor = true;
            this.cb_enableStream.CheckedChanged += new System.EventHandler(this.cb_enableStream_CheckedChanged);
            // 
            // nud_SelectedImu
            // 
            this.nud_SelectedImu.Location = new System.Drawing.Point(36, 18);
            this.nud_SelectedImu.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nud_SelectedImu.Name = "nud_SelectedImu";
            this.nud_SelectedImu.Size = new System.Drawing.Size(120, 20);
            this.nud_SelectedImu.TabIndex = 36;
            // 
            // cb_logErrors
            // 
            this.cb_logErrors.AutoSize = true;
            this.cb_logErrors.Location = new System.Drawing.Point(174, 71);
            this.cb_logErrors.Name = "cb_logErrors";
            this.cb_logErrors.Size = new System.Drawing.Size(74, 17);
            this.cb_logErrors.TabIndex = 38;
            this.cb_logErrors.Text = "Log Errors";
            this.cb_logErrors.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cb_fpBaudRate);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.lb_forwardPort);
            this.tabPage1.Controls.Add(this.cb_OutputFormat);
            this.tabPage1.Controls.Add(this.cb_EnableFowardPort);
            this.tabPage1.Controls.Add(this.btn_StopStreaming);
            this.tabPage1.Controls.Add(this.cb_forwardPorts);
            this.tabPage1.Controls.Add(this.btn_startStream);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(468, 229);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Record 485 Frames";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cb_fpBaudRate
            // 
            this.cb_fpBaudRate.FormattingEnabled = true;
            this.cb_fpBaudRate.Location = new System.Drawing.Point(252, 59);
            this.cb_fpBaudRate.Name = "cb_fpBaudRate";
            this.cb_fpBaudRate.Size = new System.Drawing.Size(156, 21);
            this.cb_fpBaudRate.TabIndex = 50;
            this.cb_fpBaudRate.Text = "Baudrate";
            this.cb_fpBaudRate.SelectedIndexChanged += new System.EventHandler(this.cb_fpBaudRate_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Output Format";
            // 
            // lb_forwardPort
            // 
            this.lb_forwardPort.AutoSize = true;
            this.lb_forwardPort.Location = new System.Drawing.Point(179, 40);
            this.lb_forwardPort.Name = "lb_forwardPort";
            this.lb_forwardPort.Size = new System.Drawing.Size(67, 13);
            this.lb_forwardPort.TabIndex = 48;
            this.lb_forwardPort.Text = "Forward Port";
            // 
            // cb_OutputFormat
            // 
            this.cb_OutputFormat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cb_OutputFormat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_OutputFormat.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cb_OutputFormat.FormattingEnabled = true;
            this.cb_OutputFormat.Location = new System.Drawing.Point(104, 118);
            this.cb_OutputFormat.Name = "cb_OutputFormat";
            this.cb_OutputFormat.Size = new System.Drawing.Size(163, 21);
            this.cb_OutputFormat.TabIndex = 47;
            // 
            // cb_EnableFowardPort
            // 
            this.cb_EnableFowardPort.AutoSize = true;
            this.cb_EnableFowardPort.Location = new System.Drawing.Point(252, 9);
            this.cb_EnableFowardPort.Name = "cb_EnableFowardPort";
            this.cb_EnableFowardPort.Size = new System.Drawing.Size(100, 17);
            this.cb_EnableFowardPort.TabIndex = 46;
            this.cb_EnableFowardPort.Text = "Forward Stream";
            this.cb_EnableFowardPort.UseVisualStyleBackColor = true;
            // 
            // btn_StopStreaming
            // 
            this.btn_StopStreaming.Location = new System.Drawing.Point(27, 80);
            this.btn_StopStreaming.Name = "btn_StopStreaming";
            this.btn_StopStreaming.Size = new System.Drawing.Size(112, 23);
            this.btn_StopStreaming.TabIndex = 45;
            this.btn_StopStreaming.Text = "Stop Streaming";
            this.btn_StopStreaming.UseVisualStyleBackColor = true;
            this.btn_StopStreaming.Click += new System.EventHandler(this.btn_StopStreaming_Click);
            // 
            // cb_forwardPorts
            // 
            this.cb_forwardPorts.FormattingEnabled = true;
            this.cb_forwardPorts.Location = new System.Drawing.Point(252, 32);
            this.cb_forwardPorts.Name = "cb_forwardPorts";
            this.cb_forwardPorts.Size = new System.Drawing.Size(156, 21);
            this.cb_forwardPorts.TabIndex = 37;
            // 
            // btn_startStream
            // 
            this.btn_startStream.Location = new System.Drawing.Point(27, 32);
            this.btn_startStream.Name = "btn_startStream";
            this.btn_startStream.Size = new System.Drawing.Size(112, 23);
            this.btn_startStream.TabIndex = 44;
            this.btn_startStream.Text = "Start Streaming";
            this.btn_startStream.UseVisualStyleBackColor = true;
            this.btn_startStream.Click += new System.EventHandler(this.btn_startStream_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 368);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(476, 255);
            this.tabControl1.TabIndex = 46;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Controls.Add(this.splitter1);
            this.tabPage4.Controls.Add(this.btn_arm_togglePort);
            this.tabPage4.Controls.Add(this.chb_arm_EnableTracing);
            this.tabPage4.Controls.Add(this.lb_loopsExecuted);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.countDelaySel);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.trackBar1);
            this.tabPage4.Controls.Add(this.btn_arm_browseFile);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.textBox1);
            this.tabPage4.Controls.Add(this.cb_robotPort);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(468, 229);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Robot Arm Control";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btb_arm_playRecording);
            this.groupBox2.Controls.Add(this.btn_arm_recMovement);
            this.groupBox2.Location = new System.Drawing.Point(248, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(86, 80);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Record";
            // 
            // btb_arm_playRecording
            // 
            this.btb_arm_playRecording.Location = new System.Drawing.Point(6, 48);
            this.btb_arm_playRecording.Name = "btb_arm_playRecording";
            this.btb_arm_playRecording.Size = new System.Drawing.Size(75, 23);
            this.btb_arm_playRecording.TabIndex = 18;
            this.btb_arm_playRecording.Text = "Play";
            this.toolTip1.SetToolTip(this.btb_arm_playRecording, "Playback recording from Robot\'s internal memory\r\n");
            this.btb_arm_playRecording.UseVisualStyleBackColor = true;
            this.btb_arm_playRecording.Click += new System.EventHandler(this.btb_arm_playRecording_Click);
            this.btb_arm_playRecording.MouseEnter += new System.EventHandler(this.btb_arm_playRecording_MouseEnter);
            // 
            // btn_arm_recMovement
            // 
            this.btn_arm_recMovement.Location = new System.Drawing.Point(6, 19);
            this.btn_arm_recMovement.Name = "btn_arm_recMovement";
            this.btn_arm_recMovement.Size = new System.Drawing.Size(75, 23);
            this.btn_arm_recMovement.TabIndex = 17;
            this.btn_arm_recMovement.Text = "Rec";
            this.btn_arm_recMovement.UseVisualStyleBackColor = true;
            this.btn_arm_recMovement.Click += new System.EventHandler(this.btn_arm_recMovement_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chb_arm_easeMovement);
            this.groupBox1.Controls.Add(this.btn_arm_startStreaming);
            this.groupBox1.Controls.Add(this.btn_arm_stopStreaming);
            this.groupBox1.Location = new System.Drawing.Point(340, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(125, 107);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stream";
            // 
            // chb_arm_easeMovement
            // 
            this.chb_arm_easeMovement.AutoSize = true;
            this.chb_arm_easeMovement.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_arm_easeMovement.Location = new System.Drawing.Point(12, 74);
            this.chb_arm_easeMovement.Name = "chb_arm_easeMovement";
            this.chb_arm_easeMovement.Size = new System.Drawing.Size(102, 17);
            this.chb_arm_easeMovement.TabIndex = 13;
            this.chb_arm_easeMovement.Text = "Ease movement";
            this.chb_arm_easeMovement.UseVisualStyleBackColor = true;
            this.chb_arm_easeMovement.CheckedChanged += new System.EventHandler(this.chb_arm_easeMovement_CheckedChanged);
            // 
            // btn_arm_startStreaming
            // 
            this.btn_arm_startStreaming.BackColor = System.Drawing.Color.LightGreen;
            this.btn_arm_startStreaming.Location = new System.Drawing.Point(12, 19);
            this.btn_arm_startStreaming.Name = "btn_arm_startStreaming";
            this.btn_arm_startStreaming.Size = new System.Drawing.Size(90, 23);
            this.btn_arm_startStreaming.TabIndex = 4;
            this.btn_arm_startStreaming.Text = "Start movement";
            this.btn_arm_startStreaming.UseVisualStyleBackColor = false;
            this.btn_arm_startStreaming.Click += new System.EventHandler(this.btn_arm_startStreaming_Click);
            // 
            // btn_arm_stopStreaming
            // 
            this.btn_arm_stopStreaming.Location = new System.Drawing.Point(12, 45);
            this.btn_arm_stopStreaming.Name = "btn_arm_stopStreaming";
            this.btn_arm_stopStreaming.Size = new System.Drawing.Size(94, 23);
            this.btn_arm_stopStreaming.TabIndex = 12;
            this.btn_arm_stopStreaming.Text = "Stop movement";
            this.btn_arm_stopStreaming.UseVisualStyleBackColor = true;
            this.btn_arm_stopStreaming.Click += new System.EventHandler(this.btn_arm_stopStreaming_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 229);
            this.splitter1.TabIndex = 18;
            this.splitter1.TabStop = false;
            // 
            // btn_arm_togglePort
            // 
            this.btn_arm_togglePort.Location = new System.Drawing.Point(136, 91);
            this.btn_arm_togglePort.Name = "btn_arm_togglePort";
            this.btn_arm_togglePort.Size = new System.Drawing.Size(75, 23);
            this.btn_arm_togglePort.TabIndex = 16;
            this.btn_arm_togglePort.Text = "Open";
            this.btn_arm_togglePort.UseVisualStyleBackColor = true;
            this.btn_arm_togglePort.Click += new System.EventHandler(this.btn_arm_togglePort_Click);
            // 
            // chb_arm_EnableTracing
            // 
            this.chb_arm_EnableTracing.AutoSize = true;
            this.chb_arm_EnableTracing.Location = new System.Drawing.Point(306, 4);
            this.chb_arm_EnableTracing.Name = "chb_arm_EnableTracing";
            this.chb_arm_EnableTracing.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chb_arm_EnableTracing.Size = new System.Drawing.Size(144, 17);
            this.chb_arm_EnableTracing.TabIndex = 15;
            this.chb_arm_EnableTracing.Text = "Enable Precision Tracing";
            this.toolTip1.SetToolTip(this.chb_arm_EnableTracing, "Checks sensor repeatability.\r\n");
            this.chb_arm_EnableTracing.UseVisualStyleBackColor = true;
            this.chb_arm_EnableTracing.CheckedChanged += new System.EventHandler(this.chb_arm_EnableTracing_CheckedChanged);
            this.chb_arm_EnableTracing.MouseEnter += new System.EventHandler(this.checkBox1_MouseEnter);
            // 
            // lb_loopsExecuted
            // 
            this.lb_loopsExecuted.AutoSize = true;
            this.lb_loopsExecuted.Location = new System.Drawing.Point(322, 159);
            this.lb_loopsExecuted.Name = "lb_loopsExecuted";
            this.lb_loopsExecuted.Size = new System.Drawing.Size(13, 13);
            this.lb_loopsExecuted.TabIndex = 14;
            this.lb_loopsExecuted.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Loops Executed: ";
            // 
            // countDelaySel
            // 
            this.countDelaySel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.countDelaySel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.countDelaySel.FormattingEnabled = true;
            this.countDelaySel.Items.AddRange(new object[] {
            "Loop Count",
            "Line Delay"});
            this.countDelaySel.Location = new System.Drawing.Point(21, 159);
            this.countDelaySel.Name = "countDelaySel";
            this.countDelaySel.Size = new System.Drawing.Size(95, 21);
            this.countDelaySel.TabIndex = 11;
            this.countDelaySel.Text = "Count / Delay";
            this.countDelaySel.ValueMemberChanged += new System.EventHandler(this.countDelaySel_ValueMemberChanged);
            this.countDelaySel.TextChanged += new System.EventHandler(this.countDelaySel_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(145, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "0";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(21, 181);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(414, 45);
            this.trackBar1.TabIndex = 8;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseUp);
            // 
            // btn_arm_browseFile
            // 
            this.btn_arm_browseFile.Location = new System.Drawing.Point(306, 27);
            this.btn_arm_browseFile.Name = "btn_arm_browseFile";
            this.btn_arm_browseFile.Size = new System.Drawing.Size(75, 23);
            this.btn_arm_browseFile.TabIndex = 7;
            this.btn_arm_browseFile.Text = "Browse";
            this.btn_arm_browseFile.UseVisualStyleBackColor = true;
            this.btn_arm_browseFile.Click += new System.EventHandler(this.btn_arm_browseFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Baudrate is always 115200";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output Com Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Plotter movement source";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 20);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox1.Size = new System.Drawing.Size(291, 34);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "File Address";
            this.textBox1.WordWrap = false;
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // cb_robotPort
            // 
            this.cb_robotPort.FormattingEnabled = true;
            this.cb_robotPort.Location = new System.Drawing.Point(9, 91);
            this.cb_robotPort.MaxDropDownItems = 100;
            this.cb_robotPort.Name = "cb_robotPort";
            this.cb_robotPort.Size = new System.Drawing.Size(121, 21);
            this.cb_robotPort.TabIndex = 1;
            this.cb_robotPort.SelectedIndexChanged += new System.EventHandler(this.cb_robotPort_SelectedIndexChanged);
            // 
            // btn_refreshComPorts
            // 
            this.btn_refreshComPorts.Location = new System.Drawing.Point(368, 255);
            this.btn_refreshComPorts.Name = "btn_refreshComPorts";
            this.btn_refreshComPorts.Size = new System.Drawing.Size(126, 23);
            this.btn_refreshComPorts.TabIndex = 47;
            this.btn_refreshComPorts.Text = "Rescan Com Ports";
            this.btn_refreshComPorts.UseVisualStyleBackColor = true;
            this.btn_refreshComPorts.Click += new System.EventHandler(this.btn_refreshComPorts_Click);
            // 
            // robotArmPort
            // 
            this.robotArmPort.BaudRate = 115200;
            this.robotArmPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.robotArmPort_DataReceived);
            // 
            // bgw_uarmFileWorker
            // 
            this.bgw_uarmFileWorker.WorkerReportsProgress = true;
            this.bgw_uarmFileWorker.WorkerSupportsCancellation = true;
            this.bgw_uarmFileWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_uarmFileWorker_DoWork);
            this.bgw_uarmFileWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgw_uarmFileWorker_ProgressChanged);
            this.bgw_uarmFileWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_uarmFileWorker_RunWorkerCompleted);
            // 
            // ofd_openUarmFile
            // 
            this.ofd_openUarmFile.FileName = "openFileDialog1";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 10;
            this.toolTip1.ReshowDelay = 100;
            // 
            // lbl_data1
            // 
            this.lbl_data1.AutoSize = true;
            this.lbl_data1.Location = new System.Drawing.Point(800, 435);
            this.lbl_data1.Name = "lbl_data1";
            this.lbl_data1.Size = new System.Drawing.Size(42, 13);
            this.lbl_data1.TabIndex = 48;
            this.lbl_data1.Text = "Data 1:";
            // 
            // lbl_data2
            // 
            this.lbl_data2.AutoSize = true;
            this.lbl_data2.Location = new System.Drawing.Point(800, 465);
            this.lbl_data2.Name = "lbl_data2";
            this.lbl_data2.Size = new System.Drawing.Size(42, 13);
            this.lbl_data2.TabIndex = 49;
            this.lbl_data2.Text = "Data 2:";
            // 
            // lbl_data3
            // 
            this.lbl_data3.AutoSize = true;
            this.lbl_data3.Location = new System.Drawing.Point(800, 493);
            this.lbl_data3.Name = "lbl_data3";
            this.lbl_data3.Size = new System.Drawing.Size(42, 13);
            this.lbl_data3.TabIndex = 50;
            this.lbl_data3.Text = "Data 3:";
            // 
            // lbl_data4
            // 
            this.lbl_data4.AutoSize = true;
            this.lbl_data4.Location = new System.Drawing.Point(800, 522);
            this.lbl_data4.Name = "lbl_data4";
            this.lbl_data4.Size = new System.Drawing.Size(42, 13);
            this.lbl_data4.TabIndex = 51;
            this.lbl_data4.Text = "Data 4:";
            // 
            // lbl_valueData1
            // 
            this.lbl_valueData1.AutoSize = true;
            this.lbl_valueData1.Location = new System.Drawing.Point(849, 435);
            this.lbl_valueData1.Name = "lbl_valueData1";
            this.lbl_valueData1.Size = new System.Drawing.Size(13, 13);
            this.lbl_valueData1.TabIndex = 52;
            this.lbl_valueData1.Text = "0";
            this.lbl_valueData1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_valueData2
            // 
            this.lbl_valueData2.AutoSize = true;
            this.lbl_valueData2.Location = new System.Drawing.Point(849, 465);
            this.lbl_valueData2.Name = "lbl_valueData2";
            this.lbl_valueData2.Size = new System.Drawing.Size(13, 13);
            this.lbl_valueData2.TabIndex = 53;
            this.lbl_valueData2.Text = "0";
            this.lbl_valueData2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_valueData3
            // 
            this.lbl_valueData3.AutoSize = true;
            this.lbl_valueData3.Location = new System.Drawing.Point(848, 493);
            this.lbl_valueData3.Name = "lbl_valueData3";
            this.lbl_valueData3.Size = new System.Drawing.Size(13, 13);
            this.lbl_valueData3.TabIndex = 54;
            this.lbl_valueData3.Text = "0";
            this.lbl_valueData3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_valueData4
            // 
            this.lbl_valueData4.AutoSize = true;
            this.lbl_valueData4.Location = new System.Drawing.Point(848, 522);
            this.lbl_valueData4.Name = "lbl_valueData4";
            this.lbl_valueData4.Size = new System.Drawing.Size(13, 13);
            this.lbl_valueData4.TabIndex = 55;
            this.lbl_valueData4.Text = "0";
            this.lbl_valueData4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 635);
            this.Controls.Add(this.lbl_valueData4);
            this.Controls.Add(this.lbl_valueData3);
            this.Controls.Add(this.lbl_valueData2);
            this.Controls.Add(this.lbl_valueData1);
            this.Controls.Add(this.lbl_data4);
            this.Controls.Add(this.lbl_data3);
            this.Controls.Add(this.lbl_data2);
            this.Controls.Add(this.lbl_data1);
            this.Controls.Add(this.btn_refreshComPorts);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btn_clearScreen);
            this.Controls.Add(this.chrt_dataChart);
            this.Controls.Add(this.btn_setAxis);
            this.Controls.Add(this.tb_y_max);
            this.Controls.Add(this.cb_BaudRate);
            this.Controls.Add(this.tb_y_min);
            this.Controls.Add(this.btn_disconnect);
            this.Controls.Add(this.lb_y_min);
            this.Controls.Add(this.tb_Console);
            this.Controls.Add(this.lb_y_max);
            this.Controls.Add(this.bnt_Connect);
            this.Controls.Add(this.cb_serialPorts);
            this.Name = "mainForm";
            this.Text = "Sean\'s Packet Tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chrt_dataChart)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_updateRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_gyroRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_accelRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_magRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SelectedImu)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_disconnect;
        private System.Windows.Forms.TextBox tb_Console;
        private System.Windows.Forms.Button bnt_Connect;
        private System.Windows.Forms.ComboBox cb_serialPorts;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.ComboBox cb_BaudRate;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrt_dataChart;
        private System.Windows.Forms.TextBox tb_y_max;
        private System.Windows.Forms.TextBox tb_y_min;
        private System.Windows.Forms.Label lb_y_max;
        private System.Windows.Forms.Label lb_y_min;
        private System.Windows.Forms.Button btn_setAxis;
        private System.Windows.Forms.Button btn_clearScreen;
        private System.IO.Ports.SerialPort forwardSerialPort;
        private System.Windows.Forms.SaveFileDialog sfd_saveFileDialog;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btn_disconnectSock;
        private System.Windows.Forms.CheckBox cb_udpSelected;
        private System.Windows.Forms.MaskedTextBox mtb_NetAddress;
        private System.Windows.Forms.MaskedTextBox mtb_netPort;
        private System.Windows.Forms.Button btn_connectSocket;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox cb_ypr;
        private System.Windows.Forms.Label lb_rates;
        private System.Windows.Forms.Button btn_setRate;
        private System.Windows.Forms.NumericUpDown nud_gyroRate;
        private System.Windows.Forms.NumericUpDown nud_accelRate;
        private System.Windows.Forms.NumericUpDown nud_magRate;
        private System.Windows.Forms.ComboBox cb_dataType;
        private System.Windows.Forms.Button btn_getStatus;
        private System.Windows.Forms.Button btn_getFrame;
        private System.Windows.Forms.Button btn_SetupMode;
        private System.Windows.Forms.CheckBox cb_SetupModeEn;
        private System.Windows.Forms.Button btn_sendUpdateCmd;
        private System.Windows.Forms.CheckBox cb_enableStream;
        private System.Windows.Forms.NumericUpDown nud_SelectedImu;
        private System.Windows.Forms.CheckBox cb_logErrors;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox cb_fpBaudRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_forwardPort;
        private System.Windows.Forms.ComboBox cb_OutputFormat;
        private System.Windows.Forms.CheckBox cb_EnableFowardPort;
        private System.Windows.Forms.Button btn_StopStreaming;
        private System.Windows.Forms.ComboBox cb_forwardPorts;
        private System.Windows.Forms.Button btn_startStream;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox tb_dataLogLocation;
        private System.Windows.Forms.CheckBox cb_saveSensorData;
        private System.Windows.Forms.Label lb_updateRate;
        private System.Windows.Forms.NumericUpDown nud_updateRate;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ComboBox cb_robotPort;
        private System.Windows.Forms.Button btn_refreshComPorts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_arm_startStreaming;
        private System.IO.Ports.SerialPort robotArmPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker bgw_uarmFileWorker;
        private System.Windows.Forms.Button btn_arm_browseFile;
        private System.Windows.Forms.OpenFileDialog ofd_openUarmFile;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox countDelaySel;
        private System.Windows.Forms.Button btn_arm_stopStreaming;
        private System.Windows.Forms.Label lb_loopsExecuted;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chb_arm_EnableTracing;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lbl_data1;
        private System.Windows.Forms.Label lbl_data2;
        private System.Windows.Forms.Label lbl_data3;
        private System.Windows.Forms.Label lbl_data4;
        private System.Windows.Forms.Label lbl_valueData1;
        private System.Windows.Forms.Label lbl_valueData2;
        private System.Windows.Forms.Label lbl_valueData3;
        private System.Windows.Forms.Label lbl_valueData4;
        private System.Windows.Forms.Button btn_arm_togglePort;
        private System.Windows.Forms.Button btn_arm_recMovement;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button btb_arm_playRecording;
        private System.Windows.Forms.CheckBox chb_arm_easeMovement;
        private System.Windows.Forms.FolderBrowserDialog fbd_folderBrowser;
    }
}

