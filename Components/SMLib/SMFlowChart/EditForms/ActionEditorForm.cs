using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.SMLib.Flow;
using MCore.Controls;

namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    public partial class ActionEditorForm : Form
    {
        private SMContainerPanel _containerPanel = null;
        private SMActionFlow _actionFlowItem = null;
        private const int SPACING = 30;
        private const int MAXITEMS = 4;
        private int _initialGBHeight = 0;
        private int _initialDLGHeight = 0;
        private List<MPIDCtl> _list = new List<MPIDCtl>();

        private string _id = string.Empty;

        /// <summary>
        /// Property used for binding
        /// </summary>
        public string ID
        {
            get { return _id; }
            set
            {
                // Reevaluate
                for(int i=_list.Count-1; i > 0; i--)
                {
                    if (string.IsNullOrEmpty(_list[i].ID) && string.IsNullOrEmpty(_list[i - 1].ID))
                    {
                        OnRemoveItem(null, null);
                    }
                }
                if (_list.Count == 0 || !string.IsNullOrEmpty(_list[_list.Count - 1].ID))
                {
                    AddMethod(null);
                }

                SetNewSizes();
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="containerPanel"></param>
        /// <param name="actionFlowItem"></param>
        public ActionEditorForm(SMContainerPanel containerPanel, SMActionFlow actionFlowItem)
        {
            _containerPanel = containerPanel;
            _actionFlowItem = actionFlowItem;
            InitializeComponent();
            _initialGBHeight = gbMethods.Height;
            _initialDLGHeight = Height;
            tbText.Text = _actionFlowItem.Text;
            Text = actionFlowItem.Name;
            mcbDryRunSkipActions.BindTwoWay(() => _actionFlowItem.DryRunSkipActions);
            if (_actionFlowItem.HasChildren)
            {
                foreach (SMMethod method in _actionFlowItem.ChildArray)
                {
                    AddMethod(method);
                }
            }
            AddMethod(null);
        }


        private void SetNewSizes()
        {
            int count = _list.Count;
            gbMethods.Size = new Size(gbMethods.Width, _initialGBHeight + count * SPACING);
            this.Size = new Size(Width, _initialDLGHeight + count * SPACING);
            if (count > 1)
            { // Second to last item
                btnRemoveItem.Location = new Point(btnRemoveItem.Left, (count - 1) * SPACING);
                btnRemoveItem.Show();
            }
            else
            {
                btnRemoveItem.Hide();
            }
        }

        private void AddMethod(SMMethod method)
        {
            // 
            // compIDCtl
            // 
            MPIDCtl compIDCtl = new MCore.Controls.MPIDCtl();
            _list.Add(compIDCtl);
            // 1 based
            int count = _list.Count;
            SetNewSizes();
            compIDCtl.Location = new System.Drawing.Point(6, count * SPACING);
            compIDCtl.Name = string.Format("IDCtl_{0}", count);
            compIDCtl.ReturnType = MPIDCtl.eTypes.Void;
           // compIDCtl.Size = new System.Drawing.Size(502, 21);
            compIDCtl.Size = new System.Drawing.Size(this.gbMethods.Width, 21);
            compIDCtl.TabIndex = count;
            compIDCtl.ScopeID = _actionFlowItem.ParentContainer.ScopeID;

            if (method != null)
            {
                this._id = method.MethodID;
            }
            else
            {
                this._id = string.Empty;
            }
            compIDCtl.BindTwoWay(() => ID);
            this.gbMethods.Controls.Add(compIDCtl);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _actionFlowItem.Text = tbText.Text;
            int i = 0;
            for(; i<_list.Count-1; i++)
            {
                if (i < _actionFlowItem.Count)
                {
                    string oldVal = (_actionFlowItem[i] as SMMethod).MethodID;
                    if (oldVal != _list[i].ID)
                    {
                        (_actionFlowItem[i] as SMMethod).MethodID = _list[i].ID;
                        U.LogChange(string.Format("{0}.Method-{1}", _actionFlowItem.Nickname, i), oldVal, _list[i].ID);
                    }
                }
                else
                {
                    SMMethod method = new SMMethod(string.Empty);
                    _actionFlowItem.Add(method);
                    method.MethodID = _list[i].ID;
                    U.LogChangeAdded(string.Format("{0}.Method-{1} : {2}", _actionFlowItem.Nickname, i, method.MethodID));
                }
            }

            int count = _actionFlowItem.Count;
            for (; i < count; i++)
            {
                int iRemove = _actionFlowItem.Count - 1;
                U.LogChangeRemoved(string.Format("{0}.Method-{1} : {2}", _actionFlowItem.Nickname, iRemove, (_actionFlowItem[iRemove] as SMMethod).MethodID));
                _actionFlowItem.RemoveLast();
            }
            _actionFlowItem.Rebuild();
            _containerPanel.Redraw(_actionFlowItem);
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _containerPanel.DeleteFlowItem(_actionFlowItem);
                Close();
            }
        }

        private void OnRemoveItem(object sender, EventArgs e)
        {
            // Delete the last item
            int index = _list.Count - 1;
            MPIDCtl compIDCtl = _list[index];
            gbMethods.Controls.Remove(compIDCtl);
            _list.Remove(compIDCtl);
            compIDCtl.Dispose();
            compIDCtl = _list[index - 1];
            // And set ID of deleted item to empty
            compIDCtl.ID = string.Empty;
            compIDCtl.BindTwoWay(() => ID);
            SetNewSizes();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }
    }
}
