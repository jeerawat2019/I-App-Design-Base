using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

using MCore.Comp.Communications;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using MDouble;

//using ElmoMotionControl.GMAS.EASComponents.MMCLibDotNET.InternalArgs;
using ElmoMotionControl.GMAS.EASComponents.MMCLibDotNET;


namespace MCore.Comp.MotionSystem
{
    public class Akribis : MotionSystemBase
    {
        private BackgroundWorker _updateLoop = null;
        private volatile bool _destroy = false;

        /// <summary>
        /// The interval of the Background Worker Thread update loop in ms
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("The interval of the Background Worker Thread update loop in ms")]
        public int LoopInterval
        {
            get { return GetPropValue(() => LoopInterval, 50); }
            set { SetPropValue(() => LoopInterval, value); }
        }

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Akribis()
        {
        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public Akribis(string name)
            : base(name)
        {

        }

        private Dictionary<CompBase, MMCSingleAxis> listAxis = new Dictionary<CompBase, MMCSingleAxis>();
        private Dictionary<CompBase, uint> outputVal = new Dictionary<CompBase, uint>();

        #endregion Constructors

        //MMCHostComm m_cCon;
        int m_connectHandle = 0;

        #region Overrides

        /// <summary>
        /// Initialize this component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }


            try
            {
                Initialized = false;

                TCPIP tcpip = GetParent<TCPIP>();
                if (tcpip == null)
                {
                    throw new ForceSimulateException("Needs TCPIP parent");
                }

                IPAddress GMasIP, LocalIP;
                IPAddress.TryParse(tcpip.IPAddress, out GMasIP);
                IPAddress.TryParse(tcpip.LocalIPAddress, out LocalIP);
                if (MMCConnection.ConnectRPC(GMasIP, 4000, LocalIP, 5000, new cbFunc(UCB.UserCallback), 0xEFFFFFFF, out m_connectHandle) != 0)
                {
                    m_connectHandle = 0;
                    throw new ForceSimulateException("Unable to Connect");
                }
                _updateLoop = new BackgroundWorker();
                _updateLoop.DoWork += new DoWorkEventHandler(UpdateLoop);
                //_bw = new AbortableBackgroundWorker() { WorkerSupportsCancellation = true };
                //_bw.DoWork += new DoWorkEventHandler(WaitForData);
                //_bw.RunWorkerAsync();
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
        /// Called after Initialization
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            RealAxis[] arrRealAxis = this.RecursiveFilterByType<RealAxis>();
            foreach (RealAxis realAxis in arrRealAxis)
            {
                try
                {
                    MMCSingleAxis singleAxis = new MMCSingleAxis(realAxis.Name, m_connectHandle);
                    listAxis.Add(realAxis, singleAxis);
                    outputVal.Add(realAxis, 0);
                    if (!IsDisabled(singleAxis))
                    {
                        //singleAxis.Stop(MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                        singleAxis.PowerOff(MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                    }
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Failed to connect to '{0}'", realAxis.Name);
                }
            }
            RealRotary[] arrRealRotary = this.RecursiveFilterByType<RealRotary>();
            foreach (RealRotary realRotary in arrRealRotary)
            {
                try
                {
                    MMCSingleAxis singleAxis = new MMCSingleAxis(realRotary.Name, m_connectHandle);
                    listAxis.Add(realRotary, singleAxis);
                    outputVal.Add(realRotary, 0);
                    //singleAxis.Stop(MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                    if (!IsDisabled(singleAxis))
                    {
                        singleAxis.PowerOff(MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                    }
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Failed to connect to '{0}'", realRotary.Name);
                }
            }

            IOBoolChannel[] arrWAGOoutput = this.RecursiveFilterByType<IOBoolChannel>();
            foreach (IOBoolChannel outPuts in arrWAGOoutput)
            {
                try
                {
                    if (outPuts.Name.Contains("WAGO"))
                    {
                        MMCSingleAxis singleAxis = new MMCSingleAxis(outPuts.Name, m_connectHandle);

                        listAxis.Add(outPuts, singleAxis);
                        outputVal.Add(outPuts, 0);
                    }

                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Failed to connect to '{0}'", outPuts.Name);
                }
            }


            ClearAllFaults();
            _updateLoop.RunWorkerAsync();
        }

        public override void Destroy()
        {
            base.Destroy();
            // _isDestroying == true
            if (_updateLoop != null)
            {
                _updateLoop.DoWork -= new DoWorkEventHandler(UpdateLoop);
            }

            if (m_connectHandle != 0)
            {
                foreach (MMCSingleAxis singleAxis in listAxis.Values)
                {
                    try
                    {
                        singleAxis.PowerOff(MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                    }
                    catch (MMCException ex)
                    {
                        U.LogError(ex, "MMC Exception PowerOff");
                    }
                }
                try
                {
                    MMCConnection.CloseConnection(MMCConnection.GetConnection(m_connectHandle));
                    m_connectHandle = 0;
                }
                catch (MMCException ex)
                {
                    U.LogError(ex, "MMC Exception on CloseConnection");
                }
            }
        }

        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public override void MoveAbsAxis(RealAxis axis, MLengthSpeed speed, bool waitForCompletion)
        {
            Millimeters mmSpeed = new Millimeters(speed);
            MoveAbsAxis(axis, axis.TargetPosition, (float)mmSpeed, waitForCompletion);
        }
        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public override void MoveAbsAxis(RealRotary axis, MRotarySpeed speed, bool waitForCompletion)
        {
            Degrees degSpeed = new Degrees(speed);
            MoveAbsAxis(axis, axis.TargetPosition, (float)degSpeed, waitForCompletion);
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
        /// EnableAxis an Axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="bEnable"></param>
        public override void EnableAxis(RealAxis axis, bool bEnable)
        {
            EnableAxis(axis, bEnable);
            axis.Enabled = bEnable;
        }

        /// <summary>
        /// EnableAxis an Axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="bEnable"></param>
        public override void EnableAxis(RealRotary axis, bool bEnable)
        {
            EnableAxis(axis, bEnable);
            axis.Enabled = bEnable;
        }
        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public override void StopAxis(RealAxis axis)
        {
            StopAxis(axis);
        }
        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public override void StopAxis(RealRotary axis)
        {
            StopAxis(axis);
        }

        public override void ClearAllFaults()
        {
            base.ClearAllFaults();
            if (m_connectHandle != 0)
            {
                try
                {
                    foreach (MMCSingleAxis singleAxis in listAxis.Values)
                    {
                        try
                        {
                            if (!IsDisabled(singleAxis))
                            {
                                singleAxis.Reset();

                            }
                        }
                        catch (MMCException ex)
                        {
                            U.LogPopup(ex, "MMC Reset");
                        }
                    }
                    MMCConnection.ResetSystemErrors(m_connectHandle);
                }
                catch (MMCException ex)
                {
                    U.LogPopup(ex, "Error MMC ResetSystemErrors");
                }
            }

        }

        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public override void HomeAxis(RealAxis axis)
        {
            if (HomeAxisDS402(axis, axis.HomeMethod))
            {
                axis.Homed = true;
            }
        }
        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public override void HomeAxis(RealRotary axis)
        {
            if (HomeAxisDS402(axis, axis.HomeMethod))
            {
                axis.Homed = true;
            }
        }

        /// <summary>
        /// Set a digital output
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value"></param>
        public override void Set(BoolOutput boolOutput, bool value)
        {

            VAR_TYPE varT = VAR_TYPE.BYTE_ARR;
            PI_VAR_UNION varUnion = new PI_VAR_UNION();


            CompBase compAxis = boolOutput.GetParent<RealAxis>();
            if (compAxis == null)
            {
                compAxis = boolOutput.GetParent<RealRotary>();
                if (compAxis == null)
                {
                    compAxis = boolOutput.GetParent<IOBoolChannel>();
                   
                }
            } 


            if (listAxis.ContainsKey(compAxis))
            {
                try
                {
                    MMCSingleAxis singleAxis = listAxis[compAxis];

                    if (compAxis is RealRotary)
                    {
                        int val = value ? 1 : 0;
                        //singleAxis.SetDigOutputs(boolOutput.Channel << 16, (byte)(value ? 0x1 : 0x0), 0x0);
                        singleAxis.SendIntCmdViaSDO(string.Format("OB[{0}]={1}", boolOutput.Channel, val), 1000);
                        
                    }
                    else if (compAxis is RealAxis)
                    {
                        uint val = outputVal[compAxis];
                        uint chanBit = (uint)(1 << (16 + boolOutput.Channel));
                        if (value)
                        {
                            val |= chanBit;
                        }
                        else
                        {
                            val &= ~chanBit;
                        }
                        singleAxis.SetDigOutputs32Bit(0, val);
                        outputVal[compAxis] = val;
                    }

                    else
                    {
                        UInt16 index = (UInt16)boolOutput.Channel;

                        if (value)
                        {
                            varUnion.s_byte = 0;
                        }
                        else
                        {
                            varUnion.s_byte = 1;
                        }
                        singleAxis.WritePIVar(index, varUnion, varT);
                    }

                    boolOutput.Value = value;
                    //singleAxis.SetDigOutputs(chan, (byte)(value ? 1 : 0), (byte)1);
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Trouble setting Digital Output ({0})", boolOutput.Name);
                }
            }

        }

        #endregion Overrides

        private void EnableAxis(CompBase compAxis, bool bEnable)
        {
            if (listAxis.ContainsKey(compAxis))
            {
                try
                {
                    MMCSingleAxis singleAxis = listAxis[compAxis];
                    bool disabled = IsDisabled(singleAxis);
                    if (bEnable && disabled)
                    {
                        singleAxis.PowerOn(MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                        //singleAxis.EnableEndMotionEvent();
                    }
                    else if (!bEnable && !disabled)
                    {
                        singleAxis.PowerOff(MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                        //singleAxis.DisableEndMotionEvent();
                    }
                }
                catch (Exception ex)
                {
                    if (bEnable)
                        U.LogPopup(ex, "Trouble enabling axis ({0})", compAxis.Name);
                    else
                        U.LogPopup(ex, "Trouble disabling axis ({0})", compAxis.Name);
                }
            }
        }

        private bool HomeAxisDS402(CompBase compAxis, int homeMethod)
        {
            if (listAxis.ContainsKey(compAxis))
            {
                MMCSingleAxis singleAxis = listAxis[compAxis];
                if (!OKtoCmdMove(singleAxis))
                {
                    return false;
                }
                try
                {
                    OPM402 opMode = singleAxis.GetOpMode();
                    if (opMode != OPM402.OPM402_HOMING_MODE)
                    {
                        try
                        {
                            singleAxis.SetOpMode(OPM402.OPM402_HOMING_MODE);
                            Thread.Sleep(500); //wait Mode Change;
                        }
                        catch (Exception ex)
                        {
                            U.LogPopup(ex, "Cannot change to Homing mode for axis '{0}'", compAxis.Name);
                            return false; ;
                        }
                    }
                    MMC_HOMEDS402Params hParam = new MMC_HOMEDS402Params();
                    hParam.dbPosition = 0;
                    hParam.eBufferMode = MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE;
                    hParam.uiHomingMethod = homeMethod;  // Index_Axis = 33, X_Axis = 1, Y_Axis = 1, Z_Axis = 2
                    hParam.fAcceleration = 200f;                  
                    //hParam.fDistanceLimit = 200f;
                    //hParam.uiTimeLimit = 10000;
                    //hParam.fTorqueLimit = 0f;
                    hParam.fVelocity = 20f;
                    hParam.ucExecute = 1;
                    if (GetOperatingMode(singleAxis) != OPM402.OPM402_HOMING_MODE)
                    {
                        try
                        {
                            singleAxis.SetOpMode(OPM402.OPM402_HOMING_MODE);
                            Thread.Sleep(500); //wait Mode Change;
                        }
                        catch (MMCException ex)
                        {
                            U.LogPopup(ex, "Cannot change to Homing mode for '{0}'", singleAxis.AxisName);
                            return false; ;
                        }
                    }

                    try
                    {
                        singleAxis.HomeDS402(hParam);
                        U.SleepWithEvents(200);
                        WaitHomingDone(singleAxis, 25000);//(int)hParam.uiTimeLimit);
                    }
                    catch (MMCException ex)
                    {
                        U.LogPopup(ex, "Homing error for '{0}'", singleAxis.AxisName);
                        singleAxis.Reset();
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Failed to home '{0}'", compAxis.Name);
                    singleAxis.Reset();
                }
            }
            return false;
        }


        private OPM402 GetOperatingMode(MMCSingleAxis singleAxis)
        {
            OPM402 opMode = OPM402.OPM402_NO;
            try
            {
                opMode = singleAxis.GetOpMode();
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Cannot Get Op Mode for '{0}'", singleAxis.AxisName);
            }
            return opMode; ;
        }

        private void StopAxis(CompBase compAxis)
        {
            if (listAxis.ContainsKey(compAxis))
            {
                MMCSingleAxis singleAxis = listAxis[compAxis];
                try
                {
                    singleAxis.Stop(MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Failed to Stop Axis '{0}'", compAxis.Name);
                }
            }
        }

        const uint plcStateMask = 0x7F8;
        private bool IsMotorError(MMCSingleAxis singleAxis)
        {
            MC_STATE_SINGLE status = (MC_STATE_SINGLE)(singleAxis.ReadStatus() & plcStateMask);
            return ((status & MC_STATE_SINGLE.ERROR_STOP) == MC_STATE_SINGLE.ERROR_STOP);
        }
        private bool IsDisabled(MMCSingleAxis singleAxis)
        {
            MC_STATE_SINGLE status = (MC_STATE_SINGLE)(singleAxis.ReadStatus() & plcStateMask);
            if ((status & MC_STATE_SINGLE.DISABLED) == MC_STATE_SINGLE.DISABLED)
            {
                return true;
            }
            return ((status & MC_STATE_SINGLE.ERROR_STOP) == MC_STATE_SINGLE.ERROR_STOP);
        }

        private bool OKtoCmdMove(MMCSingleAxis singleAxis)
        {
            MC_STATE_SINGLE status = (MC_STATE_SINGLE)(singleAxis.ReadStatus() & plcStateMask);
            if ((status & MC_STATE_SINGLE.DISCRETE_MOTION) == MC_STATE_SINGLE.DISCRETE_MOTION)
                return false;
            if ((status & MC_STATE_SINGLE.HOMING) == MC_STATE_SINGLE.HOMING)
                return false;
            if ((status & MC_STATE_SINGLE.ERROR_STOP) == MC_STATE_SINGLE.ERROR_STOP)
                return false;
            return true;
        }

        private bool IsHoming(MMCSingleAxis singleAxis)
        {
            MC_STATE_SINGLE status = (MC_STATE_SINGLE)(singleAxis.ReadStatus() & plcStateMask);
            return ((status & MC_STATE_SINGLE.HOMING) == MC_STATE_SINGLE.HOMING);
        }

        private bool IsMotionDone(MMCSingleAxis singleAxis)
        {
            MC_STATE_SINGLE status = (MC_STATE_SINGLE)(singleAxis.ReadStatus() & plcStateMask);
            if ((status & MC_STATE_SINGLE.STAND_STILL) == MC_STATE_SINGLE.STAND_STILL)
                return true;
            if ((status & MC_STATE_SINGLE.ERROR_STOP) == MC_STATE_SINGLE.ERROR_STOP)
                return true;
            if ((status & MC_STATE_SINGLE.DISABLED) == MC_STATE_SINGLE.DISABLED)
                return true;
            return false;
        }

        private bool WaitHomingDone(MMCSingleAxis singleAxis, int msTimeOut)
        {
            U.SleepWithEvents(50);
            if (msTimeOut > 0)
            {
                double mSecElapsed = 0;
                long startTime = U.DateTimeNow;
                do
                {
                    U.SleepWithEvents(10);
                    if (!IsHoming(singleAxis))
                        return true;
                    mSecElapsed = U.TicksToMS(U.DateTimeNow - startTime);
                } while (mSecElapsed < (double)msTimeOut);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Wait for move done
        /// </summary>
        /// <param name="singleAxis"></param>
        /// <param name="msWait">Calculate approximate time to wait for completion (a little less than)</param>
        /// <returns></returns>
        private bool WaitMoveDone(MMCSingleAxis singleAxis, int msWait)
        {
            long timeout = U.DateTimeNow + U.MSToTicks(msWait * 2.0 + 5000.0);
            U.SleepWithEvents(Math.Max(msWait, 50));
            do
            {
                if (IsMotionDone(singleAxis))
                    return true;
                U.SleepWithEvents(10);
            } while (U.DateTimeNow < timeout);
            return false;
        }

        private bool WaitForMoveReady(MMCSingleAxis singleAxis,double msTimeOut)
        {
            long timeout = U.DateTimeNow + U.MSToTicks(msTimeOut);
            while (!OKtoCmdMove(singleAxis))
            {
                if (U.DateTimeNow > timeout)
                {
                    U.LogPopup("Timeout waiting for {0} axis to start moving.", singleAxis.AxisName);
                    return false;
                }
                U.SleepWithEvents(20);
            }
            return true;
        }
        private void MoveAbsAxis(CompBase compAxis, double pos, float velocity, bool waitForCompletion)
        {
            if (listAxis.ContainsKey(compAxis))
            {
                MMCSingleAxis singleAxis = listAxis[compAxis];
                if (!WaitForMoveReady(singleAxis, 20000.0))
                {
                    return;
                }

                MMC_MOTIONPARAMS_SINGLE singleParam = new MMC_MOTIONPARAMS_SINGLE();
                if (compAxis is RotaryBase)
                {
                    singleParam.fAcceleration = 250f;
                    singleParam.fDeceleration = 250f;
                }
                else
                {
                    singleParam.fAcceleration = 1000f;
                    singleParam.fDeceleration = 1000f;
                }
                singleParam.fVelocity = velocity;
                singleParam.eDirection = MC_DIRECTION_ENUM.MC_SHORTEST_WAY;
                singleParam.fJerk = 100f;
                singleParam.eBufferMode = MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE;
                singleParam.ucExecute = 1;
                singleAxis.SetDefaultParams(singleParam);
                

                OPM402 opMode = singleAxis.GetOpMode();
                if (opMode != OPM402.OPM402_CYCLIC_SYNC_POSITION_MODE)
                {
                    try
                    {
                        singleAxis.SetOpMode(OPM402.OPM402_CYCLIC_SYNC_POSITION_MODE);
                        Thread.Sleep(500);  //'Wait Mode change before move'
                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex, "Cannot set to OPM402_CYCLIC_SYNC_POSITION_MODE mode axis {0}", compAxis.Name);
                        return;
                    }
                }
                try
                {
                    double actualPos = singleAxis.GetActualPosition();

                    if (actualPos != pos)
                    {
                        singleAxis.MoveAbsolute(pos, velocity, MC_BUFFERED_MODE_ENUM.MC_BUFFERED_MODE);
                        if (waitForCompletion)
                        {
                            double curPos = 0.0;
                            if (compAxis is RotaryBase)
                            {
                                curPos = (compAxis as RotaryBase).CurrentPosition;
                            }
                            else if (compAxis is RealAxis)
                            {
                                curPos = (compAxis as RealAxis).CurrentPosition;
                            }
                            else
                            {
                                U.LogPopup("Unexpected Axis type for MoveAbsAxis: Type={0} Axis={1}", compAxis.GetType().Name, compAxis.Nickname); 
                            }
                            // Calculate approximate time to wait for completion (a little less than)
                            double mSecWait = Math.Abs(pos - curPos) * 950.0 / velocity;
                            if (!WaitMoveDone(singleAxis, (int)mSecWait))
                            {
                                U.LogPopup("Timeout moving {0}", compAxis.Name); 
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Failed To Move '{0}'", compAxis.Name);
                }
            }
        }


        /// <summary>
        /// Internal Update Loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateLoop(object sender, DoWorkEventArgs e)
        {

            PIVarDirection dir = PIVarDirection.ePI_INPUT;
            VAR_TYPE varT = VAR_TYPE.S_BYTE;
            PI_VAR_UNION varUnion = new PI_VAR_UNION();

            do
            {
                try
                {
                    Thread.Sleep(LoopInterval);
                    KeyValuePair<CompBase, MMCSingleAxis>[] pairs = listAxis.ToArray();
                    foreach ( KeyValuePair<CompBase, MMCSingleAxis> pair in pairs)
                    {
                        try
                        {
                            if (pair.Key is IOBoolChannel)
                            {
                                BoolInput[] inputs = pair.Key.RecursiveFilterByType<BoolInput>();
                                foreach (BoolInput input in inputs)
                                {
                                    pair.Value.ReadPIVar((UInt16)(input.Channel), dir, varT, ref varUnion);
                                    
                                    if (varUnion.s_byte == 1)
                                    {
                                        input.Value = true;
                                    }
                                    else
                                    {
                                        input.Value = false;
                                    }
                                }
                            }
                            else
                            {
                                double dVal = pair.Value.GetActualPosition();
                                if (pair.Key is RealAxis)
                                {
                                    (pair.Key as RealAxis).SetCurrentPosition(dVal);
                                    (pair.Key as RealAxis).MoveDone = IsMotionDone(pair.Value);
                                }
                                else if (pair.Key is RealRotary)
                                {
                                    (pair.Key as RealRotary).SetCurrentPosition(dVal);
                                    (pair.Key as RealRotary).MoveDone = IsMotionDone(pair.Value);
                                }
                                BoolInput[] inputs = pair.Key.RecursiveFilterByType<BoolInput>();
                                foreach (BoolInput input in inputs)
                                {
                                    input.Value = pair.Value.GetDigInput(input.Channel + 16) != 0;
                                }

                            }


                                MC_STATE_SINGLE status = (MC_STATE_SINGLE)(pair.Value.ReadStatus() & plcStateMask);
                                if ((status & MC_STATE_SINGLE.ERROR_STOP) == MC_STATE_SINGLE.ERROR_STOP)
                                {
                                    pair.Key.MachineStatus(string.Format("{0} ErrorStop", pair.Value.AxisName));
                                }



                        }
                        catch (MMCException ex)
                        {
                            U.LogPopup(ex, "MMC Update Error Axis {0}", pair.Key.Name);
                        }
                    }

                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Error UpdateLoop: ");
                }
            } while (!_destroy);
        }
    }
    public class UCB
    {
        static public void UserCallback(object sender, MMC_CAN_REPLY_DATA_OUT data)
        {

        }
    }
}
