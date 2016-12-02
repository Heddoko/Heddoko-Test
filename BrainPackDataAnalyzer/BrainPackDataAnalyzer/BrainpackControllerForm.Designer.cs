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
            ((System.ComponentModel.ISupportInitialize)(this.nud_dataInterval)).BeginInit();
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
            this.btn_StreamReq.Location = new System.Drawing.Point(281, 384);
            this.btn_StreamReq.Name = "btn_StreamReq";
            this.btn_StreamReq.Size = new System.Drawing.Size(87, 23);
            this.btn_StreamReq.TabIndex = 60;
            this.btn_StreamReq.Text = "Start Stream";
            this.btn_StreamReq.UseVisualStyleBackColor = true;
            this.btn_StreamReq.Click += new System.EventHandler(this.btn_StreamReq_Click);
            // 
            // btn_cfgRecord
            // 
            this.btn_cfgRecord.Location = new System.Drawing.Point(281, 442);
            this.btn_cfgRecord.Name = "btn_cfgRecord";
            this.btn_cfgRecord.Size = new System.Drawing.Size(87, 23);
            this.btn_cfgRecord.TabIndex = 61;
            this.btn_cfgRecord.Text = "Config Record";
            this.btn_cfgRecord.UseVisualStyleBackColor = true;
            this.btn_cfgRecord.Click += new System.EventHandler(this.btn_cfgRecord_Click);
            // 
            // btn_stopStream
            // 
            this.btn_stopStream.Location = new System.Drawing.Point(281, 413);
            this.btn_stopStream.Name = "btn_stopStream";
            this.btn_stopStream.Size = new System.Drawing.Size(87, 23);
            this.btn_stopStream.TabIndex = 62;
            this.btn_stopStream.Text = "Stop Stream";
            this.btn_stopStream.UseVisualStyleBackColor = true;
            this.btn_stopStream.Click += new System.EventHandler(this.btn_stopStream_Click);
            // 
            // BrainpackControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 494);
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
    }
}