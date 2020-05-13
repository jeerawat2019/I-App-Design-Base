namespace MCore.Controls
{
    partial class ClassPropertyForm
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
            this.genericClassPropCtl = new MCore.Controls.GenericClassPropCtl();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // genericClassPropCtl
            // 
            this.genericClassPropCtl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.genericClassPropCtl.Location = new System.Drawing.Point(12, 12);
            this.genericClassPropCtl.Name = "genericClassPropCtl";
            this.genericClassPropCtl.Size = new System.Drawing.Size(472, 445);
            this.genericClassPropCtl.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(392, 463);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 38);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ClassPropertyForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(496, 513);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.genericClassPropCtl);
            this.Name = "ClassPropertyForm";
            this.Text = "Class Property Form";
            this.ResumeLayout(false);

        }

        #endregion

        private GenericClassPropCtl genericClassPropCtl;
        private System.Windows.Forms.Button btnClose;
    }
}