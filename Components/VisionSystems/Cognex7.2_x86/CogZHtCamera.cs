using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageProcessing;

using MDouble;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;

namespace MCore.Comp.VisionSystem
{
    public class CogZHtCamera : CameraZHt
    {


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CogZHtCamera()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CogZHtCamera(string name)
            : base(name)
        {
        }
        #endregion Constructors


        #region Overrides

        /// <summary>
        /// ConvertToThirdPartyImage
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public override object ConvertToThirdPartyImage(Bitmap bm)
        {
            return new CogImage8Grey(bm);
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
