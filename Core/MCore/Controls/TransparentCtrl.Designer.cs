namespace MCore.Controls
{
    partial class TransparentCtrl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblLockOutText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblLockOutText
            // 
            this.lblLockOutText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLockOutText.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.lblLockOutText.Location = new System.Drawing.Point(12, 9);
            this.lblLockOutText.Name = "lblLockOutText";
            this.lblLockOutText.Size = new System.Drawing.Size(461, 272);
            this.lblLockOutText.TabIndex = 1;
            this.lblLockOutText.Text = "Lock out safety is on";
            this.lblLockOutText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TransparentCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(485, 290);
            this.Controls.Add(this.lblLockOutText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TransparentCtrl";
            this.Opacity = 0.25D;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLockOutText;
    }
}