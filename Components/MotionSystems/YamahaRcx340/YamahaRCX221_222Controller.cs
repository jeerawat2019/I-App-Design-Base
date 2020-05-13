using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.IO.Ports;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;

namespace YamahaRCX221_222
{
    public enum EncoderType
    {
        Absolute,
        Incremental
    }

    public sealed class YamahaRCX221_222Controller
    {
        // Nested declarations -------------------------------------------------

        // Member variables ----------------------------------------------------
        private TcpClient _tcpClient;
        private SerialPort _serialPort;
        private YamahaRCX221_222Config _config;
        private string _lastReceivedData = "";

        // Constructors & Finalizers -------------------------------------------
        public YamahaRCX221_222Controller(string name, YamahaRCX221_222Config config)
        {
            _config = config;
            Name = "Yamaha RCX221/222 Controller";
        }

        // Properties ----------------------------------------------------------
        public string Name { get; private set; }
        public bool IsSimulation { get; private set; }
        public bool IsInitialized { get; private set; }
        public bool Ready { get; private set; }
        public bool Axis1Enabled { get; private set; }
        public bool Axis2Enabled { get; private set; }
        public bool Axis1Homed { get; private set; }
        public bool Axis2Homed { get; private set; }

        // Methods -------------------------------------------------------------
        public void Initialize(bool simulation)
        {
            lock (this)
            {
                try
                {
                    if (_config == null)
                        throw new Exception("Config is not assigned.");
                    if (IsInitialized) return;
                    IsSimulation = simulation;
                    if (IsSimulation)
                    {
                        IsInitialized = true;
                        return;
                    }

                    if (_config.CommunicationType == CommunicationType.TCP)
                    {
                        _tcpClient = new TcpClient();
                        _tcpClient.Connect(IPAddress.Parse(_config.IpAddress), _config.Port);
                    }
                    else
                    {
                        _serialPort = new SerialPort();
                        _serialPort.PortName = _config.SerialPortSetting.PortName;
                        _serialPort.BaudRate = (int)_config.SerialPortSetting.Baudrate;
                        _serialPort.DataBits = _config.SerialPortSetting.DataBits;
                        _serialPort.StopBits = (StopBits)_config.SerialPortSetting.StopBits;
                        _serialPort.Parity = (Parity)_config.SerialPortSetting.Parity;
                        _serialPort.Handshake = (Handshake)_config.SerialPortSetting.Handshake;

                        _serialPort.RtsEnable = true;
                        _serialPort.DtrEnable = true;

                        _serialPort.Open();
                        _serialPort.WriteTimeout = 1000;
                        _serialPort.DiscardInBuffer();
                    }

                    string ret = "";
                    SendCommand("STOP");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("STOP", ret);

                    SendCommand("MANUAL");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("MANUAL", ret);

                    SendCommand("EMGRST");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("EMGRST", ret);

                    //SendCommand("AUTO");
                    //ret = WaitForCommand(1000);
                    //CheckOKReplied("AUTO", ret);

                    //SendCommand("RESET");
                    //ret = WaitForCommand(1000);
                    //CheckOKReplied("RESET", ret);

                    //SendCommand("RUN");
                    //ret = WaitForCommand(1000);
                    //CheckOKReplied("RUN", ret);

                    IsInitialized = true;
                }
                catch (Exception ex)
                {
                    IsInitialized = false;
                    throw CreateEx("Initialize", null, ex);
                }
            }
        }

        public void Shutdown()
        {
            lock (this)
            {
                IsInitialized = false;
                if (IsSimulation)
                    return;
                try
                {
                    string ret = "";
                    SendCommand("STOP");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("STOP", ret);

                    SendCommand("MANUAL");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("MANUAL", ret);
                }
                catch (Exception ex)
                {
                    throw CreateEx("Shutdown", null, ex);
                }
                finally
                {
                    if (_config.CommunicationType == CommunicationType.TCP)
                    {
                        _tcpClient.Close();
                    }
                    else
                    {
                        _serialPort.Close();
                    }
                }
            }
        }

        public string DirectCommand(string command, int msTimeout)
        {
            lock (this)
            {
                try
                {
                    SendCommand(command);
                    return WaitForCommand(msTimeout);
                }
                catch (Exception ex)
                {
                    throw CreateEx("DirectCommand", null, ex);
                }
            }
        }

        public void ClearAlarm()
        {
            lock (this)
            {
                try
                {
                    string ret = "";
                    SendCommand("STOP");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("STOP", ret);

                    SendCommand("MANUAL");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("MANUAL", ret);

                    SendCommand("EMGRST");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("EMGRST", ret);

                    SendCommand("AUTO");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("AUTO", ret);

                    SendCommand("RESET");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("RESET", ret);

                    SendCommand("RUN");
                    ret = WaitForCommand(1000);
                    CheckOKReplied("RUN", ret);
                }
                catch (Exception ex)
                {
                    throw CreateEx("ClearAlarm", null, ex);
                }
            }
        }

        public void Enable(int axisNumber, bool isEnable)
        {
            lock (this)
            {
                string ret = "";
                try
                {
                    if (isEnable)
                    {
                        SendCommand(string.Format("SERVO ON({0})", axisNumber));
                        ret = WaitForCommand(1000);
                        CheckOKReplied("Enable", ret);
                        if (axisNumber == 1)
                            this.Axis1Enabled = true;
                        else
                            this.Axis2Enabled = true;
                    }
                    else
                    {
                        SendCommand(string.Format("SERVO FREE({0})", axisNumber));
                        ret = WaitForCommand(1000);
                        CheckOKReplied("Enable", ret);
                        if (axisNumber == 1)
                            this.Axis1Enabled = false;
                        else
                            this.Axis2Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    throw CreateEx("Enable", null, ex);
                }
            }
        }

        public double GetCurrentPosition(int axisNumber)
        {
            lock (this)
            {
                string ret = "";
                try
                {
                    if (IsSimulation)
                        return 0.0;
                    SendCommand(string.Format("?WHRXY"));  //WHRXY
                    ret = WaitForCommand(1000);
                    if (ret.Contains("[POS]") == false)
                    {
                        throw CreateEx("GetCurrentPosition", ret, null);
                    }
                    else
                    {
                        string[] d = ret.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        return Convert.ToDouble(d[axisNumber]);
                    }
                }
                catch (Exception ex)
                {
                    throw CreateEx("GetCurrentPosition", null, ex);
                }
            }
        }

        public void HomeStart(int axisNumber)
        {
            lock (this)
            {
                try
                {
                    if (axisNumber == 1)
                        this.Axis1Homed = false;
                    else
                        this.Axis2Homed = false;

                    if (axisNumber == 1)
                    {
                        if (_config.Axis1EncoderType == EncoderType.Incremental)
                            SendCommand(string.Format("ORGORD({0})", axisNumber));
                        else
                            SendCommand(string.Format("ABSRST"));
                    }
                    else
                    {
                        if (_config.Axis2EncoderType == EncoderType.Incremental)
                            SendCommand(string.Format("ORGORD({0})", axisNumber));
                        else
                            SendCommand(string.Format("ABSRST"));
                    }
                }
                catch (Exception ex)
                {
                    throw CreateEx("HomeStart", null, ex);
                }
            }
        }

        public void Home(int axisNumber, int msTimeout)
        {
            lock (this)
            {
                HomeStart(axisNumber);
                WaitHomeDone(axisNumber, msTimeout);
            }
        }

        public void WaitHomeDone(int axisNumber, int msTimeout)
        {
            lock (this)
            {
                string ret = "";
                try
                {
                    ret = WaitForCommand(msTimeout);
                    CheckOKReplied("WaitHomeDone", ret);
                    if (axisNumber == 1)
                        this.Axis1Homed = true;
                    else
                        this.Axis2Homed = true;
                }
                catch (Exception ex)
                {
                    throw CreateEx("WaitHomeDone", null, ex);
                }
            }
        }

        public void Stop(int axisNumber)
        {
            throw new NotImplementedException();
        }

        public void SetAcc(int axisNumber, double acc)
        {
            lock (this)
            {
                string ret = "";
                try
                {
                    double percent;
                    if (axisNumber == 1)
                    {
                        percent = (acc / _config.Axis1DefaultAcc) * 100.00;
                    }
                    else
                    {
                        percent = (acc / _config.Axis2DefaultAcc) * 100.00;
                    }
                    SendCommand(string.Format("ACCEL({0})={1:F2}", axisNumber, percent));
                    ret = WaitForCommand(1000);
                    CheckOKReplied("SetAcc", ret);
                }
                catch (Exception ex)
                {
                    throw CreateEx("SetAcc", null, ex);
                }
            }
        }

        public void SetDec(int axisNumber, double dec)
        {
            lock (this)
            {
                string ret = "";
                try
                {
                    double percent;
                    if (axisNumber == 1)
                    {
                        percent = (dec / _config.Axis1DefaultDec) * 100.00;
                    }
                    else
                    {
                        percent = (dec / _config.Axis2DefaultDec) * 100.00;
                    }
                    SendCommand(string.Format("DECEL({0})={1:F2}", axisNumber, percent));
                    ret = WaitForCommand(1000);
                    CheckOKReplied("SetDec", ret);
                }
                catch (Exception ex)
                {
                    throw CreateEx("SetDec", null, ex);
                }
            }
        }

        public void MoveAbsStart(int axisNumber, double position, double speed)
        {
            lock (this)
            {
                try
                {
                    double percent;
                    if (axisNumber == 1)
                    {
                        percent = (speed / _config.Axis1DefaultSpeed) * 100.00;
                    }
                    else
                    {
                        percent = (speed / _config.Axis2DefaultSpeed) * 100.00;
                    }
                    SendCommand(string.Format("DRIVE({0},{1:F2}),S={2:F2}", axisNumber, position, percent));
                }
                catch (Exception ex)
                {
                    throw CreateEx("MoveAbsStart", null, ex);
                }
            }
        }

        public void MoveAbs(int axisNumber, double position, double speed, int msTimeout)
        {
            lock (this)
            {
                MoveAbsStart(axisNumber, position, speed);
                WaitMoveDone(axisNumber, msTimeout);
            }
        }

        public void MoveRelStart(int axisNumber, double position, double speed)
        {
            lock (this)
            {
                try
                {
                    double percent;
                    if (axisNumber == 1)
                    {
                        percent = (speed / _config.Axis1DefaultSpeed) * 100.00;
                    }
                    else
                    {
                        percent = (speed / _config.Axis2DefaultSpeed) * 100.00;
                    }
                    SendCommand(string.Format("DRIVEI({0},{1:F2}),S={2:F2}", axisNumber, position, percent));
                }
                catch (Exception ex)
                {
                    throw CreateEx("MoveRelStart", null, ex);
                }
            }
        }

        public void MoveRel(int axisNumber, double position, double speed, int msTimeout)
        {
            lock (this)
            {
                MoveRelStart(axisNumber, position, speed);
                WaitMoveDone(axisNumber, msTimeout);
            }
        }

        public void WaitMoveDone(int axisNumber, int msTimeout)
        {
            lock (this)
            {
                string ret = "";
                try
                {
                    ret = WaitForCommand(msTimeout);
                    CheckOKReplied("WaitMoveDone", ret);
                }
                catch (Exception ex)
                {
                    throw CreateEx("WaitMoveDone", null, ex);
                }
            }
        }

        public void VectorMoveAbsStart(double firstAxisPosition, double secondAxisPosition, double speed)
        {
            lock (this)
            {
                try
                {
                    double percent;
                    percent = (speed / _config.AxesDefaultVectorSpeed) * 100.00;
                    SendCommand(string.Format("DRIVE({0},{1:F2}),({2},{3:F2}),S={4:F2}", 1, firstAxisPosition, 2, secondAxisPosition, percent));
                }
                catch (Exception ex)
                {
                    throw CreateEx("VectorMoveAbsStart", null, ex);
                }
            }
        }

        public void VectorMoveAbs(double firstAxisPosition, double secondAxisPosition, double speed, int msTimeout)
        {
            lock (this)
            {
                VectorMoveAbsStart(firstAxisPosition, secondAxisPosition, speed);
                WaitVectorMoveDone(msTimeout);
            }
        }

        public void WaitVectorMoveDone(int msTimeout)
        {
            lock (this)
            {
                string ret = "";
                try
                {
                    ret = WaitForCommand(msTimeout);
                    CheckOKReplied("WaitVectorMoveDone", ret);
                }
                catch (Exception ex)
                {
                    throw CreateEx("WaitVectorMoveDone", null, ex);
                }
            }
        }

        public void MoveAbsAndStopOnIO(int axisNumber, double position, int ioId, int msTimeout)
        {
            lock (this)
            {
                try
                {
                    SendCommand(string.Format("DRIVE({0},{1:F2}),STOPON DI({2})=1", axisNumber, position, ioId));
                    WaitMoveDone(axisNumber, msTimeout);
                }
                catch (Exception ex)
                {
                    throw CreateEx("MoveAbsAndStopOnIO", null, ex);
                }
            }
        }

        // Internal methods ----------------------------------------------------      
        private void SendCommand(string command)
        {
            lock (this)
            {
                if (IsSimulation)
                    return;
                if (_config.CommunicationType == CommunicationType.TCP)
                {
                    string fcmd = "@" + command + "\r\n";
                    _tcpClient.Client.Send(Encoding.ASCII.GetBytes(fcmd));
                }
                else
                {
                    _serialPort.DiscardInBuffer();
                    _serialPort.DiscardOutBuffer();
                    _serialPort.NewLine = "\r\n";
                    _serialPort.WriteLine("@" + command);
                }
            }
        }

        private string WaitForCommand(int msTimeout)
        {
            string rcv = "";
            if (IsSimulation)
                return "OK";
            if (_config.CommunicationType == CommunicationType.TCP)
            {
                byte[] rcvBuf = new byte[1024];
                int rcvSize = 0;

                _tcpClient.ReceiveTimeout = (int)msTimeout;
                rcvSize = _tcpClient.Client.Receive(rcvBuf);
                byte[] tmp = new byte[rcvSize];
                Array.Copy(rcvBuf, tmp, rcvSize);
                rcv = Encoding.ASCII.GetString(tmp);
            }
            else
            {

                _serialPort.ReadTimeout = (int)msTimeout;
                rcv = _serialPort.ReadLine();
            }

            rcv = rcv.Replace("\r", "");
            rcv = rcv.Replace("\n", "");
            _lastReceivedData = rcv;
            return _lastReceivedData;
        }

        private void CheckOKReplied(string action, string returned)
        {
            if (returned != "OK")
            {
                throw CreateEx(action, returned, null);
            }
        }

        private Exception CreateEx(string actionName, string errorCode, Exception innerException)
        {
            string msg;
            msg = Name + " error. " + actionName + " command failed.";
            if (errorCode != null)
            {
                msg = msg + " Error Code: " + errorCode + ".";
            }
            Exception ex;
            if (innerException != null)
                ex = new Exception(msg, innerException);
            else
                ex = new Exception(msg);
            return ex;
        }

        // Event handlers ------------------------------------------------------


    }
}
