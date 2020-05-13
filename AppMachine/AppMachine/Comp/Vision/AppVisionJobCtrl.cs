using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.VisionSystem;

using AppMachine.Control;

namespace AppMachine.Comp.Vision
{
    public partial class AppVisionJobCtrl : AppUserControlBase
    {
        VisionJobBase _visionJob = null;
        public AppVisionJobCtrl(VisionJobBase visionJob):base(visionJob)
        {
            _visionJob = visionJob;
           
        }

        protected override void Initializing()
        {
            InitializeComponent();
            base.Initializing();
            strVisionFilePath.BindTwoWay(() => _visionJob.VisionFile);
            VisionJobRunPage visionJobCtrl = new VisionJobRunPage();
            visionJobCtrl.Bind = _visionJob;
            panelVisionJob.Controls.Add(visionJobCtrl);
        }
    }
}
