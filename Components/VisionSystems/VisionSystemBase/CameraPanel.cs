using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.VisionSystem
{
    public partial class CamPanel : Panel, IComponentBinding<CameraBase>
    {
        private CameraBase _cameraBase = null;

        public bool CrossHairs
        {
            set
            {
                _cameraBase.CrossHairs(this, value);
            }
        }
        /// <summary>
        /// Get the Marker offset
        /// </summary>
        public PointF MarkerOffset()
        {
            return _cameraBase.MarkOffset(this);
        }
        /// <summary>
        /// Return the crosshairs to middle position
        /// </summary>
        /// <param name="parentCtl"></param>
        public void ResetCrosshairs()
        {
            _cameraBase.ResetCrosshairs(this);
        }

        public CamPanel()
        {
        }
        /// <summary>
        /// Bind to the CognexCamera7_1 component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CameraBase Bind
        {
            get { return _cameraBase; }
            set
            {
                if (_cameraBase != null)
                {
                    _cameraBase.UnregisterCameraWindow(this);
                }
                if (value != null)
                {
                    _cameraBase = value;
                    _cameraBase.RegisterCameraWindow(this);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_cameraBase == null)
            {
                RectangleF drawRect = new RectangleF(Point.Empty, Size);
                e.Graphics.DrawString("Not Registered", Font, Brushes.Red, drawRect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
        }
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Bind = null;
            base.Dispose(disposing);
        }
        public override string ToString()
        {
            return "Picture";
        }
    }
}
