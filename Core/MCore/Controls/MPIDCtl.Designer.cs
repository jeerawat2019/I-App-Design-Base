namespace MCore.Controls
{
    partial class MPIDCtl
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
            this.lblRoot = new System.Windows.Forms.Label();
            this.cbChildrenList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblRoot
            // 
            this.lblRoot.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblRoot.Location = new System.Drawing.Point(0, 4);
            this.lblRoot.Margin = new System.Windows.Forms.Padding(0);
            this.lblRoot.Name = "lblRoot";
            this.lblRoot.Size = new System.Drawing.Size(36, 13);
            this.lblRoot.TabIndex = 0;
            this.lblRoot.Text = ">Root";
            this.lblRoot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRoot.Visible = false;
            // 
            // cbChildrenList
            // 
            this.cbChildrenList.FormattingEnabled = true;
            this.cbChildrenList.Location = new System.Drawing.Point(97, 0);
            this.cbChildrenList.Name = "cbChildrenList";
            this.cbChildrenList.Size = new System.Drawing.Size(208, 21);
            this.cbChildrenList.TabIndex = 1;
            this.cbChildrenList.Visible = false;
            this.cbChildrenList.DropDownClosed += new System.EventHandler(this.OnDropDownClosed);
            this.cbChildrenList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.cbChildrenList.Leave += new System.EventHandler(this.OnComboBoxLeave);
            // 
            // MPIDCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbChildrenList);
            this.Controls.Add(this.lblRoot);
            this.Name = "MPIDCtl";
            this.Size = new System.Drawing.Size(467, 21);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblRoot;
        private System.Windows.Forms.ComboBox cbChildrenList;
    }
}
