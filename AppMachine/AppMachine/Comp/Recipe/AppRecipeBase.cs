using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

using MCore;
using MCore.Comp;

namespace AppMachine.Comp.Recipe
{
    public class AppRecipeBase:CompBase
    {

         #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppRecipeBase()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppRecipeBase(string name)
            : base(name)
        {
            
        }
        #endregion Constructors
    }
}
