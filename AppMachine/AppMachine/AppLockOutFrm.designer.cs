namespace AppMachine
{
    partial class AppLockOutFrm
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
            this.label1 = new System.Windows.Forms.Label();
            this.pbAnimation = new System.Windows.Forms.PictureBox();
            this.kbtnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kGroup = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup.Panel)).BeginInit();
            this.kGroup.Panel.SuspendLayout();
            this.kGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(-11, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(915, 108);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lock out safety is on";
            // 
            // pbAnimation
            // 
            this.pbAnimation.BackColor = System.Drawing.Color.Transparent;
            this.pbAnimation.Image = global::AppMachine.Properties.Resources.Waiting;
            this.pbAnimation.Location = new System.Drawing.Point(380, -4);
            this.pbAnimation.Name = "pbAnimation";
            this.pbAnimation.Size = new System.Drawing.Size(145, 129);
            this.pbAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAnimation.TabIndex = 28;
            this.pbAnimation.TabStop = false;
            // 
            // kbtnClose
            // 
            this.kbtnClose.Location = new System.Drawing.Point(801, 3);
            this.kbtnClose.Name = "kbtnClose";
            this.kbtnClose.OverrideDefault.Back.Color1 = System.Drawing.Color.Black;
            this.kbtnClose.OverrideDefault.Back.Color2 = System.Drawing.Color.Black;
            this.kbtnClose.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kbtnClose.OverrideDefault.Border.Rounding = 15;
            this.kbtnClose.OverrideDefault.Border.Width = 2;
            this.kbtnClose.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kbtnClose.OverrideDefault.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kbtnClose.Size = new System.Drawing.Size(87, 34);
            this.kbtnClose.StateCommon.Back.Color1 = System.Drawing.Color.Black;
            this.kbtnClose.StateCommon.Back.Color2 = System.Drawing.Color.Black;
            this.kbtnClose.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kbtnClose.StateCommon.Border.Rounding = 15;
            this.kbtnClose.StateCommon.Border.Width = 2;
            this.kbtnClose.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kbtnClose.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kbtnClose.TabIndex = 29;
            this.kbtnClose.Values.Text = "UNLOCK";
            this.kbtnClose.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // kGroup
            // 
            this.kGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kGroup.Location = new System.Drawing.Point(0, 0);
            this.kGroup.Name = "kGroup";
            // 
            // kGroup.Panel
            // 
            this.kGroup.Panel.Controls.Add(this.label1);
            this.kGroup.Panel.Controls.Add(this.kbtnClose);
            this.kGroup.Panel.Controls.Add(this.pbAnimation);
            this.kGroup.Size = new System.Drawing.Size(1021, 468);
            this.kGroup.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.kGroup.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.kGroup.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kGroup.StateCommon.Border.Rounding = 200;
            this.kGroup.TabIndex = 30;
            // 
            // AppLockOutFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1021, 468);
            this.ControlBox = false;
            this.Controls.Add(this.kGroup);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppLockOutFrm";
            this.Opacity = 0.5D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lock Out Safety";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup.Panel)).EndInit();
            this.kGroup.Panel.ResumeLayout(false);
            this.kGroup.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup)).EndInit();
            this.kGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbAnimation;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kGroup;
    }
}