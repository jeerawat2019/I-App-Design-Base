using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp
{
    public abstract class LogSystemBase : CompSystem
    {
        #region Private data

        #endregion

        #region Properties


        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public LogSystemBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public LogSystemBase(string name)
            : base(name)
        {
        }
        #endregion Constructors

        /// <summary>
        /// Send the log
        /// </summary>
        /// <param name="logEntry"></param>
        public abstract void Log(LogEntry logEntry);

        public virtual void SaveSettings() { }
    }
}
