using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MCore.Comp
{
    public class DefaultLogger : LoggerBase
    {
        #region Private data


        #endregion

        #region Properties


        #endregion
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultLogger()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name">The name of this component</param>
        /// <Remarks>The storage subpath will be initially "Log"</Remarks>
        /// <Remarks>The (DATETIME) format of the filename will initialluy be "yyyy-MM-dd"</Remarks>
        /// <Remarks>The extension will be initially "Log"</Remarks>
        /// <Remarks>No sub folder is created.</Remarks>
        public DefaultLogger(string name)
            : base(name)
        {
        }

        ///// <summary>
        ///// Manual creation constructor
        ///// </summary>
        ///// <param name="name">The name of this component</param>
        ///// <param name="subPath">The storage subpath</param>
        ///// <Remarks>The (DATETIME) format of the filename will initialluy be "yyyy-MM-dd"</Remarks>
        ///// <Remarks>The extension will be initially "Log"</Remarks>
        ///// <Remarks>No sub folder is created.</Remarks>
        //public DefaultLogger(string name, string subPath)
        //    : this(name, subPath, "yyyy-MM-dd")
        //{
        //}
        ///// <summary>
        ///// Manual creation constructor
        ///// </summary>
        ///// <param name="name">The name of this component</param>
        ///// <param name="subPath">The storage subpath</param>
        ///// <param name="fmtFilename">The (DATETIME) format of the filename</param>
        ///// <Remarks>The extension will be initially "Log"</Remarks>
        ///// <Remarks>No sub folder is created.</Remarks>
        //public DefaultLogger(string name, string subPath, string fmtFilename)
        //    : this(name, subPath, fmtFilename, "log")
        //{
        //}
        ///// <summary>
        ///// Manual creation constructor
        ///// </summary>
        ///// <param name="name">The name of this component</param>
        ///// <param name="subPath">The storage subpath</param>
        ///// <param name="fmtFilename">The (DATETIME) format of the filename</param>
        ///// <param name="extension">The extension of the log file</param>
        ///// <Remarks>No sub folder is created.</Remarks>
        //public DefaultLogger(string name, string subPath, string fmtFilename, string extension)
        //    : this(name, subPath, fmtFilename, extension, string.Empty)
        //{
        //}
        ///// <summary>
        ///// Manual creation constructor
        ///// </summary>
        ///// <param name="name">The name of this component</param>
        ///// <param name="subPath">The storage subpath</param>
        ///// <param name="fmtFilename">The (DATETIME) format of the filename</param>
        ///// <param name="extension">The extension of the log file</param>
        ///// <param name="fmtSubFolder">The (DATETIME) format of the sub folder to create. (Optional)</param>
        //public DefaultLogger(string name, string subPath, string fmtFilename, string extension, string fmtSubFolder)
        //    : base(name, subPath, fmtFilename, extension, fmtSubFolder)
        //{
        //}

        #endregion Constructors



        #region Public Overrides

        #endregion Public Overrides

        protected override void OnLog(LogEntry logEntry)
        {
            // Check against mask for interest
            if ((logEntry.severity & Mask) == LogSeverity.NO_FLAGS || (logEntry.severity == LogSeverity.AlarmOff))
            {
                return;
            }
            // Simulation or default behaviour
            if (U.EnsureDirectory(FullPath))
            {
                string strFullLogFileName = GetLogFileName(logEntry.dateTime);
                U.EnsureDirectory(strFullLogFileName);
                using (StreamWriter logWriter = new StreamWriter(strFullLogFileName, true))
                {
                    logWriter.WriteLine(MakeLogString(logEntry.threadId, logEntry.dateTime, logEntry.severity, logEntry.procedureName, logEntry.text));
                    logWriter.Close();
                }
            }
            // Now perform actions
            if ((logEntry.severity & LogSeverity.Popup) == LogSeverity.Popup || (logEntry.severity & LogSeverity.Fatal) == LogSeverity.Fatal)
            {
                MessageBox.Show(logEntry.text, string.Format("{0} {1}", logEntry.procedureName, logEntry.severity));
                if ((logEntry.severity & LogSeverity.AlarmOn) == LogSeverity.AlarmOn)
                {
                    U.LogAlarmOff(logEntry);
                }
            }
            else if ((logEntry.severity & LogSeverity.Alert) == LogSeverity.Alert)
            {
                // Temp
                MessageBox.Show(logEntry.text, string.Format("{0} {1}", logEntry.procedureName, logEntry.severity));
            }
        }
        /// <summary>
        /// Get the full qualified log file name
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName(DateTime now)
        {
            string subFolder = string.Empty;
            if (!string.IsNullOrEmpty(FmtSubFolder))
            {
                subFolder = string.Format(@"{0}\", now.ToString(FmtSubFolder));
            }
            return string.Format(@"{0}\{1}{2}.{3}", FullPath, subFolder, now.ToString(FmtFilename), Extension);
        }

        /// <summary>
        /// Make standard log string
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="dateTime"></param>
        /// <param name="severity"></param>
        /// <param name="procedureName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private string MakeLogString(int threadId, DateTime dateTime, LogSeverity severity, string procedureName, string text)
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}",
                                    dateTime.ToString("dd-MM-yy, HH:mm:ss.fff"),
                                    severity,
                                    procedureName,
                                    threadId,
                                    AnsiOnly(text));
        }
        /// <summary>
        /// Strip out non-ansi text
        /// </summary>
        /// <param name="multiLingualText"></param>
        /// <returns></returns>
        private string AnsiOnly(string multiLingualText)
        {
            string ansiOnly = Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                        ),
                    Encoding.UTF8.GetBytes(multiLingualText)
                )
            );
            return ansiOnly.Trim();
        }
    }
}
