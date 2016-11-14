
using System;
using System.Timers;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Microsoft.Win32;

namespace BpEmuMetroForms.Brainpack
{
    public delegate void SaveBrainpack(BrainpackController vModel);
    delegate void SetTextCallback(MetroLabel vLabel, string vText);

    public partial class BrainpackForm : MetroForm
    {
        public SaveBrainpack SaveBrainpackConfigurationEvent;
        private bool mHasUnsavedChanges;
        private System.Timers.Timer mSystemTimer = null;
        public Func<BrainpackController, bool> BrainpackValidation;
        public Func<int, bool> ConfigurationPortValidation;

        public BrainpackController BrainpackController;
        public BrainpackForm(string vTitle)
        {
            InitializeComponent();
            this.FormClosing += BrainpackFormClosing;
            BrainpackModel vModel = new BrainpackModel();
            BrainpackController   = new BrainpackController(vModel);
        }

        public BrainpackForm(BrainpackController vController, string vTitle)
        {
            InitializeComponent();
            this.Text = vTitle;
            BrainpackController = vController;
            //fill out view
            BrainpackSerial.Text = BrainpackController.Model.SerialNum;
            ComboBoxMinorVersion.Text = BrainpackController.Model.FirmwareVersion.Minor.ToString();
            ComboBoxMajorVersion.Text = BrainpackController.Model.FirmwareVersion.Major.ToString();
            ComboBoxBuild.Text = BrainpackController.Model.FirmwareVersion.Build.ToString();
            ComboBoxRevision.Text = BrainpackController.Model.FirmwareVersion.Revision.ToString();
            ConfigurationPortInputField.Text = BrainpackController.Model.ConfigurationPort.ToString(); 
            this.FormClosing += BrainpackFormClosing;
        }

        void BrainpackFormClosing(object vSender, FormClosingEventArgs vE)
        {
            mHasUnsavedChanges = !IsAllEmpty();
            if (vE.CloseReason == CloseReason.UserClosing && mHasUnsavedChanges)
            {
                var vRes = MetroMessageBox.Show(this, "Are you sure you want to close? You have unsaved changes", "save first no?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (vRes == DialogResult.Yes && mHasUnsavedChanges)
                {
                    this.FormClosing -= BrainpackFormClosing;
                    this.Close();
                }
                else if (vRes == DialogResult.Yes)
                {
                    this.FormClosing -= BrainpackFormClosing;
                    this.Close();
                }
                else
                {
                    vE.Cancel = true;
                }
            }
            if (vE.CloseReason == CloseReason.WindowsShutDown)
            {

            }

        }

        private bool IsAllEmpty()
        {
            bool vVal = false;
            var v1 = string.IsNullOrEmpty(BrainpackSerial.Text);
            var v2 = ComboBoxMajorVersion.SelectedIndex < 0 || ComboBoxMinorVersion.SelectedIndex < 0 ||
                     ComboBoxBuild.SelectedIndex < 0 || ComboBoxRevision.SelectedIndex < 0;
            var v3 = string.IsNullOrEmpty(ConfigurationPortInputField.Text);
            vVal = v1 && v2 && v3;
            return vVal;
        }



        private void BrainpackSerial_Click(object vSender, System.EventArgs vE)
        {

        }

        private void ComboBoxMajorVersion_SelectedIndexChanged(object vSender, System.EventArgs vE)
        {

        }

        private void ComboBoxMinorVersion_SelectedIndexChanged(object vSender, System.EventArgs vE)
        {

        }

        private void ComboBoxBuild_SelectedIndexChanged(object vSender, System.EventArgs vE)
        {

        }

        private void ComboBoxRevision_SelectedIndexChanged(object vSender, System.EventArgs vE)
        {

        }

        private void ConfigurationPortInputField_Click(object vSender, System.EventArgs vE)
        {

        }

        private void SaveButton_Click(object vSender, System.EventArgs vE)
        {
            BrainpackController.Model.SerialNum = BrainpackSerial.Text;
            if (string.IsNullOrEmpty(BrainpackController.Model.SerialNum))
            {
                SerialNumberErrorLabel.Text = "CANNOT BE EMPTY";
                StartClearingLabels();
                return;
            }
            if (BrainpackValidation != null)
            {
                var vBpExists = BrainpackValidation(BrainpackController );
                if (vBpExists)
                {
                    SerialNumberErrorLabel.Text = "BRAINPACK ALREADY EXISTS. CHOOSE ANOTHER";
                    StartClearingLabels();
                    return;
                }
            }

            if (ConfigurationPortValidation != null)
            {
                int vVal = 0;
                int.TryParse(ConfigurationPortInputField.Text, out vVal);
                var vConfPortExist = ConfigurationPortValidation(vVal);
                if (vConfPortExist)
                {
                    ConfigPortErrorLabel.Text = "CONFIGURATION USED FOR ANOTHER BRAINPACK. CHOOSE ANOTHER";
                    StartClearingLabels();
                    return;
                }
            }
            int vConfigurationPort = 0;
            var vValidNum = int.TryParse(ConfigurationPortInputField.Text, out vConfigurationPort);
            bool vValidConfigPort = (vConfigurationPort > 0 && vConfigurationPort <= 65535 && vValidNum);


            if (!vValidConfigPort)
            {
                ConfigPortErrorLabel.Text = "ONLY NUMBERS BETWEEN 1 AND 65535 ARE ACCEPTED";
                StartClearingLabels();
                return;
            }
            BrainpackController.Model.ConfigurationPort = vConfigurationPort;

            var vMajIdx = ComboBoxMajorVersion.SelectedIndex;
            var vMinIdx = ComboBoxMinorVersion.SelectedIndex;
            var vBuildVerIdx = ComboBoxBuild.SelectedIndex;
            var vRevVerIdx = ComboBoxRevision.SelectedIndex;

            if (vMajIdx < 0 || vMinIdx < 0 || vBuildVerIdx < 0 || vRevVerIdx < 0)
            {
                FirmwareVersionLabel.Text = "SELECT ALL VALUES FROM THE DROP DOWN OPTIONS";
                return;
            }

            var vMajVal = ComboBoxMajorVersion.Items[vMajIdx].ToString();
            var vMinVal = ComboBoxMinorVersion.Items[vMinIdx].ToString();
            var vBuildVal = ComboBoxBuild.Items[vBuildVerIdx].ToString();
            var vRevVal = ComboBoxRevision.Items[vRevVerIdx].ToString();
            BrainpackController.Model.FirmwareVersion = new Version(vMajVal + "." + vMinVal + "." + vBuildVal + "." + vRevVal);
            SaveBrainpackConfigurationEvent?.Invoke(BrainpackController);
            this.FormClosing -= BrainpackFormClosing;
            this.Close();
        }


        void StartClearingLabels()
        {
            mSystemTimer = new System.Timers.Timer(5000);
            mSystemTimer.Elapsed += Clear;
            mSystemTimer.Start();
        }

        private void Clear(object vSender, ElapsedEventArgs vE)
        {
            SetText(ConfigPortErrorLabel, "");
            SetText(SerialNumberErrorLabel, "");
        }

        private void SetText(MetroLabel vLabel, string vText)
        {
            try
            {
                if (vLabel.InvokeRequired)
                {
                    SetTextCallback vD = new SetTextCallback(SetText);
                    this.Invoke(vD, vLabel, vText);
                }
                else
                {
                    vLabel.Text = vText;
                }
            }
            catch (Exception ve)
            {
                
              
            }
          
        }


    }
}
