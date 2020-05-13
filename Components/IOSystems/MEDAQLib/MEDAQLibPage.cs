using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.IOSystem
{
    public partial class MEDAQLibPage : UserControl, IComponentBinding<MEDAQLib>
    {
        private MEDAQLib _controller = null;
        private bool _supressChanges = true;
        public MEDAQLibPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Bind to the MEDAQLib component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MEDAQLib Bind
        {
            get { return _controller; }
            set
            {
                _controller = value;

                cbMeasurementRate.BindTwoWay(() => _controller.MeasurementRate);
                cbMedianChoices.BindTwoWay(() => _controller.MedianChoice);
                cbExtInputMode.BindTwoWay(() => _controller.ExtInputMode);
                intMovingAverageCount.BindTwoWay(() => _controller.MovingCount);
                if (_controller.MMInput != null)
                {
                    mdReadValue.BindTwoWay(() => _controller.MMInput.Value);
                }

                rbAvgMedian.Checked = _controller.MedianAvg;
                rbAvgMoving.Checked = !_controller.MedianAvg;
                
                _controller.SetupGetInfo();
                tbSensor.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_Sensor);
                tbSensorType.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_SensorType);
                tbArticleNumber.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_ArticleNumber);
                tbOption.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_Option);
                tbSerialNumber.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_SerialNumber);
                tbMeasurementRange.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_Range);
                tbSoftwareVersion.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_Softwareversion);
                tbBootloaderVersion.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_BootLoaderVer);
                tbDate.Text = _controller.GetInfoItem(MEDAQLib.eGetInfo.SA_Date);

                triggerMode.Bind = _controller.MMInput;
                _supressChanges = false;
            }
        }
        /// <summary>
        /// This will define the name of the page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Controller Settings";
        }

        private void OnChangedAveragingType(object sender, EventArgs e)
        {
            if (_supressChanges)
                return;
            _controller.MedianAvg = rbAvgMedian.Checked;
        }


    }
}
