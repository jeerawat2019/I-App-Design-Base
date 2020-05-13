using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using MCore;
using MCore.Comp;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;

namespace AppMachine.Comp.IO
{
    public class AppSafetyInput : BoolInput
    {
        [Category("Safety Value"), Browsable(true)]
        public bool SafetyValue
        {

            get { return (GetPropValue(() => SafetyValue,true)); }
            set { SetPropValue(() => SafetyValue, value); }
        }

        public bool IsSafety
        {
            get { return this.Value == SafetyValue; }  
        }

        #region Constructor
        public AppSafetyInput()
        {

        }

        public AppSafetyInput(string name) :
            base(name)
        {
           
        }
        #endregion
    }
}
