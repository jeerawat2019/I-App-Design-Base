using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using MDouble;

namespace MCore.Comp.VisionSystem
{
    public partial class VisionJobBase : CompBase
    {
        private object _jobImage = null;

        protected CameraBase _cameraBase = null;
        /// <summary>
        /// The Strobe duration
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public Miliseconds StrobeDuration
        {
            get { return GetPropValue(() => StrobeDuration); }
            set { SetPropValue(() => StrobeDuration, value); }
        }

        private string _lastAcquisitionFileFolder = string.Empty;

        [Browsable(true)]
        public string LastAcquisitionFileFolder
        {
            get 
            {
                if (string.IsNullOrEmpty(_lastAcquisitionFileFolder))
                {
                    return VisionJobBase.VisionFilesRootPath;
                }
                return _lastAcquisitionFileFolder; 
            }
            set { _lastAcquisitionFileFolder = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Image LastJobBitmap
        {
            get
            {
                return ImageToBitmap(LastObjJobImage);
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public object LastObjJobImage
        {
            get 
            {
                if (_jobImage != null)
                {
                    return _jobImage;
                }
                if (RefResults != null)
                {
                    return RefResults.DefaultImage;
                }
                return null;
            }
            set 
            {
                _jobImage = value; 
            }
        }
        private ResultsExchange _refResults = null;
        /// <summary>
        /// The vision file
        /// </summary>
        [Browsable(false)]
        public ResultsExchange RefResults
        {
            get
            {
                if (_refResults == null)
                {
                    _refResults = this.FilterByTypeSingle<ResultsExchange>();
                }
                return _refResults;
            }
        }

        public delegate void InvertTargetEventHandler(int width, int height, int memRowWidth, IntPtr ptrRed, IntPtr ptrGreen, IntPtr ptrBlue);
        public delegate void VisionJobBaseEventHandler(VisionJobBase visionJobBase);

        public event InvertTargetEventHandler OnInvertTarget = null;
        public event VisionJobBaseEventHandler OnPostEditVisionFile = null;
        public event MethodInvoker OnNextHelp = null;
        public event MethodInvoker OnPrevHelp = null;

        protected bool HasInvertTargetEvent
        {
            get { return OnInvertTarget != null; }
        }
        protected void FireInvertTarget(int width, int height, int memRowWidth, IntPtr ptrRed, IntPtr ptrGreen, IntPtr ptrBlue)
        {
            if (HasInvertTargetEvent)
            {
                OnInvertTarget(width, height, memRowWidth, ptrRed, ptrGreen, ptrBlue);
            }
        }

        public static string VisionFilesFolder = "Vision Files";

        public static string VisionFilesRootPath
        {
            get { return string.Format(@"{0}\{1}\", U.RootComp.RootFolder, VisionFilesFolder); }
        }

        public CameraBase RefCamera
        {
            get { return GetParent<CameraBase>(); }
        }


        /// <summary>
        /// The vision file
        /// </summary>
        [Browsable(true)]
        [Category("Camera")]
        public string VisionFile
        {
            get 
            { 
                return GetPropValue(() => VisionFile, string.Empty); 
            }
            set 
            { 
                SetPropValue(() => VisionFile, value);
                if (Initialized)
                {
                    LoadVisionFile();
                }
            }
        }


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public VisionJobBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public VisionJobBase(string name)
            : base(name)
        {
        }
        #endregion Constructors

        #region Virtuals
        /// <summary>
        /// Reload the vision tool
        /// </summary>
        public virtual void LoadVisionFile()
        {
        }
        /// <summary>
        /// Save the vision tool
        /// </summary>
        public virtual void SaveVisionFile()
        {
        }

        /// <summary>
        /// Edit the vision File
        /// </summary>
        /// <param name="visionFile"></param>
        public void EditVisionFile()
        {
            // Simulation
            //
            EditVisionFile(LastObjJobImage);
        }

        /// <summary>
        /// Edit the vision File
        /// </summary>
        /// <param name="visionFile"></param>
        public virtual void EditVisionFile(object oImage)
        {
            // Simulation
            //
        }

        /// <summary>
        /// Get the bits of the image for editing
        /// </summary>
        /// <param name="oImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="memRowWidth"></param>
        /// <param name="ptrRed"></param>
        /// <param name="ptrGreen"></param>
        /// <param name="ptrBlue"></param>
        /// <returns></returns>
        public virtual object GetImageBits(object oImage, ref int width, ref int height, ref int memRowWidth, ref IntPtr ptrRed, ref IntPtr ptrGreen, ref IntPtr ptrBlue)
        {
            return null;
        }

        /// <summary>
        /// Close the opened image bits
        /// </summary>
        /// <param name="lockObj"></param>
        public virtual void CloseImageBits(object lockObj)
        {
        }


        public virtual void SetStrobeDuration()
        {
            // If strobe reference, set strobe duration
            if (RefCamera != null)
            {
                RefCamera.SetStrobeDuration(StrobeDuration);
            }
        }

        /// <summary>
        /// Prepare the data for an acquisition
        /// </summary>
        public virtual void PreAcquisition()
        {
            SetStrobeDuration();
        }

        /// <summary>
        /// Run the vision job
        /// </summary>
        /// <param name="oImage"></param>
        /// <param name="invertTarget"></param>
        public virtual object RunJob(object oImage, bool invertTarget)
        {
            return oImage;
        }
        /// <summary>
        /// Clear results and initialize the success to false;
        /// </summary>
        public void ResetResults()
        {
            ResultsError = string.Empty;
            ResultsSuccess = false;
            if (RefResults != null)
            {
                RefResults.Reset();
            }
        }

        /// <summary>
        /// Invert the target
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public virtual Bitmap InvertTarget(Bitmap bm)
        {
            return bm;
        }

        /// <summary>
        /// Assign input value to vision Output variable
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void AssignOutput(string key, double value)
        {
        }

        /// <summary>
        /// Add a result to the exchanger
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddResult(string key, double value)
        {
            if (RefResults != null)
            {
                RefResults.Results.Add(key, value);
            }
        }

        /// <summary>
        /// Add a result to the exchanger
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddResult(string key,string value)
        {
            if (RefResults != null)
            {
                RefResults.Results.Add(key, value);
            }
        }

        /// <summary>
        /// Add a result to the exchanger
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddResult(string key, Bitmap value)
        {
            if (RefResults != null)
            {
                RefResults.Results.Add(key, value);
            }
        }

        /// <summary>
        /// Get the 
        /// </summary>
        [XmlIgnore]
        public CompBase ImageOwner
        {
            get
            {
                if (RefResults != null)
                {
                    return RefResults.ImageOwner;
                }
                return null;
            }
            set
            {
                if (RefResults != null)
                {
                    RefResults.ImageOwner = value;
                }
            }
        }
        private bool _resultsSuccess = false;
        /// <summary>
        /// Get flag that results are a success
        /// </summary>
        public bool ResultsSuccess
        {
            get
            {
                if (!_resultsSuccess)
                {
                    return false;
                }
                if (RefResults != null)
                {
                    return RefResults.ResultsSuccess;
                }
                return true;
            }
            set
            {
                _resultsSuccess = value;
            }
        }
        private string _resultsError = string.Empty;
        /// <summary>
        /// Get results error
        /// </summary>
        [XmlIgnore]
        public string ResultsError
        {
            get
            {
                if (!string.IsNullOrEmpty(_resultsError))
                {
                    return _resultsError;
                }
                if (RefResults != null)
                {
                    return RefResults.ResultsError;
                }
                return string.Empty;
            }
            set
            {
                _resultsError = value;
            }
        }
        /// <summary>
        /// Acquisition has just completed
        /// </summary>
        public virtual void PostAcquisition()
        {
            if (_resultsSuccess && RefResults != null)
            {
                RefResults.ProcessResults();
            }
            ImageOwner = null;

        }

        /// <summary>
        /// Trigger a camera acquisition and return the image
        /// </summary>
        [StateMachineEnabled]
        public void Trigger()
        {
            TriggerForImage();
        }
        /// <summary>
        /// Trigger a camera acquisition and return the image
        /// </summary>
        [StateMachineEnabled]
        public void TriggerAndRunJob()
        {
            object oImage = TriggerForImage();
            RunJob(oImage, false);
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
        /// <summary>
        /// Convert an image to gray scale
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public virtual Bitmap ImageToGrayBitmap(object oImage)
        {
            return oImage as Bitmap;
        }

        /// <summary>
        /// Convert the Bitmap image to camera image
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public virtual object BitmapToCameraImage(Bitmap bm)
        {
            return new Bitmap(bm);
        }
        /// <summary>
        /// Copy the image produced by the camera
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public virtual object CopyImage(object oImage)
        {
            return new Bitmap(oImage as Bitmap);
        }

        /// <summary>
        /// Trigger a camera acquisistion
        /// </summary>        
        public virtual object TriggerForImage()
        {
            // Simulation

            //  Acquire

            // Allow Camera to prepare
            PreAcquisition();

            // Run
            // 

            // Allow Camera to prepare
            PostAcquisition();
            return null;
        }

        public virtual void AssignAcquireImageToWindows(Image acquireImage)
        {
            //Implemented on derived class
        }


        public virtual void DisposeImage(object oImage)
        {
        }

        #endregion Virtuals

        #region Overrides
        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            //RegisterOnChanged(() => VisionFile, OnChangedVisionFile);
            //OnChangedVisionFile(string.Empty);
            LoadVisionFile();
        }
        #endregion Overrides

        #region Public Calls to do service


        #endregion Public Calls to do service

        #region privates
        //private void OnChangedVisionFile(string visionFile)
        //{
        //    LoadVisionFile();
        //}

        protected void FirePostEditVisionFile()
        {
            if (OnPostEditVisionFile != null)
            {
                OnPostEditVisionFile(this);
            }
        }

        protected void FireNextHelp()
        {
            if (OnNextHelp != null)
            {
                OnNextHelp();
            }
        }

        protected void FirePrevHelp()
        {
            if (OnPrevHelp != null)
            {
                OnPrevHelp();
            }
        }


        #endregion privates

    }
}
