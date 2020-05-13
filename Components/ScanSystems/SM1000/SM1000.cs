using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MCore.Comp;
using MDouble;

using CtiUtils;
using LanmarkControls.LECSession;


namespace MCore.Comp.ScanSystem
{
    public class SM1000 : ScanSystemBase
    {

        private ECUtils _oECUtils = null;
        private Session _iSession = null;

        private string _name = string.Empty;
        private string _localIP = string.Empty;
        private string _remoteIP = string.Empty;
        private int _portNumber = 0;
        private const uint STREAMTIMOUT = 20;
        private const uint FIXEDDATATIMOUT = 20; 

        private Millimeters _mmXPos = new Millimeters(1.0);
        private Millimeters _mmYPos = new Millimeters(1.0);

        private Dictionary<uint, string> errorList = new Dictionary<uint, string>();


        #region Browsable Properties
        /// <summary>
        ///  The Local IP
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("SM1000")]
        public string DeviceName
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(() => Name); }
        }
        /// <summary>
        ///  The Local IP
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("SM1000")]
        public string LocalIP
        {
            get { return _localIP; }
            set { _localIP = value; NotifyPropertyChanged(() => LocalIP); }
        }

        /// <summary>
        ///  The Remote IP
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("SM1000")]
        public string RemoteIP
        {
            get { return _remoteIP; }
            set { _remoteIP = value; NotifyPropertyChanged(() => RemoteIP); }
        }

        /// <summary>
        ///  The Port
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("SM1000")]
        public int PortNumber
        {
            get { return _portNumber; }
            set { _portNumber = value; NotifyPropertyChanged(() => PortNumber); }
        }

        /// <summary>
        ///  The X relative movement
        /// </summary>
        [Browsable(true)]
        [Category("SM1000")]
        public Millimeters XPos
        {
            get { return _mmXPos; }
            set { _mmXPos = value; }
        }
        /// <summary>
        ///  The Y relative movement
        /// </summary>
        [Browsable(true)]
        [Category("SM1000")]
        public Millimeters YPos
        {
            get { return _mmYPos; }
            set { _mmYPos = value; }
        }
        
        #endregion Browsable Properties

        #region Public Properties

        public ECUtils ECUtil
        {
            get { return _oECUtils; }
        }

        public Session ECSession
        {
            get { return _iSession; }
        }

        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SM1000()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public SM1000(string name) 
            : base (name)
        {
        }
        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Initialize this Component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Initialize TCPIP
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }

            try
            {
                BuildErrorList();
                _oECUtils = new ECUtils();
                _iSession = new Session();
                Attach();
                Simulate = eSimulate.None;
            }
            catch (ForceSimulateException fsex)
            {
                throw fsex;
            }
            catch (Exception ex)
            {
                throw new ForceSimulateException(ex);
            }

        }

        public override void  Destroy()
        {
            Detach();
 	        base.Destroy();
        }


        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            GalvoX.Enable();
            GalvoY.Enable();
        }

        #endregion Overrides

        private void BuildErrorList()
        {            
            errorList.Add(0, "Success");
            errorList.Add(1, "TCP/IP networking access was denied");
            errorList.Add(2, "TCP/IP network communications error occured");
            errorList.Add(3, "Client is not connected to the server");
            errorList.Add(4, "Internal Error IllegalClientId");
            errorList.Add(5, "Internal Error InvalidPersistState");
            errorList.Add(8, "Requested server name is not valid");
            errorList.Add(9, "Bad parameter to a method call");
            errorList.Add(10, "TCP/IP networking error");
            errorList.Add(11, "Requested data file not found");
            errorList.Add(12, "Specified path does not exist");
            errorList.Add(13, "Access to server file system was denied");
            errorList.Add(14, "Access to the client file system was denied or Server is under control of a local pendant");
            errorList.Add(15, "XML data type is unknown");
            errorList.Add(16, "Internal event processing error");
            errorList.Add(17, "Server is not currently available");
            errorList.Add(19, "Server is currently aborting");
            errorList.Add(20, "Server action was aborted");
            errorList.Add(23, "Internal Error Exception");
            errorList.Add(24, "Requested action timed out");
            errorList.Add(25, "The requested fixed data was empty");
            errorList.Add(26, "Destination file already exists and over-write not specified");
            errorList.Add(28, "Server is already connected to a client");
            errorList.Add(29, "Server is in an error state and unavailable");
            errorList.Add(31, "Streaming data transmit buffer is full");
            
        }
        #region Public Calls to do service
        public override void Scan()
        {
            
        }

        public override void EnableAxis(MotionSystem.RealAxis axis, bool bEnable)
        {
            axis.Enabled = bEnable;
            axis.MotorCountsPerMM = 300;
            axis.MaxLimit = 20;
            axis.MinLimit = -20;
        }

        // Motion System overrides
        public override void HomeAxis(MotionSystem.RealAxis axis)
        {
            SendData("JumpAbs", "0,0");
            GalvoX.CurrentMotorCounts = 0;
            GalvoY.CurrentMotorCounts = 0;
        }

        public override void MoveAbsAxis(MotionSystem.RealAxis axis, MSpeed speed)
        {
            double x = GalvoX.TargetMotorCounts;
            double y = GalvoY.TargetMotorCounts;
            SendData("JumpAbs", string.Format("{0},{1}", x, y));
            GalvoX.CurrentMotorCounts = x;
            GalvoY.CurrentMotorCounts = y;
        }


        #endregion Public Calls to do service

        private const string _initJob =
            "<set id='Units'>0</set>\r\n" +
            "<set id='XYCalFactor'>500</set>\r\n";

        public void Attach()
        {
            if (null != _iSession && !_iSession.SessionActive && !string.IsNullOrEmpty(LocalIP))
            {
                uint err = _iSession.loginSession(LocalIP, RemoteIP, PortNumber, FIXEDDATATIMOUT);
                if (err != 0)
                {
                    U.LogPopup("EC1000 loginSession Error= {0}", errorList[err]);
                }
                else
                {
                    string str = string.Format("<Data type='JobData' rev='2.0'>\r\n{0}</Data>", _initJob);
                    U.LogInfo("sendStreamData({0})", str);
                    err = _iSession.sendStreamData(str, STREAMTIMOUT);
                    if (err != 0)
                    {
                        U.LogPopup("EC1000 InitJob Error= {0}", errorList[err]);
                    }
                }
            }
        }

        public void Detach()
        {
            if (null != _iSession)
            {
                uint err = _iSession.logoutSession(FIXEDDATATIMOUT);
                if (err != 0)
                {
                    U.LogPopup("EC1000 logoutSession Error= {0}", errorList[err]);
                }
                GalvoX.Disable();
                GalvoY.Disable();
            }
        }

        /// <summary>
        /// Reassign the connection
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="localIP"></param>
        /// <param name="remoteIP"></param>
        /// <param name="portNumber"></param>
        public void AttachToDevice(string name, string localIP, string remoteIP, int portNumber) 
        {
            Detach();
            DeviceName = name;
            LocalIP = localIP;
            RemoteIP = remoteIP;
            PortNumber = portNumber;
            Attach();
        }

        public string RequestFixedData(int fixedDataType, string storageName)
        {
            string strAll = "";
            uint err = 0;
            try
            {
                if (fixedDataType == 0)
                {
                    err = _iSession.getFixedDataList(out strAll, FIXEDDATATIMOUT);
                }
                else if (fixedDataType == -1)
                {
                    int count = 0;
                    err = _iSession.requestJobNameList(2, out count, out strAll, FIXEDDATATIMOUT);
                }
                else if (fixedDataType == -2)
                {
                    err = _iSession.copyJobData(2, storageName, FIXEDDATATIMOUT);
                }
                else
                {
                    err = _iSession.requestFixedData(fixedDataType, storageName, out strAll, FIXEDDATATIMOUT);
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "EC1000 requestFixedData Error");
            }
            if (err != 0)
            {
                U.LogPopup("EC1000 requestFixedData Error= {0}", errorList[err]);
            }
            return strAll;
        }

        public void RequestData(string cmd)
        {
            // <Data type='JobData' rev='2.0'>
            // <JumpRel>-25; 50</JumpRel>
            try
            {
                string str = string.Format("<Data type='JobData' rev='2.0'><get id='{0}' /></Data>", cmd);
                //string str = string.Format("<Data type='JobData' rev='2.0'>\r\n<get id='{0}' />\r\n</Data>", cmd);
                U.LogInfo("sendStreamData({0})", str);
                uint err = _iSession.sendStreamData(str, STREAMTIMOUT);
                if (err != 0)
                {
                    U.LogPopup("EC1000 SendData Error= {0}", errorList[err]);
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "EC1000 SendData Error");
            }
        }

        public void SendSetData(string name, string value)
        {
            try
            {
                string str = string.Format("<Data type='JobData' rev='2.0'><set id='{0}'>{1}</set></Data>", name, value);
               
                U.LogInfo("sendStreamData({0})", str);
                uint err = _iSession.sendStreamData(str, STREAMTIMOUT);
                if (err != 0)
                {
                    U.LogPopup("EC1000 SendData Error= {0}", errorList[err]);
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "EC1000 SendData Error");
            }
        }

        public void SendData(string cmd, string values)
        {
            // <Data type='JobData' rev='2.0'>
            // <JumpRel>-25; 50</JumpRel>
            try
            {
                string str = string.Empty;
                if (string.IsNullOrEmpty(values))
                {
                    str = string.Format("<Data type='JobData' rev='2.0'><{0}></{0}></Data>", cmd);
                }
                else
                {
                    str = string.Format("<Data type='JobData' rev='2.0'><{0}>{1}</{0}></Data>", cmd, values);
                }
                U.LogInfo("sendStreamData({0})", str);
                uint err = _iSession.sendStreamData(str, STREAMTIMOUT);
                if (err != 0)
                {
                    U.LogPopup("EC1000 SendData Error= {0}", errorList[err]);
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "EC1000 SendData Error");
            }
        }
    }
}
