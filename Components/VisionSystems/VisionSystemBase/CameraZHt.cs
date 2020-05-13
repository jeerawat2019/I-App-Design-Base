using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using MDouble;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;

namespace MCore.Comp.VisionSystem
{
    public class CameraZHt : CameraBase
    {
        private Array2DInput _refZHtSensor = null;
        /// <summary>
        /// The nickname of the ZHtSensor
        /// </summary>        
        [Browsable(true)]
        [Category("ZHtCamera")]
        [Description("The nickname of the ZHtSensor")]
        public string ZHtSensorID
        {
            get { return GetPropValue(() => ZHtSensorID); }
            set { SetPropValue(() => ZHtSensorID, value); }
        }

        /// <summary>
        /// Get the reference to the Array2DInput Zht sensor
        /// </summary>
        public Array2DInput RefZHtSensor
        {
            get { return _refZHtSensor; }
        }

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CameraZHt()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CameraZHt(string name)
            : base(name)
        {
        }
        #endregion Constructors


        #region Overrides

        /// <summary>
        /// Opportunity to do any ID referencing for this class object
        /// Occurs after Initialize
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            _refZHtSensor = U.GetComponent(ZHtSensorID) as Array2DInput;
            U.RegisterOnChanged(() => TriggerMode, OnTriggerModeChanged);
        }

        /// <summary>
        /// Manual Triggerring
        /// </summary>
        /// <param name="manualLive"></param>
        /// <returns></returns>
        public override object Acquire(bool manualLive)
        {
            _refZHtSensor.Measure();
            Bitmap bm = _refZHtSensor.BuildBitmap(double.NaN, double.NaN);
            return ConvertToThirdPartyImage(bm);
        }

        /// <summary>
        /// ConvertToThirdPartyImage
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public virtual object ConvertToThirdPartyImage(Bitmap bm)
        {
            return bm;
        }
        ///// <summary>
        ///// Do the measure, trigger and wait for result
        ///// </summary>
        ///// <param name="timeout"></param>
        //public override bool Measure()
        //{
        //    bool bRet = _refZHtSensor.Measure();

        //    // Convert Data array to Rough Bitmap

        //    // Determine rough zHt of Reference plane and peaks and locations

        //    // Convert Data array to Fine Bitmap

        //    // Determine Fine zHt of Reference plane and peaks (blind, using 'rough' locations)

        //    TriggerMode = eTriggerMode.Idle;
        //    return bRet;
        //}

        #endregion Overrides

        private void OnTriggerModeChanged(eTriggerMode triggerMode)
        {
            switch (triggerMode)
            {
                case eTriggerMode.SingleTrigger:
                    Measure();
                    break;
            }
        }


    }
}
