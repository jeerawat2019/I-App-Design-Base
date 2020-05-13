namespace AppMachine.Control
{
    partial class AppUserControlBase
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
            this.kUserDialog = new ComponentFactory.Krypton.Toolkit.KryptonTaskDialog();
            this.SuspendLayout();
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
            // AppUserControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AppUserControlBase";
            this.ParentChanged += new System.EventHandler(this.UserControlBase_ParentChanged);
            this.ResumeLayout(false);

        }

        #endregion

        protected ComponentFactory.Krypton.Toolkit.KryptonTaskDialog kUserDialog;

    }
}
