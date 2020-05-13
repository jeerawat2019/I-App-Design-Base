using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using MCore.Comp.SMLib;
using MCore.Comp.SMLib.Flow;
using MCore.Comp.SMLib.Path;
using MCore;
using MCore.Comp.SMLib.SMFlowChart.Controls;
using MCore.Comp.SMLib.SMFlowChart.EditForms;


namespace MCore.Comp.SMLib.SMFlowChart
{
    public class SMContainerPanel : Panel
    {
        #region Privates
        private SMFlowChartCtlBasic _flowChartCtlBasic = null;
        private SMFlowContainer _flowContainer = null;
        private Cursor _flowItemCursor = Cursors.Default;
        private ISelectable _currentSel = null;
        #endregion Privates
        private ContextMenuStrip contextMenuStrip;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem tsmInsertGripUpper;
        private ToolStripMenuItem tsmInsertGridBelow;
        private ToolStripMenuItem tsmInsertGridLeft;
        private ToolStripMenuItem tsmInsertGridRight;
        private ToolStripMenuItem tsmCopyItem;
        private ToolStripMenuItem tsmPasteItem;
        private ToolStripMenuItem tsmDeleteItem;
        private ToolStripMenuItem tsmClearSelection;


        //For crop control
        private int _cropX;
        private int _cropY;
        private int _cropWidth;
        private int _cropHeight;
        private int _oCropX;
        private int _oCropY;
        private Pen _cropPen;
        private bool _isCropping = false;
        private List<SMCtlBase> _selectedCtrlList = null;
        private ToolStripMenuItem tsmSelectItem;
        private bool _draging = false;
        private ToolStripMenuItem tsmDeselectItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        


        #region Static helpers
        public static Size GridSize = Size.Empty;

        public static Point GridToPixel(PointF gridLoc)
        {
            return new Point((int)Math.Round(gridLoc.X * GridSize.Width), (int)Math.Round(gridLoc.Y * GridSize.Height));
        }
        public static Size GridToPixel(SizeF gridSize)
        {
            return new Size((int)Math.Round(gridSize.Width * GridSize.Width), (int)Math.Round(gridSize.Height * GridSize.Height));
        }
        public static int GridToPixelX(float gridX)
        {
            return (int)Math.Round(gridX * GridSize.Width);
        }

        public static int GridToPixelY(float gridY)
        {
            return (int)Math.Round(gridY * GridSize.Height);
        }
        public static PointF PixelToGrid(Point pixPt)
        {
            return new PointF((float)pixPt.X / (float)GridSize.Width,
                (float)pixPt.Y / (float)GridSize.Height);
        }
        public static float PixelToGridX(int pixX)
        {
            return (float)pixX / (float)GridSize.Width;
        }
        public static float PixelToGridY(int pixY)
        {
            return (float)pixY / (float)GridSize.Height;
        }

        public static PointF PixelToGridSnap(Point pixPt)
        {
            PointF ptF = new PointF((float)(pixPt.X / GridSize.Width) + 0.5f,
                              (float)(pixPt.Y / GridSize.Height) + 0.5f);
            return ptF;
        }
        #endregion Static helpers


        #region Public Properties

        public ISelectable CurrentSel
        {
            get
            {
                return _currentSel;
            }
            set
            {
                if (EditMode)
                {
                    if (_currentSel != null)
                    {
                        _currentSel.SMSelected = false;
                    }

                    _currentSel = value;
                    if (_currentSel != null)
                    {
                        _currentSel.SMSelected = true;
                    }
                }
            }
        }
        /// <summary>
        /// Get the master control
        /// </summary>
        public SMFlowChartCtlBasic FlowChartCtlBasic
        {
            get { return _flowChartCtlBasic; }
        }

        /// <summary>
        /// Get the SMFlowContainer reference
        /// </summary>
        public SMFlowContainer FlowContainer
        {
            get { return _flowContainer; }
        }

        /// <summary>
        /// Indicates if in Edit mde
        /// </summary>
        public bool EditMode
        {
            get
            {
                return _flowContainer.IsEditing(Name);
            }
            set
            {
                if (value != _flowContainer.IsEditing(Name))
                {
                    if (value)
                    {
                        if (_flowContainer.RegisterEdit(Name))
                        {
                            BackgroundImage = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.GridBackground;
                            _flowChartCtlBasic.OnEditMode = true;
                            Redraw();
                        }
                    }
                    else
                    {
                        if (_flowContainer.UnregisterEdit(Name))
                        {
                            BackgroundImage = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.NoGridBackground;
                            _flowChartCtlBasic.OnEditMode = false;
                            Redraw();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Indicate Prevent Edit
        /// </summary>
        public bool PreventEdit
        { 
            get; 
            set; 
        }

        #endregion Public Properties

        /// <summary>
        /// Insert Flow Mode
        /// </summary>
        public enum eInsertGridMode
        {
            rowBefore,
            rowAfter,
            columnBefore,
            columnAfter,
        }



        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="flowChartCtlBasic"></param>
        /// <param name="flowContainer"></param>
        /// <param name="offsetY"></param>
        public SMContainerPanel(SMFlowChartCtlBasic flowChartCtlBasic, SMFlowContainer flowContainer, int offsetY)
            : base()
        {
            InitializeComponent();

            DoubleBuffered = true;
            _flowChartCtlBasic = flowChartCtlBasic;
            _flowContainer = flowContainer;
            Name = flowContainer.Nickname;
            TabIndex = 0;
            MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnLeftClick);
            MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            Paint += new PaintEventHandler(OnPaint);


            _flowItemCursor = CustomCursor.Create(global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.FlowItemCursor);
            BackgroundImage = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.NoGridBackground;

            GridSize = BackgroundImage.Size;

            // Set the size of the panel
            Size = GridToPixel(_flowContainer.GridSize);

            // Set the location of the panel
            Point ptLoc = GridToPixel(_flowContainer.GridCorner);
            ptLoc.Offset(0, offsetY);
            Location = ptLoc;
            Rebuild();
        }

        void OnPaint(object sender, PaintEventArgs e)
        {
            if (!string.IsNullOrEmpty(_flowChartCtlBasic.RefStateMachine.LockText))
            {
                e.Graphics.DrawString(_flowChartCtlBasic.RefStateMachine.LockText, new Font("Arial", 30), new SolidBrush(Color.FromArgb(128, 0, 0)), new Point(40, 40));
            }
        }
        /// <summary>
        /// Initial Creation
        /// </summary>
        public void Rebuild()
        {

            Control[] controls = new Control[Controls.Count];
            Controls.CopyTo(controls, 0);
            foreach (Control ctl in controls)
            {
                SMCtlBase ctlBase = ctl as SMCtlBase;
                if (ctlBase != null)
                {
                    Dispose(ctlBase);
                }
            }


            foreach (SMFlowBase flowItem in _flowContainer.FilterByType<SMFlowBase>())
            {
                Build(flowItem);
            }

            Redraw();
        }

        public void Dispose(SMCtlBase ctlBase)
        {
            ctlBase.Dispose();
            Controls.Remove(ctlBase);
        }
        public void Dispose(SMFlowBase flowItem)
        {
            SMCtlBase ctlBase = GetFlowCtl(flowItem);
            if (ctlBase != null)
            {
                Dispose(ctlBase);
            }
        }
        private void Build(SMFlowBase flowItem)
        {
            if (flowItem is SMStart || flowItem is SMExit)
            {
                new StartStopCtl(this, flowItem);
            }
            else if (flowItem is SMActionFlow)
            {
                new ActionCtl(this, flowItem);
            }
            else if (flowItem is SMDecision)
            {
                new DecisionCtl(this, flowItem);
            }
            else if (flowItem is SMSubroutine)
            {
                new SubroutineCtl(this, flowItem as SMSubroutine);
            }
            else if (flowItem is SMTransition)
            {
                new TransitionCtl(this, flowItem);
            }
            else if (flowItem is SMActTransFlow)
            {
                new ActTransCtl(this, flowItem);
            }
            else
            {
                throw new Exception(string.Format("'{0}' is not yet supported", flowItem.GetType().Name));
            }
        }

        public void Redraw()
        {
            _flowContainer.DetermineAllChildTargets();

            foreach (Control ctl in this.Controls)
            {
                SMCtlBase ctlBase = ctl as SMCtlBase;
                if (ctlBase != null)
                {
                    ctlBase.MoveItem();
                }
            }

            //foreach (SMFlowBase flowItem in _flowContainer.FilterByType<SMFlowBase>())
            //{
            //    SMCtlBase ctlItem = GetFlowCtl(flowItem);
            //    if (ctlItem != null)
            //    {
            //        ctlItem.MoveItem();
            //    }
            //}
        }

        public void AddNewFlowItem(Type flowItemType, PointF ptGridPt, string text)
        {
            SMFlowBase flowItem = Activator.CreateInstance(flowItemType, string.Empty) as SMFlowBase;
            flowItem.Text = text;
            flowItem.GridLoc = ptGridPt;
            _flowContainer.AddFlowItem(flowItem);
            U.LogChangeAdded(string.Format("{0}.{1}", flowItem.Nickname, text));
            Build(flowItem);
            Redraw(flowItem);
        }


        /// <summary>
        /// Entry into a flow item
        /// </summary>
        public void RefreshFlowItem(SMFlowBase currentFlowItem )
        {
            SMCtlBase ctlBase = GetFlowCtl(currentFlowItem);
            if (ctlBase != null)
            {
                ctlBase.OnChanged();
                if (currentFlowItem.IncomingPath != null)
                {
                    SMFlowBase pathFlowItem = currentFlowItem.IncomingPath.Owner;
                    ctlBase = GetFlowCtl(pathFlowItem);
                    if (ctlBase != null)
                    {
                        ctlBase.MoveIt(currentFlowItem.IncomingPath);
                    }
                }
            }
        }


        public void Redraw(SMFlowBase flowItem)
        {
            if (flowItem != null)
            {
                flowItem.DetermineAllPathTargets();
                SMCtlBase ctlBase = GetFlowCtl(flowItem);
                if (ctlBase != null)
                {
                    ctlBase.MoveItem();
                    ctlBase.OnChanged();
                }
            }
        }

        public void DeleteFlowItem(SMFlowBase flowItem)
        {
            U.LogChangeRemoved(flowItem.Nickname);
            _flowChartCtlBasic.RemoveFlowContainer(flowItem as SMFlowContainer);
            flowItem.Delete();
            Dispose(flowItem);
            Redraw();
        }
        public SMCtlBase GetFlowCtl(SMFlowBase flowItem)
        {
            if (flowItem == null)
                return null;
            return GetFlowCtl(flowItem.Name);
        }
        /// <summary>
        /// Get the FlowItem Ctl
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SMCtlBase GetFlowCtl(string name)
        {
            if (!string.IsNullOrEmpty(name) && this.Controls.ContainsKey(name))
            {
                return this.Controls[name] as SMCtlBase;
            }
            return null;
        }
        public event MouseEventHandler onMouseMove;
        private bool _emptyFlowSpot = false;
        private Graphics _g = null;
        private System.Drawing.Drawing2D.GraphicsContainer _gContainer = null;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _isCropping = false;
//            System.Diagnostics.Debug.WriteLine(string.Format("FlowChart MouseMove"));
            if (onMouseMove != null)
            {
  //              System.Diagnostics.Debug.WriteLine(string.Format("Fire FlowChart MouseMove event"));
                onMouseMove(this, e);
                return;
            }

            SMContainerPanel panel = sender as SMContainerPanel;
            if (panel == null)
                return;

            if (e.Button == MouseButtons.Left)
            {
                if (Control.ModifierKeys != Keys.Control)
                {
                    if (_emptyFlowSpot)
                    {
                        // turn off New Flow Item readiness
                        panel.Cursor = Cursors.Hand;
                        _emptyFlowSpot = false;
                    }
                    if (!_lastMousePos.IsEmpty)
                    {
                        // Dragging
                        Point newPos = _flowChartCtlBasic.PointToClient(MousePosition);
                        Point deltaMoved = newPos;

                        deltaMoved.Offset(-_lastMousePos.X, -_lastMousePos.Y);
                        panel.Top += deltaMoved.Y;
                        panel.Left += deltaMoved.X;
                        _lastMousePos = newPos;

                        ////Cropping
                        //this.Refresh();
                        //_cropWidth = Math.Abs(e.X - _cropX);
                        //_cropHeight = Math.Abs(e.Y - _cropY);

                        //_oCropX = Math.Min(_cropX, e.X);
                        //_oCropY = Math.Min(_cropY, e.Y);

                        //this.CreateGraphics().DrawRectangle(_cropPen, _oCropX, _oCropY, _cropWidth, _cropHeight);
                        //_isCropping = true;
                    }
                }
                else
                {
                    if (_selectedCtrlList != null)
                    {
                        _draging = true;
                        if (_selectedCtrlList != null && _selectedCtrlList.Count > 0)
                        {
                            foreach (SMCtlBase smCtrl in _selectedCtrlList)
                            {
                                try
                                {
                                    smCtrl.OnMouseMove(null, e);
                                }
                                catch { }
                                finally { }
                            }
                        }
                    }
                    else
                    {
                        //Cropping
                        //this.Refresh();
                        if(_g != null)
                        {
                            _g.Clear(Color.FromArgb(255,236,233,216));
                            _g.Dispose();
                            _g = null;
                        }
 
                        _g = this.CreateGraphics();
                        _g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                        _g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                        

                        _cropWidth = Math.Abs(e.X - _cropX);
                        _cropHeight = Math.Abs(e.Y - _cropY);

                        _oCropX = Math.Min(_cropX, e.X);
                        _oCropY = Math.Min(_cropY, e.Y);

                        _g.DrawRectangle(_cropPen, _oCropX, _oCropY, _cropWidth, _cropHeight);
                        _isCropping = true;
                    }
                }
            }
            else if (EditMode)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (!_lastMousePos.IsEmpty)
                    {
                        // Dragging
                        Point newPos = _flowChartCtlBasic.PointToClient(MousePosition);
                        Point deltaMoved = newPos;

                        deltaMoved.Offset(-_lastMousePos.X, -_lastMousePos.Y);

                        if (Math.Abs(deltaMoved.X) > GridToPixelX(1.0f))
                        {
                            // Change in X
                            if (deltaMoved.X > 0)
                            {
                                _flowContainer.MoveAll(new PointF(1f, 0f));
                            }
                            else
                            {
                                _flowContainer.MoveAll(new PointF(-1f, 0f));
                            }
                            _lastMousePos = newPos;
                            Redraw();
                        }
                        else if (Math.Abs(deltaMoved.Y) > GridToPixelY(1.0f))
                        {
                            // Chang in Y
                            if (deltaMoved.Y > 0)
                            {
                                _flowContainer.MoveAll(new PointF(0f, 1f));
                            }
                            else
                            {
                                _flowContainer.MoveAll(new PointF(0f, -1f));
                            }
                            _lastMousePos = newPos;
                            Redraw();
                        }
                    }
                }
               
                else
                {
                    // If over an empty space, show
                    PointF gridPos = PixelToGrid(panel.PointToClient(MousePosition));
                    double x = gridPos.X - (int)gridPos.X;
                    double y = gridPos.Y - (int)gridPos.Y;

                    if (x < 0.7 && x > 0.3 && y < 0.6 && y > 0.4)
                    {
                        _emptyFlowSpot = true;
                        panel.Cursor = _flowItemCursor;
                    }
                    else
                    {
                        _emptyFlowSpot = false;
                        panel.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private Point _lastMousePos = Point.Empty;

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                _lastMousePos = _flowChartCtlBasic.PointToClient(MousePosition);

                //if (Control.ModifierKeys != Keys.Control)
                //{
                //    _cropX = e.X;
                //    _cropY = e.Y;
                //    _cropPen = new Pen(Color.Black, 1);
                //    _cropPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                //    this.Refresh();
                //}
                //else
                //{
                //    if (_selectedCtrlList != null && _selectedCtrlList.Count > 0)
                //    {
                //        foreach (SMCtlBase smCtrl in _selectedCtrlList)
                //        {
                //            smCtrl.OnMouseDown(null, e);
                //        }
                //    }
                //}

                if(Control.ModifierKeys == Keys.Control)
                {
                    if (_selectedCtrlList == null)
                    {
                        _cropX = e.X;
                        _cropY = e.Y;
                        _cropPen = new Pen(Color.Black, 1);
                        _cropPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        this.Refresh();
                    }
                    else
                    {
                        foreach (SMCtlBase smCtrl in _selectedCtrlList)
                        {
                            smCtrl.OnMouseDown(null, e);
                        }
                    }
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            SMContainerPanel panel = sender as SMContainerPanel;
            _draging = false;
            if (panel != null)
            {
                _lastMousePos = Point.Empty;
                panel.Cursor = Cursors.Default;

                //if (Control.ModifierKeys != Keys.Control)
                //{
                //    if (_isCropping && !PreventEdit)
                //    {
                //        GetCropControlList();
                //    }  
                //}
                //else
                //{
                //    if (_selectedCtrlList != null && _selectedCtrlList.Count > 0)
                //    {
                //        foreach (SMCtlBase smCtrl in _selectedCtrlList)
                //        {
                //            PointF newPos = SMContainerPanel.PixelToGridSnap(smCtrl.Location);
                //            smCtrl.FlowItem.GridLoc = newPos;
                //            smCtrl.OnMouseUp(null, e);
                //        }

                //    }
                //    this.Redraw();
                //}


                if (Control.ModifierKeys == Keys.Control)
                {
                    if (_selectedCtrlList == null)
                    {
                        if (_isCropping && !PreventEdit)
                        {
                            GetCropControlList();
                        }  
                    }
                    else
                    {
                        foreach (SMCtlBase smCtrl in _selectedCtrlList)
                        {
                            PointF newPos = SMContainerPanel.PixelToGridSnap(smCtrl.Location);
                            smCtrl.FlowItem.GridLoc = newPos;
                            smCtrl.OnMouseUp(null, e);
                        }
                        this.Redraw();
                    }
                }

                this.Refresh();
            }
        }

        private void OnLeftClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(_flowChartCtlBasic.RefStateMachine.LockText))
            {
                return;
            }
            if (!EditMode)
            {
                EditMode = true;
            }
            else
            {
                SMContainerPanel panel = sender as SMContainerPanel;
                CurrentSel = null;
                if (_emptyFlowSpot && panel != null && !PreventEdit)
                {
                    if (Control.ModifierKeys != Keys.Control)
                    {
                        new NewItemForm(this, PixelToGridSnap(panel.PointToClient(MousePosition))).ShowDialog();
                    }
                    else
                    {
                        if (!_draging)
                        {
                            ShowContextMenu(null, PixelToGridSnap(panel.PointToClient(MousePosition)));
                        }
                    }
                }
            }
        }

        private PointF _latestptGridLoc;
        private SMFlowBase _latestFlowItem;
        private SMFlowBase _copyFlowItem;
        public void ShowContextMenu(SMFlowBase flowItem, PointF ptGridLoc)
        {
            tsmCopyItem.Enabled = !(flowItem == null);
            tsmPasteItem.Enabled = !(_copyFlowItem == null) && flowItem == null ;
            tsmDeselectItem.Enabled = flowItem != null && flowItem.Highlighted;
            tsmDeleteItem.Enabled = flowItem != null; //|| _selectedCtrlList != null;

            //tsmCopyItem.Visible = false;
            //tsmPasteItem.Visible = false;
            _latestFlowItem = flowItem;
            _latestptGridLoc = ptGridLoc;
            contextMenuStrip.Show(MousePosition);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmInsertGripUpper = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmInsertGridBelow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmInsertGridLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmInsertGridRight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCopyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPasteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSelectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmClearSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDeselectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmInsertGripUpper,
            this.tsmInsertGridBelow,
            this.tsmInsertGridLeft,
            this.tsmInsertGridRight,
            this.toolStripSeparator1,
            this.tsmCopyItem,
            this.tsmPasteItem,
            this.tsmDeleteItem,
            this.toolStripSeparator2,
            this.tsmSelectItem,
            this.tsmDeselectItem,
            this.tsmClearSelection});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(170, 236);
            this.contextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // tsmInsertGripUpper
            // 
            this.tsmInsertGripUpper.Name = "tsmInsertGripUpper";
            this.tsmInsertGripUpper.Size = new System.Drawing.Size(169, 22);
            this.tsmInsertGripUpper.Text = "Insert Grid Upper";
            // 
            // tsmInsertGridBelow
            // 
            this.tsmInsertGridBelow.Name = "tsmInsertGridBelow";
            this.tsmInsertGridBelow.Size = new System.Drawing.Size(169, 22);
            this.tsmInsertGridBelow.Text = "Insert Grid Below";
            // 
            // tsmInsertGridLeft
            // 
            this.tsmInsertGridLeft.Name = "tsmInsertGridLeft";
            this.tsmInsertGridLeft.Size = new System.Drawing.Size(169, 22);
            this.tsmInsertGridLeft.Text = "Insert Grid Left";
            // 
            // tsmInsertGridRight
            // 
            this.tsmInsertGridRight.Name = "tsmInsertGridRight";
            this.tsmInsertGridRight.Size = new System.Drawing.Size(169, 22);
            this.tsmInsertGridRight.Text = "Insert Grid Right";
            // 
            // tsmCopyItem
            // 
            this.tsmCopyItem.Name = "tsmCopyItem";
            this.tsmCopyItem.Size = new System.Drawing.Size(169, 22);
            this.tsmCopyItem.Text = "Copy Item";
            // 
            // tsmPasteItem
            // 
            this.tsmPasteItem.Name = "tsmPasteItem";
            this.tsmPasteItem.Size = new System.Drawing.Size(169, 22);
            this.tsmPasteItem.Text = "Paste Item";
            // 
            // tsmDeleteItem
            // 
            this.tsmDeleteItem.Name = "tsmDeleteItem";
            this.tsmDeleteItem.Size = new System.Drawing.Size(169, 22);
            this.tsmDeleteItem.Text = "Delete Item";
            // 
            // tsmSelectItem
            // 
            this.tsmSelectItem.Name = "tsmSelectItem";
            this.tsmSelectItem.Size = new System.Drawing.Size(169, 22);
            this.tsmSelectItem.Text = "Select Item";
            // 
            // tsmClearSelection
            // 
            this.tsmClearSelection.Name = "tsmClearSelection";
            this.tsmClearSelection.Size = new System.Drawing.Size(169, 22);
            this.tsmClearSelection.Text = "Clear All Selection";
            // 
            // tsmDeselectItem
            // 
            this.tsmDeselectItem.Name = "tsmDeselectItem";
            this.tsmDeselectItem.Size = new System.Drawing.Size(169, 22);
            this.tsmDeselectItem.Text = "Deselect Item";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        public void InsertGrid(PointF ptGridPt,eInsertGridMode insertMode)
        {
            switch(insertMode)
            {
                case eInsertGridMode.rowBefore:
                    _flowContainer.GridSize = new Size(_flowContainer.GridSize.Width, _flowContainer.GridSize.Height + 1);
                    foreach (SMFlowBase flow in _flowContainer.ChildArray)
                    {
                        if (flow.GridLoc.Y > ptGridPt.Y -1)
                        {
                            flow.GridLoc = new PointF(flow.GridLoc.X, flow.GridLoc.Y + 1);
                            ReconnectPathLine(ptGridPt,flow, insertMode);
                        }
                    }
                    break;
                case eInsertGridMode.rowAfter:
                    _flowContainer.GridSize = new Size(_flowContainer.GridSize.Width, _flowContainer.GridSize.Height + 1);
                    foreach (SMFlowBase flow in _flowContainer.ChildArray)
                    {
                        if (flow.GridLoc.Y > ptGridPt.Y)
                        {
                            flow.GridLoc = new PointF(flow.GridLoc.X, flow.GridLoc.Y + 1);
                            ReconnectPathLine(ptGridPt, flow, insertMode);
                        }
                    }
                    break;
                case eInsertGridMode.columnBefore:
                    _flowContainer.GridSize = new Size(_flowContainer.GridSize.Width + 1, _flowContainer.GridSize.Height);
                    foreach (SMFlowBase flow in _flowContainer.ChildArray)
                    {
                        if (flow.GridLoc.X > ptGridPt.X-1)
                        {
                            flow.GridLoc = new PointF(flow.GridLoc.X + 1, flow.GridLoc.Y);
                            ReconnectPathLine(ptGridPt, flow, insertMode);
                        }
                    }
                    break;
                case eInsertGridMode.columnAfter:
                    _flowContainer.GridSize = new Size(_flowContainer.GridSize.Width + 1, _flowContainer.GridSize.Height);
                    foreach (SMFlowBase flow in _flowContainer.ChildArray)
                    {
                        if (flow.GridLoc.X > ptGridPt.X)
                        {
                            flow.GridLoc = new PointF(flow.GridLoc.X + 1, flow.GridLoc.Y);
                            ReconnectPathLine(ptGridPt, flow, insertMode);
                        }
                    }
                    break;

            }
            
            // Set the size of the panel
            Size = GridToPixel(_flowContainer.GridSize);
            Rebuild();
            Redraw();
        }

        private void ReconnectPathLine(PointF ptGridPt,SMFlowBase targetFlow,eInsertGridMode insertMode)
        {
            switch (insertMode)
            {
                case eInsertGridMode.rowBefore:
                    foreach (SMFlowBase flow in _flowContainer.ChildArray)
                    {
                        if(flow.GridLoc.Y >= ptGridPt.Y)
                        {
                            continue;
                        }
                        foreach (SMPathOut pathOut in flow.PathArray)
                        {
                            if (pathOut.TargetID == targetFlow.Name)
                            {
                                SMPathSegment verticalSecment = GetVerticalPathSecment(pathOut.Last, eVerticalDir.Down);
                                if (verticalSecment != null)
                                {
                                    verticalSecment.GridDistance += 1;
                                }
                            }
                        }

                        foreach(SMPathOut pathOut in targetFlow.PathArray)
                        {
                            if (pathOut.TargetID == flow.Name)
                            {
                                SMPathSegment verticalSecment = GetVerticalPathSecment(pathOut.Last, eVerticalDir.Up);
                                if (verticalSecment != null)
                                {
                                    verticalSecment.GridDistance -= 1;
                                }
                            }
                        }
                    }

                    break;

                case eInsertGridMode.rowAfter:
                    foreach (SMFlowBase flow in _flowContainer.ChildArray)
                    {
                        if (flow.GridLoc.Y >= ptGridPt.Y + 1)
                        {
                            continue;
                        }
                        foreach (SMPathOut pathOut in flow.PathArray)
                        {
                            if (pathOut.TargetID == targetFlow.Name)
                            {
                                SMPathSegment verticalSecment = GetVerticalPathSecment(pathOut.Last, eVerticalDir.Down);
                                if (verticalSecment != null)
                                {
                                    verticalSecment.GridDistance += 1;
                                }
                            }
                        }

                        foreach (SMPathOut pathOut in targetFlow.PathArray)
                        {
                            if (pathOut.TargetID == flow.Name)
                            {
                                SMPathSegment verticalSecment = GetVerticalPathSecment(pathOut.Last, eVerticalDir.Up);
                                if (verticalSecment != null)
                                {
                                    verticalSecment.GridDistance -= 1;
                                }
                            }
                        }
                    }

                    break;

                case eInsertGridMode.columnBefore:
                    foreach (SMFlowBase flow in _flowContainer.ChildArray)
                    {
                        if (flow.GridLoc.X >= ptGridPt.X)
                        {
                            continue;
                        }
                        foreach (SMPathOut pathOut in flow.PathArray)
                        {
                            if (pathOut.TargetID == targetFlow.Name)
                            {
                                SMPathSegment horizontalSecment = GetHorizontalPathSecment(pathOut.Last, eHorizontalDir.Right);
                                if (horizontalSecment != null)
                                {
                                    horizontalSecment.GridDistance += 1;
                                }
                            }
                        }

                        foreach (SMPathOut pathOut in targetFlow.PathArray)
                        {
                            if (pathOut.TargetID == flow.Name)
                            {
                                SMPathSegment horizontalSecment = GetHorizontalPathSecment(pathOut.Last, eHorizontalDir.Left);
                                if (horizontalSecment != null)
                                {
                                    horizontalSecment.GridDistance -= 1;
                                }
                            }
                        }
                    }

                    break;

                case eInsertGridMode.columnAfter:
                    foreach (SMFlowBase flow in _flowContainer.ChildArray)
                    {
                        if (flow.GridLoc.X >= ptGridPt.X + 1)
                        {
                            continue;
                        }
                        foreach (SMPathOut pathOut in flow.PathArray)
                        {
                            if (pathOut.TargetID == targetFlow.Name)
                            {
                                SMPathSegment horizontalSecment = GetHorizontalPathSecment(pathOut.Last, eHorizontalDir.Right);
                                if (horizontalSecment != null)
                                {
                                    horizontalSecment.GridDistance += 1;
                                }
                            }
                        }

                        foreach (SMPathOut pathOut in targetFlow.PathArray)
                        {
                            if (pathOut.TargetID == flow.Name)
                            {
                                SMPathSegment horizontalSecment = GetHorizontalPathSecment(pathOut.Last, eHorizontalDir.Left);
                                if (horizontalSecment != null)
                                {
                                    horizontalSecment.GridDistance -= 1;
                                }
                            }
                        }
                    }

                    break;

                default:
                    break;
            }
            
            
        
        }

        private enum eVerticalDir
        {
            Up,
            Down,
        }

        private SMPathSegment GetVerticalPathSecment(SMPathSegment endPath,eVerticalDir verDir)
        {
            if((verDir == eVerticalDir.Down && endPath.Vertical && endPath.GridDistance > 0)||
               (verDir == eVerticalDir.Up && endPath.Vertical && endPath.GridDistance < 0))
            {
                return endPath;
            }
            else if(endPath.Previous != null)
            {
                return GetVerticalPathSecment(endPath.Previous,verDir);
            }
            else
            {
                return null;
            }
        }


        private enum eHorizontalDir
        {
            Left,
            Right,
        }

        private SMPathSegment GetHorizontalPathSecment(SMPathSegment endPath, eHorizontalDir horDir)
        {
            if ((horDir ==  eHorizontalDir.Right && !endPath.Vertical && endPath.GridDistance > 0) ||
               (horDir ==  eHorizontalDir.Left && !endPath.Vertical && endPath.GridDistance < 0))
            {
                return endPath;
            }
            else if (endPath.Previous != null)
            {
                return GetHorizontalPathSecment(endPath.Previous, horDir);
            }
            else
            {
                return null;
            }
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuStrip.Close();
            if(e.ClickedItem == tsmInsertGripUpper)
            {
                ClearAllSelection();
                InsertGrid(_latestptGridLoc,eInsertGridMode.rowBefore);
            }
            else if (e.ClickedItem == tsmInsertGridBelow)
            {
                ClearAllSelection();
                InsertGrid(_latestptGridLoc,eInsertGridMode.rowAfter);
            }
            else if (e.ClickedItem == tsmInsertGridLeft)
            {
                ClearAllSelection();
                InsertGrid(_latestptGridLoc,eInsertGridMode.columnBefore);
            }
            else if (e.ClickedItem == tsmInsertGridRight)
            {
                ClearAllSelection();
                InsertGrid(_latestptGridLoc, eInsertGridMode.columnAfter);
            }
            else if (e.ClickedItem == tsmCopyItem)
            {
                if(_latestFlowItem != null)
                {
                    _copyFlowItem = _latestFlowItem.Clone("", true) as SMFlowBase;
                    _latestFlowItem.ShallowCopyTo(_copyFlowItem);
                }
            }
            else if (e.ClickedItem == tsmPasteItem)
            {
                SMFlowBase newCopyflowItem = _copyFlowItem;
                newCopyflowItem.Name = "";
                _flowContainer.AddFlowItem(newCopyflowItem);
                newCopyflowItem.Text = _copyFlowItem.Text;
                newCopyflowItem.GridLoc = _latestptGridLoc;
                
                U.LogChangeAdded(string.Format("{0}.{1}", newCopyflowItem.Nickname, _copyFlowItem.Text));
                Build(newCopyflowItem);
                newCopyflowItem.Rebuild();
                Redraw(newCopyflowItem);

                if (newCopyflowItem is SMFlowContainer)
                {
                    RecurseRebuild(newCopyflowItem);
                }

                _copyFlowItem = null;
            }
            else if (e.ClickedItem == tsmDeleteItem)
            {
                if (MessageBox.Show("This Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if(_latestFlowItem != null && this._flowContainer.ChildArray.Contains(_latestFlowItem))
                    {
                        SMCtlBase deleteFlowCtrl = GetFlowCtl(_latestFlowItem);
                        if (_selectedCtrlList != null && _selectedCtrlList.Contains(deleteFlowCtrl))
                        {
                            _selectedCtrlList.Remove(deleteFlowCtrl);
                        }
                        this.DeleteFlowItem(_latestFlowItem);
                        
                    }
                    _latestFlowItem = null;
                }

            }
            else if(e.ClickedItem == tsmSelectItem)
            {
                SMCtlBase smCtrl = this.GetFlowCtl(_latestFlowItem);
                if (_selectedCtrlList == null)
                {
                    _selectedCtrlList = new List<SMCtlBase>();
                }
                _selectedCtrlList.Add(smCtrl);
                smCtrl.FlowItem.Highlighted = true;
            }
            else if (e.ClickedItem == tsmDeselectItem)
            {
                SMCtlBase smCtrl = this.GetFlowCtl(_latestFlowItem);
                smCtrl.FlowItem.Highlighted = false;
                _selectedCtrlList.Remove(smCtrl);
                if(_selectedCtrlList.Count == 0)
                {
                    _selectedCtrlList = null;
                }
                
            }
            else if(e.ClickedItem == tsmClearSelection)
            {
                ClearAllSelection();
            }
        }


        private void ClearAllSelection()
        {
            foreach (Control ctrl in this.Controls)
            {
                SMCtlBase smCtrl = ctrl is SMCtlBase ? ctrl as SMCtlBase : null;
                if (smCtrl != null)
                {
                    smCtrl.FlowItem.Highlighted = false;
                }
            }
            _selectedCtrlList = null;
            _latestFlowItem = null;
        }


        private void RecurseRebuild(SMFlowBase flow)
        {
            flow.Rebuild();
            if (flow.ChildArray != null)
            {
                foreach (SMFlowBase childFlow in flow.ChildArray)
                {
                    if (childFlow is SMFlowContainer)
                    {
                        RecurseRebuild(childFlow);
                    }
                    else
                    {
                        childFlow.Rebuild();
                    }
                }
            }
        }


        private void GetCropControlList()
        {
            _selectedCtrlList = new List<SMCtlBase>();

            PointF StartGrid = PixelToGrid(new Point(_oCropX,_oCropY));
            PointF endGrid = PixelToGrid(new Point(_oCropX + _cropWidth, _oCropY + _cropHeight));
            foreach(Control ctrl in this.Controls)
            {
                SMCtlBase smCtrl = ctrl is SMCtlBase ? ctrl as SMCtlBase : null;
                if (smCtrl != null)
                {
                    if (smCtrl.GridLoc.X > StartGrid.X &&
                    smCtrl.GridLoc.Y > StartGrid.Y &&
                    smCtrl.GridLoc.X < endGrid.X &&
                    smCtrl.GridLoc.Y < endGrid.Y)
                    {
                        _selectedCtrlList.Add(smCtrl);
                        smCtrl.FlowItem.Highlighted = true;
                    }
                    else
                    {
                        smCtrl.FlowItem.Highlighted = false;
                    }
                }
            }

            if (_selectedCtrlList.Count == 0)
            {
                _selectedCtrlList = null;
            }
        }

    }
}
