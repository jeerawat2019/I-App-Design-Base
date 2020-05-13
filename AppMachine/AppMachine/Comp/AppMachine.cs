using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.IO;


using MCore;
using MCore.Comp;

using AppMachine.Comp;
using AppMachine.Panel;
using AppMachine.Comp.Recipe;
using AppMachine.Comp.IO;
using AppMachine.Comp.Users;

using MCore.Comp.IOSystem.Output;


namespace AppMachine.Comp
{
    public class AppMachine : CompMachine
    {
        //private BoolOutput _redLamp = null;
        //private BoolOutput _amberLamp = null;
        //private BoolOutput _greenLamp = null;
        //private BoolOutput _buzzer = null;

        private AppProductRecipe _refCurrentRecipe = null;
        private CompBase _allRecipe = null;

        public enum eRunStatus
        {
            None,
            Running,
            Pause,
            Stopping,
            Stopped,
        }

        [XmlIgnore]
        public List<CompBase> CriticalCompList = new List<CompBase>();


        [XmlIgnore]
        public AppIOCollection IOList = new AppIOCollection();

        /// <summary>
        /// Self
        /// </summary>
        [XmlIgnore]
        public static AppMachine This = null;


        [Category("General"), Browsable(false), Description("Current Recipe")]
        [XmlIgnore]
        public AppProductRecipe CurrentRecipe
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => CurrentRecipe, null); }
            [StateMachineEnabled]
            set { SetPropValue(() => CurrentRecipe, value); }
        }
        


        /// <summary>
        /// CurrentProdRecipe
        /// </summary>
        [Category("General"), Browsable(true), Description("Current Product Recipe")]
        public string CurrentProdRecipeName
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => CurrentProdRecipeName,""); }

            [StateMachineEnabled]
            set { SetPropValue(() => CurrentProdRecipeName, value); }
        }


        /// <summary>
        /// CurrentUser
        /// </summary>
        [Category("General"), Browsable(true), Description("Current User")]
        public AppUserInfo CurrentUser
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => CurrentUser, null); }

            [StateMachineEnabled]
            set { SetPropValue(() => CurrentUser, value); }
        }


        /// <summary>
        /// Auto Lockout Time
        /// </summary>
        [Category("General"), Browsable(true), Description("Auto Lockout Time")]
        public MDouble.Seconds AutoLockoutTime
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => AutoLockoutTime,600); }

            [StateMachineEnabled]
            set { SetPropValue(() => AutoLockoutTime, value); }
        }


        /// <summary>
        /// IsNestedCondition
        /// </summary>
        [XmlIgnore]
        [Category("General"), Browsable(true), Description("Is Nestest Condition")]
        public bool IsNestedCondition
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => IsNestedCondition, false); }

            [StateMachineEnabled]
            set { SetPropValue(() => IsNestedCondition, value); }
        }

        /// <summary>
        /// Has Reset
        /// </summary>
        [Category("General"), Browsable(true), Description("Has reset")]
        [XmlIgnore]
        public bool HasReset
        {
            [StateMachineEnabled]
            get 
            {
                System.Threading.Thread.Sleep(100);
                return GetPropValue(() => HasReset, false); 
            }
            [StateMachineEnabled]
            set { SetPropValue(() => HasReset, value); }
        }

        /// <summary>
        /// Run Status
        /// </summary>
        [Category("General"), Browsable(true), Description("RunStatus")]
        [XmlIgnore]
        public eRunStatus RunStatus
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RunStatus, eRunStatus.None); }
            [StateMachineEnabled]
            set { SetPropValue(() => RunStatus, value); }
        }

        [Category("General"), Browsable(true), Description("Is Running")]
        [XmlIgnore]
        public bool IsRuning
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => IsRuning,false); }
            [StateMachineEnabled]
            set { SetPropValue(() => IsRuning, value); }
        }


        [Category("General"), Browsable(true), Description("Is Pause")]
        [XmlIgnore]
        public bool IsPause
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => IsPause, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => IsPause, value); }
        }


        [Category("General"), Browsable(true), Description("Is Stopping")]
        [XmlIgnore]
        public bool IsStopping
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => IsStopping, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => IsStopping, value); }
        }

        [Category("General"), Browsable(true), Description("Is Stopped")]
        [XmlIgnore]
        public bool IsStopped
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => IsStopped, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => IsStopped, value); }
        }


        [Category("General"), Browsable(true), Description("Is Alarm")]
        [XmlIgnore]
        public bool IsAlarm
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => IsAlarm, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => IsAlarm, value); }
        }



        /// <summary>
        /// Stop when the current operation is finished
        /// </summary>
        [Category("General"), Browsable(true), Description("Stop when the curremt operation is finished")]
        [XmlIgnore]
        public bool StopWhenFinished
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => StopWhenFinished, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => StopWhenFinished, value); }
        }



        /// <summary>
        /// Input Carrier
        /// </summary>
        [Category("Output"), Browsable(true), Description("Input Product")]
        [XmlIgnore]
        public int InputsProduct
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => InputsProduct, 0); }
            [StateMachineEnabled]
            set 
            { 
                SetPropValue(() => InputsProduct, value);
                Yield = (((double)InputsProduct - (double)FailedProduct) / (double)InputsProduct) * 100.00;
            }
        }


        /// <summary>
        /// Fail Carrier
        /// </summary>
        [Category("Output"), Browsable(true), Description("Failed Product")]
        [XmlIgnore]
        public int FailedProduct
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => FailedProduct, 0); }
            [StateMachineEnabled]
            set 
            { 
                SetPropValue(() => FailedProduct, value);
                Yield = (((double)InputsProduct - (double)FailedProduct) / (double)InputsProduct) * 100.00;
            }
        }


        /// <summary>
        /// Yield
        /// </summary>
        [Category("Output"), Browsable(true), Description("Yield")]
        [XmlIgnore]
        public double Yield
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Yield, 0); }
            [StateMachineEnabled]
            set { SetPropValue(() => Yield, value); }
        }


        /// <summary>
        /// Lot Id
        /// </summary>
        [Category("Input"), Browsable(true), Description("Lot Id")]
        [XmlIgnore]
        public string LotId
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => LotId, "A-001"); }
            [StateMachineEnabled]
            set { SetPropValue(() => LotId, value); }
        }

        /// <summary>
        /// Lot Size
        /// </summary>
        [Category("Input"), Browsable(true), Description("Lot Size")]
        [XmlIgnore]
        public int LotSize
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => LotSize, 200); }
            [StateMachineEnabled]
            set { SetPropValue(() => LotSize, value); }
        }

        
        /// <summary>
        /// Any SM Running
        /// </summary>
        [Category("General"), Browsable(true), Description("Any SM Running ")]
        [XmlIgnore]
        public bool AnySMRunning
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => AnySMRunning,false); }
            [StateMachineEnabled]
            set { SetPropValue(() => AnySMRunning, value); }
        }


        /// <summary>
        /// Any SM Semi Running
        /// </summary>
        [Category("General"), Browsable(true), Description("Any SM Semi Running")]
        [XmlIgnore]
        public bool AnySMSemiRunning
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => AnySMSemiRunning, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => AnySMSemiRunning, value); }
        }
       
        /// <summary>
        /// Report Path
        /// </summary>
        [System.ComponentModel.Editor(
        typeof(System.Windows.Forms.Design.FolderNameEditor),
        typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Report"), Browsable(true), Description("Report Path")]
        public string ReportPath
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ReportPath,"C:\\PDLib\\AppMachine\\Report"); }
            
            set { SetPropValue(() => ReportPath, value); }
        }


        /// <summary>
        /// Save Good Image
        /// </summary>
        [Category("Report"), Browsable(true), Description("Save Good Image")]
        public bool SaveGoodImage
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => SaveGoodImage, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => SaveGoodImage, value); }
        }

        /// <summary>
        /// Save NG Image
        /// </summary>
        [Category("Report"), Browsable(true), Description("Save NG Image")]
        public bool SaveNGImage
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => SaveNGImage, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => SaveNGImage, value); }
        }


        /// <summary>
        /// Semi Auto Main Param
        /// </summary>
        [Category("Semi Auto"), Browsable(true), Description("Semi Auto Main Param")]
        public String SemiAutoMainParam
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => SemiAutoMainParam, "None"); }
            [StateMachineEnabled]
            set { SetPropValue(() => SemiAutoMainParam, value); }
        }


        /// <summary>
        /// Semi Auto Main Param
        /// </summary>
        [Category("Semi Auto"), Browsable(true), Description("Semi Auto Reset Param")]
        public String SemiAutoResetParam
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => SemiAutoResetParam, "None"); }
            [StateMachineEnabled]
            set { SetPropValue(() => SemiAutoResetParam, value); }
        }

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppMachine()
        {
            This = this;
        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppMachine(string name)
            : base(name)
        {
            This = this;
        }
        #endregion Constructors

        public override void Initialize()
        {
            base.Initialize();

            IOList.Initialize();

           
        }

      
        /// <summary>
        /// Increment Carrier
        /// </summary>
        [StateMachineEnabled]
        public void IncrementInput()
        {
            InputsProduct ++;
        }


        /// <summary>
        /// Reset Carrier
        /// </summary>
        [StateMachineEnabled]
        public void ResetInput()
        { 
            InputsProduct = 0;
        }


        /// <summary>
        /// Increment Carrier
        /// </summary>
        [StateMachineEnabled]
        public void IncrementFailProduct()
        {
            FailedProduct++;
        }


        /// <summary>
        /// Reset Fail Carrier
        /// </summary>
        [StateMachineEnabled]
        public void ResetFailProduct()
        {
            FailedProduct = 0;
        }

        /// <summary>
        /// Reset Yield
        /// </summary>
        [StateMachineEnabled]
        public void ResetYield()
        {
            Yield = 0.0;
        }


         /// <summary>
        /// Called after Initialize
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();

            /*Example Get Component
            _redLamp = U.GetComponent(AppConstStaticName.RED_TOWER_LIGHT) as BoolOutput;
            _amberLamp = U.GetComponent(AppConstStaticName.AMBER_TOWER_LIGHT) as BoolOutput;
            _greenLamp = U.GetComponent(AppConstStaticName.GREEN_TOWER_LIGHT) as BoolOutput;
            _buzzer = U.GetComponent(AppConstStaticName.BUZZER) as BoolOutput;
             */

            _allRecipe = U.GetComponent(AppConstStaticName.ALL_RECIPES);
            _refCurrentRecipe = U.GetComponent(AppConstStaticName.REF_CURRENT_RECIPE) as AppProductRecipe;

            U.RegisterOnChanged(() => RunStatus, RunStatusOnChange);
            U.RegisterOnChanged(() => IsAlarm, AlarmOnChange);

            U.RegisterOnChanged(() => LotId, LotIDOnChanged);
            U.RegisterOnChanged(() => AppMachine.This.CurrentProdRecipeName, CurrentRecipeOnChange);
        }

        /// <summary>
        /// GC Collect
        /// </summary>
        [StateMachineEnabled]
        public void GCCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


        #region State Machine Enable Method
        

       

      
        #endregion


        private void RunStatusOnChange(eRunStatus status)
        {
            switch (status)
            {
                case eRunStatus.Running:
                    IsRuning = true;
                    IsPause = false;
                    IsStopping = false;
                    IsStopped = false;

                    ////_redLamp.SetFalse();
                    ////_amberLamp.SetFalse();
                    ////_greenLamp.SetTrue();
                    //_buzzer.SetFalse();

                    break;
                case eRunStatus.Pause:
                    IsRuning = false;
                    IsPause = true;
                    IsStopping = false;
                    IsStopped = false;

                    ////_redLamp.SetTrue();
                    ////_amberLamp.SetFalse();
                    ////_greenLamp.SetTrue();
                    //_buzzer.SetFalse();

                    break;
                case eRunStatus.Stopping:
                    IsRuning = false;
                    IsPause = false;
                    IsStopping = true;
                    IsStopped = false;

                    ////_redLamp.SetTrue();
                    ////_amberLamp.SetFalse();
                    ////_greenLamp.SetTrue();
                    //_buzzer.SetFalse();

                    break;
                case eRunStatus.Stopped:
                    IsRuning = false;
                    IsPause = false;
                    IsStopping = false;
                    IsStopped = true;

                    ////_redLamp.SetTrue();
                    ////_amberLamp.SetFalse();
                    ////_greenLamp.SetFalse();
                    //_buzzer.SetFalse();

                    break;
            }
        }


        private void AlarmOnChange(bool isAlarm)
        {
            if (isAlarm)
            {
                //_amberLamp.SetTrue();
                //_buzzer.SetTrue();
            }
            else
            {
                //_amberLamp.SetFalse();
                //_buzzer.SetFalse();
            }
        }


        private void LotIDOnChanged(string lotId)
        {
            //Handle Lot Id Change Here
        }


        public void LoadStartupRecipe()
        {
            string startupCurrentRecipe = CurrentProdRecipeName;
            CurrentProdRecipeName = "";

            CompBase _allRecipe = U.GetComponent(AppConstStaticName.ALL_RECIPES) as CompBase;
            if (_allRecipe.ChildExists(startupCurrentRecipe))
            {
                CurrentProdRecipeName = startupCurrentRecipe;
            }
            else if(startupCurrentRecipe =="")
            {
                CurrentProdRecipeName = AppConstStaticName.SAMPLE_RECIPE;
            }
        }

        private void CurrentRecipeOnChange(string recipeName)
        {

            CurrentRecipe = AppUtility.GetCurrentRecipe();
            RecipePropValue_OnChange(CurrentRecipe,null);
        }


        public void RecipePropValue_OnChange(object sender, PropertyChangedEventArgs e)
        {
            if(sender == null)
            {
                return;
            }

            AppProductRecipe recipeChanged = sender as AppProductRecipe;   
            
            //Clone data to reference recipe  
            if (recipeChanged.Name == this.CurrentRecipe.Name)
            {
                AppProductRecipe masterCopy = recipeChanged.ShallowClone(typeof(AppProductRecipe)) as AppProductRecipe;
                masterCopy.Name = AppConstStaticName.REF_CURRENT_RECIPE;
                masterCopy.CopyPropertyTo(_refCurrentRecipe);
            }
            //Copy to Current Recipe
            else if (recipeChanged.Name == AppConstStaticName.REF_CURRENT_RECIPE)
            {
                AppProductRecipe masterCopy = recipeChanged.ShallowClone(typeof(AppProductRecipe)) as AppProductRecipe;
                masterCopy.Name = this.CurrentRecipe.Name;
                masterCopy.CopyPropertyTo(this.CurrentRecipe);
            }
        }
       
    }
}
