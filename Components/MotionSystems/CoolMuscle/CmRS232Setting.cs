using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MDouble;

namespace MCore.Comp.MotionSystem
{
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class CmRS232Settings
    {
        #region Static Definitions
        /// <summary>
        /// Enum for Comport
        /// </summary>
        public enum eComport
        {
            /// <summary>Port COM 1</summary>
            COM1 = 1,
            /// <summary>Port COM 2</summary>
            COM2,
            /// <summary>Port COM 3</summary>
            COM3,
            /// <summary>Port COM 4</summary>
            COM4,
            /// <summary>Port COM 5</summary>
            COM5,
            /// <summary>Port COM 6</summary>
            COM6,
            /// <summary>Port COM 7</summary>
            COM7,
            /// <summary>Port COM 8</summary>
            COM8,
            /// <summary>Port COM 9</summary>
            COM9,
            /// <summary>Port COM 10</summary>
            COM10,
            /// <summary>Port COM 11</summary>
            COM11,
            /// <summary>Port COM 12</summary>
            COM12,
            /// <summary>Port COM 13</summary>
            COM13,
            /// <summary>Port COM 14</summary>
            COM14,
            /// <summary>Port COM 15</summary>
            COM15,
            /// <summary>Port COM 16</summary>
            COM16,
            /// <summary>Port COM 17</summary>
            COM17,
            /// <summary>Port COM 18</summary>
            COM18,
            /// <summary>Port COM 19</summary>
            COM19,
            /// <summary>Port COM 20</summary>
            COM20,
            /// <summary>Port COM 21</summary>
            COM21,
            /// <summary>Port COM 22</summary>
            COM22
        };
        /// <summary>
        /// Enum for Baud Rate
        /// </summary>
        public enum eBaudRate
        {
            /// <summary>NotDefined</summary>
            NotDefined,
            /// <summary>Baud Rate 75bps</summary>
            BR75 = 75,
            /// <summary>Baud Rate 110bps</summary>
            BR110 = 110,
            /// <summary>Baud Rate 134bps</summary>
            BR134 = 134,
            /// <summary>Baud Rate 150bps</summary>
            BR150 = 150,
            /// <summary>Baud Rate 300bps</summary>
            BR300 = 300,
            /// <summary>Baud Rate 600bps</summary>
            BR600 = 600,
            /// <summary>Baud Rate 1200bps</summary>
            BR1200 = 1200,
            /// <summary>Baud Rate 1800bps</summary>
            BR1800 = 1800,
            /// <summary>Baud Rate 2400bps</summary>
            BR2400 = 2400,
            /// <summary>Baud Rate 4800bps</summary>
            BR4800 = 4800,
            /// <summary>Baud Rate 7200bps</summary>
            BR7200 = 7200,
            /// <summary>Baud Rate 9600bps</summary>
            BR9600 = 9600,
            /// <summary>Baud Rate 14400bps</summary>
            BR14400 = 14400,
            /// <summary>Baud Rate 19200bps</summary>
            BR19200 = 19200,
            /// <summary>Baud Rate 38400bps</summary>
            BR38400 = 38400,
            /// <summary>Baud Rate 57600bps</summary>
            BR57600 = 57600,
            /// <summary>Baud Rate 115200bps</summary>
            BR115200 = 115200,
            /// <summary>Baud Rate 128000bps</summary>
            BR128000 = 128000,
        };

        /// <summary>
        /// Enum for Data Length
        /// </summary>
        public enum eDataLength
        {
            /// <summary>7 bits</summary>
            DataLength_7 = 7,
            /// <summary>8 bits</summary>
            DataLength_8 = 8,
        };

        /// <summary>
        /// Enum for new line options
        /// </summary>
        public enum eNewLine
        {
            /// <summary>CR</summary>
            CR,
            /// <summary>LF</summary>
            LF,
            /// <summary>CRLF</summary>
            CRLF
        }

        #endregion Static Definitions

        #region private Data

        /// <summary>Commport</summary>
        private eComport _commPort = eComport.COM1;
        /// <summary>
        /// Baud Rate
        /// </summary>
        private eBaudRate _baudRate = eBaudRate.BR38400;
        /// <summary>
        /// Data Length
        /// </summary>         
        private eDataLength _dataLength = eDataLength.DataLength_8;

        /// <summary>
        /// Handshake
        /// </summary>
        private Handshake _handShake = Handshake.None;

        /// <summary>
        /// Parity
        /// </summary>
        private Parity _parity = Parity.Even;

        /// <summary>
        /// Stop Bit Length
        /// </summary>
        private StopBits _stopBits = StopBits.One;

        /// <summary>
        /// Com Port Read Write Time Out
        /// </summary>
        private Miliseconds _readWriteTimeOut = 500;

        /// <summary>
        /// True if data received is to be handled instead of default 
        /// </summary>
        private bool _ownDataHandler = false;

        private string _newLine = "\r";

        #endregion private Data

        /// <summary>
        /// The port name
        /// </summary>
        [Browsable(true)]
        [Category("RS232Parms")]
        public eComport CommPort
        {
            get { return _commPort; }
            set { _commPort = value; }
        }

        /// <summary>
        /// Return the baud rate
        /// </summary>
        [Browsable(true)]
        [Category("RS232Parms")]
        public eBaudRate BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }
        /// <summary>
        /// Return the data length
        /// </summary>
        [Browsable(true)]
        [Category("RS232Parms")]
        public eDataLength DataLength
        {
            get { return _dataLength; }
            set { _dataLength = value; }
        }
        /// <summary>
        /// Return the parity
        /// </summary>
        [Browsable(true)]
        [Category("RS232Parms")]
        public Parity Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }
        /// <summary>
        /// Return the parity
        /// </summary>
        [Browsable(true)]
        [Category("RS232Parms")]
        public StopBits StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        /// <summary>
        /// The time out in miliseconds
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        public Miliseconds ReadWriteTimeOut
        {
            get { return _readWriteTimeOut; }
            set { _readWriteTimeOut = value; }
        }

        /// <summary>
        /// Get/set the handshake
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        public Handshake HandShake
        {
            get { return _handShake; }
            set { _handShake = value; }
        }

        /// <summary>
        /// True if data received is to be handled instead of default
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        public bool OwnDataHandler
        {
            get { return _ownDataHandler; }
            set { _ownDataHandler = value; }
        }

        /// <summary>
        /// Get the raw newLine string
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public string RawNewLine
        {
            get { return _newLine; }
        }

        /// <summary>
        /// Get/set the newLine
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        public eNewLine NewLine
        {
            get
            {
                switch (_newLine)
                {
                    default:
                    case "\r\n":
                        return eNewLine.CRLF;
                    case "\r":
                        return eNewLine.CR;
                    case "\n":
                        return eNewLine.LF;
                }
            }
            set
            {
                switch (value)
                {
                    default:
                    case eNewLine.CRLF:
                        _newLine = "\r\n";
                        break;
                    case eNewLine.CR:
                        _newLine = "\r";
                        break;
                    case eNewLine.LF:
                        _newLine = "\n";
                        break;
                }
            }
        }
        #region Constructors

        /// <summary>
        /// For serialization
        /// </summary>
        public CmRS232Settings()
        {
        }

        /// <summary>
        /// For first time setup
        /// </summary>
        /// <param name="commPort"></param>
        /// <param name="baudRate"></param>
        public CmRS232Settings(eComport commPort, eBaudRate baudRate)
        {
            _commPort = commPort;
            _baudRate = baudRate;
        }
        #endregion Constructors

        /// <summary>
        /// Default Text to display
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}, {1})", _commPort.ToString(), _baudRate.ToString());
        }
    }
}
