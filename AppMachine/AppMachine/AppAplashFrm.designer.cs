namespace AppMachine
{
    partial class AppSplashFrm
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
            this.lblInitial = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroup.Panel)).BeginInit();
            this.kGroup.Panel.SuspendLayout();
            this.kGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimation)).BeginInit();
            this.SuspendLayout();
            // 
            // kGroup
            // 
            this.kGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kGroup.Location = new System.Drawing.Point(0, 0);
            this.kGroup.Name = "kGroup";
            // 
            // kGroup.Panel
            // 
            this.kGroup.Panel.Controls.Add(this.pbAnimation);
            this.kGroup.Panel.Controls.Add(this.lblInitial);
            this.kGroup.Size = new System.Drawing.Size(741, 187);
            this.kGroup.StateCommon.Back.Color1 = System.Drawing.Color.Navy;
            this.kGroup.StateCommon.Border.Color1 = System.Drawing.Color.White;
            this.kGroup.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kGroup.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.kGroup.StateCommon.Border.Rounding = 70;
            this.kGroup.StateCommon.Border.Width = 9;
            this.kGroup.TabIndex = 0;
            // 
            // pbAnimation
            // 
            this.pbAnimation.BackColor = System.Drawing.Color.Navy;
            this.pbAnimation.Image = global::AppMachine.Properties.Resources.LoadingDotV2;
            this.pbAnimation.Location = new System.Drawing.Point(479, 13);
            this.pbAnimation.Name = "pbAnimation";
            this.pbAnimation.Size = new System.Drawing.Size(120, 85);
            this.pbAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAnimation.TabIndex = 1;
            this.pbAnimation.TabStop = false;
            // 
            // lblInitial
            // 
            this.lblInitial.AutoSize = true;
            this.lblInitial.BackColor = System.Drawing.Color.Navy;
            this.lblInitial.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblInitial.ForeColor = System.Drawing.Color.White;
            this.lblInitial.Location = new System.Drawing.Point(181, 26);
            this.lblInitial.Name = "lblInitial";
            this.lblInitial.Size = new System.Drawing.Size(314, 73);
            this.lblInitial.TabIndex = 0;
            this.lblInitial.Text = "Initializing";
            this.lblInitial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AppSplashFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(741, 187);
            this.ControlBox = false;
            this.Controls.Add(this.kGroup);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppSplashFrm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lock Out Safety";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
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
        private System.Windows.Forms.Label lblInitial;
        private System.Windows.Forms.PictureBox pbAnimation;

    }
}