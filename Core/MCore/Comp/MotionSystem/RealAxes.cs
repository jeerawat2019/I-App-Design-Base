using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.MotionSystem
{
    public class RealAxes : AxesBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RealAxes()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public RealAxes(string name)
            : base(name)
        {            
        }

        #endregion Constructors
    }
}
