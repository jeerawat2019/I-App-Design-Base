using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using MCore.Comp.SMLib;
using MCore.Comp.SMLib.Flow;
using MCore.Comp.SMLib.Path;
using MCore;
using MCore.Comp.SMLib.SMFlowChart.Controls;
using MCore.Comp.SMLib.SMFlowChart.EditForms;

using MCore.Controls;

namespace MCore.Comp.SMLib.SMFlowChart
{
    public partial class SMFlowChartOperateBasic : SMFlowChartCtlBase, IComponentBinding<SMStateMachine>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public SMFlowChartOperateBasic()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Bind to the State Machine
        /// </summary>
        /// <param name="axis"></param>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SMStateMachine Bind
        {
            get { return base._stateMachine; }
            set
            {
                base._stateMachine = value;
              
                U.RegisterOnChanged(() => _stateMachine.IsRunning, OnChangedIsRunning);
                _currentFlowContainer = _stateMachine;
                lblHeader.Text = _currentFlowContainer.PathText;
            }
        }

        private void OnChangedIsRunning(bool isRunning)
        {
            btnRun.BackColor = isRunning ? Color.LightGreen : SystemColors.Control;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _stateMachine != null)
            {
                
            }
            base.Dispose(disposing);
        }


        private SMFlowContainer _currentFlowContainer = null;

      
        /// <summary>
        /// Give a name for this Property Page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Basic Flow Chart";
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            SMStateMachine sm = _currentFlowContainer is SMStateMachine ? _currentFlowContainer as SMStateMachine : _currentFlowContainer.GetParent<SMStateMachine>();
            sm.Go();
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            SMStateMachine sm = _currentFlowContainer is SMStateMachine ? _currentFlowContainer as SMStateMachine : _currentFlowContainer.GetParent<SMStateMachine>();
            sm.Step();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SMStateMachine sm = _currentFlowContainer is SMStateMachine ? _currentFlowContainer as SMStateMachine : _currentFlowContainer.GetParent<SMStateMachine>();
            sm.Stop();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            SMStateMachine sm = _currentFlowContainer is SMStateMachine ? _currentFlowContainer as SMStateMachine : _currentFlowContainer.GetParent<SMStateMachine>();
            sm.Pause();
        }

      
    }
}
