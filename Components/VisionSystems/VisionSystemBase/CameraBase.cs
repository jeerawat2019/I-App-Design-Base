using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

using MCore.Helpers;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using MDouble;

namespace MCore.Comp.VisionSystem
{

    public partial class CameraBase : CompMeasure
    {
        private TriggerQueue<D.ObjectEventHandler> _triggerQue = null;
        private MiliSecOutput _msOutput = null;

        #region Public Properties
        /// <summary>
        /// The trigger queue
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public TriggerQueue<D.ObjectEventHandler> TriggerQue
        {
            get { return _triggerQue; }
            set { _triggerQue = value; }
        }

        /// <summary>
        /// The Id of the camera
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public string CameraID
        {
            get { return GetPropValue(() => CameraID, string.Empty); }
            set { SetPropValue(() => CameraID, value); }
        }

        /// <summary>
        /// The Strobe Output
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        [XmlIgnore]
        public MiliSecOutput StrobeOutput
        {
            get { return _msOutput; }
            set { _msOutput = value; }
        }

        /// <summary>
        /// The Name of the camera
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public virtual string CameraName
        {
            get { return "Unknown"; }
        }

        /// <summary>
        /// The port description of the camera
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public virtual string PortDescription
        {
            get { return "Unknown"; }
        }

        /// <summary>
        /// The SerialNumber of the camera
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public virtual string SerialNumber
        {
            get { return string.Empty; }
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Camera")]
        public MDoubleNoUnits Brightness
        {
            get { return GetPropValue(() => Brightness, new MDoubleNoUnits(0.5)); }
            set { SetPropValue(() => Brightness, value); }
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Camera")]
        public MDoubleNoUnits Contrast
        {
            get { return GetPropValue(() => Contrast, new MDoubleNoUnits(0.5)); }
            set { SetPropValue(() => Contrast, value); }
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Camera")]
        public Miliseconds Exposure
        {
            set { SetPropValue(() => Exposure, value); }
            get { return GetPropValue(() => Exposure, 35); }
        }

        /// <summary>
        /// The video Format
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public string VideoFormat
        {
            get { return GetPropValue(() => VideoFormat, string.Empty); }
            set { SetPropValue(() => VideoFormat, value); }
        }
        /// <summary>
        /// Show Image In Windows
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public bool ShowImageInWindows
        {
            get { return GetPropValue(() => ShowImageInWindows); }
            set { SetPropValue(() => ShowImageInWindows, value); }
        }
        
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Camera")]
        [XmlIgnore]
        public virtual bool ValidCamera
        {
            get { return true; }
        }


        /// <summary>
        /// Rotate the image at Acquisition
        /// </summary>
        [Category("Camera"), Browsable(true), Description("Rotate the image at Acquisition\n0,90,-90,180,270")]
        public int RotateImage
        {
            get { return GetPropValue(() => RotateImage); }
            set { SetPropValue(() => RotateImage, value); }
        }
        

        #endregion Public Properties


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CameraBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CameraBase(string name)
            : base(name)
        {
        }
        #endregion Constructors

        #region Other Public functions

        /// <summary>
        /// Get the next callback
        /// </summary>
        /// <returns></returns>
        protected D.ObjectEventHandler GetNextCallback()
        {
            if (TriggerQue != null)
            {
                return TriggerQue.GetNextCallback();
            }
            return null;
        }

        #endregion Other Public functions

        #region Public Calls to do service

        private const string CAMERAWINDOWNAME = "CameraWindow";


        public virtual void RegisterCameraWindow(Control parentWindow)
        {
            Label label = new Label();
            label.Name = CAMERAWINDOWNAME;
            label.Text = parentWindow.Name + " Simulated";
            label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            label.Size = parentWindow.Size;
            parentWindow.Controls.Add(label);
        }

        public virtual void UnregisterCameraWindow(Control parentWindow)
        {
            parentWindow.Controls.Clear();
        }

        public virtual void CrossHairs(Control parentWindow, bool crossHairs)
        {
        }

        /// <summary>
        /// Get the Marker offset
        /// </summary>
        /// <param name="parentCtl"></param>
        public virtual PointF MarkOffset(Control parentCtl)
        {
            return PointF.Empty;
        }

        /// <summary>
        /// Return the crosshairs to middle position
        /// </summary>
        /// <param name="parentCtl"></param>
        public virtual void ResetCrosshairs(Control parentCtl)
        {
        }
        /// <summary>
        /// Convert the image produced by the camera to a Bitmap
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public virtual Bitmap ImageToBitmap(object oImage)
        {
            return oImage as Bitmap;
        }

        public virtual string[] GetCameraIDs()
        {

            return new string[] { "Camera one", "Camera two" };
        }
        /// <summary>
        /// Obtain a list of Video Formats. 
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetVideoFormats()
        {
            return new string[] { "Format one", "Format two" };
        }

        public virtual void InitializeFrameGrabber()
        {
        }

        public virtual void SetStrobeDuration(Miliseconds duration)
        {
            if (StrobeOutput != null)
            {
                StrobeOutput.Set(duration);
            }
        }
        public virtual void ResendStrobeDuration()
        {
            if (StrobeOutput != null)
            {
                StrobeOutput.Resend();
            }
        }

        /// <summary>
        /// Manual Triggerring
        /// </summary>
        /// <param name="manualLive"></param>
        /// <returns></returns>
        public virtual object Acquire(bool manualLive)
        {
            return null;
        }

        #endregion Public Calls to do service
    }
}
