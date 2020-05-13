using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.VisionSystem
{
    partial class CameraBase
    {
        public enum PlugIns
        {
            /// <summary>Cognex Camera Ver 7</summary>
            CognexCamera7,
            /// <summary>Cognex Camera Ver 8</summary>
            CognexCamera8,
            /// <summary>Generic ZHt Camera that uses Cognex jobs</summary>
            CogZHtCamera,
            /// <summary>Cognex Camera Ver 9</summary>
            CognexCamera9,
        }
    }
}
