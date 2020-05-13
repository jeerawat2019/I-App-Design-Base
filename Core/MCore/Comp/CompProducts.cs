using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp
{
    public class CompProducts : CompBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CompProducts()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CompProducts(string name) 
            : base (name)
        {
        }
        #endregion Constructors

    }
}
