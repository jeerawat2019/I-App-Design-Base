using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace MDouble
{
    /// <summary>
    /// Base class for Length
    /// </summary>
    public abstract class MLengthSpeed : MDoubleBase
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
        public MLengthSpeed()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MLengthSpeed(double initialVal)
            :base(initialVal)
        {
        }

        /// <summary>
        /// Conversion from string constructor
        /// </summary>
        /// <param name="sVal"></param>
        public MLengthSpeed(string sVal)
            : base(sVal)
        {

        }

        /// <summary>
        /// Convert the number to the base number (um)
        /// </summary>
        public double ToMicronsPerSecond
        {
            get { return Val * MicronsPerUnit; }
        }
        ///// <summary>
        ///// Convert from a double
        ///// </summary>
        ///// <param name="val"></param>
        ///// <returns></returns>
        //public static implicit operator MLengthSpeed(double val)
        //{
        //    MLengthSpeed newVal = new MLengthSpeed();
        //    newVal._val = val;
        //    return newVal;
        //}

    }
    /// <summary>
    /// MicronsPerSecond (um/S)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<MicronsPerSecond>))]
    public class MicronsPerSecond : MLengthSpeed
    {
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public MicronsPerSecond()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MicronsPerSecond(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public MicronsPerSecond(MLengthSpeed val)
        {
            _val = val.ToMicronsPerSecond;
        }
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double MicronsPerUnit
        {
            get { return 1.0; }
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "um/S"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator MicronsPerSecond(double val)
        {
            MicronsPerSecond newVal = new MicronsPerSecond();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from a MilimetersPerSecond
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator MicronsPerSecond(MillimetersPerSecond val)
        {
            MicronsPerSecond newVal = new MicronsPerSecond();
            newVal._val = val.ToMicronsPerSecond;
            return newVal;
        }
        /// <summary>
        /// Convert from a NanometersPerSecond
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator MicronsPerSecond(NanometersPerSecond val)
        {
            MicronsPerSecond newVal = new MicronsPerSecond();
            newVal._val = val.ToMicronsPerSecond;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static MicronsPerSecond operator +(MicronsPerSecond val1, MLengthSpeed val2)
        {
            MicronsPerSecond newVal = new MicronsPerSecond();
            newVal._val = val1.ToMicronsPerSecond + val2.ToMicronsPerSecond;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static MicronsPerSecond operator -(MicronsPerSecond val1, MLengthSpeed val2)
        {
            MicronsPerSecond newVal = new MicronsPerSecond();
            newVal._val = val1.ToMicronsPerSecond - val2.ToMicronsPerSecond;
            return newVal;
        }
    }
    /// <summary>
    /// MilimetersPerSecond (mm)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<MillimetersPerSecond>))]
    public class MillimetersPerSecond : MLengthSpeed
    {
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public MillimetersPerSecond()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MillimetersPerSecond(double initialVal)
            :base(initialVal)
        {
        }


        /// <summary>
        /// Generic Conversion Constructor
        /// </summary>
        /// <param name="val"></param>
        public MillimetersPerSecond(string sVal)
            : base(sVal)
        {
            
        }

        /// <summary>
        /// Generic Conversion Constructor
        /// </summary>
        /// <param name="val"></param>
        public MillimetersPerSecond(MLengthSpeed val)
        {
            _val = val.ToMicronsPerSecond / MicronsPerUnit;
        }
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double MicronsPerUnit
        {
            get { return 1000.0; }
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "mm/S"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator MillimetersPerSecond(double val)
        {
            MillimetersPerSecond newVal = new MillimetersPerSecond();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from a MicronsPerSecond
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator MillimetersPerSecond(MicronsPerSecond val)
        {
            MillimetersPerSecond newVal = new MillimetersPerSecond();
            newVal._val = val.ToMicronsPerSecond / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static MillimetersPerSecond operator +(MillimetersPerSecond val1, MLengthSpeed val2)
        {
            MillimetersPerSecond newVal = new MillimetersPerSecond();
            newVal._val = (val1.ToMicronsPerSecond + val2.ToMicronsPerSecond) / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static MillimetersPerSecond operator -(MillimetersPerSecond val1, MLengthSpeed val2)
        {
            MillimetersPerSecond newVal = new MillimetersPerSecond();
            newVal._val = (val1.ToMicronsPerSecond - val2.ToMicronsPerSecond) / newVal.MicronsPerUnit;
            return newVal;
        }
    }
    /// <summary>
    /// Nano-Meters (nm)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<NanometersPerSecond>))]
    public class NanometersPerSecond : MLengthSpeed
    {
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public NanometersPerSecond()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public NanometersPerSecond(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public NanometersPerSecond(MLengthSpeed val)
        {
            _val = val.ToMicronsPerSecond / MicronsPerUnit;
        }
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double MicronsPerUnit
        {
            get { return 0.001; }
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "nm/S"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator NanometersPerSecond(double val)
        {
            NanometersPerSecond newVal = new NanometersPerSecond();
            newVal._val = val;
            return newVal;
        }
        ///// <summary>
        ///// Convert from a MilimetersPerSecond
        ///// </summary>
        ///// <param name="val"></param>
        ///// <returns></returns>
        //public static implicit operator NanometersPerSecond(MSpeed val)
        //{
        //    NanometersPerSecond newVal = new NanometersPerSecond();
        //    newVal._val = val.Val / val.ConvertToBaseFactor;
        //    return newVal;
        //}
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static NanometersPerSecond operator +(NanometersPerSecond val1, MLengthSpeed val2)
        {
            NanometersPerSecond newVal = new NanometersPerSecond();
            newVal._val = (val1.ToMicronsPerSecond + val2.ToMicronsPerSecond) / newVal.MicronsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static NanometersPerSecond operator -(NanometersPerSecond val1, MLengthSpeed val2)
        {
            NanometersPerSecond newVal = new NanometersPerSecond();
            newVal._val = (val1.ToMicronsPerSecond - val2.ToMicronsPerSecond) / newVal.MicronsPerUnit;
            return newVal;
        }
    }
    /// <summary>
    /// Base class for Length
    /// </summary>
    public abstract class MRotarySpeed : MDoubleBase
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public abstract double DegreesPerUnit
        {
            get;
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public MRotarySpeed()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MRotarySpeed(double initialVal)
            : base(initialVal)
        {
        }
        /// <summary>
        /// Convert the number to the base number (deg)
        /// </summary>
        public double ToDegreesPerSecond
        {
            get { return Val * DegreesPerUnit; }
        }
    }
    /// <summary>
    /// DegreesPerSecond (deg/S)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<DegreesPerSecond>))]
    public class DegreesPerSecond : MRotarySpeed
    {
        /// <summary>
        /// Convert degrees to units
        /// </summary>
        public override double DegreesPerUnit
        {
            get { return 1.0; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public DegreesPerSecond()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public DegreesPerSecond(double initialVal)
            : base(initialVal)
        {
        }
        /// <summary>
        /// Generic Conversion Constructor
        /// </summary>
        /// <param name="val"></param>
        public DegreesPerSecond(MRotarySpeed val)
        {
            _val = val.ToDegreesPerSecond / DegreesPerUnit;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "deg/S"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator DegreesPerSecond(double val)
        {
            DegreesPerSecond newVal = new DegreesPerSecond();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static DegreesPerSecond operator +(DegreesPerSecond val1, MRotarySpeed val2)
        {
            DegreesPerSecond newVal = new DegreesPerSecond();
            newVal._val = val1.ToDegreesPerSecond + val2.ToDegreesPerSecond;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static DegreesPerSecond operator -(DegreesPerSecond val1, MRotarySpeed val2)
        {
            DegreesPerSecond newVal = new DegreesPerSecond();
            newVal._val = val1.ToDegreesPerSecond - val2.ToDegreesPerSecond;
            return newVal;
        }
    }
    /// <summary>
    /// DegreesPerSecond (deg/S)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<RadiansPerSecond>))]
    public class RadiansPerSecond : MRotarySpeed
    {
        /// <summary>
        /// Convert degrees to units
        /// </summary>
        public override double DegreesPerUnit
        {
            get { return 180.0 / Math.PI; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public RadiansPerSecond()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public RadiansPerSecond(double initialVal)
            : base(initialVal)
        {
        }
        /// <summary>
        /// Generic Conversion Constructor
        /// </summary>
        /// <param name="val"></param>
        public RadiansPerSecond(MRotarySpeed val)
        {
            _val = val.ToDegreesPerSecond / DegreesPerUnit;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "rad/S"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator RadiansPerSecond(double val)
        {
            RadiansPerSecond newVal = new RadiansPerSecond();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static RadiansPerSecond operator +(RadiansPerSecond val1, MRotarySpeed val2)
        {
            RadiansPerSecond newVal = new RadiansPerSecond();
            newVal._val = val1.ToDegreesPerSecond + val2.ToDegreesPerSecond;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static RadiansPerSecond operator -(RadiansPerSecond val1, MRotarySpeed val2)
        {
            RadiansPerSecond newVal = new RadiansPerSecond();
            newVal._val = val1.ToDegreesPerSecond - val2.ToDegreesPerSecond;
            return newVal;
        }
    }
    /// <summary>
    /// CyclesPerSecond (hz)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<CyclesPerSecond>))]
    public class CyclesPerSecond : MRotarySpeed
    {
        /// <summary>
        /// Convert degrees to units
        /// </summary>
        public override double DegreesPerUnit
        {
            get { return 360.0; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public CyclesPerSecond()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public CyclesPerSecond(double initialVal)
            : base(initialVal)
        {
        }
        /// <summary>
        /// Generic Conversion Constructor
        /// </summary>
        /// <param name="val"></param>
        public CyclesPerSecond(MRotarySpeed val)
        {
            _val = val.ToDegreesPerSecond / DegreesPerUnit;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "hz"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator CyclesPerSecond(double val)
        {
            CyclesPerSecond newVal = new CyclesPerSecond();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static CyclesPerSecond operator +(CyclesPerSecond val1, MRotarySpeed val2)
        {
            CyclesPerSecond newVal = new CyclesPerSecond();
            newVal._val = val1.ToDegreesPerSecond + val2.ToDegreesPerSecond;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static CyclesPerSecond operator -(CyclesPerSecond val1, MRotarySpeed val2)
        {
            CyclesPerSecond newVal = new CyclesPerSecond();
            newVal._val = val1.ToDegreesPerSecond - val2.ToDegreesPerSecond;
            return newVal;
        }
    }
}
