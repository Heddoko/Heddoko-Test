// /**
// * @file BrainpackController.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 11 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Collections;
using System.Windows.Forms;
using WindowsBPEmulator.Communication;
using BpEmuMetroForms.Brainpack.Communication;
using MetroFramework.Controls;

namespace BpEmuMetroForms.Brainpack
{
    public delegate void DeleteRequested(BrainpackController vController);
    /// <summary>
    /// A brainpack controller
    /// </summary>
    public class BrainpackController : IEqualityComparer
    {
        private BrainpackModel mModel;
        private BrainpackAdvertiser mAdvertiser;
        private ServerListener mListener;
        private ProtobuffDispatch mDispatcher;
        private BrainpackControls mControls;
        public bool IsAdvertising { get; set; }
        public event DeleteRequested DeleteRequestedEvent;

        public BrainpackController(BrainpackModel vModel)
        {
            mModel = vModel;
            mAdvertiser = new BrainpackAdvertiser(mModel);
            mListener = new ServerListener();
            mDispatcher = new ProtobuffDispatch(mListener);

            mListener.DataReceived += DataReceivedHandler;
            mControls = new BrainpackControls(vModel);
            mControls.SetVisibility(false);
            mControls.BrainpackConnectionStart.Click += SetAdvertisingState;
            mControls.SettingsButton.Click += LaunchSettingsView;
            mControls.DeleteButton.Click += DestroyThis;

        }

        private void DestroyThis(object vSender, EventArgs vE)
        {
            mAdvertiser.StopAdvertising();
            IsAdvertising = false;
            DeleteRequestedEvent?.Invoke(this);
        }

        private void LaunchSettingsView(object vSender, EventArgs vE)
        {
            BrainpackForm vForm = new BrainpackForm(this, "Brainpack Settings");
            vForm.ShowDialog();
        }

        private void SetAdvertisingState(object vSender, EventArgs vE)
        {
            if (!IsAdvertising)
            {
                BeginAdvertising();
                mControls.BrainpackConnectionStart.Text = "Stop Advertising";
                IsAdvertising = true;
            }
            else
            {
                mAdvertiser.StopAdvertising();
                IsAdvertising = false;
                mControls.BrainpackConnectionStart.Text = "Start Advertising";
            }
        }


        /// <summary>
        /// Getter: the brainpack model the controller uses. 
        /// </summary>
        public BrainpackModel Model
        {
            get { return mModel; }
        }
        private void DataReceivedHandler(object vSender, byte[] vE)
        {

        }


        /// <summary>
        /// Begins advertising and listening on the provided model
        /// </summary>
        public void BeginAdvertising()
        {
            mAdvertiser.StartAdvertising(9999);
            IsAdvertising = true;
            mListener.StartServer(mModel.ConfigurationPort);
        }


        public void TransmitRecordingFile()
        {

        }

        public bool Equals(object vX, object vY)
        {
            if (vX == null || vY == null)
            {
                return false;
            }
            if (vX.GetType() != typeof(BrainpackController) || vY.GetType() != typeof(BrainpackController))
            {
                return false;
            }
            BrainpackController vModelX = (BrainpackController)vX;
            BrainpackController vModelY = (BrainpackController)vY;
            return vModelX.mModel.Equals(vModelY.mModel);
        }

        public int GetHashCode(object vObj)
        {
            BrainpackController vModelObj = (BrainpackController)vObj;
            return vModelObj.mModel.GetHashCode();
        }

        public void HideView()
        {
            mControls.SetVisibility(false);
        }

        public void ShowView()
        {
            mControls.SetVisibility(true);
        }

        public void SetViewParent(FlowLayoutPanel vParentGrid)
        {
            mControls.Parent = vParentGrid;
        }
    }
}