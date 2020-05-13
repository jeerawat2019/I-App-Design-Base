using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp;
using MDouble;
using MCore.Comp.XFunc;

namespace MCore.Comp.Geometry
{
    public class G3DCompBase : CompBase
    {
        #region Private and Protected

        private MillimetersPerSecond _speed = 0;
        protected Microseconds _pulseWidth = 0.0;

        #endregion Private and Protected

        #region Definitions
        /// <summary>
        /// Component handler
        /// </summary>
        /// <param name="padSection"></param>
        public delegate void G3DCompBaseEventHandler(G3DCompBase g3dComp);

        #endregion Definitions

        #region Public Properties

        /// <summary>
        /// Identify the possible line types supported by this component
        /// </summary>
        public virtual Type[] LineTypes
        {
            get { return new Type[] { typeof(GTriggerPoint) }; }
        }

        /// <summary>
        /// Identify the default line type
        /// </summary>
        public virtual Type DefaultLineType
        {
            get { return typeof(GTriggerPoint); }
        }

        /// <summary>
        /// Get the next G3DCompBase parent
        /// </summary>
        public G3DCompBase Container
        {
            get
            {
                return GetParent<G3DCompBase>();
            }
        }

        public G3DCompBase TransferFuncComp
        {
            get
            {
               G3DCompBase comp = this;
               while (comp.PixToMMX == null)
               {
                   comp = comp.Container;
                   if (comp == null)
                   {
                       return null;
                   }
               }
               return comp;
            }
        }

        /// <summary>
        /// For serializing Speed
        /// </summary>
        [Browsable(false)]
        [XmlElement("Speed")]
        public MillimetersPerSecond SerSpeed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /// <summary>
        /// The Speed for the line
        /// </summary>
        [Browsable(true)]
        [Category("Editor")]
        [Description("The Speed for this Component")]
        [XmlIgnore]
        public virtual MillimetersPerSecond Speed
        {
            get
            {
                if (_speed.Val == 0.0 && (Parent is G3DCompBase))
                {
                    return (Parent as G3DCompBase).Speed;
                }
                return _speed;
            }
            set 
            { 
                _speed = value;
                SetPropValue(() => Speed, value);
            }
        }

        /// <summary>
        /// For serializing PulseWidth
        /// </summary>
        [Browsable(false)]
        [XmlElement("PulseWidth")]
        public Microseconds SerPulseWidth
        {
            get { return _pulseWidth; }
            set { _pulseWidth = value; }
        }

        /// <summary>
        /// The duration of the pulse (us)
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("The duration of the pulse (us)")]
        [XmlIgnore]
        public Microseconds PulseWidth
        {
            get
            {
                if (_pulseWidth.Val == 0.0 && (Parent is G3DCompBase))
                {
                    return (Parent as G3DCompBase).PulseWidth;
                }
                return _pulseWidth;
            }
            set 
            { 
                _pulseWidth = value;
                SetPropValue(() => PulseWidth, value);
            }
        }

        /// <summary>
        /// The Focus point for the editor
        /// Defines editor image center
        /// Range is from 0 to 1000
        /// </summary>
        [Browsable(true)]
        [Category("Editor")]
        [Description("The Focus point for the editor")]
        public Point EditorFocusPt
        {
            get { return GetPropValue(() => EditorFocusPt, new Point(500, 500)); }
            set { SetPropValue(() => EditorFocusPt, value); }
        }

        /// <summary>
        /// The angle of rotation for image editing
        /// </summary>
        [Browsable(true)]
        [Category("Editor")]
        [Description("The angle of rotation for image editing")]
        public Degrees EditorImageAngle
        {
            get { return GetPropValue(() => EditorImageAngle, 90); }
            set { SetPropValue(() => EditorImageAngle, value); }
        }

        /// <summary>
        /// Adjuster for editor
        /// </summary>
        [Browsable(true)]
        [Category("Editor")]
        [Description("Adjuster value")]
        public Millimeters Adjuster
        {
            get { return GetPropValue(() => Adjuster, 0); }
            set { SetPropValue(() => Adjuster, value); }
        }


        /// <summary>
        /// The target ZHt for dispensing (measured by sensor)
        /// </summary>
        [Browsable(true)]
        [Category("Carriers")]
        [Description("The Target ZHt")]
        public Millimeters DispenseZHtTarget
        {
            get { return GetPropValue(() => DispenseZHtTarget, new Millimeters(-0.5)); }
            set { SetPropValue(() => DispenseZHtTarget, value); }
        }

        /// <summary>
        /// The editor Zoom Scale
        /// </summary>
        [Browsable(true)]
        [Category("Editor")]
        [Description("The editor Zoom Scale")]
        public double EditorZoom
        {
            get { return GetPropValue(() => EditorZoom, 1.0); }
            set { SetPropValue(() => EditorZoom, value); }
        }

        public enum eLineFormats { All_lengths_equal, All_Vertical, All_Horizontal, All_Centered_X, 
            All_Centered_Y, Pairs_Vert_Spaced, Pairs_Z_Spaced, Snap_Lines, Snap_Triggers, MinSpacing_Triggers };

        /// <summary>
        /// Indicates if is master
        /// </summary>
        [Browsable(true)]
        public bool IsMaster
        {
            get { return GetPropValue(() => IsMaster, false); }
            set { SetPropValue(() => IsMaster, value); }
        }


        /// <summary>
        /// The XYZ location or offset
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        public G3DPoint Loc
        {
            get { return GetPropValue(() =>Loc); }
            set { SetPropValue(() =>Loc, value); }
        }

        /// <summary>
        /// The width of the object (optional)
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("The width of the object (optional)")]
        public Millimeters Width
        {
            get { return GetPropValue(() => Width); }
            set { SetPropValue(() => Width, value); }
        }

        /// <summary>
        /// The height of the object (optional)
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("The height of the object (optional)")]
        public Millimeters Height
        {
            get { return GetPropValue(() => Height); }
            set { SetPropValue(() => Height, value); }
        }

        protected string GetImageFilepath(string dataName)
        {
            string strID = ID;
            string rootName = strID.Split('.')[0];
            return string.Format(@"{0}\{1}\{2}\{3}.jpg", U.RootComp.RootFolder, rootName, dataName, strID);
        }

        [Browsable(true)]
        public string VisionImageFilePath
        {
            get { return GetPropValue(() => VisionImageFilePath); }
            set { SetPropValue(() => VisionImageFilePath, value); }            
        }

        /// <summary>
        /// True if image is protected from being replaced
        /// </summary>
        [Browsable(true)]
        [Description("True if image is protected from being replaced")]
        public bool VisionImageLocked
        {
            get { return GetPropValue(() => VisionImageLocked); }
            set { SetPropValue(() => VisionImageLocked, value); }
        }

        private ReaderWriterLockSlim __imageFileLock = new ReaderWriterLockSlim();

        public override void Dispose()
        {
            base.Dispose();
            VisionImage = null;
        }
        /// <summary>
        /// Unload the image from memory
        /// </summary>
        public void UnloadVisionImage()
        {
            if (_loadedImage != null)
            {
                lock (_loadedImage)
                {
                    _loadedImage.Dispose();
                    _loadedImage = null;
                }
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Image VisionImageCopy
        {
            get
            {
                if (_loadedImage == null)
                {
                    Image loaded = VisionImage;
                    _loadedImage = null;
                    return loaded;
                }

                //Stopwatch sw = Stopwatch.StartNew();
                //GC.Collect();
                //System.Diagnostics.Debug.WriteLine(string.Format("GC.Collect time= {0} ms, {1}", sw.ElapsedMilliseconds, "VisionImageCopy"));
                lock (_loadedImage)
                {
                    return new Bitmap(_loadedImage);
                }
            }
        }

        ///// <summary>
        ///// Only save the image.  Do not copy the image into memory
        ///// </summary>
        ///// <param name="image"></param>
        //public void SaveVisionImage(Image image)
        //{
        //    if (!VisionImageLocked)
        //    {
        //        DeleteVisionImageFile();
        //        if (image != null)
        //        {
        //            string filepath = GetImageFilepath("VisionImage");
        //            U.EnsureDirectory(filepath);
        //            lock (image)
        //            {
        //                // Save Image
        //                __imageFileLock.EnterWriteLock();
        //                try
        //                {
        //                    image.Save(filepath);
        //                }
        //                finally
        //                {
        //                    __imageFileLock.ExitWriteLock();
        //                }
        //                VisionImageFilePath = filepath;
        //            }
        //        }
        //    }
        //}

        public void SetVisionImage(Image image)
        {
            VisionImage = image;
        }

        [Browsable(false)]
        [XmlIgnore]
        public Image VisionImage
        {
            get 
            {
                if (_loadedImage != null)
                {
                    return _loadedImage;
                }
                string filepath = VisionImageFilePath;
                __imageFileLock.EnterReadLock();
                try
                {
                    if (File.Exists(filepath))
                    {
                        Bitmap bm = Bitmap.FromFile(filepath) as Bitmap;
                        //Stopwatch sw = Stopwatch.StartNew();
                        //GC.Collect();
                        Debug.WriteLine(string.Format("Loaded image '{0}' for '{1}'", filepath, ID));
                        _loadedImage = new Bitmap(bm);
                        bm.Dispose();
                        return _loadedImage;
                    }
                }
                finally
                {
                    __imageFileLock.ExitReadLock();
                }
                return null; 
            }
            set 
            {
                if (VisionImageLocked)
                {
                    return;
                }
                DeleteVisionImageFile();
                if (value != null)
                {
                    //_rawVisionImage = null;
                    string filepath = GetImageFilepath("VisionImage");
                    U.EnsureDirectory(filepath);
                    // Save Image
                    __imageFileLock.EnterWriteLock();
                    try
                    {
                        value.Save(filepath, ImageFormat.Jpeg);
                    }
                    finally
                    {
                        __imageFileLock.ExitWriteLock();
                    }
                    VisionImageFilePath = filepath;
                    //_loadedImage = value;
                }
            }
        }

        protected void DeleteImageFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                if (filepath.Contains("Carrier History.Carrier"))
                {
                    string fileNewPath = filepath.Replace("Carrier History.Carrier", "Carrier_History.Carrier");
                    if (File.Exists(fileNewPath))
                    {
                        File.Delete(fileNewPath);
                    }
                    File.Move(filepath, fileNewPath);
                }
                else
                {
                    File.Delete(filepath);
                }
            }
        }
        /// <summary>
        /// Delete the image file from disk
        /// </summary>
        private void DeleteVisionImageFile()
        {
            string filepath = VisionImageFilePath;
            __imageFileLock.EnterWriteLock();
            try
            {
                DeleteImageFile(filepath);
                UnloadVisionImage();
            }
            catch { }
            finally
            {
                __imageFileLock.ExitWriteLock();
            }
            VisionImageFilePath = string.Empty;
        }
        private Image _loadedImage = null;

        protected override void OnAdded()
        {
            if (!string.IsNullOrEmpty(VisionImageFilePath))
            {
                string filepath = GetImageFilepath("VisionImage");
                if (File.Exists(VisionImageFilePath) && VisionImageFilePath != filepath)
                {
                    __imageFileLock.EnterWriteLock();
                    try
                    {
                        U.EnsureDirectory(filepath);
                        // Move the file
                        File.Move(VisionImageFilePath, filepath);
                        VisionImageFilePath = filepath;
                    }
                    catch (Exception ex)
                    {
                        U.LogError(ex, "Unable to move image file: '{0}'", VisionImageFilePath);
                    }
                    finally
                    {
                        __imageFileLock.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Get the tool offset to be applied to the true robot location
        /// </summary>
        public virtual G3DCompBase ToolComp
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// The delay used adjust for trajectory lead
        /// (Multiply this with speed to get the lead distance);
        /// </summary>
        public Microseconds ToolDelay
        {
            get { return GetPropValue(() => ToolDelay); }
            set { SetPropValue(() => ToolDelay, value); }
        }

        /// <summary>
        /// Return the final robot commaded position
        /// </summary>
        public G3DPoint RobotLoc
        {
            get
            {   
                return GetRobotLoc(true);
            }
        }

        /// <summary>
        /// Get the absolute robot coordinates for the Loc
        /// </summary>
        /// <param name="applyToolOffset"> Ad the too offset to adjust for tool moounting position</param>
        /// <returns></returns>
        public virtual G3DPoint GetRobotLoc(bool applyToolOffset)
        {
            return GetRobotLoc(0.0, applyToolOffset);
        }

        private G3DCompBase _offsetComp = null;
        private volatile bool _offsetCompCheck = false;
        /// <summary>
        /// Get the absolute robot coordinates (Used for Line elements: Trigger)
        /// </summary>
        /// <param name="distance">Distance along the Loc vector (angle of Yaw) </param>
        /// <param name="applyToolOffset"> Ad the too offset to adjust for tool moounting position</param>
        /// <returns></returns>
        public G3DPoint GetRobotLoc(Millimeters distance, bool applyToolOffset)
        {
            if (Loc.XYYawMode == G3DPoint.eMode.Absolute)
            {
                double x = distance * Math.Cos(Loc.Yaw);
                double y = distance * Math.Sin(Loc.Yaw);
                if (Loc.ZMode == G3DPoint.eMode.Absolute)
                {
                    return new G3DPoint(Loc.X + x, Loc.Y + y, Loc.Z);
                }
                else
                {
                    return new G3DPoint(Loc.X + x, Loc.Y + y, 0);
                }
            }

            G3DPoint pt = TransformXY(distance, 0);

            if (Loc.ZMode == G3DPoint.eMode.Absolute)
            {
                pt.Z.Val = Loc.Z;
            }
            else
            {
                pt.Z.Val = TransformZ(0);
            }

            if (applyToolOffset && ToolComp != null)
            {
                if (_offsetComp == null && _offsetCompCheck == false)
                {
                    _offsetComp = ToolComp.FilterByTypeSingle<G3DCompBase>();
                    _offsetCompCheck = true;
                }
                if (_offsetComp != null)
                {
                    pt.X += _offsetComp.Loc.X;
                    pt.Y += _offsetComp.Loc.Y;
                    if (Loc.ZMode == G3DPoint.eMode.Relative)
                    {
                        pt.Z += _offsetComp.Loc.Z;
                    }
                }
            }
            return pt;

        }

        /// <summary>
        /// Convert the location to pixel space, Does not app
        /// </summary>
        /// <param name="applyTrajLead"></param>
        [Browsable(false)]
        public PointF ToPixel(bool applyToolOffset)
        {
            return AbsPointToPixel(GetRobotLoc(applyToolOffset));
        }

        /// <summary>
        /// Convert the location to pixel space, Does not app
        /// </summary>
        /// <param name="applyTrajLead"></param>
        [Browsable(false)]
        public PointF ToPixel(Millimeters X, Millimeters Y)
        {
            return AbsPointToPixel(TransformXY(X, Y));
        }

        
        /// <summary>
        /// Array of references
        /// </summary>
        [Browsable(true)]
        [DisplayName("ImageReferences")]
        [XmlIgnore]
        public GReference[] ImageReferenceArray
        {
            get
            {
                return FilterByType<GReference>();
            }
        }

        /// <summary>
        /// Transfer function used to Convert pixel to millimeters
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public BasicLinear PixToMMX { get; set; }
        [Browsable(false)]
        [XmlIgnore]
        public BasicLinear PixToMMY { get; set; }
        [Browsable(false)]
        [XmlIgnore]
        public BasicLinear MMToPixX { get; set; }
        [Browsable(false)]
        [XmlIgnore]
        public BasicLinear MMToPixY { get; set; }

        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public G3DCompBase()
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public G3DCompBase(string name)
            : base(name)
        {
            Loc = new G3DPoint();
        }
        #endregion Constructors

         /// <summary>
        /// Make a copy
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bRecursivley"></param>
        /// <returns></returns>
        public override CompBase Clone(string name, bool bRecursivley)
        {
            G3DCompBase newComp = base.Clone(name, bRecursivley) as G3DCompBase;
            newComp.Loc = new G3DPoint(Loc);
            newComp._speed.Val = _speed;
            newComp._pulseWidth.Val = _pulseWidth;
            newComp.ToolDelay.Val = ToolDelay;
            newComp.DispenseZHtTarget = DispenseZHtTarget;
            newComp.EditorZoom = EditorZoom;
            newComp.EditorFocusPt = EditorFocusPt;
            newComp.EditorImageAngle = EditorImageAngle;
            return newComp;
        }

        /// <summary>
        ///  Return the Trigger point size in mm
        /// </summary>
        /// <returns></returns>        
        public virtual Millimeters TriggerPointRadius
        {
            get { return GetPropValue(() => TriggerPointRadius, new Millimeters(0.125)); }
            set { SetPropValue(() => TriggerPointRadius, value); }
        }
        /// <summary>
        /// Set the XY Offset values
        /// Assumes parent contains absolute location
        /// </summary>
        /// <param name="xCurrentPos"></param>
        /// <param name="yCurrentPos"></param>
        public void ApplyXYOffset(Millimeters xCurrentPos, Millimeters yCurrentPos)
        {
            // Get parent
            if (Container != null)
            {
                // And set it
                Loc.X.Val = xCurrentPos - Container.Loc.X;
                Loc.Y.Val = yCurrentPos - Container.Loc.Y;
            }
        }
        /// <summary>
        /// Set the Location according to an image correction from the center of the image
        /// </summary>
        /// <param name="pixelX"></param>
        /// <param name="pixelY"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public G3DPoint ConvertFromPixels(double pixelX, double pixelY, Radians theta)
        {
            G3DCompBase compTransferFuncs = TransferFuncComp;
            if (compTransferFuncs != null && compTransferFuncs.PixToMMX != null && compTransferFuncs.PixToMMX != null)
            {
                Loc.X.Val = compTransferFuncs.PixToMMX.Evaluate(pixelX, pixelY);
                Loc.Y.Val = compTransferFuncs.PixToMMY.Evaluate(pixelX, pixelY);
                Loc.Yaw = -theta;
            }
            return Loc;
        }

        /// <summary>
        /// Convert Pixel point to MM in PointF
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public PointF ConvertFromPixels(PointF pt)
        {
            G3DCompBase compTransferFuncs = TransferFuncComp;
            if (compTransferFuncs != null && compTransferFuncs.PixToMMX != null && compTransferFuncs.PixToMMX != null)
            {
                float mmX = (float)compTransferFuncs.PixToMMX.Evaluate(pt.X, pt.Y);
                float mmY = (float)compTransferFuncs.PixToMMY.Evaluate(pt.X, pt.Y);
                return new PointF(mmX, mmY);
            }
            return PointF.Empty;
        }


        /// <summary>
        /// Reverse the Zooming
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public PointF DeZoomPt(PointF pt)
        {
            return new PointF((float)(pt.X / EditorZoom), (float)(pt.Y / EditorZoom));
        }
        /// <summary>
        /// Zoom the point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public PointF ZoomPt(PointF pt)
        {
            return new PointF((float)(pt.X * EditorZoom), (float)(pt.Y * EditorZoom));
        }
        /// <summary>
        /// Convert the specific point to pixel space
        /// </summary>
        /// <param name="absPt"></param>
        /// <returns></returns>
        public PointF AbsPointToPixel(G3DPoint absPt)
        {
            return AbsPointToPixel(absPt.X, absPt.Y);
        }
        /// <summary>
        /// Convert the specific point to pixel space
        /// </summary>
        /// <param name="absPt"></param>
        /// <returns></returns>
        public PointF AbsPointToPixel(double x, double y)
        {
            PointF retPt = PointF.Empty;
            G3DCompBase compTransferFuncs = TransferFuncComp;
            if (compTransferFuncs != null && compTransferFuncs.MMToPixX != null && compTransferFuncs.MMToPixX != null)
            {
                retPt.X = (float)compTransferFuncs.MMToPixX.Evaluate(x, y);
                retPt.Y = (float)compTransferFuncs.MMToPixY.Evaluate(x, y);
                return retPt;
            }
            return retPt;
        }

        public double TotalAngle
        {
            get
            {
                G3DCompBase container = Container;
                double rd = 0;
                while (container != null && container.Loc.XYYawMode == G3DPoint.eMode.Relative)
                {
                    rd -= container.Loc.Yaw.Val;
                    container = container.Container;
                }
                return rd;
            }
        }

        /// <summary>
        /// Transform a point Z to the 
        /// </summary>
        /// <param name="Z"></param>
        /// <returns></returns>
        public Millimeters TransformZ(Millimeters Z)
        {
            G3DCompBase reference = Container;
            Z += Loc.Z;
            if (reference == null || Loc.ZMode == G3DPoint.eMode.Absolute)
            {
                return Z;
            }
            return reference.TransformZ(Z);
        }

        /// <summary>
        /// Transform a point (x,y) to the 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public G3DPoint TransformXY(Millimeters X, Millimeters Y)
        {
            G3DCompBase reference = Container;
            if (Loc.XYYawMode == G3DPoint.eMode.Absolute)
            {
                return new G3DPoint(Loc.X + X, Loc.Y + Y, 0.0);
            }
            G3DPoint outerPt = outerPt = new G3DPoint(Loc);
            double dist = Math.Sqrt(X * X + Y * Y);
            double theta = Math.Atan2(Y, X);
            outerPt.X += dist * Math.Cos(theta + Loc.Yaw);
            outerPt.Y += dist * Math.Sin(theta + Loc.Yaw);
            if (reference == null)
            {
                return outerPt;
            }
            return reference.TransformXY(outerPt.X, outerPt.Y);
        }
        /// <summary>
        /// Access to a specific reference child
        /// If it dosen't exit, it is created
        /// </summary>
        public G3DCompBase GetRef(string name)
        {
            G3DCompBase reference = this[name] as GReference;
            if (reference == null)
            {
                reference = new GReference(name);
                Add(reference);
            }
            return reference;
        }

     }
}
