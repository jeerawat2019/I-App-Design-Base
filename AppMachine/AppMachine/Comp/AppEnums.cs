using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppMachine.Comp
{
    public class AppEnums
    {
        public enum ePartStatus
        {
            None,
            Pass,
            Fail,
            Missing,
        }

        public enum eInspecStatus
        {
            None=-1,
            Pass=0,
            Fail=1,
        }

        public enum eProductTab
        {
            None,
            Up,
            Dowm,
        }

        public enum eAccessLevel
        {
            Operator,
            Supervisor
        }

    }
}
