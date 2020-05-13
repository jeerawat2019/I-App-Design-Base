namespace MCore.Comp.IOSystem
{
    partial class CompMeasureCtl
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
            this.gbTriggerMode = new MCore.Controls.EnumRadioGroupBoxCtl();
            this.rbIdle = new System.Windows.Forms.RadioButton();
            this.rbSingleTrigger = new System.Windows.Forms.RadioButton();
            this.mdInterval = new MCore.Controls.MDoubleWithUnits();
            this.rbTimedTrigger = new System.Windows.Forms.RadioButton();
            this.rbLive = new System.Windows.Forms.RadioButton();
            this.gbTriggerMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTriggerMode
            // 
            this.gbTriggerMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTriggerMode.Controls.Add(this.rbIdle);
            this.gbTriggerMode.Controls.Add(this.rbSingleTrigger);
            this.gbTriggerMode.Controls.Add(this.mdInterval);
            this.gbTriggerMode.Controls.Add(this.rbTimedTrigger);
            this.gbTriggerMode.Controls.Add(this.rbLive);
            this.gbTriggerMode.Location = new System.Drawing.Point(0, 0);
            this.gbTriggerMode.LogChanges = true;
            this.gbTriggerMode.Name = "gbTriggerMode";
            this.gbTriggerMode.Size = new System.Drawing.Size(182, 130);
            this.gbTriggerMode.TabIndex = 6;
            this.gbTriggerMode.TabStop = false;
            this.gbTriggerMode.Text = "Triggering";
            // 
            // rbIdle
            // 
            this.rbIdle.AutoSize = true;
            this.rbIdle.Location = new System.Drawing.Point(17, 29);
            this.rbIdle.Name = "rbIdle";
            this.rbIdle.Size = new System.Drawing.Size(42, 17);
            this.rbIdle.TabIndex = 0;
            this.rbIdle.TabStop = true;
            this.rbIdle.Text = "Idle";
            this.rbIdle.UseVisualStyleBackColor = true;
            // 
            // rbSingleTrigger
            // 
            this.rbSingleTrigger.AutoSize = true;
            this.rbSingleTrigger.Location = new System.Drawing.Point(17, 52);
            this.rbSingleTrigger.Name = "rbSingleTrigger";
            this.rbSingleTrigger.Size = new System.Drawing.Size(54, 17);
            this.rbSingleTrigger.TabIndex = 1;
            this.rbSingleTrigger.TabStop = true;
            this.rbSingleTrigger.Text = "Single";
            this.rbSingleTrigger.UseVisualStyleBackColor = true;
            // 
            // mdInterval
            // 
            this.mdInterval.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mdInterval.Enabled = false;
            this.mdInterval.Label = "";
            this.mdInterval.Location = new System.Drawing.Point(77, 73);
            this.mdInterval.LogChanges = true;
            this.mdInterval.Name = "mdInterval";
            this.mdInterval.Padding = new System.Windows.Forms.Padding(2);
            this.mdInterval.Size = new System.Drawing.Size(94, 22);
            this.mdInterval.TabIndex = 5;
            // 
            // rbTimedTrigger
            // 
            this.rbTimedTrigger.AutoSize = true;
            this.rbTimedTrigger.Location = new System.Drawing.Point(17, 76);
            this.rbTimedTrigger.Name = "rbTimedTrigger";
            this.rbTimedTrigger.Size = new System.Drawing.Size(54, 17);
            this.rbTimedTrigger.TabIndex = 2;
            this.rbTimedTrigger.TabStop = true;
            this.rbTimedTrigger.Text = "Timed";
            this.rbTimedTrigger.UseVisualStyleBackColor = true;
            // 
            // rbLive
            // 
            this.rbLive.AutoSize = true;
            this.rbLive.Location = new System.Drawing.Point(17, 100);
            this.rbLive.Name = "rbLive";
            this.rbLive.Size = new System.Drawing.Size(45, 17);
            this.rbLive.TabIndex = 3;
            this.rbLive.TabStop = true;
            this.rbLive.Text = "Live";
            this.rbLive.UseVisualStyleBackColor = true;
            // 
            // CompMeasureCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTriggerMode);
            this.Name = "CompMeasureCtl";
            this.Size = new System.Drawing.Size(182, 133);
            this.gbTriggerMode.ResumeLayout(false);
            this.gbTriggerMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbIdle;
        private System.Windows.Forms.RadioButton rbSingleTrigger;
        private System.Windows.Forms.RadioButton rbTimedTrigger;
        private System.Windows.Forms.RadioButton rbLive;
        private MCore.Controls.MDoubleWithUnits mdInterval;
        private MCore.Controls.EnumRadioGroupBoxCtl gbTriggerMode;
    }
}
