using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MDouble
{
    /// <summary>
    /// Base class for Weight
    /// </summary>
    public abstract class MWeight : MDoubleBase
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public abstract double GramsPerUnit
        {
            get;
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public MWeight()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MWeight(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Convert the number to the base number (grams)
        /// </summary>
        public double ToGrams
        {
            get { return Val * GramsPerUnit; }
        }
    }
    /// <summary>
    /// Grams (gm)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<Grams>))]
    public class Grams : MWeight
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double GramsPerUnit
        {
            get { return 1.0; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public Grams()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public Grams(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Grams(MWeight val)
        {
            _val = val.ToGrams;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "gm"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Grams(double val)
        {
            Grams newVal = new Grams();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from Milligrams
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Grams(Milligrams val)
        {
            Grams newVal = new Grams();
            newVal._val = val.ToGrams;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Grams operator +(Grams val1, MWeight val2)
        {
            Grams newVal = new Grams();
            newVal._val = val1.ToGrams + val2.ToGrams;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Grams operator -(Grams val1, MWeight val2)
        {
            Grams newVal = new Grams();
            newVal._val = val1.ToGrams - val2.ToGrams;
            return newVal;
        }
    }
    /// <summary>
    /// Milligrams (mg)
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<Milligrams>))]
    public class Milligrams : MWeight
    {
        /// <summary>
        /// Conversion factor to base value
        /// </summary>
        public override double GramsPerUnit
        {
            get { return 0.001; }
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public Milligrams()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public Milligrams(double initialVal)
            : base(initialVal)
        {
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="val"></param>
        public Milligrams(MWeight val)
        {
            _val = val.ToGrams / GramsPerUnit;
        }
        /// <summary>
        ///  Get the units
        /// </summary>
        public override string UnitText
        {
            get { return "mg"; }
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Milligrams(double val)
        {
            Milligrams newVal = new Milligrams();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert from Grams
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator Milligrams(Grams val)
        {
            Milligrams newVal = new Milligrams();
            newVal._val = val.ToGrams / newVal.GramsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Milligrams operator +(Milligrams val1, MWeight val2)
        {
            Milligrams newVal = new Milligrams();
            newVal._val = (val1.ToGrams + val2.ToGrams) / newVal.GramsPerUnit;
            return newVal;
        }
        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static Milligrams operator -(Milligrams val1, MWeight val2)
        {
            Milligrams newVal = new Milligrams();
            newVal._val = (val1.ToGrams - val2.ToGrams) / newVal.GramsPerUnit;
            return newVal;
        }
    }
}
