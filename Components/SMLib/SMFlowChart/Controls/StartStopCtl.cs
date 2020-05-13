using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.SMLib.Flow;
using MCore.Comp.SMLib.SMFlowChart.EditForms;

namespace MCore.Comp.SMLib.SMFlowChart.Controls
{
    public partial class StartStopCtl : SMCtlBase
    {
        //public StartStopCtl() 
        //{
        //    InitializeComponent();
        //}
        public StartStopCtl(SMContainerPanel containerPanel, SMFlowBase flowItem)
            : base(containerPanel, flowItem, global:: MCore.Comp.SMLib.SMFlowChart.Properties.Resources.StartStop.Size)
        {
            InitializeComponent();
        }
        protected override void DoEditor()
        {
            if (FlowItem is SMExit)
            {
                new StopEditorForm(_containerPanel, FlowItem as SMExit).ShowDialog();
            }
        }
    }
}
