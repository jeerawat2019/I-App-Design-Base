using MCore.Comp.IOSystem;

namespace MCore.Comp.IOSystem
{
    partial class Keyence_LJ_V7001_Page
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
            this.txtOutputResult = new MCore.Controls.StringCtl();
            this.mdDisplacement = new MCore.Controls.MDoubleWithUnits();
            this.SuspendLayout();
            // 
            // txtOutputResult
            // 
            this.txtOutputResult.Location = new System.Drawing.Point(31, 14);
            this.txtOutputResult.LogChanges = true;
            this.txtOutputResult.Name = "txtOutputResult";
            this.txtOutputResult.Size = new System.Drawing.Size(308, 20);
            this.txtOutputResult.TabIndex = 12;
            // 
            // mdDisplacement
            // 
            this.mdDisplacement.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mdDisplacement.Label = "";
            this.mdDisplacement.Location = new System.Drawing.Point(87, 69);
            this.mdDisplacement.LogChanges = true;
            this.mdDisplacement.Name = "mdDisplacement";
            this.mdDisplacement.Padding = new System.Windows.Forms.Padding(2);
            this.mdDisplacement.Size = new System.Drawing.Size(190, 22);
            this.mdDisplacement.TabIndex = 13;
            // 
            // Keyence_LJ_V7001_Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mdDisplacement);
            this.Controls.Add(this.txtOutputResult);
            this.Name = "Keyence_LJ_V7001_Page";
            this.Size = new System.Drawing.Size(369, 365);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private CompMeasureCtl triggerMode;
        private Controls.StringCtl txtOutputResult;
        private Controls.MDoubleWithUnits mdDisplacement;



    }
}
