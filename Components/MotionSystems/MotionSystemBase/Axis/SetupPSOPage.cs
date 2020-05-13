using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;


using MCore.Controls;
using MCore.Comp;
using MCore.Comp.Geometry;
using MCore.Comp.MotionSystem;
using MDouble;


namespace MCore.Comp.MotionSystem.Axis
{
    public partial class SetupPSOPage : UserControl, IComponentBinding<RealAxis>
    {
        private RealAxis _axis = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SetupPSOPage()
        {
            InitializeComponent();
            
            cbTriggerTypeFirstPSO.Items.Clear();
            cbTriggerTypeSecondPSO.Items.Clear();
            if(CompMachine.s_machine.PSOTriggerTypes != null)
            {            
                cbTriggerTypeFirstPSO.Items.AddRange(CompMachine.s_machine.PSOTriggerTypes) ;            
                cbTriggerTypeSecondPSO.Items.AddRange(CompMachine.s_machine.PSOTriggerTypes);
            }                        
        }

        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RealAxis Bind
        {
            get { return _axis; }
            set
            {
                _axis = value;

                //tbMaxLimit.BindTwoWay(() => _axis.MaxLimit);
                //cbEnable.BindTwoWay(() => _axis.Enabled);
                MotionSystemBase motionSystemBase = _axis.GetParent<MotionSystemBase>();
                if (motionSystemBase != null)
                {
                    RealAxis[] axisList = motionSystemBase.RecursiveFilterByType<RealAxis>();
                    cbPrimaryEncoderPSO.Items.Clear();
                    cbSecondaryEncoderPSO.Items.Clear();
                    cbTertiaryEncoderPSO.Items.Clear();
                    foreach (RealAxis axis in axisList)
                    {
                        cbPrimaryEncoderPSO.Items.Add(axis.Name);
                        cbSecondaryEncoderPSO.Items.Add(axis.Name);
                        cbTertiaryEncoderPSO.Items.Add(axis.Name);
                    }

                    if (_axis.PSOAxisKey != null)
                    {
                        if (_axis.PSOAxisKey.Length >= 1)
                        {
                            string[] sCommandList = Regex.Split(_axis.PSOAxisKey[0], "/ ");
                            cbTriggerTypeFirstPSO.Text = sCommandList[0];
                            if (sCommandList.Length >= 2)
                            {
                                tbTriggerCommandFirstPSO.Text = sCommandList[1];
                            }
                            if (sCommandList.Length >= 3)
                            {
                                for (int i = 2; i < sCommandList.Length; i++)
                                {
                                    tbTriggerCommandFirstPSO.Text = tbTriggerCommandFirstPSO.Text + "\r\n" + sCommandList[i];
                                }
                            }
                        }
                        if (_axis.PSOAxisKey.Length >= 2)
                        {
                            string[] sCommandList = Regex.Split(_axis.PSOAxisKey[1], "/ ");
                            cbTriggerTypeSecondPSO.Text = sCommandList[0];
                            if (sCommandList.Length >= 2)
                            {
                                tbTriggerCommandSecondPSO.Text = sCommandList[1];
                            }
                            if (sCommandList.Length >= 3)
                            {
                                for (int i = 2; i < sCommandList.Length; i++)
                                {
                                    tbTriggerCommandSecondPSO.Text = tbTriggerCommandSecondPSO.Text + "\r\n" + sCommandList[i];
                                }
                            }
                        }
                    }

                    if (_axis.PSOEncoders != null)
                    {
                        if (_axis.PSOEncoders.Length >= 3)
                        {
                            cbTertiaryEncoderPSO.Text = _axis.PSOEncoders[2];
                        }
                        if (_axis.PSOEncoders.Length >= 2)
                        {
                            cbSecondaryEncoderPSO.Text = _axis.PSOEncoders[1];
                        }
                        if (_axis.PSOEncoders.Length >= 1)
                        {
                            cbPrimaryEncoderPSO.Text = _axis.PSOEncoders[0];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generic Title for the property page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Setup PSO";
        }

        /// <summary>
        /// Apply PSO Settings to axis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonApplySetupPSO_Click(object sender, EventArgs e)
        {

            //Reset PSO Axis Key
            if (cbTriggerTypeFirstPSO.Text != "" && cbTriggerTypeSecondPSO.Text == "")
            {
                //Reset PSO Axis Key String
                U.LogChange(_axis.ID, _axis.PSOAxisKey[0], cbTriggerTypeFirstPSO.Text + tbTriggerCommandFirstPSO.Text);
                _axis.PSOAxisKey = null;
                _axis.PSOAxisKey = new string[1];
                _axis.PSOAxisKey[0] = cbTriggerTypeFirstPSO.Text;
                string[] sCommandList = Regex.Split(tbTriggerCommandFirstPSO.Text, "\r\n");
                foreach (string sCommand in sCommandList)
                {
                    // PSOTRACK YYP INPUT 0
                    if (sCommand.Contains("PSOTRACK") && sCommand.Contains("INPUT") && sCommand.Contains(',') && _axis.Parent is RealAxes)
                    {
                        (_axis.Parent as RealAxes).IsDualPSO = true;
                    }
                    if (sCommand.Trim() != "")
                    {
                        _axis.PSOAxisKey[0] = _axis.PSOAxisKey[0] + "/ " + sCommand.Trim();
                    }
                }
            }
            else
            {
                if (cbTriggerTypeFirstPSO.Text != "" && cbTriggerTypeSecondPSO.Text != "")
                {
                    //Reset PSO Axis Key String
                    U.LogChange(_axis.ID, _axis.PSOAxisKey[0], cbTriggerTypeFirstPSO.Text + tbTriggerCommandFirstPSO.Text);
                    if (_axis.PSOAxisKey.Length > 1)
                    {
                        U.LogChange(_axis.ID, _axis.PSOAxisKey[1], cbTriggerTypeSecondPSO.Text + tbTriggerCommandSecondPSO.Text);
                    }
                    _axis.PSOAxisKey = null;
                    _axis.PSOAxisKey = new string[2];
                    _axis.PSOAxisKey[0] = cbTriggerTypeFirstPSO.Text;
                    string[] sCommandList = Regex.Split(tbTriggerCommandFirstPSO.Text, "\r\n");
                    foreach (string sCommand in sCommandList)
                    {
                        if (sCommand.Trim() != "")
                        {
                            _axis.PSOAxisKey[0] = _axis.PSOAxisKey[0] + "/ " + sCommand.Trim();
                        }
                    }
                    _axis.PSOAxisKey[1] = cbTriggerTypeSecondPSO.Text;
                    sCommandList = Regex.Split(tbTriggerCommandSecondPSO.Text, "\r\n");
                    foreach (string sCommand in sCommandList)
                    {
                        if (sCommand.Trim() != "")
                        {
                            _axis.PSOAxisKey[1] = _axis.PSOAxisKey[1] + "/ " + sCommand.Trim();
                        }
                    }
                } // (cbTriggerTypeFirstPSO.Text != "" && cbTriggerTypeSecondPSO.Text != "") <==
                else
                {
                    if (_axis.PSOAxisKey != null)
                    {
                        U.LogChange(_axis.ID, _axis.PSOAxisKey[0], "null");
                    }
                    _axis.PSOAxisKey = null;
                }
            } // if (cbTriggerTypeFirstPSO.Text != "" && cbTriggerTypeSecondPSO.Text == "") {}  else  <==

            //Reset PSO Encoder 
            if (cbTertiaryEncoderPSO.Text != "")
            {
                //Reset PSO Encoder String
                U.LogChange(_axis.ID, _axis.PSOEncoders[0], cbPrimaryEncoderPSO.Text);
                U.LogChange(_axis.ID, _axis.PSOEncoders[1], cbSecondaryEncoderPSO.Text);
                U.LogChange(_axis.ID, _axis.PSOEncoders[2], cbTertiaryEncoderPSO.Text);
                _axis.PSOEncoders = null;
                _axis.PSOEncoders = new string[3];
                _axis.PSOEncoders[0] = cbPrimaryEncoderPSO.Text;
                _axis.PSOEncoders[1] = cbSecondaryEncoderPSO.Text;
                _axis.PSOEncoders[2] = cbTertiaryEncoderPSO.Text;
            }
            else
            {
                if (cbSecondaryEncoderPSO.Text != "")
                {
                    //Reset PSO Encoder String
                    U.LogChange(_axis.ID, _axis.PSOEncoders[0], cbPrimaryEncoderPSO.Text);
                    U.LogChange(_axis.ID, _axis.PSOEncoders[1], cbSecondaryEncoderPSO.Text);
                    _axis.PSOEncoders = null;
                    _axis.PSOEncoders = new string[2];
                    _axis.PSOEncoders[0] = cbPrimaryEncoderPSO.Text;
                    _axis.PSOEncoders[1] = cbSecondaryEncoderPSO.Text;
                }
                else
                {
                    if (cbPrimaryEncoderPSO.Text != "")
                    {
                        //Reset PSO Encoder String
                        U.LogChange(_axis.ID, _axis.PSOEncoders[0], cbPrimaryEncoderPSO.Text);
                        _axis.PSOEncoders = null;
                        _axis.PSOEncoders = new string[1];
                        _axis.PSOEncoders[0] = cbPrimaryEncoderPSO.Text;
                    }
                    else
                    {
                        if (_axis.PSOEncoders != null)
                        {
                            U.LogChange(_axis.ID, _axis.PSOEncoders[0], "null");
                        }
                        _axis.PSOEncoders = null;
                    }
                } // if (cbSecondaryEncoderPSO.Text != "") {}  else <==
            } // if (cbTertiaryEncoderPSO.Text != "") {}  else <==
        }

        private void buttonRefreshSetupPSO_Click(object sender, EventArgs e)
        {
            if (_axis != null)
            {
                if (_axis.PSOAxisKey != null)
                {
                    if (_axis.PSOAxisKey.Length >= 1)
                    {
                        string[] sCommandList = Regex.Split(_axis.PSOAxisKey[0], "/ ");
                        cbTriggerTypeFirstPSO.Text = sCommandList[0];
                        
                        tbTriggerCommandFirstPSO.Text = "";
                        if (sCommandList.Length >= 2)
                        {
                            tbTriggerCommandFirstPSO.Text = sCommandList[1];
                        }
                        if (sCommandList.Length >= 3)
                        {
                            for (int i = 2; i < sCommandList.Length; i++)
                            {
                                tbTriggerCommandFirstPSO.Text = tbTriggerCommandFirstPSO.Text + "\r\n" + sCommandList[i];
                            }
                        }
                    }// if (_axis.PSOAxisKey.Length >= 1) <==

                    if (_axis.PSOAxisKey.Length >= 2)
                    {
                        string[] sCommandList = Regex.Split(_axis.PSOAxisKey[1], "/ ");
                        cbTriggerTypeSecondPSO.Text = sCommandList[0];

                        tbTriggerCommandSecondPSO.Text = "";
                        if (sCommandList.Length >= 2)
                        {
                            tbTriggerCommandSecondPSO.Text = sCommandList[1];
                        }
                        if (sCommandList.Length >= 3)
                        {
                            for (int i = 2; i < sCommandList.Length; i++)
                            {
                                tbTriggerCommandSecondPSO.Text = tbTriggerCommandSecondPSO.Text + "\r\n" + sCommandList[i];
                            }
                        }
                    }// if (_axis.PSOAxisKey.Length >= 2) <==
                }// if (_axis.PSOAxisKey != null) <==
                else
                {
                    tbTriggerCommandFirstPSO.Text = "";
                    cbTriggerTypeFirstPSO.Text = "";
                    tbTriggerCommandSecondPSO.Text = "";
                    cbTriggerTypeSecondPSO.Text = "";
                }

                if (_axis.PSOEncoders != null)
                {
                    if (_axis.PSOEncoders.Length >= 3)
                    {
                        cbTertiaryEncoderPSO.Text = _axis.PSOEncoders[2];
                    }
                    else
                    {
                        cbTertiaryEncoderPSO.Text = "";
                    }

                    if (_axis.PSOEncoders.Length >= 2)
                    {
                        cbSecondaryEncoderPSO.Text = _axis.PSOEncoders[1];
                    }
                    else
                    {
                        cbSecondaryEncoderPSO.Text = "";
                    }

                    if (_axis.PSOEncoders.Length >= 1)
                    {
                        cbPrimaryEncoderPSO.Text = _axis.PSOEncoders[0];
                    }
                    else
                    {
                        cbPrimaryEncoderPSO.Text = "";
                    }
                }// if (_axis.PSOEncoders != null) <==
                else
                {
                    cbTertiaryEncoderPSO.Text = "";
                    cbSecondaryEncoderPSO.Text = "";
                    cbPrimaryEncoderPSO.Text = "";
                }
            }// if (_axis != null) <==
            else
            {
                cbTertiaryEncoderPSO.Text = "";
                cbSecondaryEncoderPSO.Text = "";
                cbPrimaryEncoderPSO.Text = "";

                tbTriggerCommandFirstPSO.Text = "";
                cbTriggerTypeFirstPSO.Text = "";
                tbTriggerCommandSecondPSO.Text = "";
                cbTriggerTypeSecondPSO.Text = "";
            }
        }
    }
}
