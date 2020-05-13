using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Drawing;

using MCore.Comp.SMLib.Path;

namespace MCore.Comp.SMLib.Flow
{
    public class SMStart : SMFlowBase
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMStart()
        {
        }
        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></name>
        public SMStart(string name)
            : base(name)
        {
            this[eDir.Down] = new SMPathOut(0.5F);
            this[eDir.Up] = new SMPathOutPlug();
            this[eDir.Left] = new SMPathOutPlug();
            this[eDir.Right] = new SMPathOutPlug();
        }
        #endregion Constructors


        #region Overrides
        /// <summary>
        /// Text to be displayed in cursor
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Start element";
        }
        /// <summary>
        /// Returns the path to take from here
        /// </summary>
        /// <returns></returns>
        public override SMPathOut Run()
        {
            // Wait till Run or step
            StateMachine.WaitHandle.WaitOne();
            StateMachine.ReceivedStop = false;

            // Only one out
            return this[typeof(SMPathOut)];
        }
        #endregion Overrides
    }
}
