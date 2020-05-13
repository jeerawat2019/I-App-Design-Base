namespace AppMachine.Panel.SubPanel
{
    partial class AppDemoVisonSetupPanel
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
            this.pgVisionX = new System.Windows.Forms.PropertyGrid();
            this.flpVisionXTeach = new System.Windows.Forms.FlowLayoutPanel();
            this.panelVisionXAxis = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.camPanel = new MCore.Comp.VisionSystem.CamPanel();
            this.panelVisionJob = new System.Windows.Forms.Panel();
            this.panelCameraProperty = new System.Windows.Forms.Panel();
            this.cbLive = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpVisionJob1 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tpVisionJob1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgVisionX
            // 
            this.pgVisionX.HelpVisible = false;
            this.pgVisionX.Location = new System.Drawing.Point(829, 728);
            this.pgVisionX.Name = "pgVisionX";
            this.pgVisionX.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgVisionX.Size = new System.Drawing.Size(286, 136);
            this.pgVisionX.TabIndex = 10;
            this.pgVisionX.ToolbarVisible = false;
            // 
            // flpVisionXTeach
            // 
            this.flpVisionXTeach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpVisionXTeach.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpVisionXTeach.Location = new System.Drawing.Point(374, 728);
            this.flpVisionXTeach.Name = "flpVisionXTeach";
            this.flpVisionXTeach.Size = new System.Drawing.Size(449, 136);
            this.flpVisionXTeach.TabIndex = 12;
            // 
            // panelVisionXAxis
            // 
            this.panelVisionXAxis.Location = new System.Drawing.Point(3, 728);
            this.panelVisionXAxis.Name = "panelVisionXAxis";
            this.panelVisionXAxis.Size = new System.Drawing.Size(355, 136);
            this.panelVisionXAxis.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(382, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Vision X Teach";
            // 
            // camPanel
            // 
            this.camPanel.BackColor = System.Drawing.Color.Navy;
            this.camPanel.Location = new System.Drawing.Point(711, 254);
            this.camPanel.Name = "camPanel";
            this.camPanel.Size = new System.Drawing.Size(404, 366);
            this.camPanel.TabIndex = 14;
            // 
            // panelVisionJob
            // 
            this.panelVisionJob.Location = new System.Drawing.Point(2, 3);
            this.panelVisionJob.Name = "panelVisionJob";
            this.panelVisionJob.Size = new System.Drawing.Size(677, 309);
            this.panelVisionJob.TabIndex = 19;
            // 
            // panelCameraProperty
            // 
            this.panelCameraProperty.Location = new System.Drawing.Point(841, 3);
            this.panelCameraProperty.Name = "panelCameraProperty";
            this.panelCameraProperty.Size = new System.Drawing.Size(274, 210);
            this.panelCameraProperty.TabIndex = 18;
            // 
            // cbLive
            // 
            this.cbLive.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cbLive.Location = new System.Drawing.Point(711, 626);
            this.cbLive.Name = "cbLive";
            this.cbLive.Size = new System.Drawing.Size(401, 49);
            this.cbLive.TabIndex = 20;
            this.cbLive.Text = "LIVE";
            this.cbLive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbLive.UseVisualStyleBackColor = true;
            this.cbLive.Click += new System.EventHandler(this.OnLiveClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpVisionJob1);
            this.tabControl1.Location = new System.Drawing.Point(3, 254);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(691, 421);
            this.tabControl1.TabIndex = 21;
            // 
            // tpVisionJob1
            // 
            this.tpVisionJob1.Controls.Add(this.panelVisionJob);
            this.tpVisionJob1.Location = new System.Drawing.Point(4, 22);
            this.tpVisionJob1.Name = "tpVisionJob1";
            this.tpVisionJob1.Padding = new System.Windows.Forms.Padding(3);
            this.tpVisionJob1.Size = new System.Drawing.Size(683, 395);
            this.tpVisionJob1.TabIndex = 0;
            this.tpVisionJob1.Text = "Vision Job1";
            this.tpVisionJob1.UseVisualStyleBackColor = true;
            // 
            // AppDemoVisonSetupPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelCameraProperty);
            this.Controls.Add(this.cbLive);
            this.Controls.Add(this.panelVisionXAxis);
            this.Controls.Add(this.flpVisionXTeach);
            this.Controls.Add(this.pgVisionX);
            this.Controls.Add(this.camPanel);
            this.Controls.Add(this.label1);
            this.Name = "AppDemoVisonSetupPanel";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.camPanel, 0);
            this.Controls.SetChildIndex(this.pgVisionX, 0);
            this.Controls.SetChildIndex(this.flpVisionXTeach, 0);
            this.Controls.SetChildIndex(this.panelVisionXAxis, 0);
            this.Controls.SetChildIndex(this.cbLive, 0);
            this.Controls.SetChildIndex(this.panelCameraProperty, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.tpVisionJob1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgVisionX;
        private System.Windows.Forms.FlowLayoutPanel flpVisionXTeach;
        private System.Windows.Forms.Panel panelVisionXAxis;
        private System.Windows.Forms.Label label1;
        private MCore.Comp.VisionSystem.CamPanel camPanel;
        private System.Windows.Forms.Panel panelCameraProperty;
        private System.Windows.Forms.Panel panelVisionJob;
        private System.Windows.Forms.CheckBox cbLive;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpVisionJob1;
    }
}
