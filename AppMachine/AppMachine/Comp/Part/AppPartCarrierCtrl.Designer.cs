namespace AppMachine.Comp.Part
{
    partial class AppPartCarrierCtrl
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
            this.flpParts = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpParts
            // 
            this.flpParts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpParts.Location = new System.Drawing.Point(3, 3);
            this.flpParts.Name = "flpParts";
            this.flpParts.Size = new System.Drawing.Size(820, 174);
            this.flpParts.TabIndex = 0;
            // 
            // AppPartCarrierCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flpParts);
            this.Name = "AppPartCarrierCtrl";
            this.Size = new System.Drawing.Size(827, 181);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpParts;
    }
}
