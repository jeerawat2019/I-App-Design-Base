﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MCore.Comp.SMLib.Path;

namespace MCore.Comp.SMLib.Flow
{
    public class SMReturnNo : SMExit
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMReturnNo()
        {
        }
        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="text"></name>
        public SMReturnNo(string text)
            : base(text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Text = "Return No";
            }
        }
        #endregion Constructors
        /// <summary>
        /// Text to be displayed in cursor
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Return No";
        }
    }
}
