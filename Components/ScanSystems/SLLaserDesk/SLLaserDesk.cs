using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

using MCore;
using MCore.Comp.Communications;

namespace MCore.Comp.ScanSystem
{
    public class SLLaserDesk:ScanSystemBase
    {

        private object _locker = new object();

        /// <summary>
        /// LaserDesk Process
        /// </summary>
        [XmlIgnore]
        public Process SLLaserDeskProcesss = null;

        private SLLaserDeskTcp _slTcp = null;

        private SLLaserDeskWrapperCtrl _slLaserDeskWrapperCtrl = null;

        private static string LaserJobFilesFolder = "Laser Job Files";

        private bool _contQueryStatus = true;

        System.Threading.Thread _statusPollingThread = null;

        [XmlIgnore]
        public string LaserJobFilesRootPath
        {
            get { return string.Format(@"{0}\{1}\", U.RootComp.RootFolder, LaserJobFilesFolder); }
        }

        
        [XmlIgnore]
        [Browsable(true)]
        [Category("LaserDesk")]
        [Description("Laser Desk TCP Comp")]
        public TCPIP TCPComm
        {
            get { return GetPropValue(() => TCPComm); }
            set { SetPropValue(() => TCPComm,value); }
        }


        #region SL LaserDesk Status

        [Category("Status"), Browsable(true), Description("Program runs")]
        [XmlIgnore]
        public bool RM_STATE_WND_OPEN
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_WND_OPEN, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_WND_OPEN, value); }
        }


        [Category("Status"), Browsable(true), Description("RTC5 PC interface borad initialized")]
        [XmlIgnore]
        public bool RM_STATE_RTC_INIT
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_RTC_INIT, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_RTC_INIT, value); }
        }


        [Category("Status"), Browsable(true), Description("Laser system is initialized")]
        [XmlIgnore]
        public bool RM_STATE_LAS_INIT
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_LAS_INIT, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_LAS_INIT, value); }
        }


        [Category("Status"), Browsable(true), Description("All external controls are initialized")]
        [XmlIgnore]
        public bool RM_STATE_MOT_INIT
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_MOT_INIT, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_MOT_INIT, value); }
        }


        [Category("Status"), Browsable(true), Description("All hardware components are initialized (sum of bit 0-3)")]
        [XmlIgnore]
        public bool RM_STATE_ALL_INIT
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_ALL_INIT, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_ALL_INIT, value); }
        }


        [Category("Status"), Browsable(true), Description("RTC command thread runs (equals output pin 12)")]
        [XmlIgnore]
        public bool RM_STATE_READY
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_READY, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_READY, value); }
        }


        [Category("Status"), Browsable(true), Description("Automatic mode on (equals output pin 13)")]
        [XmlIgnore]
        public bool RM_STATE_AUTOMODE
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_AUTOMODE, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_AUTOMODE, value); }
        }

        [Category("Status"), Browsable(true), Description("List is in execution (equals output pin 14)")]
        [XmlIgnore]
        public bool RM_STATE_LST_EXEC
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_LST_EXEC, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_LST_EXEC, value); }
        }


        [Category("Status"), Browsable(true), Description("Execution error (equals output pin 15)")]
        [XmlIgnore]
        public bool RM_STATE_LST_EXE_ERR
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_LST_EXE_ERR, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_LST_EXE_ERR, value); }
        }


        [Category("Status"), Browsable(true), Description("Remote control mode on")]
        [XmlIgnore]
        public bool RM_STATE_RM_MODE
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_RM_MODE, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_RM_MODE, value); }
        }


        [Category("Status"), Browsable(true), Description("Job is loaded, ready for execution")]
        [XmlIgnore]
        public bool RM_STATE_JOB_LOAD
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_JOB_LOAD, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_JOB_LOAD, value); }
        }


        [Category("Status"), Browsable(true), Description("Execution list calculation in progress")]
        [XmlIgnore]
        public bool RM_STATE_LST_CALC
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_LST_CALC, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_LST_CALC, value); }
        }


        [Category("Status"), Browsable(true), Description("Command execution error occured")]
        [XmlIgnore]
        public bool RM_STATE_CMD_ERR
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_CMD_ERR, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_CMD_ERR, value); }
        }


        [Category("Status"), Browsable(true), Description("Laser system is in error state")]
        [XmlIgnore]
        public bool RM_STATE_LAS_ERR
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_LAS_ERR, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_LAS_ERR, value); }
        }


        [Category("Status"), Browsable(true), Description("Laser is switched on")]
        [XmlIgnore]
        public bool RM_STATE_LAS_ON
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_LAS_ON, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_LAS_ON, value); }
        }

        [Category("Status"), Browsable(true), Description("Error detected on hardware devices")]
        [XmlIgnore]
        public bool RM_STATE_DEV_ERR
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_DEV_ERR, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_DEV_ERR, value); }
        }


        [Category("Status"), Browsable(true), Description("Scan head state is OK")]
        [XmlIgnore]
        public bool RM_STATE_HEAD_OK
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_HEAD_OK, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_HEAD_OK, value); }
        }


        [Category("Status"), Browsable(true), Description("Set, when start has been executed, reset by start command")]
        [XmlIgnore]
        public bool RM_STATE_EXEC_DONE
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_EXEC_DONE, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_EXEC_DONE, value); }
        }


        [Category("Status"), Browsable(true), Description("Set, when pilot laser mode is active")]
        [XmlIgnore]
        public bool RM_STATE_PILOT_MODE
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_PILOT_MODE, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_PILOT_MODE, value); }
        }


        [Category("Status"), Browsable(true), Description("Set if the last job execution was aborted")]
        [XmlIgnore]
        public bool RM_STATE_JOB_ABORTED
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_JOB_ABORTED, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_JOB_ABORTED, value); }
        }


        [Category("Status"), Browsable(true), Description("Set if the last job execution was aborted")]
        [XmlIgnore]
        public bool RM_STATE_SWITCH_AUTOMODE
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RM_STATE_SWITCH_AUTOMODE, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => RM_STATE_SWITCH_AUTOMODE, value); }
        }
        #endregion

        /// <summary>
        /// LaserDesk TCP
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public SLLaserDeskTcp SLTcp
        {
            get
            {
                if (!_slTcp.IsConnected)
                {
                    _slTcp.ConnectToLaserDesk(TCPComm.IPAddress,TCPComm.Port);

                    //Login
                    List<Byte> loginParam = null;
                    loginParam = _slTcp.CovertStringParamToByteList("1990");
                    //List<Byte> loginCommandList = _slTcp.BuildCommand(1, loginParam);
                    //_slTcp.SendTCPCommand(loginCommandList);
                    //List<Byte> loginRetByte = _slTcp.ReadTcpReturn();
                    ExecuteCommand(1, loginParam);

                    //Remoteon
                    List<Byte> remoteOnParam = null;
                    //List<Byte> remoteOnCommandList = _slTcp.BuildCommand(5, remoteOnParam);
                    //_slTcp.SendTCPCommand(remoteOnCommandList);
                    //List<Byte> remoteRetByte = _slTcp.ReadTcpReturn();
                    ExecuteCommand(5, remoteOnParam);

                }
                return _slTcp;
            }
        }

        

        [Browsable(true)]
        [Category("LaserDesk")]
        [Description("Laser Desk Application Path")]
        public String ApplicationPath
        {
            get { return GetPropValue(() => ApplicationPath, @"C:\Program Files (x86)\SCANLAB\laserDESK\SLLaserDesk.exe"); }
            set { SetPropValue(() => ApplicationPath, value); }
        }

        [Browsable(true)]
        [Category("LaserDesk")]
        [Description("Laser Job Path")]
        public String CurrentJobPath
        {
            get { return GetPropValue(() => CurrentJobPath, LaserJobFilesRootPath+"Sample Lapping Job.sld"); }
            set { SetPropValue(() => CurrentJobPath, value); }
        }

        

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SLLaserDesk()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public SLLaserDesk(string name) 
            : base (name)
        {
        }
        #endregion Constructors



        /// <summary>
        /// Initialize this component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            _slTcp = new SLLaserDeskTcp();
            _slTcp.Bind = this;

            TCPComm = this.FilterByType<TCPIP>()[0];
            TCPComm.IPAddress = "127.0.0.1";
            TCPComm.Port = 3000;
            
            OpenLaserDesk();
            System.Threading.Thread.Sleep(10000);
            if (_slLaserDeskWrapperCtrl == null)
            {
                _slLaserDeskWrapperCtrl = new SLLaserDeskWrapperCtrl();
                _slLaserDeskWrapperCtrl.Bind = this;
            }
            System.Threading.Thread.Sleep(3000);
            InitialLaserDesk();
            AutoConfirmRemoteGUI();

            _statusPollingThread = new System.Threading.Thread(new System.Threading.ThreadStart(PollingQueryStatus));
            _statusPollingThread.Start();
        }

        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            
           
        }

       

        /// <summary>
        /// Destroy the object
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            SLTcp.DisconnectFromLaserDesk();
            KillExistingLaserDeskProcess();
            if(_statusPollingThread != null && _statusPollingThread.IsAlive)
            {
                _statusPollingThread.Abort();
            }
        }


        public void KillExistingLaserDeskProcess()
        {
            try
            {
                Process p = Process.GetProcessesByName("SLLaserDesk")[0];
                p.Kill();
                System.Threading.Thread.Sleep(500);
            }
            catch
            {

            }
        }


        public void OpenLaserDesk()
        {
            try
            {
                KillExistingLaserDeskProcess();
                ProcessStartInfo pInfo = new ProcessStartInfo(ApplicationPath);
                if (CurrentJobPath != "")
                {

                    if (!File.Exists(CurrentJobPath))
                    {
                        MessageBox.Show(String.Format("Could not found template path {0}", CurrentJobPath));
                    }
                    else
                    {
                        pInfo.FileName = CurrentJobPath;
                    }
                }

                if (SLLaserDeskProcesss != null)
                {
                    try
                    {
                        SLLaserDeskProcesss.CloseMainWindow();
                    }
                    catch
                    {
                        SLLaserDeskProcesss = null;
                        GC.Collect();
                    }
                }

                SLLaserDeskProcesss = new Process();
                SLLaserDeskProcesss.StartInfo.FileName = CurrentJobPath;
                SLLaserDeskProcesss.Start();
                SLLaserDeskProcesss.WaitForInputIdle();

            }
            catch (Exception ex)
            {
                try
                {
                    ProcessStartInfo pInfo = new ProcessStartInfo(ApplicationPath);
                    if (!File.Exists(CurrentJobPath))
                    {
                        MessageBox.Show(String.Format("Could not found template path {0}", CurrentJobPath));
                        return;
                    }


                    pInfo.FileName = CurrentJobPath;
                    if (SLLaserDeskProcesss != null)
                    {
                        try
                        {
                            SLLaserDeskProcesss.CloseMainWindow();
                        }
                        catch
                        {
                            SLLaserDeskProcesss = null;
                            GC.Collect();
                        }
                    }
                    SLLaserDeskProcesss = Process.Start(pInfo);
                    SLLaserDeskProcesss.WaitForInputIdle();
                }
                catch
                {

                }
            }
        }


        public override void RegisterApiPanel(string apiPanel,Panel parrent)
        {
            try
            {
                if (apiPanel == "MainApi")
                {
                    //SLLaserDeskUtility.ShowWindow(SLLaserDeskProcesss.MainWindowHandle, SLLaserDeskUtility.WindowShowStyle.Show);
                    //SLLaserDeskUtility.SetParent(SLLaserDeskProcesss.MainWindowHandle, parrent.Handle);
                    //SLLaserDeskUtility.SendMessage(SLLaserDeskProcesss.MainWindowHandle, SLLaserDeskUtility.WM_SYSCOMMAND, SLLaserDeskUtility.SC_MAXIMIZE, 0);


                    //if (_slLaserDeskWrapperCtrl == null)
                    //{
                    //    _slLaserDeskWrapperCtrl = new SLLaserDeskWrapperCtrl();
                    //    _slLaserDeskWrapperCtrl.Bind = this;
                    //}
                    //UnregisterApiPanel();
                    parrent.Controls.Add(_slLaserDeskWrapperCtrl);
                }
                else if(apiPanel == "StatusApi")
                {
                    SLStatusCtrl statusCtrl = new SLStatusCtrl();
                    statusCtrl.Bind = this;
                    parrent.Controls.Add(statusCtrl);
                }
            }
            catch
            {

            }

        }

        public override void UnregisterApiPanel(string apiPanel)
        {
            if (apiPanel == "MainApi")
            {
                //SLLaserDeskUtility.ShowWindow(SLLaserDeskProcesss.MainWindowHandle, SLLaserDeskUtility.WindowShowStyle.Hide);
                //SLLaserDeskUtility.SetParent(SLLaserDeskProcesss.MainWindowHandle,IntPtr.Zero);
                //SLLaserDeskUtility.SendMessage(SLLaserDeskProcesss.MainWindowHandle, SLLaserDeskUtility.WM_SYSCOMMAND, SLLaserDeskUtility.SC_MINIMIZE, 0);
                //HideWindowFromTaskbar(SLLaserDeskProcesss.MainWindowHandle);
                _slLaserDeskWrapperCtrl.Parent = null;
            }
        }

        private void HideWindowFromTaskbar(IntPtr pMainWindow)
        {
           
            //SLLaserDeskUtility.SetWindowLong(pMainWindow, SLLaserDeskUtility.GWL_EXSTYLE,~SLLaserDeskUtility.WS_EX_APPWINDOW);
            SLLaserDeskUtility.ShowWindow(pMainWindow, SLLaserDeskUtility.WindowShowStyle.Hide);
            //SLLaserDeskUtility.SetWindowLong(pMainWindow, SLLaserDeskUtility.GWL_EXSTYLE, SLLaserDeskUtility.GetWindowLong(pMainWindow, SLLaserDeskUtility.GWL_EXSTYLE) | SLLaserDeskUtility.WS_EX_TOOLWINDOW);
           // SLLaserDeskUtility.ShowWindow(pMainWindow, SLLaserDeskUtility.WindowShowStyle.Show);
        }


        public void InitialLaserDesk()
        {
            if (!SLTcp.IsConnected)
            {
                SLTcp.ConnectToLaserDesk(TCPComm.IPAddress,TCPComm.Port);
            }
            ////Login
            //List<Byte> loginParam = null;
            //loginParam = SLTcp.CovertStringParamToByteList("1990");
            //ExecuteCommand(1, loginParam);

            ////Remoteon
            //List<Byte> remoteOnParam = null;
            //ExecuteCommand(5, remoteOnParam);

            Login();
            RemoteOn();
        }

        public void AutoConfirmRemoteGUI()
        {
            System.Threading.Thread.Sleep(1000);
            //Auto Confirm Remote Dialog
            IntPtr hwndRemote = SLLaserDeskUtility.FindWindow(null, "Remote Control");
            SLLaserDeskUtility.PostMessage(hwndRemote, SLLaserDeskUtility.WM_KEYDOWN, SLLaserDeskUtility.VK_RETURN, 1);

            System.Threading.Thread.Sleep(1000);
            IntPtr hwndLogIn = SLLaserDeskUtility.FindWindow(null, "SCANLAB laserDESK");
            SLLaserDeskUtility.PostMessage(hwndLogIn, SLLaserDeskUtility.WM_KEYDOWN, SLLaserDeskUtility.VK_RETURN, 1);
        }

        public void DisconnectLaserDesk()
        {
            if (SLTcp.IsConnected)
            {
                ////LogOff
                //List<Byte> logOffParam = null;
                //logOffParam = null;          
                //ExecuteCommand(2, logOffParam);

                ////RemoteOff
                //List<Byte> remoteOffParam = null;
                //ExecuteCommand(6, remoteOffParam);
                //SLTcp.DisconnectFromLaserDesk();

                Logout();
                RemoteOff();
                SLTcp.DisconnectFromLaserDesk();

            }
        }

        public void RemoteOffLaserDesk()
        {
            if (SLTcp.IsConnected)
            {
                ////LogOff
                //List<Byte> logOffParam = null;
                //logOffParam = null;
                //ExecuteCommand(2, logOffParam);

                ////RemoteOff
                //List<Byte> remoteOffParam = null;
                //ExecuteCommand(6, remoteOffParam);

                Logout();
                RemoteOff();
            }
        }


        private void PollingQueryStatus()
        {
            while(!IsDestroying)
            {
                if (!IsDestroying)
                {
                    QueryStatus();
                    if (_slTcp.IsConnected)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
        }


        #region support Remote Operation
        public void QueryStatus()
        {
            if(!_contQueryStatus)
            {
                return;
            }

            int commandNum = 4;
            List<Byte> returnStatus = (List<Byte>)QueryCommand(commandNum, null);
            List<Byte> returnStatBytesList = GetParamsFromReturnQuery(commandNum, returnStatus);
            Int32 statusInt = BitConverter.ToInt32(returnStatBytesList.ToArray(), 0);



            RM_STATE_WND_OPEN = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_WND_OPEN) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_WND_OPEN;
            RM_STATE_RTC_INIT = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_RTC_INIT) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_RTC_INIT;
            RM_STATE_LAS_INIT = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LAS_INIT) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LAS_INIT;
            RM_STATE_MOT_INIT = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_MOT_INIT) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_MOT_INIT;
            RM_STATE_ALL_INIT = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_ALL_INIT) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_ALL_INIT;
            RM_STATE_READY = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_READY) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_READY;
            RM_STATE_AUTOMODE = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_AUTOMODE) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_AUTOMODE;
            RM_STATE_LST_EXEC = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LST_EXEC) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LST_EXEC;
            RM_STATE_LST_EXE_ERR = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LST_EXE_ERR) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LST_EXE_ERR;
            RM_STATE_RM_MODE = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_RM_MODE) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_RM_MODE;
            RM_STATE_JOB_LOAD = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_JOB_LOAD) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_JOB_LOAD;
            RM_STATE_LST_CALC = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LST_CALC) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LST_CALC;
            RM_STATE_CMD_ERR = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_CMD_ERR) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_CMD_ERR;
            RM_STATE_LAS_ERR = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LAS_ERR) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LAS_ERR;
            RM_STATE_LAS_ON = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LAS_ON) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_LAS_ON;
            RM_STATE_DEV_ERR = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_DEV_ERR) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_DEV_ERR;
            RM_STATE_HEAD_OK = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_HEAD_OK) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_HEAD_OK;
            RM_STATE_EXEC_DONE = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_EXEC_DONE) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_EXEC_DONE;
            RM_STATE_PILOT_MODE = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_PILOT_MODE) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_PILOT_MODE;
            RM_STATE_JOB_ABORTED = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_JOB_ABORTED) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_JOB_ABORTED;
            RM_STATE_SWITCH_AUTOMODE = (statusInt & (int)SLLaserDeskUtility.eStatusBit.RM_STATE_SWITCH_AUTOMODE) == (int)SLLaserDeskUtility.eStatusBit.RM_STATE_SWITCH_AUTOMODE;
        }


        public void OpenLaserJob(string laserJobPath)
        {
            try
            { 
                if (!File.Exists(laserJobPath))
                {
                    throw new FileNotFoundException(String.Format("Not found laser job file {0}", laserJobPath));
                }
            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup("SL Laser Desk [{0}] return from open job command {1}", this.Nickname, ex.ToString());
            }

            int commandNumber = 7;
            CurrentJobPath = laserJobPath;
            List<Byte> param = SLTcp.CovertStringParamToByteList(CurrentJobPath);

            //Overwrite File Int Param ( 0 = no over write)
            param.Add(0x00);
            param.Add(0x00);
            param.Add(0x00);
            param.Add(0x00);
            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] cannot open laser job [{1}]", this.Nickname, CurrentJobPath));
            }
        }


        public void SelectUID(string UID)
        {
            
            int commandNumber = 11;
            List<Byte> param = new List<byte>();
            //Param1 = 3 is Select by UID
            param.Add(0x03);
            param.Add(0x00);
            param.Add(0x00);
            param.Add(0x00);
            if (UID != null && UID !="")
            {
                param.AddRange(SLTcp.CovertStringParamToByteList(UID));
            }
            else
            {
                throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] no select UID specify on laser job [{1}]", this.Nickname, CurrentJobPath));
            }

            int retry = 3;
            Int32 paramsInt = 0;

            do
            {
                List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
                List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
                paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
                if (paramsInt == 0)
                {
                    //Throw Exception when max retry 
                    if (retry <= 0)
                    {
                        throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] cannot select UID [{1}] on laser job [{2}]", this.Nickname, UID, CurrentJobPath));
                    }
                    System.Threading.Thread.Sleep(100);
                }
            } while (paramsInt == 0 && retry > 0);

        }


        public void LaserStart()
        {
            int commandNumber = 12;
            List<Byte> param = null;
            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] cannot Start Laser on laser job [{1}]", this.Nickname, CurrentJobPath));
            }
        }


        public void SwitchLaserOn()
        {
            int commandNumber = 9;
            List<Byte> param = null;
            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] cannot Switch Laser ON on laser job [{1}]", this.Nickname, CurrentJobPath));
            }
        }


        public void SwitchLaserOff()
        {
            int commandNumber = 10;
            List<Byte> param = null;
            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] cannot Switch Laser OFF on laser job [{1}]", this.Nickname, CurrentJobPath));
            }
        }


        public void Login()
        {
            int commandNumber = 1;
            List<Byte> param = SLTcp.CovertStringParamToByteList("1990");

            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                //throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] login failed", this.Nickname));
            }
        }


        public void Logout()
        {
            int commandNumber = 2;
            List<Byte> param = null;

            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                //throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] logout failed", this.Nickname));
            }
        }


        public void RemoteOn()
        {
            int commandNumber = 5;
            List<Byte> param = null;

            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                //throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] remote on failed", this.Nickname));
            }
        }


        public void RemoteOff()
        {
            int commandNumber = 6;
            List<Byte> param = null;

            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                //throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] remote on failed", this.Nickname));
            }
        }


        public void ShowLaserDesk()
        {
            int commandNumber = 35;
            List<Byte> param = new List<byte>();
            param.Add(0x00);

            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                //throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] remote on failed", this.Nickname));
            }
        }


        public void HideLaserDesk()
        {
            int commandNumber = 35;
            List<Byte> param = new List<byte>();
            param.Add(0x01);

            List<Byte> returnQuery = (List<Byte>)QueryCommand(commandNumber, param);
            List<Byte> returnParams = GetParamsFromReturnQuery(commandNumber, returnQuery);
            Int32 paramsInt = BitConverter.ToInt32(returnParams.ToArray(), 0);
            if (paramsInt == 0)
            {
                //throw new MCoreExceptionPopup(string.Format("SL Laser Desk [{0}] remote on failed", this.Nickname));
            }
        }

        #endregion


        public override void ExecuteCommand(string command)
        {
            if (command.Contains("QueryStatus"))
            {
                QueryStatus();
            }
            else if (command.Contains("OpenLaserJob"))
            {
               string jobPath = command.Split(new char[] { ',' })[1];
               OpenLaserJob(jobPath);
            }
            else if (command.Contains("SelectUID"))
            {
                string uid = command.Split(new char[] { ',' })[1];
                SelectUID(uid);
            }

            else if (command.Contains("LaserStart"))
            {
                LaserStart();
            }

            else if (command.Contains("ShowLaserDesk"))
            {
                ShowLaserDesk();
            }
            else if (command.Contains("HideLaserDesk"))
            {
                HideLaserDesk();
            }
            else if (command.Contains("SwitchLaserON"))
            {
                SwitchLaserOn();
            }
            else if (command.Contains("SwitchLaserOFF"))
            {
                SwitchLaserOff();
            }
            else if (command.Contains("PauseQueryStatus"))
            {
                _contQueryStatus = false;
            }
            else if (command.Contains("ContQueryStatus"))
            {
                _contQueryStatus = true;
            }

        }




        private int _retryRead = 5;
        private int _retryReadCount = 0;

        public override void ExecuteCommand(object command, object parameter)
        {
            lock (_locker)
            {
                List<Byte> commandList = SLTcp.BuildCommand((int)command, (List<Byte>)parameter);
                bool updateMsg = (int)command != 4;
                SLTcp.SendTCPCommand(commandList,updateMsg);
                List<Byte> tcpReturn = null;

                _retryReadCount = _retryRead;
                while (true)
                {
                    tcpReturn = SLTcp.ReadTcpReturn(updateMsg);

                    if (tcpReturn == null)
                    {
                        _retryReadCount--;
                        if (_retryReadCount <= 0)
                        {
                            throw new MCoreExceptionPopup("SL Laser Desk [{0}] return from execute command is null", this.Nickname);
                        }
                        System.Threading.Thread.Sleep(500);
                        continue;
                    }

                    if ((int)command == 0x02 ||
                        (int)command == 0x03 ||
                        (int)command == 0x10)
                    {
                        if (tcpReturn[6] == (int)command)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (tcpReturn[5] == (int)command)
                        {
                            break;
                        }
                    }
                }
            }

        }




        public override object QueryCommand(object command, object parameter)
        {
            lock (_locker)
            {
                List<Byte> commandList = SLTcp.BuildCommand((int)command, (List<Byte>)parameter);
                bool updateMsg = (int)command != 4;
                SLTcp.SendTCPCommand(commandList, updateMsg);
                List<Byte> tcpReturn = null;

                _retryReadCount = _retryRead;
                while (true)
                {

                    tcpReturn = SLTcp.ReadTcpReturn(updateMsg);

                    if (tcpReturn == null)
                    {
                        _retryReadCount--;
                        if (_retryReadCount <= 0)
                        {
                            throw new MCoreExceptionPopup("SL Laser Desk [{0}] return from query command is null", this.Nickname);
                        }
                        System.Threading.Thread.Sleep(500);
                        continue;
                    }

                    if ((int)command == 0x02 ||
                        (int)command == 0x03 ||
                        (int)command == 0x10)
                    {
                        if (tcpReturn[6] == (int)command)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (tcpReturn[5] == (int)command)
                        {
                            break;
                        }
                    }
                }

                return tcpReturn;
            }
        }



        private List<Byte> GetParamsFromReturnQuery(int command, List<Byte> returnQuery)
        {
            Byte[] returnArray = new Byte[returnQuery.Count];
            returnQuery.CopyTo(returnArray);

            List<Byte> returnParams = returnArray.ToList();
            //Remove Header
            if (command == 0x02 || command == 0x03 || command == 0x10)
            {
                //remove fisrt 10 bytes
                returnParams.RemoveRange(0, 10);
            }
            else
            {
                //remove fisrt 9 bytes
                returnParams.RemoveRange(0, 9);
            }

            //remove ETX
            returnParams.RemoveAt(returnParams.Count - 1);

            //remove check sum
            Byte checksum = returnParams[returnParams.Count - 1];
            //Remove checksum and  DLE
            if (checksum == 0x02 || checksum == 0x03 || checksum == 0x10)
            {
                returnParams.RemoveRange(returnParams.Count - 2, 2);
            }
            else
            {
                returnParams.RemoveRange(returnParams.Count - 1, 1);
            }

            List<Byte> managedRetParam = new List<byte>();
            bool foundDLE = false;
            //Eleminate DLE
            foreach (Byte byt in returnParams)
            {
                if (byt == 0x10 && !foundDLE)
                {
                    foundDLE = true;
                    continue;
                }
                managedRetParam.Add(byt);
                foundDLE = false;
            }


            return managedRetParam;
        }

    }
}
