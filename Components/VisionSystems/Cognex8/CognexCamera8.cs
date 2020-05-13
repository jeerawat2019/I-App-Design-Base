using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageProcessing;

using MDouble;
using MCore.Comp.IOSystem;
using System.Threading.Tasks;

namespace MCore.Comp.VisionSystem
{
    class CrossHairsControl : Control
    {
        CogDisplay _cogDisplay = null;
        bool _validCam = false;
        public bool ShowCrossHairs
        {
            get;
            set;
        }
        public CrossHairsControl(CogDisplay cogDisplay, bool validCamera)
        {
            _cogDisplay = cogDisplay;
            _validCam = validCamera;
            Location = cogDisplay.Location;
            Size = cogDisplay.Size;
            TabIndex = 1;
            ResetCrossHairs();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;  // WS_EX_TRANSPARENT
                return cp;
            }
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);

            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTTRANSPARENT;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //if (!_validCam)
            //{
            //    RectangleF drawRect = new RectangleF(Point.Empty, Size);
            //    e.Graphics.DrawString("Camera not valid", Font, Brushes.Red, drawRect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            //}
            if (ShowCrossHairs)
            {
                float x = CenterLoc.X + (float)(_cogDisplay.Zoom * (_cogDisplay.PanX + _markerOffset.X));
                e.Graphics.DrawLine(Pens.Pink, x, 0, x, _cogDisplay.ClientSize.Height);
                float y = CenterLoc.Y + (float)(_cogDisplay.Zoom * (_cogDisplay.PanY + _markerOffset.Y));
                e.Graphics.DrawLine(Pens.Pink, 0, y, _cogDisplay.ClientSize.Width, y);
            }
        }

        private PointF CenterLoc
        {
            get
            {
                return new PointF((float)_cogDisplay.ClientSize.Width / 2f, (float)_cogDisplay.ClientSize.Height / 2f);
            }
        }

        public void ResetCrossHairs()
        {
            _markerOffset = PointF.Empty;
            _cogDisplay.Invalidate();
            Invalidate();
        }

        PointF _markerOffset = PointF.Empty;
        public PointF MarkerOffset
        {
            get
            {
                return _markerOffset;
                /*
                float scale = 1f;
                switch (_cogDisplay.ScalingMethod)
                {
                    case CogDisplayScalingMethodConstants.Integer:                        
                        break;
                    case CogDisplayScalingMethodConstants.Continuous:
                        scale = (float)_cogDisplay.Zoom;
                        break;
                    case CogDisplayScalingMethodConstants.ContinuousBilinear:
                        scale = (float)_cogDisplay.Zoom;
                        break;
                }
                return new PointF((_markerOffset.X - CenterLoc.X)/scale, (_markerOffset.Y - CenterLoc.Y)/scale);
                */
            }
        }

        public void CogMouseDown(MouseEventArgs e)
        {
            if (ShowCrossHairs & e.Button == MouseButtons.Left && _cogDisplay.MouseMode == CogDisplayMouseModeConstants.Pointer)
            {
                _markerOffset.X = (float)(((float)e.Location.X - CenterLoc.X) / _cogDisplay.Zoom - _cogDisplay.PanX);
                _markerOffset.Y = (float)(((float)e.Location.Y - CenterLoc.Y) / _cogDisplay.Zoom - _cogDisplay.PanY);
                _cogDisplay.Invalidate();
                Invalidate();
            }
        }
    }
    public class CognexCamera8 : CameraBase
    {
        private ICogAcqFifo _cogAcqFifo = null;
        private ICogFrameGrabber _frameGrabber = null;
        private List<CogDisplay> _cogDisplayWindows = new List<CogDisplay>();
        private CogCompleteEventHandler _delCogCompleteHandler = null;
        //private volatile bool _isCogCompleteHandler = false;


        //private string _simFileName;

        //[System.ComponentModel.Editor(
        //typeof(System.Windows.Forms.Design.FileNameEditor),
        //typeof(System.Drawing.Design.UITypeEditor))]
        //[Browsable(true)]
        //[Category("Camera")]
        //public string SimFileName
        //{

        //    get { return this._simFileName; }

        //    set { this._simFileName = value; }

        //}

        [Browsable(false)]
        [XmlIgnore]
        public List<CogDisplay> CogDisplayWindows
        {
            get { return _cogDisplayWindows; }
            set { _cogDisplayWindows = value; }
        }

        private const string COGNEXWINDOWNAME = "CognexWindow";
        private const string CROSSHAIRSCTLNAME = "CrossHairs";

        private Object _lockAcqFifo = new object();
        private Object _lockCogDisplay = new object();

        [Browsable(false)]
        [XmlIgnore]
        public Object LockCogDisplay
        {
            get { return _lockCogDisplay; }
            set { _lockCogDisplay = value; }
        }

        /// <summary>
        /// The Name of the camera
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public override string CameraName
        {
            get 
            {
                if (_frameGrabber != null)
                {
                    return _frameGrabber.Name;
                }
                return base.CameraName; 
            }
        }

        /// <summary>
        /// The SerialNumber of the camera
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public override string SerialNumber
        {
            get
            {
                if (_frameGrabber != null)
                {
                    return _frameGrabber.SerialNumber;
                }
                return base.SerialNumber;
            }
        }

        /// <summary>
        /// The SerialNumber of the camera
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public override string PortDescription
        {
            get
            {
                if (_frameGrabber != null)
                {
                    ICogGigEAccess iCogGig = _frameGrabber.OwnedGigEAccess;
                    if (iCogGig != null)
                    {
                        return iCogGig.CurrentIPAddress;
                    }
                }
                return base.SerialNumber;
            }
        }


        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Camera")]
        [XmlIgnore]
        public override bool ValidCamera
        {
            get { return _frameGrabber != null; }
        }

        private Seconds _acquireTime = new Seconds();
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Camera")]
        public Seconds AcquireTime
        {
            get { return _acquireTime; }
            set 
            {
                _acquireTime = value;
            }
        }

        [Browsable(true)]
        [Category("Camera")]
        [Description("Tppe of Bayer camera\n" +
            "  -1: Not a Bayer Camera\n"+
            "   0: GR\n" +
            "   1: RG\n" +
            "   2: BG\n" +
            "   3: GB\n")]
        public int BayerType
        {
            get { return GetPropValue(() => BayerType, 3); }
            set { SetPropValue(() => BayerType, value); }
        }

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CognexCamera8()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CognexCamera8(string name)
            : base(name)
        {
        }
        #endregion Constructors

        #region Overrides
        /// <summary>
        /// Initialize this Component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _delCogCompleteHandler = new CogCompleteEventHandler(CogAcqFifo_Complete);

            IgnorePageList.Add(typeof(CameraBaseCtl));
            IgnorePageList.Add(typeof(CamPanel));

            RegisterOnChanged(() => Brightness, OnChangedBrightness);
            RegisterOnChanged(() => Contrast, OnChangedContrast);
            RegisterOnChanged(() => Exposure, OnChangedExposure);
            RegisterOnChanged(() => VideoFormat,OnChangeVideoFormat);
            RegisterOnChanged(() => CameraID, OnChangeCameraID);
            RegisterOnChanged(() => TriggerMode, OnChangedTriggerMode);
            
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }

            try
            {
                // This also assigns the frameGrabber if match is found
                GetCameraIDs();
                GetVideoFormats();
                InitializeFrameGrabber();

                OnChangedBrightness(Brightness);
                OnChangedContrast(Contrast);
                OnChangedExposure(Exposure);

                Simulate = eSimulate.None;
            }
            catch (ForceSimulateException fsex)
            {
                throw fsex;
            }
            catch (Exception ex)
            {
                throw new ForceSimulateException(ex);
            }
        }

        public override void PreDestroy()
        {
            base.PreDestroy();
        }

        /// <summary>
        /// Destroy this Component
        /// </suimmary>
        public override void Destroy()
        {
            base.Destroy();
            StopLiveAllCameraWindows();
            lock (LockCogDisplay)
            {
                _cogDisplayWindows.Clear();
            }
            if (_frameGrabber != null)
            {
                lock (_lockAcqFifo)
                {
                    if (_cogAcqFifo != null)
                    {
                        _cogAcqFifo.Flush();
                    }
                }
                _frameGrabber.Disconnect(false);
                _frameGrabber = null;
            }
        }


        public override void RegisterCameraWindow(Control parentWindow)
        {
           // return;
            lock (LockCogDisplay)
            {
                CogDisplay cogDisplay = new Cognex.VisionPro.Display.CogDisplay();
                ((System.ComponentModel.ISupportInitialize)(cogDisplay)).BeginInit();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraBaseCtl));
                // 
                // cogDisplay
                // 
                cogDisplay.Location = new System.Drawing.Point(0, 0);
                cogDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
                cogDisplay.MouseWheelSensitivity = 1D;
                cogDisplay.Name = COGNEXWINDOWNAME;
                cogDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay.OcxState")));
                cogDisplay.Size = parentWindow.Size;
                cogDisplay.Anchor = parentWindow.Anchor;
                cogDisplay.TabIndex = 1;
                cogDisplay.ClientSizeChanged += new EventHandler(CogDisplay_ClientSizeChanged);
                cogDisplay.Changed += new CogChangedEventHandler(CogDisplay_Changed);
                cogDisplay.MouseDown += new MouseEventHandler(CogDisplay_MouseDown);
                parentWindow.Controls.Clear();
                parentWindow.Controls.Add(cogDisplay);
                ((System.ComponentModel.ISupportInitialize)(cogDisplay)).EndInit();
                if (_cogDisplayWindows.Count > 0)
                {
                    cogDisplay.Image = _cogDisplayWindows[0].Image;
                }
                _cogDisplayWindows.Add(cogDisplay);
                cogDisplay.AutoFit = true;

                if (!this.ValidCamera)
                {
                    try
                    {
                        Bitmap bm = MCore.Comp.VisionSystem.Properties.Resources.InvalidCamera;
                        cogDisplay.Image = new CogImage24PlanarColor(bm);
                    }
                    catch
                    {
                    }

                    
                }

                CrossHairsControl crossHairsCtl = new CrossHairsControl(cogDisplay, ValidCamera) { Name = CROSSHAIRSCTLNAME };
                parentWindow.Controls.Add(crossHairsCtl);
                crossHairsCtl.BringToFront();
                cogDisplay.Tag = crossHairsCtl; 
            }
        }

        private void CogDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            CogDisplay cogDisplay = sender as CogDisplay;
            if (cogDisplay != null)
            {
                CrossHairsControl crossHairsCtl = cogDisplay.Tag as CrossHairsControl;
                if (crossHairsCtl != null)
                {
                    crossHairsCtl.CogMouseDown(e);
                }
            }
        }

        //CrossHairsControl _crossHairsCtl = null;

        /// <summary>
        /// Get the Marker offset
        /// </summary>
        /// <param name="parentCtl"></param>
        public override PointF MarkOffset(Control parentCtl)
        {
            CrossHairsControl crossHairsCtl = GetCrossHairsCtl(parentCtl);
            if (crossHairsCtl == null)
            {
                return PointF.Empty;
            }            
            return crossHairsCtl.MarkerOffset;
        }
        /// <summary>
        /// Return the crosshairs to middle position
        /// </summary>
        public override void ResetCrosshairs(Control parentCtl)
        {
            CrossHairsControl crossHairsCtl = GetCrossHairsCtl(parentCtl);
            if (crossHairsCtl != null)
            {
                crossHairsCtl.ResetCrossHairs();
            }
        }
        private CrossHairsControl GetCrossHairsCtl(Control parentWindow)
        {
            if (parentWindow.Controls.ContainsKey(CROSSHAIRSCTLNAME))
            {
                return parentWindow.Controls[CROSSHAIRSCTLNAME] as CrossHairsControl;
            }
            return null;
        }

        public override void CrossHairs(Control parentWindow, bool showCrossHairs)
        {
            CrossHairsControl crossHairsCtl = GetCrossHairsCtl(parentWindow);
            if (crossHairsCtl != null)
            {
                crossHairsCtl.ShowCrossHairs = showCrossHairs;
                crossHairsCtl.ResetCrossHairs();
            }
        }

        void CogDisplay_ClientSizeChanged(object sender, EventArgs e)
        {
            CogDisplay cog = (sender as CogDisplay);
            if (cog.Parent.Controls.ContainsKey(CROSSHAIRSCTLNAME))
            {
                cog.Parent.Controls[CROSSHAIRSCTLNAME].Size = cog.Size;
                cog.Parent.Controls[CROSSHAIRSCTLNAME].Invalidate();
            }

        }

        void CogDisplay_Changed(object sender, CogChangedEventArgs e)
        {
            CogDisplay cog = (sender as CogDisplay);
            if (cog.Parent.Controls.ContainsKey(CROSSHAIRSCTLNAME))
            {
                cog.Parent.Controls[CROSSHAIRSCTLNAME].Invalidate();
            }
        }

        public override void UnregisterCameraWindow(Control parentWindow)
        {
            lock (LockCogDisplay)
            {
                CogDisplay cogDisplay = GetCogDisplay(parentWindow);
                if (cogDisplay != null && _cogDisplayWindows.Contains(cogDisplay))
                {
                    _cogDisplayWindows.Remove(cogDisplay);
                }
            }
        }

        //void CogDisplay_Paint(object sender, PaintEventArgs e)
        //{
        //    e.Graphics.DrawLine(Pens.LightPink, 0, 0, 110, 110);
        //}

        //void OnCogDisplay_Changed(object sender, CogChangedEventArgs e)
        //{
            
        //    (sender as CogDisplay).
        //}

        private CogDisplay GetCogDisplay(Control parentWindow)
        {
            CogDisplay cogDisplay = parentWindow.Controls.ContainsKey(COGNEXWINDOWNAME) ? parentWindow.Controls[COGNEXWINDOWNAME] as CogDisplay : null;
            if (cogDisplay != null && _cogDisplayWindows.Contains(cogDisplay))
            {
                return cogDisplay;
            }
            return null;
        }

        private delegate void _delParamTriggerMode(CompMeasure.eTriggerMode triggerMode);
        private void OnChangedTriggerMode(CompMeasure.eTriggerMode triggerMode)
        {
            if (U.GetDummyControl().InvokeRequired)
            {
                U.GetDummyControl().BeginInvoke(new _delParamTriggerMode(OnChangedTriggerMode), new object[] { triggerMode });
                return;
            }
            switch (triggerMode)
            {
                case CompMeasure.eTriggerMode.Idle:
                    StopMeasureLoop();
                    MakeLiveAllVisibleWindows(false);
                    break;
                case CompMeasure.eTriggerMode.SingleTrigger:
                    StopMeasureLoop();
                    MakeLiveAllVisibleWindows(false);
                    Acquire(false);
                    TriggerMode = eTriggerMode.Idle;
                    break;
                case CompMeasure.eTriggerMode.TimedTrigger:
                    MakeLiveAllVisibleWindows(false);
                    StartMeasureLoop(DoManualLiveAcquire);
                    break;
                case CompMeasure.eTriggerMode.Live:
                    if (ExternalTrigger)
                    {
                        StartMeasureLoop(DoManualLiveAcquire);
                    }
                    else
                    {
                        MakeLiveAllVisibleWindows(true);
                    }
                    break;
            }
        }

        //private void MakeExternalTrigger(bool extTrigger)
        //{
        //    if (extTrigger)
        //    {
        //        if (!_isCogCompleteHandler)
        //        {
        //            _cogAcqFifo.Complete += _delCogCompleteHandler;
        //            _isCogCompleteHandler = true;
        //        }
        //    }
        //    else
        //    {
        //        if (_isCogCompleteHandler)
        //        {
        //            _cogAcqFifo.Complete -= _delCogCompleteHandler;
        //            _isCogCompleteHandler = false;
        //        }
        //    }
            
        //    if (_cogAcqFifo != null)
        //    {
        //        lock (_lockAcqFifo)
        //        {
        //            //ICogAcqTrigger mTrigger = _cogAcqFifo.OwnedTriggerParams;
        //            //if (mTrigger != null)
        //            //{
        //            //   // mTrigger.TriggerEnabled = extTrigger;
        //            //    if (extTrigger)
        //            //    {
        //            //        mTrigger.TriggerModel = CogAcqTriggerModelConstants.Semi;
        //            //        mTrigger.TriggerLowToHigh = false;
        //            //    }
        //            //}
        //        }
        //    }
        //}

        private void MakeLiveAllVisibleWindows(bool bLive)
        {
            lock (LockCogDisplay)
            {
                if (_cogAcqFifo == null)
                {
                    return;
                }
                foreach (CogDisplay cogDisplay in _cogDisplayWindows)
                {
                    bool makeThisOneLive = bLive;
                    if (!cogDisplay.Visible)
                    {
                        makeThisOneLive = false;
                    }
                    if (makeThisOneLive)
                    {
                        if (!cogDisplay.LiveDisplayRunning)
                        {
                            lock (_lockAcqFifo)
                            {
                                cogDisplay.StartLiveDisplay(_cogAcqFifo, false);
                            }
                            cogDisplay.AutoFit = true;
                        }
                    }
                    else
                    {
                        if (cogDisplay.LiveDisplayRunning)
                            StopLiveDisplay(cogDisplay);
                    }
                }
            }
        }

        private string BuildCameraID(ICogFrameGrabber frameGrabber)
        {
            return string.Format("{0}-{1}", frameGrabber.Name, frameGrabber.SerialNumber);
        }

        /// <summary>
        /// Obtain a list of camera.  Also assigns the frame grabber if it matches the ID
        /// </summary>
        /// <returns></returns>
        public override string[] GetCameraIDs()
        {
            List<string> list = new List<string>();
            ICogFrameGrabber foundGrabber = null;
            if (Parent is Cognex8)
            {
                lock (_lockAcqFifo)
                {
                    CogFrameGrabbers grabbers = (Parent as Cognex8).FramegrabberCams;
                    if (grabbers != null)
                    {
                        foreach (ICogFrameGrabber grabber in grabbers)
                        {
                            list.Add(BuildCameraID(grabber));
                            if (BuildCameraID(grabber) == CameraID)
                            {
                                foundGrabber = grabber;
                            }
                        }
                    }
                }
                _frameGrabber = foundGrabber;
                if (_frameGrabber == null)
                {
                    U.LogError("Camera not found! : '{0}'", CameraID);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// Obtain a list of Video Formats. 
        /// </summary>
        /// <returns></returns>
        public override string[] GetVideoFormats()
        {
            if (_frameGrabber == null)
            {
                return new string[0];
            }
            string[] retArray = null;
            try
            {
                lock (_lockAcqFifo)
                {
                    retArray = new string[_frameGrabber.AvailableVideoFormats.Count];
                    _frameGrabber.AvailableVideoFormats.CopyTo(retArray, 0);
                    if (_frameGrabber.AvailableVideoFormats.IndexOf(VideoFormat) == -1)
                    {
                        VideoFormat = _frameGrabber.AvailableVideoFormats[0];
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogError(ex, "Unable to get available Viideo formates for {0}", Name);
                retArray = new string[] { ex.Message };
            }

            return retArray;
        }
        public override void InitializeFrameGrabber()
        {
            if (_frameGrabber != null)
            {
                if (string.IsNullOrEmpty(VideoFormat))
                {
                    return;
                }
                // Confirm video format
                lock (_lockAcqFifo)
                {
                    try
                    {
                        if (_cogAcqFifo != null)
                        {
                            _cogAcqFifo.Flush();
                        }

                        bool b16FGrey = _frameGrabber.GetSupportsPixelFormat(CogAcqFifoPixelFormatConstants.Format16Grey);
                        bool b32RGB = _frameGrabber.GetSupportsPixelFormat(CogAcqFifoPixelFormatConstants.Format32RGB);
                        bool bFormat3Plane = _frameGrabber.GetSupportsPixelFormat(CogAcqFifoPixelFormatConstants.Format3Plane);
                        bool bFormat565RGB = _frameGrabber.GetSupportsPixelFormat(CogAcqFifoPixelFormatConstants.Format565RGB);

                        _cogAcqFifo = _frameGrabber.CreateAcqFifo(VideoFormat, CogAcqFifoPixelFormatConstants.Format8Grey, 0, false);
                        //_cogAcqFifo.OutputPixelFormat = CogImagePixelFormatConstants.PlanarRGB8;
                        _cogAcqFifo.Prepare();
                        _cogAcqFifo.Flush();
                        ICogAcqTrigger mTrigger = _cogAcqFifo.OwnedTriggerParams;
                        if (mTrigger != null && ExternalTrigger)
                        {
                            mTrigger.TriggerModel = CogAcqTriggerModelConstants.Auto;
                            mTrigger.TriggerLowToHigh = false;
                            _cogAcqFifo.Complete += _delCogCompleteHandler;
                        }

                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex.Message);
                    }
                }
                OnChangedTriggerMode(TriggerMode);
            }
        }
        /// <summary>
        /// Convert the image produced by the camera to a Bitmap
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public override Bitmap ImageToBitmap(object oImage)
        {
            return CognexJob8.CogImageToBitmap(oImage);
        }

        #endregion Overrides

        /// <summary>
        /// Manual Triggerring
        /// </summary>
        /// <returns></returns>
        public void DoManualLiveAcquire()
        {
            Acquire(true);
        }

        private ManualResetEvent _waitForImage = new ManualResetEvent(false);
        /// <summary>
        /// Manual Triggerring
        /// </summary>
        /// <param name="manualLive"></param>
        /// <returns></returns>
        public override object Acquire(bool manualLive)
        {
            if (_cogAcqFifo == null)
            {
                //if (SimFileName !="" && File.Exists(SimFileName))
                //{
                //    return Bitmap.FromFile(SimFileName);
                //}
                return null;
            }

            ICogImage cogImage = null;
            StopLiveAllCameraWindows();
            if (ExternalTrigger)
            {
                if (!manualLive)
                {
                    TriggerMode = eTriggerMode.Idle;
                }
                _waitForImage.Reset();
                FireTrigger();
                try
                {
                    // Wait for response
                    U.BlockOrDoEvents(_waitForImage, 2000);
                    cogImage = _lastCogImage;
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Image Acquisition error");
                }
            }
            else
            {
                try
                {
                    System.Threading.Thread.Sleep(1);
                    int ticket = -1;
                    lock (_lockAcqFifo)
                    {
                        ticket = _cogAcqFifo.StartAcquire();
                    }
                    cogImage = WaitForImage(ticket);
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Cognex Acquisition Error");
                }
                D.ObjectEventHandler delCallback = GetNextCallback();
                if (delCallback != null)
                {
                    delCallback(cogImage);
                }
            }
            AssignImageToWindows(cogImage);

            return cogImage;
        }

        private ICogImage _lastCogImage = null;
        private object _lockFifoComplete = new object();
        /// <summary>
        /// Auto-Triggerring
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CogAcqFifo_Complete(object sender, CogCompleteEventArgs e)
        {
            lock (_lockFifoComplete)
            {
                _lastCogImage = null;
                try
                {
                    do
                    {
                        Stopwatch sw = Stopwatch.StartNew();
                        ICogImage cogImage = WaitForImage(-1);
                        if (cogImage == null)
                        {
                            return;
                        }
                        _lastCogImage = cogImage;
                        if (ShowImageInWindows)
                        {
                            AssignImageToWindows(cogImage);
                        }
                        D.ObjectEventHandler delCallback = GetNextCallback();
                        if (delCallback != null)
                        {
                            delCallback(_lastCogImage);
                        }
                        _waitForImage.Set();
                    } while (true);
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Error in obtaining image");
                    System.Diagnostics.Debug.WriteLine(string.Format("Error in obtaining image {0}", Name));
                }
                _waitForImage.Set();
            }
        }

        CogImageConvertTool _mConGrey = new CogImageConvertTool();
        CogIPOneImageTool _mOneImageTool = new CogIPOneImageTool();
        CogIPOneImageFlipRotate _ImageRotate = new CogIPOneImageFlipRotate() { OperationInPixelSpace = CogIPOneImageFlipRotateOperationConstants.Rotate270Deg };

        /// <summary>
        /// Wait for an expected image
        /// </summary>
        /// <param name="acqTicket">Use -1 if Auto-triggering</param>
        /// <returns></returns>
        private ICogImage WaitForImage(int acqTicket)
        {
            long startTime0 = U.DateTimeNow;
            ICogImage cogImage = null;
            int completeTicket, triggerNumber, numPending, numReady=0;
            bool busy;
            do
            {
                lock (_lockAcqFifo)
                {
                    try
                    {
                        _cogAcqFifo.GetFifoState(out numPending, out numReady, out busy);
                        //System.Diagnostics.Debug.WriteLine(string.Format("{0} Acquire numPending={1} numReady={2} busy={3}", Name, numPending, numReady, busy));

                        if (numReady > 0)
                        {
                            long startTime1 = U.DateTimeNow;
                            cogImage = _cogAcqFifo.CompleteAcquire(acqTicket, out completeTicket, out triggerNumber);
                            long startTime2 = U.DateTimeNow;
                            if (cogImage == null)
                            {
                                U.LogPopup("Unexpected CogImage type = {0}", cogImage.GetType().Name);
                                return null;
                            }
                            if (cogImage is CogImage8Grey)
                            {

                                CogImage8Grey cogImage8Grey = cogImage as CogImage8Grey;
                               
                                _mConGrey.InputImage = cogImage8Grey;
                                _mConGrey.Run();
                                _mConGrey.InputImage = null;
                                (cogImage as IDisposable).Dispose();
                                return _mConGrey.OutputImage as CogImage8Grey;
                            }
                            else if (cogImage is CogImage24PlanarColor)
                            {
                                CogImage24PlanarColor copyColorImage = null;
                                if (RotateImage == 0)
                                {
                                    copyColorImage = new CogImage24PlanarColor((cogImage as CogImage24PlanarColor).ToBitmap());
                                }
                                else
                                {
                                    if (_mOneImageTool.Operators.Count == 0)
                                    {
                                        switch (RotateImage)
                                        {
                                            case 90:
                                                _ImageRotate.OperationInPixelSpace = CogIPOneImageFlipRotateOperationConstants.Rotate270Deg;
                                                break;
                                            case -90:
                                            case 270:
                                                _ImageRotate.OperationInPixelSpace = CogIPOneImageFlipRotateOperationConstants.Rotate270Deg;
                                                break;
                                            case 180:
                                                _ImageRotate.OperationInPixelSpace = CogIPOneImageFlipRotateOperationConstants.Rotate180Deg;
                                                break;
                                            default:
                                                U.LogPopup("Unexpected rotate value ({0}) for camera {1}", RotateImage, Nickname);
                                                break;
                                        }
                                        _mOneImageTool.Operators.Add(_ImageRotate);
                                    }
                                    _mOneImageTool.InputImage = cogImage;
                                    _mOneImageTool.Run();
                                    _mOneImageTool.InputImage = null;
                                    copyColorImage = _mOneImageTool.OutputImage as CogImage24PlanarColor;
                                }
                                long startTime3 = U.DateTimeNow;
                                double ms10 = U.TicksToMS(startTime1 - startTime0);
                                double ms21 = U.TicksToMS(startTime2 - startTime1);
                                double ms32 = U.TicksToMS(startTime3 - startTime2);
                                Debug.WriteLine(string.Format("GotImage={0}  Complete={1}  Rotate={2}", ms10, ms21, ms32)); 
                                (cogImage as IDisposable).Dispose();
                                return copyColorImage;

                            }
                            else
                            {
                                U.LogPopup("Unexpected CogImage type = {0}", cogImage.GetType().Name);
                            }
                            (cogImage as IDisposable).Dispose();
                            return null;
                        }
                        U.SleepWithEvents(10);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(string.Format("{0} WaitForImage Exception={1}", Name, ex.ToString()));
                        _cogAcqFifo.Flush();
                        U.LogError(ex, "Error in CompleteAcquire");
                        System.Threading.Thread.Sleep(100);
                    }
                }
            } while (numReady <= 0 && acqTicket >= 0);

            return null;
        }
        private delegate void _delParamCogImage(CogDisplay cogDisplay, ICogImage cogImage);
        private void SetCogImage(CogDisplay cogDisplay, ICogImage cogImage)
        {
            if (cogDisplay.InvokeRequired)
            {
                cogDisplay.BeginInvoke(new _delParamCogImage(SetCogImage), new object[] { cogDisplay, cogImage });
                return;
            }

            try
            {
                cogDisplay.Image = cogImage;
                cogDisplay.Invalidate();
            }
            catch (Exception ex)
            {
                U.LogError(ex, "Unable to set CognexWindow image");
            }
        }
    

        /// <summary>
        /// Assign the new image to windows
        /// </summary>
        /// <param name="cogImage"></param>
        public void AssignImageToWindows(ICogImage cogImage)
        {
            if (!IsDestroying && cogImage != null)
            {
                lock (LockCogDisplay)
                {
                    foreach (CogDisplay cogDisplay in _cogDisplayWindows)
                    {
                        if (!cogDisplay.IsDisposed)
                        {
                            if (object.ReferenceEquals(cogDisplay.Image, cogImage))
                            {
                                System.Diagnostics.Debug.WriteLine("Saved time in AssignImageToWindows.  Image already there");
                            }
                            else
                            {
                                SetCogImage(cogDisplay, cogImage);
                            }
                        }
                    }

                    //Parallel.ForEach<CogDisplay>(_cogDisplayWindows, currentDisplay =>
                    //{

                    //    if (!currentDisplay.IsDisposed)
                    //    {
                    //        if (object.ReferenceEquals(currentDisplay.Image, cogImage))
                    //        {
                    //            System.Diagnostics.Debug.WriteLine("Saved time in AssignImageToWindows.  Image already there");
                    //        }
                    //        else
                    //        {
                    //            SetCogImage(currentDisplay, cogImage);
                    //        }
                    //    }
                    //});
                }
            }
        }


        /// <summary>
        /// Assign the new Gaphic to windows
        /// </summary>
        /// <param name="cogImage"></param>
        public void AssignGraphicToWindows(ICogGraphic cogGraphic)
        {
            lock (LockCogDisplay)
            {
                foreach (CogDisplay cogDisplay in _cogDisplayWindows)
                {
                    cogDisplay.InteractiveGraphics.Add((ICogGraphicInteractive)cogGraphic, "", false);
                }
            }
        }


        /// <summary>
        /// Change Camera Brigness
        /// </summary>
        /// <param name="dVal"></param>
        private void OnChangedBrightness(MDoubleNoUnits dNoUnits)
        {
            lock (_lockAcqFifo)
            {
                if (_cogAcqFifo != null)
                {
                    try
                    {
                        if (_cogAcqFifo.OwnedBrightnessParams != null)
                        {
                            _cogAcqFifo.OwnedBrightnessParams.Brightness = dNoUnits.Val;
                        }
                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Change Camera Contrast
        /// </summary>
        /// <param name="dVal"></param>
        private void OnChangedContrast(MDoubleNoUnits dNoUnits)
        {
            lock (_lockAcqFifo)
            {
                if(_cogAcqFifo!= null)
                {
                    try
                    {
                        if (_cogAcqFifo.OwnedContrastParams != null)
                        {
                            _cogAcqFifo.OwnedContrastParams.Contrast = dNoUnits.Val;
                        }
                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Change Camera Exposure
        /// </summary>
        /// <param name="dVal"></param>
        private void OnChangedExposure(Miliseconds sec)
        {
            lock (_lockAcqFifo)
            {
                if(_cogAcqFifo!= null)
                {
                    try
                    {
                        _cogAcqFifo.OwnedExposureParams.Exposure = sec.Val;
                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Change Camera Acquistion
        /// </summary>
        /// <param name="dVal"></param>
        private void OnChangedAquireTime(Seconds sec)
        {

        }

        private void OnChangeVideoFormat(String videoFormat)
        {
            if (_frameGrabber != null)
            {
                lock (_lockAcqFifo)
                {
                    _cogAcqFifo = null;
                    _cogAcqFifo = _frameGrabber.CreateAcqFifo(videoFormat, CogAcqFifoPixelFormatConstants.Format8Grey, 0, false);
                }

                CorrectDisplayWindows();
            }

        }

        private delegate void _delParamVoid();
        private void CorrectDisplayWindows()
        {
            lock (LockCogDisplay)
            {
                if (_cogDisplayWindows == null || _cogDisplayWindows.Count == 0)
                {
                    return;
                }

                if (_cogDisplayWindows[0].InvokeRequired)
                {
                    _cogDisplayWindows[0].BeginInvoke(new _delParamVoid(CorrectDisplayWindows));
                    return;
                }


                foreach (CogDisplay cogDisplay in _cogDisplayWindows)
                {
                    if (cogDisplay.LiveDisplayRunning)
                    {
                        cogDisplay.StopLiveDisplay();
                        lock (_lockAcqFifo)
                        {
                            if (_cogAcqFifo != null)
                            {
                                cogDisplay.StartLiveDisplay(_cogAcqFifo, true);
                            }
                        }
                    }
                }
            }
        }


        private void OnChangeCameraID(String cameraId)
        {
            StopLiveAllCameraWindows();
            GetCameraIDs();
            GetVideoFormats();
            InitializeFrameGrabber();
        }

        private void StopLiveAllCameraWindows()
        {
            lock (LockCogDisplay)
            {
                if (_cogDisplayWindows == null || _cogDisplayWindows.Count == 0)
                {
                    return;
                }

                foreach (CogDisplay cogDisplay in _cogDisplayWindows)
                {
                    StopLiveDisplay(cogDisplay);
                }
            }
        }


        private delegate void _delParamCogDisplay(CogDisplay display);
        private void StopLiveDisplay(CogDisplay display)
        {
            if (display.InvokeRequired)
            {
                display.BeginInvoke(new _delParamCogDisplay(StopLiveDisplay), new object[] { display });
                return;
            }

            display.StopLiveDisplay();
        }



    }
}
