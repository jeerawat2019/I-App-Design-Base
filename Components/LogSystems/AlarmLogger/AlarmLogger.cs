using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MCore.Comp.LogSystem
{
    public class AlarmLogger : LoggerBase
    {
        #region Private data


        #endregion

        #region Properties


        #endregion
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AlarmLogger()
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
        public AlarmLogger(string name)
            : base(name)
        {
        }


        #endregion Constructors



        #region Public Overrides

        public override void Initialize()
        {
            Mask = LogSeverity.AlarmOn | LogSeverity.AlarmOff;
            base.Initialize();
        }

        #endregion Public Overrides

        protected override void OnLog(LogEntry logEntry)
        {
            // Check against mask for interest
            if ((logEntry.severity & Mask) == LogSeverity.NO_FLAGS)
            {
                return;
            }
            // Simulation or default behaviour
            if (U.EnsureDirectory(FullPath))
            {
                string strFullLogFileName = GetLogFileName(logEntry.dateTime);
                U.EnsureDirectory(strFullLogFileName);
                string startStop = (logEntry.severity & LogSeverity.AlarmOn) == LogSeverity.AlarmOn ? "START" : "END";
                using (StreamWriter logWriter = new StreamWriter(strFullLogFileName, true))
                {
                    logWriter.WriteLine(MakeLogString(logEntry.dateTime, startStop, logEntry.text));
                    logWriter.Close();
                }
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
        private string MakeLogString(DateTime dateTime, string startStop, string text)
        {
            return string.Format("{0}, {1}, {2}",
                                    dateTime.ToString("yyyy-MM-dd, HH:mm:ss"),
                                    startStop,
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
