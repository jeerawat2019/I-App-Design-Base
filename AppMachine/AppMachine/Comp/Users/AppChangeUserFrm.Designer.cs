namespace AppMachine.Comp.Users
{
    partial class AppChangeUserFrm
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
            this.kGroup = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.pbAnimation = new System.Windows.Forms.PictureBox();
            this.kbtnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.strUserInputCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.labelCtl1 = new MCore.Controls.LabelCtl();
            this.kUserDialog = new ComponentFactory.Krypton.Toolkit.KryptonTaskDialog();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup.Panel)).BeginInit();
            this.kGroup.Panel.SuspendLayout();
            this.kGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimation)).BeginInit();
            this.SuspendLayout();
            // 
            // kGroup
            // 
            this.kGroup.Location = new System.Drawing.Point(2, 1);
            this.kGroup.Name = "kGroup";
            // 
            // kGroup.Panel
            // 
            this.kGroup.Panel.Controls.Add(this.pbAnimation);
            this.kGroup.Panel.Controls.Add(this.kbtnClose);
            this.kGroup.Panel.Controls.Add(this.strUserInputCode);
            this.kGroup.Panel.Controls.Add(this.labelCtl1);
            this.kGroup.Size = new System.Drawing.Size(741, 187);
            this.kGroup.StateCommon.Back.Color1 = System.Drawing.Color.Navy;
            this.kGroup.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.kGroup.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kGroup.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.kGroup.StateCommon.Border.Rounding = 70;
            this.kGroup.StateCommon.Border.Width = 9;
            this.kGroup.TabIndex = 22;
            // 
            // pbAnimation
            // 
            this.pbAnimation.BackColor = System.Drawing.Color.Navy;
            this.pbAnimation.Image = global::AppMachine.Properties.Resources.WaitingV2;
            this.pbAnimation.Location = new System.Drawing.Point(0, 3);
            this.pbAnimation.Name = "pbAnimation";
            this.pbAnimation.Size = new System.Drawing.Size(121, 121);
            this.pbAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAnimation.TabIndex = 27;
            this.pbAnimation.TabStop = false;
            // 
            // kbtnClose
            // 
            this.kbtnClose.Location = new System.Drawing.Point(642, 3);
            this.kbtnClose.Name = "kbtnClose";
            this.kbtnClose.Size = new System.Drawing.Size(34, 34);
            this.kbtnClose.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kbtnClose.StateCommon.Border.Rounding = 15;
            this.kbtnClose.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kbtnClose.TabIndex = 26;
            this.kbtnClose.Values.Text = "X";
            this.kbtnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // strUserInputCode
            // 
            this.strUserInputCode.Location = new System.Drawing.Point(121, 31);
            this.strUserInputCode.Name = "strUserInputCode";
            this.strUserInputCode.PasswordChar = '●';
            this.strUserInputCode.Size = new System.Drawing.Size(442, 67);
            this.strUserInputCode.StateCommon.Back.Color1 = System.Drawing.Color.Black;
            this.strUserInputCode.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.strUserInputCode.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.strUserInputCode.StateCommon.Border.Rounding = 7;
            this.strUserInputCode.StateCommon.Border.Width = 3;
            this.strUserInputCode.StateCommon.Content.Color1 = System.Drawing.Color.Lime;
            this.strUserInputCode.StateCommon.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.strUserInputCode.TabIndex = 25;
            this.strUserInputCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.strUserInputCode.UseSystemPasswordChar = true;
            this.strUserInputCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.strUserInputCode_KeyUp);
            // 
            // labelCtl1
            // 
            this.labelCtl1.AutoSize = true;
            this.labelCtl1.BackColor = System.Drawing.Color.Navy;
            this.labelCtl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelCtl1.ForeColor = System.Drawing.Color.White;
            this.labelCtl1.Location = new System.Drawing.Point(259, 3);
            this.labelCtl1.Name = "labelCtl1";
            this.labelCtl1.Size = new System.Drawing.Size(182, 25);
            this.labelCtl1.TabIndex = 22;
            this.labelCtl1.Text = "User Code Input";
            // 
            // kUserDialog
            // 
            this.kUserDialog.CheckboxText = null;
            this.kUserDialog.Content = null;
            this.kUserDialog.DefaultButton = ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK;
            this.kUserDialog.DefaultRadioButton = null;
            this.kUserDialog.FooterHyperlink = null;
            this.kUserDialog.FooterText = null;
            this.kUserDialog.MainInstruction = null;
            this.kUserDialog.WindowTitle = null;
            // 
            // AppChangeUserFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(745, 191);
            this.Controls.Add(this.kGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppChangeUserFrm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Changer";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppChangeUserFrm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.kGroup.Panel)).EndInit();
            this.kGroup.Panel.ResumeLayout(false);
            this.kGroup.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup)).EndInit();
            this.kGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonGroup kGroup;
        private MCore.Controls.LabelCtl labelCtl1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox strUserInputCode;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonTaskDialog kUserDialog;
        private System.Windows.Forms.PictureBox pbAnimation;
    }
}