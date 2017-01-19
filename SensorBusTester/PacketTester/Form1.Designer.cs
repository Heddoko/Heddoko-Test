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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
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
            this.sfd_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btn_setSensorId = new System.Windows.Forms.Button();
            this.nud_setId = new System.Windows.Forms.NumericUpDown();
            this.lb_GyroRange = new System.Windows.Forms.Label();
            this.lb_accelRange = new System.Windows.Forms.Label();
            this.lb_magRange = new System.Windows.Forms.Label();
            this.cb_gyroRange = new System.Windows.Forms.ComboBox();
            this.cb_accelRange = new System.Windows.Forms.ComboBox();
            this.cb_magRange = new System.Windows.Forms.ComboBox();
            this.btn_getConfig = new System.Windows.Forms.Button();
            this.btn_SaveConfig = new System.Windows.Forms.Button();
            this.btn_SendConfig = new System.Windows.Forms.Button();
            this.lb_AlgorithmControl = new System.Windows.Forms.Label();
            this.clb_algoConfig = new System.Windows.Forms.CheckedListBox();
            this.lbl_AlgorithmStatus = new System.Windows.Forms.Label();
            this.clb_algorithmStatus = new System.Windows.Forms.CheckedListBox();
            this.btn_getWarmUp = new System.Windows.Forms.Button();
            this.btn_updateWarmUp = new System.Windows.Forms.Button();
            this.btn_getCfgParam = new System.Windows.Forms.Button();
            this.btn_updateCfgParam = new System.Windows.Forms.Button();
            this.tb_dataLogLocation = new System.Windows.Forms.TextBox();
            this.cb_saveSensorData = new System.Windows.Forms.CheckBox();
            this.lb_updateRate = new System.Windows.Forms.Label();
            this.nud_updateRate = new System.Windows.Forms.NumericUpDown();
            this.lb_rates = new System.Windows.Forms.Label();
            this.btn_setRate = new System.Windows.Forms.Button();
            this.nud_gyroRate = new System.Windows.Forms.NumericUpDown();
            this.nud_accelRate = new System.Windows.Forms.NumericUpDown();
            this.nud_magRate = new System.Windows.Forms.NumericUpDown();
            this.cb_dataType = new System.Windows.Forms.ComboBox();
            this.btn_getStatus = new System.Windows.Forms.Button();
            this.btn_getFrame = new System.Windows.Forms.Button();
            this.cb_SetupModeEn = new System.Windows.Forms.CheckBox();
            this.btn_sendUpdateCmd = new System.Windows.Forms.Button();
            this.cb_enableStream = new System.Windows.Forms.CheckBox();
            this.nud_SelectedImu = new System.Windows.Forms.NumericUpDown();
            this.cb_logErrors = new System.Windows.Forms.CheckBox();
            this.btn_refreshComPorts = new System.Windows.Forms.Button();
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
            this.btn_sendEepromFile = new System.Windows.Forms.Button();
            this.btn_getEepromFile = new System.Windows.Forms.Button();
            this.ofd_openFile = new System.Windows.Forms.OpenFileDialog();
            this.btn_cancelTransfer = new System.Windows.Forms.Button();
            this.tmr_transferTimer = new System.Windows.Forms.Timer(this.components);
            this.clb_sensorStatus = new System.Windows.Forms.CheckedListBox();
            this.lb_SensorStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chrt_dataChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_setId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_updateRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_gyroRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_accelRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_magRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SelectedImu)).BeginInit();
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
            this.cb_BaudRate.SelectedIndexChanged += new System.EventHandler(this.cb_BaudRate_SelectedIndexChanged);
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
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.MarkerSize = 3;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "X";
            series1.YValuesPerPoint = 2;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series2.BorderWidth = 5;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.MarkerSize = 3;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Y";
            series3.BorderWidth = 5;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series3.Legend = "Legend1";
            series3.MarkerSize = 3;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Z";
            series4.BorderWidth = 5;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series4.Legend = "Legend1";
            series4.MarkerSize = 3;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series4.Name = "W";
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
            this.tb_y_max.Location = new System.Drawing.Point(954, 595);
            this.tb_y_max.Name = "tb_y_max";
            this.tb_y_max.Size = new System.Drawing.Size(100, 20);
            this.tb_y_max.TabIndex = 29;
            this.tb_y_max.Text = "1.1";
            this.tb_y_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_y_max.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_y_max_KeyPress);
            // 
            // tb_y_min
            // 
            this.tb_y_min.Location = new System.Drawing.Point(954, 628);
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
            this.lb_y_max.Location = new System.Drawing.Point(913, 602);
            this.lb_y_max.Name = "lb_y_max";
            this.lb_y_max.Size = new System.Drawing.Size(37, 13);
            this.lb_y_max.TabIndex = 31;
            this.lb_y_max.Text = "Y-Max";
            // 
            // lb_y_min
            // 
            this.lb_y_min.AutoSize = true;
            this.lb_y_min.Location = new System.Drawing.Point(913, 635);
            this.lb_y_min.Name = "lb_y_min";
            this.lb_y_min.Size = new System.Drawing.Size(34, 13);
            this.lb_y_min.TabIndex = 32;
            this.lb_y_min.Text = "Y-Min";
            // 
            // btn_setAxis
            // 
            this.btn_setAxis.Location = new System.Drawing.Point(954, 658);
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
            // sfd_saveFileDialog
            // 
            this.sfd_saveFileDialog.Filter = "Comma Separated Files (*.csv)|*.csv|All Files (*.*)|*.*";
            this.sfd_saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.sfd_saveFileDialog_FileOk);
            // 
            // btn_setSensorId
            // 
            this.btn_setSensorId.Location = new System.Drawing.Point(132, 496);
            this.btn_setSensorId.Name = "btn_setSensorId";
            this.btn_setSensorId.Size = new System.Drawing.Size(101, 23);
            this.btn_setSensorId.TabIndex = 69;
            this.btn_setSensorId.Text = "Set Sensor Id";
            this.btn_setSensorId.UseVisualStyleBackColor = true;
            this.btn_setSensorId.Click += new System.EventHandler(this.btn_setSensorId_Click);
            // 
            // nud_setId
            // 
            this.nud_setId.Location = new System.Drawing.Point(36, 499);
            this.nud_setId.Name = "nud_setId";
            this.nud_setId.Size = new System.Drawing.Size(90, 20);
            this.nud_setId.TabIndex = 68;
            // 
            // lb_GyroRange
            // 
            this.lb_GyroRange.AutoSize = true;
            this.lb_GyroRange.Location = new System.Drawing.Point(474, 634);
            this.lb_GyroRange.Name = "lb_GyroRange";
            this.lb_GyroRange.Size = new System.Drawing.Size(84, 13);
            this.lb_GyroRange.TabIndex = 67;
            this.lb_GyroRange.Text = "Gyro Range(+/-)";
            // 
            // lb_accelRange
            // 
            this.lb_accelRange.AutoSize = true;
            this.lb_accelRange.Location = new System.Drawing.Point(469, 607);
            this.lb_accelRange.Name = "lb_accelRange";
            this.lb_accelRange.Size = new System.Drawing.Size(89, 13);
            this.lb_accelRange.TabIndex = 66;
            this.lb_accelRange.Text = "Accel Range(+/-)";
            // 
            // lb_magRange
            // 
            this.lb_magRange.AutoSize = true;
            this.lb_magRange.Location = new System.Drawing.Point(469, 584);
            this.lb_magRange.Name = "lb_magRange";
            this.lb_magRange.Size = new System.Drawing.Size(83, 13);
            this.lb_magRange.TabIndex = 65;
            this.lb_magRange.Text = "Mag Range(+/-)";
            // 
            // cb_gyroRange
            // 
            this.cb_gyroRange.FormattingEnabled = true;
            this.cb_gyroRange.Items.AddRange(new object[] {
            "125",
            "500",
            "1000",
            "2000"});
            this.cb_gyroRange.Location = new System.Drawing.Point(558, 631);
            this.cb_gyroRange.Name = "cb_gyroRange";
            this.cb_gyroRange.Size = new System.Drawing.Size(75, 21);
            this.cb_gyroRange.TabIndex = 64;
            this.cb_gyroRange.SelectedIndexChanged += new System.EventHandler(this.cb_gyroRange_SelectedIndexChanged);
            // 
            // cb_accelRange
            // 
            this.cb_accelRange.FormattingEnabled = true;
            this.cb_accelRange.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16"});
            this.cb_accelRange.Location = new System.Drawing.Point(558, 604);
            this.cb_accelRange.Name = "cb_accelRange";
            this.cb_accelRange.Size = new System.Drawing.Size(75, 21);
            this.cb_accelRange.TabIndex = 63;
            // 
            // cb_magRange
            // 
            this.cb_magRange.FormattingEnabled = true;
            this.cb_magRange.Items.AddRange(new object[] {
            "1000"});
            this.cb_magRange.Location = new System.Drawing.Point(558, 574);
            this.cb_magRange.Name = "cb_magRange";
            this.cb_magRange.Size = new System.Drawing.Size(75, 21);
            this.cb_magRange.TabIndex = 62;
            // 
            // btn_getConfig
            // 
            this.btn_getConfig.Location = new System.Drawing.Point(558, 687);
            this.btn_getConfig.Name = "btn_getConfig";
            this.btn_getConfig.Size = new System.Drawing.Size(75, 23);
            this.btn_getConfig.TabIndex = 61;
            this.btn_getConfig.Text = "Get Config";
            this.btn_getConfig.UseVisualStyleBackColor = true;
            this.btn_getConfig.Click += new System.EventHandler(this.btn_getConfig_Click);
            // 
            // btn_SaveConfig
            // 
            this.btn_SaveConfig.Location = new System.Drawing.Point(422, 658);
            this.btn_SaveConfig.Name = "btn_SaveConfig";
            this.btn_SaveConfig.Size = new System.Drawing.Size(120, 23);
            this.btn_SaveConfig.TabIndex = 60;
            this.btn_SaveConfig.Text = "Save Config to NVM";
            this.btn_SaveConfig.UseVisualStyleBackColor = true;
            this.btn_SaveConfig.Click += new System.EventHandler(this.btn_SaveConfig_Click);
            // 
            // btn_SendConfig
            // 
            this.btn_SendConfig.Location = new System.Drawing.Point(558, 658);
            this.btn_SendConfig.Name = "btn_SendConfig";
            this.btn_SendConfig.Size = new System.Drawing.Size(75, 23);
            this.btn_SendConfig.TabIndex = 59;
            this.btn_SendConfig.Text = "Send Config";
            this.btn_SendConfig.UseVisualStyleBackColor = true;
            this.btn_SendConfig.Click += new System.EventHandler(this.btn_SendConfig_Click);
            // 
            // lb_AlgorithmControl
            // 
            this.lb_AlgorithmControl.AutoSize = true;
            this.lb_AlgorithmControl.Location = new System.Drawing.Point(510, 419);
            this.lb_AlgorithmControl.Name = "lb_AlgorithmControl";
            this.lb_AlgorithmControl.Size = new System.Drawing.Size(86, 13);
            this.lb_AlgorithmControl.TabIndex = 58;
            this.lb_AlgorithmControl.Text = "Algorithm Control";
            // 
            // clb_algoConfig
            // 
            this.clb_algoConfig.FormattingEnabled = true;
            this.clb_algoConfig.Items.AddRange(new object[] {
            "Standby EN",
            "Raw Data EN",
            "HPR EN",
            "6-axis EN",
            "ENU EN",
            "Gyro Off when still",
            "Load Warm Start",
            "Load Ranges"});
            this.clb_algoConfig.Location = new System.Drawing.Point(513, 440);
            this.clb_algoConfig.Name = "clb_algoConfig";
            this.clb_algoConfig.Size = new System.Drawing.Size(120, 124);
            this.clb_algoConfig.TabIndex = 57;
            // 
            // lbl_AlgorithmStatus
            // 
            this.lbl_AlgorithmStatus.AutoSize = true;
            this.lbl_AlgorithmStatus.Location = new System.Drawing.Point(803, 421);
            this.lbl_AlgorithmStatus.Name = "lbl_AlgorithmStatus";
            this.lbl_AlgorithmStatus.Size = new System.Drawing.Size(83, 13);
            this.lbl_AlgorithmStatus.TabIndex = 56;
            this.lbl_AlgorithmStatus.Text = "Algorithm Status";
            // 
            // clb_algorithmStatus
            // 
            this.clb_algorithmStatus.FormattingEnabled = true;
            this.clb_algorithmStatus.Items.AddRange(new object[] {
            "AlgorithmStandBy",
            "AlgorithmSlow",
            "Still",
            "CalStable",
            "MagTransient"});
            this.clb_algorithmStatus.Location = new System.Drawing.Point(803, 440);
            this.clb_algorithmStatus.Name = "clb_algorithmStatus";
            this.clb_algorithmStatus.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.clb_algorithmStatus.Size = new System.Drawing.Size(120, 94);
            this.clb_algorithmStatus.TabIndex = 55;
            this.clb_algorithmStatus.SelectedIndexChanged += new System.EventHandler(this.clb_algorithmStatus_SelectedIndexChanged);
            // 
            // btn_getWarmUp
            // 
            this.btn_getWarmUp.Location = new System.Drawing.Point(147, 572);
            this.btn_getWarmUp.Name = "btn_getWarmUp";
            this.btn_getWarmUp.Size = new System.Drawing.Size(103, 23);
            this.btn_getWarmUp.TabIndex = 53;
            this.btn_getWarmUp.Text = "Get Warm Up";
            this.btn_getWarmUp.UseVisualStyleBackColor = true;
            this.btn_getWarmUp.Click += new System.EventHandler(this.btn_getWarmUp_Click);
            // 
            // btn_updateWarmUp
            // 
            this.btn_updateWarmUp.Location = new System.Drawing.Point(31, 572);
            this.btn_updateWarmUp.Name = "btn_updateWarmUp";
            this.btn_updateWarmUp.Size = new System.Drawing.Size(103, 23);
            this.btn_updateWarmUp.TabIndex = 52;
            this.btn_updateWarmUp.Text = "Update Warm Up";
            this.btn_updateWarmUp.UseVisualStyleBackColor = true;
            this.btn_updateWarmUp.Click += new System.EventHandler(this.btn_updateWarmUp_Click);
            // 
            // btn_getCfgParam
            // 
            this.btn_getCfgParam.Location = new System.Drawing.Point(147, 542);
            this.btn_getCfgParam.Name = "btn_getCfgParam";
            this.btn_getCfgParam.Size = new System.Drawing.Size(103, 23);
            this.btn_getCfgParam.TabIndex = 51;
            this.btn_getCfgParam.Text = "Get Config";
            this.btn_getCfgParam.UseVisualStyleBackColor = true;
            this.btn_getCfgParam.Click += new System.EventHandler(this.btn_getCfgParam_Click);
            // 
            // btn_updateCfgParam
            // 
            this.btn_updateCfgParam.Location = new System.Drawing.Point(31, 542);
            this.btn_updateCfgParam.Name = "btn_updateCfgParam";
            this.btn_updateCfgParam.Size = new System.Drawing.Size(103, 23);
            this.btn_updateCfgParam.TabIndex = 50;
            this.btn_updateCfgParam.Text = "Update Config";
            this.btn_updateCfgParam.UseVisualStyleBackColor = true;
            this.btn_updateCfgParam.Click += new System.EventHandler(this.btn_updateCfgParam_Click);
            // 
            // tb_dataLogLocation
            // 
            this.tb_dataLogLocation.Location = new System.Drawing.Point(147, 625);
            this.tb_dataLogLocation.Name = "tb_dataLogLocation";
            this.tb_dataLogLocation.Size = new System.Drawing.Size(301, 20);
            this.tb_dataLogLocation.TabIndex = 49;
            // 
            // cb_saveSensorData
            // 
            this.cb_saveSensorData.AutoSize = true;
            this.cb_saveSensorData.Location = new System.Drawing.Point(31, 628);
            this.cb_saveSensorData.Name = "cb_saveSensorData";
            this.cb_saveSensorData.Size = new System.Drawing.Size(108, 17);
            this.cb_saveSensorData.TabIndex = 48;
            this.cb_saveSensorData.Text = "Save Data to File";
            this.cb_saveSensorData.UseVisualStyleBackColor = true;
            // 
            // lb_updateRate
            // 
            this.lb_updateRate.AutoSize = true;
            this.lb_updateRate.Location = new System.Drawing.Point(255, 358);
            this.lb_updateRate.Name = "lb_updateRate";
            this.lb_updateRate.Size = new System.Drawing.Size(87, 13);
            this.lb_updateRate.TabIndex = 47;
            this.lb_updateRate.Text = "Update Rate(ms)";
            // 
            // nud_updateRate
            // 
            this.nud_updateRate.Location = new System.Drawing.Point(258, 375);
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
            10,
            0,
            0,
            0});
            // 
            // lb_rates
            // 
            this.lb_rates.AutoSize = true;
            this.lb_rates.Location = new System.Drawing.Point(326, 414);
            this.lb_rates.Name = "lb_rates";
            this.lb_rates.Size = new System.Drawing.Size(176, 13);
            this.lb_rates.TabIndex = 44;
            this.lb_rates.Text = "Mag Rate    Accel Rate   Gyro Rate";
            // 
            // btn_setRate
            // 
            this.btn_setRate.Location = new System.Drawing.Point(390, 463);
            this.btn_setRate.Name = "btn_setRate";
            this.btn_setRate.Size = new System.Drawing.Size(102, 23);
            this.btn_setRate.TabIndex = 43;
            this.btn_setRate.Text = "Set Rate Config";
            this.btn_setRate.UseVisualStyleBackColor = true;
            this.btn_setRate.Click += new System.EventHandler(this.btn_setRate_Click);
            // 
            // nud_gyroRate
            // 
            this.nud_gyroRate.Location = new System.Drawing.Point(453, 432);
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
            this.nud_accelRate.Location = new System.Drawing.Point(391, 432);
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
            this.nud_magRate.Location = new System.Drawing.Point(328, 432);
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
            this.cb_dataType.Location = new System.Drawing.Point(36, 381);
            this.cb_dataType.Name = "cb_dataType";
            this.cb_dataType.Size = new System.Drawing.Size(121, 21);
            this.cb_dataType.TabIndex = 39;
            this.cb_dataType.SelectedIndexChanged += new System.EventHandler(this.cb_dataType_SelectedIndexChanged);
            // 
            // btn_getStatus
            // 
            this.btn_getStatus.Location = new System.Drawing.Point(653, 705);
            this.btn_getStatus.Name = "btn_getStatus";
            this.btn_getStatus.Size = new System.Drawing.Size(75, 23);
            this.btn_getStatus.TabIndex = 34;
            this.btn_getStatus.Text = "Get Status";
            this.btn_getStatus.UseVisualStyleBackColor = true;
            this.btn_getStatus.Click += new System.EventHandler(this.btn_getStatus_Click);
            // 
            // btn_getFrame
            // 
            this.btn_getFrame.Location = new System.Drawing.Point(36, 406);
            this.btn_getFrame.Name = "btn_getFrame";
            this.btn_getFrame.Size = new System.Drawing.Size(75, 23);
            this.btn_getFrame.TabIndex = 23;
            this.btn_getFrame.Text = "Get Frame";
            this.btn_getFrame.UseVisualStyleBackColor = true;
            this.btn_getFrame.Click += new System.EventHandler(this.btn_getFrame_Click);
            // 
            // cb_SetupModeEn
            // 
            this.cb_SetupModeEn.AutoSize = true;
            this.cb_SetupModeEn.Location = new System.Drawing.Point(36, 476);
            this.cb_SetupModeEn.Name = "cb_SetupModeEn";
            this.cb_SetupModeEn.Size = new System.Drawing.Size(84, 17);
            this.cb_SetupModeEn.TabIndex = 25;
            this.cb_SetupModeEn.Text = "Setup Mode";
            this.cb_SetupModeEn.UseVisualStyleBackColor = true;
            this.cb_SetupModeEn.CheckedChanged += new System.EventHandler(this.cb_SetupModeEn_CheckedChanged);
            // 
            // btn_sendUpdateCmd
            // 
            this.btn_sendUpdateCmd.Location = new System.Drawing.Point(36, 436);
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
            this.cb_enableStream.Location = new System.Drawing.Point(174, 378);
            this.cb_enableStream.Name = "cb_enableStream";
            this.cb_enableStream.Size = new System.Drawing.Size(59, 17);
            this.cb_enableStream.TabIndex = 27;
            this.cb_enableStream.Text = "Stream";
            this.cb_enableStream.UseVisualStyleBackColor = true;
            this.cb_enableStream.CheckedChanged += new System.EventHandler(this.cb_enableStream_CheckedChanged);
            // 
            // nud_SelectedImu
            // 
            this.nud_SelectedImu.Location = new System.Drawing.Point(36, 356);
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
            this.cb_logErrors.Location = new System.Drawing.Point(174, 401);
            this.cb_logErrors.Name = "cb_logErrors";
            this.cb_logErrors.Size = new System.Drawing.Size(74, 17);
            this.cb_logErrors.TabIndex = 38;
            this.cb_logErrors.Text = "Log Errors";
            this.cb_logErrors.UseVisualStyleBackColor = true;
            this.cb_logErrors.CheckedChanged += new System.EventHandler(this.cb_logErrors_CheckedChanged);
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
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 10;
            this.toolTip1.ReshowDelay = 100;
            // 
            // lbl_data1
            // 
            this.lbl_data1.AutoSize = true;
            this.lbl_data1.Location = new System.Drawing.Point(993, 433);
            this.lbl_data1.Name = "lbl_data1";
            this.lbl_data1.Size = new System.Drawing.Size(43, 13);
            this.lbl_data1.TabIndex = 48;
            this.lbl_data1.Text = "Data X:";
            // 
            // lbl_data2
            // 
            this.lbl_data2.AutoSize = true;
            this.lbl_data2.Location = new System.Drawing.Point(993, 463);
            this.lbl_data2.Name = "lbl_data2";
            this.lbl_data2.Size = new System.Drawing.Size(43, 13);
            this.lbl_data2.TabIndex = 49;
            this.lbl_data2.Text = "Data Y:";
            // 
            // lbl_data3
            // 
            this.lbl_data3.AutoSize = true;
            this.lbl_data3.Location = new System.Drawing.Point(993, 491);
            this.lbl_data3.Name = "lbl_data3";
            this.lbl_data3.Size = new System.Drawing.Size(43, 13);
            this.lbl_data3.TabIndex = 50;
            this.lbl_data3.Text = "Data Z:";
            // 
            // lbl_data4
            // 
            this.lbl_data4.AutoSize = true;
            this.lbl_data4.Location = new System.Drawing.Point(993, 520);
            this.lbl_data4.Name = "lbl_data4";
            this.lbl_data4.Size = new System.Drawing.Size(47, 13);
            this.lbl_data4.TabIndex = 51;
            this.lbl_data4.Text = "Data W:";
            // 
            // lbl_valueData1
            // 
            this.lbl_valueData1.AutoSize = true;
            this.lbl_valueData1.Location = new System.Drawing.Point(1042, 433);
            this.lbl_valueData1.Name = "lbl_valueData1";
            this.lbl_valueData1.Size = new System.Drawing.Size(13, 13);
            this.lbl_valueData1.TabIndex = 52;
            this.lbl_valueData1.Text = "0";
            this.lbl_valueData1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_valueData2
            // 
            this.lbl_valueData2.AutoSize = true;
            this.lbl_valueData2.Location = new System.Drawing.Point(1042, 463);
            this.lbl_valueData2.Name = "lbl_valueData2";
            this.lbl_valueData2.Size = new System.Drawing.Size(13, 13);
            this.lbl_valueData2.TabIndex = 53;
            this.lbl_valueData2.Text = "0";
            this.lbl_valueData2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_valueData3
            // 
            this.lbl_valueData3.AutoSize = true;
            this.lbl_valueData3.Location = new System.Drawing.Point(1041, 491);
            this.lbl_valueData3.Name = "lbl_valueData3";
            this.lbl_valueData3.Size = new System.Drawing.Size(13, 13);
            this.lbl_valueData3.TabIndex = 54;
            this.lbl_valueData3.Text = "0";
            this.lbl_valueData3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_valueData4
            // 
            this.lbl_valueData4.AutoSize = true;
            this.lbl_valueData4.Location = new System.Drawing.Point(1041, 520);
            this.lbl_valueData4.Name = "lbl_valueData4";
            this.lbl_valueData4.Size = new System.Drawing.Size(13, 13);
            this.lbl_valueData4.TabIndex = 55;
            this.lbl_valueData4.Text = "0";
            this.lbl_valueData4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_sendEepromFile
            // 
            this.btn_sendEepromFile.Location = new System.Drawing.Point(30, 666);
            this.btn_sendEepromFile.Name = "btn_sendEepromFile";
            this.btn_sendEepromFile.Size = new System.Drawing.Size(120, 23);
            this.btn_sendEepromFile.TabIndex = 70;
            this.btn_sendEepromFile.Text = "Send EEPROM File";
            this.btn_sendEepromFile.UseVisualStyleBackColor = true;
            this.btn_sendEepromFile.Click += new System.EventHandler(this.btn_sendEepromFile_Click);
            // 
            // btn_getEepromFile
            // 
            this.btn_getEepromFile.Location = new System.Drawing.Point(31, 696);
            this.btn_getEepromFile.Name = "btn_getEepromFile";
            this.btn_getEepromFile.Size = new System.Drawing.Size(119, 23);
            this.btn_getEepromFile.TabIndex = 71;
            this.btn_getEepromFile.Text = "Get EEPROM File";
            this.btn_getEepromFile.UseVisualStyleBackColor = true;
            this.btn_getEepromFile.Click += new System.EventHandler(this.btn_getEepromFile_Click);
            // 
            // ofd_openFile
            // 
            this.ofd_openFile.FileName = "pni binary.fw";
            // 
            // btn_cancelTransfer
            // 
            this.btn_cancelTransfer.Location = new System.Drawing.Point(174, 666);
            this.btn_cancelTransfer.Name = "btn_cancelTransfer";
            this.btn_cancelTransfer.Size = new System.Drawing.Size(95, 23);
            this.btn_cancelTransfer.TabIndex = 72;
            this.btn_cancelTransfer.Text = "Cancel Transfer";
            this.btn_cancelTransfer.UseVisualStyleBackColor = true;
            this.btn_cancelTransfer.Click += new System.EventHandler(this.btn_cancelTransfer_Click);
            // 
            // tmr_transferTimer
            // 
            this.tmr_transferTimer.Tick += new System.EventHandler(this.tmr_transferTimer_Tick);
            // 
            // clb_sensorStatus
            // 
            this.clb_sensorStatus.FormattingEnabled = true;
            this.clb_sensorStatus.Items.AddRange(new object[] {
            "CPU Reset",
            "Error",
            "Quaternion Result",
            "Mag Result",
            "Accel Result",
            "Gyro Result",
            "Mag NACK",
            "Accel NACK",
            "Gyro NACK",
            "Mag Dev ID Err",
            "Accel Dev ID Err",
            "Gyro Dev ID Err",
            "EEPROM Detected",
            "EE Upload Done",
            "EEPROM Error",
            "Idle",
            "No EEPROM"});
            this.clb_sensorStatus.Location = new System.Drawing.Point(653, 440);
            this.clb_sensorStatus.Name = "clb_sensorStatus";
            this.clb_sensorStatus.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.clb_sensorStatus.Size = new System.Drawing.Size(120, 259);
            this.clb_sensorStatus.TabIndex = 73;
            // 
            // lb_SensorStatus
            // 
            this.lb_SensorStatus.AutoSize = true;
            this.lb_SensorStatus.Location = new System.Drawing.Point(650, 421);
            this.lb_SensorStatus.Name = "lb_SensorStatus";
            this.lb_SensorStatus.Size = new System.Drawing.Size(73, 13);
            this.lb_SensorStatus.TabIndex = 74;
            this.lb_SensorStatus.Text = "Sensor Status";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 759);
            this.Controls.Add(this.lb_SensorStatus);
            this.Controls.Add(this.clb_sensorStatus);
            this.Controls.Add(this.btn_cancelTransfer);
            this.Controls.Add(this.btn_getEepromFile);
            this.Controls.Add(this.btn_sendEepromFile);
            this.Controls.Add(this.btn_setSensorId);
            this.Controls.Add(this.lbl_valueData4);
            this.Controls.Add(this.nud_setId);
            this.Controls.Add(this.lbl_valueData3);
            this.Controls.Add(this.lb_GyroRange);
            this.Controls.Add(this.lbl_valueData2);
            this.Controls.Add(this.lb_accelRange);
            this.Controls.Add(this.lbl_valueData1);
            this.Controls.Add(this.lb_magRange);
            this.Controls.Add(this.lbl_data4);
            this.Controls.Add(this.cb_gyroRange);
            this.Controls.Add(this.lbl_data3);
            this.Controls.Add(this.cb_accelRange);
            this.Controls.Add(this.lbl_data2);
            this.Controls.Add(this.cb_magRange);
            this.Controls.Add(this.lbl_data1);
            this.Controls.Add(this.btn_getConfig);
            this.Controls.Add(this.btn_refreshComPorts);
            this.Controls.Add(this.btn_SaveConfig);
            this.Controls.Add(this.btn_SendConfig);
            this.Controls.Add(this.btn_clearScreen);
            this.Controls.Add(this.lb_AlgorithmControl);
            this.Controls.Add(this.chrt_dataChart);
            this.Controls.Add(this.clb_algoConfig);
            this.Controls.Add(this.btn_setAxis);
            this.Controls.Add(this.lbl_AlgorithmStatus);
            this.Controls.Add(this.tb_y_max);
            this.Controls.Add(this.clb_algorithmStatus);
            this.Controls.Add(this.cb_BaudRate);
            this.Controls.Add(this.tb_y_min);
            this.Controls.Add(this.btn_getWarmUp);
            this.Controls.Add(this.btn_disconnect);
            this.Controls.Add(this.btn_updateWarmUp);
            this.Controls.Add(this.lb_y_min);
            this.Controls.Add(this.btn_getCfgParam);
            this.Controls.Add(this.btn_updateCfgParam);
            this.Controls.Add(this.tb_Console);
            this.Controls.Add(this.tb_dataLogLocation);
            this.Controls.Add(this.lb_y_max);
            this.Controls.Add(this.cb_saveSensorData);
            this.Controls.Add(this.bnt_Connect);
            this.Controls.Add(this.lb_updateRate);
            this.Controls.Add(this.cb_serialPorts);
            this.Controls.Add(this.nud_updateRate);
            this.Controls.Add(this.cb_logErrors);
            this.Controls.Add(this.nud_SelectedImu);
            this.Controls.Add(this.lb_rates);
            this.Controls.Add(this.cb_enableStream);
            this.Controls.Add(this.btn_setRate);
            this.Controls.Add(this.btn_sendUpdateCmd);
            this.Controls.Add(this.nud_gyroRate);
            this.Controls.Add(this.cb_SetupModeEn);
            this.Controls.Add(this.nud_accelRate);
            this.Controls.Add(this.btn_getFrame);
            this.Controls.Add(this.nud_magRate);
            this.Controls.Add(this.btn_getStatus);
            this.Controls.Add(this.cb_dataType);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainForm";
            this.Text = "Sean\'s Sensor Tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chrt_dataChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_setId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_updateRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_gyroRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_accelRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_magRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SelectedImu)).EndInit();
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
        private System.Windows.Forms.SaveFileDialog sfd_saveFileDialog;
        private System.Windows.Forms.Label lb_rates;
        private System.Windows.Forms.Button btn_setRate;
        private System.Windows.Forms.NumericUpDown nud_gyroRate;
        private System.Windows.Forms.NumericUpDown nud_accelRate;
        private System.Windows.Forms.NumericUpDown nud_magRate;
        private System.Windows.Forms.ComboBox cb_dataType;
        private System.Windows.Forms.Button btn_getStatus;
        private System.Windows.Forms.Button btn_getFrame;
        private System.Windows.Forms.CheckBox cb_SetupModeEn;
        private System.Windows.Forms.Button btn_sendUpdateCmd;
        private System.Windows.Forms.CheckBox cb_enableStream;
        private System.Windows.Forms.NumericUpDown nud_SelectedImu;
        private System.Windows.Forms.CheckBox cb_logErrors;
        private System.Windows.Forms.TextBox tb_dataLogLocation;
        private System.Windows.Forms.CheckBox cb_saveSensorData;
        private System.Windows.Forms.Label lb_updateRate;
        private System.Windows.Forms.NumericUpDown nud_updateRate;
        private System.Windows.Forms.Button btn_refreshComPorts;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lbl_data1;
        private System.Windows.Forms.Label lbl_data2;
        private System.Windows.Forms.Label lbl_data3;
        private System.Windows.Forms.Label lbl_data4;
        private System.Windows.Forms.Label lbl_valueData1;
        private System.Windows.Forms.Label lbl_valueData2;
        private System.Windows.Forms.Label lbl_valueData3;
        private System.Windows.Forms.Label lbl_valueData4;
        private System.Windows.Forms.FolderBrowserDialog fbd_folderBrowser;
        private System.Windows.Forms.Button btn_getCfgParam;
        private System.Windows.Forms.Button btn_updateCfgParam;
        private System.Windows.Forms.Button btn_getWarmUp;
        private System.Windows.Forms.Button btn_updateWarmUp;
        private System.Windows.Forms.Label lbl_AlgorithmStatus;
        private System.Windows.Forms.CheckedListBox clb_algorithmStatus;
        private System.Windows.Forms.Button btn_setSensorId;
        private System.Windows.Forms.NumericUpDown nud_setId;
        private System.Windows.Forms.Label lb_GyroRange;
        private System.Windows.Forms.Label lb_accelRange;
        private System.Windows.Forms.Label lb_magRange;
        private System.Windows.Forms.ComboBox cb_gyroRange;
        private System.Windows.Forms.ComboBox cb_accelRange;
        private System.Windows.Forms.ComboBox cb_magRange;
        private System.Windows.Forms.Button btn_getConfig;
        private System.Windows.Forms.Button btn_SaveConfig;
        private System.Windows.Forms.Button btn_SendConfig;
        private System.Windows.Forms.Label lb_AlgorithmControl;
        private System.Windows.Forms.CheckedListBox clb_algoConfig;
        private System.Windows.Forms.Button btn_sendEepromFile;
        private System.Windows.Forms.Button btn_getEepromFile;
        private System.Windows.Forms.OpenFileDialog ofd_openFile;
        private System.Windows.Forms.Button btn_cancelTransfer;
        private System.Windows.Forms.Timer tmr_transferTimer;
        private System.Windows.Forms.CheckedListBox clb_sensorStatus;
        private System.Windows.Forms.Label lb_SensorStatus;
    }
}

