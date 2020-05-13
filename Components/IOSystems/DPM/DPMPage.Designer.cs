namespace MCore.Comp.IOSystem
{
    partial class DPMPage
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
            this.triggerMode = new MCore.Comp.IOSystem.CompMeasureCtl();
            this.mdCurrentValue = new MCore.Controls.MDoubleWithUnits();
            this.SuspendLayout();
            // 
            // triggerMode
            // 
            this.triggerMode.Label = "Trigger mode";
            this.triggerMode.Location = new System.Drawing.Point(13, 69);
            this.triggerMode.Name = "triggerMode";
            this.triggerMode.Size = new System.Drawing.Size(300, 154);
            this.triggerMode.TabIndex = 15;
            // 
            // mdCurrentValue
            // 
            this.mdCurrentValue.Label = "Current Value";
            this.mdCurrentValue.Location = new System.Drawing.Point(33, 20);
            this.mdCurrentValue.LogChanges = true;
            this.mdCurrentValue.Name = "mdCurrentValue";
            this.mdCurrentValue.Size = new System.Drawing.Size(227, 22);
            this.mdCurrentValue.TabIndex = 16;
            // 
            // DPMPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mdCurrentValue);
            this.Controls.Add(this.triggerMode);
            this.Name = "DPMPage";
            this.Size = new System.Drawing.Size(424, 356);
            this.ResumeLayout(false);

        }

        #endregion

        private MCore.Comp.IOSystem.CompMeasureCtl triggerMode;
        private MCore.Controls.MDoubleWithUnits mdCurrentValue;
    }
}
