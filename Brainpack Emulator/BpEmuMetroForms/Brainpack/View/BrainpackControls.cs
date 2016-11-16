// /**
// * @file BrainpackControls.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 11 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BpEmuMetroForms.Brainpack
{
    public class BrainpackControls : UserControl
    {
        internal FlowLayoutPanel ParentGrid;
        internal MetroFramework.Controls.MetroGrid BrainpackGrid;
        internal DataGridViewTextBoxColumn Column1;
        internal DataGridViewComboBoxColumn Column2;
        internal DataGridViewCheckBoxColumn Column3;
        internal DataGridViewCheckBoxColumn Column4;
        internal MetroFramework.Controls.MetroPanel metroPanel1;
        internal MetroFramework.Controls.MetroButton DeleteButton;
        internal MetroFramework.Controls.MetroButton SettingsButton;
        internal MetroFramework.Controls.MetroButton BrainpackConnectionStart;
        internal MetroFramework.Controls.MetroButton SelectRecordingFileButton;
        internal MetroFramework.Controls.MetroButton StartStreamingButton;
        internal MetroFramework.Controls.MetroLabel StreamingPortLabel;
        internal MetroFramework.Controls.MetroLabel metroLabel3;
        internal MetroFramework.Controls.MetroLabel metroLabel2;
        internal MetroFramework.Controls.MetroToggle metroToggle1;
        internal BrainpackModel mModel;

        public BrainpackControls(BrainpackModel vModel)
        {
            mModel = vModel;
            InitializeComponent();

            for (int vI = 0; vI < 9; vI++)
            {
                BrainpackGrid.Rows.Add(vI, Sensor.State.Disabled.ToString(), true, false);
            }
            BrainpackGrid.CellValueChanged += BrainpackGridCellChanged;
        }
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ParentGrid = new System.Windows.Forms.FlowLayoutPanel();
            this.BrainpackGrid = new MetroFramework.Controls.MetroGrid();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroToggle1 = new MetroFramework.Controls.MetroToggle();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.StreamingPortLabel = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.StartStreamingButton = new MetroFramework.Controls.MetroButton();
            this.SelectRecordingFileButton = new MetroFramework.Controls.MetroButton();
            this.DeleteButton = new MetroFramework.Controls.MetroButton();
            this.SettingsButton = new MetroFramework.Controls.MetroButton();
            this.BrainpackConnectionStart = new MetroFramework.Controls.MetroButton();
            this.ParentGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrainpackGrid)).BeginInit();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ParentGrid
            // 
            this.ParentGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ParentGrid.Controls.Add(this.BrainpackGrid);
            this.ParentGrid.Controls.Add(this.metroPanel1);
            this.ParentGrid.Location = new System.Drawing.Point(3, 3);
            this.ParentGrid.Name = "ParentGrid";
            this.ParentGrid.Size = new System.Drawing.Size(557, 331);
            this.ParentGrid.TabIndex = 4;
            // 
            // BrainpackGrid
            // 
            this.BrainpackGrid.AllowDrop = true;
            this.BrainpackGrid.AllowUserToAddRows = false;
            this.BrainpackGrid.AllowUserToDeleteRows = false;
            this.BrainpackGrid.AllowUserToResizeRows = false;
            this.BrainpackGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BrainpackGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BrainpackGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.BrainpackGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BrainpackGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.BrainpackGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrainpackGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.BrainpackGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.BrainpackGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.BrainpackGrid.EnableHeadersVisualStyles = false;
            this.BrainpackGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BrainpackGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BrainpackGrid.Location = new System.Drawing.Point(3, 3);
            this.BrainpackGrid.MultiSelect = false;
            this.BrainpackGrid.Name = "BrainpackGrid";
            this.BrainpackGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BrainpackGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.BrainpackGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.BrainpackGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.BrainpackGrid.Size = new System.Drawing.Size(552, 199);
            this.BrainpackGrid.TabIndex = 4;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Sensor ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "State";
            this.Column2.Items.AddRange(new object[] {
            "Disabled",
            "Enabled"});
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Calibrated";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Magnetic Transience? ";
            this.Column4.Name = "Column4";
            // 
            // metroPanel1
            // 
            this.metroPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel1.Controls.Add(this.metroToggle1);
            this.metroPanel1.Controls.Add(this.metroLabel2);
            this.metroPanel1.Controls.Add(this.StreamingPortLabel);
            this.metroPanel1.Controls.Add(this.metroLabel3);
            this.metroPanel1.Controls.Add(this.StartStreamingButton);
            this.metroPanel1.Controls.Add(this.SelectRecordingFileButton);
            this.metroPanel1.Controls.Add(this.DeleteButton);
            this.metroPanel1.Controls.Add(this.SettingsButton);
            this.metroPanel1.Controls.Add(this.BrainpackConnectionStart);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(3, 208);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(552, 121);
            this.metroPanel1.TabIndex = 6;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // metroToggle1
            // 
            this.metroToggle1.AutoSize = true;
            this.metroToggle1.Location = new System.Drawing.Point(441, 22);
            this.metroToggle1.Name = "metroToggle1";
            this.metroToggle1.Size = new System.Drawing.Size(80, 17);
            this.metroToggle1.TabIndex = 11;
            this.metroToggle1.Text = "Off";
            this.metroToggle1.UseSelectable = true;
            this.metroToggle1.CheckedChanged += new System.EventHandler(this.metroToggle1_CheckedChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(219, 20);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(216, 19);
            this.metroLabel2.TabIndex = 10;
            this.metroLabel2.Text = "Randomize Pain Report Generation";
            // 
            // StreamingPortLabel
            // 
            this.StreamingPortLabel.AutoSize = true;
            this.StreamingPortLabel.Location = new System.Drawing.Point(441, 3);
            this.StreamingPortLabel.Name = "StreamingPortLabel";
            this.StreamingPortLabel.Size = new System.Drawing.Size(37, 19);
            this.StreamingPortLabel.TabIndex = 9;
            this.StreamingPortLabel.Text = "9999";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(297, 3);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(147, 19);
            this.metroLabel3.TabIndex = 8;
            this.metroLabel3.Text = "Streaming File On Port:";
            // 
            // StartStreamingButton
            // 
            this.StartStreamingButton.Location = new System.Drawing.Point(136, 69);
            this.StartStreamingButton.Name = "StartStreamingButton";
            this.StartStreamingButton.Size = new System.Drawing.Size(123, 27);
            this.StartStreamingButton.TabIndex = 6;
            this.StartStreamingButton.Text = "Start Streaming";
            this.StartStreamingButton.UseSelectable = true;
            this.StartStreamingButton.Click += new System.EventHandler(this.StartStreamingButton_Click);
            // 
            // SelectRecordingFileButton
            // 
            this.SelectRecordingFileButton.Location = new System.Drawing.Point(7, 69);
            this.SelectRecordingFileButton.Name = "SelectRecordingFileButton";
            this.SelectRecordingFileButton.Size = new System.Drawing.Size(123, 27);
            this.SelectRecordingFileButton.TabIndex = 5;
            this.SelectRecordingFileButton.Text = "Select Recording File";
            this.SelectRecordingFileButton.UseSelectable = true;
            this.SelectRecordingFileButton.Click += new System.EventHandler(this.SelectRecordingFileButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(265, 69);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(108, 27);
            this.DeleteButton.TabIndex = 4;
            this.DeleteButton.Text = "Delete Brainpack";
            this.DeleteButton.UseSelectable = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButtonClick);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Location = new System.Drawing.Point(7, 36);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(123, 27);
            this.SettingsButton.TabIndex = 3;
            this.SettingsButton.Text = "Settings";
            this.SettingsButton.UseSelectable = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButtonClick);
            // 
            // BrainpackConnectionStart
            // 
            this.BrainpackConnectionStart.Location = new System.Drawing.Point(7, 3);
            this.BrainpackConnectionStart.Name = "BrainpackConnectionStart";
            this.BrainpackConnectionStart.Size = new System.Drawing.Size(123, 26);
            this.BrainpackConnectionStart.TabIndex = 2;
            this.BrainpackConnectionStart.Text = "Start Advertising";
            this.BrainpackConnectionStart.UseSelectable = true;
            this.BrainpackConnectionStart.Click += new System.EventHandler(this.BrainpackConnectionStart_Click);
            // 
            // BrainpackControls
            // 
            this.Controls.Add(this.ParentGrid);
            this.Name = "BrainpackControls";
            this.Size = new System.Drawing.Size(563, 337);
            this.ParentGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BrainpackGrid)).EndInit();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void BrainpackGridCellChanged(object vSender, DataGridViewCellEventArgs vE)
        {
            int vRowIndex = vE.RowIndex;

            Sensor.State vState = (Sensor.State)Enum.Parse(typeof(Sensor.State),
                BrainpackGrid[1, vRowIndex].Value.ToString(), true);
            mModel.Sensors[vRowIndex].CurrentState = vState;
            mModel.Sensors[vRowIndex].IsCalibrated = (bool)BrainpackGrid[2, vRowIndex].Value;
            mModel.Sensors[vRowIndex].IsInMagneticTransience = (bool)BrainpackGrid[3, vRowIndex].Value;
        }

        private void BrainpackConnectionStart_Click(object sender, System.EventArgs e)
        {

        }

        private void SettingsButtonClick(object sender, System.EventArgs e)
        {

        }

        private void DeleteButtonClick(object sender, System.EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vVisibilityFlag"></param>
        public void SetVisibility(bool vVisibilityFlag)
        {
            this.Visible = vVisibilityFlag;
        }

        private void SelectRecordingFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog vDialog = new OpenFileDialog();
            vDialog.Filter = "hsm files | *.hsm";
            if (vDialog.ShowDialog() == DialogResult.OK)
            {
                mModel.RecordingFile = new FileInfo(vDialog.FileName);
            }
        }

        private void StartStreamingButton_Click(object sender, EventArgs e)
        {

        }

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {


        }
    }
}