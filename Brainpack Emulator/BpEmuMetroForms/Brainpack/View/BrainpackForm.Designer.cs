namespace BpEmuMetroForms.Brainpack
{
    partial class BrainpackForm
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.BrainpackSerial = new MetroFramework.Controls.MetroTextBox();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.ComboBoxMajorVersion = new MetroFramework.Controls.MetroComboBox();
            this.ComboBoxMinorVersion = new MetroFramework.Controls.MetroComboBox();
            this.ComboBoxBuild = new MetroFramework.Controls.MetroComboBox();
            this.ComboBoxRevision = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.ConfigurationPortInputField = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.SelectFolderButton = new MetroFramework.Controls.MetroButton();
            this.SaveButton = new MetroFramework.Controls.MetroButton();
            this.ConfigPortErrorLabel = new MetroFramework.Controls.MetroLabel();
            this.SerialNumberErrorLabel = new MetroFramework.Controls.MetroLabel();
            this.FirmwareVersionLabel = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(24, 82);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(102, 19);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "Brainpack Serial";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(23, 113);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(110, 19);
            this.metroLabel2.TabIndex = 2;
            this.metroLabel2.Text = "Firmware Version";
            // 
            // BrainpackSerial
            // 
            // 
            // 
            // 
            this.BrainpackSerial.CustomButton.Image = null;
            this.BrainpackSerial.CustomButton.Location = new System.Drawing.Point(75, 1);
            this.BrainpackSerial.CustomButton.Name = "";
            this.BrainpackSerial.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.BrainpackSerial.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.BrainpackSerial.CustomButton.TabIndex = 1;
            this.BrainpackSerial.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.BrainpackSerial.CustomButton.UseSelectable = true;
            this.BrainpackSerial.CustomButton.Visible = false;
            this.BrainpackSerial.Lines = new string[0];
            this.BrainpackSerial.Location = new System.Drawing.Point(148, 82);
            this.BrainpackSerial.MaxLength = 32767;
            this.BrainpackSerial.Name = "BrainpackSerial";
            this.BrainpackSerial.PasswordChar = '\0';
            this.BrainpackSerial.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.BrainpackSerial.SelectedText = "";
            this.BrainpackSerial.SelectionLength = 0;
            this.BrainpackSerial.SelectionStart = 0;
            this.BrainpackSerial.ShortcutsEnabled = true;
            this.BrainpackSerial.Size = new System.Drawing.Size(97, 23);
            this.BrainpackSerial.TabIndex = 0;
            this.BrainpackSerial.UseSelectable = true;
            this.BrainpackSerial.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.BrainpackSerial.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.BrainpackSerial.Click += new System.EventHandler(this.BrainpackSerial_Click);
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Tag = "Tool tiperino";
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // ComboBoxMajorVersion
            // 
            this.ComboBoxMajorVersion.ItemHeight = 23;
            this.ComboBoxMajorVersion.Items.AddRange(new object[] {
            "0",
            "1",
            "2"});
            this.ComboBoxMajorVersion.Location = new System.Drawing.Point(148, 110);
            this.ComboBoxMajorVersion.Name = "ComboBoxMajorVersion";
            this.ComboBoxMajorVersion.Size = new System.Drawing.Size(41, 29);
            this.ComboBoxMajorVersion.TabIndex = 3;
            this.ComboBoxMajorVersion.UseSelectable = true;
            this.ComboBoxMajorVersion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMajorVersion_SelectedIndexChanged);
            // 
            // ComboBoxMinorVersion
            // 
            this.ComboBoxMinorVersion.ItemHeight = 23;
            this.ComboBoxMinorVersion.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.ComboBoxMinorVersion.Location = new System.Drawing.Point(195, 110);
            this.ComboBoxMinorVersion.Name = "ComboBoxMinorVersion";
            this.ComboBoxMinorVersion.Size = new System.Drawing.Size(50, 29);
            this.ComboBoxMinorVersion.TabIndex = 4;
            this.ComboBoxMinorVersion.UseSelectable = true;
            this.ComboBoxMinorVersion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMinorVersion_SelectedIndexChanged);
            // 
            // ComboBoxBuild
            // 
            this.ComboBoxBuild.ItemHeight = 23;
            this.ComboBoxBuild.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.ComboBoxBuild.Location = new System.Drawing.Point(251, 110);
            this.ComboBoxBuild.Name = "ComboBoxBuild";
            this.ComboBoxBuild.Size = new System.Drawing.Size(50, 29);
            this.ComboBoxBuild.TabIndex = 5;
            this.ComboBoxBuild.UseSelectable = true;
            this.ComboBoxBuild.SelectedIndexChanged += new System.EventHandler(this.ComboBoxBuild_SelectedIndexChanged);
            // 
            // ComboBoxRevision
            // 
            this.ComboBoxRevision.ItemHeight = 23;
            this.ComboBoxRevision.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.ComboBoxRevision.Location = new System.Drawing.Point(307, 110);
            this.ComboBoxRevision.Name = "ComboBoxRevision";
            this.ComboBoxRevision.Size = new System.Drawing.Size(50, 29);
            this.ComboBoxRevision.TabIndex = 6;
            this.ComboBoxRevision.UseSelectable = true;
            this.ComboBoxRevision.SelectedIndexChanged += new System.EventHandler(this.ComboBoxRevision_SelectedIndexChanged);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(23, 154);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(118, 19);
            this.metroLabel3.TabIndex = 7;
            this.metroLabel3.Text = "Configuration Port";
            // 
            // ConfigurationPortInputField
            // 
            // 
            // 
            // 
            this.ConfigurationPortInputField.CustomButton.Image = null;
            this.ConfigurationPortInputField.CustomButton.Location = new System.Drawing.Point(75, 1);
            this.ConfigurationPortInputField.CustomButton.Name = "";
            this.ConfigurationPortInputField.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.ConfigurationPortInputField.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.ConfigurationPortInputField.CustomButton.TabIndex = 1;
            this.ConfigurationPortInputField.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ConfigurationPortInputField.CustomButton.UseSelectable = true;
            this.ConfigurationPortInputField.CustomButton.Visible = false;
            this.ConfigurationPortInputField.Lines = new string[0];
            this.ConfigurationPortInputField.Location = new System.Drawing.Point(148, 150);
            this.ConfigurationPortInputField.MaxLength = 32767;
            this.ConfigurationPortInputField.Name = "ConfigurationPortInputField";
            this.ConfigurationPortInputField.PasswordChar = '\0';
            this.ConfigurationPortInputField.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ConfigurationPortInputField.SelectedText = "";
            this.ConfigurationPortInputField.SelectionLength = 0;
            this.ConfigurationPortInputField.SelectionStart = 0;
            this.ConfigurationPortInputField.ShortcutsEnabled = true;
            this.ConfigurationPortInputField.Size = new System.Drawing.Size(97, 23);
            this.ConfigurationPortInputField.TabIndex = 8;
            this.ConfigurationPortInputField.UseSelectable = true;
            this.ConfigurationPortInputField.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.ConfigurationPortInputField.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.ConfigurationPortInputField.Click += new System.EventHandler(this.ConfigurationPortInputField_Click);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(24, 198);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(117, 19);
            this.metroLabel4.TabIndex = 9;
            this.metroLabel4.Text = "Firmware Location";
            // 
            // SelectFolderButton
            // 
            this.SelectFolderButton.Location = new System.Drawing.Point(148, 193);
            this.SelectFolderButton.Name = "SelectFolderButton";
            this.SelectFolderButton.Size = new System.Drawing.Size(111, 23);
            this.SelectFolderButton.TabIndex = 10;
            this.SelectFolderButton.Text = "Select Folder";
            this.SelectFolderButton.UseSelectable = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(320, 273);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(111, 23);
            this.SaveButton.TabIndex = 11;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseSelectable = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ConfigPortErrorLabel
            // 
            this.ConfigPortErrorLabel.AutoSize = true;
            this.ConfigPortErrorLabel.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.ConfigPortErrorLabel.Location = new System.Drawing.Point(251, 154);
            this.ConfigPortErrorLabel.Name = "ConfigPortErrorLabel";
            this.ConfigPortErrorLabel.Size = new System.Drawing.Size(0, 0);
            this.ConfigPortErrorLabel.Style = MetroFramework.MetroColorStyle.Red;
            this.ConfigPortErrorLabel.TabIndex = 12;
            this.ConfigPortErrorLabel.UseStyleColors = true;
            // 
            // SerialNumberErrorLabel
            // 
            this.SerialNumberErrorLabel.AutoSize = true;
            this.SerialNumberErrorLabel.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.SerialNumberErrorLabel.Location = new System.Drawing.Point(251, 88);
            this.SerialNumberErrorLabel.Name = "SerialNumberErrorLabel";
            this.SerialNumberErrorLabel.Size = new System.Drawing.Size(0, 0);
            this.SerialNumberErrorLabel.Style = MetroFramework.MetroColorStyle.Red;
            this.SerialNumberErrorLabel.TabIndex = 13;
            this.SerialNumberErrorLabel.UseStyleColors = true;
            // 
            // FirmwareVersionLabel
            // 
            this.FirmwareVersionLabel.AutoSize = true;
            this.FirmwareVersionLabel.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.FirmwareVersionLabel.Location = new System.Drawing.Point(378, 120);
            this.FirmwareVersionLabel.Name = "FirmwareVersionLabel";
            this.FirmwareVersionLabel.Size = new System.Drawing.Size(0, 0);
            this.FirmwareVersionLabel.Style = MetroFramework.MetroColorStyle.Red;
            this.FirmwareVersionLabel.TabIndex = 14;
            this.FirmwareVersionLabel.UseStyleColors = true;
            // 
            // BrainpackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 319);
            this.Controls.Add(this.FirmwareVersionLabel);
            this.Controls.Add(this.SerialNumberErrorLabel);
            this.Controls.Add(this.ConfigPortErrorLabel);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SelectFolderButton);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.ConfigurationPortInputField);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.ComboBoxRevision);
            this.Controls.Add(this.ComboBoxBuild);
            this.Controls.Add(this.ComboBoxMinorVersion);
            this.Controls.Add(this.ComboBoxMajorVersion);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.BrainpackSerial);
            this.Name = "BrainpackForm";
            this.Text = "New Brainpack";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox BrainpackSerial;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private MetroFramework.Controls.MetroComboBox ComboBoxMajorVersion;
        private MetroFramework.Controls.MetroComboBox ComboBoxMinorVersion;
        private MetroFramework.Controls.MetroComboBox ComboBoxBuild;
        private MetroFramework.Controls.MetroComboBox ComboBoxRevision;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroTextBox ConfigurationPortInputField;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroButton SelectFolderButton;
        private MetroFramework.Controls.MetroButton SaveButton;
        private MetroFramework.Controls.MetroLabel ConfigPortErrorLabel;
        private MetroFramework.Controls.MetroLabel SerialNumberErrorLabel;
        private MetroFramework.Controls.MetroLabel FirmwareVersionLabel;
    }
}