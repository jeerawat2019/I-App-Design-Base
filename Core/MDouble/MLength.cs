using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MDouble
{
    public delegate void MillimeterEventHandler(Millimeters mm);
    public delegate void MicronEventHandler(Microns um);


    /// <summary>
    /// Delegate for Millimeters/MillimetersPerSecond
    /// </summary>
    /// <param name="mm"></param>
    /// <param name="mmps"></param>
    public delegate void delVoid_MillimeterMillimeterPerSec(Millimeters mm, MillimetersPerSecond mmps);


    /// <summary>
    /// Base class for Length
    /// </summary>
    public abstract class MLength : MDoubleBase
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public abstract double MicronsPerUnit
        {
            get;
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public MLength()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MLength(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Conversion from string constructor
        /// </summary>
        /// <param name="sVal"></param>
        public MLength(string sVal)
            : base(sVal)
        {

        }
        /// <summary>
        /// Convert the number to the base number (microns)
        /// </summary>
        public double ToMicrons
        {
            get { return Val * MicronsPerUnit; }
        }
        /// <summary>
        /// Convert the number to the base number (microns)
        /// </summary>
        public double ToMM
        {
            get { return ToMicrons / 1000.0; }
        }
    }
    /// <summary>
    /// Microns (um)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<Microns>))]
    public class Microns : MLength
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double MicronsPerUnit
        {
            get { return 1.0; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public Microns()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public Microns(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Microns(MLength val)
        {
            _val = val.ToMicrons;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "um"; }
        }

        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Microns(double val)
        {
            Microns newVal = new Microns();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from a milimeters
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Microns(Millimeters val)
        {
            Microns newVal = new Microns();
            newVal._val = val.ToMicrons;
            return newVal;
        }
        /// <summary>
        /// Convert from a Nanometers
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Microns(Nanometers val)
        {
            Microns newVal = new Microns();
            newVal._val = val.ToMicrons;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Microns operator +(Microns val1, MLength val2)
        {
            Microns newVal = new Microns();
            newVal._val = val1.ToMicrons + val2.ToMicrons;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Microns operator -(Microns val1, MLength val2)
        {
            Microns newVal = new Microns();
            newVal._val = val1.ToMicrons - val2.ToMicrons;
            return newVal;
        }
    }
    /// <summary>
    /// Milimeters (mm)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<Millimeters>))]
    public class Millimeters : MLength
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double MicronsPerUnit
        {
            get { return 1000.0; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public Millimeters()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public Millimeters(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Conversion from string constructor
        /// </summary>
        /// <param name="sVal"></param>
        public Millimeters(string sVal)
            : base(sVal)
        {

        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Millimeters(MLength val)
        {
            _val = val.ToMicrons / MicronsPerUnit;
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Millimeters(MLengthSpeed val)
        {
            _val = val.ToMicronsPerSecond / MicronsPerUnit;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "mm"; }
        }

        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Millimeters(double val)
        {
            Millimeters newVal = new Millimeters();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from a Microns
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Millimeters(Microns val)
        {
            Millimeters newVal = new Millimeters();
            newVal._val = val.Val / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Convert from a Nanometers
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Millimeters(Nanometers val)
        {
            Millimeters newVal = new Millimeters();
            newVal._val = val.ToMicrons / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Millimeters operator +(Millimeters val1, MLength val2)
        {
            Millimeters newVal = new Millimeters();
            newVal._val = (val1.ToMicrons + val2.ToMicrons) / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Millimeters operator -(Millimeters val1, MLength val2)
        {
            Millimeters newVal = new Millimeters();
            newVal._val = (val1.ToMicrons - val2.ToMicrons) / newVal.MicronsPerUnit;
            return newVal;
        }
    }
    /// <summary>
    /// Nano-Meters (nm)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<Nanometers>))]
    public class Nanometers : MLength
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double MicronsPerUnit
        {
            get { return 0.001; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public Nanometers()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public Nanometers(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Nanometers(MLength val)
        {
            _val = val.ToMicrons / MicronsPerUnit;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "nm"; }
        }

        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Nanometers(double val)
        {
            Nanometers newVal = new Nanometers();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from a milimeters
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Nanometers(Millimeters val)
        {
            Nanometers newVal = new Nanometers();
            newVal._val = val.ToMicrons / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Convert from a Microns
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Nanometers(Microns val)
        {
            Nanometers newVal = new Nanometers();
            newVal._val = val.ToMicrons / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Nanometers operator +(Nanometers val1, MLength val2)
        {
            Nanometers newVal = new Nanometers();
            newVal._val = (val1.ToMicrons + val2.ToMicrons) / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Nanometers operator -(Nanometers val1, MLength val2)
        {
            Nanometers newVal = new Nanometers();
            newVal._val = (val1.ToMicrons - val2.ToMicrons) / newVal.MicronsPerUnit;
            return newVal;
        }
    }
}
