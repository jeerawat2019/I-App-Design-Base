namespace AppMachine.Panel
{
    partial class AppUsersPanel
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
            this.cbDelUser = new System.Windows.Forms.ComboBox();
            this.tbNewUser = new System.Windows.Forms.TextBox();
            this.btnDelUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.componentBrowser = new MCore.Controls.ComponentBrowser();
            this.SuspendLayout();
            // 
            // cbDelUser
            // 
            this.cbDelUser.FormattingEnabled = true;
            this.cbDelUser.Location = new System.Drawing.Point(360, 4);
            this.cbDelUser.Name = "cbDelUser";
            this.cbDelUser.Size = new System.Drawing.Size(244, 21);
            this.cbDelUser.TabIndex = 7;
            this.cbDelUser.DropDown += new System.EventHandler(this.cbDelRecipe_DropDown);
            // 
            // tbNewUser
            // 
            this.tbNewUser.Location = new System.Drawing.Point(3, 6);
            this.tbNewUser.Name = "tbNewUser";
            this.tbNewUser.Size = new System.Drawing.Size(255, 20);
            this.tbNewUser.TabIndex = 6;
            // 
            // btnDelUser
            // 
            this.btnDelUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDelUser.Location = new System.Drawing.Point(610, 3);
            this.btnDelUser.Name = "btnDelUser";
            this.btnDelUser.Size = new System.Drawing.Size(75, 23);
            this.btnDelUser.TabIndex = 5;
            this.btnDelUser.Text = "REMOVE";
            this.btnDelUser.UseVisualStyleBackColor = false;
            this.btnDelUser.Click += new System.EventHandler(this.btnDelUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddUser.Location = new System.Drawing.Point(259, 4);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(75, 23);
            this.btnAddUser.TabIndex = 4;
            this.btnAddUser.Text = "NEW";
            this.btnAddUser.UseVisualStyleBackColor = false;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // componentBrowser
            // 
            this.componentBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.componentBrowser.Location = new System.Drawing.Point(3, 32);
            this.componentBrowser.Name = "componentBrowser";
            this.componentBrowser.Size = new System.Drawing.Size(1243, 867);
            this.componentBrowser.TabIndex = 2;
            // 
            // AppUsersPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.cbDelUser);
            this.Controls.Add(this.tbNewUser);
            this.Controls.Add(this.btnDelUser);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.componentBrowser);
            this.Name = "AppUsersPanel";
            this.Size = new System.Drawing.Size(1256, 906);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MCore.Controls.ComponentBrowser componentBrowser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnDelUser;
        private System.Windows.Forms.TextBox tbNewUser;
        private System.Windows.Forms.ComboBox cbDelUser;


    }
}
