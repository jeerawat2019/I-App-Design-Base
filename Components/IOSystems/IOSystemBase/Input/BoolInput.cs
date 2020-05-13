using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace MCore.Comp.IOSystem.Input
{
    public class BoolInput : IOBoolChannel
    {

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BoolInput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public BoolInput(string name)
            : base(name)
        {
        }
            

        #endregion Constructors
    }
}
