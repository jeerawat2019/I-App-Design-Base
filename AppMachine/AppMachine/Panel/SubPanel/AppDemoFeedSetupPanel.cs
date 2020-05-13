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

using AppMachine.Comp.Motion;
using AppMachine.Comp.CommonParam;

namespace AppMachine.Panel.SubPanel
{
    public partial class AppDemoFeedSetupPanel : AppSubPanelBase
    {
        private AppRealAxis _feedYAxis = null;
        private AppRealAxis _lift1ZAxis = null;
        private AppRealAxis _lift2ZAxis = null;


        public AppDemoFeedSetupPanel()
        {
            InitializeComponent();
        }

        protected override void Initializing()
        {
            InitializeComponent();
            base.Initializing();
            This = this;

           

            //Create Demo Axis (Actaul Axis Follow in Below Pattern)
            _feedYAxis = new AppRealAxis("Demo Feed Y");
            _lift1ZAxis = new AppRealAxis("Demo Lift1");
            _lift2ZAxis = new AppRealAxis("Demo Lift2");

            /*Get Actual Component Example Pattern
            _feedYAxis = U.GetComponent(AppConstStaticName.FEEDYAXIS) as AppRealAxis;
            _lift1ZAxis = U.GetComponent(AppConstStaticName.LIFT1ZAXIS) as AppRealAxis;
            _lift2ZAxis = U.GetComponent(AppConstStaticName.LIFT2ZAXIS) as AppRealAxis;
            */

            panelFeedYAxis.Controls.Add(new AppMotionAxisCtrl(_feedYAxis,false,false) );
            panelLift1ZAxis.Controls.Add(new AppMotionAxisCtrl(_lift1ZAxis,false,false));
            panelLift2ZAxis.Controls.Add(new AppMotionAxisCtrl(_lift2ZAxis,false,false));

            pgFeedY.BrowsableAttributes = new AttributeCollection(new CategoryAttribute("Feeder Motion"));
            pgFeedY.SelectedObject = AppCommonParam.This;

            pgLift1.BrowsableAttributes = new AttributeCollection(new CategoryAttribute("Lift1 Motion"));
            pgLift1.SelectedObject = AppCommonParam.This;

            pgLift2.BrowsableAttributes = new AttributeCollection(new CategoryAttribute("Lift2 Motion"));
            pgLift2.SelectedObject = AppCommonParam.This;

            /*Axis Teach Control Binding Example
            #region Add Feed Y Teach
            AppAxisTeachCtrl feedYLift1IncomeTeach = new AppAxisTeachCtrl("Y Lift1 Income Pos", _feedYAxis);
            feedYLift1IncomeTeach.RegisterProperty(() => AppCommonParam.This.FeedYLift1IncomePos);
            feedYLift1IncomeTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnFeedYTeachValueChange);
            flpFeedYTeach.Controls.Add(feedYLift1IncomeTeach);

            AppAxisTeachCtrl feedYLift1RejTeach = new AppAxisTeachCtrl("Y Lift1 Reject Pos", _feedYAxis);
            feedYLift1RejTeach.RegisterProperty(() => AppCommonParam.This.FeedYLift1RejectPos);
            feedYLift1RejTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnFeedYTeachValueChange);
            flpFeedYTeach.Controls.Add(feedYLift1RejTeach);

            AppAxisTeachCtrl feedYLift1InspecTeach = new AppAxisTeachCtrl("Y Lift1 Inspec Pos", _feedYAxis);
            feedYLift1InspecTeach.RegisterProperty(() => AppCommonParam.This.FeedYLift1InspecPos);
            feedYLift1InspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnFeedYTeachValueChange);
            flpFeedYTeach.Controls.Add(feedYLift1InspecTeach);


            AppAxisTeachCtrl feedYLift2RejectTeach = new AppAxisTeachCtrl("Y Lift2 Reject Pos", _feedYAxis);
            feedYLift2RejectTeach.RegisterProperty(() => AppCommonParam.This.FeedYLift2RejectPos);
            feedYLift2RejectTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnFeedYTeachValueChange);
            flpFeedYTeach.Controls.Add(feedYLift2RejectTeach);

            AppAxisTeachCtrl feedYLift2EmptyTeach = new AppAxisTeachCtrl("Y Lift2 Empty Pos", _feedYAxis);
            feedYLift2EmptyTeach.RegisterProperty(() => AppCommonParam.This.FeedYLift2EmptyPos);
            feedYLift2EmptyTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnFeedYTeachValueChange);
            flpFeedYTeach.Controls.Add(feedYLift2EmptyTeach);

            AppAxisTeachCtrl feedYLift2InspecTeach = new AppAxisTeachCtrl("Y Lift2 Inspec Pos", _feedYAxis);
            feedYLift2InspecTeach.RegisterProperty(() => AppCommonParam.This.FeedYLift2InspecPos);
            feedYLift2InspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnFeedYTeachValueChange);
            flpFeedYTeach.Controls.Add(feedYLift2InspecTeach);
            #endregion

            #region Add Lift1 Z Teach
            AppAxisTeachCtrl Lift1ZPreUpIncomeTeach = new AppAxisTeachCtrl("Lift1Z Pre-Up Inc Pos", _lift1ZAxis);
            Lift1ZPreUpIncomeTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZPreUpIncomePos);
            Lift1ZPreUpIncomeTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZPreUpIncomeTeach);

            AppAxisTeachCtrl Lift1ZUpIncomeTeach = new AppAxisTeachCtrl("Lift1Z Up Inc Pos", _lift1ZAxis);
            Lift1ZUpIncomeTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZUpIncomePos);
            Lift1ZUpIncomeTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZUpIncomeTeach);

            AppAxisTeachCtrl Lift1ZDnStepIncomeTeach = new AppAxisTeachCtrl("Lift1Z Dn Step Inc Pos", _lift1ZAxis);
            Lift1ZDnStepIncomeTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZDnStepIncomePos);
            Lift1ZDnStepIncomeTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZDnStepIncomeTeach);


            AppAxisTeachCtrl Lift1ZPreUpRejectTeach = new AppAxisTeachCtrl("Lift1Z Pre-Up Rej Pos", _lift1ZAxis);
            Lift1ZPreUpRejectTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZPreUpRejectPos);
            Lift1ZPreUpRejectTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZPreUpRejectTeach);

            AppAxisTeachCtrl Lift1ZUpRejectTeach = new AppAxisTeachCtrl("Lift1Z Up Rej Pos", _lift1ZAxis);
            Lift1ZUpRejectTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZUpRejectPos);
            Lift1ZUpRejectTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZUpRejectTeach);

            AppAxisTeachCtrl Lift1ZDnStepRejectTeach = new AppAxisTeachCtrl("Lift1Z Dn Rej Inc Pos", _lift1ZAxis);
            Lift1ZDnStepRejectTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZDnStepRejectPos);
            Lift1ZDnStepRejectTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZDnStepRejectTeach);

            AppAxisTeachCtrl Lift1ZPreUpInspecTeach = new AppAxisTeachCtrl("Lift1Z Pre-Up Insp Pos", _lift1ZAxis);
            Lift1ZPreUpInspecTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZPreUpInspecPos);
            Lift1ZPreUpInspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZPreUpInspecTeach);

            AppAxisTeachCtrl Lift1ZUpInspecTeach = new AppAxisTeachCtrl("Lift1Z Up Insp Pos", _lift1ZAxis);
            Lift1ZUpInspecTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZUpInspecPos);
            Lift1ZUpInspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZUpInspecTeach);

            AppAxisTeachCtrl Lift1ZDnStepInspecTeach = new AppAxisTeachCtrl("Lift1Z Dn Insp Pos", _lift1ZAxis);
            Lift1ZDnStepInspecTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZDnStepInspecPos);
            Lift1ZDnStepInspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZDnStepInspecTeach);

            AppAxisTeachCtrl Lift1ZDnTeach = new AppAxisTeachCtrl("Lift1Z Dn Pos", _lift1ZAxis);
            Lift1ZDnTeach.RegisterProperty(() => AppCommonParam.This.Lift1ZDnPos);
            Lift1ZDnTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift1TeachValueChange);
            flpLift1ZTeach.Controls.Add(Lift1ZDnTeach);
            
            
            #endregion

            #region Add Lift2 Z Teach
           
            AppAxisTeachCtrl Lift2ZPreUpRejectTeach = new AppAxisTeachCtrl("Lift2Z Pre-Up Rej Pos", _lift2ZAxis);
            Lift2ZPreUpRejectTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZPreUpRejectPos);
            Lift2ZPreUpRejectTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZPreUpRejectTeach);

            AppAxisTeachCtrl Lift2ZUpRejectTeach = new AppAxisTeachCtrl("Lift2Z Up Rej Pos", _lift2ZAxis);
            Lift2ZUpRejectTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZUpRejectPos);
            Lift2ZUpRejectTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZUpRejectTeach);

            AppAxisTeachCtrl Lift2ZDnStepRejectTeach = new AppAxisTeachCtrl("Lift2Z Dn Rej Inc Pos", _lift2ZAxis);
            Lift2ZDnStepRejectTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZDnStepRejectPos);
            Lift2ZDnStepRejectTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZDnStepRejectTeach);

            AppAxisTeachCtrl Lift2ZPreUpEmptyTeach = new AppAxisTeachCtrl("Lift2Z Pre-Up Inc Pos", _lift2ZAxis);
            Lift2ZPreUpEmptyTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZPreUpEmptyPos);
            Lift2ZPreUpEmptyTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZPreUpEmptyTeach);

            AppAxisTeachCtrl Lift2ZUpEmptyTeach = new AppAxisTeachCtrl("Lift2Z Up Inc Pos", _lift2ZAxis);
            Lift2ZUpEmptyTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZUpEmptyPos);
            Lift2ZUpEmptyTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZUpEmptyTeach);

            AppAxisTeachCtrl Lift2ZDnStepEmptyTeach = new AppAxisTeachCtrl("Lift2Z Dn Step Inc Pos", _lift2ZAxis);
            Lift2ZDnStepEmptyTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZDnStepEmptyPos);
            Lift2ZDnStepEmptyTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZDnStepEmptyTeach);

            AppAxisTeachCtrl Lift2ZPreUpInspecTeach = new AppAxisTeachCtrl("Lift2Z Pre-Up Insp Pos", _lift2ZAxis);
            Lift2ZPreUpInspecTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZPreUpInspecPos);
            Lift2ZPreUpInspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZPreUpInspecTeach);

            AppAxisTeachCtrl Lift2ZUpInspecTeach = new AppAxisTeachCtrl("Lift2Z Up Insp Pos", _lift2ZAxis);
            Lift2ZUpInspecTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZUpInspecPos);
            Lift2ZUpInspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZUpInspecTeach);

            AppAxisTeachCtrl Lift2ZDnStepInspecTeach = new AppAxisTeachCtrl("Lift2Z Dn Insp Pos", _lift2ZAxis);
            Lift2ZDnStepInspecTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZDnStepInspecPos);
            Lift2ZDnStepInspecTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZDnStepInspecTeach);

            AppAxisTeachCtrl Lift2ZDnTeach = new AppAxisTeachCtrl("Lift2Z Dn Pos", _lift2ZAxis);
            Lift2ZDnTeach.RegisterProperty(() => AppCommonParam.This.Lift2ZDnPos);
            Lift2ZDnTeach.evOnTeached += new AppAxisTeachCtrl.DelParamMillimeters(OnLift2TeachValueChange);
            flpLift2ZTeach.Controls.Add(Lift2ZDnTeach);

            #endregion
            */



            this.Update();
        }



        private void OnFeedYTeachValueChange(MDouble.Millimeters teachPosition)
        {
            pgFeedY.Refresh();
        }

        private void OnLift1TeachValueChange(MDouble.Millimeters teachPosition)
        {
            pgLift1.Refresh();
        }

        private void OnLift2TeachValueChange(MDouble.Millimeters teachPosition)
        {
            pgLift2.Refresh();
        }
    }
}
