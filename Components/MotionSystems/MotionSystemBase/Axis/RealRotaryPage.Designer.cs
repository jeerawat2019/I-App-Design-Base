namespace MCore.Comp.MotionSystem.Axis
{
    partial class RealRotaryPage
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
            this.rotaryBasePage = new MCore.Comp.MotionSystem.RotaryBasePage();
            this.cbEnable = new MCore.Controls.MCheckBox();
            this.tbMaxLimit = new MCore.Controls.MDoubleWithUnits();
            this.tbMinLimit = new MCore.Controls.MDoubleWithUnits();
            this.tbDeafultSpeed = new MCore.Controls.MDoubleWithUnits();
            this.tbCurrentPos = new MCore.Controls.MDoubleWithUnits();
            this.btnHome = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // axisBasePage
            // 
            this.rotaryBasePage.Location = new System.Drawing.Point(3, 119);
            this.rotaryBasePage.Name = "axisBasePage";
            this.rotaryBasePage.Reverse = false;
            this.rotaryBasePage.Size = new System.Drawing.Size(363, 49);
            this.rotaryBasePage.TabIndex = 6;
            // 
            // cbEnable
            // 
            this.cbEnable.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbEnable.Location = new System.Drawing.Point(275, 7);
            this.cbEnable.Name = "cbEnable";
            this.cbEnable.Size = new System.Drawing.Size(91, 23);
            this.cbEnable.TabIndex = 4;
            this.cbEnable.Text = "Enable";
            this.cbEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbEnable.UseVisualStyleBackColor = true;
            // 
            // tbMaxLimit
            // 
            this.tbMaxLimit.Label = "";
            this.tbMaxLimit.Location = new System.Drawing.Point(12, 64);
            this.tbMaxLimit.Name = "tbMaxLimit";
            this.tbMaxLimit.Size = new System.Drawing.Size(235, 22);
            this.tbMaxLimit.TabIndex = 3;
            // 
            // tbMinLimit
            // 
            this.tbMinLimit.Label = "";
            this.tbMinLimit.Location = new System.Drawing.Point(12, 36);
            this.tbMinLimit.Name = "tbMinLimit";
            this.tbMinLimit.Size = new System.Drawing.Size(235, 22);
            this.tbMinLimit.TabIndex = 2;
            // 
            // tbDeafultSpeed
            // 
            this.tbDeafultSpeed.Label = "";
            this.tbDeafultSpeed.Location = new System.Drawing.Point(12, 8);
            this.tbDeafultSpeed.Name = "tbDeafultSpeed";
            this.tbDeafultSpeed.Size = new System.Drawing.Size(235, 22);
            this.tbDeafultSpeed.TabIndex = 1;
            // 
            // tbCurrentPos
            // 
            this.tbCurrentPos.Label = "";
            this.tbCurrentPos.Location = new System.Drawing.Point(12, 91);
            this.tbCurrentPos.Name = "tbCurrentPos";
            this.tbCurrentPos.Size = new System.Drawing.Size(235, 22);
            this.tbCurrentPos.TabIndex = 0;
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(275, 36);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(91, 22);
            this.btnHome.TabIndex = 7;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // RealAxisPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.rotaryBasePage);
            this.Controls.Add(this.cbEnable);
            this.Controls.Add(this.tbMaxLimit);
            this.Controls.Add(this.tbMinLimit);
            this.Controls.Add(this.tbDeafultSpeed);
            this.Controls.Add(this.tbCurrentPos);
            this.Name = "RealAxisPage";
            this.Size = new System.Drawing.Size(381, 179);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MDoubleWithUnits tbCurrentPos;
        private Controls.MDoubleWithUnits tbDeafultSpeed;
        private Controls.MDoubleWithUnits tbMinLimit;
        private Controls.MDoubleWithUnits tbMaxLimit;
        private Controls.MCheckBox cbEnable;
        private RotaryBasePage rotaryBasePage;
        private System.Windows.Forms.Button btnHome;
    }
}
