namespace BrainPackDataAnalyzer
{
    partial class BrainpackControllerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrainpackControllerForm));
            this.tb_Console = new System.Windows.Forms.TextBox();
            this.btn_getStatus = new System.Windows.Forms.Button();
            this.btn_disconnectSock = new System.Windows.Forms.Button();
            this.mtb_NetAddress = new System.Windows.Forms.MaskedTextBox();
            this.mtb_netPort = new System.Windows.Forms.MaskedTextBox();
            this.btn_connectSocket = new System.Windows.Forms.Button();
            this.mtb_streamPort = new System.Windows.Forms.MaskedTextBox();
            this.mtb_streamAddress = new System.Windows.Forms.MaskedTextBox();
            this.tb_recordingName = new System.Windows.Forms.TextBox();
            this.nud_dataInterval = new System.Windows.Forms.NumericUpDown();
            this.lb_streamPort = new System.Windows.Forms.Label();
            this.lb_streamAddress = new System.Windows.Forms.Label();
            this.lb_recordingFilename = new System.Windows.Forms.Label();
            this.lb_DataRate = new System.Windows.Forms.Label();
            this.btn_StreamReq = new System.Windows.Forms.Button();
            this.btn_cfgRecord = new System.Windows.Forms.Button();
            this.btn_stopStream = new System.Windows.Forms.Button();
            this.pb_bodyPicture = new System.Windows.Forms.PictureBox();
            this.cb_sensor0 = new System.Windows.Forms.CheckBox();
            this.cb_sensor1 = new System.Windows.Forms.CheckBox();
            this.cb_sensor2 = new System.Windows.Forms.CheckBox();
            this.cb_sensor4 = new System.Windows.Forms.CheckBox();
            this.cb_sensor3 = new System.Windows.Forms.CheckBox();
            this.cb_sensor5 = new System.Windows.Forms.CheckBox();
            this.cb_sensor6 = new System.Windows.Forms.CheckBox();
            this.cb_sensor7 = new System.Windows.Forms.CheckBox();
            this.cb_sensor8 = new System.Windows.Forms.CheckBox();
            this.tb_firmwareFilename = new System.Windows.Forms.TextBox();
            this.mtb_transferIp = new System.Windows.Forms.MaskedTextBox();
            this.mtb_transferPort = new System.Windows.Forms.MaskedTextBox();
            this.lb_transferPort = new System.Windows.Forms.Label();
            this.lb_transferIp = new System.Windows.Forms.Label();
            this.lb_firmwareFile = new System.Windows.Forms.Label();
            this.btn_FwUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nud_dataInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_bodyPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_Console
            // 
            this.tb_Console.Location = new System.Drawing.Point(12, 12);
            this.tb_Console.Multiline = true;
            this.tb_Console.Name = "tb_Console";
            this.tb_Console.ReadOnly = true;
            this.tb_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Console.Size = new System.Drawing.Size(596, 218);
            this.tb_Console.TabIndex = 8;
            // 
            // btn_getStatus
            // 
            this.btn_getStatus.Location = new System.Drawing.Point(12, 330);
            this.btn_getStatus.Name = "btn_getStatus";
            this.btn_getStatus.Size = new System.Drawing.Size(106, 23);
            this.btn_getStatus.TabIndex = 9;
            this.btn_getStatus.Text = "Get Status";
            this.btn_getStatus.UseVisualStyleBackColor = true;
            this.btn_getStatus.Click += new System.EventHandler(this.btn_getStatus_Click);
            // 
            // btn_disconnectSock
            // 
            this.btn_disconnectSock.Location = new System.Drawing.Point(132, 280);
            this.btn_disconnectSock.Name = "btn_disconnectSock";
            this.btn_disconnectSock.Size = new System.Drawing.Size(105, 23);
            this.btn_disconnectSock.TabIndex = 51;
            this.btn_disconnectSock.Text = "Disconnect Sock";
            this.btn_disconnectSock.UseVisualStyleBackColor = true;
            this.btn_disconnectSock.Click += new System.EventHandler(this.btn_disconnectSock_Click);
            // 
            // mtb_NetAddress
            // 
            this.mtb_NetAddress.Location = new System.Drawing.Point(15, 255);
            this.mtb_NetAddress.Name = "mtb_NetAddress";
            this.mtb_NetAddress.Size = new System.Drawing.Size(100, 20);
            this.mtb_NetAddress.TabIndex = 48;
            this.mtb_NetAddress.Text = "192.168.2.1";
            // 
            // mtb_netPort
            // 
            this.mtb_netPort.Location = new System.Drawing.Point(15, 284);
            this.mtb_netPort.Name = "mtb_netPort";
            this.mtb_netPort.Size = new System.Drawing.Size(100, 20);
            this.mtb_netPort.TabIndex = 49;
            this.mtb_netPort.Text = "6665";
            // 
            // btn_connectSocket
            // 
            this.btn_connectSocket.Location = new System.Drawing.Point(132, 253);
            this.btn_connectSocket.Name = "btn_connectSocket";
            this.btn_connectSocket.Size = new System.Drawing.Size(105, 23);
            this.btn_connectSocket.TabIndex = 50;
            this.btn_connectSocket.Text = "Connect Socket";
            this.btn_connectSocket.UseVisualStyleBackColor = true;
            this.btn_connectSocket.Click += new System.EventHandler(this.btn_connectSocket_Click);
            // 
            // mtb_streamPort
            // 
            this.mtb_streamPort.Location = new System.Drawing.Point(95, 381);
            this.mtb_streamPort.Name = "mtb_streamPort";
            this.mtb_streamPort.Size = new System.Drawing.Size(100, 20);
            this.mtb_streamPort.TabIndex = 52;
            this.mtb_streamPort.Text = "6666";
            // 
            // mtb_streamAddress
            // 
            this.mtb_streamAddress.Location = new System.Drawing.Point(95, 407);
            this.mtb_streamAddress.Name = "mtb_streamAddress";
            this.mtb_streamAddress.Size = new System.Drawing.Size(100, 20);
            this.mtb_streamAddress.TabIndex = 53;
            this.mtb_streamAddress.Text = "192.168.2.1";
            // 
            // tb_recordingName
            // 
            this.tb_recordingName.Location = new System.Drawing.Point(95, 434);
            this.tb_recordingName.Name = "tb_recordingName";
            this.tb_recordingName.Size = new System.Drawing.Size(142, 20);
            this.tb_recordingName.TabIndex = 54;
            this.tb_recordingName.Text = "DataLog";
            // 
            // nud_dataInterval
            // 
            this.nud_dataInterval.Location = new System.Drawing.Point(95, 461);
            this.nud_dataInterval.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nud_dataInterval.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nud_dataInterval.Name = "nud_dataInterval";
            this.nud_dataInterval.Size = new System.Drawing.Size(70, 20);
            this.nud_dataInterval.TabIndex = 55;
            this.nud_dataInterval.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lb_streamPort
            // 
            this.lb_streamPort.AutoSize = true;
            this.lb_streamPort.Location = new System.Drawing.Point(27, 384);
            this.lb_streamPort.Name = "lb_streamPort";
            this.lb_streamPort.Size = new System.Drawing.Size(62, 13);
            this.lb_streamPort.TabIndex = 56;
            this.lb_streamPort.Text = "Stream Port";
            // 
            // lb_streamAddress
            // 
            this.lb_streamAddress.AutoSize = true;
            this.lb_streamAddress.Location = new System.Drawing.Point(8, 410);
            this.lb_streamAddress.Name = "lb_streamAddress";
            this.lb_streamAddress.Size = new System.Drawing.Size(81, 13);
            this.lb_streamAddress.TabIndex = 57;
            this.lb_streamAddress.Text = "Stream Address";
            // 
            // lb_recordingFilename
            // 
            this.lb_recordingFilename.AutoSize = true;
            this.lb_recordingFilename.Location = new System.Drawing.Point(12, 437);
            this.lb_recordingFilename.Name = "lb_recordingFilename";
            this.lb_recordingFilename.Size = new System.Drawing.Size(78, 13);
            this.lb_recordingFilename.TabIndex = 58;
            this.lb_recordingFilename.Text = "Filename Prefix";
            this.lb_recordingFilename.Click += new System.EventHandler(this.label2_Click);
            // 
            // lb_DataRate
            // 
            this.lb_DataRate.AutoSize = true;
            this.lb_DataRate.Location = new System.Drawing.Point(2, 463);
            this.lb_DataRate.Name = "lb_DataRate";
            this.lb_DataRate.Size = new System.Drawing.Size(87, 13);
            this.lb_DataRate.TabIndex = 59;
            this.lb_DataRate.Text = "Data Interval(ms)";
            // 
            // btn_StreamReq
            // 
            this.btn_StreamReq.Location = new System.Drawing.Point(243, 379);
            this.btn_StreamReq.Name = "btn_StreamReq";
            this.btn_StreamReq.Size = new System.Drawing.Size(87, 23);
            this.btn_StreamReq.TabIndex = 60;
            this.btn_StreamReq.Text = "Start Stream";
            this.btn_StreamReq.UseVisualStyleBackColor = true;
            this.btn_StreamReq.Click += new System.EventHandler(this.btn_StreamReq_Click);
            // 
            // btn_cfgRecord
            // 
            this.btn_cfgRecord.Location = new System.Drawing.Point(243, 437);
            this.btn_cfgRecord.Name = "btn_cfgRecord";
            this.btn_cfgRecord.Size = new System.Drawing.Size(87, 23);
            this.btn_cfgRecord.TabIndex = 61;
            this.btn_cfgRecord.Text = "Config Record";
            this.btn_cfgRecord.UseVisualStyleBackColor = true;
            this.btn_cfgRecord.Click += new System.EventHandler(this.btn_cfgRecord_Click);
            // 
            // btn_stopStream
            // 
            this.btn_stopStream.Location = new System.Drawing.Point(243, 408);
            this.btn_stopStream.Name = "btn_stopStream";
            this.btn_stopStream.Size = new System.Drawing.Size(87, 23);
            this.btn_stopStream.TabIndex = 62;
            this.btn_stopStream.Text = "Stop Stream";
            this.btn_stopStream.UseVisualStyleBackColor = true;
            this.btn_stopStream.Click += new System.EventHandler(this.btn_stopStream_Click);
            // 
            // pb_bodyPicture
            // 
            this.pb_bodyPicture.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pb_bodyPicture.BackgroundImage")));
            this.pb_bodyPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_bodyPicture.Location = new System.Drawing.Point(650, 12);
            this.pb_bodyPicture.Name = "pb_bodyPicture";
            this.pb_bodyPicture.Size = new System.Drawing.Size(202, 340);
            this.pb_bodyPicture.TabIndex = 63;
            this.pb_bodyPicture.TabStop = false;
            // 
            // cb_sensor0
            // 
            this.cb_sensor0.AutoSize = true;
            this.cb_sensor0.Location = new System.Drawing.Point(745, 86);
            this.cb_sensor0.Name = "cb_sensor0";
            this.cb_sensor0.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor0.TabIndex = 73;
            this.cb_sensor0.UseVisualStyleBackColor = true;
            // 
            // cb_sensor1
            // 
            this.cb_sensor1.AutoSize = true;
            this.cb_sensor1.Location = new System.Drawing.Point(704, 103);
            this.cb_sensor1.Name = "cb_sensor1";
            this.cb_sensor1.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor1.TabIndex = 74;
            this.cb_sensor1.UseVisualStyleBackColor = true;
            // 
            // cb_sensor2
            // 
            this.cb_sensor2.AutoSize = true;
            this.cb_sensor2.Location = new System.Drawing.Point(683, 150);
            this.cb_sensor2.Name = "cb_sensor2";
            this.cb_sensor2.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor2.TabIndex = 75;
            this.cb_sensor2.UseVisualStyleBackColor = true;
            // 
            // cb_sensor4
            // 
            this.cb_sensor4.AutoSize = true;
            this.cb_sensor4.Location = new System.Drawing.Point(801, 150);
            this.cb_sensor4.Name = "cb_sensor4";
            this.cb_sensor4.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor4.TabIndex = 76;
            this.cb_sensor4.UseVisualStyleBackColor = true;
            // 
            // cb_sensor3
            // 
            this.cb_sensor3.AutoSize = true;
            this.cb_sensor3.Location = new System.Drawing.Point(788, 103);
            this.cb_sensor3.Name = "cb_sensor3";
            this.cb_sensor3.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor3.TabIndex = 77;
            this.cb_sensor3.UseVisualStyleBackColor = true;
            // 
            // cb_sensor5
            // 
            this.cb_sensor5.AutoSize = true;
            this.cb_sensor5.Location = new System.Drawing.Point(723, 216);
            this.cb_sensor5.Name = "cb_sensor5";
            this.cb_sensor5.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor5.TabIndex = 78;
            this.cb_sensor5.UseVisualStyleBackColor = true;
            // 
            // cb_sensor6
            // 
            this.cb_sensor6.AutoSize = true;
            this.cb_sensor6.Location = new System.Drawing.Point(720, 280);
            this.cb_sensor6.Name = "cb_sensor6";
            this.cb_sensor6.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor6.TabIndex = 79;
            this.cb_sensor6.UseVisualStyleBackColor = true;
            // 
            // cb_sensor7
            // 
            this.cb_sensor7.AutoSize = true;
            this.cb_sensor7.Location = new System.Drawing.Point(769, 216);
            this.cb_sensor7.Name = "cb_sensor7";
            this.cb_sensor7.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor7.TabIndex = 80;
            this.cb_sensor7.UseVisualStyleBackColor = true;
            // 
            // cb_sensor8
            // 
            this.cb_sensor8.AutoSize = true;
            this.cb_sensor8.Location = new System.Drawing.Point(775, 280);
            this.cb_sensor8.Name = "cb_sensor8";
            this.cb_sensor8.Size = new System.Drawing.Size(15, 14);
            this.cb_sensor8.TabIndex = 81;
            this.cb_sensor8.UseVisualStyleBackColor = true;
            // 
            // tb_firmwareFilename
            // 
            this.tb_firmwareFilename.Location = new System.Drawing.Point(466, 440);
            this.tb_firmwareFilename.Name = "tb_firmwareFilename";
            this.tb_firmwareFilename.Size = new System.Drawing.Size(142, 20);
            this.tb_firmwareFilename.TabIndex = 84;
            this.tb_firmwareFilename.Text = "firmware.bin";
            // 
            // mtb_transferIp
            // 
            this.mtb_transferIp.Location = new System.Drawing.Point(466, 413);
            this.mtb_transferIp.Name = "mtb_transferIp";
            this.mtb_transferIp.Size = new System.Drawing.Size(100, 20);
            this.mtb_transferIp.TabIndex = 83;
            this.mtb_transferIp.Text = "192.168.2.1";
            // 
            // mtb_transferPort
            // 
            this.mtb_transferPort.Location = new System.Drawing.Point(466, 387);
            this.mtb_transferPort.Name = "mtb_transferPort";
            this.mtb_transferPort.Size = new System.Drawing.Size(100, 20);
            this.mtb_transferPort.TabIndex = 82;
            this.mtb_transferPort.Text = "69";
            // 
            // lb_transferPort
            // 
            this.lb_transferPort.AutoSize = true;
            this.lb_transferPort.Location = new System.Drawing.Point(387, 393);
            this.lb_transferPort.Name = "lb_transferPort";
            this.lb_transferPort.Size = new System.Drawing.Size(68, 13);
            this.lb_transferPort.TabIndex = 85;
            this.lb_transferPort.Text = "Transfer Port";
            // 
            // lb_transferIp
            // 
            this.lb_transferIp.AutoSize = true;
            this.lb_transferIp.Location = new System.Drawing.Point(373, 418);
            this.lb_transferIp.Name = "lb_transferIp";
            this.lb_transferIp.Size = new System.Drawing.Size(87, 13);
            this.lb_transferIp.TabIndex = 86;
            this.lb_transferIp.Text = "Transfer Address";
            // 
            // lb_firmwareFile
            // 
            this.lb_firmwareFile.AutoSize = true;
            this.lb_firmwareFile.Location = new System.Drawing.Point(390, 446);
            this.lb_firmwareFile.Name = "lb_firmwareFile";
            this.lb_firmwareFile.Size = new System.Drawing.Size(68, 13);
            this.lb_firmwareFile.TabIndex = 87;
            this.lb_firmwareFile.Text = "Firmware File";
            // 
            // btn_FwUpdate
            // 
            this.btn_FwUpdate.Location = new System.Drawing.Point(634, 438);
            this.btn_FwUpdate.Name = "btn_FwUpdate";
            this.btn_FwUpdate.Size = new System.Drawing.Size(101, 23);
            this.btn_FwUpdate.TabIndex = 88;
            this.btn_FwUpdate.Text = "Start FW Update";
            this.btn_FwUpdate.UseVisualStyleBackColor = true;
            this.btn_FwUpdate.Click += new System.EventHandler(this.btn_FwUpdate_Click);
            // 
            // BrainpackControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 616);
            this.Controls.Add(this.btn_FwUpdate);
            this.Controls.Add(this.lb_firmwareFile);
            this.Controls.Add(this.lb_transferIp);
            this.Controls.Add(this.lb_transferPort);
            this.Controls.Add(this.tb_firmwareFilename);
            this.Controls.Add(this.mtb_transferIp);
            this.Controls.Add(this.mtb_transferPort);
            this.Controls.Add(this.cb_sensor8);
            this.Controls.Add(this.cb_sensor7);
            this.Controls.Add(this.cb_sensor6);
            this.Controls.Add(this.cb_sensor5);
            this.Controls.Add(this.cb_sensor3);
            this.Controls.Add(this.cb_sensor4);
            this.Controls.Add(this.cb_sensor2);
            this.Controls.Add(this.cb_sensor1);
            this.Controls.Add(this.cb_sensor0);
            this.Controls.Add(this.pb_bodyPicture);
            this.Controls.Add(this.btn_stopStream);
            this.Controls.Add(this.btn_cfgRecord);
            this.Controls.Add(this.btn_StreamReq);
            this.Controls.Add(this.lb_DataRate);
            this.Controls.Add(this.lb_recordingFilename);
            this.Controls.Add(this.lb_streamAddress);
            this.Controls.Add(this.lb_streamPort);
            this.Controls.Add(this.nud_dataInterval);
            this.Controls.Add(this.tb_recordingName);
            this.Controls.Add(this.mtb_streamAddress);
            this.Controls.Add(this.mtb_streamPort);
            this.Controls.Add(this.btn_disconnectSock);
            this.Controls.Add(this.mtb_NetAddress);
            this.Controls.Add(this.mtb_netPort);
            this.Controls.Add(this.btn_connectSocket);
            this.Controls.Add(this.btn_getStatus);
            this.Controls.Add(this.tb_Console);
            this.Name = "BrainpackControllerForm";
            this.Text = "Brainpack Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrainpackControllerForm_FormClosing);
            this.Load += new System.EventHandler(this.BrainpackControllerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_dataInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_bodyPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Console;
        private System.Windows.Forms.Button btn_getStatus;
        private System.Windows.Forms.Button btn_disconnectSock;
        private System.Windows.Forms.MaskedTextBox mtb_NetAddress;
        private System.Windows.Forms.MaskedTextBox mtb_netPort;
        private System.Windows.Forms.Button btn_connectSocket;
        private System.Windows.Forms.MaskedTextBox mtb_streamPort;
        private System.Windows.Forms.MaskedTextBox mtb_streamAddress;
        private System.Windows.Forms.TextBox tb_recordingName;
        private System.Windows.Forms.NumericUpDown nud_dataInterval;
        private System.Windows.Forms.Label lb_streamPort;
        private System.Windows.Forms.Label lb_streamAddress;
        private System.Windows.Forms.Label lb_recordingFilename;
        private System.Windows.Forms.Label lb_DataRate;
        private System.Windows.Forms.Button btn_StreamReq;
        private System.Windows.Forms.Button btn_cfgRecord;
        private System.Windows.Forms.Button btn_stopStream;
        private System.Windows.Forms.PictureBox pb_bodyPicture;
        private System.Windows.Forms.CheckBox cb_sensor0;
        private System.Windows.Forms.CheckBox cb_sensor1;
        private System.Windows.Forms.CheckBox cb_sensor2;
        private System.Windows.Forms.CheckBox cb_sensor4;
        private System.Windows.Forms.CheckBox cb_sensor3;
        private System.Windows.Forms.CheckBox cb_sensor5;
        private System.Windows.Forms.CheckBox cb_sensor6;
        private System.Windows.Forms.CheckBox cb_sensor7;
        private System.Windows.Forms.CheckBox cb_sensor8;
        private System.Windows.Forms.TextBox tb_firmwareFilename;
        private System.Windows.Forms.MaskedTextBox mtb_transferIp;
        private System.Windows.Forms.MaskedTextBox mtb_transferPort;
        private System.Windows.Forms.Label lb_transferPort;
        private System.Windows.Forms.Label lb_transferIp;
        private System.Windows.Forms.Label lb_firmwareFile;
        private System.Windows.Forms.Button btn_FwUpdate;
    }
}