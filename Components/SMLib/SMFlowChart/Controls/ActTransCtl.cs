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
    public partial class ActTransCtl : SMCtlBase
    {
        private ActTransEditorFormV2 _editor = null;
        private SMActTransFlow RefActTrans
        {
            get { return FlowItem as SMActTransFlow; }
        }
        public ActTransCtl() 
        {
            InitializeComponent();
        }
        public ActTransCtl(SMContainerPanel containerPanel, SMFlowBase flowItem)
            : base(containerPanel, flowItem, global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.ActTransFlow.Size)
        {
            InitializeComponent();
            U.RegisterOnChanged(() => (FlowItem as SMActTransFlow).HasProblem, hasProblemOnChanged);
            OnChanged();
            
        }

        private void hasProblemOnChanged(bool hasProblem)
        {
            OnChanged();
        }

        public override void OnChanged()
        {
            if (RefActTrans.HasProblem)
            {
                _state = eflowItemState.Problem;
            }
            else if (RefActTrans.HasChildren)
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
                _editor = new ActTransEditorFormV2(_containerPanel, FlowItem as SMActTransFlow);
            }
            //new ActTransEditorFormV2(_containerPanel, _flowItem as SMActTransFlow).Show();
            _editor.Show();
        }
    }
}
