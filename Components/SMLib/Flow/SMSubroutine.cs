using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MCore.Comp.SMLib.Path;

namespace MCore.Comp.SMLib.Flow
{
    public class SMSubroutine : SMFlowContainer
    {
        private object _locker = new object();
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMSubroutine()
        {
        }
        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="text"></name>
        public SMSubroutine(string name)
            : base(name)
        {
            // Must have a Path in and a path out
            // Must have a Path in and a path out
            this[eDir.Down] = new SMPathOut(0.5F);
            this[eDir.Up] = new SMPathOutPlug();
            this[eDir.Left] = new SMPathOutStop(-0.5F);
            this[eDir.Right] = new SMPathOutPlug();
            
            //Move to Initialize for fixbug after clone 
            //Add(new SMStart(string.Empty) { Text = "Enter", GridLoc = new PointF(2.5f, 0.5f) });
            //Add(new SMExit(string.Empty) { Text = "Return", GridLoc = new PointF(2.5f, 6.5f) });
            //Add(new SMReturnStop(string.Empty) { Text = "Stop", GridLoc = new PointF(0.5f, 3.5f) });

        }
        /// <summary>
        /// Text to be displayed in cursor
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string text = string.Empty;
            text = "ID:" + this.Name + Environment.NewLine;
            return text;
        }
        #endregion Constructors


        #region Override

        public override void Initialize()
        {
            if (this.FilterByType(typeof(SMStart)) == null)
            {
                Add(new SMStart(string.Empty) { Text = "Enter", GridLoc = new PointF(2.5f, 0.5f) });
                Add(new SMExit(string.Empty) { Text = "Return", GridLoc = new PointF(2.5f, 6.5f) });
                Add(new SMReturnStop(string.Empty) { Text = "Stop", GridLoc = new PointF(0.5f, 3.5f) });
            }
            base.Initialize();
            
        }
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            base.PreRun();
        }

     
        #endregion

        /// <summary>
        /// Call this subroutine
        /// </summary>
        /// <remarks>Single entry limited</remarks>
        [StateMachineEnabled]
        public void Call()
        {
            lock (_locker)
            {
                SMPathOut smOut = Run();
                if (smOut is SMPathOutStop || smOut == null)
                {
                    StateMachine.ReceivedStop = true;
                }
                else if (smOut is SMPathOutError)
                {
                    // Already logged
                    throw new Exception();
                }
            }
        }
       
    }
}
