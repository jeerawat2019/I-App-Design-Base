using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MCore.Comp.SMLib.Path;

namespace MCore.Comp.SMLib.Flow
{
    public class SMReturnStop : SMExit
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMReturnStop()
        {
        }
        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="text"></name>
        public SMReturnStop(string text)
            : base(text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Text = "Return Stop";
            }
        }
        #endregion Constructors
        /// <summary>
        /// Text to be displayed in cursor
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Return Stop";
        }
    }
}
