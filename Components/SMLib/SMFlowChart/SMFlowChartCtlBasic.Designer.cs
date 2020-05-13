using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.SMLib.SMFlowChart
{
    partial class SMFlowChartCtlBasic
    {

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbSubroutineName = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.cbAutoScope = new System.Windows.Forms.CheckBox();
            this.smFlowPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHeader.Location = new System.Drawing.Point(200, 1);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(610, 23);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Title";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnClose.Location = new System.Drawing.Point(977, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(22, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbSubroutineName
            // 
            this.tbSubroutineName.Location = new System.Drawing.Point(202, 1);
            this.tbSubroutineName.Name = "tbSubroutineName";
            this.tbSubroutineName.Size = new System.Drawing.Size(224, 20);
            this.tbSubroutineName.TabIndex = 2;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(0, 0);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(50, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(50, 0);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(50, 23);
            this.btnStep.TabIndex = 4;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(100, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(50, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(150, 0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(50, 23);
            this.btnPause.TabIndex = 6;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // cbAutoScope
            // 
            this.cbAutoScope.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoScope.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAutoScope.AutoSize = true;
            this.cbAutoScope.Location = new System.Drawing.Point(870, 1);
            this.cbAutoScope.Name = "cbAutoScope";
            this.cbAutoScope.Size = new System.Drawing.Size(108, 23);
            this.cbAutoScope.TabIndex = 7;
            this.cbAutoScope.Text = "Auto Scope : None";
            this.cbAutoScope.UseVisualStyleBackColor = true;
            this.cbAutoScope.CheckedChanged += new System.EventHandler(this.OnChangedAutoScope);
            this.cbAutoScope.SizeChanged += new System.EventHandler(this.OnChangedAutoScopeSize);
            // 
            // smFlowPanel
            // 
            this.smFlowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smFlowPanel.AutoScroll = true;
            this.smFlowPanel.BackColor = System.Drawing.SystemColors.Control;
            this.smFlowPanel.Location = new System.Drawing.Point(1, 27);
            this.smFlowPanel.Name = "smFlowPanel";
            this.smFlowPanel.Size = new System.Drawing.Size(996, 570);
            this.smFlowPanel.TabIndex = 8;
            // 
            // SMFlowChartCtlBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.smFlowPanel);
            this.Controls.Add(this.cbAutoScope);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.tbSubroutineName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblHeader);
            this.Name = "SMFlowChartCtlBasic";
            this.Size = new System.Drawing.Size(1000, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox tbSubroutineName;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.CheckBox cbAutoScope;
        private System.Windows.Forms.Panel smFlowPanel;

    }
}
