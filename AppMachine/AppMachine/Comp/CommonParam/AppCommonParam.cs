using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

using MCore;
using MCore.Comp;


namespace AppMachine.Comp.CommonParam
{
    public class AppCommonParam : CompBase
    {
        public static AppCommonParam This = null;

        #region Constructor
        public AppCommonParam()
        {
          
        }

        public AppCommonParam(String name):base(name)
        {

        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
            This = this;
        }


        //Common Prarmeter Property
        /*Put Common Parameter Property Here (Example in bleow)

        #region Feeder Property
        [Category("Feeder Motion"), Browsable(true)]
        public MDouble.Millimeters FeedYLift1IncomePos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYLift1IncomePos); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYLift1IncomePos, value); }
        }

      
        [Category("Feeder Motion"), Browsable(true),]
        public MDouble.Millimeters FeedYLift1RejectPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYLift1RejectPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYLift1RejectPos, value); }
        }


      

        
        [Category("Feeder Motion"), Browsable(true)]
        public MDouble.Millimeters FeedYLift1InspecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYLift1InspecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYLift1InspecPos, value); }
        }


        [Category("Feeder Motion"), Browsable(true)]
        public MDouble.Millimeters FeedYLift2RejectPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYLift2RejectPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYLift2RejectPos, value); }
        }



        [Category("Feeder Motion"), Browsable(true)]
        public MDouble.Millimeters FeedYLift2EmptyPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYLift2EmptyPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYLift2EmptyPos, value); }
        }

     
        [Category("Feeder Motion"), Browsable(true)]
        public MDouble.Millimeters FeedYLift2InspecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYLift2InspecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYLift2InspecPos, value); }
        }

    
        [Category("Feeder Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond FeedYSpeedLow
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYSpeedLow, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYSpeedLow, value); }
        }


        [Category("Feeder Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond FeedYSpeedMiddle
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYSpeedMiddle, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYSpeedMiddle, value); }
        }


        [Category("Feeder Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond FeedYSpeedHigh
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FeedYSpeedHigh, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => FeedYSpeedHigh, value); }
        }

        #endregion

        #region Lift1

        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZPreUpIncomePos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZPreUpIncomePos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZPreUpIncomePos, value); }
        }


        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZUpIncomePos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZUpIncomePos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZUpIncomePos, value); }
        }


        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZDnStepIncomePos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZDnStepIncomePos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZDnStepIncomePos, value); }
        }


        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZPreUpRejectPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZPreUpRejectPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZPreUpRejectPos, value); }
        }


        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZUpRejectPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZUpRejectPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZUpRejectPos, value); }
        }


        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZDnStepRejectPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZDnStepRejectPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZDnStepRejectPos, value); }
        }

        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZPreUpInspecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZPreUpInspecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZPreUpInspecPos, value); }
        }

        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZUpInspecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZUpInspecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZUpInspecPos, value); }
        }

        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZDnStepInspecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZDnStepInspecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZDnStepInspecPos, value); }
        }


        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift1ZDnPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZDnPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZDnPos, value); }
        }

        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond Lift1ZSpeedLow
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZSpeedLow, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZSpeedLow, value); }
        }

        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond Lift1ZSpeedMiddle
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZSpeedMiddle, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZSpeedMiddle, value); }
        }


        [Category("Lift1 Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond Lift1ZSpeedHigh
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift1ZSpeedHigh, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift1ZSpeedHigh, value); }
        }

        #endregion

        #region Lift2

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZPreUpRejectPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZPreUpRejectPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZPreUpRejectPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZUpRejectPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZUpRejectPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZUpRejectPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZDnStepRejectPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZDnStepRejectPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZDnStepRejectPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZPreUpEmptyPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZPreUpEmptyPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZPreUpEmptyPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZUpEmptyPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZUpEmptyPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZUpEmptyPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZDnStepEmptyPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZDnStepEmptyPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZDnStepEmptyPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZPreUpInspecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZPreUpInspecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZPreUpInspecPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZUpInspecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZUpInspecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZUpInspecPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZDnStepInspecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZDnStepInspecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZDnStepInspecPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.Millimeters Lift2ZDnPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZDnPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZDnPos, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond Lift2ZSpeedLow
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZSpeedLow, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZSpeedLow, value); }
        }


        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond Lift2ZSpeedMiddle
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZSpeedMiddle, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZSpeedMiddle, value); }
        }

        [Category("Lift2 Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond Lift2ZSpeedHigh
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Lift2ZSpeedHigh, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => Lift2ZSpeedHigh, value); }
        }
        #endregion

        #region Vision Property
     

        [Category("Vision Motion"), Browsable(true)]
        public MDouble.Millimeters VisionXInpecPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => VisionXInpecPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => VisionXInpecPos, value); }
        }


        [Category("Vision Motion"), Browsable(true)]
        public MDouble.Millimeters VisionXIndexShiftPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => VisionXIndexShiftPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => VisionXIndexShiftPos, value); }
        }

        [Category("Vision Motion"), Browsable(true)]
        public MDouble.Millimeters VisionXStandbyPos
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => VisionXStandbyPos); }
            [StateMachineEnabled]
            set { SetPropValue(() => VisionXStandbyPos, value); }
        }

        [Category("Vision Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond VisionXSpeedLow
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => VisionXSpeedLow, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => VisionXSpeedLow, value); }
        }

        [Category("Vision Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond VisionXSpeedMiddle
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => VisionXSpeedMiddle, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => VisionXSpeedMiddle, value); }
        }

        [Category("Vision Motion"), Browsable(true)]
        public MDouble.MillimetersPerSecond VisionXSpeedHigh
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => VisionXSpeedHigh, 5); }
            [StateMachineEnabled]
            set { SetPropValue(() => VisionXSpeedHigh, value); }
        }
        #endregion
        */
      
    }
}
