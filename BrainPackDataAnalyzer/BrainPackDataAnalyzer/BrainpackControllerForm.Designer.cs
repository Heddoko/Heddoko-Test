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
            // BrainpackControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 494);
            this.Controls.Add(this.btn_disconnectSock);
            this.Controls.Add(this.mtb_NetAddress);
            this.Controls.Add(this.mtb_netPort);
            this.Controls.Add(this.btn_connectSocket);
            this.Controls.Add(this.btn_getStatus);
            this.Controls.Add(this.tb_Console);
            this.Name = "BrainpackControllerForm";
            this.Text = "Brainpack Controller";
            this.Load += new System.EventHandler(this.BrainpackControllerForm_Load);
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
    }
}