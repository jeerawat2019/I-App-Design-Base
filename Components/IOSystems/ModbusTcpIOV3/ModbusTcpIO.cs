using System;
using System.Collections;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using MDouble;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;



namespace MCore.Comp.IOSystem
{
    public class ModbusTcpIO : IOSystemBase
    {

        private ModbusClient _mbReaderClient = null;
        private ModbusClient _mbWriterClient = null;


        private Timer _readerTimer = null;
        private Timer _writerTimer = null;
        private Miliseconds _timerPeriod = 10.0;
        private Inputs _inputs = null;
        private Outputs _outputs = null;
        

        private object _clientLocker = new object();


        private object _lockObject = new object();

        private object _lockSetOutputListBuffer = new object();
        private List<Tuple<int, bool>> _setOutputListBuffer = new List<Tuple<int, bool>>();
       

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


        /// <summary>
        /// Read Write Multi Thread
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("General")]
        public bool ReadWriteMultiThread
        {
            get { return GetPropValue(() => ReadWriteMultiThread, false); }
            set { SetPropValue(() => ReadWriteMultiThread, value); }
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
               
                _mbReaderClient = new ModbusClient(IPAddress, Port);
                _mbReaderClient.Connect();

                if (ReadWriteMultiThread)
                {
                    _mbWriterClient = new ModbusClient(IPAddress, Port);
                    _mbWriterClient.Connect();
                }
               
                _outputs = FilterByTypeSingle<Outputs>();
                if (_outputs != null)
                {
                    //Do some task
                }

                 _inputs = FilterByTypeSingle<Inputs>();
                 if (_inputs != null)
                 {
                     _readerTimer = new Timer(OnReaderTimer, this, 0, _timerPeriod.ToInt);

                     if (ReadWriteMultiThread)
                     {
                         _writerTimer = new Timer(OnWriterTimer, this, 0, _timerPeriod.ToInt);
                     }

                     RegisterOnChanged(() => TimerPeriod, OnChangeTimerPeriod);
                 }

                 Simulate = eSimulate.None;
            }

         

            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, this.Nickname);
                this.Simulate = eSimulate.SimulateDontAsk;
            }


           
        }

        /// <summary>
        /// Destroy this Compopnent
        /// </summary>
        public override void Destroy()
        {
            if (_readerTimer != null)
            {
                _readerTimer.Dispose();
                _readerTimer = null;
            }

            if (_writerTimer != null)
            {
                _writerTimer.Dispose();
                _writerTimer = null;
            }


            base.Destroy();


            if(_mbReaderClient != null)
            {
                _mbReaderClient.Disconnect();
                _mbReaderClient = null;
            }

            if (_mbWriterClient != null)
            {
                _mbWriterClient.Disconnect();
                _mbWriterClient = null;
            }
            
        }

        private void OnReaderTimer(Object state)
        {
            UpdateInput();
           
        }


        private void OnWriterTimer(Object state)
        {
            UpdateOutput();

        }


        private void OnChangeTimerPeriod(Miliseconds ms)
        {
            _readerTimer.Change(0, ms.ToInt);
            if (ReadWriteMultiThread)
            {
                _writerTimer.Change(0, ms.ToInt);
            }
        }


        private void UpdateInput()
        {
            //int id = 1;
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


                bool[] values = null;
                //lock (_lockObject)
                //{
                if (ReadWriteMultiThread)
                {
                    values = _mbReaderClient.ReadCoils(0, lenght);
                }
                else
                {
                    try
                    {
                        Tuple<int, bool>[] setOutputList = null;
                        lock (_setOutputListBuffer)
                        {
                            if (_setOutputListBuffer.Count > 0)
                            {
                                setOutputList = _setOutputListBuffer.ToArray();
                                _setOutputListBuffer.Clear();
                            }
                        }
                        lock (_lockObject)
                        {
                            if (setOutputList != null)
                            {
                                Array.ForEach(setOutputList, output => _mbReaderClient.WriteSingleCoil(output.Item1, output.Item2));
                            }
                            values = _mbReaderClient.ReadCoils(0, lenght);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                //}
                Task.Run(() => UpdateEthernetInputValue(values));
            }
        }





        private void UpdateEthernetInputValue(bool[] values)
        {
           
            BoolInput[] inputs = _inputs.FilterByType<BoolInput>();

            for (int inputCount = 0; inputCount < _inputs.Count; inputCount++)
            {
                try
                {
                    inputs[inputCount].Value = values[inputCount];
                }
                catch(Exception ex)
                {
                    throw ex;
                }

            }

        }


        private void UpdateOutput()
        {
            try
            {
                Tuple<int, bool>[] setOutputList = null;
                lock (_setOutputListBuffer)
                {
                    if (_setOutputListBuffer.Count > 0)
                    {
                        setOutputList = _setOutputListBuffer.ToArray();
                        _setOutputListBuffer.Clear();
                    }
                }
                lock (_lockObject)
                {
                    if (setOutputList != null)
                    {
                        Array.ForEach(setOutputList, output => _mbReaderClient.WriteSingleCoil(output.Item1, output.Item2));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }              
        }


        /// <summary>
        /// Set the boolean outputs
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value"></param>
        public override void Set(BoolOutput boolOutput, bool value)
        {
            int bitAddress = boolOutput.Channel;
            Tuple<int, bool> setOutTuple = new Tuple<int, bool>(bitAddress, value);

            lock (_lockSetOutputListBuffer)
            {
                _setOutputListBuffer.Add(setOutTuple);
            }
            //lock (_lockObject)
            //{
            //    if (_mbClient != null)
            //    {
            //        _mbClient.WriteSingleCoil( bitAddress, value);
            //    }
            //}
        }


      
    }
}
