using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.Geometry
{
    public partial class G3DPointCtl : UserControl, IComponentBinding<G3DPoint>
    {
        private G3DPoint _g3DPoint = null;
        private string _label;
        private MethodInvoker _delChangedX = null;
        private MethodInvoker _delChangedY = null;
        private MethodInvoker _delChangedZ = null;
        public event G3DPoint.G3DPointEventHandler OnSetClicked;
        public event G3DPoint.G3DPointEventHandler OnMoveClicked;

        [Browsable(true)]
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                gbPoint.Text = value;
            }
        }

        private bool _enableSetButton = true;
        [Browsable(true)]
        public bool EnableSetButton
        {
            get { return _enableSetButton; }
            set
            {
                _enableSetButton = value;
            }
        }

        [Browsable(true)]
        public string SetBtnText
        {
            get { return btnSet.Text; }
            set
            {
                btnSet.Text = value;
            }
        }

        public G3DPointCtl()
        {
            InitializeComponent();
            btnSet.Enabled = _enableSetButton;
            _delChangedX = new MethodInvoker(OnChangedX);
            _delChangedY = new MethodInvoker(OnChangedY);
            _delChangedZ = new MethodInvoker(OnChangedZ);
        }

        private void UnBind()
        {
            if (_g3DPoint != null)
            {
                _g3DPoint.OnChangedX -= new MethodInvoker(OnChangedX);
                _g3DPoint.OnChangedY -= new MethodInvoker(OnChangedY);
                _g3DPoint.OnChangedZ -= new MethodInvoker(OnChangedZ);
            }
        }

        /// <summary>
        /// One-way Bind to the 3D point
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public G3DPoint Bind
        {
            get { return _g3DPoint; }
            set
            {
                _g3DPoint = value;
                tbX.ReadOnly = true;
                tbY.ReadOnly = true;
                tbZ.ReadOnly = true;
                SetupBind();
            }
        }
        /// <summary>
        /// Two-way Bind to the 3D point
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public G3DPoint Bind2Way
        {
            get { return _g3DPoint; }
            set
            {
                UnBind();
                _g3DPoint = value;
                if (_g3DPoint != null)
                {
                    SetupBind();
                    // Add changes

                    tbX.TextChanged += new EventHandler(tbX_TextChanged);
                    tbY.TextChanged += new EventHandler(tbY_TextChanged);
                    tbZ.TextChanged += new EventHandler(tbZ_TextChanged);
                }
            }
        }

        private void SetupBind()
        {
            gbPoint.Text = _label;
            _g3DPoint.OnChangedX += _delChangedX;
            _g3DPoint.OnChangedY += _delChangedY;
            _g3DPoint.OnChangedZ += _delChangedZ;
            UpdateControls();
            btnSet.Enabled = OnSetClicked != null;
            btnMove.Enabled = OnMoveClicked != null;
        }
        void tbX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double d = System.Convert.ToDouble(tbX.Text);
                if (d != _g3DPoint.X)
                {
                    _g3DPoint.X = d;
                }
            }
            catch { }
        }

        void tbY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double d = System.Convert.ToDouble(tbY.Text);
                if (d != _g3DPoint.Y)
                {
                    _g3DPoint.Y = d;
                }
            }
            catch { }
        }

        void tbZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double d = System.Convert.ToDouble(tbZ.Text);
                if (d != _g3DPoint.Z)
                {
                    _g3DPoint.Z = d;
                }
            }
            catch { }
        }

        private void UpdateControls()
        {
            OnChangedX();
            OnChangedY();
            OnChangedZ();
        }

        private void OnChangedX()
        {
            if (InvokeRequired)
            {
                BeginInvoke(_delChangedX);
                return;
            }
            tbX.Text = _g3DPoint.X.Val.ToString("0.###");
        }
        private void OnChangedY()
        {
            if (InvokeRequired)
            {
                BeginInvoke(_delChangedY);
                return;
            }
            tbY.Text = _g3DPoint.Y.Val.ToString("0.###");
        }
        private void OnChangedZ()
        {
            if (InvokeRequired)
            {
                BeginInvoke(_delChangedZ);
                return;
            }
            tbZ.Text = _g3DPoint.Z.Val.ToString("0.###");
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            G3DPoint oldPt = new G3DPoint(_g3DPoint);
            OnSetClicked(_g3DPoint);
            U.LogChange(Label, oldPt.ToString(), _g3DPoint.ToString());
            UpdateControls();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            OnMoveClicked(_g3DPoint);
        }
    }
}
