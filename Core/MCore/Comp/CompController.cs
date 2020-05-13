using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCLib.Comp
{
    public class CompController : CompBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CompController()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CompController(string name) 
            : base (name)
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nickname"></param>
        public CompController(string name, string nickname)
            : base(name, nickname)
        {
        }
        #endregion Constructors
    }
}
