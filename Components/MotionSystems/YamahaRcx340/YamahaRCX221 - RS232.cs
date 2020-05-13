using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml.Serialization;

using MPT.USN.CommonLib;
using MPT.USN.CommonLib.Comp;
using MPT.USN.CommonLib.Comp.MotionSystem;
using MPT.USN.CommonLib.Data;
#region SSafe History
/*
 * $History: $
 * 
 */
#endregion

namespace MPT.USN.CommonLib.Comp.MotionSystem.YamahaRCX221
{
    /// <summary>
    /// YamahaRCX221_222 driver interface
    /// </summary>
    public class YamahaRCX221_222 : MotionSystem
    {
        /// <summary>
        /// class objects to store the port settings
        /// </summary>
        public RS232 portSettings;

        #region Persistent Data properties

        /// <summary>
        /// The read timeout
        /// </summary>
        public DataDouble ddReadTimeout = new DataDouble("Read Timeout", 500, Enums.UnitTypes.msec);

        [XmlIgnore]
        private DataDouble ddPosUpdate = new DataDouble("SystemPos");

        [XmlIgnore]
        public DataString dsResponse = new DataString("Response");

        /// <summary>
        /// The retry count
        /// </summary>
        public DataInt _diRetryCount = new DataInt("Retry Count", 1);
        /// <summary>
        ///  Default speed used to calcultate percentage speed
        /// </summary>
        public DataDouble ddDefaultVectorSpeed = new DataDouble("Default Vector Speed", 100.0, MPT.USN.CommonLib.Enums.UnitTypes.mm, MPT.USN.CommonLib.Enums.RateType.velocity, MPT.USN.CommonLib.Enums.UnitTypes.sec);

        #endregion
        /// <summary>
		/// Default constructor for xml streaming
		/// </summary>
		public YamahaRCX221_222() 
		{
			// Keep empty, xml will stream data into class
		}

		/// <summary>
		/// Constructor used for first-time construction
		/// </summary>
		/// <param name="name"></param>
        public YamahaRCX221_222(string name)
            : base(name)
		{
            // Only used to create 1st xml data file
            portSettings = new RS232(1, 9600);
		}

        /// <summary>
		/// Initialize this component
		/// </summary>
		/// <returns></returns>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            try
            {
                if (portSettings == null)
                {
                    portSettings = new RS232(1, 9600);
                }
                if (!string.IsNullOrEmpty(dsIPAddress.Val))
                {
                    portSettings.diCommPort.Val = System.Convert.ToInt32(dsIPAddress.Val);
                    dsIPAddress.Val = string.Empty;
                }

                portSettings.ddReadWriteTimeOut.Val = System.Convert.ToInt32(U.ConvertFromInternal(Enums.UnitTypes.msec, ddReadTimeout.Val));
                portSettings.Initialize();

                string ret = string.Empty;
                SendCommand("STOP");
                ret = WaitForCommand();
                CheckOKReplied("STOP", ret);

                SendCommand("MANUAL");
                ret = WaitForCommand();
                CheckOKReplied("MANUAL", ret);

                SendCommand("EMGRST");
                ret = WaitForCommand();
                CheckOKReplied("EMGRST", ret);            
            }
            catch (Exception ex)
            {
                portSettings.Destroy();
                throw ex;
            }
        }
        
        /// <summary>
        /// Opportunity to cleanup
        /// </summary>
        protected override void OnDestroy()
        {
            if (portSettings.IsOpen)
            {
                string ret = "";
                SendCommand("STOP");
                ret = WaitForCommand();
                CheckOKReplied("STOP", ret);

                SendCommand("MANUAL");
                ret = WaitForCommand();
                CheckOKReplied("MANUAL", ret);
            }

            portSettings.Destroy();
            base.OnDestroy();
        }


        /// <summary>
        /// Add a settings page to this class
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="bSkip"></param>
        public override void AddPages(TabControl tab, bool bSkip)
        {
            base.AddPages(tab, false);
            if (!bSkip) 
            {
                AddPage(tab, typeof(YamahaRCX221_222Page), "Config");
            }
        }

        /// <summary>
        /// Add any events for simulation
        /// </summary>
        protected override void OnEventsForComponent(ComponentBase comp)
        {
            base.OnEventsForComponent(comp);
            if (comp == this)
            {
                evMoveSingleAxisRel += new DelegateParmSMAxisBaseDoubleDouble(OnMoveSingleAxisRel);
                evMoveSingleAxisAbs += new DelegateParmSMAxisBaseDoubleDouble(OnMoveSingleAxisAbs);
                evMoveContoured += new DelegateParmSMAxesDoublesDouble(OnMoveContoured);
                evHome += new DelegateParmSMAxisBase(OnHome);
                evEnableAxis += new DelegateParmSMAxisBaseBool(OnEnableAxis);
                evSetAccel += new DelegateParmSMAxisBaseDouble(OnSetAccel);
                evSetDecel += new DelegateParmSMAxisBaseDouble(OnSetDecel);
                evSendCommand += new DelegateParmSMString(OnSendCommand);
                evReset += new DelegateParmSMMethodCall(OnReset);

                ddPosUpdate.UpdatePeriod = 10000000;  // 100 msec
                OnUpdateEvents.Add(ddPosUpdate, new DelegateParmVoid(OnUpdateCurrentPositions));
            }
        }

        private void OnSendCommand(SMMethodCall mc, string strCmd)
        {
            SendCommand(strCmd);
            dsResponse.Val = WaitForCommand();
            mc.End();
        }
        /// <summary>
        /// Same as Clear Alarm
        /// </summary>
        /// <param name="mc"></param>
        private void OnReset(SMMethodCall mc)
        {
            string ret = "";
            SendCommand("STOP");
            ret = WaitForCommand();
            CheckOKReplied("STOP", ret);

            SendCommand("MANUAL");
            ret = WaitForCommand();
            CheckOKReplied("MANUAL", ret);

            SendCommand("EMGRST");
            ret = WaitForCommand();
            CheckOKReplied("EMGRST", ret);

            SendCommand("AUTO");
            ret = WaitForCommand();
            CheckOKReplied("AUTO", ret);

            SendCommand("RESET");
            ret = WaitForCommand();
            CheckOKReplied("RESET", ret);

            SendCommand("RUN");
            ret = WaitForCommand();
            CheckOKReplied("RUN", ret);

            mc.End();
        }

        private void OnUpdateCurrentPositions()
        {
            SendCommand(string.Format("?WHRXY"));  //WHRXY
            string ret = WaitForCommand();
            if (ret.Contains("[POS]"))
            {
                string[] d = ret.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i] is Axis.Axis)
                    {
                        Axis.Axis axis = this[i] as Axis.Axis;
                        int axisNumber = axis.Id + 1;
                        if (axisNumber < d.Length)
                        {
                            axis.CurrentPos = U.ConvertToInternal(Enums.UnitTypes.mm, System.Convert.ToDouble(d[axisNumber]));
                        }
                    }
                }
            }
        }

        private void OnHome(SMMethodCall mc, Axis.AxisBase axis)
        {
            // If Incremental 
                //SendCommand(string.Format("ORGORD({0})", axisNumber));
            
            SendCommand(string.Format("ABSRST"));
            string ret = WaitForCommand();
            CheckOKReplied("WaitHomeDone", ret);
            mc.End();
        }

        private void OnEnableAxis(SMMethodCall mc, Axis.AxisBase axis, bool bEnable)
        {
            string ret = string.Empty;
            int axisNumber = axis.Id + 1;
            if (bEnable)
            {
                SendCommand(string.Format("SERVO ON({0})", axisNumber));
                ret = WaitForCommand();
                CheckOKReplied("Enable", ret);
            }
            else
            {
                SendCommand(string.Format("SERVO FREE({0})", axisNumber));
                ret = WaitForCommand();
                CheckOKReplied("Enable", ret);
            }
            axis.Enabled = bEnable;
            mc.End();
        }

        private void OnSetAccel(SMMethodCall mc, Axis.AxisBase axis, double accel)
        {
            int axisNumber = axis.Id + 1;
            double percent = GetPercentage(axis, "Default Accel", accel);
            SendCommand(string.Format("ACCEL({0})={1:F2}", axisNumber, percent));
            string ret = WaitForCommand();
            CheckOKReplied("SetAcc", ret);                
            mc.End();
        }

        private void OnSetDecel(SMMethodCall mc, Axis.AxisBase axis, double decel)
        {
            int axisNumber = axis.Id + 1;
            double percent = GetPercentage(axis, "Default Decel", decel);
            SendCommand(string.Format("DECEL({0})={1:F2}", axisNumber, percent));
            string ret = WaitForCommand();
            CheckOKReplied("SetDec", ret);
            mc.End();
        }

        private double GetPercentage(ComponentBase comp, string dataName, double val)
        {
            double defaultVal = 100.0;
            DataDouble ddDefault = comp.mclData.Find(dataName) as DataDouble;
            if (ddDefault != null)
            {
                defaultVal = ddDefault.Val;
            }

            try
            {
                return val / defaultVal * 100.0;
            }
            catch
            {
                return 10.0;
            }
        }

        // Single Axis move commands
        /// <summary>
        /// Move single axis in absolute
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="axis"></param>
        /// <param name="dPos"></param>
        /// <param name="dSpeed"></param>
        private void OnMoveSingleAxisAbs(SMMethodCall mc, Axis.AxisBase axis, double dPos, double dSpeed)
        {
            int axisNumber = axis.Id + 1;
            double percent = GetPercentage(axis, "Default Speed", dSpeed);
            dPos = U.ConvertFromInternal(Enums.UnitTypes.mm, dPos);
            SendCommand(string.Format("DRIVE({0},{1:F2}),S={2:F2}", axisNumber, dPos, percent));
            string ret = WaitForCommand();
            CheckOKReplied("WaitMoveDone", ret);
            mc.End();
        }
        // Single Axis move commands
        /// <summary>
        /// Move single axis in Relative
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="axis"></param>
        /// <param name="dPos"></param>
        /// <param name="dSpeed"></param>
        private void OnMoveSingleAxisRel(SMMethodCall mc, Axis.AxisBase axis, double dPos, double dSpeed)
        {
            int axisNumber = axis.Id + 1;
            double percent = GetPercentage(axis, "Default Speed", dSpeed);
            dPos = U.ConvertFromInternal(Enums.UnitTypes.mm, dPos);
            SendCommand(string.Format("DRIVEI({0},{1:F2}),S={2:F2}", axisNumber, dPos, percent));
            string ret = WaitForCommand();
            CheckOKReplied("WaitMoveDone", ret);
            mc.End();
        }

        /// <summary>
        /// Make Contoured move
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="axes"></param>
        /// <param name="positions"></param>
        /// <param name="speed"></param>
        private void OnMoveContoured(SMMethodCall mc, Axis.Axes axes, double[] positions, double speed)
        {

            double percent = GetPercentage(this, "Default Vector Speed", speed);
            double pos1 = U.ConvertFromInternal(Enums.UnitTypes.mm, positions[0]);
            double pos2 = U.ConvertFromInternal(Enums.UnitTypes.mm, positions[1]);
            SendCommand(string.Format("DRIVE({0},{1:F2}),({2},{3:F2}),S={4:F2}", 1, pos1, 2, pos2, percent));
            string ret = WaitForCommand();
            CheckOKReplied("WaitVectorMoveDone", ret);
            mc.End();
        }

        /// <summary>
        /// Send a user-defined command
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(string command)
        {
            //if (_config.CommunicationType == CommunicationType.TCP)
            //{
            //    string fcmd = "@" + command + "\r\n";
            //    _tcpClient.Client.Send(Encoding.ASCII.GetBytes(fcmd));
            //}
            //else
            {
                SerialPort port = portSettings.Port;
                port.DiscardInBuffer();
                port.DiscardOutBuffer();
                port.NewLine = "\r\n";
                port.WriteLine("@" + command);
            }
        }
        private string WaitForCommand()
        {
            string rcv = string.Empty;
            //if (_config.CommunicationType == CommunicationType.TCP)
            //{
            //    byte[] rcvBuf = new byte[1024];
            //    int rcvSize = 0;

            //    _tcpClient.ReceiveTimeout = (int)msTimeout;
            //    rcvSize = _tcpClient.Client.Receive(rcvBuf);
            //    byte[] tmp = new byte[rcvSize];
            //    Array.Copy(rcvBuf, tmp, rcvSize);
            //    rcv = Encoding.ASCII.GetString(tmp);
            //}
            //else
            {

                portSettings.Port.ReadTimeout = (int)U.ConvertFromInternal(Enums.UnitTypes.msec, ddReadTimeout.Val);
                rcv = portSettings.Port.ReadLine();
            }

            return rcv.Trim();
        }
        private void CheckOKReplied(string action, string returned)
        {
            if (returned != "OK")
            {
                throw new MCLExceptionErrorAlert(action, string.Format("Error in command for {0}.  String returned= '{1}'", returned));
            }
        }

    }
}
