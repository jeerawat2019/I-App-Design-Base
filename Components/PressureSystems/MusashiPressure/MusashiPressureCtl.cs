using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp.Communications;
using MDouble;

namespace MCore.Comp.PressureSystem
{
    public class MusashiPressureCtl : PressureSystemBase
    {

        #region privates

        const char STX = '\x02';
        const char ETX = '\x03';
        const char EOT = '\x04';
        const char ENQ = '\x05';
        const char ACK = '\x06';
        const string sSTX = "\x02";
        const string sETX = "\x03";
        const string sEOT = "\x04";
        const string sENQ = "\x05";
        const string sACK = "\x06";
        const string SUCESSFUL = "Successful";
        const string ERROR = "Error";

        private string sSuccessful = new string(new char[] { STX, '0', '2', 'A', '0', '2', 'D', ETX });
        private string sError = new string(new char[] { STX, '0', '2', 'A', '2', '2', 'B', ETX });
        private string sCAN = new string(new char[] { STX, '0', '2', '\x18', '\x18', '6', 'E', ETX });
        private object _readOrWrite = new object();

        private KPaChannel _kPaChannel = null;

        #endregion privates
        
        #region Public Properties
        /// <summary>
        /// Get the RS 232 device
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public RS232 Port
        {
            get { return GetPropValue(() => Port); }
            set { SetPropValue(() => Port, value); }
        }

        public KPaChannel PressureDevice
        {
            get { return _kPaChannel; }
        }

        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MusashiPressureCtl()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MusashiPressureCtl(string name)
            : base(name)
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

            // Initialize 
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }

            Port = GetParent<RS232>();
            if (Port == null)
            {
                throw new ForceSimulateException("MusashiPressureCtl needs an RS232 parent");
            }

            if (!Enabled || !Parent.Enabled)
            {
                return;
            }
            Port.OwnDataHandler = true;
            Port.FullyDefined = true;
            Port.RefreshPort();
            
            if (!Port.IsOpen)
            {
                throw new ForceSimulateException("Port is not opened");
            }

            try
            {
                // Send upload command to return version info
                string result = Upload("UL001D08");
                if (!result.Contains("0DDA08VR"))
                {
                    throw new ForceSimulateException("Device is not responding");
                }
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

        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            // For now:
            _kPaChannel = this.FilterByExactTypeSingle<KPaChannel>();
            UpdateAll();
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        /// <summary>
        /// Set a Pressure
        /// </summary>
        /// <param name="prChannel"></param>
        /// <param name="kPa"></param>
        public override void SetPressure(KPaChannel prChannel, KiloPascal kPa)
        {
            string cmd = string.Format("PR  P{0:0000}T{1:0000}V-{2:0000}", 
                kPa.Val*10.0, prChannel.DispenseTime.Val, prChannel.VacuumPressure.Val*100);
            if (Download(cmd) == SUCESSFUL)
            {
                prChannel.Value = kPa.Val;
            }
        }
        public override double UpdateAll()
        {
            double press = 0.0;
            if (PressureDevice != null)
            {
                string cmd = string.Format("UL{0:000}D01", PressureDevice.Channel);
                string result = Upload(cmd);
                try
                {
                    // Parse it all
                    // stx21DA01P1206T00009V0000M0
                    if (result.Length > 24)
                    {
                        press = Convert.ToDouble(result.Substring(8, 4)) / 10.0;
                        PressureDevice.Value = press;
                        PressureDevice.DispenseTime = Convert.ToDouble(result.Substring(13, 5));
                        PressureDevice.VacuumPressure = Convert.ToDouble(result.Substring(19, 4)) / 100.0;
                        switch (result[24])
                        {
                            case '0':
                                PressureDevice.TimedMode = true;
                                break;
                            case '1':
                                PressureDevice.TimedMode = false;
                                break;
                        }
                    }
                }
                catch { }
            }
            return press;
        }

        #endregion Overrides

        private string PrepareString(string cmd)
        {
            string sendString = string.Format("{0}{1}", cmd.Length.ToString("X2"), cmd);
            byte[] s = Encoding.UTF8.GetBytes(sendString);
            byte checksum = 0;
            foreach (byte ch in s)
            {
                checksum -= ch;
            }
            string chks = checksum.ToString("X2");
            return string.Format("{0}{1}{2}{3}", STX, sendString, chks, ETX);
        }

        public string Download(string cmd)
        {
            string rd = SendAndRead(sENQ);
            if (rd != sACK)
            {
                return "No Acq";
            }
            rd = SendAndRead(PrepareString(cmd));
            if( rd == sSuccessful)
            {
                Port.Write(sEOT);
                return SUCESSFUL;
            }
            Port.Write(sCAN);
            Port.Write(sEOT);
            return ERROR;
        }

        string SendAndRead(string cmd)
        {
            lock (Port.LockPort)
            {
                Port.Write(cmd);
                return ReadString();
            }
        }


        public string Upload(string cmd)
        {
            lock (_readOrWrite)
            {
                string rd = string.Empty;
                try
                {
                    rd = SendAndRead(sENQ);
                    if (rd != sACK)
                    {
                        return "No Acq";
                    }
                    rd = SendAndRead(PrepareString(cmd));
                    if(rd != sSuccessful)
                    {
                        lock (Port.LockPort)
                        {
                            Port.Write(sCAN);
                            Port.Write(sEOT);
                        }
                        return "Error";
                    }

                    rd = SendAndRead(sACK);
                    if (rd == sError)
                    {
                        lock (Port.LockPort)
                        {
                            Port.Write(sCAN);
                            Port.Write(sEOT);
                        }
                        return "No Reponse";
                    }
                    lock (Port.LockPort)
                    {
                        Port.Write(sEOT);
                    }
                }
                catch
                {
                    U.LogError("Timeout waiting for measure of '{0}'", Nickname);
                }
                return rd;
            }
        }

        string ReadString()
        {
            double timeout = 3500;
            double stime = Environment.TickCount;
            byte[] by1 = new byte[1];
            List<byte> by = new List<byte>();
            try
            {
                while (Port.Port.BytesToRead == 0 && Environment.TickCount - stime < timeout)
                {
                    U.SleepWithEvents(20);
                }
                if (Port.Port.BytesToRead == 0)
                {
                    return string.Empty;
                }
                while (Port.Port.BytesToRead > 0)
                {
                    if (Port.Port.Read(by1, 0, 1) == 1)
                    {
                        by.Add(by1[0]);
                    }
                    else
                    {
                        int jjb = 0;
                    }
                    if (Port.Port.BytesToRead == 0)
                    {
                        U.SleepWithEvents(20);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            byte[] arrRes = by.ToArray();
            string val = Encoding.UTF8.GetString(arrRes, 0, arrRes.Length);
            return val;
        }
    }
}
