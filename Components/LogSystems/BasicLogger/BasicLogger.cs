using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MCore.Comp
{
    public class BasicLogger : LoggerBase
    {
        #region Private data


        #endregion

        #region Properties


        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BasicLogger()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public BasicLogger(string name)
            : base(name)
        {
        }
        #endregion Constructors

        protected override void OnLog(LogEntry logEntry)
        {
        }
    }
}
