using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Threading;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.ResultsAnalysis;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.SearchMax;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Implementation;
using System.Threading.Tasks;

namespace MCore.Comp.VisionSystem
{
    public class CognexJob8 : VisionJobBase
    {
        private CogToolBlock _grabberBuffer = null;

        /// <summary>
        /// Get Camera object that the job belong to. 
        /// </summary>
        public CognexCamera8 CogCamera
        {
            get { return _cameraBase as CognexCamera8; }
        }

        private bool _rgbFromBayer = true;


        private string _simFileName;

        private bool _isDisplayGraphic = true;

        [System.ComponentModel.Editor(
        typeof(System.Windows.Forms.Design.FileNameEditor),
        typeof(System.Drawing.Design.UITypeEditor))]
        [Browsable(true)]
        [Category("Camera")]
        public string SimFileName
        {

            get { return this._simFileName; }

            set { this._simFileName = value; }

        }


        [Browsable(true)]
        [Category("Cognex")]
        public bool EnableDisplayTimeLabel
        {
            get { return GetPropValue(() => EnableDisplayTimeLabel, false); }
            set { SetPropValue(() => EnableDisplayTimeLabel,value); }
        }

        [Browsable(true)]
        [Category("Cognex")]
        public double TimeLabelLocX
        {
            get { return GetPropValue(() => TimeLabelLocX, 250); }
            set { SetPropValue(() => TimeLabelLocX, value); }
        }

        [Browsable(true)]
        [Category("Cognex")]
        public double TimeLabelLocY
        {
            get { return GetPropValue(() => TimeLabelLocY, 80); }
            set { SetPropValue(() => TimeLabelLocY, value); }
        }

        /// <summary>
        /// Last CogDisplayResult
        /// </summary>
        [Browsable(true)]
        [Category("Cognex")]
        [XmlIgnore]
        public Bitmap CogDisplayResult
        {
            get;
            set;
        }

        /// <summary>
        /// _rgbFromBayer
        /// </summary>
        [Browsable(true)]
        [Category("Cognex")]
        public bool RGBFromBayer
        {
            get { return _rgbFromBayer; }
            set { _rgbFromBayer = value; }
        }

        /// <summary>
        /// _rgbFromBayer
        /// </summary>
        [Browsable(true)]
        [Category("Cognex")]
        public bool IsDisplayGraphic
        {
            get { return _isDisplayGraphic; }
            set { _isDisplayGraphic = value; }
        }


        private CogToolGroup _cogToolGroup = null;

        /// <summary>
        /// Access CogToolGroup Object
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public CogToolGroup CogToolGroup
        {
            get { return _cogToolGroup; }
            set 
            { 
                if(_cogToolGroup != null)
                {
                    _cogToolGroup.Ran -= new EventHandler(CogToolGroup_OnRan);
                    if (!IsDestroying)
                    {
                        _cogToolGroup.Dispose();
                        _cogToolGroup = null;
                        GC.Collect();
                    }
                    else
                    {
                        _cogToolGroup = null;
                    }

                }
                if(value != null)
                {
                    _cogToolGroup = value;
                  _cogToolGroup.Ran += new EventHandler(CogToolGroup_OnRan);
                }
            }
        }

        private object _sLockRunToolAndDisplayImage = new object();
        private EventWaitHandle _autoResetEventWaitToolRun = new AutoResetEvent(false);
       
      

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CognexJob8()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CognexJob8(string name)
            : base(name)
        {
        }
        #endregion Constructors

        #region overrides

        /// <summary>
        /// Initialize this Component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
         
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }

            try
            {
                // 
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

        public override void Destroy()
        {
            base.Destroy();
            DisposeToolGroup();
        }

        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            _cameraBase = Parent as CameraBase;

            if (_cameraBase == null)
            {
                U.LogError("The CognexJob ({0}) must be child of CameraBase derived component", Name);
            }


        }
      
        public override void DisposeImage(object oImage)
        {
            if (oImage is IDisposable)
            {
                ((IDisposable)oImage).Dispose();
            }
            else if (oImage is Bitmap)
            {
                (oImage as Bitmap).Dispose();
            }
        }

        /// <summary>
        /// Trigger a camera acquisistion
        /// </summary>
        /// <param name="camera"></param>
        public override object TriggerForImage()
        {
            ResetResults();
            if (_cameraBase == null)
            {
                U.LogError("The CognexJob ({0}) must be child of a CameraBase", Name);

                return null;
            }

            if (CogToolGroup == null)
            {
                return null;
            }

            //
            // Allow Camera to prepare
            //
            PreAcquisition();

            //
            //  Acquiire image
            //
            return _cameraBase.Acquire(false);

        }

        /// <summary>
        /// Prepare the data for an acquisition
        /// </summary>
        public override void PreAcquisition()
        {
            base.PreAcquisition();
            if (CogToolGroup == null)
            {
                LoadVisionFile();
            }
        }
        /// <summary>
        /// Convert the image produced by the camera to a Bitmap
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public static Bitmap CogImageToBitmap(object oImage)
        {
            if (oImage != null && oImage is ICogImage)
            {
                
                Bitmap bm = (oImage as ICogImage).ToBitmap();
                //U.LOHAdded(bm.Width * bm.Height * 3);
                return bm;
            }
            return oImage as Bitmap;
        }
        
        /// <summary>
        /// Convert the image to Gray scale
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public static Bitmap CogImageToGrayBitmap(object oImage)
        {
            // Put in Gray format
            CogImage8Grey greyImage = null;
            if (oImage != null)
            {
                if (oImage is ICogImage)
                {
                    if (oImage is CogImage8Grey)
                        greyImage = oImage as CogImage8Grey;
                    else
                    {

                    }
                }
                else
                {
                    greyImage = new CogImage8Grey(oImage as Bitmap);
                }
            }
            // Put in ICognex 24 color
            return greyImage.ToBitmap();

        }
        /// <summary>
        /// Convert the image produced by the camera to a Bitmap
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public override Bitmap ImageToBitmap(object oImage)
        {
            return CogImageToBitmap(oImage);
        }

        /// <summary>
        /// Convert an image to gray scale
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public override Bitmap ImageToGrayBitmap(object oImage)
        {
            return CogImageToGrayBitmap(oImage);
        }

        /// <summary>
        /// Convert the Bitmap image to camera image
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public override object BitmapToCameraImage(Bitmap bm)
        {
            return new CogImage24PlanarColor(bm);
        }
        /// <summary>
        /// Copy the image produced by the camera
        /// </summary>
        /// <param name="oImage"></param>
        /// <returns></returns>
        public override object CopyImage(object oImage)
        {
            if (oImage is CogImage24PlanarColor)
            {
                CogImage24PlanarColor copyColorImage = new CogImage24PlanarColor(oImage as CogImage24PlanarColor);
                return copyColorImage;
            }
            else if (oImage is ICogImage)
            {
                return (oImage as ICogImage).CopyBase(CogImageCopyModeConstants.CopyPixels);
            }
            return base.CopyImage(oImage);
        }
        /// <summary>
        /// Run the vision job
        /// </summary>
        /// <param name="oImage"></param>
        /// <param name="invertTarget"></param>
        public override object RunJob(object oImage, bool invertTarget)
        {
            ICogImage cogImage = null;
            if (oImage != null)
            {
                if (oImage is Bitmap)
                {
                    cogImage = new CogImage24PlanarColor(oImage as Bitmap);
                }
                else
                {
                    cogImage = oImage as ICogImage;
                }
                if (invertTarget)
                {
                    cogImage = InvertTarget(cogImage);
                }
            }
            else if (SimFileName != "" && File.Exists(SimFileName))
            {
                cogImage = new CogImage24PlanarColor(new Bitmap(Bitmap.FromFile(SimFileName)));
            }
            else if(LastObjJobImage !=null)
            {
                 cogImage = LastObjJobImage as ICogImage;
            }
            

           
            if (cogImage == null)
            {
                U.LogPopup("Expected to find a Cognex image to process");
                return null;
            }
            LastObjJobImage = cogImage;
               
            AssignInputImage(cogImage);

            _autoResetEventWaitToolRun.Reset();

            // Run
            // 
            // This lock help to prevent hang/crash in case a Run is called at the same time
            // when another job is assigning Image to CogDisplay
            lock (_sLockRunToolAndDisplayImage)
            {
                CogToolGroup.Run();
            }
            // Then we will wait for time out before return.
            // We are sure that when we return, the result is available.
            _autoResetEventWaitToolRun.WaitOne(5000, false);

            // Re-Populate the results
            PopulateResults();

            // Perform any post acquisition operations
            try
            {
                PostAcquisition();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("PostAcquisition Exception : {0}", ex.Message));
            }
            return LastObjJobImage;
        }

        /// <summary>
        /// Invert the target
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public override Bitmap InvertTarget(Bitmap bm)
        {
            CogImage24PlanarColor imgColor = new CogImage24PlanarColor(bm);
            imgColor = InvertTarget(imgColor) as CogImage24PlanarColor;
            return imgColor.ToBitmap();
        }

        /// <summary>
        /// Get the bits of the image for editing
        /// </summary>
        /// <param name="oImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="stride"></param>
        /// <param name="ptrRed"></param>
        /// <param name="ptrGreen"></param>
        /// <param name="ptrBlue"></param>
        /// <returns></returns>
        public override object GetImageBits(object oImage, ref int width, ref int height, ref int stride, ref IntPtr ptrRed, ref IntPtr ptrGreen, ref IntPtr ptrBlue)
        {
            if (oImage is CogImage24PlanarColor)
            {
                CogImage24PlanarColor imgColor = oImage as CogImage24PlanarColor;
                ICogImage8PixelMemory memRed, memGreen, memBlue;
                width = imgColor.Width;
                height = imgColor.Height;
                stride = width + 8;
                imgColor.Get24PlanarColorPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, width, height, out memRed, out memGreen, out memBlue);
                ptrRed = memRed.Scan0;
                ptrGreen = memGreen.Scan0;
                ptrBlue = memBlue.Scan0;
                return new ImagePixelLock() { MemRed = memRed, MemGreen = memGreen, MemBlue = memBlue };
            }
            return null;
        }
        /// <summary>
        /// Close the opened image bits
        /// </summary>
        /// <param name="lockPtrs"></param>
        public override void CloseImageBits(object lockPtrs)
        {
            ImagePixelLock imgPixelLock = lockPtrs as ImagePixelLock;
            imgPixelLock.MemRed.Dispose();
            imgPixelLock.MemGreen.Dispose();
            imgPixelLock.MemBlue.Dispose();
        }
        private ICogImage InvertTarget(ICogImage cogImage)
        {
            if (cogImage is CogImage24PlanarColor)
            {
                CogImage24PlanarColor imgColor = cogImage as CogImage24PlanarColor;
                if (HasInvertTargetEvent)
                {
                    ICogImage8PixelMemory memRed, memGreen, memBlue;
                    int width = imgColor.Width;
                    int height = imgColor.Height;
                    int memRowWidth = width + 8;
                    imgColor.Get24PlanarColorPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, width, height, out memRed, out memGreen, out memBlue);
                    FireInvertTarget(width, height, memRowWidth, memRed.Scan0, memGreen.Scan0, memBlue.Scan0);
                    memRed.Dispose();
                    memGreen.Dispose();
                    memBlue.Dispose();
                }
                cogImage = imgColor;
            }
            return cogImage;
        }
        //}
        //else
        //{
        //    Bitmap bm = oImage as Bitmap;
        //    if (bm == null && LastObjJobImage != null)
        //    {
        //        bm = LastJobBitmap as Bitmap;
        //    }
        //    if (HasColorModEvent)
        //    {
        //        CogImage24PlanarColor imgColor = new CogImage24PlanarColor(bm);
        //        ICogImage8PixelMemory memRed, memGreen, memBlue;
        //        int width = imgColor.Width;
        //        int height = imgColor.Height;
        //        int memRowWidth = width + 8;
        //        imgColor.Get24PlanarColorPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, width, height, out memRed, out memGreen, out memBlue);
        //        FireColorImageMod(width, height, memRowWidth, memRed.Scan0, memGreen.Scan0, memBlue.Scan0);
        //        memRed.Dispose();
        //        memGreen.Dispose();
        //        memBlue.Dispose();
        //        LastRunJobCogImage = imgColor;
        //    }
        //    else
        //    {
        //        LastRunJobCogImage = new CogImage8Grey(bm);
        //    }
        //    //Stopwatch sw = Stopwatch.StartNew();
        //    //GC.Collect();
        //    //System.Diagnostics.Debug.WriteLine(string.Format("GC.Collect time= {0} ms, {1}", sw.ElapsedMilliseconds, "VisionImageCopy"));
        //    LastObjJobImage = LastRunJobCogImage;
        //}


        /// <summary>
        /// Edit the vision File
        /// </summary>
        public override void EditVisionFile(object oImage)
        {
            // Assign the image first
            try
            {
                AssignInputImage(oImage);
                //if (_cogCamera != null && LastObjJobImage is ICogImage)
                //{
                //    AssignInputImage(LastObjJobImage as ICogImage);
                //}

            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error assigning image the Cog tool group file before editing");
            }

            SaveVisionFile();

            try
            {
                //RegisterToolEvents(false);
                using (CognexJobEditForm editForm = new CognexJobEditForm(this))
                {
                    editForm.DelPrevHelp = FirePrevHelp;
                    editForm.DelNextHelp = FireNextHelp;
                    editForm.ShowDialog();
                    FirePostEditVisionFile();
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error editing the Cog vpp file");
            }
            // Force a GC
            GC.Collect();
        }


        /// <summary>
        /// Save the vision tool
        /// </summary>
        public override void SaveVisionFile()
        {
            if (CogToolGroup != null)
            {
                try
                {
                    string visonPath = VisionFile.Substring(0, VisionFile.LastIndexOf('\\'));

                    if (!Directory.Exists(visonPath))
                    {
                        Directory.CreateDirectory(visonPath);
                    }
                    CogSerializer.SaveObjectToFile(CogToolGroup, VisionFile);
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Error saving the Cog vpp file");
                }
                finally
                {
                   
                }
            }
        }

        private void DisposeToolGroup()
        {
            if (CogToolGroup != null)
            {
                CogToolGroup = null;
            }
        }

        /// <summary>
        /// Reload the vision tool
        /// </summary>
        public override void LoadVisionFile()
        {
            DisposeToolGroup();

            if (string.IsNullOrEmpty(VisionFile))
            {
                VisionFile = this.UniqueNames[1];
            }

            if (!VisionFile.Contains('\\'))
            {
                VisionFile = String.Format(@"{0}{1}.vpp", VisionFilesRootPath, VisionFile);
            }


            if (File.Exists(VisionFile))
            {
                // Load it
                try
                {
                    CogToolGroup = CogSerializer.LoadObjectFromFile(VisionFile) as CogToolGroup;
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Vision File load error");
                }

            }
            else
            {
                CogToolGroup = new CogToolGroup();
            }

            //_cogToolGroup.Ran += new EventHandler(OnRunComplete);
        }
        #endregion Overrides


        /// <summary>
        /// Assign input valiue to vision output variable
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public override void AssignOutput(string key, double value)
        {
            CogToolBlock grabberBuffer = GetGrabberBuffer();
            if (grabberBuffer != null)
            {
                foreach (CogToolBlockTerminal output in grabberBuffer.Outputs)
                {
                    if (key == output.Name)
                    {
                        output.Value = value;
                        return;
                    }
                }
                // Create it
                grabberBuffer.Outputs.Add(new CogToolBlockTerminal(key, value));

            }
        }

        public override void AssignAcquireImageToWindows(Image acquireImage)
        {
            CognexCamera8 cogCam = CogCamera;
            if (cogCam != null && cogCam.CogDisplayWindows != null && cogCam.CogDisplayWindows.Count > 0)
            {
                ICogImage cogImage = null;
                if (acquireImage != null)
                {
                    if (acquireImage is Bitmap)
                    {
                        cogImage = new CogImage24PlanarColor(acquireImage as Bitmap);
                    }
                    else
                    {
                        cogImage = acquireImage as ICogImage;
                    }
                }


                Parallel.ForEach<CogDisplay>(cogCam.CogDisplayWindows, currentDisplay =>
                {
                    if (!currentDisplay.IsDisposed)
                    {
                        if (object.ReferenceEquals(currentDisplay.Image, acquireImage))
                        {
                            System.Diagnostics.Debug.WriteLine("Saved time in AssignImageToWindows.  Image already there");
                        }
                        else
                        {
                            currentDisplay.BeginInvoke(new MethodInvoker(delegate { currentDisplay.Image = cogImage;}));
                        }
                    }
                });
            }
        }
        



        private void PopulateResults()
        {
            ResetResults();

            ResultsSuccess = CogToolGroup.RunStatus.Result == CogToolResultConstants.Accept;

            try
            {
                if (!ResultsSuccess && !string.IsNullOrEmpty(CogToolGroup.RunStatus.Message))
                {
                    ResultsError = CogToolGroup.RunStatus.Message;
                }
                
                ICogImage cogImage = LastObjJobImage as ICogImage;
                int width = cogImage.Width;
                int height = cogImage.Height;
                AddResult("Image Width", Convert.ToDouble(width));
                AddResult("Image Height", Convert.ToDouble(height));

                foreach (ICogTool cogTool in CogToolGroup.Tools)
                {                    
                    if (cogTool is CogPMAlignTool)
                    {
                        AssignToolResult(cogTool as CogPMAlignTool);
                    }
                    else if (cogTool is CogSearchMaxTool)
                    {
                        AssignToolResult(cogTool as CogSearchMaxTool);
                    }
                    else if (cogTool is CogBlobTool)
                    {
                        AssignToolResult(cogTool as CogBlobTool);
                    }
                    else if (cogTool is CogToolBlock)
                    {
                        AssignToolResult(cogTool as CogToolBlock);
                    }
                    else if (cogTool is CogResultsAnalysisTool)
                    {
                        AssignToolResult(cogTool as CogResultsAnalysisTool);
                    }
                    else if (cogTool is CogAffineTransformTool)
                    {
                        AssignToolResult(cogTool as CogAffineTransformTool);
                    }
                    else if (cogTool is CogHistogramTool)
                    {
                        AssignToolResult(cogTool as CogHistogramTool);
                    }
                    //    CogResultsAnalysisTool anTool = cogTool as CogResultsAnalysisTool;
                    //    CogResultsAnalysisRunParams runParams = anTool.RunParams;
                    //    System.Collections.IDictionaryEnumerator en = runParams.GetEnumerator();
                    //    while (en.MoveNext())
                    //    {
                    //        System.Collections.DictionaryEntry entry = en.Entry;

                    //        if (entry.Key.ToString().StartsWith("ZHt"))
                    //        {
                    //            entry.Value = 25.432;
                    //        }
                    //    }
                    //    //anTool.RunParams.Values["ZHtRefIn"] = 5.432;
                    //}
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("PopulateResults Exception{0}", ex.Message));
            }
        }

        #region Assign Tool Result

        private void AssignToolResult(CogAffineTransformTool cogAffineTransform)
        {
            if (cogAffineTransform.OutputImage != null)
            {
                AddResult(string.Format("{0}.Bitmap", cogAffineTransform.Name), cogAffineTransform.OutputImage.ToBitmap());
                AddResult(string.Format("{0}.ImageX", cogAffineTransform.Name), cogAffineTransform.Region.CornerOriginX);
                AddResult(string.Format("{0}.ImageY", cogAffineTransform.Name), cogAffineTransform.Region.CornerOriginY);
                AddResult(string.Format("{0}.ImageTheta", cogAffineTransform.Name), cogAffineTransform.Region.Rotation);
                AddResult(string.Format("{0}.ScalingX", cogAffineTransform.Name), cogAffineTransform.RunParams.ScalingX);
                AddResult(string.Format("{0}.ScalingY", cogAffineTransform.Name), cogAffineTransform.RunParams.ScalingY);
            }
        }
        private void AssignToolResult(CogResultsAnalysisTool cogResults)
        {

            CogResultsAnalysisResult results = cogResults.Result;
            CogResultsAnalysisEvaluationInfoCollection evaluatedExpressions = results.EvaluatedExpressions;
            for (int i = 0; i < evaluatedExpressions.Count; i++)
            {
                CogResultsAnalysisEvaluationInfo info = evaluatedExpressions[i];
                System.Collections.Specialized.StringCollection keys = evaluatedExpressions.Keys as System.Collections.Specialized.StringCollection;
                if (info != null && keys != null)
                {

                    StateFlagsCollection sfc = info.Expression.StateFlags;
                    AddResult(string.Format("{0}.{1}", cogResults.Name, keys[i]), Convert.ToDouble(info.Value));
                }
            }
           

            //foreach (CogToolBlockTerminal output in cogResults.Result)
            //{
            //    if ((output.ValueType == typeof(double) || output.ValueType == typeof(int)) && output.Value != null)
            //    {
            //        AddResult(string.Format("{0}.{1}", cogToolBlock.Name, output.Name), Convert.ToDouble(output.Value));
            //    }
            //    else if (output.ValueType == typeof(CogBlobResults))
            //    {
            //        AssignToolResult(string.Format("{0}.{1}", cogToolBlock.Name, output.Name), output.Value as CogBlobResults);

            //    }
            //}
        }

        private void AssignToolResult(CogToolBlock cogToolBlock)
        {

            foreach (CogToolBlockTerminal output in cogToolBlock.Outputs)
            {
                if ((output.ValueType == typeof(double) || output.ValueType == typeof(int)) && output.Value != null)
                {
                    AddResult(string.Format("{0}.{1}", cogToolBlock.Name, output.Name), Convert.ToDouble(output.Value));
                }
                else if ((output.ValueType == typeof(string)) && output.Value != null)
                {
                    AddResult(string.Format("{0}.{1}", cogToolBlock.Name, output.Name), output.Value.ToString());
                }
                
                else if (output.ValueType == typeof(CogBlobResults))
                {
                    AssignToolResult(string.Format("{0}.{1}", cogToolBlock.Name, output.Name), output.Value as CogBlobResults);
                    
                }
                else if (output.ValueType == typeof(CogHistogramResult))
                {
                    AssignToolResult(string.Format("{0}.{1}", cogToolBlock.Name, output.Name), output.Value as CogHistogramResult);
                }
            }
        }
        private void AssignToolResult(string parent, CogBlobResults blobResults)
        {
            CogBlobResultCollection blobResultColl = blobResults.GetBlobs(false);
            int count = blobResultColl.Count;
            AddResult(string.Format("{0}.Count", parent), blobResultColl.Count);
            double areaX = 0.0;
            double areaY = 0.0;
            double totalArea = 0.0;
            for (int i = 0; i < count; i++)
            {
                CogBlobResult blobResult = blobResultColl[i];
                double x = blobResult.CenterOfMassX;
                double y = blobResult.CenterOfMassY;
                double area = blobResult.Area;
                totalArea += area;
                areaX += x * area;
                areaY += y * area;
                AddResult(string.Format("{0}[{1}].Area", parent, i), area);
                AddResult(string.Format("{0}[{1}].CenterOfMassX", parent, i), x);
                AddResult(string.Format("{0}[{1}].CenterOfMassY", parent, i), y);
                AddResult(string.Format("{0}[{1}].Angle", parent, i), blobResult.Angle);
            }

            AddResult(string.Format("{0}[+].Area", parent), totalArea);
            AddResult(string.Format("{0}[A].CenterOfMassX", parent), totalArea > 0 ? areaX / totalArea : 0.0);
            AddResult(string.Format("{0}[A].CenterOfMassY", parent), totalArea > 0 ? areaY / totalArea : 0.0);
        }

        private void AssignToolResult(CogPMAlignTool cogPMAlign)
        {
            
            CogPMAlignResults cogPMAlignResults = cogPMAlign.Results;
            if (cogPMAlignResults != null)
            {
                AssignToolResult(cogPMAlign.Name, cogPMAlignResults);
            }

        }
        private void AssignToolResult(CogSearchMaxTool cogSearchMax)
        {
            CogSearchMaxResults cogSearchMaxResults = cogSearchMax.Results;
            if (cogSearchMaxResults != null)
            {
                AssignToolResult(cogSearchMax.Name, cogSearchMaxResults);
            }

        }
        private void AssignToolResult(CogBlobTool cogBlob)
        {
            CogBlobResults cogBlobResults = cogBlob.Results;
            if (cogBlobResults != null)
            {
                AssignToolResult(cogBlob.Name, cogBlobResults);
            }

        }
        private void AssignToolResult(CogHistogramTool cogHistool)
        {
            CogHistogramResult cogHisResults = cogHistool.Result;
            if (cogHisResults != null)
            {
                AssignToolResult(cogHistool.Name, cogHisResults);
            }
        }

        private void AssignToolResult(string parent, CogHistogramResult results)
        {
            AddResult(parent + ".Mean", results.Mean);
        }
        private void AssignToolResult(string parent, CogPMAlignResults results)
        {
            AddResult(parent + ".Count", results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                AddResult(string.Format("{0}[{1}].Score", parent, i), results[i].Score);
                AddResult(string.Format("{0}[{1}].TranslationX", parent, i), results[i].GetPose().TranslationX);
                AddResult(string.Format("{0}[{1}].TranslationY", parent, i), results[i].GetPose().TranslationY);
                AddResult(string.Format("{0}[{1}].Rotation", parent, i), results[i].GetPose().Rotation);
            }
        }
        private void AssignToolResult(string parent, CogSearchMaxResults results)
        {
            AddResult(parent + ".Count", results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                AddResult(string.Format("{0}[{1}].Score", parent, i), results[i].Score);
                AddResult(string.Format("{0}[{1}].TranslationX", parent, i), results[i].GetPose().TranslationX);
                AddResult(string.Format("{0}[{1}].TranslationY", parent, i), results[i].GetPose().TranslationY);
                AddResult(string.Format("{0}[{1}].Rotation", parent, i), results[i].GetPose().Rotation);
            }
        }
        #endregion




        /// <summary>
        /// Assign the input image
        /// </summary>
        /// <param name="cogImage"></param>
        private void AssignInputImage(object oImage)
        {
            if (oImage != null)
            {
                try
                {
                    ICogImage cogImage = oImage as ICogImage;
                    if (cogImage == null)
                    {
                        if (oImage is Bitmap)
                        {
                            // Assume bitmap
                            cogImage = new CogImage24PlanarColor(oImage as Bitmap);
                        }
                        else
                        {
                            return;
                        }
                    }

                    CogToolBlock grabberBuffer = GetGrabberBuffer();
                    if (grabberBuffer != null)
                    {
                        grabberBuffer.Outputs["OutputBuffer"].Value = cogImage;
                    }
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Error AssignInputImage image to the Cog vpp file");
                }
            }
        }

        private CogToolBlock GetGrabberBuffer()
        {
            CogToolBlock grabberBuffer = null;
            foreach (ICogTool cogTool in CogToolGroup.Tools)
            {
                //AssignImageToTool(cogTool, cogImage);
                if (cogTool.Name == "Grabber Buffer" && cogTool is CogToolBlock)
                {
                    grabberBuffer = cogTool as CogToolBlock;
                    _grabberBuffer = grabberBuffer;
                    break;
                }
            }
            if (grabberBuffer == null)
            {
                grabberBuffer = new CogToolBlock();
                grabberBuffer.Name = "Grabber Buffer";
                CogToolBlockTerminal outputTerminal = new CogToolBlockTerminal("OutputBuffer", typeof(ICogImage));
                grabberBuffer.Outputs.Add(outputTerminal);
                CogToolGroup.Tools.Insert(0, grabberBuffer);
            }
            return grabberBuffer;
        }

        private void AssignImageToTool(ICogTool cogTool, ICogImage image)
        {
            Type ty = null;
            PropertyInfo pi = null;
            try
            {
                ty = cogTool.GetType();
                pi = ty.GetProperty("InputImage");
            }
            catch { }
            if (pi == null)
            {
                U.LogPopup("Could not get the Input Image property from tool '{0}'.", cogTool.Name);
            }
            else
            {
                try
                {
                 
                    pi.SetValue(cogTool, image, null);
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Could not set Input Image to '{0}'. The type expected is '{1}' and the type we have is '{2}'", 
                        cogTool.Name, 
                        pi.PropertyType.Name, 
                        image.GetType().Name);
                }
            }
        }

       private void CogToolGroup_OnRan(object sender, EventArgs e)
       {
            ICogRecord lastRunRecord = CogToolGroup.CreateLastRunRecord();
            _autoResetEventWaitToolRun.Set();
            Task.Run(() => AssignLastRunRecord(lastRunRecord));
           
            
            /*
            #region Graphics
            // Graphics
            ICogRecord lastRunRecord = null;
            // Only assign graphic if the display has been created
            CognexCamera8 cogCam = CogCamera;
            if (cogCam != null && cogCam.CogDisplayWindows != null && cogCam.CogDisplayWindows.Count > 0)
            {
                foreach (CogDisplay cogDisplay in cogCam.CogDisplayWindows)
                {

                    if (lastRunRecord == null)
                    {
                        lastRunRecord = CogToolGroup.CreateLastRunRecord();
                    }
                    cogDisplay.Image = null;
                    AssignCogRecordToDisplay(cogDisplay as Control, lastRunRecord);
                }
            }
            
            

            #endregion
            */

            // Release the wait handle
            //_autoResetEventWaitToolRun.Set();
        }

        private void AssignLastRunRecord(ICogRecord lastRunRecord)
        {
            CognexCamera8 cogCam = CogCamera;
            if (cogCam != null && cogCam.CogDisplayWindows != null && cogCam.CogDisplayWindows.Count > 0)
            {

                //Parallel.ForEach<CogDisplay>(cogCam.CogDisplayWindows, currentDisplay =>
                //{

                //    if (lastRunRecord == null)
                //    {
                //        lastRunRecord = CogToolGroup.CreateLastRunRecord();
                //    }
                //    //currentDisplay.Image = null;
                //    //currentDisplay.Invalidate();
                //    AssignCogRecordToDisplay(currentDisplay as Control, lastRunRecord);

                //});

                foreach (CogDisplay cogDisplay in cogCam.CogDisplayWindows)
                {

                    if (lastRunRecord == null)
                    {
                        lastRunRecord = CogToolGroup.CreateLastRunRecord();
                    }
                    //cogDisplay.Image = null;
                    AssignCogRecordToDisplay(cogDisplay as Control, lastRunRecord);
                }
            }
        }


        private delegate void _delParamCogDisplayIcogRecord(Control cogDisplay, ICogRecord cogRecord);
        private void AssignCogRecordToDisplay(Control cogDisplay, ICogRecord cogRecord)
        {
            if(cogDisplay.Visible != true)
            {
                return;
            }

            if (cogDisplay.InvokeRequired)
            {
                cogDisplay.BeginInvoke(new _delParamCogDisplayIcogRecord(AssignCogRecordToDisplay), new object[] { cogDisplay, cogRecord });
                return;
            }

            if (cogDisplay is CogRecordsDisplay)
            {
                (cogDisplay as CogRecordsDisplay).Subject = cogRecord;
            }

            else if (cogDisplay is CogDisplay)
            {
                CogDisplay cDisplay = cogDisplay as CogDisplay;
                try
                {
                    //cogDisplay.Visible = false;
                    if (cogRecord.SubRecords == null || cogRecord.SubRecords.Count == 0)
                    {
                        return;
                    }


                    if (cogRecord.Content != null && cogRecord.Content is List<ICogRecord>)
                    {
                        List<ICogRecord> recordList = cogRecord.Content as List<ICogRecord>;
                        CogGraphicInteractiveCollection cogGraphicInteractCollection = new CogGraphicInteractiveCollection();
                        foreach (ICogRecord record in recordList)
                        {

                            // Assign image if it is not there
                            if ((cDisplay.Image == null || !(cDisplay.Image is CogImage8Grey)) &&
                                 CogCamera != null)
                            {

                                // Assign image 
                                lock (CogCamera.LockCogDisplay)
                                {
                                    cDisplay.Image = record.SubRecords[0].Content as ICogImage;
                                    cDisplay.Fit(true);                                    
                                }

                            }

                            cDisplay.InteractiveGraphics.Clear();

                            if (IsDisplayGraphic)
                            {
                                //Assign graphic if any
                                foreach (ICogRecord subRecord in record.SubRecords[0].SubRecords)
                                {
                                    if (subRecord.Content is CogGraphicCollection)
                                    {
                                        CogGraphicCollection contentCollect = subRecord.Content as CogGraphicCollection;
                                        if (contentCollect != null)
                                        {
                                            foreach (CogCompositeShape graphic in contentCollect)
                                            {
                                                //cDisplay.InteractiveGraphics.Add(graphic, this.Name, false);
                                                cogGraphicInteractCollection.Add(graphic);
                                            }
                                        }

                                    }
                                    if (subRecord.Content is ICogGraphicInteractive)
                                    {
                                        //cDisplay.InteractiveGraphics.Add(subRecord.Content as ICogGraphicInteractive, this.Name, false);
                                        cogGraphicInteractCollection.Add(subRecord.Content as ICogGraphicInteractive);
                                    }
                                }


                                if (EnableDisplayTimeLabel)
                                {
                                    CogGraphicLabel cogLabel = new CogGraphicLabel();
                                    cogLabel.SelectedSpaceName = "@";
                                    cogLabel.Color = CogColorConstants.Green;
                                    cogLabel.SetXYText(TimeLabelLocX, TimeLabelLocY, DateTime.Now.ToString("HH:mm:ss:fff"));
                                    cogGraphicInteractCollection.Add(cogLabel);
                                }

                                cDisplay.InteractiveGraphics.AddList(cogGraphicInteractCollection, this.Name, false);
                                //(cogDisplay as CogDisplay).Invalidate();
                            }

                        }

                    }
                    else
                    {
                        // Assign image if it is not there
                        if ((cDisplay.Image == null || !(cDisplay.Image is CogImage8Grey)) &&
                             CogCamera != null)
                        {

                            // Assign image 
                            lock (CogCamera.LockCogDisplay)
                            {
                                //(cogDisplay as CogDisplay).Image = cogRecord.SubRecords[0].Content as ICogImage;
                                cDisplay.Image = cogRecord.SubRecords[cogRecord.SubRecords.Count - 1].Content as ICogImage;
                                cDisplay.Fit(true);
                                
                            }

                        }

                        cDisplay.InteractiveGraphics.Clear();
                        if (IsDisplayGraphic)
                        {
                            // Assign graphic if any
                            CogGraphicInteractiveCollection cogGraphicInteractCollection = new CogGraphicInteractiveCollection();

                            
                            foreach (ICogRecord record in cogRecord.SubRecords[cogRecord.SubRecords.Count - 1].SubRecords)
                            {
                                if (record.Content is CogGraphicCollection)
                                {
                                    CogGraphicCollection contentCollect = record.Content as CogGraphicCollection;
                                   
                                    foreach (CogObjectBase objBase in contentCollect)
                                    {
                                        ICogGraphicInteractive graphic = objBase as ICogGraphicInteractive;
                                        if (graphic != null)
                                        {
                                            //cDisplay.InteractiveGraphics.Add(graphic, this.Name, false);
                                            cogGraphicInteractCollection.Add(graphic);
                                        }
                                    }
                                }
                                else if (record.Content is ICogGraphicInteractive)
                                {
                                    //cDisplay.InteractiveGraphics.Add(record.Content as ICogGraphicInteractive, this.Name, false);
                                    cogGraphicInteractCollection.Add(record.Content as ICogGraphicInteractive);
                                }
                            }


                            if (EnableDisplayTimeLabel)
                            {
                                CogGraphicLabel cogLabel = new CogGraphicLabel();
                                cogLabel.SelectedSpaceName = "@";
                                cogLabel.Color = CogColorConstants.Green;
                                cogLabel.SetXYText(TimeLabelLocX,TimeLabelLocY, DateTime.Now.ToString("HH:mm:ss:fff"));
                                cogGraphicInteractCollection.Add(cogLabel);
                            }

                            cDisplay.InteractiveGraphics.AddList(cogGraphicInteractCollection, this.Name, false);
                           
                            //(cogDisplay as CogDisplay).Invalidate();
                        }
                    }
                }
                finally
                {
                    //cogDisplay.Visible = true;
                    //cogDisplay.Update();
                }

                //CogDisplayResult =new Bitmap((cogDisplay as CogDisplay).CreateContentBitmap(CogDisplayContentBitmapConstants.Image, null, 0));
               
            }
            else
            {
                // There is no Cognex Display, do nothing
            }
        }
    }
    class ImagePixelLock
    {
        public ICogImage8PixelMemory MemRed, MemGreen, MemBlue;
    }
}
