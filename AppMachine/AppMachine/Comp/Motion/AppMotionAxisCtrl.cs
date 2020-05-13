using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp;
using MCore.Comp.MotionSystem;
using MCore.Comp.IOSystem.Input;

using AppMachine.Control;

namespace AppMachine.Comp.Motion
{
    public partial class AppMotionAxisCtrl : AppUserControlBase
    {

        private AppRealAxis _axis = null;

        private System.Windows.Forms.Control _dockParentControl;
        private Size _dockSize;
        private Form _floatingForm;
        private delegate void _delParamVoid();
        private bool _isInvertDir = false;
        private bool _isRelative = false;


        public AppMotionAxisCtrl()
        {
            InitializeComponent();
          
        }


        public AppMotionAxisCtrl(AppRealAxis axis,bool isInvertDir,bool isRelative)
        {
            _axis = axis;
            _isInvertDir = isInvertDir;
            _isRelative = isRelative;
            InitializeComponent();
            gbAxis.Text = _axis.Name;

            _floatingForm = new Form();
            _floatingForm.ShowInTaskbar = false;
            _floatingForm.TopMost = true;

            _floatingForm.FormClosing += new FormClosingEventHandler(_floatingForm_FormClosing);
            _floatingForm.Hide();


            mDJogStep.BindTwoWay(() => _axis.JogStep);
            mDCurrentPosition.BindTwoWay(() => _axis.CurrentPosition);
            mCbEnableAxis.BindTwoWay(() => _axis.Enabled);

            BoolInputsCtl inputsCtl = new BoolInputsCtl();
            inputsCtl.Bind = _axis.SafetyInputs;
            inputsCtl.Dock = DockStyle.Fill;
            panelSafetyInputs.Controls.Add(inputsCtl);

            this.PreventDispose = true;
        }


        #region Base Machanism
        /// <summary>
        /// Initializing 
        /// </summary>
        protected override void Initializing()
        {
            //Override method in sub class
          

        }

        /// <summary>
        /// Destroy
        /// </summary>
        protected override void DestroyHandle()
        {
            if (_floatingForm != null)
            {
                _floatingForm.FormClosing -= new FormClosingEventHandler(_floatingForm_FormClosing);
                _floatingForm.Dispose();
            }

            base.DestroyHandle();
        }


        void _floatingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DoDocking();

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }


        private void _lnkDockFloating_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_lnkDockFloating.Text == "Floating")
            {
                DoFloating();
            }
            else
            {
                DoDocking();
            }
        }



        private void DoFloating()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new _delParamVoid(DoFloating));
                return;
            }

            // If already float, do nothing
            if (this.Parent == _floatingForm)
            {
                return;
            }

            // Back up the parent and size
            _dockParentControl = this.Parent;
            _dockSize = this.Size;

            // Remove itself from current parent
            this.Parent.Controls.Remove(this);

            // Add to floating form and show
            _floatingForm.ClientSize = this.Size;
            _floatingForm.Controls.Add(this);
            _floatingForm.Show();

            // Set the link text
            _lnkDockFloating.Text = "Dock";

        }


        private void DoDocking()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new _delParamVoid(DoDocking));
                return;
            }

            // If not currently belong to the floating form, do nothing
            if (this.Parent != _floatingForm)
            {
                return;
            }

            // Remove itself from the form
            _floatingForm.Controls.Remove(this);

            // Add back to the old parent
            this.Size = _dockSize;
            _dockParentControl.Controls.Add(this);

            // Hide the form
            _floatingForm.Hide();

            // Set the link text
            _lnkDockFloating.Text = "Floating";
        }
        #endregion


        private void EnableButton()
        {
            btnHome.Enabled = true;
            btnForward.Enabled = true;
            btnReverse.Enabled = true;
           
        }

        private void DisableButton()
        {
            btnHome.Enabled = false;
            btnForward.Enabled = false;
            btnReverse.Enabled = false;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            DisableButton();

            try
            {
                _axis.Home();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error in Home: {0}", ex.Message), "Error");
            }
            finally
            {
                EnableButton();
            }

        }

        private void btnForward_Click(object sender, EventArgs e)
        {

            MDouble.Millimeters nextPosition = new MDouble.Millimeters();
            if (!_isInvertDir)
            {
                nextPosition.Val = Math.Abs(_axis.JogStep.Val);
            }
            else
            {
                nextPosition.Val = -Math.Abs(_axis.JogStep.Val);
            }


            if (!_isRelative)
            {
                nextPosition.Val += _axis.CurrentPosition.Val;
            }
           

            DisableButton();

            try
            {
                if (!_isRelative)
                {
                    _axis.MoveAbs(nextPosition, _axis.DefaultSpeed);
                }
                else
                {
                    _axis.MoveRel(nextPosition, _axis.DefaultSpeed);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error in jog Forward: {0}", ex.Message), "Error");
            }
            finally
            {

                EnableButton();
            }


        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            MDouble.Millimeters nextPosition = new MDouble.Millimeters();
            if (!_isInvertDir)
            {
                nextPosition.Val = -Math.Abs(_axis.JogStep.Val);
            }
            else
            {
                nextPosition.Val = Math.Abs(_axis.JogStep.Val);
            }


            if (!_isRelative)
            {
                nextPosition.Val += _axis.CurrentPosition.Val;
            }

            DisableButton();

            try
            {
                if (!_isRelative)
                {
                    _axis.MoveAbs(nextPosition, _axis.DefaultSpeed);
                }
                else
                {
                    _axis.MoveRel(nextPosition, _axis.DefaultSpeed);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error in jog Forward: {0}", ex.Message), "Error");
            }
            finally
            {

                EnableButton();
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DisableButton();

            try
            {
                _axis.Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error in jog Forward: {0}", ex.Message), "Error");
            }
            finally
            {

                EnableButton();
            }
        }


       

    }
}
