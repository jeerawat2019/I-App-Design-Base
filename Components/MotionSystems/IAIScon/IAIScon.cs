using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Threading;
using System.IO.Ports;
using System.ComponentModel;
using System.Diagnostics;

using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using MDouble;
using EasyModbus;

namespace MCore.Comp.MotionSystem
{
    /// <summary>
    /// IAI SCON Driver Interface
    /// </summary>
    public class IAIScon:MotionSystemBase
    {
        /// <summary>
        /// class objects to store the port settings
        /// </summary>
        private BackgroundWorker _updateLoop = null;
        private List<RealAxis> _listRealAxis = null;
        private volatile bool _destroy = false;
        private ManualResetEvent _waitDataRecived = new ManualResetEvent(true);
        private ModbusClient _modbusClient = null;


        /// <summary>
        /// class objects to store the port settings
        /// </summary>
        [Category("Communication Port")]
        public IAIRS232Settings PortSetting
        {
            get { return GetPropValue(() => PortSetting); }
            set { SetPropValue(() => PortSetting, value); }
        }

        [Category("Communication Port")]
        public Miliseconds ReadTimeOut
        {
            get { return GetPropValue(() => ReadTimeOut, 1000); }
            set { SetPropValue(() => ReadTimeOut, value); }

        }


        [Category("Motion")]
        public bool IsMoveSFTY
        {
            get { return GetPropValue(() => IsMoveSFTY, true); }
            set { SetPropValue(() => IsMoveSFTY, value); }

        }


          /// <summary>
        /// Default constructor for xml streaming
        /// </summary>
        public IAIScon()
        {
            // Keep empty, xml will stream data into class
        }

        /// <summary>
        /// Constructor used for first-time construction
        /// </summary>
        /// <param name="name"></param>
        public IAIScon(string name)
            : base(name)
        {
            // Only used to create 1st xml data file
            
        }

        public override void Initialize()
        {
            base.Initialize();
            try
            {
    
                _modbusClient = new ModbusClient(PortSetting.CommPort.ToString());
                _modbusClient.Baudrate = (int)PortSetting.BaudRate;
                _modbusClient.StopBits = PortSetting.StopBits;
                _modbusClient.Parity = PortSetting.Parity;
                _modbusClient.ConnectionTimeout = (int)PortSetting.ReadWriteTimeOut;
                _modbusClient.Available((int)PortSetting.ReadWriteTimeOut);

                _modbusClient.Connect();
              
                if (!_modbusClient.Connected)
                {
                    throw new ForceSimulateException(string.Format("Port {0} is not opened",PortSetting.CommPort.ToString()));
                }

                CheckStatus(2000);

                U.RegisterOnChanged(() => IsMoveSFTY, IsMoveSFTY_OnChanged);

                Simulate = eSimulate.None;
            }
            catch (Exception ex)
            {
                _modbusClient.Disconnect();
                //throw new ForceSimulateException(ex);
                U.LogAlarmPopup(ex,this.Nickname);
                this.Simulate = eSimulate.SimulateDontAsk;
            }

             _updateLoop = new BackgroundWorker();
             _updateLoop.DoWork += new DoWorkEventHandler(UpdateLoop);
           
        }

        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            _listRealAxis = this.RecursiveFilterByType<RealAxis>().ToList();

            foreach (RealAxis realAxis in _listRealAxis)
            {
                //Do some Axis Initialize Task
            }

            _modbusClient.receiveDataChanged += new ModbusClient.ReceiveDataChanged(OnDataReceived);
             ClearAllFaults();

             //if (this.Simulate == eSimulate.None)
             //{
             //    _updateLoop.RunWorkerAsync();
             //}
        }

        /// <summary>
        /// Destroy
        /// </summary>
        public override void PreDestroy()
        {
            _destroy = true;
            base.PreDestroy();
        }

        /// <summary>
        /// Opportunity to cleanup
        /// </summary>
        public override void Destroy()
        {

            if (_modbusClient == null)
                return;

            if (_modbusClient != null && _modbusClient.Connected)
            {
            }
            _modbusClient.Disconnect();
            base.Destroy();
        }

       


        private void UpdateLoop(object sender, DoWorkEventArgs e)
        {
            do
            {
                try
                {
                    OnUpdateCurrentPositions();
                    Thread.Sleep(100);
                }
                catch(Exception ex)
                {
                    U.Log(ex);
                    return;
                }
            } while (!_destroy);
        }


        public void CheckStatus(int timeout)
        {
            Stopwatch timeoutStopw = new Stopwatch();
            timeoutStopw.Start();
            while (true)
            {
                try
                {
                    lock (this)
                    {

                        int[] regStatus = _modbusClient.ReadHoldingRegisters(IAIConstAddr.DSS1, 1);
                        
                        if (regStatus != null)
                        {

                            break;
                        }

                        if (timeoutStopw.ElapsedMilliseconds > timeout)
                        {
                            //U.LogAlarmPopup("IAI SCON [{0}] Check Status timeout waiting for Origin State signal to be ON.", this.Nickname);
                            throw new MCoreExceptionPopup("IAI SCON [{0}] Check Status timeout waiting for Origin State signal to be ON.", this.Nickname);
                        }

                    }
                }
                catch (Exception ex)
                {
                    //U.LogAlarmPopup(ex, "CheckStatus Fail {0}", this.Nickname);
                    throw new MCoreExceptionPopup(ex, "CheckStatus Fail {0}", this.Nickname);
                }

                Thread.Sleep(100);
            }
        }



        private int _retryUpdate = 10;
        private void OnUpdateCurrentPositions()
        {
            _retryUpdate = 10;
            bool readSuccess = false;
            do
            {
                // We assume only one axis
                RealAxis axis = _listRealAxis[0] as RealAxis;
                if (axis != null)
                {

                    int lenght = 2;
                    try
                    {
                        lock (this)
                        {
                            Int32[] values = _modbusClient.ReadHoldingRegisters(IAIConstAddr.PNOW, lenght);

                            string sHighBytePos = ((Int16)values[0]).ToString("X4");
                            string sLowBytePos = ((Int16)values[1]).ToString("X4");
                            string sPos = sHighBytePos + sLowBytePos;

                            Int32 iPos = Int32.Parse(sPos, System.Globalization.NumberStyles.AllowHexSpecifier);


                            double factor = 0.01;
                            double dPos = (double)iPos * factor;
                            axis.SetCurrentPosition(dPos);
                            readSuccess = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        _retryUpdate--;
                        readSuccess = false;
                        System.Threading.Thread.Sleep(100);
                        //U.LogAlarmPopup(ex, "Error for set current position '{0}'", this.Nickname);
                        if (_retryUpdate <= 0)
                        {
                            throw new MCoreExceptionPopup(ex, "Error for set current position '{0}'", this.Nickname);
                        }
                    }

                }
            } while (!readSuccess && _retryUpdate > 0);
        }

        [StateMachineEnabled]
        public override void Reset()
        {
            base.Reset();

            try
            {
                lock (this)
                {
                    _waitDataRecived.Reset();
                    System.Threading.Thread.Sleep(500);
                    _modbusClient.WriteSingleCoil(IAIConstAddr.PMSL, true);
                    WaitDataReceived(ReadTimeOut);

                    _waitDataRecived.Reset();
                    _modbusClient.WriteSingleCoil(IAIConstAddr.ALRS, true);
                    WaitDataReceived(ReadTimeOut);
                   

                    _waitDataRecived.Reset();
                    _modbusClient.WriteSingleCoil(IAIConstAddr.ALRS, false);
                    WaitDataReceived(ReadTimeOut);

                    OnUpdateCurrentPositions();
                }

            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "Reset Command Fail '{0}'", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Reset Command Fail '{0}'", this.Nickname);
            }

        }

        public override void HomeAxis(RealAxis axis)
        {
            int axisNumber = axis.AxisNo + 1;
            try
            {
                lock (this)
                {
                    _waitDataRecived.Reset();
                    _modbusClient.WriteSingleCoil(IAIConstAddr.HOME, false);
                    WaitDataReceived(ReadTimeOut);
                  

                    _waitDataRecived.Reset();
                    _modbusClient.WriteSingleCoil(IAIConstAddr.HOME, true);
                    WaitDataReceived(ReadTimeOut);
                 
                }

            }
            catch (Exception ex)
            {
                axis.Homed = false;
                //U.LogAlarmPopup(ex, "Origin command failed {0}", this.Nickname);
                throw new MCoreExceptionPopup(ex, "origin command failed {0}", this.Nickname);
            }

            WaitForHomeDone(20000);
            axis.Homed = true;
            System.Threading.Thread.Sleep(100);
            lock (this)
            {
                OnUpdateCurrentPositions();
            }

        }
    

        public void WaitForHomeDone(int timeout)
        {
            Stopwatch timeoutStopw = new Stopwatch();
            timeoutStopw.Start();
            while (true)
            {
                try
                {
                    lock (this)
                    {
                        try
                        {
                            int[] regStatus = _modbusClient.ReadHoldingRegisters(IAIConstAddr.DSS1, 1);
                            int HEND = regStatus[0] & 0x0010;
                            if (HEND > 0)
                            {

                                break;
                            }
                        }
                        catch
                        {

                        }
                        finally
                        {

                        }

                        if (timeoutStopw.ElapsedMilliseconds > timeout)
                        {
                            //U.LogAlarmPopup("IAI SCON [{0}] Home timeout waiting for Origin State signal to be ON.", this.Nickname);
                            throw new MCoreExceptionPopup("IAI SCON [{0}] Home timeout waiting for Origin State signal to be ON.", this.Nickname);
                        }

                    }
                }
                catch (Exception ex)
                {
                    //U.LogAlarmPopup(ex, "Wait Home Done Fail {0}", this.Nickname);
                    throw new MCoreExceptionPopup(ex, "Wait Home Done Fail {0}", this.Nickname);
                }

                Thread.Sleep(100);
            }
        }


        private void IsMoveSFTY_OnChanged(bool isMoveSFTY)
        {
            _waitDataRecived.Reset();
            _modbusClient.WriteSingleCoil(IAIConstAddr.SFTY, isMoveSFTY);
            WaitDataReceived(ReadTimeOut);
        }
		
		
        public override void EnableAxis(RealAxis axis, bool bEnable)
        {
            bool servoOnOff = true;
            if (bEnable == true)
            {
                servoOnOff = true;
            }
            else
            {
                servoOnOff = false;
            }
            try
            {

                int retryCount = 3;
                string ret = "";
                for (int i = 0; i < retryCount; i++)
                {
                    lock (this)
                    {
                        _waitDataRecived.Reset();
                        _modbusClient.WriteSingleCoil(IAIConstAddr.SON, servoOnOff);
                        WaitDataReceived(ReadTimeOut);
                    }
                   
                }
                
                axis.Enabled = bEnable;
                OnUpdateCurrentPositions();
            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "Enable Axis Command fail {0}", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Enable Axis Command fail {0}", this.Nickname);
            }
        }

          /// <summary>
        /// MoveAbsAxis
        /// </summary>
        /// <param name="axis"></param>
        public override void MoveAbsAxis(RealAxis axis, MLengthSpeed speed, bool waitForCompletion)
        {
            
                try
                {
          
              
                    int iPos = (int)(axis.TargetPosition /0.01);
                    int iInpos = 1;
                    int iSpeed = (int)(speed / 0.01);
                    int iAccel = (int)(axis.AccelDecel /0.01);

                    string sPos = iPos.ToString("X8");
                    string sInpos = iInpos.ToString("X8");
                    string sSPeed = iSpeed.ToString("X8");

                    string s = sPos.Substring(4, 4);
                    int iPos1 = int.Parse(sPos.Substring(0, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    int iPos2 = int.Parse(sPos.Substring(4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    int iInpos1 = int.Parse(sInpos.Substring(0, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    int iInpos2 = int.Parse(sInpos.Substring(4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    int iSpeed1 = int.Parse(sSPeed.Substring(0, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    int iSpeed2 = int.Parse(sSPeed.Substring(4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                 

         
                    int[] regValue = new int[7];
                    regValue[0] = iPos1;
                    regValue[1] = iPos2;
                    regValue[2] = iInpos1;
                    regValue[3] = iInpos2;
                    regValue[4] = iSpeed1;
                    regValue[5] = iSpeed2;
                    regValue[6] = iAccel;

                    lock (this)
                    {
                        _waitDataRecived.Reset();
 
                        try
                        {
                            _modbusClient.WriteMultipleRegisters(IAIConstAddr.PCMD, regValue);
                        }
                        catch
                        {
                            
                        }
                        finally
                        {

                        }

                        WaitDataReceived(ReadTimeOut);
                    }

                    WaitForMoveDone(10000);

                    lock (this)
                    {
                        System.Threading.Thread.Sleep(100);
                        OnUpdateCurrentPositions();
                    }
                }
                catch (Exception ex)
                {
                    //U.LogAlarmPopup(ex, "Move Absolute Command fail {0}", this.Nickname);
                    throw new MCoreExceptionPopup(ex, "Move Absolute Command fail {0}", this.Nickname);
                }
            
        }

        public void WaitForMoveDone(int timeout)
        {
          
                Stopwatch timeoutStopw = new Stopwatch();
                timeoutStopw.Start();
                while (true)
                {
                    try
                    {
                        lock (this)
                        {
                            try
                            {
                                int[] regStatus = _modbusClient.ReadHoldingRegisters(IAIConstAddr.DSS1, 1);
                                int PEND = regStatus[0] & 0x0008;


                                if (PEND > 0)
                                {

                                    break;
                                }
                            }
                            catch
                            {

                            }
                            finally
                            {

                            }

                            if (timeoutStopw.ElapsedMilliseconds > timeout)
                            {
                                //U.LogAlarmPopup(string.Format("IAI SCON [{0}] error: Time out wait move absolute position.", this.Name));
                                throw new MCoreExceptionPopup("IAI SCON [{0}] error: Time out wait move absolute position.", this.Name);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //U.LogAlarmPopup(ex, "Wait Move Done Fail {0}", this.Nickname);
                        throw new MCoreExceptionPopup(ex, "Wait Move Done Fail {0}", this.Nickname);
                    }
                    Thread.Sleep(100);
                }
          
        }

      

        private byte[] WaitDataReceived(  Miliseconds timeout)
        {
            try
            {
                //U.BlockOrDoEvents(_waitDataRecived, timeout.ToInt);
                System.Threading.Thread.Sleep(100);
                return _modbusClient.receiveData;
            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "Timeout waiting for data recived of '{0}'", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Timeout waiting for data recived of '{0}'", this.Nickname);
            }
        }


        private void OnDataReceived(object sender)
        {
            try
            {
                lock (_waitDataRecived)
                {
                    _waitDataRecived.Set();
                }
            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "'{0}' received data error", Nickname);
                throw new MCoreExceptionPopup(ex, "'{0}' received data error", Nickname);
            }
        }


        //private void CheckReceivedData(byte[] sendData,byte[] recievedData)
        //{
        //    try
        //    {
        //        bool sendSucceed = sendData[sendData.Length - 1] == recievedData[recievedData.Length - 1];
        //        if(!sendSucceed)
        //        {
        //            throw new Exception("Return not sent not succeed");
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        U.LogAlarmPopup(ex, "Error Check Recieved Data {0}", Nickname);
        //        throw new MCoreExceptionPopup(ex, "Error Check Recieved Data {0}", Nickname);
        //    }

        //}

    }
}
