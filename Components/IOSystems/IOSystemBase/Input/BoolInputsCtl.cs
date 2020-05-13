using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.IOSystem.Input
{
    public partial class BoolInputsCtl : UserControl, IComponentBinding<Inputs>
    {
        private Inputs _inputs = null;
        private D.DelPropertyChanged _delPropChanged = null;
        public BoolInputsCtl()
        {
            _delPropChanged = new D.DelPropertyChanged(OnBoolInput_PropertyChanged);
            InitializeComponent();
        }

        /// <summary>
        /// Bind to the Inputs component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Inputs Bind
        {
            get { return _inputs; }
            set
            {
                _inputs = value;

                BoolInput[] boolInputs = _inputs.FilterByType<BoolInput>();
                if (boolInputs != null && boolInputs.Length > 0)
                {
                    foreach (BoolInput boolInput in boolInputs)
                    {
                        int index = checkedListBox.Items.Add(boolInput);
                        SetCheckboxState(index, boolInput);
                        boolInput.PropertyChanged += new PropertyChangedEventHandler(OnBoolInput_PropertyChanged);
                    }
                }
            }
        }

        void OnBoolInput_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(_delPropChanged, new object[] { sender, e });
                return;
            }
            BoolInput boolInput = sender as BoolInput;
            int index = checkedListBox.Items.IndexOf(boolInput);
            if (index >= 0)
            {
                SetCheckboxState(index, boolInput);
            }
        }



        private void SetCheckboxState(int index, BoolInput boolInput)
        {
            CheckState checkState = CheckState.Indeterminate;
            if (boolInput.Enabled)
            {
                checkState = boolInput.Value ? CheckState.Checked : CheckState.Unchecked;
            }
            checkedListBox.SetItemCheckState(index, checkState);

        }
        /// <summary>
        /// This will be the name for the tab page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Bool Inputs";
        }

    }
}