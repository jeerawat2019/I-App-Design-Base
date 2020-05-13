using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

using MCore;
using MCore.Comp;
using MCore.Comp.SMLib;
using MCore.Comp.SMLib.SMFlowChart;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using MCore.Comp.VisionSystem;
using MCore.Comp.MotionSystem;
using MCore.Comp.Communications;

using AppMachine.Panel;
using AppMachine.Comp.Vision;
using AppMachine.Comp.Station;
using AppMachine.Control;
using AppMachine.Comp.Recipe;
using AppMachine.Comp.CommonParam;
using AppMachine.Comp.Motion;
using AppMachine.Comp.Users;

using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Navigator;

namespace AppMachine
{
    public partial class AppMainForm : KryptonForm
    {

        #region Standard Pattern

        public static AppMainForm This = null;
        public AppMainPanel _mainPanel = null;
        private SMStateMachine _activeSM = null;
        private List<SMStateMachine> _allStateMachine = new List<SMStateMachine>();
        private List<SMStateMachine> _allSemiStateMachine = new List<SMStateMachine>();

        private AppChangeUserFrm _changeUserFrm = null;

        private SMStateMachine _smMain = null;
        private SMStateMachine _smReset = null;
        private SMStateMachine _smSemiAutoMain = null;
        private SMStateMachine _smSemiAutoReset = null;

        private AppLockOutFrm _lockoutFrm = null;

        private Color _operatorModeColor = Color.Navy;
        private Color _supervisorModeColor = Color.DarkRed;

        private System.Threading.Thread _splashThread = null;

        private System.Timers.Timer _autoLockoutTimer = new System.Timers.Timer();

        //Check SM Running
        private bool IsSMRunning
        {
            get
            {
                foreach (SMStateMachine sm in _allStateMachine)
                {
                    if (sm.IsRunning)
                    {
                        AppMachine.Comp.AppMachine.This.AnySMRunning = true;
                        return true;
                    }
                }
                AppMachine.Comp.AppMachine.This.AnySMRunning = false;
                return false;
            }
        }


        //Check SM Running
        private bool IsSMSemiRunning
        {
            get
            {
                foreach (SMStateMachine sm in _allSemiStateMachine)
                {
                    if (sm.IsRunning)
                    {
                        AppMachine.Comp.AppMachine.This.AnySMSemiRunning = true;
                        return true;
                    }
                }
                AppMachine.Comp.AppMachine.This.AnySMSemiRunning = false;
                return false;
            }
        }



        public AppMainForm()
        {
            This = this;
            _splashThread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowSplash));
            _splashThread.Start();
            InitializeComponent();
            _mainPanel = new AppMainPanel();
            _mainPanel.Dock = DockStyle.Fill;
            this.splitContainer.Panel1.Controls.Add(_mainPanel);
               
           

            if (!this.DesignMode)
            {
                U.RootComp.ApplicationSetup(this, @"C:\PDLib Machine Template\AppMachine");
            }
        }


        private void ShowSplash()
        {
            Application.Run(new AppSplashFrm());
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

            Task initialTask = new Task(() => InitializeApplication());
            initialTask.Start();
            do
            {
                Application.DoEvents();
            } while (!initialTask.IsCompleted);

            //Initialize Windows Message Filter
            CLMessageFilter.Instance();
            CheckForIllegalCrossThreadCalls = true;
            return;
        }

        private void DestroyAll()
        {
            U.LogInfo("We are going to Save and close!");
            try
            {
                if (_smMain.IsRunning)
                {
                    _smMain.Stop();
                    _smMain.Abort();
                }

                CompRoot.AppStatus("Pre-Destroy");
                U.RootComp.PreDestroy();
                U.RootComp.SaveSettings();
                CompRoot.AppStatus("Destroying Root");
                U.RootComp.Destroy();
            }
            catch (MCoreException ex)
            {
                U.Log(ex);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Destroy Problem");
            }
        }


        public virtual bool OnPreFilterMessage(ref Message m) { return false; }

        /// <summary>
        /// Responds to kepress
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool OnKeyDown(Keys key)
        {

            if (System.Windows.Forms.Control.ModifierKeys == (Keys.Control | Keys.Alt))
            {
                switch (key)
                {

                    case Keys.O:
                        if (_changeUserFrm == null)
                        {
                            _changeUserFrm = new AppChangeUserFrm();
                        }
                        _changeUserFrm.PerformChangeUser();
                        break;
                }

            }
            return false;
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            DialogResult result = AppUtility.ShowKryptonMessageBox("Confirm Closing", String.Format("Are you sure to Close?"), "", TaskDialogButtons.Yes | TaskDialogButtons.No, MessageBoxIcon.Question, this);

            if (result == DialogResult.Yes)
            {
                _autoLockoutTimer.Enabled = false;
                DestroyAll();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void RunAndWaitForSM()
        {
            UpdateControlButtons();

            if(_waithSMTask == null)
            {
                _waithSMTask = Task.Run(() => AsyncWaitSM());
            }
            
        }


        Task _waithSMTask = null;
        private void AsyncWaitSM()
        {
            //Ensure Correct GUI
            this.BeginInvoke((MethodInvoker)delegate { _mainPanel.AnySMRunningOnChange(true); });
            do
            {
                System.Threading.Thread.Sleep(100);
            } while (IsSMRunning);
            _activeSM = null;
            UpdateControlButtons();
            _waithSMTask = null;

            //Ensure Correct GUI
            this.BeginInvoke((MethodInvoker)delegate { _mainPanel.AnySMRunningOnChange(false); });
        }


        private void RunAndWaitForSMReset()
        {
            UpdateControlButtons();

            if (_waithSMTask == null)
            {
                _waithSMResetTask = Task.Run(() => AsyncWaitSMReset());
            }

        }

        Task _waithSMResetTask = null;
        private void AsyncWaitSMReset()
        {
            //Ensure Correct GUI
            this.BeginInvoke((MethodInvoker)delegate { _mainPanel.AnySMRunningOnChange(true); });

            do
            {
                System.Threading.Thread.Sleep(100);
            } 
            while (_smReset.IsRunning);
           
            _activeSM = null;
            UpdateControlButtons();
            _waithSMResetTask = null;

            //Ensure Correct GUI
            this.BeginInvoke((MethodInvoker)delegate { _mainPanel.AnySMRunningOnChange(false); });
            
        }


        private delegate void _delParamVoid();
        private void UpdateControlButtons()
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new _delParamVoid(UpdateControlButtons));
                return;
            }

            if (_activeSM == null && !AppMachine.Comp.AppMachine.This.HasReset)
            {
                btnRun.Text = "RUN";
                btnRun.Enabled = false;
                btnPause.Enabled = false;
                btnEStop.Enabled = false;
                btnHomeAll.Enabled = true;
                mcbStopWhenFinished.Enabled = false;
                btnApply.Enabled = true;
                AppMachine.Comp.AppMachine.This.RunStatus = Comp.AppMachine.eRunStatus.Stopped;
            }
            else if (_activeSM == null && AppMachine.Comp.AppMachine.This.HasReset)
            {
                AppMachine.Comp.AppMachine.This.StopWhenFinished = false;
                mcbStopWhenFinished.Enabled = false;
                btnRun.Text = "RUN";
                btnRun.Enabled = true;
                btnPause.Enabled = false;
                btnEStop.Enabled = false;
                btnHomeAll.Enabled = true;
                btnApply.Enabled = true;
                AppMachine.Comp.AppMachine.This.RunStatus = Comp.AppMachine.eRunStatus.Stopped;
            }
            else if (_activeSM != null && _activeSM == _smMain)
            {
                btnRun.Text = "RUNNING...";
                btnRun.Enabled = false;
                btnPause.Enabled = true;
                btnEStop.Enabled = true;
                btnHomeAll.Enabled = false;
                btnApply.Enabled = false;
                AppMachine.Comp.AppMachine.This.RunStatus = Comp.AppMachine.eRunStatus.Running;
            }

            else if (_activeSM != null && _activeSM == _smReset)
            {
                btnRun.Text = "HOMING...";
                btnRun.Enabled = false;
                btnPause.Enabled = false;
                btnEStop.Enabled = true;
                btnHomeAll.Enabled = false;
                btnApply.Enabled = false;
                AppMachine.Comp.AppMachine.This.RunStatus = Comp.AppMachine.eRunStatus.Running;
            }

            btnPause.Text = "PAUSE";
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            U.RootComp.SaveSettings();
            CompRoot.AppStatus("All Setting Saved");
        }

        private void btnStartRun_Click(object sender, EventArgs e)
        {
            StopAllSemiStateMachine();

            mcbStopWhenFinished.Enabled = true;
            _activeSM = _smMain;

            List<Task> smStartTask = new List<Task>();
            foreach (SMStateMachine smState in _allStateMachine)
            {
                if (smState.Name != AppConstStaticName.SM_RESET)
                {
                    smStartTask.Add(new Task(() => smState.Go()));
                }
            }
            Parallel.ForEach<Task>(smStartTask, (sm) => sm.Start());

            RunAndWaitForSM();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            StopAllSemiStateMachine();

            if (_activeSM != null)
            {
                if (btnPause.Text == "PAUSE")
                {
                    foreach (SMStateMachine sm in _allStateMachine)
                    {
                        if (sm.IsRunning)
                        {
                            sm.Pause();
                        }
                    }
                   
                    btnPause.Text = "CONTINUE";
                    mcbStopWhenFinished.Enabled = false;
                    AppMachine.Comp.AppMachine.This.RunStatus = Comp.AppMachine.eRunStatus.Pause;
                }
                else if (btnPause.Text == "CONTINUE")
                {           
                    foreach (SMStateMachine sm in _allStateMachine)
                    {
                        if (sm.IsRunning)
                        {
                            sm.Go();
                        }
                    }
                    
                    btnPause.Text = "PAUSE";
                    mcbStopWhenFinished.Enabled = true;
                    AppMachine.Comp.AppMachine.This.RunStatus = Comp.AppMachine.eRunStatus.Running;
                }
        
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            StopAllSemiStateMachine();

            AppMachine.Comp.AppMachine.This.StopWhenFinished = true;
            do
            {
                foreach (SMStateMachine smState in _allStateMachine)
                {
                    if (smState.IsRunning && smState.Mode == SMStateMachine.eMode.Pause)
                    {
                        smState.Mode = SMStateMachine.eMode.Run;
                    }
                    smState.Stop();
                }
                Application.DoEvents();
            } while (IsSMRunning);

            _activeSM = null;
            AppMachine.Comp.AppMachine.This.StopWhenFinished = false;
            UpdateControlButtons();
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            StopAllSemiStateMachine();
            _activeSM = _smReset;
            _activeSM.Go();
            RunAndWaitForSMReset();
        }

        private void mcbStopWhenFinished_CheckedChanged(object sender, EventArgs e)
        {
            StopAllSemiStateMachine();
            AppMachine.Comp.AppMachine.This.StopWhenFinished = mcbStopWhenFinished.Checked;
            mcbStopWhenFinished.Text = AppMachine.Comp.AppMachine.This.StopWhenFinished ? "STOPPING..." : "STOP";
            if (mcbStopWhenFinished.Text == "STOPPING...")
            {
                AppMachine.Comp.AppMachine.This.RunStatus = Comp.AppMachine.eRunStatus.Stopping;
            }
        }

        private void GetSMInstanceWithBindControl()
        {

            _smReset = U.GetComponent(AppConstStaticName.SM_RESET) as SMStateMachine;
            _smMain = U.GetComponent(AppConstStaticName.SM_MAIN) as SMStateMachine;
            CompBase allState = U.GetComponent(AppConstStaticName.ALL_STATE_MACHINE);
            foreach (CompBase childState in allState.ChildArray)
            {
                SMStateMachine sm = childState as SMStateMachine;
                if (sm != null)
                {
                    _allStateMachine.Add(sm);
                    _mainPanel.AddStateMachinePage(sm);
                }
            }
        }


        private void GetSMSemiAutoInstanceWithBindControl()
        {

            _smSemiAutoReset = U.GetComponent(AppConstStaticName.SM_SEMI_RESET) as SMStateMachine;
            _smSemiAutoMain = U.GetComponent(AppConstStaticName.SM_SEMI_MAIN) as SMStateMachine;
            CompBase allSemiAutoState = U.GetComponent(AppConstStaticName.ALL_SEMI_AUTO_STATE_MACHINE);
            foreach (CompBase childState in allSemiAutoState.ChildArray)
            {
                SMStateMachine sm = childState as SMStateMachine;
                if (sm != null)
                {
                    _allSemiStateMachine.Add(sm);
                    _mainPanel.AddSemiStateMachinePage(sm);
                }
            }
        }
        #endregion


        private void InitializeApplication()
        {

            try
            {


                #region  Machine Standard Pattern
                // Essential
                U.RootComp.Add(new DefaultLogger(AppConstStaticName.DEFAULT_LOGGER));
                // Force load of certain references
                Type ty = typeof(SMFlowChartCtlBasic);
                //Machine
                ComponentDefinition machineDef = new ComponentDefinition(typeof(AppMachine.Comp.AppMachine), AppConstStaticName.APP_MACHINE);
                //Essential add Machine to Root

                #endregion

                #region Standard Machine Common Param Pattern
                U.RootComp.Add(new AppCommonParam(AppConstStaticName.COMMON_PARAMETER));
                #endregion

                #region Station Standard Pattern
                //Station
                CompBase stations = new CompBase(AppConstStaticName.ALL_STATIONS);
                #endregion

                //Station Define
                /*Add New Station Here (Example in Below)
                 stations.Add(new AppFeederStation(AppConstStaticName.FEEDSTATION));
                 stations.Add(new AppVisionStation(AppConstStaticName.VISIONSTATION)); 
                 */

                stations.Add(new AppStationBase("Sample Station"));

                #region Station Standard Pattern
                U.RootComp.Add(stations, CompRoot.eCleanUpMode.AllLayer);
                #endregion


                //IO System Define
                //Add IO Component Here (Example in Below) 
                //ComponentDefinition ioSystem = new ComponentDefinition(IOSystemBase.PlugIns.ModbusTcpIO, AppConstStaticName.ALL_IO);
                ComponentDefinition ioSystem = new ComponentDefinition(typeof(IOSystemBase),"ModbusTcpIO", AppConstStaticName.ALL_IO);
                Inputs inputsIO = new Inputs(AppConstStaticName.INPUTS);
                ComponentDefinition inputs = ioSystem.Add(inputsIO);


                //Inputs List Define
                /* Add IO Component Here (Example in Below) 
                inputs.Add(new AppSafetyInput(AppConstStaticName.STARTPB) { Channel = 0 });//X1
                inputs.Add(new AppSafetyInput(AppConstStaticName.STOPPB) { Channel = 1 });//X2
                inputs.Add(new AppSafetyInput(AppConstStaticName.RESETPB) { Channel = 2 });//X3
                */
                

                Outputs outputsIO = new Outputs(AppConstStaticName.OUTPUTS);
                ComponentDefinition outputs = ioSystem.Add(outputsIO);

                //Outputs List Define
                /*
                outputs.Add(new BoolOutput(AppConstStaticName.REDLIGHT) { Channel = 0 });
                outputs.Add(new BoolOutput(AppConstStaticName.AMBERLIGHT) { Channel = 1 });
                outputs.Add(new BoolOutput(AppConstStaticName.GREENLIGHT) { Channel = 2 });
                outputs.Add(new BoolOutput(AppConstStaticName.BUZZER) { Channel = 3 });
                */

                try
                {
                    U.RootComp.Add(ioSystem, CompRoot.eCleanUpMode.AllLayer);
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "IO System Initialize Unsuccessful in loading of application");
                }




                //Vision System Define
                /* Add Vision Conponent Here (Example in Below)
                //Vision
                ComponentDefinition visionSys = machineDef.Add(VisionSystemBase.PlugIns.Cognex9, AppConstStaticName.VISIONSYSTEM);
                ComponentDefinition camAOI = visionSys.Add(CameraBase.PlugIns.CognexCamera9, AppConstStaticName.VISIONCAMERA);
                ComponentDefinition aoiJob = camAOI.Add(VisionJobBase.PlugIns.CognexJob9, AppConstStaticName.VISIONJOB);
                aoiJob.Add(new AppVisionAOIResult(AppConstStaticName.VISIONJOBRESULT));
                */



                //Motion System Define
                /* Add Motion Conponent Here (Example in Below)
                
                //Feed Motion
                ComponentDefinition feedMotionSys = machineDef.Add(MotionSystemBase.PlugIns.YamahaTrServo, AppConstStaticName.FEEDMOTIONSYSTEM);
                ComponentDefinition feedMotionAxes = feedMotionSys.Add(new AppRealAxes(AppConstStaticName.FEEDMOTIONAXES));
                ComponentDefinition feedYAxis = feedMotionAxes.Add(new AppRealAxis(AppConstStaticName.FEEDYAXIS) { AxisNo = 0, MaxLimit = 75, MinLimit = 0, DefaultSpeed = 70.0, AccelDecel = 100.0 });
                
                //Lift1 Motion
                ComponentDefinition lift1MotionSys = machineDef.Add(MotionSystemBase.PlugIns.IAIScon, AppConstStaticName.LIFT1MOTIONSYSTEM);
                ComponentDefinition lift1MotionAxes = lift1MotionSys.Add(new AppRealAxes(AppConstStaticName.LIFT1MOTIONAXES));
                ComponentDefinition lift1ZAxis = lift1MotionAxes.Add(new AppRealAxis(AppConstStaticName.LIFT1ZAXIS) { AxisNo = 0, MaxLimit = 75, MinLimit = 0, DefaultSpeed = 70.0, AccelDecel = 0.1  });

                //Lift2 Motion
                ComponentDefinition lift2MotionSys = machineDef.Add(MotionSystemBase.PlugIns.IAIScon, AppConstStaticName.LIFT2MOTIONSYSTEM);
                ComponentDefinition lift2MotionAxes = lift2MotionSys.Add(new AppRealAxes(AppConstStaticName.LIFT2MOTIONAXES));
                ComponentDefinition lift2ZAxis = lift2MotionAxes.Add(new AppRealAxis(AppConstStaticName.LIFT2ZAXIS) { AxisNo = 0, MaxLimit = 75, MinLimit = 0, DefaultSpeed = 70.0, AccelDecel = 0.1 });
               */


                #region State Machine Standard Pattern
                //Define Stae Machine Component
                ComponentDefinition smDef = new ComponentDefinition(typeof(CompBase), AppConstStaticName.ALL_STATE_MACHINE);

                smDef.Add(new SMStateMachine(AppConstStaticName.SM_RESET));
                smDef.Add(new SMStateMachine(AppConstStaticName.SM_MAIN));
                #endregion

                //State Machine Define
                /* Add New State Machine Here (Example in Below)
                smDef.Add(new SMStateMachine(AppConstStaticName.SM_FEED));
                smDef.Add(new SMStateMachine(AppConstStaticName.SM_VISION));
                */


                #region State Semi-Auto Machine Standard Pattern
                //Define Stae Machine Component
                ComponentDefinition smSemiAutoDef = new ComponentDefinition(typeof(CompBase), AppConstStaticName.ALL_SEMI_AUTO_STATE_MACHINE);
                smSemiAutoDef.Add(new SMStateMachine(AppConstStaticName.SM_SEMI_RESET));
                smSemiAutoDef.Add(new SMStateMachine(AppConstStaticName.SM_SEMI_MAIN));
                #endregion

                //Semi State Machine Define
                /* Add New State Machine Here (Example in Below)
                smSemiAutoDef.Add(new SMStateMachine(AppConstStaticName.SM_SEMI_FEED));
                smSemiAutoDef.Add(new SMStateMachine(AppConstStaticName.SM_SEMI_VISION));
                */


                #region State Machine Standard Pattern
                U.RootComp.Add(smDef, CompRoot.eCleanUpMode.FirstLayer);
                #endregion

                #region Semi Auto State Machine Standard Pattern
                U.RootComp.Add(smSemiAutoDef, CompRoot.eCleanUpMode.FirstLayer);
                #endregion

                #region Recipe Standard Pattern
                ComponentDefinition recipeDef = new ComponentDefinition(typeof(CompBase), AppConstStaticName.ALL_RECIPES);
                recipeDef.Add(new AppProductRecipe(AppConstStaticName.REF_CURRENT_RECIPE));
                recipeDef.Add(new AppProductRecipe(AppConstStaticName.SAMPLE_RECIPE));
                U.RootComp.Add(recipeDef);
                #endregion

                #region User Standard Pattern
                ComponentDefinition users = new ComponentDefinition(typeof(CompBase), AppConstStaticName.ALL_USER);
                AppUserInfo adminUser = new AppUserInfo(AppConstStaticName.ADMIN_USER) {UserEN = "00000", UserCode = "00000", UserLevel = Comp.AppEnums.eAccessLevel.Supervisor};
                users.Add(adminUser );
                AppUserInfo guestUser = new AppUserInfo(AppConstStaticName.GUEST_USER) { UserEN = "00001", UserCode = "00001", UserLevel = Comp.AppEnums.eAccessLevel.Operator };
                users.Add(adminUser);
                users.Add(guestUser);

                U.RootComp.Add(users);
                #endregion

                #region Standard Pattern

                try
                {
                    //Add machine to root comp after add all components belong to the machine.
                    U.RootComp.Add(machineDef);
                    AppMachine.Comp.AppMachine.This.IOList.Initialize();
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Machine Initialize Unsuccessful in loading of application");
                }


                // Essential call after all component definitions
                U.RootComp.InitializeIDReferences();

                this.BeginInvoke((MethodInvoker)delegate
                {
                    _mainPanel.RebuildCompBrowser();
                    

                   // //State Machine Instance with bind to control
                    GetSMInstanceWithBindControl();
                    

                   // //Semi Auto State Machine Instance with bind to control
                    GetSMSemiAutoInstanceWithBindControl();
                    

                    //Initiate User
                    if (AppMachine.Comp.AppMachine.This.CurrentUser == null)
                    {
                        AppMachine.Comp.AppMachine.This.CurrentUser = U.GetComponent(AppConstStaticName.ADMIN_USER) as AppUserInfo;
                    }

                    //Prepare Stop When Finish
                    mcbStopWhenFinished.DataBindings.Add("Checked", AppMachine.Comp.AppMachine.This, "StopWhenFinished");
                  
                    //Prepare All Panel
                    _mainPanel.PrepareAllPanel();
                   

                    //Update Operation Button
                    UpdateControlButtons();

                    //Set User Access for Current User
                    SetUserAccess(AppMachine.Comp.AppMachine.This.CurrentUser);

                });

               
                U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.CurrentUser, CurrentUserOnChange);

                AppMachine.Comp.AppMachine.This.LoadStartupRecipe();
                VerifyCriticalSimulateComponent();
                _mainPanel.RegisterMachineStatusEvent();
                #endregion

               
            }

            #region Standard Pattern
            catch (Exception ex)
            {
                U.LogPopup(ex + " "+ex.StackTrace, "Unsuccessful in loading of application");
            }
            finally
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    _splashThread.Abort();
                });
            }
            #endregion


            #region Auto Lockout Standard Pattern
            _autoLockoutTimer.Interval = 500;
            _autoLockoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(AutoLockoutTimer_OnElapse);
            _autoLockoutTimer.Enabled = true;
            #endregion
        }


        
        private void VerifyCriticalSimulateComponent()
        {
         #region Standard Pattern
            AppMachine.Comp.AppMachine machine =  AppMachine.Comp.AppMachine.This;
            #endregion

          //Critical Component define
          /* Add Critical Component name to verify Here (Example in Below)
          machine.CriticalCompList.Add(U.GetComponent(AppConstStaticName.ALLWAGOIO));
          machine.CriticalCompList.Add(U.GetComponent(AppConstStaticName.FEEDMOTIONSYSTEM));
          machine.CriticalCompList.Add(U.GetComponent(AppConstStaticName.LIFT1MOTIONSYSTEM));
          machine.CriticalCompList.Add(U.GetComponent(AppConstStaticName.LIFT2MOTIONSYSTEM));
          machine.CriticalCompList.Add(U.GetComponent(AppConstStaticName.VISIONSYSTEM));
          */

         #region Standard Pattern
          string simulatedCompList = "";
          bool anySimulated = false;
          foreach(CompBase comp in machine.CriticalCompList)
          {
              if(comp.Simulate != CompBase.eSimulate.None)
              {
                  simulatedCompList += String.Format("Component \"{0}\" is simulted"+Environment.NewLine,comp.Nickname);
                  anySimulated = true;
              }
          }

          if(anySimulated)
          {
              this.BackColor = Color.Red;
              simulatedCompList += "Machine Unable to Start.";
              btnRun.Visible = false;
              btnPause.Visible = false;
              mcbStopWhenFinished.Visible = false;
              btnEStop.Visible = false;
              btnHomeAll.Visible = false;
              machine.Simulate = CompBase.eSimulate.SimulateDontAsk;

              AppUtility.ShowKryptonMessageBox("Machine Unable to Start", "Some Component are Simulated", simulatedCompList, TaskDialogButtons.OK, MessageBoxIcon.Information, this);
              
          }
          #endregion


        }


        private void AutoLockoutTimer_OnElapse(object sender, ElapsedEventArgs e)
        {
            if (CLMessageFilter.InstanceObj != null)
            {
                TimeSpan timeOutTs = new TimeSpan(0, 0, AppMachine.Comp.AppMachine.This.AutoLockoutTime.ToInt);
                TimeSpan lockoutCountTs = timeOutTs - CLMessageFilter.InstanceObj.UserActionTimeSpan;

                String lockoutTime = String.Format("{0}:{1}:{2}", lockoutCountTs.Hours.ToString("00"),
                                                                  lockoutCountTs.Minutes.ToString("00"),
                                                                  lockoutCountTs.Seconds.ToString("00"));
                if(lockoutCountTs.TotalSeconds <=0)
                {
                    lockoutTime = "00:00:00";
                }

                this.BeginInvoke((MethodInvoker)delegate
                {
                    lblLockoutTime.Text = lockoutTime;
                });

                if (lockoutCountTs.TotalSeconds <= 0)
                {
                    CLMessageFilter.InstanceObj.LastUserMessage = DateTime.Now;
                    if(AppMachine.Comp.AppMachine.This.CurrentUser.UserLevel ==  Comp.AppEnums.eAccessLevel.Supervisor)
                    {
                        AppUserInfo guestUser = U.GetComponent(AppConstStaticName.GUEST_USER) as AppUserInfo;
                        AppMachine.Comp.AppMachine.This.CurrentUser = guestUser;
                    }
                    btnLockOut_Click(null, null);
                }
            }

        }


        #region Standard Pattern
        


        private void btnLockOut_Click(object sender, EventArgs e)
        {
            MethodInvoker mi = (MethodInvoker)delegate
            {
                if (_lockoutFrm == null)
                {
                    _lockoutFrm = new AppLockOutFrm();
                }

                if (!_lockoutFrm.Visible)
                {
                    _lockoutFrm.ShowDialog(this);
                }
            };

            this.BeginInvoke(mi);
        }

        
		
	    #region Standard Pattern
        private void CurrentUserOnChange(AppUserInfo user)
        {
            SetUserAccess(user);
        }
        #endregion
		
		
       private void SetUserAccess(AppUserInfo user)
        {
            switch(user.UserLevel)
            {
                case Comp.AppEnums.eAccessLevel.Operator:
                    SetOperatorAccess();
                    break;
                case Comp.AppEnums.eAccessLevel.Supervisor:
                    SetSupervisorAccess();
                    break;
                default:
                    break;
            }
        }


        private void SetOperatorAccess()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                _mainPanel.SetOperatorAccess();

                //Change Form Color
                this.StateCommon.Back.Color1 = _operatorModeColor;
                this.StateCommon.Back.Color2 = _operatorModeColor;
                this.StateCommon.Header.Back.Color1 = _operatorModeColor;
                this.StateCommon.Header.Back.Color2 = _operatorModeColor;
                splitContainer.BackColor = _operatorModeColor;
                splitContainer.Panel1.BackColor = _operatorModeColor;
                splitContainer.Panel2.BackColor = _operatorModeColor;
            });
        }

        private void SetSupervisorAccess()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                _mainPanel.SetSupervisorAccess();

                //Change Form Color
                this.StateCommon.Back.Color1 = _supervisorModeColor;
                this.StateCommon.Back.Color2 = _supervisorModeColor;
                this.StateCommon.Header.Back.Color1 = _supervisorModeColor;
                this.StateCommon.Header.Back.Color2 = _supervisorModeColor;
                splitContainer.BackColor = _supervisorModeColor;
                splitContainer.Panel1.BackColor = _supervisorModeColor;
                splitContainer.Panel2.BackColor = _supervisorModeColor;
            });
        }

        private void StopAllSemiStateMachine()
        {
            do
            {
                foreach (SMStateMachine smState in _allSemiStateMachine)
                {
                    smState.Stop();
                }
                Application.DoEvents();
            } while (IsSMSemiRunning);
        }

        #endregion



    }


    #region Hot Key Standard Pattern 
    public class CLMessageFilter : IMessageFilter
    {
        private DateTime _lastUserMessage = DateTime.Now;
        public DateTime LastUserMessage
        {
            get { return _lastUserMessage; }
            set { _lastUserMessage = value; }
        }


        private static CLMessageFilter _instance = null;

        public static CLMessageFilter InstanceObj
        {
            get { return CLMessageFilter._instance; }
        }

        public TimeSpan UserActionTimeSpan;

        /// <summary>
        /// Create an instance of the filter
        /// </summary>
        public static void Instance()
        {
            if (_instance == null)
            {
                _instance = new CLMessageFilter();
                Application.AddMessageFilter(_instance);
            }
        }



        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_MOUSELBTNDOWN = 0x0201;
        public const int WM_MOUSERBTNDOWN = 0x0204;
        public const int WM_MOUSEMBTNDOWN = 0x0207;
        public const int WM_MOUSEXBTNDOWN = 0x020B;

        /// <summary>
        /// This is called for all Windows messages
        /// </summary>
        /// <param name="m">Incoming message passes by Windows</param>
        /// <returns>true if we have processed the message on our own</returns>
        public bool PreFilterMessage(ref Message m)
        {
            // Give application a chance at it
            if (AppMainForm.This != null && AppMainForm.This.OnPreFilterMessage(ref m))
            {
                return true;
            }
            System.Threading.Thread.Sleep(0);

            System.Windows.Forms.Control ctl = System.Windows.Forms.Control.FromHandle(m.HWnd);

            if (m.Msg == U.WM_KEYDOWN)
            {
                LastUserMessage = DateTime.Now;
                Keys keys = (Keys)m.WParam.ToInt32();

                ctl = System.Windows.Forms.Control.FromHandle(m.HWnd);

                if (AppMainForm.This != null)
                    return AppMainForm.This.OnKeyDown(keys);

            }
            else if (m.Msg == WM_MOUSEMOVE || m.Msg == WM_MOUSELBTNDOWN || m.Msg == WM_MOUSERBTNDOWN || m.Msg == WM_MOUSEMBTNDOWN || m.Msg == WM_MOUSEXBTNDOWN)
            {
                LastUserMessage = DateTime.Now;
            }

            UserActionTimeSpan = new TimeSpan(DateTime.Now.Ticks - LastUserMessage.Ticks);
            

            return false;
        }
    }
    #endregion Standard Pattern
}
