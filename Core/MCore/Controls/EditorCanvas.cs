using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

using MCore.Comp;

namespace MCore.Controls
{
    public class EditorCanvas : Panel
    {
        private PropDelegate<Point> _focusPoint = null;
        private PropDelegate<double> _zoom = null;
        private VScrollBar _vScrollBar = null;
        private HScrollBar _hScrollBar = null;
        private Image _image = null;
        private Size _imageSize = Size.Empty;
        private Point _imageOffset = Point.Empty;
        private ListBox _lbZoom = null;
        private bool _dragging = false;
        private Stopwatch _stopWatch = new Stopwatch();
        private Point _dragPrev = Point.Empty;
        private bool _bCanScroll = true;
        private Cursor _dragSaveCursor = null;
        private PointF _bkgndOffset = new PointF();
        private int _marginLeft = 0;
        private int _marginRight = 0;
        private int _marginTop = 0;
        private int _marginBot = 0;


        public Image BkgndImage
        {
            set 
            { 
                _image = value;
                OnZoomChanged();
            }

        }
        public PointF BkgndOffset
        {
            get { return _bkgndOffset; }
            set { _bkgndOffset = value; }
        }
        public bool CanScroll
        {
            get { return _bCanScroll; }
            set { _bCanScroll = value; }
        }
        public Point ImageOffset
        {
            get { return _imageOffset; }
        }

        public int MarginLeft
        {
            get { return _marginLeft; }
            set { _marginLeft = value; }
        }

        public int MarginRight
        {
            get { return _marginRight; }
            set { _marginRight = value; }
        }

        public int MarginTop
        {
            get { return _marginTop; }
            set { _marginTop = value; }
        }

        public int MarginBot
        {
            get { return _marginBot; }
            set { _marginBot = value; }
        }

        /// <summary>
        /// Initialize this Canvas
        /// </summary>
        /// <param name="focusPointLambda"></param>
        /// <param name="zoomLambda"></param>
        /// <param name="image"></param>
        /// <param name="vScrollBar"></param>
        /// <param name="hScrollBar"></param>
        public void Initialize(Expression<Func<Point>> focusPointLambda, Expression<Func<double>> zoomLambda, Image image, VScrollBar vScrollBar, HScrollBar hScrollBar, ListBox lbZoom)
        {
            _focusPoint = new PropDelegate<Point>(focusPointLambda);
            _zoom = new PropDelegate<double>(zoomLambda, OnZoomChanged);
            _image = image;

            _vScrollBar = vScrollBar;
            _vScrollBar.Minimum = 0;
            _vScrollBar.Maximum = 1000;
            _vScrollBar.Value = _focusPoint.Value.Y;

            _hScrollBar = hScrollBar;
            _hScrollBar.Minimum = 0;
            _hScrollBar.Maximum = 1000;
            _hScrollBar.Value = _focusPoint.Value.X;
            _hScrollBar.Scroll += new ScrollEventHandler(OnScrollChanged);
            _vScrollBar.Scroll += new ScrollEventHandler(OnScrollChanged);

            _lbZoom = lbZoom;
            _lbZoom.IntegralHeight = false;
            _lbZoom.Size = new Size(_lbZoom.Width, _lbZoom.Width);
            _lbZoom.MouseClick += new MouseEventHandler(OnZoomClick);
            _lbZoom.MouseWheel += new MouseEventHandler(OnChangedMouseWheel);
            OnZoomChanged();

            MouseMove += new MouseEventHandler(EditorCanvas_MouseMove);
            MouseDown += new MouseEventHandler(EditorCanvas_MouseDown);
            MouseUp += new MouseEventHandler(EditorCanvas_MouseUp);
            //CompRoot.Root.AddKeyDownRequest(this, OnKeyDown);

        }
        //private bool OnKeyDown(Keys key)
        //{
        //    return true;
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        CompRoot.Root.RemoveKeyDownRequest(this);
        //    }
        //    base.Dispose(disposing);

        //}

        public void SetImage(Image image)
        {
            _image = image;
        }

        void EditorCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _dragging = false;
                Cursor.Current = _dragSaveCursor;
            }
        }

        void EditorCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (CanScroll && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _dragSaveCursor = Cursor.Current;
                Cursor.Current = Cursors.Hand;
                _dragPrev = e.Location;
                _dragging = true;
            }
        }


        void EditorCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //_lbZoom.Focus();
            if (_dragging)
            {
                int delX = _dragPrev.X - e.Location.X;
                int delY = _dragPrev.Y - e.Location.Y;
                int delScrollX = (int)Math.Round((double)delX * 1000.0 / (double)_imageSize.Width);
                int delScrollY = (int)Math.Round((double)delY * 1000.0 / (double)_imageSize.Height);
                int newScrollX = _hScrollBar.Value + delScrollX;
                int newScrollY = _vScrollBar.Value + delScrollY;

                newScrollX = Math.Min(_hScrollBar.Maximum, newScrollX);
                newScrollX = Math.Max(_hScrollBar.Minimum, newScrollX);
                newScrollY = Math.Min(_vScrollBar.Maximum, newScrollY);
                newScrollY = Math.Max(_vScrollBar.Minimum, newScrollY);

                _hScrollBar.Value = newScrollX;
                _vScrollBar.Value = newScrollY;
                OnScrollChanged(null, null);
                _dragPrev = e.Location;
            }
        }

        private void OnChangedMouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
            {
                return;
            }

            if (_stopWatch.IsRunning && _stopWatch.ElapsedMilliseconds < 250)
            {
                return;
            }

            if (e.Delta > 0)
            {
                _zoom.Value *= 2.0;
            }
            else 
            {
                _zoom.Value *= 0.5;
            }
            _stopWatch.Restart();

        }

        public EditorCanvas()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }



        public void OnScrollChanged(object sender, ScrollEventArgs e)
        {
            _focusPoint.Value = new Point(_hScrollBar.Value, _vScrollBar.Value);
            Invalidate();
        }

        private void OnZoomChanged()
        {
            if (_image != null)
            {
                int w = _image.Size.Width;
                int h = _image.Size.Height;
                double zoom = _zoom.Value;
                _imageSize = new Size((int)(w * zoom), (int)(h * zoom));
                Invalidate();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.FillRectangle(SystemBrushes.ControlDarkDark, 0, 0, Size.Width, Size.Height);
            if (_image != null)
            {
                double marginLeft = (double)_marginLeft * _zoom.Value;
                double marginRight = (double)_marginRight * _zoom.Value;
                double marginTop = (double)_marginTop * _zoom.Value;
                double marginBot = (double)_marginBot * _zoom.Value;
                double totalScrollableWidth = _imageSize.Width - marginLeft + marginRight;
                double totalScrollableHeight = _imageSize.Height - marginTop + marginBot;
                // Draw image
                int imageX = (int)(totalScrollableWidth * (double)_hScrollBar.Value / 1000.0);
                int imageY = (int)(totalScrollableHeight * (double)_vScrollBar.Value / 1000.0);
                _imageOffset = new Point(imageX + (int)marginLeft - Size.Width / 2, imageY + (int)marginTop - Size.Height / 2);
                e.Graphics.DrawImage(_image,
                    (int)Math.Round(BkgndOffset.X * _zoom.Value) - _imageOffset.X,
                    (int)Math.Round(BkgndOffset.Y * _zoom.Value) - _imageOffset.Y, 
                    _imageSize.Width, _imageSize.Height);
            }
        }

        private void OnZoomClick(object senders, MouseEventArgs e)
        {
            if (e.Location.X + e.Location.Y < _lbZoom.Width)
            {
                _zoom.Value *= 2.0;
            }
            else
            {
                _zoom.Value *= 0.5;
            }
        }
    }
}
