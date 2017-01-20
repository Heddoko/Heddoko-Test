namespace BpEmuMetroForms
{
    partial class Form1
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
            this.BrainpackTileFlowParent = new System.Windows.Forms.FlowLayoutPanel();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.ParentGrid = new System.Windows.Forms.FlowLayoutPanel();
            this.stateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.metroTile2 = new MetroFramework.Controls.MetroTile();
            ((System.ComponentModel.ISupportInitialize)(this.stateBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // BrainpackTileFlowParent
            // 
            this.BrainpackTileFlowParent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BrainpackTileFlowParent.Location = new System.Drawing.Point(23, 149);
            this.BrainpackTileFlowParent.Name = "BrainpackTileFlowParent";
            this.BrainpackTileFlowParent.Size = new System.Drawing.Size(280, 442);
            this.BrainpackTileFlowParent.TabIndex = 2;
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.Location = new System.Drawing.Point(23, 63);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(42, 40);
            this.metroTile1.TabIndex = 3;
            this.metroTile1.Text = "+";
            this.metroTile1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTile1.UseSelectable = true;
            this.metroTile1.Click += new System.EventHandler(this.AddBrainpackButtonClick);
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Tag = "";
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // ParentGrid
            // 
            this.ParentGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ParentGrid.Location = new System.Drawing.Point(323, 149);
            this.ParentGrid.Name = "ParentGrid";
            this.ParentGrid.Size = new System.Drawing.Size(710, 442);
            this.ParentGrid.TabIndex = 3;
            // 
            // stateBindingSource
            // 
            this.stateBindingSource.DataSource = typeof(BpEmuMetroForms.Brainpack.Sensor.State);
            this.stateBindingSource.CurrentChanged += new System.EventHandler(this.stateBindingSource_CurrentChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 597);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(217, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Save brainpack list";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // metroTile2
            // 
            this.metroTile2.ActiveControl = null;
            this.metroTile2.Location = new System.Drawing.Point(103, 63);
            this.metroTile2.Name = "metroTile2";
            this.metroTile2.Size = new System.Drawing.Size(70, 40);
            this.metroTile2.TabIndex = 5;
            this.metroTile2.Text = "Load";
            this.metroTile2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTile2.UseSelectable = true;
            this.metroTile2.Click += new System.EventHandler(this.metroTile2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 642);
            this.Controls.Add(this.metroTile2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ParentGrid);
            this.Controls.Add(this.metroTile1);
            this.Controls.Add(this.BrainpackTileFlowParent);
            this.Name = "Form1";
            this.Text = "Brainpack Emulator";
            ((System.ComponentModel.ISupportInitialize)(this.stateBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel BrainpackTileFlowParent;
        private MetroFramework.Controls.MetroTile metroTile1;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private System.Windows.Forms.FlowLayoutPanel ParentGrid;
        private System.Windows.Forms.BindingSource stateBindingSource;
        private System.Windows.Forms.Button button1;
        private MetroFramework.Controls.MetroTile metroTile2;
    }
}

