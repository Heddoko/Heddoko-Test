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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btn_disconnect = new System.Windows.Forms.Button();
            this.tb_Console = new System.Windows.Forms.TextBox();
            this.bnt_Connect = new System.Windows.Forms.Button();
            this.cb_serialPorts = new System.Windows.Forms.ComboBox();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.cb_BaudRate = new System.Windows.Forms.ComboBox();
            this.btn_getFrame = new System.Windows.Forms.Button();
            this.btn_SetupMode = new System.Windows.Forms.Button();
            this.cb_SetupModeEn = new System.Windows.Forms.CheckBox();
            this.btn_sendUpdateCmd = new System.Windows.Forms.Button();
            this.cb_enableStream = new System.Windows.Forms.CheckBox();
            this.chrt_dataChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tb_y_max = new System.Windows.Forms.TextBox();
            this.tb_y_min = new System.Windows.Forms.TextBox();
            this.lb_y_max = new System.Windows.Forms.Label();
            this.lb_y_min = new System.Windows.Forms.Label();
            this.btn_setAxis = new System.Windows.Forms.Button();
            this.btn_getStatus = new System.Windows.Forms.Button();
            this.btn_clearScreen = new System.Windows.Forms.Button();
            this.nud_SelectedImu = new System.Windows.Forms.NumericUpDown();
            this.cb_forwardPorts = new System.Windows.Forms.ComboBox();
            this.forwardSerialPort = new System.IO.Ports.SerialPort(this.components);
            this.cb_logErrors = new System.Windows.Forms.CheckBox();
            this.mtb_NetAddress = new System.Windows.Forms.MaskedTextBox();
            this.mtb_netPort = new System.Windows.Forms.MaskedTextBox();
            this.btn_connectSocket = new System.Windows.Forms.Button();
            this.btn_disconnectSock = new System.Windows.Forms.Button();
            this.cb_udpSelected = new System.Windows.Forms.CheckBox();
            this.btn_startStream = new System.Windows.Forms.Button();
            this.btn_StopStreaming = new System.Windows.Forms.Button();
            this.sfd_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_forwardPort = new System.Windows.Forms.Label();
            this.cb_OutputFormat = new System.Windows.Forms.ComboBox();
            this.cb_EnableFowardPort = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cb_dataType = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btn_sendCommand = new System.Windows.Forms.Button();
            this.tb_nodAddress = new System.Windows.Forms.TextBox();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.btn_Record = new System.Windows.Forms.Button();
            this.cb_EnableNodStream = new System.Windows.Forms.CheckBox();
            this.nud_magRate = new System.Windows.Forms.NumericUpDown();
            this.nud_accelRate = new System.Windows.Forms.NumericUpDown();
            this.nud_gyroRate = new System.Windows.Forms.NumericUpDown();
            this.btn_setRate = new System.Windows.Forms.Button();
            this.lb_rates = new System.Windows.Forms.Label();
            this.cb_ypr = new System.Windows.Forms.CheckBox();
            this.cb_fpBaudRate = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chrt_dataChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SelectedImu)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_magRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_accelRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_gyroRate)).BeginInit();
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
            // chrt_dataChart
            // 
            chartArea2.AxisY.Maximum = 1.1D;
            chartArea2.AxisY.Minimum = -1.1D;
            chartArea2.Name = "ChartArea1";
            this.chrt_dataChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chrt_dataChart.Legends.Add(legend2);
            this.chrt_dataChart.Location = new System.Drawing.Point(541, 30);
            this.chrt_dataChart.Name = "chrt_dataChart";
            this.chrt_dataChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series5.BorderWidth = 5;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.MarkerSize = 1;
            series5.Name = "Qx";
            series5.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series6.BorderWidth = 5;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.MarkerSize = 1;
            series6.Name = "Qy";
            series7.BorderWidth = 5;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Legend = "Legend1";
            series7.MarkerSize = 1;
            series7.Name = "Qz";
            series8.BorderWidth = 5;
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Legend = "Legend1";
            series8.MarkerSize = 1;
            series8.Name = "Qw";
            this.chrt_dataChart.Series.Add(series5);
            this.chrt_dataChart.Series.Add(series6);
            this.chrt_dataChart.Series.Add(series7);
            this.chrt_dataChart.Series.Add(series8);
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
            this.nud_SelectedImu.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cb_forwardPorts
            // 
            this.cb_forwardPorts.FormattingEnabled = true;
            this.cb_forwardPorts.Location = new System.Drawing.Point(252, 32);
            this.cb_forwardPorts.Name = "cb_forwardPorts";
            this.cb_forwardPorts.Size = new System.Drawing.Size(156, 21);
            this.cb_forwardPorts.TabIndex = 37;
            // 
            // forwardSerialPort
            // 
            this.forwardSerialPort.BaudRate = 115200;
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
            // sfd_saveFileDialog
            // 
            this.sfd_saveFileDialog.Filter = "Comma Separated Files (*.csv)|*.csv|All Files (*.*)|*.*";
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
            this.tabControl1.Size = new System.Drawing.Size(476, 196);
            this.tabControl1.TabIndex = 46;
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
            this.tabPage1.Size = new System.Drawing.Size(468, 170);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Record 485 Frames";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.cb_OutputFormat.Location = new System.Drawing.Point(125, 118);
            this.cb_OutputFormat.Name = "cb_OutputFormat";
            this.cb_OutputFormat.Size = new System.Drawing.Size(112, 21);
            this.cb_OutputFormat.TabIndex = 47;
            // 
            // cb_EnableFowardPort
            // 
            this.cb_EnableFowardPort.AutoSize = true;
            this.cb_EnableFowardPort.Location = new System.Drawing.Point(252, 86);
            this.cb_EnableFowardPort.Name = "cb_EnableFowardPort";
            this.cb_EnableFowardPort.Size = new System.Drawing.Size(100, 17);
            this.cb_EnableFowardPort.TabIndex = 46;
            this.cb_EnableFowardPort.Text = "Forward Stream";
            this.cb_EnableFowardPort.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
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
            this.tabPage2.Size = new System.Drawing.Size(468, 170);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Debug 485 Sensors";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            this.tabPage3.Size = new System.Drawing.Size(468, 170);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "TCP IP Debug";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.comboBox1);
            this.tabPage4.Controls.Add(this.btn_sendCommand);
            this.tabPage4.Controls.Add(this.tb_nodAddress);
            this.tabPage4.Controls.Add(this.btn_Reset);
            this.tabPage4.Controls.Add(this.btn_Record);
            this.tabPage4.Controls.Add(this.cb_EnableNodStream);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(468, 170);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Nod Analyzer";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(37, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(125, 21);
            this.comboBox1.TabIndex = 7;
            // 
            // btn_sendCommand
            // 
            this.btn_sendCommand.Location = new System.Drawing.Point(193, 23);
            this.btn_sendCommand.Name = "btn_sendCommand";
            this.btn_sendCommand.Size = new System.Drawing.Size(97, 23);
            this.btn_sendCommand.TabIndex = 6;
            this.btn_sendCommand.Text = "Send Command";
            this.btn_sendCommand.UseVisualStyleBackColor = true;
            // 
            // tb_nodAddress
            // 
            this.tb_nodAddress.Location = new System.Drawing.Point(37, 67);
            this.tb_nodAddress.Name = "tb_nodAddress";
            this.tb_nodAddress.Size = new System.Drawing.Size(125, 20);
            this.tb_nodAddress.TabIndex = 5;
            this.tb_nodAddress.Text = "A0E5E90013B6";
            // 
            // btn_Reset
            // 
            this.btn_Reset.Location = new System.Drawing.Point(193, 97);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(75, 23);
            this.btn_Reset.TabIndex = 3;
            this.btn_Reset.Text = "Reset";
            this.btn_Reset.UseVisualStyleBackColor = true;
            // 
            // btn_Record
            // 
            this.btn_Record.Location = new System.Drawing.Point(193, 67);
            this.btn_Record.Name = "btn_Record";
            this.btn_Record.Size = new System.Drawing.Size(75, 23);
            this.btn_Record.TabIndex = 2;
            this.btn_Record.Text = "Record";
            this.btn_Record.UseVisualStyleBackColor = true;
            // 
            // cb_EnableNodStream
            // 
            this.cb_EnableNodStream.AutoSize = true;
            this.cb_EnableNodStream.Location = new System.Drawing.Point(308, 27);
            this.cb_EnableNodStream.Name = "cb_EnableNodStream";
            this.cb_EnableNodStream.Size = new System.Drawing.Size(95, 17);
            this.cb_EnableNodStream.TabIndex = 1;
            this.cb_EnableNodStream.Text = "Enable Stream";
            this.cb_EnableNodStream.UseVisualStyleBackColor = true;
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
            // lb_rates
            // 
            this.lb_rates.AutoSize = true;
            this.lb_rates.Location = new System.Drawing.Point(272, 78);
            this.lb_rates.Name = "lb_rates";
            this.lb_rates.Size = new System.Drawing.Size(176, 13);
            this.lb_rates.TabIndex = 44;
            this.lb_rates.Text = "Mag Rate    Accel Rate   Gyro Rate";
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
            // cb_fpBaudRate
            // 
            this.cb_fpBaudRate.FormattingEnabled = true;
            this.cb_fpBaudRate.Location = new System.Drawing.Point(252, 59);
            this.cb_fpBaudRate.Name = "cb_fpBaudRate";
            this.cb_fpBaudRate.Size = new System.Drawing.Size(156, 21);
            this.cb_fpBaudRate.TabIndex = 50;
            this.cb_fpBaudRate.SelectedIndexChanged += new System.EventHandler(this.cb_fpBaudRate_SelectedIndexChanged);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 564);
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
            ((System.ComponentModel.ISupportInitialize)(this.nud_SelectedImu)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_magRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_accelRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_gyroRate)).EndInit();
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
        private System.Windows.Forms.Button btn_getFrame;
        private System.Windows.Forms.Button btn_SetupMode;
        private System.Windows.Forms.CheckBox cb_SetupModeEn;
        private System.Windows.Forms.Button btn_sendUpdateCmd;
        private System.Windows.Forms.CheckBox cb_enableStream;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrt_dataChart;
        private System.Windows.Forms.TextBox tb_y_max;
        private System.Windows.Forms.TextBox tb_y_min;
        private System.Windows.Forms.Label lb_y_max;
        private System.Windows.Forms.Label lb_y_min;
        private System.Windows.Forms.Button btn_setAxis;
        private System.Windows.Forms.Button btn_getStatus;
        private System.Windows.Forms.Button btn_clearScreen;
        private System.Windows.Forms.NumericUpDown nud_SelectedImu;
        private System.Windows.Forms.ComboBox cb_forwardPorts;
        private System.IO.Ports.SerialPort forwardSerialPort;
        private System.Windows.Forms.CheckBox cb_logErrors;
        private System.Windows.Forms.MaskedTextBox mtb_NetAddress;
        private System.Windows.Forms.MaskedTextBox mtb_netPort;
        private System.Windows.Forms.Button btn_connectSocket;
        private System.Windows.Forms.Button btn_disconnectSock;
        private System.Windows.Forms.CheckBox cb_udpSelected;
        private System.Windows.Forms.Button btn_startStream;
        private System.Windows.Forms.Button btn_StopStreaming;
        private System.Windows.Forms.SaveFileDialog sfd_saveFileDialog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_forwardPort;
        private System.Windows.Forms.ComboBox cb_OutputFormat;
        private System.Windows.Forms.CheckBox cb_EnableFowardPort;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox cb_dataType;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.Button btn_Record;
        private System.Windows.Forms.CheckBox cb_EnableNodStream;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btn_sendCommand;
        private System.Windows.Forms.TextBox tb_nodAddress;
        private System.Windows.Forms.CheckBox cb_ypr;
        private System.Windows.Forms.Label lb_rates;
        private System.Windows.Forms.Button btn_setRate;
        private System.Windows.Forms.NumericUpDown nud_gyroRate;
        private System.Windows.Forms.NumericUpDown nud_accelRate;
        private System.Windows.Forms.NumericUpDown nud_magRate;
        private System.Windows.Forms.ComboBox cb_fpBaudRate;
    }
}

