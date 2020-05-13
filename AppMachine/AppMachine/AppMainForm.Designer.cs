namespace AppMachine
{
    partial class AppMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLockoutTime = new System.Windows.Forms.Label();
            this.btnLockOut = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnHomeAll = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnApply = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEStop = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.mcbStopWhenFinished = new ComponentFactory.Krypton.Toolkit.KryptonCheckButton();
            this.btnPause = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnRun = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.Color.Navy;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.Color.Navy;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.Color.Navy;
            this.splitContainer.Panel2.Controls.Add(this.label1);
            this.splitContainer.Panel2.Controls.Add(this.lblLockoutTime);
            this.splitContainer.Panel2.Controls.Add(this.btnLockOut);
            this.splitContainer.Panel2.Controls.Add(this.btnHomeAll);
            this.splitContainer.Panel2.Controls.Add(this.btnApply);
            this.splitContainer.Panel2.Controls.Add(this.btnEStop);
            this.splitContainer.Panel2.Controls.Add(this.mcbStopWhenFinished);
            this.splitContainer.Panel2.Controls.Add(this.btnPause);
            this.splitContainer.Panel2.Controls.Add(this.btnRun);
            this.splitContainer.Size = new System.Drawing.Size(1264, 986);
            this.splitContainer.SplitterDistance = 932;
            this.splitContainer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(966, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "LOCK OUT ";
            // 
            // lblLockoutTime
            // 
            this.lblLockoutTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLockoutTime.AutoSize = true;
            this.lblLockoutTime.ForeColor = System.Drawing.Color.White;
            this.lblLockoutTime.Location = new System.Drawing.Point(973, 28);
            this.lblLockoutTime.Name = "lblLockoutTime";
            this.lblLockoutTime.Size = new System.Drawing.Size(49, 13);
            this.lblLockoutTime.TabIndex = 24;
            this.lblLockoutTime.Text = "00:00:00";
            // 
            // btnLockOut
            // 
            this.btnLockOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLockOut.Location = new System.Drawing.Point(1029, 5);
            this.btnLockOut.Name = "btnLockOut";
            this.btnLockOut.OverrideDefault.Back.Color1 = System.Drawing.Color.Black;
            this.btnLockOut.OverrideDefault.Back.Color2 = System.Drawing.Color.Black;
            this.btnLockOut.OverrideDefault.Border.Color1 = System.Drawing.Color.White;
            this.btnLockOut.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnLockOut.OverrideDefault.Border.Rounding = 15;
            this.btnLockOut.OverrideDefault.Border.Width = 2;
            this.btnLockOut.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnLockOut.OverrideDefault.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnLockOut.Size = new System.Drawing.Size(71, 39);
            this.btnLockOut.StateCommon.Back.Color1 = System.Drawing.Color.Black;
            this.btnLockOut.StateCommon.Back.Color2 = System.Drawing.Color.Black;
            this.btnLockOut.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.btnLockOut.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnLockOut.StateCommon.Border.Rounding = 15;
            this.btnLockOut.StateCommon.Border.Width = 2;
            this.btnLockOut.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnLockOut.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnLockOut.StateDisabled.Back.Color1 = System.Drawing.Color.Silver;
            this.btnLockOut.StateDisabled.Back.Color2 = System.Drawing.Color.Gray;
            this.btnLockOut.StateDisabled.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Dashed;
            this.btnLockOut.StateDisabled.Border.Color1 = System.Drawing.Color.Gray;
            this.btnLockOut.StateDisabled.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnLockOut.StateDisabled.Border.Rounding = 15;
            this.btnLockOut.StateDisabled.Border.Width = 2;
            this.btnLockOut.StateDisabled.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLockOut.StateDisabled.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnLockOut.TabIndex = 23;
            this.btnLockOut.Values.Text = "LOCK";
            this.btnLockOut.Click += new System.EventHandler(this.btnLockOut_Click);
            // 
            // btnHomeAll
            // 
            this.btnHomeAll.Location = new System.Drawing.Point(564, 5);
            this.btnHomeAll.Name = "btnHomeAll";
            this.btnHomeAll.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnHomeAll.OverrideDefault.Back.Color2 = System.Drawing.Color.Olive;
            this.btnHomeAll.OverrideDefault.Border.Color1 = System.Drawing.Color.White;
            this.btnHomeAll.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnHomeAll.OverrideDefault.Border.Rounding = 15;
            this.btnHomeAll.OverrideDefault.Border.Width = 2;
            this.btnHomeAll.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnHomeAll.OverrideDefault.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnHomeAll.Size = new System.Drawing.Size(129, 39);
            this.btnHomeAll.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnHomeAll.StateCommon.Back.Color2 = System.Drawing.Color.Olive;
            this.btnHomeAll.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.btnHomeAll.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnHomeAll.StateCommon.Border.Rounding = 15;
            this.btnHomeAll.StateCommon.Border.Width = 2;
            this.btnHomeAll.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnHomeAll.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnHomeAll.StateDisabled.Back.Color1 = System.Drawing.Color.Silver;
            this.btnHomeAll.StateDisabled.Back.Color2 = System.Drawing.Color.Gray;
            this.btnHomeAll.StateDisabled.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Dashed;
            this.btnHomeAll.StateDisabled.Border.Color1 = System.Drawing.Color.Gray;
            this.btnHomeAll.StateDisabled.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnHomeAll.StateDisabled.Border.Rounding = 15;
            this.btnHomeAll.StateDisabled.Border.Width = 2;
            this.btnHomeAll.StateDisabled.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnHomeAll.StateDisabled.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnHomeAll.TabIndex = 22;
            this.btnHomeAll.Values.Text = "HOME ALL";
            this.btnHomeAll.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(1131, 5);
            this.btnApply.Name = "btnApply";
            this.btnApply.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnApply.OverrideDefault.Back.Color2 = System.Drawing.Color.Navy;
            this.btnApply.OverrideDefault.Border.Color1 = System.Drawing.Color.White;
            this.btnApply.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnApply.OverrideDefault.Border.Rounding = 15;
            this.btnApply.OverrideDefault.Border.Width = 2;
            this.btnApply.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnApply.OverrideDefault.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnApply.Size = new System.Drawing.Size(129, 39);
            this.btnApply.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnApply.StateCommon.Back.Color2 = System.Drawing.Color.Navy;
            this.btnApply.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.btnApply.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnApply.StateCommon.Border.Rounding = 15;
            this.btnApply.StateCommon.Border.Width = 2;
            this.btnApply.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnApply.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnApply.StateDisabled.Back.Color1 = System.Drawing.Color.Silver;
            this.btnApply.StateDisabled.Back.Color2 = System.Drawing.Color.Gray;
            this.btnApply.StateDisabled.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Dashed;
            this.btnApply.StateDisabled.Border.Color1 = System.Drawing.Color.Gray;
            this.btnApply.StateDisabled.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnApply.StateDisabled.Border.Rounding = 15;
            this.btnApply.StateDisabled.Border.Width = 2;
            this.btnApply.StateDisabled.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnApply.StateDisabled.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnApply.TabIndex = 21;
            this.btnApply.Values.Text = "APPLY";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnEStop
            // 
            this.btnEStop.Location = new System.Drawing.Point(424, 5);
            this.btnEStop.Name = "btnEStop";
            this.btnEStop.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnEStop.OverrideDefault.Back.Color2 = System.Drawing.Color.Maroon;
            this.btnEStop.OverrideDefault.Border.Color1 = System.Drawing.Color.White;
            this.btnEStop.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEStop.OverrideDefault.Border.Rounding = 15;
            this.btnEStop.OverrideDefault.Border.Width = 2;
            this.btnEStop.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnEStop.OverrideDefault.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnEStop.Size = new System.Drawing.Size(129, 39);
            this.btnEStop.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnEStop.StateCommon.Back.Color2 = System.Drawing.Color.Maroon;
            this.btnEStop.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.btnEStop.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEStop.StateCommon.Border.Rounding = 15;
            this.btnEStop.StateCommon.Border.Width = 2;
            this.btnEStop.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnEStop.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnEStop.StateDisabled.Back.Color1 = System.Drawing.Color.Silver;
            this.btnEStop.StateDisabled.Back.Color2 = System.Drawing.Color.Gray;
            this.btnEStop.StateDisabled.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Dashed;
            this.btnEStop.StateDisabled.Border.Color1 = System.Drawing.Color.Gray;
            this.btnEStop.StateDisabled.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEStop.StateDisabled.Border.Rounding = 15;
            this.btnEStop.StateDisabled.Border.Width = 2;
            this.btnEStop.StateDisabled.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEStop.StateDisabled.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnEStop.TabIndex = 19;
            this.btnEStop.Values.Text = "ESTOP";
            this.btnEStop.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // mcbStopWhenFinished
            // 
            this.mcbStopWhenFinished.Location = new System.Drawing.Point(284, 5);
            this.mcbStopWhenFinished.Name = "mcbStopWhenFinished";
            this.mcbStopWhenFinished.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.mcbStopWhenFinished.OverrideDefault.Back.Color2 = System.Drawing.Color.Red;
            this.mcbStopWhenFinished.OverrideDefault.Border.Color1 = System.Drawing.Color.White;
            this.mcbStopWhenFinished.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.mcbStopWhenFinished.OverrideDefault.Border.Rounding = 15;
            this.mcbStopWhenFinished.OverrideDefault.Border.Width = 2;
            this.mcbStopWhenFinished.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.mcbStopWhenFinished.OverrideDefault.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.mcbStopWhenFinished.Size = new System.Drawing.Size(129, 39);
            this.mcbStopWhenFinished.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.mcbStopWhenFinished.StateCommon.Back.Color2 = System.Drawing.Color.Red;
            this.mcbStopWhenFinished.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.mcbStopWhenFinished.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.mcbStopWhenFinished.StateCommon.Border.Rounding = 15;
            this.mcbStopWhenFinished.StateCommon.Border.Width = 2;
            this.mcbStopWhenFinished.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.mcbStopWhenFinished.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.mcbStopWhenFinished.StateDisabled.Back.Color1 = System.Drawing.Color.Silver;
            this.mcbStopWhenFinished.StateDisabled.Back.Color2 = System.Drawing.Color.Gray;
            this.mcbStopWhenFinished.StateDisabled.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Dashed;
            this.mcbStopWhenFinished.StateDisabled.Border.Color1 = System.Drawing.Color.Gray;
            this.mcbStopWhenFinished.StateDisabled.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.mcbStopWhenFinished.StateDisabled.Border.Rounding = 15;
            this.mcbStopWhenFinished.StateDisabled.Border.Width = 2;
            this.mcbStopWhenFinished.StateDisabled.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.mcbStopWhenFinished.StateDisabled.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.mcbStopWhenFinished.TabIndex = 18;
            this.mcbStopWhenFinished.Values.Text = "STOP";
            this.mcbStopWhenFinished.CheckedChanged += new System.EventHandler(this.mcbStopWhenFinished_CheckedChanged);
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Location = new System.Drawing.Point(144, 5);
            this.btnPause.Name = "btnPause";
            this.btnPause.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnPause.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPause.OverrideDefault.Border.Color1 = System.Drawing.Color.White;
            this.btnPause.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPause.OverrideDefault.Border.Rounding = 15;
            this.btnPause.OverrideDefault.Border.Width = 2;
            this.btnPause.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnPause.OverrideDefault.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPause.Size = new System.Drawing.Size(129, 39);
            this.btnPause.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnPause.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPause.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.btnPause.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPause.StateCommon.Border.Rounding = 15;
            this.btnPause.StateCommon.Border.Width = 2;
            this.btnPause.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnPause.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPause.StateDisabled.Back.Color1 = System.Drawing.Color.Silver;
            this.btnPause.StateDisabled.Back.Color2 = System.Drawing.Color.Gray;
            this.btnPause.StateDisabled.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Dashed;
            this.btnPause.StateDisabled.Border.Color1 = System.Drawing.Color.Gray;
            this.btnPause.StateDisabled.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPause.StateDisabled.Border.Rounding = 15;
            this.btnPause.StateDisabled.Border.Width = 2;
            this.btnPause.StateDisabled.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPause.StateDisabled.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPause.TabIndex = 16;
            this.btnPause.Values.Text = "PAUSE";
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnRun
            // 
            this.btnRun.Enabled = false;
            this.btnRun.Location = new System.Drawing.Point(4, 5);
            this.btnRun.Name = "btnRun";
            this.btnRun.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnRun.OverrideDefault.Back.Color2 = System.Drawing.Color.Green;
            this.btnRun.OverrideDefault.Border.Color1 = System.Drawing.Color.White;
            this.btnRun.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRun.OverrideDefault.Border.Rounding = 15;
            this.btnRun.OverrideDefault.Border.Width = 2;
            this.btnRun.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnRun.OverrideDefault.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnRun.Size = new System.Drawing.Size(129, 39);
            this.btnRun.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnRun.StateCommon.Back.Color2 = System.Drawing.Color.Green;
            this.btnRun.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.btnRun.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRun.StateCommon.Border.Rounding = 15;
            this.btnRun.StateCommon.Border.Width = 2;
            this.btnRun.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnRun.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnRun.StateDisabled.Back.Color1 = System.Drawing.Color.Silver;
            this.btnRun.StateDisabled.Back.Color2 = System.Drawing.Color.Gray;
            this.btnRun.StateDisabled.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Dashed;
            this.btnRun.StateDisabled.Border.Color1 = System.Drawing.Color.Gray;
            this.btnRun.StateDisabled.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRun.StateDisabled.Border.Rounding = 15;
            this.btnRun.StateDisabled.Border.Width = 2;
            this.btnRun.StateDisabled.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRun.StateDisabled.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnRun.TabIndex = 15;
            this.btnRun.Values.Text = "RUN";
            this.btnRun.Click += new System.EventHandler(this.btnStartRun_Click);
            // 
            // AppMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 986);
            this.Controls.Add(this.splitContainer);
            this.Name = "AppMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StateCommon.Back.Color1 = System.Drawing.Color.Navy;
            this.StateCommon.Back.Color2 = System.Drawing.Color.Navy;
            this.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.StateCommon.Border.Rounding = 10;
            this.StateCommon.Border.Width = 1;
            this.StateCommon.Header.Back.Color1 = System.Drawing.Color.Navy;
            this.StateCommon.Header.Back.Color2 = System.Drawing.Color.Navy;
            this.StateCommon.Header.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.StateCommon.Header.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRun;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckButton mcbStopWhenFinished;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPause;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnApply;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEStop;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnHomeAll;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnLockOut;
        private System.Windows.Forms.Label lblLockoutTime;
        private System.Windows.Forms.Label label1;
    }
}

