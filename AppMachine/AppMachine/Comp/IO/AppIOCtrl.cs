using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCore;
using MCore.Controls;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using AppMachine.Control;

namespace AppMachine.Comp.IO
{
    public partial class AppIOCtrl : AppUserControlBase
    {
        Inputs _inputs = null;
        Outputs _outputs = null;
        bool _enaCtrlOutputs;

        [Browsable(true)]
        public bool EnaControlOutput
        {
            get
            {
             return _enaCtrlOutputs;
            }
            set
            {
                _enaCtrlOutputs = value;
                CheckedListBox outClrt = panelOutputs.Controls[0].Controls[0] as CheckedListBox;
                if(!value)
                {
                    outClrt.SelectionMode = SelectionMode.None;
                }
                else
                {
                    outClrt.SelectionMode = SelectionMode.One;
                }
            }
        }

        public AppIOCtrl():base()
        {
            InitializeComponent();
            _inputs = U.GetComponent(AppConstStaticName.INPUTS) as Inputs;
            _outputs = U.GetComponent(AppConstStaticName.OUTPUTS) as Outputs;

            BoolInputsCtl inputsCtl = new BoolInputsCtl();
            inputsCtl.Bind = _inputs;
            inputsCtl.Dock = DockStyle.Fill;
            panelInputs.Controls.Add(inputsCtl);

            BoolOutputsCtl outputsCtl = new BoolOutputsCtl();
            outputsCtl.Bind = _outputs;
            outputsCtl.Dock = DockStyle.Fill;
            panelOutputs.Controls.Add(outputsCtl);
        }
    }
}
