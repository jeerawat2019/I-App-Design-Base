using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.Geometry
{
    public class G3DSuspension : G3DCompBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public G3DSuspension()
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public G3DSuspension(string name)
            : base(name)
        {
        }
        #endregion Constructors

    }
}
