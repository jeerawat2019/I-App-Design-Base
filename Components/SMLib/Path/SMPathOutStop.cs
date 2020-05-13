using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.SMLib.Path
{
    public class SMPathOutStop : SMPathOut
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMPathOutStop()
        {
        }
        /// <summary>
        /// Manual creation Constructor
        /// </summary>
        /// <param name="initialGridDistance"></param>
        public SMPathOutStop(float initialGridDistance)
        {
            // Starts out to the left
            GridDistance = initialGridDistance;
        }
        #endregion Constructors
    }
}
