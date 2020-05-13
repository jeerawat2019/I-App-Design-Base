using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp;
using MDouble;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;

namespace MCore.Comp.MotionSystem.Axis
{
    public class BoolAxis : AxisBase
    {
        private BoolOutput _outputClose = null;
        private BoolInput _inputClosed = null;
        private BoolOutput _outputOpen = null;
        private BoolInput _inputOpened = null;

        #region Public Browsable Properties


        /// <summary>
        /// Output Nickname for 'Close' direction
        /// </summary>
        [Browsable(true)]
        [Category("Outputs")]
        [Description("Output Nickname for 'Close' direction")]
        public string OutputNickNameClose
        {
            get { return GetPropValue(() => OutputNickNameClose, string.Empty); }
            set { SetPropValue(() => OutputNickNameClose, value); }
        }

        /// <summary>
        /// Output Nickname for 'Open' direction
        /// </summary>
        [Browsable(true)]
        [Category("Outputs")]
        [Description("Output Nickname for 'Open' direction")]
        public string OutputNickNameOpen
        {
            get { return GetPropValue(() => OutputNickNameOpen, string.Empty); }
            set { SetPropValue(() => OutputNickNameOpen, value); }
        }

        /// <summary>
        /// Input Nickname for 'Closed' direction
        /// </summary>
        [Browsable(true)]
        [Category("Inputs")]
        [Description("Input Nickname for 'Closed' direction")]
        public string InputNickNameClosed
        {
            get { return GetPropValue(() => InputNickNameClosed, string.Empty); }
            set { SetPropValue(() => InputNickNameClosed, value); }
        }

        /// <summary>
        /// Input Nickname for 'Open' direction
        /// </summary>
        [Browsable(true)]
        [Category("Inputs")]
        [Description("Input Nickname for 'Open' direction")]
        public string InputNickNameOpened
        {
            get { return GetPropValue(() => InputNickNameOpened, string.Empty); }
            set { SetPropValue(() => InputNickNameOpened, value); }
        }

        /// <summary>
        /// Is axis closed?
        /// </summary>
        [Browsable(true)]
        [Category("Inputs")]
        [Description("Is axis closed?")]
        public bool Closed
        {
            [StateMachineEnabled]
            get { return _inputClosed.Value; }
        }

        /// <summary>
        /// Is axis Opened?
        /// </summary>
        [Browsable(true)]
        [Category("Inputs")]
        [Description("Is axis opened?")]
        public bool Opened
        {
            [StateMachineEnabled]
            get { return _inputOpened.Value; }
        }


        #endregion Public Browsable Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BoolAxis()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public BoolAxis(string name, string inputOpened, string outputOpen, string inputClosed)
            : this(name, inputOpened, outputOpen, inputClosed, string.Empty)
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public BoolAxis(string name, string inputOpened, string outputOpen, string inputClosed, string outputClose)
            : base(name)
        {
            InputNickNameOpened = inputOpened;
            OutputNickNameOpen = outputOpen;
            InputNickNameClosed = inputClosed;
            OutputNickNameClose = outputClose;
            NegativeText = "Close";
            PositiveText = "Open";
        }

        #endregion Constructors

        /// <summary>
        /// Initialize the axis
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            MotorCountsPerMM = 1.0;
            MinLimit = -1.0;
            MaxLimit = 1.0;
        }

        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            _inputOpened = U.GetComponent(InputNickNameOpened) as BoolInput;
            _outputOpen = U.GetComponent(OutputNickNameOpen) as BoolOutput;
            _inputClosed = U.GetComponent(InputNickNameClosed) as BoolInput;
            _outputClose = U.GetComponent(OutputNickNameClose) as BoolOutput;
            _inputOpened.RegisterOnChanged(() => _inputOpened.Value, OnChangedOpenDirection);
            _inputClosed.RegisterOnChanged(() => _inputClosed.Value, OnChangedCloseDirection);
        }
        

        private void OnChangedOpenDirection(bool val)
        {
            if (val)
            {
                PositiveText = "Opened";
                NegativeText = "Close";
                CurrentMotorCounts = 0.5;

            }
            else if(CurrentMotorCounts == 0.5)
            {
                PositiveText = "Open";
                CurrentMotorCounts = 0.0;
            }
            NotifyPropertyChanged(() => Opened);
            NotifyPropertyChanged(() => Closed);
        }

        private void OnChangedCloseDirection(bool val)
        {
            if (val)
            {
                NegativeText = "Closed";
                PositiveText = "Open";
                CurrentMotorCounts = -0.5;
            }
            else if (CurrentMotorCounts == -0.5)
            {
                NegativeText = "Close";
                CurrentMotorCounts = 0.0;
            }
            NotifyPropertyChanged(() => Opened);
            NotifyPropertyChanged(() => Closed);
        }

        public override void MoveScale(double scale)
        {
            double newPos = scale * Range + MinLimit;
            
            if (newPos > CurrentPosition)
            {
                // Command a move to the right (Open)
                if (_outputClose != null)
                {
                    _outputClose.SetFalse();
                }
                if (_outputOpen != null)
                {
                    _outputOpen.SetTrue();
                }
                if (_inputClosed.Simulated)
                {
                    _inputClosed.Value = false;
                    _inputOpened.Value = true;
                }
            }
            else if (newPos < CurrentPosition)
            {
                // Command a move to the left (Close)
                if (_outputOpen != null)
                {
                    _outputOpen.SetFalse();
                }
                if (_outputClose != null)
                {
                    _outputClose.SetTrue();
                }
                if (_inputClosed.Simulated)
                {
                    _inputClosed.Value = true;
                    _inputOpened.Value = false;
                }
            }
        }

        public override void MoveRel(Millimeters position)
        {
            if (position.Val > 0)
            {
                MoveScale(0.75);
            }
            else
            {
                MoveScale(0.25);
            }            
        }

        /// <summary>
        /// Ooen the clamp
        /// </summary>
        [StateMachineEnabled]
        public virtual void Open()
        {
            MoveScale(0.75);
        }
        /// <summary>
        /// Close the clamp
        /// </summary>
        [StateMachineEnabled]
        public virtual void Close()
        {
            MoveScale(0.25);
        }
    }
}
