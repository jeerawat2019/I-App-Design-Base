using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MCore;
using MDouble;

namespace MCore.Comp.SMLib.Path
{
    public class SMPath
    {
        #region Serialize properties

        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMPath()
        {
        }
        #endregion Constructors

        /// <summary>
        /// Recursivley make a clone of this item 
        /// </summary>
        /// <returns>New cloned instance</returns>
        public virtual SMPath Clone()
        {
            return Activator.CreateInstance(GetType()) as SMPath;
        }

    }
}
