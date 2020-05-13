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
    public partial class CognexCamPage : UserControl, IComponentBinding<CognexCamera9>
    {
        private CognexCamera9 _camera = null;
        public CognexCamPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Bind to the CognexCamera7_1 component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CognexCamera9 Bind
        {
            get { return _camera; }
            set
            {
                _camera = value;
                mDoubleBrightness.BindTwoWay(() => _camera.Brightness);
                mDoubleContrast.BindTwoWay(() => _camera.Contrast);
                mDoubleUnitsExposure.BindTwoWay(() => _camera.Exposure);
                mDoubleUnitsAquireTime.BindTwoWay(() => _camera.AcquireTime);
                triggerMode.Bind = _camera;
                _camera.RegisterCameraWindow(panelCamera);
                RefreshCamera();
            }
        }


        private void RefreshCamera()
        {
            cbCamera.Items.Clear();
            cbCamera.Items.AddRange(_camera.GetCameraIDs());

            // Do we have a valid camera selected?
            if (!_camera.ValidCamera)
            {
                cbVideoFormat.Items.Clear();
                cbVideoFormat.Enabled = false;
                btnInitialize.Enabled = false;
                return;
            }

            // Camera is good.  Select it            
            cbCamera.SelectedItem = _camera.CameraID;

            // Now get the Video formats
            GetVideoFormats();
        }

        private void GetVideoFormats()
        {
            cbVideoFormat.Items.Clear();
            cbVideoFormat.Items.AddRange(_camera.GetVideoFormats());
            cbVideoFormat.SelectedItem = _camera.VideoFormat;
            btnInitialize.Enabled = cbVideoFormat.Items.Count > 0;
            cbVideoFormat.Enabled = cbVideoFormat.Items.Count > 0;
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            _camera.InitializeFrameGrabber();
        }

        private void OnChangeCamera(object sender, EventArgs e)
        {
            _camera.CameraID = cbCamera.SelectedItem as string;
            GetVideoFormats();
        }

        private void OnChangeVideoFormat(object sender, EventArgs e)
        {
            _camera.VideoFormat = cbVideoFormat.SelectedItem as string;
        }

        public override string ToString()
        {
            return "Cognex Camera";
        }

    }
}
