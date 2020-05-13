using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

using MDouble;
using MCore.Comp.MotionSystem;
using MCore.Comp.IOSystem.Input;

namespace AppMachine.Comp.Motion
{
    public class AppRealAxes:AxesBase
    {

        #region Constructor
        public AppRealAxes()
        {

        }

        public AppRealAxes(string name)
            : base(name)
        {

        }
        #endregion

    }
}
