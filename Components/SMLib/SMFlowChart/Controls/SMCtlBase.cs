using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.SMLib.Flow;
using MCore.Comp.SMLib.Path;
using MCore.Comp.SMLib.SMFlowChart.EditForms;


namespace MCore.Comp.SMLib.SMFlowChart.Controls
{
    public class SMCtlBase : UserControl
    {
        private SMFlowBase _flowItem = null;
      
        protected SMContainerPanel _containerPanel = null;
        protected enum eflowItemState { Empty, Problem, Ok };
        protected eflowItemState _state = eflowItemState.Ok;
        protected Font _fontBase = null;
        protected Font _fontBold = null;

        protected SMFlowBase.DelHightedOnChange delFlowHightlightedChanged = null;


        public PointF GridLoc
        {
            get { return _flowItem.GridLoc;  }
        }

        public SMFlowBase FlowItem
        {
            get { return _flowItem; }
            set { _flowItem = value; }
        }

        public SMCtlBase()
        { 
        }

        public SMCtlBase(SMContainerPanel containerPanel, SMFlowBase flowItem, Size ctlSize)
        {
            _containerPanel = containerPanel;
            _flowItem = flowItem;
            InitializeComponent();
            this.Name = flowItem.Name;
            Size = ctlSize;
            Location = GridLocToLocation();
            label.Size = new Size(ctlSize.Width - 8, label.Size.Height);
            this.Cursor = Cursors.Default;
            _fontBase = this.label.Font;
            _fontBold = new Font(this.label.Font, FontStyle.Bold);

            containerPanel.Controls.Add(this);

            Build(_flowItem[SMFlowBase.eDir.Up]);
            Build(_flowItem[SMFlowBase.eDir.Down]);
            Build(_flowItem[SMFlowBase.eDir.Left]);
            Build(_flowItem[SMFlowBase.eDir.Right]);
            delFlowHightlightedChanged = new SMFlowBase.DelHightedOnChange(HighlitedOnChanged);
            flowItem.evHighlightedOnChaged += delFlowHightlightedChanged;

            OnChanged();
            BringToFront();
        }

        private string GetSegTypeName(SMPathSegment pathSeg)
        {
            SMPathOut pathSegOut = pathSeg.First;
            string typeName = typeName = pathSegOut.GetType().Name.Substring(6);
            if (pathSegOut is SMPathOutBool)
            {
                typeName += (pathSegOut as SMPathOutBool).ID;
            }
            return typeName;
        }

        private string ArrowName(SMPathOut pathOut)
        {
            return string.Format("{0}-{1}-Arrow", this.Name, GetSegTypeName(pathOut));
        }
        /// <summary>
        /// Returns true if this segment path is selected
        /// </summary>
        /// <param name="pathOut"></param>
        /// <returns></returns>
        public bool IsSelected(SMPathOut pathOut)
        {
            SegmentCtl segCtl = GetSegmentCtl(pathOut);
            if (segCtl != null)
            {
                return (segCtl as ISelectable).SMSelected;
            }
            return false;
        }

        /// <summary>
        /// Set this path to be selected
        /// </summary>
        /// <param name="pathOut"></param>
        public void SetSelected(SMPathOut pathOut)
        {
            SegmentCtl segCtl = GetSegmentCtl(pathOut);
            _containerPanel.CurrentSel = segCtl as ISelectable;
        }

        // protected virtual void RepaintPath(SMFlowBase.eDir dir)
       // {
        //     SMPathOut pathOut = _flowItem[dir];
        //     if (pathOut != null)
       //     {
        //         Repaint(pathOut);
       //     }
       // }
       //public void Repaint(SMPathOut pathOut)
       // {
       //     SMPathSegment pathSeg = pathOut;
       //     int nSeg = 0;
       //     do 
       //     {
       //         SegmentCtl segCtl = GetSegmentCtl(pathSeg);
       //         if (segCtl != null)
       //         {
       //             segCtl.Refresh();
       //         }
       //         pathSeg = pathSeg.Next;
       //         nSeg++;
       //     } while(pathSeg != null);
       // }

        private string BuildSegName(SMPathSegment pathSeg)
        {
            return string.Format("{0}-{1}-{2}", this.Name, GetSegTypeName(pathSeg), pathSeg.Index);
        }

        protected virtual void Build(SMPathOut pathOut)
        {
            if (!(pathOut is SMPathOutPlug))
            {
                BuildSegment(pathOut);
                ArrowCtl arrowCtl = new ArrowCtl(_containerPanel, ArrowName(pathOut));
                _containerPanel.Controls.Add(arrowCtl);
            }
        }

        private void BuildSegment(SMPathSegment pathSeg)
        {
            if (pathSeg != null)
            {
                CreateSegmentCtl(pathSeg);
                BuildSegment(pathSeg.Next);
            }
        }

        public void DeleteLastSegment(SMPathOut firstSeg)
        {
            // Never delete the first one.
            SMPathSegment thisPathSeg = firstSeg.Next;

            int nSeg = 1;
            while (thisPathSeg != null)
            {
                if (thisPathSeg.Next == null)
                {
                    // Delete this one
                    SegmentCtl segCtl = GetSegmentCtl(thisPathSeg);
                    if (segCtl != null)
                    {
                        _containerPanel.Controls.Remove(segCtl);
                        segCtl.Dispose();
                    }
                    thisPathSeg.Delete();
                    PreMoveIt(firstSeg);
                    return;
                }
                thisPathSeg = thisPathSeg.Next;
                nSeg++;
            }
        }

        public SegmentCtl AppendSegmentCtl(SMPathSegment newPathSeg)
        {
            // it is already appended, just not created
            SMPathSegment pathSeg = newPathSeg.First;
            int nSeg = 0;
            while (pathSeg.Next != null)
            {
                pathSeg = pathSeg.Next;
                nSeg++;
            } 
            return CreateSegmentCtl(newPathSeg);
        }

        //public SegmentCtl GetLastSegmentCtl(SMPathOut pathOut)
        //{
        //    SMPathSegment pathSeg = pathOut.Next;
        //    int nSeg = 0;
        //    while (pathSeg != null)
        //    {
        //        if (pathSeg.Next == null)
        //        {
        //            return GetSegmentCtl(pathSeg, nSeg);
        //        }
        //        pathSeg = pathSeg.Next;
        //        nSeg++;
        //    }
        //    return null;
        //}
        private SegmentCtl GetSegmentCtl(SMPathSegment pathSeg)
        {
            string name = BuildSegName(pathSeg);
            if (_containerPanel.Controls.ContainsKey(name))
            {
                return _containerPanel.Controls[name] as SegmentCtl;
            }
            return null;
        }

        private SegmentCtl CreateSegmentCtl(SMPathSegment pathSeg)
        {
            SegmentCtl segCtl = new SegmentCtl(_containerPanel, _flowItem, this, pathSeg);
            segCtl.Name = BuildSegName(pathSeg);
            _containerPanel.Controls.Add(segCtl);
            return segCtl;
        }
        private Cursor _hoverCursor = null;
        private string _hoverText = string.Empty;
        public virtual void OnChanged()
        {
            //if (!this.Visible)
            //{
            //    return;
            //}


            //label.Text = _flowItem.Text;
            string helpText = _flowItem.ToString();
            if (string.IsNullOrEmpty(helpText))
            {
                _hoverCursor = Cursors.Default;
            }
            else if (_hoverText != helpText)
            {
                if (_hoverCursor != null && _hoverCursor.Tag != null && _hoverCursor.Tag is String && (_hoverCursor.Tag as String) == "Custom")
                    _hoverCursor.Dispose();
                _hoverCursor = CustomCursor.CreateText(helpText);
                _hoverCursor.Tag = "Custom";
            }
            _hoverText = helpText;

            if (_flowItem.Highlighted)
            {
                this.label.ForeColor = System.Drawing.Color.Blue;
                this.label.Font = _fontBold;
                //this.label.BackColor = System.Drawing.SystemColors.Highlight;
            }
            else
            {
                this.label.ForeColor = System.Drawing.SystemColors.ControlText;
                this.label.Font = _fontBase;
                //this.label.BackColor = System.Drawing.Color.Transparent;
            }


            switch (_state)
            {
                case eflowItemState.Empty:
                    this.label.ForeColor = Color.Gray;
                    this.label.Update();
                    break;
                case eflowItemState.Problem:
                    this.label.ForeColor = Color.Red;
                    this.label.Update();
                    break;
            }
        }

        public virtual void HighlitedOnChanged(bool highligted)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new SMFlowBase.DelHightedOnChange(HighlitedOnChanged), new object[] { highligted });
                return;
            }

            //if (!this.Visible)
            //{
            //    return;
            //}

            if (highligted)
            {
                if (!label.Text.StartsWith("+"))
                {
                    this.label.Text = "+" + _flowItem.Text;
                }
                else
                {
                    this.label.Text = "-" + _flowItem.Text;
                }
                this.label.ForeColor = System.Drawing.Color.Blue;
                this.label.Font = _fontBold;
                this.label.Update();
            }
            else
            {
                if (this.label.ForeColor == System.Drawing.Color.Blue)
                {
                    this.label.ForeColor = System.Drawing.SystemColors.ControlText;
                    this.label.Font = _fontBase;
                    this.label.Update();
                }
            }
        }

            public Point GridLocToLocation()
        {
            Point location = SMContainerPanel.GridToPixel(_flowItem.GridLoc);
            // Center in the grid
            location.Offset(-Size.Width/2, -Size.Height/2);
            return location;
        }
        protected PointF LocationToGridSnap()
        {
            Point ptCtlCenter = Location;
            ptCtlCenter.Offset(this.Size.Width / 2, this.Size.Height / 2);
            return SMContainerPanel.PixelToGridSnap(ptCtlCenter);
        }

        /// <summary>
        /// Get the middle of the horizontal or vertical edge of this control in panel coordinates
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public Point MidEdgePt(SMFlowBase.eDir dir)
        {
            Point pt = Location;
            switch (dir)
            {
                case SMFlowBase.eDir.Up:
                    pt.Offset(Width / 2, 0);
                    break;
                case SMFlowBase.eDir.Down:
                    pt.Offset(Width / 2, Height);
                    break;
                case SMFlowBase.eDir.Left:
                    pt.Offset(0, Height / 2);
                    break;
                case SMFlowBase.eDir.Right:
                    pt.Offset(Width, Height / 2);
                    break;
            }
            return pt;
        }

        private void MoveArrow(SMPathOut pathOut)
        {
            string name = ArrowName(pathOut);
            if (_containerPanel.Controls.ContainsKey(name))
            {
                ArrowCtl arrowCtl = _containerPanel.Controls[name] as ArrowCtl;
                if (IsSelected(pathOut) ||  !pathOut.HasTargetID)
                {
                    arrowCtl.Hide();
                    return;
                }

                // It has a target and is not selected
                arrowCtl.MoveIt(_flowItem, pathOut);
            }
        }

        protected string BuildLabelName(bool bTrue)
        {
            return string.Format("{0}-{1}", _flowItem.Name, bTrue.ToString());
        }



        private DateTime _lastUpdate;
        private int _updateMs = 0;

        /// <summary>
        /// Move just one single segment path
        /// </summary>
        /// <param name="pathOut"></param>
        public void MoveIt(SMPathOut pathOut)
        {
           
            //Do No Any GUI on Production Run for Beeter Performance
            
            /*
            if (pathOut != null && !(pathOut is SMPathOutPlug))
            {

                if(DateTime.Now > _lastUpdate)
                {
                    PointF endGridPt = MoveSegment(pathOut, _flowItem.GridLoc);
                    //Add the arrow.Either at the beginning or end
                    MoveArrow(pathOut);


                    if (pathOut is SMPathOutBool)
                    {
                        SMPathOutBool pathOutBool = pathOut as SMPathOutBool;
                        MoveYesNo(_flowItem[pathOut], BuildLabelName(pathOutBool.True));
                    }

                    _lastUpdate = DateTime.Now.AddTicks(_updateMs * 10000);

                }
            }
            */
 
        }


        public void PreMoveIt(SMPathOut pathOut)
        {

            if (pathOut != null && !(pathOut is SMPathOutPlug))
            {

                PointF endGridPt = MoveSegment(pathOut, _flowItem.GridLoc);
                // Add the arrow. Either at the beginning or end
                MoveArrow(pathOut);

                if (pathOut is SMPathOutBool)
                {
                    SMPathOutBool pathOutBool = pathOut as SMPathOutBool;
                    MoveYesNo(_flowItem[pathOut], BuildLabelName(pathOutBool.True));
                }
            }
        }

        private void MoveYesNo(SMFlowBase.eDir dir, string labelName)
        {
            if (_containerPanel.Controls.ContainsKey(labelName))
            {
                Label label = _containerPanel.Controls[labelName] as Label;
                Point labelLoc = MidEdgePt(dir);
                switch (dir)
                {
                    case SMFlowBase.eDir.Up:
                        labelLoc.Offset(2, -label.Height - 4);
                        break;
                    case SMFlowBase.eDir.Down:
                        labelLoc.Offset(2, 2);
                        break;
                    case SMFlowBase.eDir.Left:
                        labelLoc.Offset(-label.Width, -label.Height - 4);
                        break;
                    case SMFlowBase.eDir.Right:
                        labelLoc.Offset(2, -label.Height - 4);
                        break;
                }

                label.Location = labelLoc;
                if(label is YesNoLabel)
                {
                    (label as YesNoLabel).SetLocOfstfromCtrlBase();
                }
            }
        }

        private PointF MoveSegment(SMPathSegment pathSeg, PointF startGridPt)
        {
            if (pathSeg != null)
            {
                string segName = BuildSegName(pathSeg);
                if (_containerPanel.Controls.ContainsKey(segName))
                {
                    SegmentCtl segCtl = _containerPanel.Controls[segName] as SegmentCtl;
                    PointF endGridPt = segCtl.MoveIt(startGridPt);
                    return MoveSegment(pathSeg.Next, endGridPt);
                }

            }
            return startGridPt;
        }
        public virtual void MoveItem()
        {
            label.Text = _flowItem.Text;
            Location = GridLocToLocation();
            PreMoveIt(_flowItem[SMFlowBase.eDir.Up]);
            PreMoveIt(_flowItem[SMFlowBase.eDir.Down]);
            PreMoveIt(_flowItem[SMFlowBase.eDir.Left]);
            PreMoveIt(_flowItem[SMFlowBase.eDir.Right]);
        }


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.BackColor = System.Drawing.Color.Transparent;
            this.label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label.ForeColor = System.Drawing.Color.Black;
            this.label.Location = new System.Drawing.Point(0, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(99, 21);
            this.label.TabIndex = 0;
            this.label.Text = "label1";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.label.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.label.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.label.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // SMCtlBase
            // 
            this.Controls.Add(this.label);
            this.Name = "SMCtlBase";
            this.Size = new System.Drawing.Size(99, 21);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Label label;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispose(_flowItem[SMFlowBase.eDir.Up]);
                Dispose(_flowItem[SMFlowBase.eDir.Down]);
                Dispose(_flowItem[SMFlowBase.eDir.Left]);
                Dispose(_flowItem[SMFlowBase.eDir.Right]);
                if (_hoverCursor != null && _hoverCursor.Tag != null && _hoverCursor.Tag is String && (_hoverCursor.Tag as String) == "Custom")
                    _hoverCursor.Dispose();
                _fontBold.Dispose();
            }
            _flowItem.evHighlightedOnChaged -= delFlowHightlightedChanged;
            base.Dispose(disposing);
        }
        private void Dispose(SMPathOut pathOut)
        {
            if (pathOut != null && !(pathOut is SMPathOutPlug))
            {
                DisposeSegment(pathOut);
                string arrowName = ArrowName(pathOut);
                if (_containerPanel.Controls.ContainsKey(arrowName))
                {
                    ArrowCtl arrowCtl = _containerPanel.Controls[arrowName] as ArrowCtl;
                    _containerPanel.Controls.Remove(arrowCtl);
                    arrowCtl.Dispose();
                }
            }
        }
        /// <summary>
        /// Rebuild the segment
        /// </summary>
        /// <param name="pathOut"></param>
        public void RebuildSegment(SMPathOut pathOut)
        {
            Dispose(pathOut);
            Build(pathOut);
            PreMoveIt(pathOut);

        }

        private void DisposeSegment(SMPathSegment pathSeg)
        {
            if (pathSeg != null)
            {
                string segName = BuildSegName(pathSeg);
                if (_containerPanel.Controls.ContainsKey(segName))
                {
                    SegmentCtl segCtl = _containerPanel.Controls[segName] as SegmentCtl;
                    _containerPanel.Controls.Remove(segCtl);
                    segCtl.Dispose();
                    DisposeSegment(pathSeg.Next);
                }
            }
        }
        protected bool OverEmptySpace
        {
            get
            {
                return (_flowItem.Parent as SMFlowBase).FindFlowItem(LocationToGridSnap()) == null;
            }
        }

        private Point _mouseAtFirstClick = Point.Empty;
        private Point _locationOffset = Point.Empty;
        private bool _bMoving = false;
        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!_containerPanel.EditMode || _containerPanel.PreventEdit)
            {
                return;
            }
            _mouseAtFirstClick = MousePosition;
            _locationOffset = this.PointToClient(_mouseAtFirstClick);
            _bMoving = false;
        }


        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_containerPanel.EditMode)
            {
                if (e.Button == MouseButtons.Left && !_containerPanel.PreventEdit)
                {
                    Point newPos = MousePosition;
                    if (!_mouseAtFirstClick.Equals(newPos))
                    {
                        _bMoving = true;
                        Point newLoc = Parent.PointToClient(newPos);
                        newLoc.Offset(-_locationOffset.X, -_locationOffset.Y);
                        if (!this.Location.Equals(newLoc))
                        {
                            this.Location = newLoc;
                        }
                        if (OverEmptySpace)
                        {
                            Cursor = Cursors.Default;
                        }
                        else
                        {
                            Cursor = Cursors.No;
                        }
                    }
                }
                else
                {
                    Cursor = _hoverCursor;
                }
            }
        }






        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_bMoving)
            {
                if (OverEmptySpace)
                {                  
                    // Drop item
                    //PointF newPos = SMContainerPanel.PixelToGridSnap(_containerPanel.PointToClient(MousePosition));
                    PointF newPos = SMContainerPanel.PixelToGridSnap(this.Location);
                    _flowItem.GridLoc = newPos;
                    _containerPanel.Redraw();
                }
                else
                {
                    MoveItem();
                }
            }
            //this.Cursor = Cursors.Default;
            _mouseAtFirstClick = Point.Empty;
            Cursor = Cursors.Default;
            _bMoving = false;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(_containerPanel.FlowChartCtlBasic.RefStateMachine.LockText))
            {
                return;
            }
            if (_bMoving)
                return;

            if (_flowItem.HasChildren && _flowItem is SMFlowContainer)
            {
                // Check if is in upper left hand corner
                Point loc = PointToClient(MousePosition);
                if (loc.X < Height && loc.Y < Height / 2)
                {
                    _containerPanel.FlowChartCtlBasic.ShowFlowContainer(_flowItem as SMFlowContainer);
                    return;
                }
            }

            if (_containerPanel.EditMode && !_containerPanel.PreventEdit)
            {
                if (Control.ModifierKeys != Keys.Control)
                {
                    // Call up editor
                    DoEditor();
                }
                else
                {
                    DoEditGrid();
                }
            }
            else
            {
                _containerPanel.EditMode = true;                 
            }
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            _mouseAtFirstClick = Point.Empty;
            _bMoving = false;
            Cursor = Cursors.Default;
        }
        protected virtual void DoEditor()
        {
            MessageBox.Show(string.Format("Please implement Editor for '{0}'", this.GetType().Name), "Developer alert"); 
        }

        protected void DoEditGrid()
        {
            _containerPanel.ShowContextMenu(_flowItem, _flowItem.GridLoc);
        }
    }
}
