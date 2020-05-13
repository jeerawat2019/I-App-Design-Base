using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

using MCore.Comp.Communications;
using MCore.Comp.IOSystem.Output;

using MDouble;

namespace MCore.Comp.IOSystem
{
    public class USTDCtrl : IOSystemBase
    {
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
        #endregion Public Properties
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public USTDCtrl()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public USTDCtrl(string name)
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
                throw new ForceSimulateException("USTDCtrl needs an RS232 parent");
            }

            if (!Enabled || !Parent.Enabled)
            {
                return;
            }

            Port.FullyDefined = true;
            Port.RefreshPort();
            
            if (!Port.IsOpen)
            {
                throw new ForceSimulateException("Port is not opened");
            }

            try
            {
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
        /// Opportunity to do any ID referencing for this class object
        /// Occurs after Initialize
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public override void Set(MiliSecOutput msOutput, Miliseconds value)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Set Strobe '{0}' = {1}", Nickname, value.ToString()));
            if (SetPW(DecimalToBinary((int)Math.Round(value.Val * 100.0))))
            {
                msOutput.Value = value;
            }
        }

        #endregion Overrides

        private string DecimalToBinary(int decimalNumber)
        {
            //int decimalNumber = int.Parse(dec);
            int remainder; string result = string.Empty;
            while (decimalNumber > 0)
            {
                remainder = decimalNumber % 2;
                decimalNumber /= 2;
                result = remainder.ToString() + result;
            }
            return result;
        }
        private bool SetPW(string bin)
        {
            try
            {
                if (!Port.IsOpen) { return false; }
                int no = 10 - bin.Length;
                for (int i = 0; i < no; i++)
                {
                    bin = "0" + bin;
                }
                string section1 = string.Empty; string section2 = string.Empty;
                string cmd = String.Empty; string cmd1 = String.Empty;
                //if (ch == Chanel.CH1)
                //{
                    section1 = "00001" + bin.Substring(0, 3);
                    section2 = "1" + bin.Substring(3, 7);
                    SendToPort(section1, section2);
                //}
                //if (ch == Chanel.CH2)
                {
                    section1 = "01101" + bin.Substring(0, 3);
                    section2 = "1" + bin.Substring(3, 7);
                    SendToPort(section1, section2);
                }
                return true;
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error setting the Strobe duration");
            }
            return false;

        }
        private void SendToPort(string section1, string section2)
        {
            try
            {
                byte hd1 = Convert.ToByte(section1, 2);
                byte hd2 = Convert.ToByte(section2, 2);
                Port.Port.ReadExisting();
                Port.Port.Write(new byte[] { hd1, hd2 }, 0, 2);
                double timeout = 1000;
                double stime = Environment.TickCount;
                while (Port.Port.BytesToRead == 0 && Environment.TickCount - stime < timeout)
                {
                    System.Threading.Thread.Sleep(10);
                    Application.DoEvents();

                }
                byte[] arrRes = new byte[Port.Port.BytesToRead];
                Port.Port.Read(arrRes, 0, arrRes.Length);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error setting the Strobe duration");
            }
        }
    }
}
