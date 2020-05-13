using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;
using MCore.Comp.IOSystem;

namespace MCore.Comp.VisionSystem
{
    public partial class CameraBaseCtl : UserControl, IComponentBinding<CameraBase>
    {
        private CameraBase _cameraBase = null;
        public CameraBaseCtl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Bind to the CognexCamera7_1 component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CameraBase Bind
        {
            get { return _cameraBase; }
            set
            {
                _cameraBase = value;
                panelCamera.Bind = _cameraBase;
                RefreshCamera();
            }
        }
        private void RefreshCamera()
        {
            cbCamera.Items.Clear();
            cbCamera.Items.AddRange(_cameraBase.GetCameraIDs());

            // Do we have a valid camera selected?
            if (!_cameraBase.ValidCamera)
            {
                cbVideoFormat.Items.Clear();
                cbVideoFormat.Enabled = false;
                btnInitialize.Enabled = false;
                return;
            }

            // Camera is good.  Select it            
            cbCamera.SelectedItem = _cameraBase.CameraID;

            // Now get the Video formats
            GetVideoFormats();
        }

        private void GetVideoFormats()
        {
            cbVideoFormat.Items.Clear();
            cbVideoFormat.Items.AddRange(_cameraBase.GetVideoFormats());
            cbVideoFormat.SelectedItem = _cameraBase.VideoFormat;
            btnInitialize.Enabled = cbVideoFormat.Items.Count > 0;
            cbVideoFormat.Enabled = cbVideoFormat.Items.Count > 0;
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            _cameraBase.InitializeFrameGrabber();
        }

        private void OnChangeCamera(object sender, EventArgs e)
        {
            _cameraBase.CameraID = cbCamera.SelectedItem as string;
            _cameraBase.GetCameraIDs();
            GetVideoFormats();
        }

        private void OnChangeVideoFormat(object sender, EventArgs e)
        {
            _cameraBase.VideoFormat = cbVideoFormat.SelectedItem as string;
        }

        private void OnToggleLive(object sender, EventArgs e)
        {
            
        }

        public override string ToString()
        {
            return "Camera";
        }

        private void OnLiveClicked(object sender, EventArgs e)
        {
            _cameraBase.TriggerMode = cbLive.Checked ? CompMeasure.eTriggerMode.Live : CompMeasure.eTriggerMode.Idle;
        }
    }
}
