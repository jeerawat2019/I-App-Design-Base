using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SMLib.Path;
using SMLib.Flow;

namespace SMFlowChart.Controls
{
    public partial class SegmentCtl : UserControl, ISelectable
    {
        /// <summary>
        /// The last one?
        /// </summary>
        public bool IsLast 
        {
            get { return _pathSeg.nextSegement == null; }    
        }

        public bool IsEditable
        {
            get { return (!FirstPathSeg.HasTargetID ); }
        }
        public bool Vertical { get; set; } 
        public bool ArrowEnd { get; set; }

        // Only applies to first PathOut
        private bool _selected = false;
        bool ISelectable.SMSelected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                if (_selected)
                {
                    SegmentCtl segCtl = _ctlBase.GetLastSegmentCtl(FirstPathSeg);
                    if (segCtl != null)
                    {
                        segCtl.BringToFront();
                    }
                }
                else
                {
                    // Bring to front the target ID ctl
                }
                _ctlBase.Repaint(FirstPathSeg as SMPathOut);
            }
        }


        private bool IsSelected
        {
            get
            {
                return _ctlBase.IsSelected(FirstPathSeg);
            }

        }

        private SMPathOut FirstPathSeg
        {
            get { return _pathSeg.FirstSegement; }
        }
        private SMPathSegment _pathSeg = null;
        private SMGenericFlowChart _flowChart = null;
        private SMFlowBase _flowItem = null;
        private Panel _panel = null;
        private int _lineWidth = 2;
        private Rectangle _rcHead = Rectangle.Empty;
        private Rectangle _rcTail = Rectangle.Empty;
        private SMFlowBase.eDir _dir = SMFlowBase.eDir.Down;
        private SMCtlBase _ctlBase = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flowChart"></param>
        /// <param name="panel"></param>
        /// <param name="flowItem"></param>
        /// <param name="ctlBase"></param>
        /// <param name="pathSeg"></param>
        public SegmentCtl(SMGenericFlowChart flowChart, Panel panel, SMFlowBase flowItem, 
            SMCtlBase ctlBase, SMPathSegment pathSeg)
        {
            _flowChart = flowChart;
            _panel = panel;
            _flowItem = flowItem;
            _ctlBase = ctlBase;
            _pathSeg = pathSeg;
            ArrowEnd = IsLast;
            InitializeComponent();
        }
        public PointF MoveIt(ref SMFlowBase.eDir dir, PointF startGridPt, bool vertical)
        {
            _lineWidth = IsEditable ? 8 : 2;
            Vertical = vertical;
            PointF endGridPt = startGridPt;
            float gridDistance = _pathSeg.GridDistance;
            Point startPt = SMGenericFlowChart.GridToPixel(startGridPt);
            int pixDistance = 0;
            int halfLine = _lineWidth / 2;
            if (vertical)
            {
                endGridPt.Y += gridDistance;
                pixDistance = SMGenericFlowChart.GridToPixelY(gridDistance);
            }
            else
            {
                endGridPt.X += gridDistance;
                pixDistance = SMGenericFlowChart.GridToPixelX(gridDistance);
            }

            Point loc = startPt;
            Size size = Size.Empty;
            if (vertical)
            {
                size = new Size(_lineWidth, Math.Abs(pixDistance) + _lineWidth);
                if (pixDistance >= 0)
                {
                    // Down
                    dir = SMFlowBase.eDir.Down;
                    loc.Offset(-halfLine, -halfLine);
                }
                else
                {
                    // Up
                    dir = SMFlowBase.eDir.Up;
                    loc.Offset(-halfLine, pixDistance-halfLine);
                }
            }
            else
            {
                size = new Size(Math.Abs(pixDistance) + _lineWidth, _lineWidth);
                if (pixDistance >= 0)
                {
                    // Right
                    dir = SMFlowBase.eDir.Right;
                    loc.Offset(-halfLine, -halfLine);
                }
                else
                {
                    // Left
                    dir = SMFlowBase.eDir.Left;
                    loc.Offset(pixDistance - halfLine, -halfLine);
                }
            }
            _dir = dir;

            Location = loc;
            Size = size;

            if (IsLast)
            {
                _flowItem.ParentPanel.DetermineTarget(FirstPathSeg, endGridPt);
            }
            return endGridPt;
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            float gridDistance = _pathSeg.GridDistance;
            int halfLine = _lineWidth / 2 + 1;
            Rectangle rcTL = Rectangle.Empty;
            Rectangle rcBR = Rectangle.Empty;
            if (Vertical)
            {
                rcTL = new Rectangle(0, 0, Width, Width);
                rcBR = new Rectangle(0, Height - Width, Width, Width);
            }
            else
            {
                rcTL = new Rectangle(0, 0, Height, Height);
                rcBR = new Rectangle(Width - Height, 0, Height, Height);
            }
            if (gridDistance >= 0)
            {
                _rcHead = rcBR;
                _rcTail = rcTL;
            }
            else
            {
                _rcHead = rcTL;
                _rcTail = rcBR;
            }
            Color color = Color.Black;
            if (IsSelected)
            {
                color = Color.Blue;
            }
            else if (!FirstPathSeg.HasTargetID)
            {
                color = Color.DarkGray;
            }


            Pen penEllipse = new Pen(color, 2);
            if (IsEditable)
            {
                e.Graphics.DrawEllipse(penEllipse, _rcHead);
                e.Graphics.DrawEllipse(penEllipse, _rcTail);
            }
            else
            {
                e.Graphics.DrawRectangle(penEllipse, _rcHead);
                e.Graphics.DrawRectangle(penEllipse, _rcTail);
            }
            e.Graphics.DrawLine(new Pen(color, _lineWidth), Center(_rcHead), Center(_rcTail));
        }

        private SegmentCtl _newSegCtl = null;
        private Point _lastMousePosition = Point.Empty;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_newSegCtl != null)
            {
                _newSegCtl.OnMouseMove(sender, e);
                return;
            }
            if (_flowChart.EditMode) // && object.ReferenceEquals(this, sender))
            {
                Point newMousePosition = MousePosition;
                if (e.Button != MouseButtons.Left)
                {
                    Point ptCtlLoc = PointToClient(newMousePosition);
                    // No Left button.  Not trying to drag.  Just set cursor
                    if (_rcHead.Contains(ptCtlLoc) || _rcTail.Contains(ptCtlLoc))
                    {
                        _panel.Cursor = Cursors.SizeAll;
                    }
                    else
                    {
                        _panel.Cursor = Cursors.Default;
                    }
                }
                else 
                {
                    // Get Head in panel coordinates
                    int x = newMousePosition.X - _lastMousePosition.X;
                    int y = newMousePosition.Y - _lastMousePosition.Y;
                    switch(DragMode)
                    {
                        case eDrag.None:
                            return;
                        case eDrag.HeadClick:
                        case eDrag.TailClick:
                            // Not dragging yet
                            // Have we dragged far enough
                            int totalPixDragDistance = (x * x + y * y);
                            if (totalPixDragDistance < 5)
                            {
                                // Not far enough
                                return;
                            }
                            break;
                    }
                    // Which Drag are we trying to do?
                    switch(DragMode)
                    {
                        case eDrag.None:
                            return;
                        case eDrag.HeadClick:
                            if (IsLast)
                            {
                                // Head move
                                if (Math.Abs(x) > Math.Abs(y))
                                {
                                    //Trying to move Horizontal.  Extend or newSegment?   
                                    if (Vertical)
                                    {
                                        CreateNewSegment(eDrag.HeadExtendHorz, GetGridDistanceX(x));
                                        return;
                                    }
                                    DragMode = eDrag.HeadExtendHorz;
                                }
                                else
                                {
                                    if (!Vertical)
                                    {
                                        CreateNewSegment(eDrag.HeadExtendVert, GetGridDistanceY(y));
                                        return;
                                    }
                                    //Trying to move Vertical.  Extend or newSegment?                                
                                    DragMode = eDrag.HeadExtendVert;
                                }
                            }
                            break;
                    }
                    //
                    switch (DragMode)
                    {

                        case eDrag.HeadExtendHorz:
                            if (x != 0)
                            {
                                MoveHeadHorz(x);
                                _lastMousePosition = newMousePosition;
                                Refresh();
                            }
                            break;
                        case eDrag.HeadExtendVert:
                            if (y != 0)
                            {
                                MoveHeadVert(y);
                                _lastMousePosition = newMousePosition;
                                Refresh();
                            }
                            break;
                    }
                }
            }
        }
        private void MoveHeadHorz(int x)
        {
            // Vertical shortening or lengthening
            int newWidth = 0;
            Point headCtr = Center(_rcHead);
            Point tailCtr = Center(_rcTail);
            headCtr.Offset(x, 0);
            newWidth = headCtr.X - tailCtr.X;
            NewGridDistanceX(newWidth);
            Size = new Size(Math.Abs(newWidth) + _lineWidth, Height);
            if (headCtr.X <= tailCtr.X)
            {
                // Moving LEFT past tail point
                // Move Location
                Location = new Point(Left + x, Top);
            }
        }
        private void MoveHeadVert(int y)
        {
            // Vertical shortening or lengthening
            int newHeight = 0;
            Point headCtr = Center(_rcHead);
            Point tailCtr = Center(_rcTail);
            headCtr.Offset(0, y);
            newHeight = headCtr.Y - tailCtr.Y;
            NewGridDistanceY(newHeight);
            Size = new Size(Width, Math.Abs(newHeight) + _lineWidth);
            if (headCtr.Y <= tailCtr.Y)
            {
                // Moving UP past tail point
                // Move Location
                Location = new Point(Left, Top + y);
            }
        }
        private float GetGridDistanceX(int signValue)
        {
            return SMGenericFlowChart.PixelToGridX(_rcHead.Height) * Math.Sign(signValue);
        }

        private float GetGridDistanceY(int signValue)
        {
            return SMGenericFlowChart.PixelToGridY(_rcHead.Height) * Math.Sign(signValue);
        }

        private void NewGridDistanceX(int newPixDistance)
        {
            NewGridDistance(SMGenericFlowChart.PixelToGridX(newPixDistance));
        }
        private void NewGridDistanceY(int newPixDistance)
        {
            NewGridDistance(SMGenericFlowChart.PixelToGridY(newPixDistance));
        }
        private void NewGridDistance(float newGridDistance)
        {
            _pathSeg.GridDistance = newGridDistance;
        }

         private void CreateNewSegment(eDrag dragMode, float gridDistance)
        {
            SMPathSegment newPathSeg = _pathSeg.Append();

            newPathSeg.GridDistance = gridDistance;
            // Create the control
            _newSegCtl = _ctlBase.AppendSegmentCtl(newPathSeg);
            //newSegCtl.Size = new Size(_rcHead.Width, _rcHead.Height);
            _ctlBase.MoveItem();
            _newSegCtl.Handoff(_lastMousePosition, dragMode);
        }
       
        private enum eDrag { None, HeadClick, TailClick, HeadExtendVert, HeadExtendHorz};
        private eDrag _dragMode = eDrag.None;
        private eDrag DragMode
        {
            get 
            {
                return _dragMode;
            }
            set
            {
                switch (value)
                {
                    case eDrag.None:
                        // Clean up
                        _lastMousePosition = Point.Empty;
                        switch(_dragMode)
                        {
                            case eDrag.HeadExtendHorz:
                            case eDrag.HeadExtendVert:
                                _flowChart.onMouseMove -= new MouseEventHandler(OnMouseMove);
                                //_flowChart.OnModifiedSegment(_flowItem, dir);
                                break;
                        }
                        break;
                    // Setup
                    case eDrag.HeadClick:
                        _lastMousePosition = PointToScreen(Center(_rcHead));
                        Cursor.Position = _lastMousePosition;
                        break;
                    case eDrag.TailClick:
                        _lastMousePosition = PointToScreen(Center(_rcTail));
                        Cursor.Position = _lastMousePosition;
                        break;
                    case eDrag.HeadExtendHorz:
                    case eDrag.HeadExtendVert:
                        _flowChart.onMouseMove += new MouseEventHandler(OnMouseMove);
                        break;
                }
                _dragMode = value;
            }
        }

        private void Handoff( Point lastMousePosition, eDrag dragMode)
        {
            _lastMousePosition = lastMousePosition;
            BringToFront();
            Refresh();
            DragMode = dragMode;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Point ptCtlLoc = PointToClient(MousePosition);
            // No Left button.  Not trying to drag.  Just set cursor
            if (_rcHead.Contains(ptCtlLoc))
            {
                _ctlBase.SetSelected(FirstPathSeg);
                DragMode = eDrag.HeadClick; 
            }
            else if (_rcTail.Contains(ptCtlLoc))
            {
                _ctlBase.SetSelected(FirstPathSeg);
                DragMode = eDrag.TailClick;
            }
            else
            {
                _ctlBase.SetSelected(FirstPathSeg);
            }
       }

        public static Point Center(Rectangle rc)
        {
            return new Point(rc.Left + rc.Width / 2, rc.Top + rc.Height / 2);
        }


        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_newSegCtl != null)
            {
                _newSegCtl.OnMouseUp(sender, e);
                _newSegCtl = null;
            }
            else
            {
                DragMode = eDrag.None;
            }
            if (IsLast)
            {
                _ctlBase.MoveItem();
            }

        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (IsSelected && e.KeyCode == Keys.Delete)
            {
                // Delete last segment
                _ctlBase.DeleteLastSegment(FirstPathSeg);
            }
        }
    }
}
