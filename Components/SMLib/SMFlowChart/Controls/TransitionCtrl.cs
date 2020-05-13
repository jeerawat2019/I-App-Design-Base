using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.SMLib.Flow;
using MCore.Comp.SMLib.Path;
using MCore.Comp.SMLib.SMFlowChart.EditForms;

namespace MCore.Comp.SMLib.SMFlowChart.Controls
{
    public partial class TransitionCtl : SMCtlBase
    {
        private TransitionEditorForm _editor = null;
        private SMTransition RefTransition
        {
            get { return FlowItem as SMTransition; }
        }
        public TransitionCtl() 
        {
            InitializeComponent();
        }
        public TransitionCtl(SMContainerPanel containerPanel, SMFlowBase flowItem)
            : base(containerPanel, flowItem, global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.TransitionFlow.Size)
        {
            InitializeComponent();
            U.RegisterOnChanged(() => (FlowItem as SMTransition).HasProblem, hasProblemOnChanged);
            OnChanged();
        }

        private void hasProblemOnChanged(bool hasProblem)
        {
            OnChanged();
        }

        public override void OnChanged()
        {

            if (RefTransition.HasProblem)
            {
                _state = eflowItemState.Problem;
            }
            else if (RefTransition.HasChildren)
            {
                _state = eflowItemState.Ok;
            }
            else
            {
                _state = eflowItemState.Empty;
            }
            base.OnChanged();

        }
        protected override void DoEditor()
        {
            if (_editor == null)
            {
                _editor = new TransitionEditorForm(_containerPanel, FlowItem as SMTransition);
            }
            //new TransitionEditorForm(_containerPanel, _flowItem as SMTransition).Show();
            _editor.Show();
        }
    }
}
