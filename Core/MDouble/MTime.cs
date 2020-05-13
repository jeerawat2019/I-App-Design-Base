using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MDouble
{
    /// <summary>
    /// Base class for Time
    /// </summary>
    public abstract class MTime : MDoubleBase
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public abstract double SecondsPerUnit
        {
            get;
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public MTime()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MTime(double initialVal)
            :base(initialVal)
        {
        }
        ///// <summary>
        ///// Convert from a double
        ///// </summary>
        ///// <param name="val"></param>
        ///// <returns></returns>
        //public static implicit operator MTime(double val)
        //{
        //    MTime newVal = new MTime();
        //    newVal._val = val;
        //    return newVal;
        //}
        /// <summary>
        /// Convert the number to the base number (sec)
        /// </summary>
        public double ToSeconds
        {
            get { return Val * SecondsPerUnit; }
        }
    }
    /// <summary>
    /// Seconds (sec)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<Seconds>))]
    public class Seconds : MTime
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double SecondsPerUnit
        {
            get { return 1.0; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public Seconds()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public Seconds(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Seconds(MTime val)
        {
            _val = val.ToSeconds;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "sec"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Seconds(double val)
        {
            Seconds newVal = new Seconds();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from Miliseconds
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Seconds(Miliseconds val)
        {
            Seconds newVal = new Seconds();
            newVal._val = val.ToSeconds;
            return newVal;
        }
        /// <summary>
        /// Convert from Microseconds
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Seconds(Microseconds val)
        {
            Seconds newVal = new Seconds();
            newVal._val = val.ToSeconds;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Seconds operator +(Seconds val1, MTime val2)
        {
            Seconds newVal = new Seconds();
            newVal._val = val1.ToSeconds + val2.ToSeconds;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Seconds operator -(Seconds val1, MTime val2)
        {
            Seconds newVal = new Seconds();
            newVal._val = val1.ToSeconds - val2.ToSeconds;
            return newVal;
        }
    }
    /// <summary>
    /// Miliseconds (ms)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<Miliseconds>))]
    public class Miliseconds : MTime
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double SecondsPerUnit
        {
            get { return 0.001; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public Miliseconds()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public Miliseconds(double initialVal)
            : base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Miliseconds(MTime val)
        {
            _val = val.ToSeconds / SecondsPerUnit;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "ms"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Miliseconds(double val)
        {
            Miliseconds newVal = new Miliseconds();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from Seconds
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Miliseconds(Seconds val)
        {
            Miliseconds newVal = new Miliseconds();
            newVal._val = val.ToSeconds / newVal.SecondsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Convert from Microseconds
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Miliseconds(Microseconds val)
        {
            Miliseconds newVal = new Miliseconds();
            newVal._val = val.ToSeconds / newVal.SecondsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Miliseconds operator +(Miliseconds val1, MTime val2)
        {
            Miliseconds newVal = new Miliseconds();
            newVal._val = (val1.ToSeconds + val2.ToSeconds) / newVal.SecondsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Miliseconds operator -(Miliseconds val1, MTime val2)
        {
            Miliseconds newVal = new Miliseconds();
            newVal._val = (val1.ToSeconds - val2.ToSeconds) / newVal.SecondsPerUnit;
            return newVal;
        }
    }
    /// <summary>
    /// Microseconds (ms)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<Microseconds>))]
    public class Microseconds : MTime
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double SecondsPerUnit
        {
            get { return 0.000001; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public Microseconds()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public Microseconds(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Microseconds(MTime val)
        {
            _val = val.ToSeconds / SecondsPerUnit;
        }        
        /// <summary>
        /// return Ticks
        /// </summary>
        public long ToTicks
        {
            get { return (long)Math.Ceiling(Val/1000.0 * TimeSpan.TicksPerMillisecond); }
        }
        /// <summary>
        /// return seconds
        /// </summary>
        public void FromSeconds(double seconds)
        {
            Val = seconds / SecondsPerUnit;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "us"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Microseconds(double val)
        {
            Microseconds newVal = new Microseconds();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from Seconds
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Microseconds(Seconds val)
        {
            Microseconds newVal = new Microseconds();
            newVal._val = val.ToSeconds / newVal.SecondsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Convert from Miliseconds
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Microseconds(Miliseconds val)
        {
            Microseconds newVal = new Microseconds();
            newVal._val = val.ToSeconds / newVal.SecondsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Microseconds operator +(Microseconds val1, MTime val2)
        {
            Microseconds newVal = new Microseconds();
            newVal._val = (val1.ToSeconds + val2.ToSeconds) / newVal.SecondsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Microseconds operator -(Microseconds val1, MTime val2)
        {
            Microseconds newVal = new Microseconds();
            newVal._val = (val1.ToSeconds - val2.ToSeconds) / newVal.SecondsPerUnit;
            return newVal;
        }
    }
}
