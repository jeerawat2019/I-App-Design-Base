using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

using MCore.Comp;
using MCore.Comp.Communications;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Helpers;

//using MCore.Comp.MeasurementSystem;
//using MCore.Comp.IOSystem;
using MDouble;

namespace MCore.Comp.IOSystem
{
    public class Keyence_LJ_V7001 : IOSystemBase
    {
        #region Private data


        private static Dictionary<int,MillimeterInput> s_mmInputs = new Dictionary<int,MillimeterInput>();
        private static Dictionary<int,Array2DInput> s_arrInputs = new Dictionary<int,Array2DInput>();
        private MillimeterInput[] _mmInputs = null;
        private Array2DInput[] _arrInputs = null;
        //private AbortableBackgroundWorker _bw = null;

        //private LJV7IF_TARGET_SETTING _targetSetting;
        //private byte _depth;
        //private byte[] _data;

        #endregion



        #region Public Properties
        /// <summary>
        /// True If using High Speed
        /// </summary>
        [Browsable(true), Category("Keyence"), Description("True if using High Speed")]
        public bool HighSpeedCom
        {
            get { return GetPropValue(() => HighSpeedCom, true); }
            set { SetPropValue(() => HighSpeedCom, value); }
        }

        //public string millimeterInputString
        //{
        //    get { return _millimeterInputString; }
        //}



        #endregion


        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Keyence_LJ_V7001()
        {
        }


        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public Keyence_LJ_V7001(string name) : base(name)
        {

        }



        #endregion Constructors




        #region Overrides


        /// <summary>
        /// Initailize this component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            if (Simulate == eSimulate.SimulateDontAsk)
            {
                Initialized = false;
                throw new ForceSimulateException("Always Simulate");
            }


            try
            {
                _mmInputs = this.FilterByType<MillimeterInput>();
                _arrInputs = this.FilterByType<Array2DInput>();
                foreach (MillimeterInput mmInput in _mmInputs)
                {
                    s_mmInputs.Add(mmInput.Channel, mmInput);
                }
                foreach (Array2DInput arrInput in _arrInputs)
                {
                    s_arrInputs.Add(arrInput.Channel, arrInput);
                }
                Initialized = false;
                ConnectDevicesAndController();


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
        /// Opportunity to do any ID referencing for this class object
        /// Occurs after Initialize
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();


        }

        public override void Destroy()
        {
            if (Initialized)
            {
                DisconnectDevicesAndController();
            }
            base.Destroy();
            //if (_bw != null)
            //{
            //    if (_bw.IsBusy)
            //    {
            //        _bw.CancelAsync();
            //    }
            //    _bw.Dispose();
            //    _bw = null;
            //}
        }


		/// <summary>
		/// Measurement range X direction
		/// </summary>
		public const int MEASURE_RANGE_FULL = 800;
		public const int MEASURE_RANGE_MIDDLE = 600;
		public const int MEASURE_RANGE_SMALL = 400;

		/// <summary>
		/// Light reception characteristic
		/// </summary>
		public const int RECEIVED_BINNING_OFF = 1;
		public const int RECEIVED_BINNING_ON = 2;

		public const int COMPRESS_X_OFF = 1;
		public const int COMPRESS_X_2 = 2;
		public const int COMPRESS_X_4 = 4;
        		/// <summary>
		/// Get the profile data size
		/// </summary>
		/// <returns>Data size of one profile (in units of bytes)</returns>
		private uint GetOneProfileDataSize()
		{
			// Buffer size (in units of bytes)
			uint retBuffSize = 0;

			// Basic size
			int basicSize = MEASURE_RANGE_FULL / RECEIVED_BINNING_OFF;
			basicSize /= COMPRESS_X_OFF;

			// Number of headers  (Plus whether one or two head (1 or 2)
			retBuffSize += (uint)basicSize * 1U;

			// Envelope setting (Either 2 or 1)
			retBuffSize *= 2U;

			//in units of bytes
			retBuffSize *= (uint)Marshal.SizeOf(typeof(uint));

			// Sizes of the header and footer structures
			LJV7IF_PROFILE_HEADER profileHeader = new LJV7IF_PROFILE_HEADER();
			retBuffSize += (uint)Marshal.SizeOf(profileHeader);
			LJV7IF_PROFILE_FOOTER profileFooter = new LJV7IF_PROFILE_FOOTER();
			retBuffSize += (uint)Marshal.SizeOf(profileFooter);

			return retBuffSize;
		}

        static public int CalculateDataSize(LJV7IF_PROFILE_INFO profileInfo)
        {
            LJV7IF_PROFILE_HEADER header = new LJV7IF_PROFILE_HEADER();
            LJV7IF_PROFILE_FOOTER footer = new LJV7IF_PROFILE_FOOTER();

            return profileInfo.wProfDataCnt * profileInfo.byProfileCnt * (profileInfo.byEnvelope + 1) +
                (Marshal.SizeOf(header) + Marshal.SizeOf(footer)) / Marshal.SizeOf(typeof(int));
        }


        ///// <summary>
        ///// Trigger the input to set the value
        ///// </summary>
        ///// <param name="input"></param>
        //public override void Trigger(CompMeasure input)
        //{
        //    Array2DInput inputBase = input as Array2DInput;

        //    if (inputBase == null)
        //    {
        //        U.LogInfo("Cannot measure because '{0}' is not of type Array2DInput", input.Nickname);
        //    }
        //    else
        //    {
        //        lock (inputBase)
        //        {
        //            Rc rc = 0; // (Rc)NativeMethods.LJV7IF_StartMeasure(inputBase.Channel);
        //            U.SleepWithEvents(500);

        //            // Specify the target batch to get.
        //            LJV7IF_GET_BATCH_PROFILE_REQ req = new LJV7IF_GET_BATCH_PROFILE_REQ();
        //            req.byTargetBank = (byte)ProfileBank.Active;
        //            req.byPosMode = (byte)BatchPos.Commited;
        //            req.dwGetBatchNo = 0;
        //            req.dwGetProfNo = 0;
        //            req.byGetProfCnt = byte.MaxValue;
        //            req.byErase = 0;

        //            LJV7IF_GET_BATCH_PROFILE_RSP rsp = new LJV7IF_GET_BATCH_PROFILE_RSP();
        //            LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();

        //            int profileDataSize = MAX_PROFILE_COUNT +
        //                (Marshal.SizeOf(typeof(LJV7IF_PROFILE_HEADER)) + Marshal.SizeOf(typeof(LJV7IF_PROFILE_FOOTER))) / Marshal.SizeOf(typeof(int));
        //            int[] receiveBuffer = new int[profileDataSize * req.byGetProfCnt];

        //            List<ProfileData> profileDatas = new List<ProfileData>();
        //            // Get profiles
        //            using (PinnedObject pin = new PinnedObject(receiveBuffer))
        //            {
        //                rc = (Rc)NativeMethods.LJV7IF_GetBatchProfile(inputBase.Channel, ref req, ref rsp, ref profileInfo, pin.Pointer,
        //                    (uint)(receiveBuffer.Length * Marshal.SizeOf(typeof(int))));
        //                // @Point
        //                // # When reading all the profiles from a single batch, the specified number of profiles may not be read.
        //                // # To read the remaining profiles after the first set of profiles have been read, set the specification method (byPosMode)to 0x02, 
        //                //   specify the batch number (dwGetBatchNo), and then set the number to start reading profiles from (dwGetProfNo) and 
        //                //   the number of profiles to read (byGetProfCnt) to values that specify a range of profiles that have not been read to read the profiles in order.
        //                // # In more detail, this process entails:
        //                //   * First configure req as listed below and call this function again.
        //                //      byPosMode = LJV7IF_BATCH_POS_SPEC
        //                //      dwGetBatchNo = batch number that was read
        //                //      byGetProfCnt = Profile number of unread in the batch
        //                //   * Furthermore, if all profiles in the batch are not read,update the starting position for reading profiles (req.dwGetProfNo) and
        //                //     the number of profiles to read (req.byGetProfCnt), and then call LJV7IF_GetBatchProfile again. (Repeat this process until all the profiles have been read.)

        //                if (!CheckReturnCode(rc)) return;

        //                // Output the data of each profile
        //                int unitSize = ProfileData.CalculateDataSize(profileInfo);
        //                for (int i = 0; i < rsp.byGetProfCnt; i++)
        //                {
        //                    profileDatas.Add(new ProfileData(receiveBuffer, unitSize * i, profileInfo));
        //                }

        //                // Get all profiles within the batch.
        //                req.byPosMode = (byte)BatchPos.Spec;
        //                req.dwGetBatchNo = rsp.dwGetBatchNo;
        //                do
        //                {
        //                    // Update the get profile position
        //                    req.dwGetProfNo = rsp.dwGetBatchTopProfNo + rsp.byGetProfCnt;
        //                    req.byGetProfCnt = (byte)Math.Min((uint)(byte.MaxValue), (rsp.dwCurrentBatchProfCnt - req.dwGetProfNo));

        //                    rc = (Rc)NativeMethods.LJV7IF_GetBatchProfile(inputBase.Channel, ref req, ref rsp, ref profileInfo, pin.Pointer,
        //                        (uint)(receiveBuffer.Length * Marshal.SizeOf(typeof(int))));
        //                    if (!CheckReturnCode(rc)) return;
        //                    for (int i = 0; i < rsp.byGetProfCnt; i++)
        //                    {
        //                        profileDatas.Add(new ProfileData(receiveBuffer, unitSize * i, profileInfo));
        //                    }
        //                } while (rsp.dwGetBatchProfCnt != (rsp.dwGetBatchTopProfNo + rsp.byGetProfCnt));
        //            }
        //        }
        //    }
        //}        
        
        /// <summary>
        /// Trigger the input to set the value
        /// </summary>
        /// <param name="input"></param>
        public override void Trigger(CompMeasure input)
        {
            Array2DInput inputBase = input as Array2DInput;

            if (inputBase == null)
            {
                U.LogInfo("Cannot measure because '{0}' is not of type Array2DInput", input.Nickname);
            }
            else
            {
                inputBase.Instantiate();
                //U.LogInfo("{0},StartMeasureBegin", input.Nickname);

                // Start high-speed data communication.
                long startTime = U.DateTimeNow;
                Rc rc = (Rc)NativeMethods.LJV7IF_StartMeasure(inputBase.Channel);
                double ms = U.TicksToMS(U.DateTimeNow - startTime);
                //U.LogInfo("{0},StartMeasureEnd,{1}", input.Nickname,ms);
                CheckReturnCode(rc);
                if (!HighSpeedCom)
                {
                    BackgroundWorker waitForResults = new BackgroundWorker();
                    waitForResults.DoWork += new DoWorkEventHandler(WaitForResults);
                    waitForResults.RunWorkerAsync(inputBase);
                }
            }
        }
        const uint BATCHDONE = 0x80000000;
        const uint CLEARMEM  = 0x00000100;
        /// <summary>
        /// Method that is called from the DLL as a callback function
        /// </summary>
        /// <param name="buffer">Leading pointer of the received data</param>
        /// <param name="size">Size in units of bytes of one profile</param>
        /// <param name="count">Number of profiles</param>
        /// <param name="notify">Completion flag</param>
        /// <param name="user">Thread ID (value passed during initialization)</param>
        public void ReceiveHighSpeedData(IntPtr buffer, uint size, uint count, uint notify, uint user)
        {
            try
            {
                double ms = 0;
                long startTime = U.DateTimeNow;
                int channel = (int)user;
                Array2DInput arrayInput = s_arrInputs[channel];
                //U.LogInfo("{0},CallbackEntry", arrayInput.Name);
                arrayInput.DataId = (uint)(channel + 1);
                if (count > 0)
                {
                    uint profileSize = (uint)(size / Marshal.SizeOf(typeof(int)));
                    List<int[]> receiveBuffer = new List<int[]>();
                    int[] bufferArray = new int[profileSize * count];
                    Marshal.Copy(buffer, bufferArray, 0, (int)(profileSize * count));
                    // Profile data retention
                    for (int i = 0; i < count; i++)
                    {
                        int[] oneProfile = new int[profileSize];
                        Array.Copy(bufferArray, i * profileSize, oneProfile, 0, profileSize);
                        arrayInput.ReceiveOneProfile(oneProfile);
                    }
                    ms = U.TicksToMS(U.DateTimeNow - startTime);
                    //U.LogInfo("{0},HighSpData,{1},Count={2},Notify={3}", arrayInput.Name, ms, count, notify);
                    //Debug.WriteLine("{0},HighSpData,{1},Count={2},Notify={3}", arrayInput.Name, ms, count, notify);
                }
                if (notify != 0)
                {
                    //U.LogInfo("{0},AcquisitionComplete,{1},Count={2},Notify={3}", arrayInput.Name, ms, count, notify);
                    //U.AsyncCall(StopMeasure, channel);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            //ThreadSafeBuffer.Add((int)user, receiveBuffer, notify);
        }

        /// <summary>
        /// Stop Measure
        /// </summary>
        /// <param name="input"></param>
        public override void StopMeasure(Array2DInput input)
        {
            StopMeasure(input.Channel);
        }

        private void StopMeasure(int channel)
        {
            long startTime = U.DateTimeNow;
            Array2DInput inputBase = null;
            double ms = 0;
            if (s_arrInputs.Count > channel)
            {
                inputBase = s_arrInputs[channel];
                //U.LogInfo("{0},AppProcessingBegin", inputBase.Name);
                inputBase.AcquisitionComplete(string.Empty);
                ms = U.TicksToMS(U.DateTimeNow - startTime);
                //U.LogInfo("{0},AppProcessingEnd,{1}", inputBase.Name, ms);
                //U.LogInfo("{0},StopMeasureBegin", inputBase.Name);
            }

            if (!HighSpeedCom)
            {
                startTime = U.DateTimeNow;
                Rc rc = (Rc)NativeMethods.LJV7IF_StopMeasure(channel);
                if (inputBase != null)
                {
                    ms = U.TicksToMS(U.DateTimeNow - startTime);
                    //U.LogInfo("{0},StopMeasureEnd,{1}, {2}", inputBase.Name, ms, rc.ToString());
                }
            }
        }
        public const int MAX_PROFILE_COUNT = 3200;
        private void WaitForResults(object sender, DoWorkEventArgs e)
        {
            Array2DInput inputBase = e.Argument as Array2DInput;
            lock (inputBase)
            {
                LJV7IF_GET_BATCH_PROFILE_RSP rsp = new LJV7IF_GET_BATCH_PROFILE_RSP();
                try
                {
                    Rc rc = 0;
                    double ms = 0.0;
                    long startTime = U.DateTimeNow;
                    byte byAcqSize = 100; // byte.MaxValue;

                    // Specify the target batch to get.
                    LJV7IF_GET_BATCH_PROFILE_REQ req = new LJV7IF_GET_BATCH_PROFILE_REQ();
                    req.byTargetBank = (byte)ProfileBank.Active;
                    req.byPosMode = (byte)BatchPos.Current;
                    req.dwGetBatchNo = 0;
                    req.dwGetProfNo = 0;
                    req.byGetProfCnt = byAcqSize;
                    req.byErase = 0;

                    LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();

                    uint oneDataSize = GetOneProfileDataSize();
                    //req.byGetProfCnt = byte.MaxValue;
                    uint allDataSize = oneDataSize * byte.MaxValue;

                    int[] profileData = new int[allDataSize / Marshal.SizeOf(typeof(int))];
                    bool hasData = false;
                    using (PinnedObject pin = new PinnedObject(profileData))
                    {
                        do
                        {
                            rc = (Rc)NativeMethods.LJV7IF_GetBatchProfile(inputBase.Channel, ref req, ref rsp, ref profileInfo, pin.Pointer,
                                (uint)(profileData.Length * Marshal.SizeOf(typeof(int))));
                            if (rc != Rc.Ok && rc != Rc.NoBatchData)
                            {
                                inputBase.AcquisitionComplete(string.Format("Get Batch returned an Error={0}", rc.ToString()));
                                return;
                            }
                            hasData = rc == Rc.Ok && rsp.dwGetBatchNo != inputBase.DataId;
                            if (!hasData)
                            {
                                ms = U.TicksToMS(U.DateTimeNow - startTime);

                                if (ms > 5000)
                                {
                                    inputBase.AcquisitionComplete("Unable to get first profile in time");
                                    return;
                                }
                                U.SleepWithEvents(10);
                            }
                        } while (!hasData);
                        startTime = U.DateTimeNow;
                        //U.LogInfo("{0},First profile,0,rsp.byGetProfCnt={1}", inputBase.Name, rsp.byGetProfCnt);
                        int dataUnit = CalculateDataSize(profileInfo);
                        int readPropfileDataSize = dataUnit;
                        inputBase.NumCols = profileInfo.wProfDataCnt;
                        inputBase.Instantiate();

                        LJV7IF_PROFILE_HEADER profileHeader = new LJV7IF_PROFILE_HEADER();
                        uint dataOffset = (uint)Marshal.SizeOf(profileHeader) / sizeof(int);

                        int iProfile = 0;
                        for (; iProfile < (int)rsp.byGetProfCnt; iProfile++)
                        {
                            int prevVal = 0;
                            double [,] data = inputBase.Data;
                            for (int n = 0; n < profileInfo.wProfDataCnt; n++)
                            {
                                int val = profileData[iProfile * readPropfileDataSize + n + dataOffset];
                                if (val == int.MinValue)
                                {
                                    val = prevVal;
                                }
                                prevVal = val;
                                data[iProfile, n] = (double)val;
                            }
                        }
                        
                        // Get all profiles within the batch.
                        req.byPosMode = (byte)BatchPos.Spec;
                        inputBase.DataId = rsp.dwGetBatchNo;
                        req.dwGetBatchNo = rsp.dwGetBatchNo;
                        ms = U.TicksToMS(U.DateTimeNow - startTime);
                        //U.LogInfo("{0},App processing First profile,{1}", inputBase.Name, ms);
                        do
                        {
                            //U.SleepWithEvents(10);
                            // Update the get profile position
                            req.dwGetProfNo = rsp.dwGetBatchTopProfNo + rsp.byGetProfCnt;
                            req.byGetProfCnt = (byte)Math.Min((uint)byAcqSize, (rsp.dwCurrentBatchProfCnt - req.dwGetProfNo));
                            long timeWaitingForProfile = U.DateTimeNow;
                            rc = (Rc)NativeMethods.LJV7IF_GetBatchProfile(inputBase.Channel, ref req, ref rsp, ref profileInfo, pin.Pointer,
                                    (uint)(profileData.Length * Marshal.SizeOf(typeof(int))));
                            if (rc != 0)
                            {
                                inputBase.AcquisitionComplete(string.Format("Get BatchProfile returned an Error={0}", rc.ToString()));
                                return;
                            }
                            long timeGotProfile = U.DateTimeNow;
                            ms = U.TicksToMS(timeGotProfile - startTime);

                            if (ms > 5000)
                            {
                                inputBase.AcquisitionComplete("Unable to get all profiles in time");
                                return;
                            }
                            //U.SleepWithEvents(10);
                            ms = U.TicksToMS(U.DateTimeNow - timeWaitingForProfile);
                            //U.LogInfo("{0},Next Batch,{1}, byGetProfCnt={2} GetBatchProfCount={3}  dwGetBatchTopProfNo={4}",
                            // inputBase.Name, ms, rsp.byGetProfCnt, rsp.dwGetBatchProfCnt, rsp.dwGetBatchTopProfNo);
                            for (int i = 0; i < rsp.byGetProfCnt; i++, iProfile++)
                            {
                                int prevVal = 0;
                                for (int n = 0; n < profileInfo.wProfDataCnt; n++)
                                {
                                    int val = profileData[i * readPropfileDataSize + n + dataOffset];
                                    if (val == int.MinValue)
                                    {
                                        val = prevVal;
                                    }
                                    prevVal = val;
                                    inputBase.Data[iProfile, n] = (double)val;
                                }
                            }
                            ms = U.TicksToMS(U.DateTimeNow - timeGotProfile);
                            //U.LogInfo("{0},App processing,{1}", inputBase.Name, ms);
                        } while ((uint)inputBase.NumRows != (rsp.dwGetBatchTopProfNo + rsp.byGetProfCnt));
                    }
                    ms = U.TicksToMS(U.DateTimeNow - startTime);
                    //U.LogInfo("{0},Complete Scan,{1}", inputBase.Name, ms);
                    inputBase.Duration = ms;
                    inputBase.AcquisitionComplete(string.Empty);

                }
                finally
                {
                    long startTime = U.DateTimeNow; 
                    NativeMethods.LJV7IF_StopMeasure(inputBase.Channel);
                    double ms = U.TicksToMS(U.DateTimeNow - startTime);
                    //U.LogInfo("{0},StopMeasure,{1}", inputBase.Name, ms);
                }
            }
        }
        /// <summary>
        /// Return code check
        /// </summary>
        /// <param name="rc">Return code</param>
        /// <returns>Is the return code OK?</returns>
        /// <remarks>If the return code is not OK, display a message and return false.</remarks>
        private static bool CheckReturnCode(Rc rc)
        {
            if (rc == Rc.Ok) return true;
            U.LogPopup("Keyence Error: 0x{0,8:x}", rc);
            return false;
        }
        private void WaitForResults2(object sender, DoWorkEventArgs e)
        {
            string errorMsg = string.Empty;
            Array2DInput inputBase = e.Argument as Array2DInput;
            lock (inputBase)
            {
                long startTime = U.DateTimeNow;
                double ms;
                LJV7IF_GET_BATCH_PROFILE_REQ req = new LJV7IF_GET_BATCH_PROFILE_REQ();
                req.byTargetBank = (byte)ProfileBank.Active;
                req.byPosMode = (byte)BatchPos.Commited;
                req.dwGetBatchNo = 0;
                req.dwGetProfNo = 0;
                req.byGetProfCnt = byte.MaxValue;
                req.byErase = 0;
                LJV7IF_GET_BATCH_PROFILE_RSP rsp = new LJV7IF_GET_BATCH_PROFILE_RSP();
                LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();

                uint oneDataSize = GetOneProfileDataSize();
                req.byGetProfCnt = byte.MaxValue;
                uint allDataSize = oneDataSize * req.byGetProfCnt;

                int[] profileData = new int[allDataSize / Marshal.SizeOf(typeof(int))];
                GCHandle _Handle = GCHandle.Alloc(profileData, GCHandleType.Pinned);
                IntPtr pointer = _Handle.AddrOfPinnedObject();
                //req.byPosMode = (byte)BatchPos.Current;
                //do
                //{
                    int rc = NativeMethods.LJV7IF_GetBatchProfile(inputBase.Channel, ref req, ref rsp, ref profileInfo, pointer,
                        (uint)(profileData.Length * Marshal.SizeOf(typeof(int))));
                    if (rc != 0)
                    {
                        inputBase.AcquisitionComplete("Get Batch returned an Error");
                        return;
                    }

                    //U.SleepWithEvents(20);
                    //ms = U.TicksToMS(U.DateTimeNow - startTime);

                    //if (ms > 2000)
                    //{
                    //    errorMsg = "Unable to get first Profile";
                    //    break;
                    //}
                //} while (false);

                if (string.IsNullOrEmpty(errorMsg))
                {
                    int dataUnit = CalculateDataSize(profileInfo);
                    //AnalyzeProfileData(profileCount, ref profileInfo, profileData, 0, dataUnit);
                    int readPropfileDataSize = dataUnit;
                    inputBase.NumCols = profileInfo.wProfDataCnt;
                    inputBase.Instantiate();

                    LJV7IF_PROFILE_HEADER profileHeader = new LJV7IF_PROFILE_HEADER();
                    uint dataOffset = (uint)Marshal.SizeOf(profileHeader) / sizeof(int);

                    int iProfile = 0;
                    for (; iProfile < (int)rsp.byGetProfCnt; iProfile++)
                    {
                        int prevVal = 0;
                        for (int n = 0; n < profileInfo.wProfDataCnt; n++)
                        {
                            int val = profileData[iProfile * readPropfileDataSize + n + dataOffset];
                            if (val == int.MinValue)
                            {
                                val = prevVal;
                            }
                            prevVal = val;
                            inputBase.Data[iProfile, n] = (double)val;
                        }
                    }

                    // Get all profiles within the batch.

                    req.byPosMode = (byte)BatchPos.Spec;
                    req.dwGetBatchNo = rsp.dwGetBatchNo;
                    do
                    {
                        //req.byGetProfCnt = (byte)Math.Min((int)byte.MaxValue, (int)(inputBase.NumRows - rsp.dwCurrentBatchProfCnt));
                        //// Update the get profile position
                        //req.dwGetProfNo = rsp.dwCurrentBatchProfCnt;
                        req.dwGetProfNo = rsp.dwGetBatchTopProfNo + rsp.byGetProfCnt;
                        req.byGetProfCnt = (byte)Math.Min((uint)(byte.MaxValue), (rsp.dwCurrentBatchProfCnt - req.dwGetProfNo));

                        rc = NativeMethods.LJV7IF_GetBatchProfile(inputBase.Channel, ref req, ref rsp, ref profileInfo, pointer,
                            (uint)(profileData.Length * Marshal.SizeOf(typeof(int))));
                        if (rc != 0)
                        {
                            errorMsg = "Get Batch returned an Error";
                            break;
                        }
                        for (int i = 0; i < rsp.byGetProfCnt; i++)
                        //                        for (int i = (int)req.dwGetProfNo; i < rsp.dwCurrentBatchProfCnt; i++)
                        {
                            iProfile = i - (int)req.dwGetProfNo;
                            int prevVal = 0;
                            for (int n = 0; n < profileInfo.wProfDataCnt; n++)
                            {
                                int val = profileData[iProfile * readPropfileDataSize + n + dataOffset];
                                if (val == int.MinValue)
                                {
                                    val = prevVal;
                                }
                                prevVal = val;
                                inputBase.Data[i, n] = (double)val;
                            }
                        }
                    //} while (rsp.dwCurrentBatchProfCnt < inputBase.NumRows);
                    } while (rsp.dwGetBatchProfCnt != (rsp.dwGetBatchTopProfNo + rsp.byGetProfCnt));
                }
                ms = U.TicksToMS(U.DateTimeNow - startTime);
                inputBase.Duration = ms;

                //for (int i = 0; i < inputBase.NumRows; i++)
                //{
                //    string line = string.Empty;
                //    for (int n = 0; n < profileInfo.wProfDataCnt; n++)
                //    {
                //        line += string.Format("{0},", inputBase.Data[i, n]);
                //    }
                //    U.LogCustom("KeyData.csv", line);
                //}
                inputBase.AcquisitionComplete(errorMsg);
            }
        }


        /// <summary>
        /// Do the measure, trigger and wait for result
        /// </summary>
        /// <param name="input"></param>
        /// <param name="timeout"></param>
        public override bool Measure(CompMeasure input, Miliseconds timeout)
        {
            Array2DInput inputBase = input as Array2DInput;

            if (inputBase == null)
            {
                U.LogInfo("Cannot measure because '{0}' is not of type InputBase", input.Nickname);
                return false;
            }
            // Start Batch
            int rc = NativeMethods.LJV7IF_StopMeasure(inputBase.Channel);
            rc = NativeMethods.LJV7IF_StartMeasure(inputBase.Channel);

            long startTime = U.DateTimeNow;

            LJV7IF_GET_BATCH_PROFILE_REQ req = new LJV7IF_GET_BATCH_PROFILE_REQ();
            LJV7IF_GET_BATCH_PROFILE_RSP rsp = new LJV7IF_GET_BATCH_PROFILE_RSP();
            LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();

            uint oneDataSize = GetOneProfileDataSize();
            req.byGetProfCnt = byte.MaxValue;
            uint allDataSize = oneDataSize * req.byGetProfCnt;

            int[] profileData = new int[allDataSize / Marshal.SizeOf(typeof(int))];
            GCHandle _Handle = GCHandle.Alloc(profileData, GCHandleType.Pinned);
            IntPtr pointer = _Handle.AddrOfPinnedObject();
            req.byPosMode = (byte)BatchPos.Current;
            do
            {
                rc = NativeMethods.LJV7IF_GetBatchProfile(inputBase.Channel, ref req, ref rsp, ref profileInfo, pointer,
                    (uint)(profileData.Length * Marshal.SizeOf(typeof(int))));
                if (rc != 0)
                {
                    return false;
                }
                Thread.Sleep(20);
            } while (rsp.byCurrentBatchCommited > 0);


            int dataUnit = CalculateDataSize(profileInfo);
            //AnalyzeProfileData(profileCount, ref profileInfo, profileData, 0, dataUnit);
            int readPropfileDataSize = dataUnit;
            inputBase.NumCols = profileInfo.wProfDataCnt;
            inputBase.Instantiate();

            LJV7IF_PROFILE_HEADER profileHeader = new LJV7IF_PROFILE_HEADER();
            uint dataOffset = (uint)Marshal.SizeOf(profileHeader)/sizeof(int);

            int iProfile = 0;
            for (; iProfile < rsp.dwCurrentBatchProfCnt; iProfile++)
            {
                int prevVal = 0;
                for (int n = 0; n < profileInfo.wProfDataCnt; n++)
                {
                    int val = profileData[iProfile * readPropfileDataSize + n + dataOffset];
                    if (val == int.MinValue)
                    {
                        val = prevVal;
                    }
                    prevVal = val;
                    inputBase.Data[iProfile, n] = (double)val;
                }
            }

            // Get all profiles within the batch.

            req.byPosMode = (byte)BatchPos.Spec;
            req.dwGetBatchNo = rsp.dwGetBatchNo;
            do
            {
                req.byGetProfCnt = (byte)Math.Min((int)byte.MaxValue, (int)(inputBase.NumRows - rsp.dwCurrentBatchProfCnt));
                // Update the get profile position
                req.dwGetProfNo = rsp.dwCurrentBatchProfCnt;

                rc = NativeMethods.LJV7IF_GetBatchProfile(inputBase.Channel, ref req, ref rsp, ref profileInfo, pointer,
                    (uint)(profileData.Length * Marshal.SizeOf(typeof(int))));
                if (rc != 0)
                {
                    break;
                }
                for (int i = (int)req.dwGetProfNo; i < rsp.dwCurrentBatchProfCnt; i++)
                {
                    iProfile = i - (int)req.dwGetProfNo;
                    int prevVal = 0;
                    for (int n = 0; n < profileInfo.wProfDataCnt; n++)
                    {
                        int val = profileData[iProfile * readPropfileDataSize + n + dataOffset];
                        if (val == int.MinValue)
                        {
                            val = prevVal;
                        }
                        prevVal = val;
                        inputBase.Data[i, n] = (double)val;
                    }
                }
            } while (rsp.dwCurrentBatchProfCnt < inputBase.NumRows);

            double ms = U.TicksToMS(U.DateTimeNow - startTime);
            inputBase.Duration = ms;

            //for (int i = 0; i < inputBase.NumRows; i++)
            //{
            //    string line = string.Empty;
            //    for (int n = 0; n < profileInfo.wProfDataCnt; n++)
            //    {
            //        line += string.Format("{0},", inputBase.Data[i, n]);
            //    }
            //    U.LogCustom("KeyData.csv", line);
            //}

            return true;
        }

        ///// <summary>
        ///// Update the "Laser Level" data parameter
        ///// </summary>
        ///// <param name="input"></param>
        //private void WaitForData(object sender, DoWorkEventArgs e)
        //{
        //    while (!IsDestroying)
        //    {
        //        try
        //        {
        //            for (int i = 0; i < _mmInputs.Length; i++)
        //            {
        //                LJV7IF_MEASURE_DATA[] measureData = new LJV7IF_MEASURE_DATA[NativeMethods.MeasurementDataCount];
        //                int rc = NativeMethods.LJV7IF_GetMeasurementValue(_mmInputs[i].Channel, measureData);
        //                //AddLogResult(rc, Resources.SID_GET_MEASUREMENT_VALUE);
        //                if (rc == (int)Rc.Ok)
        //                {
        //                    //_measureDatas.Clear();
        //                    //_measureDatas.Add(new MeasureData(0, measureData));
        //                    _mmInputs[i].Value = measureData[i].fValue;                            
        //                }
        //                else
        //                {
        //                    _mmInputs[i].Value = -1.0E+10;  
        //                }
        //            }
        //            for (int i = 0; i < _arrInputs.Length; i++)
        //            {
        //                //LJV7IF_MEASURE_DATA[] measureData = new LJV7IF_MEASURE_DATA[NativeMethods.MeasurementDataCount];
        //                int rc = 0; // NativeMethods.LJV7IF_GetMeasurementValue(_mmInputs[i].Channel, measureData);
        //                //AddLogResult(rc, Resources.SID_GET_MEASUREMENT_VALUE);
        //                if (rc == (int)Rc.Ok)
        //                {
        //                    //_measureDatas.Clear();
        //                    //_measureDatas.Add(new MeasureData(0, measureData));
        //                    _arrInputs[i].Data = null; // ? measureData[i].fValue;
        //                }
        //                else
        //                {
        //                    _arrInputs[i].Data = null;
        //                }
        //            }
        //            Thread.Sleep(20);

        //        }
        //        catch (Exception ex)
        //        {
        //            U.LogError(ex, "Problem reading sensor.");
        //            Thread.Sleep(500);
        //        }
        //    }
        //    U.LogInfo("{0} thread completed.", Nickname);
        //}


        ///// <summary>
        ///// Set Limit on Controller
        ///// </summary>
        ///// <param name="upperlimit"></param>
        ///// <param name="lowerlimit"></param>
        //public override void SetLimit(double limit)
        //{

        //    _data = new byte[NativeMethods.ProgramSettingSize];
        //    _data[0] = (byte)limit;

        //}


        //public void SetLimitSensor(string upperlimit, string lowerlimit, string programNo, string outputNo)
        //{

        //    // † There are three setting areas: a) the write settings area, b) the running area, and c) the save area.
        //    //   * Specify a) for the setting level when you want to change multiple settings. However, to reflect settings in the LJ-V operations, you have to call LJV7IF_ReflectSetting.
        //    //	 * Specify b) for the setting level when you want to change one setting but you don't mind if this setting is returned to its value prior to the change when the power is turned off.
        //    //	 * Specify c) for the setting level when you want to change one setting and you want this new value to be retained even when the power is turned off.

        //    // @Point
        //    //  As a usage example, we will show how to use SettingForm to configure settings such that sending a setting, with SettingForm using its initial values,
        //    //  will change the sampling period in the running area to "100 Hz."
        //    //  Also see the GetSetting function.

            
        //    // Set upper limit
        //    _depth = 0x01;						                        // Setting depth: Running settings area
        //    _targetSetting.byType = Convert.ToByte(programNo, 16);		// Setting type: Program number
        //    _targetSetting.byCategory = 0x06;	                        // Category: Output settings
        //    _targetSetting.byItem = 0x0E;		                        // Setting item: Upper limit
        //    _targetSetting.byTarget1 = Convert.ToByte(outputNo);		// Setting target 1: output number
        //    _targetSetting.byTarget2 = 0x0;		                        // Setting target 2: None
        //    _targetSetting.byTarget3 = 0x0;		                        // Setting target 3: None
        //    _targetSetting.byTarget4 = 0x0;		                        // Setting target 4: None
        //    SetLimit(Convert.ToInt16(upperlimit));

        //    uint dwError = 0;
        //    using (PinnedObject pin = new PinnedObject(_data))

        //    try 
        //    {
        //        Rc rc = (Rc)NativeMethods.LJV7IF_SetSetting(_deviceID, _depth, _targetSetting, pin.Pointer, (uint)_data.Length, ref dwError);

        //        if (rc != Rc.Ok)
        //        {
        //            U.LogPopup("Cannot set upper limit on sensor");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        U.LogPopup(ex, "Keyence_LJ_V7001_Ctrl.SetLimit error");
        //    }

        //}




        //public override void SetLimit(DispSensor dispSensor, double upperLimit, double lowerLimit)
        //{
        //    base.SetLimit(dispSensor, upperLimit, lowerLimit);

        //    // † There are three setting areas: a) the write settings area, b) the running area, and c) the save area.
        //    //   * Specify a) for the setting level when you want to change multiple settings. However, to reflect settings in the LJ-V operations, you have to call LJV7IF_ReflectSetting.
        //    //	 * Specify b) for the setting level when you want to change one setting but you don't mind if this setting is returned to its value prior to the change when the power is turned off.
        //    //	 * Specify c) for the setting level when you want to change one setting and you want this new value to be retained even when the power is turned off.

        //    // @Point
        //    //  As a usage example, we will show how to use SettingForm to configure settings such that sending a setting, with SettingForm using its initial values,
        //    //  will change the sampling period in the running area to "100 Hz."
        //    //  Also see the GetSetting function.


        //    // Set upper limit
        //    _depth = 0x01;						                        // Setting depth: Running settings area
        //    _targetSetting.byType = (byte)(dispSensor.ProgramNo);		// Setting type: Program number
        //    _targetSetting.byCategory = 0x06;	                        // Category: Output settings
        //    _targetSetting.byItem = 0x0E;		                        // Setting item: Upper limit
        //    _targetSetting.byTarget1 = (byte)(dispSensor.OutputNo);		// Setting target 1: output number
        //    _targetSetting.byTarget2 = 0x0;		                        // Setting target 2: None
        //    _targetSetting.byTarget3 = 0x0;		                        // Setting target 3: None
        //    _targetSetting.byTarget4 = 0x0;		                        // Setting target 4: None
        //    SetLimit(upperLimit);

        //    uint dwError = 0;
        //    using (PinnedObject pin = new PinnedObject(_data))

        //        try
        //        {
        //            Rc rc = (Rc)NativeMethods.LJV7IF_SetSetting(_deviceID, _depth, _targetSetting, pin.Pointer, (uint)_data.Length, ref dwError);

        //            if (rc != Rc.Ok)
        //            {
        //                U.LogPopup("Cannot set upper limit on sensor");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            U.LogPopup(ex, "Keyence_LJ_V7001_Ctrl.SetLimit error");
        //        }





        //    // Set lower limit
        //    _depth = 0x01;						                        // Setting depth: Running settings area
        //    _targetSetting.byType = (byte)(dispSensor.ProgramNo);		// Setting type: Program number
        //    _targetSetting.byCategory = 0x06;	                        // Category: Output settings
        //    _targetSetting.byItem = 0x0E;		                        // Setting item: Upper limit
        //    _targetSetting.byTarget1 = (byte)(dispSensor.OutputNo);		// Setting target 1: output number
        //    _targetSetting.byTarget2 = 0x0;		                        // Setting target 2: None
        //    _targetSetting.byTarget3 = 0x0;		                        // Setting target 3: None
        //    _targetSetting.byTarget4 = 0x0;		                        // Setting target 4: None
        //    SetLimit(lowerLimit);


        //    using (PinnedObject pin = new PinnedObject(_data))

        //        try
        //        {
        //            Rc rc = (Rc)NativeMethods.LJV7IF_SetSetting(_deviceID, _depth, _targetSetting, pin.Pointer, (uint)_data.Length, ref dwError);

        //            if (rc != Rc.Ok)
        //            {
        //                U.LogPopup("Cannot set upper limit on sensor");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            U.LogPopup(ex, "Keyence_LJ_V7001_Ctrl.SetLimit error");
        //        }


        //}


        private static bool s_FirstTime = true;
        /// <summary>Callback function used during high-speed communication</summary>
        private HighSpeedDataCallBack _callback;

        private void ConnectDevicesAndController()
        {

            // initialize DLL
            try
            {
                Rc rc = Rc.Ok;
                if (s_FirstTime)
                {
                    rc = (Rc)NativeMethods.LJV7IF_Initialize();
                    uint uiVer = NativeMethods.LJV7IF_GetVersion();
                    s_FirstTime = false;
                }

                _callback = new HighSpeedDataCallBack(ReceiveHighSpeedData);
                TCPIP tcpip = GetParent<TCPIP>();

                LJV7IF_ETHERNET_CONFIG ethernetConfig = new LJV7IF_ETHERNET_CONFIG();
                if (tcpip != null)
                {
                    try
                    {
                        ethernetConfig.abyIpAddress = tcpip.ToByteArray();
                        ethernetConfig.wPortNo = tcpip.Port;
                    }
                    catch (Exception ex)
                    {
                        throw new ForceSimulateException(ex, "Failed to extract tcpip adresss for Keyence");
                    }
                }
                if (rc == Rc.Ok)
                {
                    foreach (MillimeterInput mmInput in _mmInputs)
                    {
                        if (tcpip != null)
                        {
                            rc = (Rc)NativeMethods.LJV7IF_EthernetOpen(mmInput.Channel, ref ethernetConfig);
                        }
                        else
                        {
                            rc = (Rc)NativeMethods.LJV7IF_UsbOpen(mmInput.Channel);
                        }
                        mmInput.Initialized = rc == Rc.Ok;
                        mmInput.Enabled = rc == Rc.Ok;
                        if (!mmInput.Initialized)
                        {
                            U.LogPopup("MillimeterInput '{0}' could not open USB connection.", mmInput.Nickname);
                        }
                    }
                    foreach (Array2DInput arrInput in _arrInputs)
                    {
                        if (tcpip != null)
                        {
                            rc = (Rc)NativeMethods.LJV7IF_EthernetOpen(arrInput.Channel, ref ethernetConfig);
                            NativeMethods.LJV7IF_ClearMemory(arrInput.Channel);
                            if (rc == Rc.Ok && HighSpeedCom)
                            {
                                rc = (Rc)NativeMethods.LJV7IF_HighSpeedDataEthernetCommunicationInitalize(
                                    arrInput.Channel, ref ethernetConfig, 24692, _callback, 10, (uint)arrInput.Channel
                                    );
                                if (rc == Rc.Ok)
                                {
                                    LJV7IF_HIGH_SPEED_PRE_START_REQ req = new LJV7IF_HIGH_SPEED_PRE_START_REQ();
                                    req.bySendPos = 0;
                                    // High-speed data communication start preparations
                                    LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();
                                    rc = (Rc)NativeMethods.LJV7IF_PreStartHighSpeedDataCommunication(arrInput.Channel, ref req, ref profileInfo);
                                    if (rc == Rc.Ok)
                                    {
                                        arrInput.NumCols = profileInfo.wProfDataCnt;
                                        rc = (Rc)NativeMethods.LJV7IF_StartHighSpeedDataCommunication(arrInput.Channel);
                                        if (rc != Rc.Ok)
                                        {
                                            U.LogError("Unable to StartHighSpeedDataCommunication: {0}", arrInput.Nickname);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            rc = (Rc)NativeMethods.LJV7IF_UsbOpen(arrInput.Channel);
                            NativeMethods.LJV7IF_ClearMemory(arrInput.Channel);
                        }

                        arrInput.Initialized = rc == Rc.Ok;
                        arrInput.Enabled = rc == Rc.Ok;
                        if (!arrInput.Initialized)
                        {
                            U.LogPopup("Array2DInput '{0}' could not open USB connection.", arrInput.Nickname);
                        }
                    }
                    Initialized = true;
                }
            }
            catch
            {
            }


            if (Initialized)
            {
                U.LogInfo("Controller Connected");
            }
            else
            {
                throw new ForceSimulateException("Failed to connect to Keyence");
            }

        }

        private void DisconnectDevicesAndController()
        {
            Rc rc = Rc.Ok;
            TCPIP tcpip = GetParent<TCPIP>();
            foreach (MillimeterInput mmInput in _mmInputs)
            {
                if (mmInput.Initialized)
                {
                    if (tcpip == null)
                    {
                        rc = (Rc)NativeMethods.LJV7IF_CommClose(mmInput.Channel);
                        if (rc != Rc.Ok)
                        {
                            U.LogPopup("MillimeterInput '{0}' could not close USB connection.", mmInput.Nickname);
                        }
                    }
                    mmInput.Initialized = false;
                    s_mmInputs.Remove(mmInput.Channel);
                }
            }
            foreach (Array2DInput arrInput in _arrInputs)
            {
                if (arrInput.Initialized)
                {
                    if (tcpip != null && HighSpeedCom)
                    {
                        NativeMethods.LJV7IF_StopHighSpeedDataCommunication(arrInput.Channel);
                        NativeMethods.LJV7IF_HighSpeedDataCommunicationFinalize(arrInput.Channel);
                    }
                    rc = (Rc)NativeMethods.LJV7IF_CommClose(arrInput.Channel);
                    if (rc != Rc.Ok)
                    {
                        U.LogPopup("Array2DInput '{0}' could not close USB connection.", arrInput.Nickname);
                    }
                    arrInput.Initialized = false;
                    s_arrInputs.Remove(arrInput.Channel);
                }
            }
            // Disconnect DLL
            if (s_arrInputs.Count == 0 && s_mmInputs.Count == 0)
            {
                rc = (Rc)NativeMethods.LJV7IF_Finalize();
            }
            if (rc != Rc.Ok)
            {
                U.LogPopup("Keyence DLL could not be disconnected");
            }
        }



        //private void OnChangeTriggerMode(CompMeasure.eTriggerMode triggerMode)
        //{
        //    switch (triggerMode)
        //    {
        //        case CompMeasure.eTriggerMode.Idle:
        //            {
        //                break;
        //            }
        //        case CompMeasure.eTriggerMode.SingleTrigger:
        //            {
        //                SendBatchStart();
        //                GetMeasurement();
        //                SendBatchStop();

        //                _dispSensor.TriggerMode = CompMeasure.eTriggerMode.Idle;
        //                break;
        //            }
        //        case CompMeasure.eTriggerMode.TimedTrigger:
        //            {
        //                break;
        //            }
        //        case CompMeasure.eTriggerMode.Live:
        //            {
        //                break;
        //            }
        //    }
        //}


        //private void SendBatchStart()
        //{
        //    try
        //    {
        //        Rc rc = (Rc)NativeMethods.LJV7IF_StartMeasure(_deviceID);
        //        if (rc != Rc.Ok)
        //        {
        //            U.LogPopup("Cannot send batch start to controller");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        U.LogError(ex, "Keyence_LJ_V7001.SendBatchStart");
        //    }
        //}

        //private void SendBatchStop()
        //{
        //    try
        //    {
        //        Rc rc = (Rc)NativeMethods.LJV7IF_StopMeasure(_deviceID);
        //        if (rc != Rc.Ok)
        //        {
        //            U.LogPopup("Cannot send batch stop to controller");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        U.LogError(ex, "Keyence_LJ_V7001.SendBatchStop");
        //    }

        //}


        //private void GetMeasurement()
        //{
        //    int _timeout_ms = 1000;
        //    DateTime stop = DateTime.Now.AddTicks(_timeout_ms * 10000);

        //    try
        //    {
        //        LJV7IF_MEASURE_DATA[] measureData = new LJV7IF_MEASURE_DATA[NativeMethods.MeasurementDataCount];
        //        Rc rc1 = (Rc)NativeMethods.LJV7IF_GetMeasurementValue(_deviceID, measureData);
        //        if (rc1 != Rc.Ok) return;


        //        while ((measureData[0].byDataInfo != 0) && (DateTime.Now < stop))
        //        {
        //            Rc rc = (Rc)NativeMethods.LJV7IF_GetMeasurementValue(_deviceID, measureData);
        //            if (rc != Rc.Ok) return;
        //            Application.DoEvents();
                   
        //        }
                
 
        //        MeasureData data = new MeasureData(measureData);

        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 0; i < NativeMethods.MeasurementDataCount; i++)
        //        {
        //            sb.Append(string.Format("OUT {0:d2}:\t{1,0:f4}\r\n", (i + 1), measureData[i].fValue));
        //        }

        //        //_dispSensor.ValueString = sb.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        U.LogError(ex, "Keyence_LJ_V7001.GetMeasurement");
        //    }
        //}


        #endregion Overrides


    }
}
