using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MDouble;

namespace MCore.Comp.Communications
{
    public class RS232 : CommunicationBase
    {

        #region private Data

        private SerialPort _port = null;
        private object _lockPort = new object();

        public object LockPort
        {
            get { return _lockPort; }
        }

        private string _incompleteReadBuffer = string.Empty;

        private RS232Settings _settings = null;
 
        #endregion private Data

        /// <summary>
        /// For serialization
        /// </summary>
        [Browsable(false)]
        [XmlElement(ElementName="Settings")]
        public RS232Settings SerSettings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }


        #region Public Browsable Properties

        /// <summary>
        /// Get the Comm Port
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public bool FullyDefined
        {
            get { return GetPropValue(() => FullyDefined); }
            set { SetPropValue(() => FullyDefined, value); }
        }

        /// <summary>
        /// Get the Comm Port
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public RS232Settings.eComport CommPort
        {
            get { return _settings.CommPort; }
            set { _settings.CommPort = value; }
        }

        /// <summary>
        /// Return the baud rate
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public RS232Settings.eBaudRate BaudRate
        {
            get { return _settings.BaudRate; }
            set { _settings.BaudRate = value; }
        }
        /// <summary>
        /// Return the data length
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public RS232Settings.eDataLength DataLength
        {
            get { return _settings.DataLength; }
            set { _settings.DataLength = value; }
        }
        /// <summary>
        /// Return the parity
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public Parity Parity
        {
            get { return _settings.Parity; }
            set { _settings.Parity = value; }
        }
        /// <summary>
        /// Return the parity
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public StopBits StopBits
        {
            get { return _settings.StopBits; }
            set { _settings.StopBits = value; }
        }

        /// <summary>
        /// The time out in miliseconds
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public Miliseconds ReadWriteTimeOut
        {
            get { return _settings.ReadWriteTimeOut; }
            set { _settings.ReadWriteTimeOut = value; }
        }
        /// <summary>
        /// Get the port statuys
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public string PortStatus
        {
            get
            {
                lock (_lockPort)
                {
                    if (_port != null)
                    {
                        return _port.IsOpen ? "Opened" : "Closed";
                    }
                }
                return "Not yet Initialized";
            }
        }
        /// <summary>
        /// Get the Last port read
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public string LastPortRead
        {
            get { return GetPropValue(() => LastPortRead, string.Empty); }
            set { SetPropValue(() => LastPortRead, value); }
        }
        /// <summary>
        /// Get the Last port read
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public string LastPortWrite
        {
            get { return GetPropValue(() => LastPortWrite, string.Empty); }
            set { SetPropValue(() => LastPortWrite, value); }
        }
        /// <summary>
        /// Used to send command in property browser
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public string SendCommand
        {
            get { return string.Empty;  }
            set { WriteLine(value); }
        }
        /// <summary>
        /// Get/set the handshake
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public Handshake HandShake
        {
            get { return _settings.HandShake; }
            set { _settings.HandShake = value; }
        }

        /// <summary>
        /// True if data received is to be handled instead of default
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public bool OwnDataHandler
        {
            get { return _settings.OwnDataHandler; }
            set { _settings.OwnDataHandler = value; }
        }


        /// <summary>
        /// Get/set the newLine
        /// </summary>
        [Browsable(true)]
        [Category("RS232")]
        [XmlIgnore]
        public RS232Settings.eNewLine NewLine
        {
            get { return _settings.NewLine; }
            set { _settings.NewLine = value; }
        }
        #endregion Public Browsable Properties

        #region Non Browsable Properties

        /// <summary>
        /// Get/Set the internal port
        /// </summary>
        [Browsable(true)]
        [XmlIgnore]
        public SerialPort Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// Return true if the Serial Port is Open
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool IsOpen
        {
            get 
            {
                lock (_lockPort)
                {
                    return _port != null && _port.IsOpen;
                }
            }
        }
        /// <summary>
        /// Return the baud rate as an integer
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public Int32 BaudRateInt
        {
            get { return (int)(RS232Settings.eBaudRate)_settings.BaudRate; }
        }

        #endregion Non Browsable Properties



        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RS232()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public RS232(string name)
            : base(name)
        {
            _settings = new RS232Settings();
        }
        #endregion Constructors


        #region Public Methods
        /// <summary>
        /// Initialize this component
        /// </summary>
        public override void Initialize()
        {
            if (FullyDefined && !IsOpen)
            {
                try
                {
                    RefreshPort();
                }
                catch (Exception ex)
                {
                    U.LogInfo(ex, "Failed to open {0}", Nickname);
                }
            }
            base.Initialize();
        }


        private void ClosePort()
        {
            lock (_lockPort)
            {
                if (_port != null)
                {
                    System.IO.Ports.SerialPort port = _port;
                    _port = null;
                    try
                    {
                        port.DataReceived -= new SerialDataReceivedEventHandler(OnSerialDataReceived);
                        port.Close();
                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex, "Unable to close Serial Port {0}", CommPort);
                    }
                }
            }
        }

        /// <summary>
        /// Destroy the object
        /// </summary>
        public override void Destroy()
        {
            ClosePort();
            base.Destroy();
        }

        /// <summary>
        /// Port Write function.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override void WriteLine(string cmd, params object[] args)
        {
            if (_port == null)
                return;
            lock (_lockPort)
            {
                if (_port == null)
                    return;
                cmd = string.Format(cmd, args);
                _port.WriteLine(cmd);
                LastPortWrite = cmd;
            }
        }
        /// <summary>
        /// Port Write function.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override void Write(string cmd, params object[] args)
        {
            if (_port == null)
                return;
            lock (_lockPort)
            {
                if (_port == null)
                    return;
                cmd = string.Format(cmd, args);
                _port.Write(cmd);
                LastPortWrite = cmd;
            }
        }

        /// <summary>
        /// Port Refresh.
        /// </summary>
        public void RefreshPort()
        {
            // Validate all settings
            if (!FullyDefined)
            {
                return;
            }

            lock (_lockPort)
            {
                if (_port != null && _port.IsOpen)
                {
                    //Cannot change any configuration during port open.
                    ClosePort();
                }

                _port = new SerialPort(CommPort.ToString());

                try
                {
                    //_port.PortName = CommPort.ToString();
                    //_port.DtrEnable = true;
                    _port.BaudRate = BaudRateInt;
                    _port.Handshake = HandShake;
                    _port.DataBits = (int)DataLength;
                    _port.StopBits = StopBits;
                    _port.Parity = Parity;
                    _port.ReceivedBytesThreshold = 1;
                    _port.WriteBufferSize = 100;
                    _port.ReadBufferSize = 100;
                    _port.NewLine = _settings.RawNewLine;
                    _port.ReadTimeout = (int)(double)ReadWriteTimeOut;
                    _port.WriteTimeout = (int)(double)ReadWriteTimeOut;
                    if (!OwnDataHandler)
                    {
                        _port.DataReceived += new SerialDataReceivedEventHandler(OnSerialDataReceived);
                    }
                    _port.Open();
                    U.LogInfo("{0} finished opening port {1} port status = {2}", this.Nickname, CommPort, PortStatus);
                }
                catch (Exception ex)
                {
                    ClosePort();
                    throw new ForceSimulateException(ex, "Could not open CommPort {0}", CommPort);
                }
            }
        }

        private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string newData = string.Empty;
            lock (_lockPort)
            {
                newData = _port.ReadExisting();
            }
            string completeData = _incompleteReadBuffer + newData;
            if (completeData.EndsWith(_settings.RawNewLine))
            {
                // Its whole and complete.  Use it all
                _incompleteReadBuffer = string.Empty;
            }
            else
            {
                // Strip off the incomplete and process the complete
                int i = completeData.LastIndexOf(_settings.RawNewLine);
                if (i < 0)
                {
                    // None of it is complete
                    _incompleteReadBuffer = completeData;
                    return;
                }
                // Point to end of new line
                i += _settings.RawNewLine.Length;
                // Some is complete. strip off incomplete
                _incompleteReadBuffer = completeData.Substring(i);
                completeData = completeData.Substring(0, i);
            }
            //if (CommPort == eComport.COM3)
            //{
            //    U.LogInfo("Serial Received = {0}", completeData);
            //}
            string[] split = completeData.Split(new string[] { _settings.RawNewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string text in split)
            {
                LastPortRead = text.Trim();
                FireDataRecieved(LastPortRead);
            }
        }

        #endregion
    }
}
