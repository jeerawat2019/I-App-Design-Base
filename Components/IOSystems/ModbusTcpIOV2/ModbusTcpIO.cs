using System;
using System.Collections;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Threading;

using MDouble;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;

namespace MCore.Comp.IOSystem
{
    public class ModbusTcpIO : IOSystemBase
    {

        private ModbusMaster _mbMaster;
        private Timer _updateTimer = null;
        private Miliseconds _timerPeriod = 50.0;
        private Inputs _inputs = null;
        private Outputs _outputs = null;

        private object _masterLocker = new object();


        private object _lockObject = new object();

        private int _numFailUpdateCount = 0;

        public ModbusMaster MbMaster
        {
            get
            {
                lock (_masterLocker)
                {
                    return _mbMaster;
                }
            }
        }



        /// <summary>
        /// Period of the timer
        /// </summary>
        [Browsable(true)]
        [Category("WAGO")]
        [Description("Input Timer Period")]
        [DisplayName("Input Timer Period")]
        public Miliseconds TimerPeriod
        {
            get { return _timerPeriod; }
            set
            {
                _timerPeriod = value;
                NotifyPropertyChanged(() => TimerPeriod);
            }
        }


        /// <summary>
        /// Get the Global IP address
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("TCPIP")]
        public string IPAddress
        {
            get { return GetPropValue(() => IPAddress,"1.1.1.1"); }
            set { SetPropValue(() => IPAddress, value); }
        }


        /// <summary>
        /// Get the Comm Port
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("TCPIP")]
        public ushort Port
        {
            get { return GetPropValue(() => Port,(ushort)502); }
            set { SetPropValue(() => Port, value); }
        }


        /// <summary>
        /// Allow Retry
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("General")]
        public ushort AllowFail
        {
            get { return GetPropValue(() => AllowFail, (ushort)10); }
            set { SetPropValue(() => AllowFail, value); }
        }


        /// <summary>
        /// Allow Retry
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("General")]
        public ushort AllowRetry
        {
            get { return GetPropValue(() => AllowRetry, (ushort)3); }
            set { SetPropValue(() => AllowRetry, value); }
        }



        #region Constructors
       
        /// <summary>
		/// Default constructor for xml streaming
		/// </summary>
		public ModbusTcpIO()
		{

		}
		


        public ModbusTcpIO(string name):base(name)
        {
            
        }

        #endregion Constructors


        /// <summary>
        /// Initialize the component
        /// </summary>
        public override void Initialize()
        {
            try
            {
                _mbMaster = new ModbusMaster(IPAddress, Port);
                _mbMaster.OnResponseData += new ModbusMaster.ResponseData(MbMaster_OnResponseData);
                _mbMaster.OnException += new ModbusMaster.ExceptionData(MbMaster_OnException);

                _outputs = FilterByTypeSingle<Outputs>();
                if (_outputs != null)
                {
                    //Do some task
                }

                 _inputs = FilterByTypeSingle<Inputs>();
                 if (_inputs != null)
                 {
                     _updateTimer = new Timer(OnTimer, this, (int)TimerPeriod, _timerPeriod.ToInt);
                     RegisterOnChanged(() => TimerPeriod, OnChangeTimerPeriod);
                 }

                 Simulate = eSimulate.None;
            }

            //catch (ForceSimulateException fsex)
            //{
            //    throw fsex;
            //}
            //catch (Exception ex)
            //{
            //    throw new ForceSimulateException(ex);
            //}

            catch (Exception ex)
            {
                
                //throw new ForceSimulateException(ex);
                U.LogAlarmPopup(ex, this.Nickname);
                this.Simulate = eSimulate.SimulateDontAsk;
            }


           
        }

        /// <summary>
        /// Destroy this Compopnent
        /// </summary>
        public override void Destroy()
        {
            if (_updateTimer != null)
            {
                _updateTimer.Dispose();
                _updateTimer = null;
            }

            base.Destroy();
            if (_mbMaster != null)
            {
                _mbMaster.disconnect();
                _mbMaster.Dispose();
            }
            
        }

        private void OnTimer(Object state)
        {

            UpdateInput();
           
        }


        private void OnChangeTimerPeriod(Miliseconds ms)
        {
            _updateTimer.Change(0, ms.ToInt);
        }


        private void UpdateInput()
        {
            int id = 1;
            byte lenght = 0x00;

            if (_inputs != null)
            {
                try
                {
                    lenght = Convert.ToByte(_inputs.Count);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return;
                }

                lock (_lockObject)
                {
                    MbMaster.ReadCoils(id, 0, lenght);
                }
            }
        }




        int retryRead = 0;
        private void MbMaster_OnResponseData(int ID, byte function, byte[] values)
        {
            // Ignore watchdog response data
            if (ID == 0xFF)
                return;

            // ------------------------------------------------------------------------
            // Identify requested data
            switch (ID)
            {
                case 1:
                    retryRead = AllowRetry;
                    UpdateEthernetInputValue(values);
                    _numFailUpdateCount = 0;
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:
                    _numFailUpdateCount = 0;
                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
            }
        }



        private void UpdateEthernetInputValue(byte[] values)
        {
            BitArray bitArray = new BitArray(values);
            bool[] bits = new bool[bitArray.Count];
            bitArray.CopyTo(bits, 0);

            BoolInput[] inputs = _inputs.FilterByType<BoolInput>();

            for (int inputCount = 0; inputCount < _inputs.Count; inputCount++)
            {

               inputs[inputCount].Value = bits[inputCount];

            }

        }


        /// <summary>
        /// Set the boolean outputs
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value"></param>
        public override void Set(BoolOutput boolOutput, bool value)
        {

            int id = 5;
            int bitAddress = boolOutput.Channel;
            lock (_lockObject)
            {
                if (MbMaster != null)
                {
                    MbMaster.WriteSingleCoils(id, bitAddress, value);
                }
            }

        }


        // ------------------------------------------------------------------------
        // Modbus TCP slave exception
        // ------------------------------------------------------------------------
        private void MbMaster_OnException(int id, byte function, byte exception)
        {
            retryRead--;
            if (retryRead > 0)
            {
                return;
            }

            string exc = "Modbus says error: ";
            switch (exception)
            {
                case ModbusMaster.excIllegalFunction:
                    exc += "Illegal function!";
                    break;
                case ModbusMaster.excIllegalDataAdr:
                    exc += "Illegal data adress!";
                    break;
                case ModbusMaster.excIllegalDataVal:
                    exc += "Illegal data value!";
                    break;
                case ModbusMaster.excSlaveDeviceFailure:
                    exc += "Slave device failure!";
                    break;
                case ModbusMaster.excAck:
                    exc += "Acknoledge!";
                    break;
                case ModbusMaster.excMemParityErr:
                    exc += "Memory parity error!";
                    break;
                case ModbusMaster.excGatePathUnavailable:
                    exc += "Gateway path unavailbale!";
                    break;
                case ModbusMaster.excExceptionTimeout:
                    exc += "Slave timed out!";
                    break;
                case ModbusMaster.excExceptionConnectionLost:
                    exc += "Connection is lost!";
                    break;
                case ModbusMaster.excExceptionNotConnected:
                    exc += "Not connected!";
                    //retry connect
                    _mbMaster.connect(IPAddress,Port);
                    break;
            }
            _numFailUpdateCount++;
            if (_numFailUpdateCount <= AllowFail)
            {
                throw (new Exception(string.Format("[{0}]:ModbusTcpIO Error. {1}"
                                                , this.Name, exc)));
            }

        }
      
    }
}
