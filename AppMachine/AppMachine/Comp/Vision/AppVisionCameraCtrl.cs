using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.VisionSystem;
using MCore.Comp.IOSystem;

using AppMachine.Control;

namespace AppMachine.Comp.Vision
{
    public partial class AppVisionCameraCtrl : AppUserControlBase
    {
        CameraBase _camera = null;

        public AppVisionCameraCtrl():base()
        {
            InitializeComponent();
        }

        public AppVisionCameraCtrl(CameraBase camera):base(camera)
        {
            _camera = camera;
        }


        protected override void Initializing()
        {
            InitializeComponent();
            camPanel.Bind = _camera;
            CameraPropertiesCtl camPropertyCttrl = new CameraPropertiesCtl();
            camPropertyCttrl.Dock = DockStyle.Fill;
            camPropertyCttrl.Bind = _camera;
            panelCameraProperty.Controls.Add(camPropertyCttrl);

        }

        private void OnLiveClicked(object sender, EventArgs e)
        {
            _camera.TriggerMode = cbLive.Checked ? CompMeasure.eTriggerMode.TimedTrigger : CompMeasure.eTriggerMode.Idle;
        }

        private void cbCrossHair_CheckedChanged(object sender, EventArgs e)
        {
            camPanel.CrossHairs = cbCrossHair.Checked;
        }
    }
}
