using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.SMLib.Path;
using MCore.Comp.SMLib.Flow;

namespace MCore.Comp.SMLib.SMFlowChart.Controls
{
    public partial class SegmentCtl : UserControl, ISelectable
    {
        /// <summary>
        /// The last one?
        /// </summary>
        public bool IsLast 
        {
            get { return _pathSeg.Next == null; }    
        }

        public bool IsEditable
        {
            get { return (!FirstPathSeg.HasTargetID || IsSelected); }
        }
        public bool Vertical 
        { 
            get
            {
                return _pathSeg.Vertical;
            }
        } 

        // Only applies to first PathOut
        private volatile bool _selected = false;
        private volatile bool _selectionBusy = false;
        bool ISelectable.SMSelected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (_selectionBusy)
                    return;
                _selectionBusy = true;
                _selected = value;
                if (!_selected)
                {
                    SMCtlBase ctlBaseTgt = _containerPanel.GetFlowCtl(FirstPathSeg.TargetID);
                    if (ctlBaseTgt != null)
                    {
                        ctlBaseTgt.BringToFront();
                    }
                }
                _ctlBase.PreMoveIt(FirstPathSeg);
                _selectionBusy = false;
            }
        }

        private bool IsFirst
        {
            get
            {
                return _pathSeg is SMPathOut;
            }

        }

        public bool Highlighted
        {
            get 
            {
                return FirstPathSeg.Highlighted &&  _pathSeg.Next == null; 
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
            get { return _pathSeg.First; }
        }
        private SMPathSegment _pathSeg = null;
        private SMFlowBase _flowItem = null;
        private SMContainerPanel _containerPanel = null;
        private int _lineWidth = 2;
        private Rectangle _rcHead = Rectangle.Empty;
        private Rectangle _rcTail = Rectangle.Empty;
        private SMCtlBase _ctlBase = null;
        private Pen _penLine = null;
        private Pen _penSelected = null;
        private Pen _penNoTarget = null;
        private Point _locOfstFromCtrlBase;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="containerPanel"></param>
        /// <param name="flowItem"></param>
        /// <param name="ctlBase"></param>
        /// <param name="pathSeg"></param>
        public SegmentCtl(SMContainerPanel containerPanel, SMFlowBase flowItem, 
            SMCtlBase ctlBase, SMPathSegment pathSeg)
        {
            _containerPanel = containerPanel;
            _flowItem = flowItem;
            _ctlBase = ctlBase;
            _pathSeg = pathSeg;
            if (pathSeg.First is SMPathOutError)
            {
                _penLine = new Pen(Color.Red, 2);
                _penSelected = new Pen(Color.Red, 6);
                _penNoTarget = new Pen(Color.Salmon, 6);
            }
            else if (pathSeg.First is SMPathOutStop)
            {
                _penLine = new Pen(Color.Black, 2);
                _penSelected = new Pen(Color.Blue, 6);
                _penNoTarget = new Pen(Color.DarkGray, 6);
            }
            else
            {
                _penLine = new Pen(Color.Green, 2);
                _penSelected = new Pen(Color.Green, 6);
                _penNoTarget = new Pen(Color.DarkGreen, 6);
            }
            _ctlBase.LocationChanged += new EventHandler(ctlBase_OnLocationChanged);
            InitializeComponent();
        }
        public PointF MoveIt(PointF startGridPt)
        {
            if (Highlighted && IsSelected)
            {
                (this as ISelectable).SMSelected = false;
            }
            bool StopPath = _pathSeg is SMPathOutStop;
            bool ErrorPath = _pathSeg is SMPathOutError;
            _lineWidth = IsEditable || Highlighted ? 6 : 2;
            PointF endGridPt = startGridPt;
            float gridDistance = _pathSeg.GridDistance;
            Point startPt = SMContainerPanel.GridToPixel(startGridPt);
            int pixDistance = 0;
            int halfLine = _lineWidth / 2;
            if (Vertical)
            {
                pixDistance = SMContainerPanel.GridToPixelY(gridDistance);
                if (IsFirst && _pathSeg.Next == null)
                {
                    // Check for minimum distance
                    if (pixDistance > 0)
                    {
                        if (pixDistance < _ctlBase.Height / 2)
                        {
                            pixDistance = _ctlBase.Height / 2 + 3;
                            gridDistance = SMContainerPanel.PixelToGridY(pixDistance);
                            _pathSeg.GridDistance = gridDistance;
                            pixDistance = SMContainerPanel.GridToPixelY(gridDistance);
                        }
                    }
                    else
                    {
                        if (pixDistance > -_ctlBase.Height / 2)
                        {
                            pixDistance = -_ctlBase.Height / 2 - 3;
                            gridDistance = SMContainerPanel.PixelToGridY(pixDistance);
                            _pathSeg.GridDistance = gridDistance;
                            pixDistance = SMContainerPanel.GridToPixelY(gridDistance);
                        }
                    }

                }
                endGridPt.Y += gridDistance;
            }
            else
            {
                pixDistance = SMContainerPanel.GridToPixelX(gridDistance);
                if (IsFirst && _pathSeg.Next == null)
                {
                    // Check for minimum distance
                    if (pixDistance > 0)
                    {
                        if (pixDistance < _ctlBase.Width / 2)
                        {
                            pixDistance = _ctlBase.Width / 2 + 3;
                            gridDistance = SMContainerPanel.PixelToGridX(pixDistance);
                            _pathSeg.GridDistance = gridDistance;
                            pixDistance = SMContainerPanel.GridToPixelX(gridDistance);
                        }
                    }
                    else
                    {
                        if (pixDistance > -_ctlBase.Width / 2)
                        {
                            pixDistance = -_ctlBase.Width / 2 - 3;
                            gridDistance = SMContainerPanel.PixelToGridX(pixDistance);
                            _pathSeg.GridDistance = gridDistance;
                            pixDistance = SMContainerPanel.GridToPixelX(gridDistance);
                        }
                    }

                }
                endGridPt.X += gridDistance;
            }

            Point loc = startPt;
            Size size = Size.Empty;
            if (Vertical)
            {
                size = new Size(_lineWidth, Math.Abs(pixDistance) + _lineWidth);
                if (pixDistance >= 0)
                {
                    // Down
                    loc.Offset(-halfLine, -halfLine);
                }
                else
                {
                    // Up
                    loc.Offset(-halfLine, pixDistance-halfLine);
                }
            }
            else
            {
                size = new Size(Math.Abs(pixDistance) + _lineWidth, _lineWidth);
                if (pixDistance >= 0)
                {
                    // Right
                    loc.Offset(-halfLine, -halfLine);
                }
                else
                {
                    // Left
                    loc.Offset(pixDistance - halfLine, -halfLine);
                }
            }

            Location = loc;
            _locOfstFromCtrlBase = new Point(_ctlBase.Location.X - this.Location.X, _ctlBase.Location.Y - this.Location.Y);
            Size = size;

            if (IsLast && !IsFirst)
            {
                _flowItem.DetermineTarget(FirstPathSeg);
                if (IsSelected)
                {
                    BringToFront();
                }
            }
            if (!_containerPanel.EditMode && _pathSeg.Next == null && _pathSeg is SMPathOut && !(_pathSeg as SMPathOut).HasTargetID) 
            {
                if (ErrorPath || StopPath || _pathSeg is SMPathOutBool)
                {
                    this.Hide();
                    return endGridPt;
                }
            }

            this.Show();
            Refresh();
            return endGridPt;
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            float gridDistance = _pathSeg.GridDistance;
            Point ptBkStart = Point.Empty;
            Point ptBkEnd = Point.Empty;
            bool StopPath = _pathSeg is SMPathOutStop;
            bool ErrorPath = _pathSeg is SMPathOutError;
            Image imageArrow = null;
            if (Vertical)
            {
                int halfWidth = Width / 2;
                if (gridDistance >= 0)
                {  // Down
                    _rcTail = new Rectangle(0, 0, Width, Width);
                    _rcHead = new Rectangle(0, Height - Width, Width, Width);
                    ptBkStart = new Point(halfWidth, 0);
                    ptBkEnd =  new Point(halfWidth, Height);
                    imageArrow = ErrorPath ?
                        global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.BigRedArrowDown :
                        global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.BigArrowDown;
                }
                else
                {   // Up
                    _rcHead = new Rectangle(0, 0, Width, Width);
                    _rcTail = new Rectangle(0, Height - Width, Width, Width);
                    ptBkStart = new Point(halfWidth, Height);
                    ptBkEnd =  new Point(halfWidth, 0);
                    imageArrow = ErrorPath ?
                        global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.BigRedArrowUp :
                        global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.BigArrowUp;
                }
            }
            else
            {
                int halfHeight = Height / 2;
               if (gridDistance >= 0)
                {  // Right
                    _rcTail = new Rectangle(0, 0, Height, Height);
                    _rcHead = new Rectangle(Width - Height, 0, Height, Height);
                    ptBkStart = new Point(0, halfHeight);
                    ptBkEnd = new Point(Width, halfHeight);
                    imageArrow = ErrorPath ?
                        global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.BigRedArrowRight :
                        global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.BigArrowRight;
                }
                else
                {   // Left
                    _rcHead = new Rectangle(0, 0, Height, Height);
                    _rcTail = new Rectangle(Width - Height, 0, Height, Height);
                    ptBkStart = new Point(Width, halfHeight);
                    ptBkEnd = new Point(0, halfHeight);
                    imageArrow = ErrorPath ?
                        global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.BigRedArrowLeft :
                        global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.BigArrowLeft;
                }
            }
            Pen penLine = _penLine;
            if (IsSelected || Highlighted)
            {
                penLine = _penSelected;
            }
            else if (!FirstPathSeg.HasTargetID)
            {
                penLine = _penNoTarget;
            }


            //if (penBgnd != null)
            //{
            e.Graphics.DrawLine(penLine, ptBkStart, ptBkEnd);
            //}
            //e.Graphics.DrawLine(penLine, Center(_rcHead), Center(_rcTail));
            if (IsEditable || Highlighted)
            {
                if (IsLast)
                {
                    // Draw Arrow
                    e.Graphics.DrawImage(imageArrow, _rcHead);
                }
            }
        }

        private SegmentCtl _newSegCtl = null;
        private Point _lastMousePosition = Point.Empty;
        private SMPathSegment[] _segMoves = null;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDisposed)
                return;
            if (_newSegCtl != null)
            {
                _newSegCtl.OnMouseMove(sender, e);
                return;
            }
            if (_containerPanel.EditMode) // && object.ReferenceEquals(this, sender))
            {
                Point newMousePosition = MousePosition;
                if (e.Button != MouseButtons.Left)
                {
                    Point ptCtlLoc = PointToClient(newMousePosition);
                    // No Left button.  Not trying to drag.  Just set cursor
                    Rectangle rcEntire = new Rectangle(Point.Empty, Size);
                    if (rcEntire.Contains(ptCtlLoc))
                    {
                        _containerPanel.Cursor = Cursors.SizeAll;
                    }
                    else
                    {
                        _containerPanel.Cursor = Cursors.Default;
                    }
                }
                else 
                {
                    // Get Head in panel coordinates
                    int x = newMousePosition.X - _lastMousePosition.X;
                    int y = newMousePosition.Y - _lastMousePosition.Y;
                    if (x == 0 && y == 0)
                        return;
                    // Which Drag are we trying to do?
                    bool bMovingVertical = Math.Abs(x) < Math.Abs(y);
                    bool bInLine = bMovingVertical == Vertical;
                    switch(DragMode)
                    {
                        case eDrag.None:
                            return;
                        case eDrag.BodyClick:
                        case eDrag.HeadClick:
                        case eDrag.TailClick:
                            // Not dragging yet
                            // Have we dragged far enough
                            int totalPixDragDistance = (x * x + y * y);
                            if (totalPixDragDistance < 5 || Math.Abs(x) == Math.Abs(y))
                            {
                                // Not far enough or indeterminatant
                                return;
                            }
                            if (bInLine)
                            {
                                _segMoves = new SMPathSegment[] { _pathSeg };
                                DragMode = eDrag.Extending;
                            }
                            break;
                            
                    }
                    // Not in line move
                    SMPathSegment segPrevious = _pathSeg.Previous; ;
                    SMPathSegment segNext = _pathSeg.Next; ;
                    switch (DragMode)
                    {
                        case  eDrag.BodyClick:                            
                            if (segPrevious == null)
                            {
                                DragMode = eDrag.None;
                                _flowItem.ChangeExit(FirstPathSeg, x, y);
                                _ctlBase.RebuildSegment(FirstPathSeg);
                                return;
                            }
                            _segMoves = new SMPathSegment[] { segPrevious, segNext };                        
                            DragMode = eDrag.Extending;
                            break;

                        case eDrag.HeadClick:
                            if (IsLast)
                            {
                                CreateNewSegment(eDrag.Extending, x, y);
                            }
                            else
                            {
                                // Same as moving the body
                                _segMoves = new SMPathSegment[] { segPrevious, segNext };
                                DragMode = eDrag.Extending;
                            }
                            break;
                        case eDrag.TailClick:
                            _segMoves = new SMPathSegment[] { segPrevious, segNext };                        
                            DragMode = eDrag.Extending;
                            break;
                    }
                    //
                    if (DragMode == eDrag.Extending)
                    {
                        DoExtensions(x, y);
                        _lastMousePosition = newMousePosition;
                    }
                }
            }
        }

        private void DoMove(SMPathSegment pathSeg, int xDelta, int yDelta)
        {
            if (pathSeg != null)
            {
                float gridDelta = pathSeg.Vertical ?
                    SMContainerPanel.PixelToGridY(yDelta) :
                    SMContainerPanel.PixelToGridX(xDelta);

                pathSeg.GridDistance += gridDelta;
            }
        }

        private void SetSegGridLoc(SMPathSegment pathSeg, int xPix, int yPix)
        {
            float gridDelta = pathSeg.Vertical ?
                SMContainerPanel.PixelToGridY(yPix) :
                SMContainerPanel.PixelToGridX(xPix);

            pathSeg.GridDistance = gridDelta;
        }

        private void DoExtensions(int xDelta, int yDelta)
        {
            if (_segMoves != null)
            {
                if (_segMoves.Length == 2)
                {
                    // We are extending previous and next
                    DoMove(_segMoves[0], xDelta, yDelta);
                    DoMove(_segMoves[1], -xDelta, -yDelta);
                }
                else
                {
                    // Only extending this
                    DoMove(_segMoves[0], xDelta, yDelta);
                }
            }
            _ctlBase.PreMoveIt(FirstPathSeg);
        }

        private void CreateNewSegment(eDrag dragMode, int pixDeltaX, int pixDeltaY)
        {
            SMPathSegment newPathSeg = _pathSeg.Append();

            SetSegGridLoc(newPathSeg, pixDeltaX, pixDeltaY);
            // Create the control
            _newSegCtl = _ctlBase.AppendSegmentCtl(newPathSeg);
            _ctlBase.MoveItem();
            _newSegCtl.Handoff(_lastMousePosition, dragMode, new SMPathSegment[] {newPathSeg} );
        }
       
        private enum eDrag { None, HeadClick, TailClick, BodyClick, Extending};
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
                            case eDrag.Extending:
                                _containerPanel.onMouseMove -= new MouseEventHandler(OnMouseMove);
                                _segMoves = null;
                                _flowItem.DetermineTarget(FirstPathSeg);
                                if (FirstPathSeg.HasTargetID)
                                {
                                    // Desselect and bring target to front
                                    _containerPanel.CurrentSel = null;
                                }
                                else
                                {
                                    _ctlBase.PreMoveIt(FirstPathSeg);
                                }
                                break;
                        }
                        break;
                    // Setup
                    case eDrag.BodyClick:
                        _lastMousePosition = MousePosition;
                        //Cursor.Position = _lastMousePosition;
                        break;
                    case eDrag.HeadClick:
                        _lastMousePosition = PointToScreen(Center(_rcHead));
                        Cursor.Position = _lastMousePosition;
                        break;
                    case eDrag.TailClick:
                        _lastMousePosition = PointToScreen(Center(_rcTail));
                        Cursor.Position = _lastMousePosition;
                        break;
                    case eDrag.Extending:
                        _containerPanel.onMouseMove += new MouseEventHandler(OnMouseMove);
                        break;
                }
                _dragMode = value;
            }
        }

        private void Handoff( Point lastMousePosition, eDrag dragMode, SMPathSegment[] segMoves)
        {
            _lastMousePosition = lastMousePosition;
            _segMoves = segMoves;
            BringToFront();
            Refresh();
            DragMode = dragMode;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(_containerPanel.FlowChartCtlBasic.RefStateMachine.LockText))
            {
                return;
            }
            if (!_containerPanel.EditMode || _containerPanel.PreventEdit)
            {
                _containerPanel.EditMode = true;
                return;
            }
            Point ptCtlLoc = PointToClient(MousePosition);
            // No Left button.  Not trying to drag.  Just set cursor
            if (_rcHead.Contains(ptCtlLoc))
            {
                DragMode = eDrag.HeadClick; 
            }
            else if (_rcTail.Contains(ptCtlLoc))
            {
                DragMode = eDrag.TailClick;
            }
            else
            {
                DragMode = eDrag.BodyClick;
            }
            _ctlBase.SetSelected(FirstPathSeg);
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
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (IsSelected && e.KeyCode == Keys.Delete)
            {
                // Delete last segment
                _ctlBase.DeleteLastSegment(FirstPathSeg);
            }
        }

        private void ctlBase_OnLocationChanged(object sender, EventArgs e)
        {
            this.Location = new Point(_ctlBase.Location.X - _locOfstFromCtrlBase.X, _ctlBase.Location.Y - _locOfstFromCtrlBase.Y);
        }
    }
}
