using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MDouble
{
    public delegate void MDoubleEventHandler(MDoubleNoUnits dNoUnit);
    /// <summary>
    /// Base class for Length
    /// </summary>
    [TypeConverterAttribute(typeof(MDoubleConverter<MDoubleNoUnits>))]
    public class MDoubleNoUnits : MDoubleBase
    {
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        public  MDoubleNoUnits()
        {
        }
        /// <summary>
        /// Serializing Constructor
        /// </summary>
        /// <param name="initialVal"></param>
        public MDoubleNoUnits(double initialVal)
            :base(initialVal)
        {
        }
        /// <summary>
        /// Convert from a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator MDoubleNoUnits(double val)
        {
            MDoubleNoUnits newVal = new MDoubleNoUnits();
            newVal._val = val;
            return newVal;
        }
        /// <summary>
        /// Convert to a string.  Show the units
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Val.ToString("##0.###");
        }
    }
}
