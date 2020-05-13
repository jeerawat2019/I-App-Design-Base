using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore;
using MCore.Comp.MotionSystem;
using MCore.Comp.VisionSystem;
using MCore.Comp.IOSystem;

using AppMachine.Comp.Motion;
using AppMachine.Comp.CommonParam;
using AppMachine.Control;


namespace AppMachine.Panel.SubPanel
{
    public partial class AppDemoVisonSetupPanel : AppSubPanelBase
    {

        private AppRealAxis _visionXAxis = null;
        private CameraBase _visionCamera = null;
        private VisionJobBase _visionJob = null;
        
        public AppDemoVisonSetupPanel()
        {
            InitializeComponent();
        }

        protected override void Initializing()
        {
            InitializeComponent();
            base.Initializing();
            This = this;

            _visionXAxis = new AppRealAxis("Vision X Axis");
            _visionCamera = new CameraBase("Vision Camera");
            _visionJob = new VisionJobBase("Vision Job");

            /*Get Actual Component Example Pattern
            _visionXAxis = U.GetComponent(AppConstStaticName.VISIONXAXIS) as AppRealAxis;
            _visionCamera = U.GetComponent(AppConstStaticName.VISIONCAMERA) as CameraBase;
            _visionJob = U.GetComponent(AppConstStaticName.VISIONJOB) as VisionJobBase;
            */

            panelVisionXAxis.Controls.Add(new AppMotionAxisCtrl(_visionXAxis, false,false));
            pgVisionX.BrowsableAttributes = new AttributeCollection(new CategoryAttribute("Vision Motion"));
            pgVisionX.SelectedObject = AppCommonParam.This;



            /*Axis Teach Control Binding Example
            AppAxisTeachCtrl visionXInspecTeach = new AppAxisTeachCtrl("VisionX Insp Pos", _visionXAxis);
            visionXInspecTeach.RegisterProperty(() => AppCommonParam.This.VisionXInpecPos);
            visionXInspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnVisionXTeachValueChange);
            flpVisionXTeach.Controls.Add(visionXInspecTeach);

          

            AppAxisTeachCtrl visionXStbyTeach = new AppAxisTeachCtrl("VisionX Stby Pos", _visionXAxis);
            visionXStbyTeach.RegisterProperty(() => AppCommonParam.This.VisionXStandbyPos);
            visionXStbyTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnVisionXTeachValueChange);
            flpVisionXTeach.Controls.Add(visionXStbyTeach);
            */

            CameraPropertiesCtl camPropertyCttrl = new CameraPropertiesCtl();
            camPropertyCttrl.Bind = _visionCamera;
            panelCameraProperty.Controls.Add(camPropertyCttrl);

            VisionJobRunPage visionJobCtrl = new VisionJobRunPage();
            visionJobCtrl.Bind = _visionJob;
            panelVisionJob.Controls.Add(visionJobCtrl);
            _visionCamera.RegisterCameraWindow(camPanel);

            this.Update();
        }

        private void OnVisionXTeachValueChange(MDouble.Millimeters teachPosition)
        {
            pgVisionX.Refresh();
        }

        private void OnLiveClicked(object sender, EventArgs e)
        {
            _visionCamera.TriggerMode = cbLive.Checked ? CompMeasure.eTriggerMode.TimedTrigger : CompMeasure.eTriggerMode.Idle;
        }
    }
}
