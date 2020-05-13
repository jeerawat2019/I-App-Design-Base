namespace MCore.Comp.MotionSystem
{
    partial class RotaryBasePage
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
            if (disposing)
            {
                UnBind();
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.tbPosition0 = new MCore.Controls.MDoubleWithUnits();
            this.gbPosition = new System.Windows.Forms.Panel();
            this.btnIncrease = new System.Windows.Forms.Button();
            this.dragMover = new System.Windows.Forms.Splitter();
            this.btnDecrease = new System.Windows.Forms.Button();
            this.gbPosition.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPosition0
            // 
            this.tbPosition0.Label = "";
            this.tbPosition0.Location = new System.Drawing.Point(17, 16);
            this.tbPosition0.LogChanges = true;
            this.tbPosition0.Name = "tbPosition0";
            this.tbPosition0.Size = new System.Drawing.Size(194, 22);
            this.tbPosition0.TabIndex = 1;
            // 
            // gbPosition
            // 
            this.gbPosition.Controls.Add(this.btnIncrease);
            this.gbPosition.Controls.Add(this.dragMover);
            this.gbPosition.Controls.Add(this.btnDecrease);
            this.gbPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPosition.Location = new System.Drawing.Point(0, 0);
            this.gbPosition.Name = "gbPosition";
            this.gbPosition.Size = new System.Drawing.Size(363, 28);
            this.gbPosition.TabIndex = 8;
            // 
            // btnIncrease
            // 
            this.btnIncrease.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIncrease.Location = new System.Drawing.Point(73, 0);
            this.btnIncrease.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnIncrease.MinimumSize = new System.Drawing.Size(10, 0);
            this.btnIncrease.Name = "btnIncrease";
            this.btnIncrease.Size = new System.Drawing.Size(290, 28);
            this.btnIncrease.TabIndex = 2;
            this.btnIncrease.UseVisualStyleBackColor = true;
            this.btnIncrease.Click += new System.EventHandler(this.OnIncreaseClick);
            // 
            // dragMover
            // 
            this.dragMover.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dragMover.Location = new System.Drawing.Point(63, 0);
            this.dragMover.MinSize = 20;
            this.dragMover.Name = "dragMover";
            this.dragMover.Size = new System.Drawing.Size(10, 28);
            this.dragMover.TabIndex = 1;
            this.dragMover.TabStop = false;
            this.dragMover.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnDragMove);
            // 
            // btnDecrease
            // 
            this.btnDecrease.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDecrease.Location = new System.Drawing.Point(0, 0);
            this.btnDecrease.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnDecrease.MinimumSize = new System.Drawing.Size(10, 0);
            this.btnDecrease.Name = "btnDecrease";
            this.btnDecrease.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnDecrease.Size = new System.Drawing.Size(63, 28);
            this.btnDecrease.TabIndex = 0;
            this.btnDecrease.Tag = "   ";
            this.btnDecrease.UseVisualStyleBackColor = true;
            this.btnDecrease.Click += new System.EventHandler(this.OnDecreaseClick);
            // 
            // AxisBasePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbPosition);
            this.Name = "AxisBasePage";
            this.Size = new System.Drawing.Size(363, 28);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.gbPosition.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MDoubleWithUnits tbPosition0;
        private System.Windows.Forms.Panel gbPosition;
        private System.Windows.Forms.Splitter dragMover;
        private System.Windows.Forms.Button btnDecrease;
        private System.Windows.Forms.Button btnIncrease;
    }
}
