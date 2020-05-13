using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.PressureSystem
{
    public partial class MusashiPPg : UserControl, IComponentBinding<MusashiPressureCtl>
    {
        private MusashiPressureCtl _pressureCtl = null;





        public override string ToString()
        {
            return "Musashi Control";
        }
        public MusashiPPg()
        {
            InitializeComponent();
            mdPressure.UnitsLabel = "kPa";
        }

        /// <summary>
        /// Bind to the MusashiPressureCtl
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MusashiPressureCtl Bind
        {
            get { return _pressureCtl; }
            set
            {
                if (_pressureCtl != null)
                {
                    strChannel.UnBind();
                    mdDispenseTime.UnBind();
                    mdVacuum.UnBind();
                    mchTimedMode.UnBind();
                    mdPressure.UnBind();
                }
                _pressureCtl = value;
                if (_pressureCtl != null)
                {
                    strChannel.BindTwoWay(() => _pressureCtl.PressureDevice.ChannelID);
                    mdDispenseTime.BindTwoWay(() => _pressureCtl.PressureDevice.DispenseTime);
                    mdVacuum.BindTwoWay(() => _pressureCtl.PressureDevice.VacuumPressure);
                    mchTimedMode.BindTwoWay(() => _pressureCtl.PressureDevice.TimedMode);
                    mdPressure.Enabled = true;
                    mdPressure.DoubleVal = _pressureCtl.UpdateAll();
                }
                else
                {
                    mdPressure.Enabled = false;
                }
            }
        }

        private void btnDownloadExecute_Click(object sender, EventArgs e)
        {
            lblDownloadResponse.Text = _pressureCtl.Download(tbDownloadCommand.Text);
            mdPressure.DoubleVal = _pressureCtl.UpdateAll();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            lblUploadResponse.Text = _pressureCtl.Upload(tbUploadCommand.Text);
        }

        private void btnSetPressure_Click(object sender, EventArgs e)
        {
            _pressureCtl.SetPressure(_pressureCtl.PressureDevice, mdPressure.DoubleVal);
        }

        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            mdPressure.DoubleVal = _pressureCtl.UpdateAll();
        }
    }
}
