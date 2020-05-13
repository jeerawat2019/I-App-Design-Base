namespace AppMachine.Panel
{
    partial class AppCommonParamPanel
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
            this.pgCommonParam = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // pgCommonParam
            // 
            this.pgCommonParam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgCommonParam.Location = new System.Drawing.Point(6, 6);
            this.pgCommonParam.Name = "pgCommonParam";
            this.pgCommonParam.Size = new System.Drawing.Size(1243, 893);
            this.pgCommonParam.TabIndex = 0;
            // 
            // AppCommonParamPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.pgCommonParam);
            this.Name = "AppCommonParamPanel";
            this.Size = new System.Drawing.Size(1256, 906);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgCommonParam;

    }
}
