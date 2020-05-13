using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.IOSystem
{
    partial class IOSystemBase
    {
        public enum PlugIns
        {
            /// <summary>ADLinkIO</summary>
            ADLinkIO,
            /// <summary>MEDAQLib</summary>
            MEDAQLib,
            /// <summary>DPMCtrl</summary>
            DPMCtrl,
            /// <summary>USTDCtrl</summary>
            USTDCtrl,
            /// <summary>Keyence_LJ_V7001</summary>
            Keyence_LJ_V7001,
            /// <summary>Adventech</summary>
            Advantech,
            /// <summary>ModbusTcpIO</summary>
            ModbusTcpIO
        }
    }
}
