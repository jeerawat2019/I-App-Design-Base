using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;

using MDouble;
using MCore;
using MCore.Comp.XFunc;

namespace MCore.Comp.IOSystem.Input
{
    public class Array2DInput : InputBase
    {

        private ManualResetEvent _waitMeasureDone = new ManualResetEvent(true);
        private int _currentImage = 0;
        protected BasicLinear _xferFuncClearence = null;
        public const double CountsPerMicron = 100.0;
        const double CountsPerMillimeter = 100000.0;
        const double GreyScaleRange = 256.0;


        /// <summary>
        /// Return true if needs instantiation
        /// </summary>
        [Browsable(true)]
        [XmlIgnore]
        public bool NeedsInstantiation
        {
            get
            {
                return _dataList.Count < NumImages;
            }
        }
        [Browsable(true)]
        [XmlIgnore]
        public int CurrentImage
        {
            get { return _currentImage; }
            set { _currentImage = value; }
        }

        [XmlIgnore]
        private List<double[,]> _dataList = new List<double[,]>();

        public delegate void delArray2DInputInt(Array2DInput array2DInput, int iData);
        public event D.BitmapEventHandler OnAcquisitionComplete = null;
        public event delArray2DInputInt OnSingleImageAcquisitionComplete = null;
        public const string XFerFuncClear = "Z Clearance (mm)";
        public const string XFerFuncCorr = "Z Correction (mm)";

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public double[,] Data
        {
            get
            {
                if (CurrentImage < _dataList.Count)
                {
                    return _dataList[CurrentImage];
                }
                return null;
            }
        }

        #region Public Properties
        /// <summary>
        /// The ZHt transfer function for Gap between Needle and surface
        /// </summary>
        [Browsable(false)]
        public BasicLinear XferFuncClear
        {
            get
            {
                if (_xferFuncClearence != null)
                {
                    return _xferFuncClearence;
                }
                _xferFuncClearence = FilterByTypeSingle<BasicLinear>();
                return _xferFuncClearence;
            }
        }
        /// <summary>
        /// The uinque ID for this data acquisition
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("The uinque ID for this data acquisition")]
        [XmlIgnore]
        public uint DataId
        {
            get { return GetPropValue(() => DataId, uint.MaxValue); }
            set { SetPropValue(() => DataId, value); }
        }

        /// <summary>
        /// Last measure
        /// </summary>
        [Browsable(true)]
        [Category("Array 2D")]
        public Millimeters LastGapMeasure
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => LastGapMeasure, 0); }
            set { SetPropValue(() => LastGapMeasure, value); }
        }

        /// <summary>
        /// Z Offset
        /// </summary>
        [Browsable(true)]
        [Category("Array 2D")]
        public Millimeters ZOffset
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ZOffset, 0); }
            set { SetPropValue(() => ZOffset, value); }
        }

        [Browsable(true)]
        [Category("Array 2D")]
        public Microns ZRefMargin
        {
            get { return GetPropValue(() => ZRefMargin, 50); }
            set { SetPropValue(() => ZRefMargin, value); }
        }
        [Browsable(true)]
        [Category("Array 2D")]
        public Microns ZRefMarginRaw
        {
            get { return ZRefMargin * CountsPerMicron; }
        }
        [Browsable(true)]
        [Description("Microns per grayscale value")]
        [Category("Array 2D")]
        public Microns Precision
        {
            get { return GetPropValue(() => Precision, 1); }
            set { SetPropValue(() => Precision, value); }
        }
        /// <summary>
        /// The number of rows
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("The number of rows")]
        public int NumRows
        {
            get { return GetPropValue(() => NumRows, 400); }
            set { SetPropValue(() => NumRows, value); }
        }

        /// <summary>
        /// The number of images
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("The number of images")]
        public int NumImages
        {
            get { return GetPropValue(() => NumImages, 1); }
            set { SetPropValue(() => NumImages, value); }
        }

        /// <summary>
        /// The number of degrees to rotate
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]        
        [Description("Rotate the image\n" +
    "   0: No rotate\n" +
    "   -90: rotate counterclockwise 90 deg\n" +
    "   90: rotate clockwise 90 deg\n" +
    "   180: rotate clockwise 180 deg\n"
        )]
        public int Rotate
        {
            get { return GetPropValue(() => Rotate, 0); }
            set { SetPropValue(() => Rotate, value); }
        }

        /// <summary>
        /// The number of profiles to ignore before recording
        /// </summary>
        [Category("Layout"), Browsable(true), Description("The number of profiles to ignore before recording")]
        public int NumProfilesToIgnore
        {
            get { return GetPropValue(() => NumProfilesToIgnore, 0); }
            set { SetPropValue(() => NumProfilesToIgnore, value); }
        }

        /// <summary>
        /// The number of cols
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("The number of cols")]
        public int NumCols
        {
            get { return GetPropValue(() => NumCols, 400); }
            set { SetPropValue(() => NumCols, value); }
        }
        /// <summary>
        /// Min Acceptable Value
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("Min Acceptable Value")]
        public double MinAcceptableValue
        {
            get { return GetPropValue(() => MinAcceptableValue, -5000.0); }
            set { SetPropValue(() => MinAcceptableValue, value); }
        }

        /// <summary>
        /// Max Acceptable Value
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("Max Acceptable Value")]
        public double MaxAcceptableValue
        {
            get { return GetPropValue(() => MaxAcceptableValue, 5000.0); }
            set { SetPropValue(() => MaxAcceptableValue, value); }
        }

        /// <summary>
        /// Actual Min Raw Value
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("Actual Min Raw Value")]
        [XmlIgnore]
        public double MinActualRawValue
        {
            get { return GetPropValue(() => MinActualRawValue, double.MaxValue); }
            set { SetPropValue(() => MinActualRawValue, value); }
        }

        /// <summary>
        /// Actual Max Raw Value
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("Actual Max Raw Value")]
        [XmlIgnore]
        public double MaxActualRawValue
        {
            get { return GetPropValue(() => MaxActualRawValue, double.MinValue); }
            set { SetPropValue(() => MaxActualRawValue, value); }
        }


        /// <summary>
        /// Duration of the acquisition
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("Duration of the acquisition")]
        public Miliseconds Duration
        {
            get { return GetPropValue(() => Duration); }
            set { SetPropValue(() => Duration, value); }
        }

        /// <summary>
        /// Correction Slope
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("Correction Slope")]
        public double CorrectionSlope
        {
            get { return GetPropValue(() => CorrectionSlope, 1.0); }
            set { SetPropValue(() => CorrectionSlope, value); }
        }

        /// <summary>
        /// Correction Intercept
        /// </summary>        
        [Browsable(true)]
        [Category("Array 2D")]
        [Description("Correction Intercept")]
        public Microns CorrectionIntercept
        {
            get { return GetPropValue(() => CorrectionIntercept, 0); }
            set { SetPropValue(() => CorrectionIntercept, value); }
        }

        /// <summary>
        /// Dump the Raw Data
        /// </summary>
        [Browsable(true)]
        [Category("Array 2D")]
        public bool DumpRawData
        {
            get { return GetPropValue(() => DumpRawData); }
            set { SetPropValue(() => DumpRawData, value); }
        }
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Array2DInput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public Array2DInput(string name)
            : base(name)
        {
        }
        #endregion Constructors


        #region Overrides

        /// <summary>
        /// Reset this component
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            MinActualRawValue = double.MaxValue;
            MaxActualRawValue = double.MinValue;
            _dataList.Clear();

            if (XferFuncClear != null)
            {
                XferFuncClear.Reset();
            }
        }
        public override void Trigger()
        {
            _dataList.Clear();
            lock (_waitMeasureDone)
            {
                _waitMeasureDone.Reset();
                _ioSystem.Trigger(this);
            }
            TriggerMode = eTriggerMode.Idle;
        }
        /// <summary>
        /// Opportunity to do any ID referencing for this class object
        /// Occurs after Initialize
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            U.RegisterOnChanged(() => TriggerMode, OnTriggerModeChanged);
        }

        /// <summary>
        /// Do the measure, trigger and wait for result
        /// </summary>
        /// <param name="timeout"></param>
        public override bool Measure(Miliseconds timeout)
        {
            Trigger();
            try
            {
                U.BlockOrDoEvents(_waitMeasureDone, timeout.ToInt);
                return true;
            }
            catch
            {
                U.LogError("Timeout waiting for measure of '{0}'", Nickname);
                return false;
            }
        }

        #endregion Overrides

        private void OnTriggerModeChanged(eTriggerMode triggerMode)
        {
            switch (triggerMode)
            {
                case eTriggerMode.SingleTrigger:
                    Trigger();
                    break;
            }
        }
        /// <summary>
        ///  Convert the grey scale value and set and get the measurement in mm
        /// </summary>
        /// <param name="ZHtGreyScale"></param>
        /// <returns></returns>
        public double ConvertGreyScaleToRawAbs(double ZHtGreyScale)
        {
            return ConvertGreyScaleToRawAbs(ZHtGreyScale, MaxAcceptableValue, MinAcceptableValue);
        }

        /// <summary>
        /// Convert the Grey scale to raw
        /// </summary>
        /// <param name="ZHtGreyScale"></param>
        /// <param name="maxData"></param>
        /// <param name="minData"></param>
        /// <returns></returns>
        public double ConvertGreyScaleToRawAbs(double ZHtGreyScale, double maxData, double minData)
        {
            double actualRawValue = ConvertGreyScaleToRawDel(ZHtGreyScale, maxData, minData) + minData;
            MaxActualRawValue = Math.Max(actualRawValue, MaxActualRawValue);
            MinActualRawValue = Math.Min(actualRawValue, MinActualRawValue);
            return actualRawValue;
        }

        /// <summary>
        /// Convert the Grey scale to raw
        /// </summary>
        /// <param name="ZHtGreyScale"></param>
        /// <returns></returns>
        public double ConvertGreyScaleToRawDel(double ZHtGreyScale)
        {
            return ConvertGreyScaleToRawDel(ZHtGreyScale, MaxAcceptableValue, MinAcceptableValue);
        }
        /// <summary>
        /// Convert the Grey scale to raw
        /// </summary>
        /// <param name="ZHtGreyScale"></param>
        /// <param name="maxData"></param>
        /// <param name="minData"></param>
        /// <returns></returns>
        public double ConvertGreyScaleToRawDel(double ZHtGreyScale, double maxData, double minData)
        {
            double scale = (maxData - minData) / GreyScaleRange;
            return ZHtGreyScale * scale;
        }

        /// <summary>
        /// Get the Raw Target Range
        /// </summary>
        /// <returns></returns>
        public double GetRawTargetRange()
        {
            return GreyScaleRange * Precision * CountsPerMicron;
        }

        /// <summary>
        /// Get the offset from reference in microns
        /// Apply any defined correction
        /// </summary>
        /// <param name="greyScale"></param>
        /// <param name="planarDatum"></param>
        /// <param name="max2"></param>
        /// <param name="min2"></param>
        /// <returns></returns>
        public double GetMicronOffsetFromReference(double greyScale, double greyScaleX, double greyScaleY, BasicLinear planarDatum, double max2, double min2)
        {
            double greyScaleRef = planarDatum.Evaluate(greyScaleX, greyScaleY);

            double micronOffset = ConvertGreyScaleToRawDel(greyScale - greyScaleRef, max2, min2) / CountsPerMicron;
            return micronOffset * CorrectionSlope + CorrectionIntercept;

        }
        /// <summary>
        ///  Convert the grey scale value and set and get the measurement in mm
        /// </summary>
        /// <param name="zRawVal"></param>
        /// <returns></returns>
        public Millimeters ConvertRawToMMGap(double zRawVal)
        {
            if (XferFuncClear != null && XferFuncClear.Valid)
            {
                LastGapMeasure = XferFuncClear.Evaluate(zRawVal);
            }
            else
            {
                LastGapMeasure = zRawVal/CountsPerMillimeter;
            }
            return LastGapMeasure + ZOffset;
        }

        /// <summary>
        /// Instantiate for the data
        /// </summary>
        public void Instantiate()
        {
            _dataList.Clear();
            long startTime = U.DateTimeNow;
            int nImages = NumImages;
            int nRows = NumRows;
            int nCols = NumCols;
            for (int i = 0; i < nImages; i++)
            {
                _dataList.Add(new double[nRows, nCols]);
            }
            _iProfile = 0;
            _iLastData = -1;
            double ms = U.TicksToMS(U.DateTimeNow - startTime);
            U.LogInfo("{0},Instantiate,{1}", Name, ms);
        }
        int _iProfile = 0;
        int _iLastData = -1;
        /// <summary>
        /// Acquisition has data for us.  Store it
        /// </summary>
        /// <param name="oneProfile"></param>
        public void ReceiveOneProfile(int[] oneProfile)
        {
            try
            {
                int index = _iProfile - NumProfilesToIgnore;
                if (index >= 0)
                {
                    int nRows = NumRows;
                    int nCols = NumCols;
                    int iData = index / nRows;
                    if (_iLastData >= 0 & _iLastData != iData && OnSingleImageAcquisitionComplete != null)
                    {
                        OnSingleImageAcquisitionComplete(this, _iLastData);
                    }
                    _iLastData = iData;
                    int iRow = index % nRows;
                    if (iData < _dataList.Count)
                    {
                        double[,] data = _dataList[iData];

                        int len = Math.Min(oneProfile.Length-6, nCols);
                        for (int i = 0; i < len; i++)
                        {
                            data[iRow, i] = oneProfile[i+6];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            _iProfile++;
        }

        /// <summary>
        /// Stop the measuring
        /// </summary>
        public void StopMeasure()
        {
            _ioSystem.StopMeasure(this);
            
        }
        /// <summary>
        /// Call back acquisition completed
        /// Data array should be filled with all acquisitions
        /// </summary>
        /// <param name="errorMsg">empty is succesful</param>
        public void AcquisitionComplete(string errorMsg)
        {
            lock (_waitMeasureDone)
            {
                _waitMeasureDone.Set();
            }
            if (OnAcquisitionComplete != null)
            {

                if (string.IsNullOrEmpty(errorMsg))
                {
                    OnAcquisitionComplete(BuildBitmap(MaxAcceptableValue, MinAcceptableValue));
                }
                else
                {
                    U.LogError(errorMsg);
                    OnAcquisitionComplete(null);
                }
            }
        }

        /// <summary>
        /// Read from file and populate Data array
        /// </summary>
        /// <param name="filename"></param>
        public bool ReadFromFile(string filename)
        {
            try
            {
                int row = 0;
                int len = 0;
                double[,] data = Data;
                using (TextReader tr = File.OpenText(filename))
                {
                    string line = tr.ReadLine();
                    while (!string.IsNullOrEmpty(line))
                    {
                        string[] arr = line.Split(',');
                        len = arr.Length;
                        while (string.IsNullOrEmpty(arr[len - 1]))
                        {
                            len--;
                        }

                        if (len != NumCols || _dataList.Count == 0 || data == null)
                        {
                            NumCols = len;
                            Instantiate();
                            data = Data;
                        }
                        for (int i = 0; i < len; i++)
                        {
                            double dVal = Convert.ToDouble(arr[i]);
                            data[row, i] = dVal;
                        }
                        line = tr.ReadLine();
                        row++;
                        if (row >= NumRows)
                            break;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                U.LogPopup(ex, "Error reading '{0}'", filename);
                return false;
            }
        }

        /// <summary>
        /// Write current data array to file
        /// </summary>
        /// <param name="filename"></param>
        public void SaveToFile(string filename)
        {
            try
            {
                U.EnsureDirectory(filename);
                double[,] data = Data;
                using (TextWriter tw = new StreamWriter(filename, false))
                {
                    int nRows = NumRows;
                    int nCols = NumCols;
                    for (int iRow = 0; iRow < nRows; iRow++)
                    {
                        string line = string.Empty;
                        for (int iCol = 0; iCol < nCols; iCol++)
                        {
                            if (iCol > 0)
                            {
                                line += ",";
                            }
                            line += Convert.ToString(data[iRow, iCol]); 
                        }
                        tw.WriteLine(line);
                        if ((iRow % 100) == 0)
                        {
                            U.SleepWithEvents(10);
                        }
                    }
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Could not write file");
            }
        }
        /// <summary>
        /// Return a single value from the entire array
        /// </summary>
        [Browsable(false)]
        public int SingleRawValue
        {
            get
            {
                // Get average of all values;
                double[,] data = Data;
                double dVal = 0;
                int nRows = NumRows;
                int nCols = NumCols;
                int nVals = 0;
                for (int iRow = 10; iRow < 20; iRow++)
                {
                    string line = string.Empty;
                    for (int iCol = nCols/4; iCol < (nCols*3)/4; iCol++)
                    {
                        double d = data[iRow, iCol];
                        if (d != 0 && d != -2147483645)
                        {
                            dVal += d;
                            nVals++;
                        }
                    }                    
                }
                if (nVals == 0)
                {
                    dVal = -2147483645;
                }
                else
                {
                    dVal /= nVals;
                }
                return (int)dVal;
            }
        }
        /// <summary>
        /// Convert raw value at X,Y to grey scale
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public byte CoarseRawToGreyscale(int x, int y)
        {
            double scale = 256.0 / (MaxAcceptableValue - MinAcceptableValue + 1);
            double[,] data = Data;
            double dVal = data != null ? data[y, x] : 0.0;
            dVal = Math.Max(dVal, MinAcceptableValue);
            dVal = Math.Min(dVal, MaxAcceptableValue);
            // Let zero be the floor
            dVal -= MinAcceptableValue;
            // Let 255 be the max
            byte color = (byte)(dVal * scale);
            return color;
        }

        //public void FromBitmap(Bitmap bm)
        //{
        //    CurrentImage = Math.Max(CurrentImage, 0);
        //    double scale = (MaxAcceptableValue - MinAcceptableValue + 1) / 256.0;
        //    int nRows = NumRows;
        //    int nCols = NumCols;
        //    while (_dataList.Count <= CurrentImage)
        //    {
        //        _dataList.Add(new double[nRows, nCols]);
        //    }
        //    double[,] data = _dataList[CurrentImage];
        //    for (int x = 0; x < bm.Width; x++)
        //    {
        //        for (int y = 0; y < bm.Height; y++)
        //        {
        //            data[x, y] = bm.GetPixel(x, y).R * scale + MinAcceptableValue;
        //        }
        //    }
        //}

        public Bitmap BuildBitmap(double maxAcceptableValue, double minAcceptable)
        {

            if (_dataList.Count == 0)
            {
                return null;
            }

            int rotate = Rotate;
            // Create Bitmap
            //Cursor origCursor = Cursor.Current;
            //Cursor.Current = Cursors.WaitCursor;

            long startTime = U.DateTimeNow;
            int numRows = NumRows;
            int numCols = NumCols;
            if (rotate == -90 || rotate == 90)
            {
                numRows = NumCols;
                numCols = NumRows;
            }

            Bitmap bm = new Bitmap(numCols, numRows, PixelFormat.Format24bppRgb); // @"D:\PDLib\Components\IOSystems\Keyence_LJ_V7001\KeyData.bmp");//
           
            PropertyItem[] props = bm.PropertyItems;
            double ms = U.TicksToMS(U.DateTimeNow - startTime);
            if (double.IsNaN(maxAcceptableValue))
            {
                maxAcceptableValue = MaxAcceptableValue;
            }
            if (double.IsNaN(minAcceptable))
            {
                minAcceptable = MinAcceptableValue;
            }
            
            double scale = 256.0 / (maxAcceptableValue - minAcceptable + 1);
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);

            BitmapData bmpDataWrite = bm.LockBits(rect, ImageLockMode.ReadWrite, bm.PixelFormat);
            int stride = bmpDataWrite.Stride;
            int bpp = stride / numCols;
            var ptrWrite = bmpDataWrite.Scan0;
            int bytes  = Math.Abs(stride) * bm.Height;
            byte[] rgbValues = new byte[bytes];
            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptrWrite, rgbValues, 0, bytes);
            double[,] data = _dataList[CurrentImage];

            for (int y = 0; y < bmpDataWrite.Height; y++)
            {
                for (var x = 0; x < bmpDataWrite.Width; x++)
                {
                    int n = y * stride + x * bpp;
                    int iy = y;
                    int ix = x;
                    if (rotate == 90)
                    {
                        iy = numCols - x - 1;
                        ix = y;
                    }
                    else if (rotate == -90)
                    {
                        iy = numCols - x - 1;
                        ix = numRows - y - 1;
                    }
                    double dVal = data != null ? data[iy, ix] : 0.0;
                    dVal = Math.Max(dVal, minAcceptable);
                    dVal = Math.Min(dVal, maxAcceptableValue);
                    // Let zero be the floor
                    dVal -= minAcceptable;
                    // Let 255 be the max
                    byte color = (byte)(dVal * scale);
                    rgbValues[n] = (byte)color;
                    rgbValues[n + 1] = (byte)color;
                    rgbValues[n + 2] = (byte)color;
                    for (int j = 3; j < bpp; j++)
                    {
                        rgbValues[n + j] = (byte)255;
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptrWrite, bytes);

            bm.UnlockBits(bmpDataWrite);

            //double ms1 = U.TicksToMS(U.DateTimeNow - startTime);

            //U.LogInfo("{0},BuildBitmap,{1},", Name, ms1);
            //bm.Save(string.Format(@"C:\Users\MDD_016\Pictures\Key Test\First{0}.jpg", nImage.ToString("0#")), ImageFormat.Jpeg);
            //Cursor.Current = origCursor;
            //U.LogInfo("Create-{0}", nImage--);
            //if (nImage < 0)
            //    nImage = 58;
            return bm;
        }
        //public static int nImage = 58;
    }
}
