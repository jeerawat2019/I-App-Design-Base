using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using Automation.BDaq;

namespace MCore.Comp.IOSystem
{
    public class Advantech : IOSystemBase
    {
        InstantDiCtrl instantDiCtrl = new InstantDiCtrl();
        InstantDoCtrl instantDoCtrl = new InstantDoCtrl();
        private BackgroundWorker _updateLoop = null;
        private volatile bool _destroy = false;

        /// <summary>
        /// The interval of the Background Worker Thread update loop in ms
        /// </summary>
        [Browsable(true)]
        [Category("Advantech")]
        [Description("The interval of the Background Worker Thread update loop in ms")]
        public int LoopInterval
        {
            get { return GetPropValue(() => LoopInterval, 50); }
            set { SetPropValue(() => LoopInterval, value); }
        }
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Advantech()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public Advantech(string name)
            : base(name)
        {
        }

        #endregion Constructors

        static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }

        public override void Initialize()
        {

            base.Initialize();
            // Initialize 
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }

            try
            {
                // Establish Communication with Controller
                string deviceDescription = "USB-4761,BID#0";

                instantDiCtrl.SelectedDevice = new DeviceInformation(deviceDescription);
                instantDoCtrl.SelectedDevice = new DeviceInformation(deviceDescription);

                _updateLoop = new BackgroundWorker();
                _updateLoop.DoWork += new DoWorkEventHandler(UpdateLoop);
                _updateLoop.RunWorkerAsync();

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
        /// Set a digital output
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value"></param>
        public override void Set(BoolOutput boolOutput, bool value)
        {
            ErrorCode errorCode = ErrorCode.Success;
            errorCode = instantDoCtrl.WriteBit(0, boolOutput.Channel, (byte)(value ? 0xff : 0x00));
            boolOutput.Value = value;

        }
        /// <summary>
        /// Internal Update Loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateLoop(object sender, DoWorkEventArgs e)
        {
            do
            {
                if (LoopInterval == -1)
                {
                    return;
                }
                try
                {
                    ErrorCode errorCode = ErrorCode.Success;
                    BoolInput[] boolInputs = RecursiveFilterByType<BoolInput>();

                    byte[] buffer = new byte[64];
                    // Step 3: Read DI ports' status and show.
                    errorCode = instantDiCtrl.Read(0, 1, buffer);
                    if (BioFailed(errorCode))
                    {
                        U.LogError("Advantech DigInput failed");
                    }
                    //Show ports' status
                    int n = 1;
                    for (int i = 0; i < boolInputs.Length; i++)
                    {
                        boolInputs[i].Value = (buffer[0] & n) != 0;
                        n <<= 1;
                    }
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Advantech DigInput failed");
                }
                U.SleepWithEvents(LoopInterval);
            } while (!_destroy);
        }
        /// <summary>
        /// Pre Destroy
        /// </summary>
        public override void PreDestroy()
        {
            _destroy = true;
            base.PreDestroy();
        }

        /// <summary>
        /// Destroy
        /// </summary>
        public override void Destroy()
        {
            instantDoCtrl.Dispose();
            instantDiCtrl.Dispose();
            base.Destroy();
        }
    }
}
