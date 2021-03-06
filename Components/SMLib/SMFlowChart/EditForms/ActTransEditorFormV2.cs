﻿using System;
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
    public partial class ActTransEditorFormV2 : Form
    {
        private SMContainerPanel _containerPanel = null;
        private SMActTransFlow _actTransFlowItem = null;
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
 
        public ActTransEditorFormV2(SMContainerPanel containerPanel, SMActTransFlow actTransFlowItem)
        {
            _containerPanel = containerPanel;
            _actTransFlowItem = actTransFlowItem;
            InitializeComponent();

            _initialGBHeight = gbMethods.Height;
            _initialDLGHeight = Height;
            tbText.Text = _actTransFlowItem.Text;
            Text = actTransFlowItem.Name;
            if (_actTransFlowItem.HasMethod)
            {
                foreach (SMMethod method in _actTransFlowItem.ChildArray)
                {
                    AddMethod(method);
                }
            }
            AddMethod(null);

            mcbDryRunSkipActions.BindTwoWay(() => _actTransFlowItem.DryRunSkipActions);
            panelTransitionEdit.Controls.Add(new TransitionEditorCtrlV2(_actTransFlowItem));
        }


        private void SetNewSizes()
        {
            int count = _list.Count;
            gbMethods.Size = new Size(gbMethods.Width, _initialGBHeight + count * SPACING);
            //this.Size = new Size(Width, _initialDLGHeight + count * SPACING);
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
            compIDCtl.ScopeID = _actTransFlowItem.ParentContainer.ScopeID;

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
            _actTransFlowItem.Text = tbText.Text;
            int i = 0;
            for(; i<_list.Count-1; i++)
            {
                if (i < _actTransFlowItem.Count)
                {
                    string oldVal = (_actTransFlowItem[i] as SMMethod).MethodID;
                    if (oldVal != _list[i].ID)
                    {
                        (_actTransFlowItem[i] as SMMethod).MethodID = _list[i].ID;
                        U.LogChange(string.Format("{0}.Method-{1}", _actTransFlowItem.Nickname, i), oldVal, _list[i].ID);
                    }
                }
                else
                {
                    SMMethod method = new SMMethod(string.Empty);
                    _actTransFlowItem.Add(method);
                    method.MethodID = _list[i].ID;
                    U.LogChangeAdded(string.Format("{0}.Method-{1} : {2}", _actTransFlowItem.Nickname, i, method.MethodID));
                }
            }

            int count = _actTransFlowItem.Count;
            for (; i < count; i++)
            {
                int iRemove = _actTransFlowItem.Count - 1;
                U.LogChangeRemoved(string.Format("{0}.Method-{1} : {2}", _actTransFlowItem.Nickname, iRemove, (_actTransFlowItem[iRemove] as SMMethod).MethodID));
                _actTransFlowItem.RemoveLast();
            }
            _actTransFlowItem.Rebuild();
            _containerPanel.Redraw(_actTransFlowItem);
            Hide();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _containerPanel.DeleteFlowItem(_actTransFlowItem);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void ActTransEditorFormV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    }
}
