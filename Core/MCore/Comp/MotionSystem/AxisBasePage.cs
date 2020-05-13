using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;
using MDouble;

namespace MCore.Comp.MotionSystem
{
    public partial class AxisBasePage : UserControl, IComponentBinding<AxisBase>
    {
        private AxisBase _axis = null;
        private DoubleEventHandler _delPosChanged = null;
        private bool _vert = false;
        private bool _reverse = false;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AxisBasePage()
        {
            InitializeComponent();
            _delPosChanged = new DoubleEventHandler(OnChangedPosition);
        
        }

        [Browsable(true)]
        [Category("Orientation")]
        public bool Reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        }

        private void UnBind()
        {
            if (_axis != null)
            {
                U.UnRegisterOnChanged(() => _axis.CurrentMotorCounts, OnChangedPosition);
                U.UnRegisterOnChanged(() => _axis.CanMove, OnChangedCanMove);
            }
        }

        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AxisBase Bind
        {
            get { return _axis; }
            set
            {
                _axis = value;

                U.RegisterOnChanged(() => _axis.CurrentMotorCounts, OnChangedPosition);
                U.RegisterOnChanged(() => _axis.CanMove, OnChangedCanMove);
                OnChangedPosition(_axis.CurrentMotorCounts);
                gbPosition.Text = "Jog " + _axis.Nickname;
            }
        }

        private int PixelRange(ref int w0)
        {
            int minSize = dragMover.MinSize;
            if (_vert)
            {
                w0 = Math.Max(0, btnDecrease.Height - minSize);
                int w1 = Math.Max(0, btnIncrease.Height - minSize);
                return w0 + w1;
            }
            else
            {
                w0 = Math.Max(0, btnDecrease.Width - minSize);
                int w1 = Math.Max(0, btnIncrease.Width - minSize);
                return w0 + w1;
            }
        }

        private void OnChangedCanMove(bool bCanMove)
        {
            Enabled = bCanMove;
        }

        private void OnChangedPosition(double mc)
        {
            try
            {
                double scale = (_axis.FromMotorCounts(mc) - _axis.MinLimit) / _axis.Range;
                if (_reverse)
                {
                    scale = 1.0 - scale;
                }
                int w0 = 0;
                int pixRange = PixelRange(ref w0);
                if (_vert)
                {
                    int newHeight = (int)Math.Round(scale * pixRange) + dragMover.MinSize;
                    btnDecrease.Size = new Size(btnDecrease.Width, newHeight);
                }
                else
                {
                    int newWidth = (int)Math.Round(scale * pixRange) + dragMover.MinSize;
                    btnDecrease.Size = new Size(newWidth, btnDecrease.Height);
                }
                btnDecrease.Text = _axis.NegativeText;
                btnIncrease.Text = _axis.PositiveText;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// Generic Title for the property page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Jog";
        }

        private void OnDragMove(object sender, SplitterEventArgs e)
        {
            int w0 = 0;
            int total = PixelRange(ref w0);
            double scale = (double)w0 / (double)total;
            if (_reverse)
            {
                scale = 1.0 - scale;
            }
            _axis.MoveScale(scale);
        }

        private void OnDecreaseClick(object sender, EventArgs e)
        {
            Point ptCtlLoc = btnDecrease.PointToClient(MousePosition);
            int delta = _vert ? ptCtlLoc.Y : btnDecrease.Width - ptCtlLoc.X;
            _axis.MoveRel(GetDeltaTravel(-delta));
        }

        private void OnIncreaseClick(object sender, EventArgs e)
        {
            Point ptCtlLoc = btnIncrease.PointToClient(MousePosition);
            int delta = _vert ? btnIncrease.Height - ptCtlLoc.Y : ptCtlLoc.X;
            _axis.MoveRel(GetDeltaTravel(delta));        
        }

        private Millimeters GetDeltaTravel(int delPixels)
        {
            // First 10 pixels are reserved for single steps
            double counts = 0;
            double countRange = _axis.Range * _axis.MotorCountsPerMM;
            int pixelRange = _vert ? btnDecrease.Height + btnIncrease.Height : btnDecrease.Width + btnIncrease.Width;
            double movePercentage = (double)delPixels / (double)pixelRange;
            counts = movePercentage * countRange;
            int iFactor = 1;
            for (int i = 0; i < Math.Abs(delPixels) / 4 && iFactor < Math.Abs(counts); i++, iFactor *= 2) ;
            if (iFactor < Math.Abs(counts))
            {
                if (counts > 0)
                {
                    counts = iFactor;
                }
                else
                {
                    counts = -iFactor;
                }
            }
            double mm = _axis.FromMotorCounts(counts);

            return _reverse ? -mm : mm;
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (this.Width > this.Height)
            {
                _vert = false;
                if (this.btnDecrease.Dock != DockStyle.Left)
                {
                    // We want Vertical (Left/right)
                    this.btnIncrease.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.dragMover.Dock = System.Windows.Forms.DockStyle.Left;
                    this.btnDecrease.Dock = System.Windows.Forms.DockStyle.Left;
                }
            }
            if (this.Width < this.Height)
            {
                _vert = true;
                if (this.btnDecrease.Dock != DockStyle.Bottom)
                {
                    // We want Horizontal (Top/Bot)
                    this.btnIncrease.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.dragMover.Dock = System.Windows.Forms.DockStyle.Bottom;
                    this.btnDecrease.Dock = System.Windows.Forms.DockStyle.Bottom;
                }
            }

        }

    }
}
