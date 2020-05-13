using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MCore.Comp
{
    public abstract partial class LoggerBase : CompBase
    {
        
        #region Private data

        private string _subPath = "Log";
        private string _rootFolder = string.Empty;
        private string _fmtSubFolder = string.Empty;
        private string _fmtFilename = "yyyy-MM-dd";
        private string _extension = "log";
        private LogSeverity _mask = LogSeverity.MASK_ALL;

        #endregion

        #region Browsable Properties

        /// <summary>
        /// The folder where to put the data
        /// </summary>
        [Browsable(true)]
        public string SubPath
        {
            get { return _subPath; }
            set { _subPath = value; }
        }

        /// <summary>
        /// The root folder where to put the data
        /// </summary>
        [Browsable(true)]
        public string RootFolder
        {
            get { return _rootFolder; }
            set { _rootFolder = value; }
        }

        /// <summary>
        /// The full path where to put the data
        /// </summary>
        [Browsable(true)]
        [XmlIgnore]
        protected string FullPath
        {
            get 
            { 
                string rootFolder = RootFolder;
                if (string.IsNullOrEmpty(rootFolder))
                {
                    rootFolder = U.RootComp.RootFolder;
                }
                return string.Format(@"{0}\{1}", rootFolder, _subPath); 
            }
        }

        /// <summary>
        /// The mask of allowed log entries
        /// </summary>
        [Browsable(true)]
        public LogSeverity Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }

        /// <summary>
        /// The format of the Filename 
        /// </summary>
        [Browsable(true)]
        public string FmtFilename
        {
            get { return _fmtFilename; }
            set { _fmtFilename = value; }
        }

        /// <summary>
        /// The log file extension 
        /// </summary>
        [Browsable(true)]
        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }
        /// <summary>
        /// The format of the sub folder to create 
        /// </summary>
        [Browsable(true)]
        public string FmtSubFolder
        {
            get { return _fmtSubFolder; }
            set { _fmtSubFolder = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoggerBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name">The name of this component</param>
        public LoggerBase(string name)
            : base(name)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            CompRoot.OnLogEntry += new CompRoot.DelLogEntry(OnLog);
        }
        /// <summary>
        ///// Manual creation constructor
        ///// </summary>
        ///// <param name="name">The name of this component</param>
        ///// <param name="subPath">The storage subpath</param>
        ///// <param name="fmtFilename">The (DATETIME) format of the filename</param>
        ///// <param name="extension">The extension of the log file</param>
        ///// <param name="fmtSubFolder">The (DATETIME) format of the sub folder to create. (Optional)</param>
        //public LoggerBase(string name, string subPath, string fmtFilename, string extension, string fmtSubFolder)
        //    : base(name)
        //{
        //    _basePath = string.Format(@"{0}\{1}", U.RootComp.RootFolder, subPath);
        //    _extension = extension;
        //    _fmtFilename = fmtFilename;
        //    _fmtSubFolder = fmtSubFolder;
        //    CompRoot.OnLogEntry += new CompRoot.DelLogEntry(OnLog);
        //}
        #endregion Constructors

        #region Pure Virtuals

        protected abstract void OnLog(LogEntry logEntry);

        #endregion Virtuals
    }
}
