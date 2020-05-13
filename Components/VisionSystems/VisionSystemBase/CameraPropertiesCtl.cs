using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.VisionSystem
{
    public partial class CameraPropertiesCtl : UserControl, IComponentBinding<CameraBase>
    {
        private CameraBase _cameraBase = null;
        public CameraPropertiesCtl()
        {
            InitializeComponent();
        }
        public override string ToString()
        {
            return "Properties";
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
                gbCamera.Text = _cameraBase.Nickname;
                tbCameraName.Text = _cameraBase.CameraName;
                tbSerialNumber.Text = _cameraBase.SerialNumber;
                tbPortDescription.Text = _cameraBase.PortDescription;
                tbBrightness.BindTwoWay(() => _cameraBase.Brightness);
                tbContrast.BindTwoWay(() => _cameraBase.Contrast);
                tbExposure.BindTwoWay(() => _cameraBase.Exposure);
            }
        }
    }
}
