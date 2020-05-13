using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.Geometry
{
    public class GReference : G3DCompBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public GReference()
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public GReference(string name)
            : base(name)
        {
        }
        #endregion Constructors
    }
}
