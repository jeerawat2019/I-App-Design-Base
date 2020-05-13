using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;

using MCore.Controls;
using MDouble;

namespace MCore.Comp.VisionSystem
{
    public abstract class ResultsExchange : CompBase
    {
        private Dictionary<string, object> _results = new Dictionary<string, object>();

        public const string NONE = "-- None --";

        [XmlIgnore]
        public ExchangeDataMapBase[] dataMap = null;

        /// <summary>
        /// The list of available results for this camera.
        /// This is auto-populated per acquisition
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Camera")]
        [ReadOnly(true)]
        public Dictionary<string, object> Results
        {
            get { return _results; }
            set { _results = value; }
        }
        /// <summary>
        /// Flag to tell us results are what we expected
        /// </summary>
        [Browsable(true)]
        [XmlIgnore]
        public bool ResultsSuccess
        {
            get { return GetPropValue(() => ResultsSuccess); }
            set { SetPropValue(() => ResultsSuccess, value); }
        }
        /// <summary>
        /// Flag to tell us results are what we expected
        /// </summary>
        [Browsable(true)]
        [XmlIgnore]
        public string ResultsError
        {
            get { return GetPropValue(() => ResultsError); }
            set { SetPropValue(() => ResultsError, value); }
        }

        [Browsable(false)]
        [XmlIgnore]
        public virtual Image DefaultImage
        {
            get { return null; }
        }

        private CompBase _visionOwner = null;
        [XmlIgnore]
        public CompBase ImageOwner
        {
            get { return _visionOwner; }
            set { _visionOwner = value; }
        }


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultsExchange()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public ResultsExchange(string name)
            : base(name)
        {
        }
        #endregion Constructors

        public override void Reset()
        {
            ResultsSuccess = false;
            ResultsError = string.Empty;
            // Reset the results
            Results.Clear();

        }
        ///// <summary>
        ///// Get the offset values
        ///// </summary>
        ///// <param name="resultIds">list of result IDs.  X and Y are the first two</param>
        ///// <returns></returns>
        //public double[] GetXYFromCenter(params string[] resultIds)
        //{
        //    double[] results = GetValidResults(resultIds);

        //    if (results != null && results.Length >= 2)
        //    {

        //        float xCenter = 0f;
        //        float yCenter = 0f;
        //        if ((Parent as VisionJobBase).TrainImage == null)
        //        {
        //            xCenter = 320;
        //            yCenter = 480;
        //        }
        //        else
        //        {
        //            xCenter = (Parent as VisionJobBase).TrainImage.Width / 2;
        //            yCenter = (Parent as VisionJobBase).TrainImage.Height / 2;
        //        }
        //        results[0] -= xCenter;
        //        results[1] -= yCenter;
        //    }
        //    return results;
        //}

        /// <summary>
        /// Process the results, set any data values, and determine if success
        /// </summary>
        public virtual void ProcessResults()
        {            
            foreach (ExchangeDataMapBase dMap in dataMap)
            {
                string key = dMap.Key;
                if (Results.ContainsKey(key))
                {
                    dMap.DataValue = Results[key];
                }
                else if (key != ResultsExchange.NONE)
                {
                    dMap.DataValue = 0.0;
                    dMap.KeyNotFound = true;
                    //ResultsError = "Key not found";
                    //U.LogPopup("'{0}' key not found for '{1}'", key, Nickname);
                }
            }
            DoConversions();
            foreach (ExchangeDataMapBase dMap in dataMap)
            {
                if (Results.ContainsKey(dMap.Key))
                {
                    if (!dMap.IsInSpec)
                    {
                        ResultsError = "Out of spec";
                    }
                }
            }
            ResultsSuccess = string.IsNullOrEmpty(ResultsError);
        }
        protected virtual void DoConversions()
        {
        }
        protected double GetDoubleResult(Spec<double> spec)
        {
            if (Results.ContainsKey(spec.key))
            {
                return Convert.ToDouble(Results[spec.key]);
            }
            return 0.0;
        }
        protected int GetIntResult(Spec<int> spec)
        {
            if (Results.ContainsKey(spec.key))
            {
                return (int)Math.Round(Convert.ToDouble(Results[spec.key]));
            }
            return 0;
        }
        protected Bitmap GetImageResult(SpecObj<Bitmap> spec)
        {
            if (Results.ContainsKey(spec.key))
            {
                return Results[spec.key] as Bitmap;
            }
            return null;
        }
    }

    public abstract class ExchangeDataMapBase
    {
        public string DataName = string.Empty;
        public bool KeyNotFound = false; 
        public abstract string Key
        {
            get;
            set;
        }
        public abstract string DataToString
        {
            get;
        }
        public abstract object DataValue
        {
            set;
        }
        public abstract string MinString
        {
            get;
            set;
        }
        public abstract string MaxString
        {
            get;
            set;
        }
        public abstract SpecBase.eMode Mode
        {
            get;
            set;
        }
        public abstract SpecBase.eMode Actual
        {
            get;
        }
        public abstract bool IsInSpec
        {
            get;
        }
    }

    public class ExchangeDataMapObj<T> : ExchangeDataMapBase
    {
        public override string Key
        {
            get
            {
                return SpecProp.Value.key;
            }
            set
            {
                SpecProp.Value.key = value;
            }
        }
        public override string DataToString
        {
            get 
            {
                if (DataValueProp == null || DataValueProp.Value == null)
                {
                    return "null";
                }
                if (DataValueProp.Value is Bitmap)
                {
                    Bitmap bm = DataValueProp.Value as Bitmap;
                    return string.Format("Bitmap {0} x {1}", bm.Width, bm.Height);
                }

                return DataValueProp.SValue; 
            }
        }
        public override object DataValue
        {
            set
            {
                DataValueProp.Value = (T)Convert.ChangeType(value, typeof(T));
            }
        }
        public override SpecBase.eMode Mode
        {
            get
            {
                return SpecProp.Value.mode;
            }
            set
            {
                SpecProp.Value.mode = value;
            }
        }
        public override SpecBase.eMode Actual
        {
            get
            {
                return SpecProp.Value.actual;
            }
        }
        private T QualifyVal(string val)
        {
            try
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
        public override string MinString
        {
            get { return string.Empty; }
            set { }
        }
        public override string MaxString
        {
            get { return string.Empty; }
            set { }
        }
        public PropDelegate<T> DataValueProp = null;
        public PropDelegate<SpecObj<T>> SpecProp = null;
        public ExchangeDataMapObj(Expression<Func<SpecObj<T>>> specProp, Expression<Func<T>> dataValueProp)
        {
            SpecProp = new PropDelegate<SpecObj<T>>(specProp);
            DataValueProp = new PropDelegate<T>(dataValueProp);
            var memberExpression = dataValueProp.Body as MemberExpression;
            DataName = memberExpression.Member.Name;
        }
        public override bool IsInSpec
        {
            get { return SpecProp.Value.IsInSpec(DataValueProp.Value); }
        }
    }
    public class ExchangeDataMap<T> : ExchangeDataMapBase where T : struct, IComparable 
    {
        public override string Key
        {
            get
            {
                return SpecProp.Value.key;
            }
            set
            {
                SpecProp.Value.key = value;
            }
        }
        public override string DataToString
        {
            get { return DataValueProp.SValue; }
        }
        public override object DataValue
        {
            set
            {
                DataValueProp.Value =  (T)Convert.ChangeType(value, typeof(T));
            }
        }
        public override SpecBase.eMode Mode
        {
            get
            {
                return SpecProp.Value.mode;
            }
            set
            {
                SpecProp.Value.mode = value;
            }
        }
        public override SpecBase.eMode Actual
        {
            get
            {
                return SpecProp.Value.actual;
            }
        }
        private T QualifyVal(string val)
        {
            try
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
        public override string MinString
        {
            get { return SpecProp.Value.min.ToString(); }
            set { SpecProp.Value.min = QualifyVal(value); }
        }
        public override string MaxString
        {
            get { return SpecProp.Value.max.ToString(); }
            set { SpecProp.Value.max = QualifyVal(value); }
        }
        public PropDelegate<T> DataValueProp = null;
        public PropDelegate<Spec<T>> SpecProp = null;
        public ExchangeDataMap(Expression<Func<Spec<T>>> specProp, Expression<Func<T>> dataValueProp)
        {
            SpecProp = new PropDelegate<Spec<T>>(specProp);
            DataValueProp = new PropDelegate<T>(dataValueProp);
            var memberExpression = dataValueProp.Body as MemberExpression;
            DataName = memberExpression.Member.Name;
        }
        public override bool IsInSpec
        {
            get { return SpecProp.Value.IsInSpec(DataValueProp.Value); }
        }
    }
    public class SpecBase
    {
        public enum eMode { None, Under, Within, Over };
        public eMode mode = eMode.None;
        public eMode actual = eMode.None;
        public string key = string.Empty;
        public SpecBase()
        {
        }
        public SpecBase(string ky)
        {
            key = ky;
        }
    }
    public class SpecObj<T> : SpecBase
    {
        public SpecObj()
            : base()
        {
        }
        public SpecObj(string ky)
            : base(ky)
        {
        }
        virtual public bool IsInSpec(T val)
        {
            return true;
        }
    }
    public class Spec<T> : SpecObj<T> where T : struct, IComparable 
    {
        public T min = default(T);
        public T max = default(T);
        public Spec()
        {
        }
        public Spec(string ky)
            : base(ky)
        {
        }
        public override bool IsInSpec(T val)
        {
            switch(mode)
            {
                case eMode.None:
                    actual = eMode.None;
                    break;
                case eMode.Under:
                    // Check against Max
                    actual = val.CompareTo(max) < 0 ? eMode.Under : eMode.Over;
                    break;
                case eMode.Over:
                    // Check against Min
                    actual = val.CompareTo(min) < 0 ? eMode.Under : eMode.Over;
                    break;
                case eMode.Within:
                    // Check against Min and max
                    if (val.CompareTo(min) < 0)
                    {
                        actual = eMode.Under;
                    }
                    else if (val.CompareTo(max) > 0)
                    {
                        actual = eMode.Over;
                    }
                    else
                    {
                        actual = eMode.Within;
                    }
                    break;                    
            }
            return mode == actual;
        }
    }
}
