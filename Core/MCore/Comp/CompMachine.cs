using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Timers;

namespace MCore.Comp
{
    /// <summary>
    /// Class for the one and only machine
    /// </summary>
    public class CompMachine : CompBase
    {
        #region Privates
        private System.Timers.Timer _processUsageTimer = new System.Timers.Timer();
        #endregion Privates

        #region Constants


        #endregion Constants


        #region Public Properties

        [Browsable(true)]
        [Category("Component Status")]
        [XmlIgnore]
        public bool IsDryRun
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => IsDryRun, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => IsDryRun, value); }
        }

        /// <summary>
        /// Returns the current language selection
        /// </summary>
        public static string CurLanguage
        {
            get
            {
                if (s_machine == null)
                    return "EN";
                return s_machine.Languauge;
            }

        }
        /// <summary>
        /// The folder where to put the data
        /// </summary>
        public string Languauge
        {
            get { return GetPropValue(() => Languauge, "EN"); }
            set { SetPropValue(() => Languauge, value); }
        }
        /// <summary>
        /// Get/Set the name
        /// </summary>
        [XmlIgnore]
        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        /// <summary>
        /// Get/Set the PSO Trigger types
        /// </summary>
        [XmlIgnore]
        [ReadOnly(true)]
        public string[] PSOTriggerTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Root folder for all things related to this machine
        /// </summary>
        public string RootFolder { get; set; }

        #endregion Public Properties

        #region Events

        #endregion Events


        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public CompMachine()
        {
            s_machine = this;
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CompMachine(string name)
            : base(name)
        {
            s_machine = this;
        }
        #endregion Constructors

        #region Public Functions

        /// <summary>
        /// Sets the control text to the string stored in the resx file in the chosen language
        /// </summary>
        /// <param name="ty"></param>
        /// <param name="ctl"></param>
        public static void SetLangString(Type ty, Control ctl)
        {
            ResourceManager resMan = new ResourceManager(ty.FullName, ty.Assembly);
            if (resMan != null)
            {
                string value = string.Empty;
                string strName = string.Format("{0}.Text", ctl.Name);
                if (s_machine.Languauge != "EN")
                {
                    CultureInfo culture = CultureInfo.GetCultureInfoByIetfLanguageTag(s_machine.Languauge);
                    value = resMan.GetString(strName, culture);
                }
                if (string.IsNullOrEmpty(value))
                {
                    value = resMan.GetString(strName);
                }
                if (!string.IsNullOrEmpty(value))
                {
                    ctl.Text = value;
                }
            }
        }

        /// <summary>
        /// Get the resource string based on the chosen language
        /// </summary>
        /// <param name="ty"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string GetLangFromControlText(Type ty, string strName)
        {
            ResourceManager resMan = new ResourceManager(ty.FullName, ty.Assembly);
            if (resMan != null)
            {
                string value = string.Empty;
                if (Languauge != "EN")
                {
                    CultureInfo culture = CultureInfo.GetCultureInfoByIetfLanguageTag(Languauge);
                    value = resMan.GetString(strName + ".Text", culture);
                }
                if (string.IsNullOrEmpty(value))
                {
                    value = resMan.GetString(strName + ".Text");
                }
                if (string.IsNullOrEmpty(value))
                {
                    Control userControl = Activator.CreateInstance(ty) as Control;
                    if (userControl.Controls.ContainsKey(strName))
                    {
                        value = userControl.Controls[strName].Text;
                    }
                    userControl.Dispose();
                }
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }
            return strName;
        }

        #endregion Public Functions


        #region Public State Machine Functions
        [StateMachineEnabled]
        public void SimError(string errMsg)
        {
            throw new Exception(errMsg);
        }

        #endregion Public State Machine Functions


        #region Override
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();

            _processUsageTimer.Interval = 1000;
            _processUsageTimer.Elapsed += new System.Timers.ElapsedEventHandler(ProcessUsageTimer_OnElapse);
 
            U.RegisterOnChanged(() => ProcessUsageTrigger, ProcessUsageTrigger_OnChaged);
        }
        #endregion

        #region Process Usage

        public MDouble.Seconds ProcessUsageDuration
        {
            get { return GetPropValue(() => ProcessUsageDuration,120); }
            set { SetPropValue(() => ProcessUsageDuration, value); }
        }

        [XmlIgnore]
        public MDouble.Seconds ProcessUsageDurationCount
        {
            get { return GetPropValue(() => ProcessUsageDurationCount,0); }
            set { SetPropValue(() => ProcessUsageDurationCount, value); }
        }

        [XmlIgnore]
        public Boolean ProcessUsageTrigger
        {
            get { return GetPropValue(() => ProcessUsageTrigger,false); }
            set { SetPropValue(() => ProcessUsageTrigger, value); }
        }

       

        private void ProcessUsageTrigger_OnChaged(Boolean trigger)
        {
            if(trigger)
            {
                ProcessUsageDurationCount = ProcessUsageDuration;
                _processUsageTimer.Enabled = true;
            }
            else
            {
                _processUsageTimer.Enabled = false;
                ProcessUsageDurationCount = ProcessUsageDuration;
            }
        }
        private void ProcessUsageTimer_OnElapse(object sender,ElapsedEventArgs e)
        {
            if (ProcessUsageDurationCount > 0)
            {
                ProcessUsageDurationCount = ProcessUsageDurationCount - 1;
            }

            if(ProcessUsageDurationCount <= 0)
            {
                ProcessUsageDurationCount = ProcessUsageDuration;
                ProcessUsageTrigger = false;
            }

        }

        #endregion
    }
}
