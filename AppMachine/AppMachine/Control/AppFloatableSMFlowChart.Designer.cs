namespace AppMachine.Control
{
    partial class AppFloatableSMFlowChart
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
            this.btnFloatDock = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFloatDock
            // 
            this.btnFloatDock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFloatDock.Location = new System.Drawing.Point(870, 27);
            this.btnFloatDock.Name = "btnFloatDock";
            this.btnFloatDock.Size = new System.Drawing.Size(108, 23);
            this.btnFloatDock.TabIndex = 8;
            this.btnFloatDock.Text = "Float";
            this.btnFloatDock.UseVisualStyleBackColor = true;
            this.btnFloatDock.Click += new System.EventHandler(this.btnFloatDock_Click);
            // 
            // FloatableSMFlowChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnFloatDock);
            this.Name = "FloatableSMFlowChart";
            this.Controls.SetChildIndex(this.btnFloatDock, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFloatDock;
    }
}
