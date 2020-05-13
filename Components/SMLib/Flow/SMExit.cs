using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MCore.Comp.SMLib.Path;

namespace MCore.Comp.SMLib.Flow
{
    public class SMExit : SMFlowBase
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMExit()
        {
        }
        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="text"></name>
        public SMExit(string text)
            : base(text)
        {
            // Must have a path out
            this[eDir.Up] = new SMPathOutPlug();
            this[eDir.Down] = new SMPathOutPlug();
            this[eDir.Left] = new SMPathOutPlug();
            this[eDir.Right] = new SMPathOutPlug();
        }
        #endregion Constructors
        /// <summary>
        /// Text to be displayed in cursor
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Exit";
        }
    }
}
