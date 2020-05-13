namespace AppMachine.Comp.Motion
{
    partial class AppMotionAxisCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbAxis = new System.Windows.Forms.GroupBox();
            this.mCbEnableAxis = new MCore.Controls.MCheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelSafetyInputs = new System.Windows.Forms.Panel();
            this._lnkDockFloating = new System.Windows.Forms.LinkLabel();
            this.panelCuurentPos = new System.Windows.Forms.Panel();
            this.mDCurrentPosition = new MCore.Controls.MDoubleWithUnits();
            this.mDJogStep = new MCore.Controls.MDoubleWithUnits();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnReverse = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.gbAxis.SuspendLayout();
            this.panelCuurentPos.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAxis
            // 
            this.gbAxis.Controls.Add(this.btnReset);
            this.gbAxis.Controls.Add(this.mCbEnableAxis);
            this.gbAxis.Controls.Add(this.label1);
            this.gbAxis.Controls.Add(this.panelSafetyInputs);
            this.gbAxis.Controls.Add(this._lnkDockFloating);
            this.gbAxis.Controls.Add(this.panelCuurentPos);
            this.gbAxis.Controls.Add(this.mDJogStep);
            this.gbAxis.Controls.Add(this.btnHome);
            this.gbAxis.Controls.Add(this.btnReverse);
            this.gbAxis.Controls.Add(this.btnForward);
            this.gbAxis.Location = new System.Drawing.Point(6, 2);
            this.gbAxis.Name = "gbAxis";
            this.gbAxis.Size = new System.Drawing.Size(342, 133);
            this.gbAxis.TabIndex = 8;
            this.gbAxis.TabStop = false;
            this.gbAxis.Text = "AxisName";
            // 
            // mCbEnableAxis
            // 
            this.mCbEnableAxis.AutoSize = true;
            this.mCbEnableAxis.Location = new System.Drawing.Point(7, 110);
            this.mCbEnableAxis.LogChanges = true;
            this.mCbEnableAxis.Name = "mCbEnableAxis";
            this.mCbEnableAxis.Size = new System.Drawing.Size(99, 17);
            this.mCbEnableAxis.TabIndex = 20;
            this.mCbEnableAxis.Text = "Enable/Disable";
            this.mCbEnableAxis.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "SafetyIO";
            // 
            // panelSafetyInputs
            // 
            this.panelSafetyInputs.Location = new System.Drawing.Point(140, 62);
            this.panelSafetyInputs.Name = "panelSafetyInputs";
            this.panelSafetyInputs.Size = new System.Drawing.Size(196, 65);
            this.panelSafetyInputs.TabIndex = 18;
            // 
            // _lnkDockFloating
            // 
            this._lnkDockFloating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._lnkDockFloating.AutoSize = true;
            this._lnkDockFloating.Location = new System.Drawing.Point(301, 0);
            this._lnkDockFloating.Name = "_lnkDockFloating";
            this._lnkDockFloating.Size = new System.Drawing.Size(44, 13);
            this._lnkDockFloating.TabIndex = 9;
            this._lnkDockFloating.TabStop = true;
            this._lnkDockFloating.Text = "Floating";
            this._lnkDockFloating.VisitedLinkColor = System.Drawing.Color.Blue;
            this._lnkDockFloating.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkDockFloating_LinkClicked);
            // 
            // panelCuurentPos
            // 
            this.panelCuurentPos.Controls.Add(this.mDCurrentPosition);
            this.panelCuurentPos.Enabled = false;
            this.panelCuurentPos.Location = new System.Drawing.Point(91, 31);
            this.panelCuurentPos.Name = "panelCuurentPos";
            this.panelCuurentPos.Size = new System.Drawing.Size(245, 28);
            this.panelCuurentPos.TabIndex = 17;
            // 
            // mDCurrentPosition
            // 
            this.mDCurrentPosition.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mDCurrentPosition.DoubleVal = 0D;
            this.mDCurrentPosition.Enabled = false;
            this.mDCurrentPosition.Label = "";
            this.mDCurrentPosition.Location = new System.Drawing.Point(13, 3);
            this.mDCurrentPosition.LogChanges = true;
            this.mDCurrentPosition.Name = "mDCurrentPosition";
            this.mDCurrentPosition.Padding = new System.Windows.Forms.Padding(2);
            this.mDCurrentPosition.Size = new System.Drawing.Size(232, 22);
            this.mDCurrentPosition.TabIndex = 16;
            this.mDCurrentPosition.TextBackColor = System.Drawing.SystemColors.Window;
            this.mDCurrentPosition.UnitsLabel = "Unit";
            // 
            // mDJogStep
            // 
            this.mDJogStep.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mDJogStep.DoubleVal = 0D;
            this.mDJogStep.Enabled = false;
            this.mDJogStep.Label = "";
            this.mDJogStep.Location = new System.Drawing.Point(104, 10);
            this.mDJogStep.LogChanges = true;
            this.mDJogStep.Name = "mDJogStep";
            this.mDJogStep.Padding = new System.Windows.Forms.Padding(2);
            this.mDJogStep.Size = new System.Drawing.Size(232, 22);
            this.mDJogStep.TabIndex = 15;
            this.mDJogStep.TextBackColor = System.Drawing.SystemColors.Window;
            this.mDJogStep.UnitsLabel = "Unit";
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.LightSalmon;
            this.btnHome.Location = new System.Drawing.Point(7, 78);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(78, 26);
            this.btnHome.TabIndex = 6;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnReverse
            // 
            this.btnReverse.BackColor = System.Drawing.Color.Pink;
            this.btnReverse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnReverse.Location = new System.Drawing.Point(7, 19);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(78, 25);
            this.btnReverse.TabIndex = 0;
            this.btnReverse.Text = "--";
            this.btnReverse.UseVisualStyleBackColor = false;
            this.btnReverse.Click += new System.EventHandler(this.btnReverse_Click);
            // 
            // btnForward
            // 
            this.btnForward.BackColor = System.Drawing.Color.Lime;
            this.btnForward.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnForward.Location = new System.Drawing.Point(7, 49);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(78, 26);
            this.btnForward.TabIndex = 1;
            this.btnForward.Text = "++";
            this.btnForward.UseVisualStyleBackColor = false;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Red;
            this.btnReset.Location = new System.Drawing.Point(91, 78);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(43, 26);
            this.btnReset.TabIndex = 21;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // AppMotionAxisCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbAxis);
            this.Name = "AppMotionAxisCtrl";
            this.Size = new System.Drawing.Size(355, 136);
            this.gbAxis.ResumeLayout(false);
            this.gbAxis.PerformLayout();
            this.panelCuurentPos.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAxis;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnReverse;
        private System.Windows.Forms.Button btnForward;
        private MCore.Controls.MDoubleWithUnits mDCurrentPosition;
        private MCore.Controls.MDoubleWithUnits mDJogStep;
        private System.Windows.Forms.Panel panelCuurentPos;
        protected System.Windows.Forms.LinkLabel _lnkDockFloating;
        private System.Windows.Forms.Panel panelSafetyInputs;
        private System.Windows.Forms.Label label1;
        private MCore.Controls.MCheckBox mCbEnableAxis;
        private System.Windows.Forms.Button btnReset;
    }
}
