using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.IOSystem.Output
{
    public partial class BoolOutputsCtl : UserControl, IComponentBinding<Outputs>
    {
        private Outputs _outputs = null;
        private D.DelPropertyChanged _delPropChanged = null;
        public BoolOutputsCtl()
        {
            _delPropChanged = new D.DelPropertyChanged(OnBoolOutput_PropertyChanged);
            InitializeComponent();
        }

        /// <summary>
        /// Bind to the Inputs component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Outputs Bind
        {
            get { return _outputs; }
            set
            {
                _outputs = value;
                BoolOutput[] boolOutputs = _outputs.FilterByType<BoolOutput>();
                if (boolOutputs != null && boolOutputs.Length > 0)
                {
                    foreach (BoolOutput boolOutput in boolOutputs)
                    {
                        int index = checkedListBox.Items.Add(boolOutput);
                        SetCheckboxState(index, boolOutput);
                        boolOutput.PropertyChanged += new PropertyChangedEventHandler(OnBoolOutput_PropertyChanged);
                    }
                }
            }
        }
        void OnBoolOutput_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(_delPropChanged, new object[] { sender, e });
                return;
            }
            BoolOutput boolOutput = sender as BoolOutput;
            int index = checkedListBox.Items.IndexOf(boolOutput);
            if (index >= 0)
            {
                SetCheckboxState(index, boolOutput);
            }
        }

        private volatile bool _bInternalSelect = false;


        private void SetCheckboxState(int index, BoolOutput boolOutput)
        {
            CheckState checkState = CheckState.Indeterminate;
            if (boolOutput.Enabled)
            {
                checkState = boolOutput.Value ? CheckState.Checked : CheckState.Unchecked;
            }
            _bInternalSelect = true;
            checkedListBox.SetItemCheckState(index, checkState);
            _bInternalSelect = false;

        }
        /// <summary>
        /// This will be the name for the tab page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Bool Outputs";
        }

        private void OnItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_bInternalSelect)
            {
                return;
            }
            int index = e.Index;
            if (index >= 0 && e.NewValue != CheckState.Indeterminate)
            {
                BoolOutput boolOutput = checkedListBox.Items[index] as BoolOutput;
                if (boolOutput != null)
                {
                    bool newVal = e.NewValue == CheckState.Checked; // checkedListBox.GetItemChecked(index);
                    bool oldVal = e.CurrentValue == CheckState.Checked; // boolOutput.Value;
                    boolOutput.Set(newVal);
                    U.LogChange(string.Format("BoolOutput '{0}' changed from {1} to {2}", boolOutput.Name, oldVal, newVal));
                }
                else
                {
                    U.LogError(string.Format("BoolOutput is null for index '{0}'", index));
                }
            }
        }

    }
}