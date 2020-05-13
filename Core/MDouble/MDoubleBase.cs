using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MDouble
{
    public delegate void DoubleEventHandler(double dVal);
    /// <summary>
    /// Magnecomp Double
    /// </summary>
    public class MDoubleBase
    {
        /// <summary>
        /// The double value
        /// </summary>
        protected double _val = 0.0;
        /// <summary>
        /// Get set the double value (Thread safe)
        /// </summary>
        [Browsable(true)]
        public double Val
        {
            get
            {
                lock (this)
                {
                    return _val;
                }
            }
            set
            {
                lock (this)
                {
                    _val = value;
                }
            }
        }
        /// <summary>
        /// Get the integer value
        /// </summary>
        public int ToInt
        {
            get { return (int)Math.Round(Val); }
        }
        /// <summary>
        /// Invert the value
        /// </summary>
        public void Invert()
        {
            lock (this)
            {
                _val = -_val;
            }
        }

        /// <summary>
        ///  Get the units
        /// </summary>
        public virtual string UnitText
        {
            get { return ""; }
        }

        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public MDoubleBase()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MDoubleBase(double initialVal)
        {
            _val = initialVal;
        }
        /// <summary>
        /// Conversion from string constructorr
        /// </summary>
        /// <param name="sVal"></param>
        public MDoubleBase(string sVal)
        {
            if (!string.IsNullOrEmpty(sVal))
            {
                _val = Convert.ToDouble(sVal.Split(' ')[0]);
            }
        }
        public override int GetHashCode()
        {
            return _val.GetHashCode();
        }
        /// <summary>
        /// Convert to a string.  Show the units
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", Val.ToString("##0.###"), UnitText);
        }
        /// <summary>
        /// Convert the MDouble to a double
        /// </summary>
        /// <param name="mVal"></param>
        /// <returns></returns>
        public static implicit operator double(MDoubleBase mVal)
        {
            return mVal.Val;
        }
        /// <summary>
        /// Convert the double to a MDouble
        /// </summary>
        /// <param name="mVal"></param>
        /// <returns></returns>
        public static implicit operator MDoubleBase(double dVal)
        {
            return new MDoubleBase(dVal);
        }

    //    /// <summary>
    //    /// Pass equals check to double value
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    /// <returns></returns>
    //    public override bool Equals(object obj)
    //    {
    //        if (obj is MDoubleBase)
    //        {
    //            return this.ToBase.Equals((obj as MDoubleBase).ToBase);
    //        }
    //        return base.Equals(obj);
    //    }

    //    /// <summary>
    //    /// Divide two MDoubles (Unitless)
    //    /// </summary>
    //    /// <param name="val1"></param>
    //    /// <param name="val2"></param>
    //    /// <returns></returns>
    //    public static double operator /(MDoubleBase val1, MDoubleBase val2)
    //    {
    //        if (val1.GetType().Equals(val1.GetType()))
    //        {
    //            return val1.Val / val2.Val;
    //        }
    //        return val1.ToBase / val2.ToBase;
    //    }
    //    /// <summary>
    //    /// muliply two MDoubles (Unitless)
    //    /// </summary>
    //    /// <param name="val1"></param>
    //    /// <param name="val2"></param>
    //    /// <returns></returns>
    //    public static double operator *(MDoubleBase val1, MDoubleBase val2)
    //    {
    //        if (val1.GetType().Equals(val1.GetType()))
    //        {
    //            return val1.Val * val2.Val;
    //        }
    //        return val1.ToBase * val2.ToBase;
    //    }

    //    /// <summary>
    //    /// Compare two MDoubles
    //    /// </summary>
    //    /// <param name="val1"></param>
    //    /// <param name="val2"></param>
    //    /// <returns></returns>
    //    public static bool operator ==(MDoubleBase val1, MDoubleBase val2)
    //    {
    //        if (Object.Equals(val1, null))
    //        {
    //            return Object.Equals(val2, null);
    //        }
    //        if (Object.Equals(val2, null))
    //        {
    //            return Object.Equals(val1, null);
    //        }
    //        return val1.ToBase == val2.ToBase;
    //    }

    //    /// <summary>
    //    /// Compare two MDoubles
    //    /// </summary>
    //    /// <param name="val1"></param>
    //    /// <param name="val2"></param>
    //    /// <returns></returns>
    //    public static bool operator !=(MDoubleBase val1, MDoubleBase val2)
    //    {
    //        if (Object.Equals(val1, null))
    //        {
    //            return !Object.Equals(val2, null);
    //        }
    //        if (Object.Equals(val2, null))
    //        {
    //            return !Object.Equals(val1, null);
    //        }
    //        return val1.ToBase != val2.ToBase;
    //    }
    }

    public class MDoubleConverter<T> :  ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.IsSubclassOf(typeof(MDoubleBase)))
                  return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] split = (value as string).Split(' ');
                double dVal = 0.0;
                if (split.Length > 0 && !string.IsNullOrEmpty(split[0]))
                {
                    try
                    {
                        dVal = Convert.ToDouble(split[0]);
                    }
                    catch { }
                }
                return Activator.CreateInstance(typeof(T), dVal);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is MDoubleBase)
            {
                return (value as MDoubleBase).ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
