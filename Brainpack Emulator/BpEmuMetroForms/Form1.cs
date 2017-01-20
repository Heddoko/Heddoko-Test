
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BpEmuMetroForms.Brainpack;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BpEmuMetroForms.Brainpack.View;
using heddoko;
using Newtonsoft.Json;

namespace BpEmuMetroForms
{
    public partial class Form1 : MetroForm
    {
        private BrainpackControllerSet mControllers;
        private MetroTile mCurrentTile;

        private static string SerializationDir => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private const string JSonFilePath = "BpModels.json";
        public int MaxNum = 10;
        public Form1()
        {

            InitializeComponent();
            mControllers = new BrainpackControllerSet(ParentGrid);
        }



        private void AddBrainpackButtonClick(object vSender, System.EventArgs vE)
        {

            BrainpackForm vForm = new BrainpackForm("Create New Brainpack");
            vForm.BrainpackValidation = mControllers.BrainpackExist;
            vForm.ConfigurationPortValidation = mControllers.ConfigurationPortExist;
            vForm.SaveBrainpackConfigurationEvent += AddBrainpack;
            vForm.ShowDialog(this);
        }

        private void AddBrainpack(BrainpackController vController)
        {
            MetroTile vTile = new MetroTile();
            vTile.Parent = BrainpackTileFlowParent;
            vTile.Show();
            vTile.Text = vController.Model.SerialNum;
            vTile.Click += BrainpackTileClick;
            vTile.Size = new Size(120, 80);
            mControllers.AddBrainpack(vController, vTile);

        }

        private void BrainpackTileClick(object vSender, EventArgs vE)
        {
            //hide previous view
            if (mCurrentTile != null)
            {
                BrainpackController vPrevController = mControllers.GetController(mCurrentTile.Text);
                vPrevController.HideView();
            }
            mCurrentTile = (MetroTile)vSender;
            BrainpackController vController = mControllers.GetController(mCurrentTile.Text);
            vController.ShowView();
            ParentGrid.Show();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            BrainpackController vController = mControllers.GetController(mCurrentTile.Text);
   
        }

        /// <summary>
        /// Delete selected brainpack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton1_Click(object sender, EventArgs e)
        {
            mControllers.RemoveBrainpack(mControllers.GetController(mCurrentTile.Text));
            this.Controls.Remove(mCurrentTile);
            mCurrentTile.Dispose();
            mCurrentTile = null;
            ParentGrid.Hide();
        }

        private void BrainpackConnectionStart_Click(object sender, EventArgs e)
        {
            var vController = mControllers.GetController(mCurrentTile.Text);

        }

        private void stateBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mControllers.SerializeModels();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            var vPath = SerializationDir + JSonFilePath;
            JsonSerializer vDeserializer = new JsonSerializer();
            JsonTextReader vJsonTxtReader = new JsonTextReader(File.OpenText(vPath));
            vDeserializer.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            List<BrainpackModel> vModels = (List<BrainpackModel>)vDeserializer.Deserialize(vJsonTxtReader, typeof(List<BrainpackModel>));
            foreach (var vBrainpackModel in vModels)
            {
                BrainpackController vController = new BrainpackController(vBrainpackModel);
                MetroTile vTile = new MetroTile();
                vTile.Parent = ParentGrid;
                vTile.Show();
                vTile.Text = vController.Model.SerialNum;
                vTile.Click += BrainpackTileClick;
                vTile.Size = new Size(120, 80);
                mControllers.AddBrainpack(vController, vTile);
            }
        }
    }
}
