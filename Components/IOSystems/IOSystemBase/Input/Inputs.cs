using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.IOSystem.Input
{
    public class Inputs : CompBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Inputs()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public Inputs(string name) 
            : base (name)
        {
        }
        #endregion Constructors
    }
}