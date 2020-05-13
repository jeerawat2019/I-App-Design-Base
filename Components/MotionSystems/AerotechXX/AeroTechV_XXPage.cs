using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using MCore;
using MCore.Controls;
using MDouble;

// Aerotech References =>
using Aerotech.A3200;
using Aerotech.A3200.Tasks;
using Aerotech.Common.Collections;

// <=

namespace MCore.Comp.MotionSystem
{
    public partial class AeroTechV_XXPage : UserControl, IComponentBinding<AerotechV_XX>
    {

        private AerotechV_XX _Controler = null;
        private int selectedTask = 1;

        public AeroTechV_XXPage()
        {
            InitializeComponent();

        }


        /// <summary>
        /// Generic Title for the property page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Aerotech Controler";
        }


        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AerotechV_XX Bind
        {
            get { return _Controler; }
            set
            {
                _Controler = value;
                cbConnectControler.Checked = _Controler.Connected;
                tbStatusTask.Text = _Controler.GetTaskState;
                tbInitialCommands.BindTwoWay(() => _Controler.InitializeCommands);
            }
        }

        private void OnConnect(object sender, EventArgs e)
        {

            if (cbConnectControler.Checked)
            {
                try
                {

                    if (this._Controler.Connected == false)
                    {
                        _Controler.ConnectControler();
                    }
                    
                    // populate task names
                    comboBoxSelectTask.Items.Clear();
                    string[] s = Regex.Split( _Controler.GetTaskState, "\r\n");
                    foreach (string sTask in s)
                    {
                        string[] ss = Regex.Split(sTask, ": ");
                        comboBoxSelectTask.Items.Add(ss[0]);
                    }
                    // Task 0 is reserved
                    comboBoxSelectTask.SelectedIndex = this.selectedTask - 1;

                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Error Connect Controler");
                }
            }
            else
            {
                try
                {
                    _Controler.DisconnectControler();
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Error Disconnect Controler");
                }

            }
            cbConnectControler.Checked = _Controler.Connected;
            if (cbConnectControler.Checked == false)
            {
                cbResetControler.Enabled = false;
                cbConnectControler.Text = "Connect Controler";
            }
            else
            {
                cbResetControler.Enabled = true;
                cbConnectControler.Text = "Disconnect Controler";
            }
        }

        private void OnReset(object sender, EventArgs e)
        {
            if (cbResetControler.Checked)
            {
                
                cbResetControler.Enabled = false;
                cbConnectControler.Enabled = false;

                _Controler.ResetControler();
                
                cbResetControler.Checked = false;
                cbResetControler.Enabled = true;
                cbConnectControler.Enabled = true;

            }
        }

        private void buttonUpdateGlobalDouble_Click(object sender, EventArgs e)
        {
            double[] MyDoubles = null;
            lbGlobalDouble.Items.Clear();
            string s = null;

            _Controler.UpdateGlobalDouble();
            
            try
            {
                MyDoubles = _Controler.GetGlobalDouble;
                if (MyDoubles == null)
                {
                    lbGlobalDouble.Items.Add("null");
                }
                else
                {
                    // add all global doubles to List Box
                    foreach (double GlobalDouble in MyDoubles)
                    {
                        s = "$global[" + Convert.ToString(lbGlobalDouble.Items.Count) + "] = " + Convert.ToString(GlobalDouble);
                        lbGlobalDouble.Items.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error Show Updated Global Doubles");
            }
            
        }

        private void buttonUpdateTaskDouble_Click(object sender, EventArgs e)
        {
            double[] MyDoubles = null;
            lbTaskDouble.Items.Clear();
            string s = null;

            _Controler.UpdateTaskDouble(selectedTask);
            
            try
            {
                MyDoubles = _Controler.GetTaskDouble;

                if (MyDoubles == null)
                {
                    lbTaskDouble.Items.Add("null");
                }
                else
                {
                    // add all task doubles to List Box
                    foreach (double TaskDouble in MyDoubles)
                    {
                        s = "Task" + selectedTask.ToString() + ": $task[" + Convert.ToString(lbTaskDouble.Items.Count) + "] = " + Convert.ToString(TaskDouble);
                        lbTaskDouble.Items.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error Show Updated Task{0} Doubles", selectedTask.ToString());
            }
            
        }

        private void OnSelectedTaskChange(object sender, EventArgs e)
        {
            // Task 0 is reserved
            this.selectedTask = comboBoxSelectTask.SelectedIndex + 1;
        }

        private void buttonExecuteCommandOnTask_Click(object sender, EventArgs e)
        {
            string[] commands = Regex.Split(tbCommandList.Text, "\r\n");

            // Execute Commands Line by Line
            foreach (string s in commands)
            {
                this._Controler.ExecuteCommand(s, selectedTask);
            }
        }

        private void buttonAcknowledgeFault_Click(object sender, EventArgs e)
        {
            _Controler.ClearAllFaults();
        }

        private void buttonAbortMotion_Click(object sender, EventArgs e)
        {
            _Controler.Abort();
        }

        private void buttonUpdateTaskStatus_Click(object sender, EventArgs e)
        {
            tbStatusTask.Text = _Controler.GetTaskState;
        }

    }
}
