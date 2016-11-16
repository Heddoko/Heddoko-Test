// /**
// * @file BrainpackController.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 11 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Collections;
using System.Timers;
using System.Windows.Forms;
using WindowsBPEmulator.Communication;
using BpEmuMetroForms.Brainpack.Communication;
using heddoko;

namespace BpEmuMetroForms.Brainpack
{
    public delegate void DeleteRequested(BrainpackController vController);
    /// <summary>
    /// A brainpack controller
    /// </summary>
    public class BrainpackController : IEqualityComparer
    {
        private BrainpackModel mModel;
        private RandomPathGiver mRandomPathGiver = new RandomPathGiver();
        private BrainpackAdvertiser mAdvertiser;
        private ServerListener mListener;
        private ProtobuffDispatch mDispatcher;
        private BrainpackControls mControls;
        private FileStreamer mStreamer;
        private UdpConnectionSend mSender;
        private System.Timers.Timer mConcernReportTimer;
        public bool IsAdvertising { get; set; }
        public event DeleteRequested DeleteRequestedEvent;

        public BrainpackController(BrainpackModel vModel)
        {
            mModel = vModel;
            mAdvertiser = new BrainpackAdvertiser(mModel);
            mListener = new ServerListener();
            mDispatcher = new ProtobuffDispatch(mListener);
            mListener.DataReceived += DataReceivedHandler;
            mDispatcher.DataStreamRequestStartEvent += DataStreamRequestStartHandler;
            mDispatcher.DataStreamRequestEndEvent += DataStreamRequestEndHandler;
            mControls = new BrainpackControls(vModel);
            mControls.SetVisibility(false);
            mControls.BrainpackConnectionStart.Click += SetAdvertisingState;
            mControls.SettingsButton.Click += LaunchSettingsView;
            mControls.DeleteButton.Click += DestroyThis;
            mControls.StartStreamingButton.Visible = false;
            mControls.SelectRecordingFileButton.Visible = false;
            mControls.StreamingPortLabel.Visible = false;
            mControls.metroLabel3.Visible = false;
            mControls.metroToggle1.Visible = false;
            mControls.metroToggle1.CheckedChanged += CheckChangedEvent;
            mModel.ModelChangedEvent += (x) =>
            {
                if (mAdvertiser != null)
                {
                    mAdvertiser.SetTickRate(x.AdvertisingTickRate);
                }
            };
        }

        private void CheckChangedEvent(object vSender, EventArgs vE)
        {
            bool vChecked = mControls.metroToggle1.Checked;
            if (vChecked)
            {
                if (mConcernReportTimer == null)
                {
                    OnConcernReportTimerElapsed(null, null);
                    mConcernReportTimer = new System.Timers.Timer(mModel.ConcernReportTickRate);
                    mConcernReportTimer.Elapsed += OnConcernReportTimerElapsed;
                }
                mConcernReportTimer.AutoReset = true;
                mConcernReportTimer.Start();
            }
            else
            {
                mConcernReportTimer.Stop();
            }
        }

        private void OnConcernReportTimerElapsed(object vSender, ElapsedEventArgs vE)
        {
            if (mStreamer != null)
            {
                Packet vPacket = new Packet();
                vPacket.type = PacketType.DataFrame;
                vPacket.fullDataFrame = new FullDataFrame();
                vPacket.fullDataFrame.reportTypeSpecified = true;
                vPacket.fullDataFrame.reportType = ReportType.pain;
                vPacket.fullDataFrame.gpsCoordinates = mRandomPathGiver.GetNextPointInPath();
                mStreamer.ItemToInterleave = vPacket;
            }

        }

        private void DataStreamRequestEndHandler(Packet vPacket)
        {

        }

        private void DataStreamRequestStartHandler(Packet vPacket)
        {
            //Enable the Start Stream Button to work
            mControls.Invoke(new MethodInvoker(delegate ()
            {
                mControls.SelectRecordingFileButton.Visible = true;
                mControls.SelectRecordingFileButton.Click += FileSelected;
                mControls.StreamingPortLabel.Text = vPacket.endpoint.port.ToString();
            }));


            if (mSender == null)
            {
                mSender = new UdpConnectionSend(vPacket.endpoint.address, (int)vPacket.endpoint.port);
            }
            else
            {
                if (!string.IsNullOrEmpty(vPacket.endpoint.address))
                {
                    if (!mSender.IpAddress.Equals(vPacket.endpoint.address) ||
                        mSender.Port != vPacket.endpoint.port)
                    {
                        mStreamer.Stop();
                        mSender.CloseClient();
                        mSender.SetConnectionInfo(vPacket.endpoint.address, (int)vPacket.endpoint.port);
                    }
                }
            }

        }

        private void FileSelected(object vSender, EventArgs vE)
        {
            mControls.StartStreamingButton.Visible = true;
            mControls.StartStreamingButton.Click += BeginStreaming;
            mControls.metroToggle1.Visible = true;
        }

        private void BeginStreaming(object vSender, EventArgs vE)
        {
            if (mStreamer == null)
            {
                mSender.Start();
                mStreamer = new FileStreamer();
                mStreamer.SetConnection(mSender);
                mStreamer.BeginFileSend(mModel.RecordingFile);
                mControls.StartStreamingButton.Visible = false;
                mControls.StreamingPortLabel.Visible = true;
                mControls.metroLabel3.Visible = true;
            }

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
            mAdvertiser.StartAdvertising();
            IsAdvertising = true;
            mListener.StartServer(mModel.ConfigurationPort);
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