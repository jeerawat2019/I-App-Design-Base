using MCore.Comp;

namespace MCore.Controls
{
    partial class CompBasePropCtl
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
                CompBase.OnChangedName -= new CompBase.NameChangedEventHandler(OnChangedCompName);
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
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.cbClassChooser = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(0, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(610, 744);
            this.propertyGrid.TabIndex = 0;
            // 
            // cbClassChooser
            // 
            this.cbClassChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClassChooser.FormattingEnabled = true;
            this.cbClassChooser.Location = new System.Drawing.Point(259, 102);
            this.cbClassChooser.Name = "cbClassChooser";
            this.cbClassChooser.Size = new System.Drawing.Size(288, 21);
            this.cbClassChooser.TabIndex = 1;
            this.cbClassChooser.Visible = false;
            this.cbClassChooser.SelectedIndexChanged += new System.EventHandler(this.OnClassTypeChanged);
            // 
            // CompBasePropCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbClassChooser);
            this.Controls.Add(this.propertyGrid);
            this.Name = "CompBasePropCtl";
            this.Size = new System.Drawing.Size(613, 750);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ComboBox cbClassChooser;
    }
}
