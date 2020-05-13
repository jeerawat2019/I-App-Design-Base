namespace MCore.Controls
{
    partial class MBoolRadio
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
            this.rbFalse = new System.Windows.Forms.RadioButton();
            this.rbTrue = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rbFalse
            // 
            this.rbFalse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rbFalse.Location = new System.Drawing.Point(20, 4);
            this.rbFalse.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.rbFalse.Name = "rbFalse";
            this.rbFalse.Size = new System.Drawing.Size(90, 20);
            this.rbFalse.TabIndex = 0;
            this.rbFalse.TabStop = true;
            this.rbFalse.Text = "radioButton1";
            this.rbFalse.UseVisualStyleBackColor = true;
            this.rbFalse.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // rbTrue
            // 
            this.rbTrue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rbTrue.Location = new System.Drawing.Point(20, 24);
            this.rbTrue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.rbTrue.Name = "rbTrue";
            this.rbTrue.Size = new System.Drawing.Size(90, 20);
            this.rbTrue.TabIndex = 1;
            this.rbTrue.TabStop = true;
            this.rbTrue.Text = "radioButton2";
            this.rbTrue.UseVisualStyleBackColor = true;
            // 
            // MBoolRadio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbTrue);
            this.Controls.Add(this.rbFalse);
            this.Name = "MBoolRadio";
            this.Size = new System.Drawing.Size(128, 52);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbFalse;
        private System.Windows.Forms.RadioButton rbTrue;
    }
}
