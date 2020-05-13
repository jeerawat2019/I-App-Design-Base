namespace MCore.Comp.VisionSystem
{
    partial class VisionJobRunPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAcquisition = new System.Windows.Forms.Button();
            this.btnRunJob = new System.Windows.Forms.Button();
            this.btnEditJob = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cbSuccess = new System.Windows.Forms.CheckBox();
            this.dgCogJobResult = new System.Windows.Forms.DataGridView();
            this.dataName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.min = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.max = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.actual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.visionName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lblResultsError = new System.Windows.Forms.Label();
            this.mdStrobeDuration = new MCore.Controls.MDoubleWithUnits();
            this.flpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCogJobResult)).BeginInit();
            this.SuspendLayout();
            // 
            // flpButtons
            // 
            this.flpButtons.Controls.Add(this.btnAcquisition);
            this.flpButtons.Controls.Add(this.btnRunJob);
            this.flpButtons.Controls.Add(this.btnEditJob);
            this.flpButtons.Controls.Add(this.btnRefresh);
            this.flpButtons.Controls.Add(this.cbSuccess);
           
            this.flpButtons.Location = new System.Drawing.Point(4, 3);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(5);
            this.flpButtons.Size = new System.Drawing.Size(369, 30);
            this.flpButtons.TabIndex = 1;
            // 
            // btnAcquisition
            // 
            this.btnAcquisition.Location = new System.Drawing.Point(5, 5);
            this.btnAcquisition.Margin = new System.Windows.Forms.Padding(0);
            this.btnAcquisition.Name = "btnAcquisition";
            this.btnAcquisition.Size = new System.Drawing.Size(75, 23);
            this.btnAcquisition.TabIndex = 5;
            this.btnAcquisition.Text = "Acquisition";
            this.btnAcquisition.UseVisualStyleBackColor = true;
            this.btnAcquisition.Click += new System.EventHandler(this.btnAcquisition_Click);
            // 
            // btnRunJob
            // 
            this.btnRunJob.Location = new System.Drawing.Point(80, 5);
            this.btnRunJob.Margin = new System.Windows.Forms.Padding(0);
            this.btnRunJob.Name = "btnRunJob";
            this.btnRunJob.Size = new System.Drawing.Size(69, 23);
            this.btnRunJob.TabIndex = 1;
            this.btnRunJob.Text = "Run Job";
            this.btnRunJob.UseVisualStyleBackColor = true;
            this.btnRunJob.Click += new System.EventHandler(this.btnRunJob_Click);
            // 
            // btnEditJob
            // 
            this.btnEditJob.Location = new System.Drawing.Point(149, 5);
            this.btnEditJob.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditJob.Name = "btnEditJob";
            this.btnEditJob.Size = new System.Drawing.Size(75, 23);
            this.btnEditJob.TabIndex = 2;
            this.btnEditJob.Text = "Edit Job";
            this.btnEditJob.UseVisualStyleBackColor = true;
            this.btnEditJob.Click += new System.EventHandler(this.btnEditJob_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(224, 5);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.OnRefreshClick);
            // 
            // cbSuccess
            // 
            this.cbSuccess.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbSuccess.BackColor = System.Drawing.Color.YellowGreen;
            this.cbSuccess.Enabled = false;
            this.cbSuccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSuccess.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbSuccess.Location = new System.Drawing.Point(299, 5);
            this.cbSuccess.Margin = new System.Windows.Forms.Padding(0);
            this.cbSuccess.Name = "cbSuccess";
            this.cbSuccess.Size = new System.Drawing.Size(65, 23);
            this.cbSuccess.TabIndex = 4;
            this.cbSuccess.Text = "Success";
            this.cbSuccess.UseVisualStyleBackColor = false;
            // 
            // dgCogJobResult
            // 
            this.dgCogJobResult.AllowUserToAddRows = false;
            this.dgCogJobResult.AllowUserToDeleteRows = false;
            this.dgCogJobResult.AllowUserToResizeColumns = false;
            this.dgCogJobResult.AllowUserToResizeRows = false;
            this.dgCogJobResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCogJobResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgCogJobResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgCogJobResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataName,
            this.Value,
            this.min,
            this.max,
            this.mode,
            this.actual,
            this.visionName});
            this.dgCogJobResult.Location = new System.Drawing.Point(4, 39);
            this.dgCogJobResult.Name = "dgCogJobResult";
            this.dgCogJobResult.RowHeadersVisible = false;
            this.dgCogJobResult.RowTemplate.Height = 24;
            this.dgCogJobResult.Size = new System.Drawing.Size(670, 267);
            this.dgCogJobResult.TabIndex = 2;
            this.dgCogJobResult.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.OnCellFormating);
            this.dgCogJobResult.CurrentCellDirtyStateChanged += new System.EventHandler(this.OnCurrentCellDirty);
            this.dgCogJobResult.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.OnDataError);
            // 
            // dataName
            // 
            this.dataName.HeaderText = "Data Name";
            this.dataName.Name = "dataName";
            this.dataName.ReadOnly = true;
            this.dataName.Width = 150;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // min
            // 
            this.min.HeaderText = "Min";
            this.min.Name = "min";
            // 
            // max
            // 
            this.max.HeaderText = "Max";
            this.max.Name = "max";
            // 
            // mode
            // 
            this.mode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mode.HeaderText = "Mode";
            this.mode.Name = "mode";
            this.mode.Width = 75;
            // 
            // actual
            // 
            this.actual.HeaderText = "Actual";
            this.actual.Name = "actual";
            this.actual.ReadOnly = true;
            this.actual.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.actual.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.actual.Width = 75;
            // 
            // visionName
            // 
            this.visionName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.visionName.HeaderText = "Assigned vision name";
            this.visionName.Name = "visionName";
            this.visionName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.visionName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.visionName.Width = 290;
            // 
            // lblResultsError
            // 
            this.lblResultsError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultsError.Location = new System.Drawing.Point(544, 3);
            this.lblResultsError.Name = "lblResultsError";
            this.lblResultsError.Size = new System.Drawing.Size(129, 33);
            this.lblResultsError.TabIndex = 3;
            this.lblResultsError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mdStrobeDuration
            // 
            this.mdStrobeDuration.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mdStrobeDuration.DoubleVal = 0D;
            this.mdStrobeDuration.Enabled = false;
            this.mdStrobeDuration.Label = "Strobe Dur";
            this.mdStrobeDuration.Location = new System.Drawing.Point(378, 7);
            this.mdStrobeDuration.LogChanges = true;
            this.mdStrobeDuration.Name = "mdStrobeDuration";
            this.mdStrobeDuration.Padding = new System.Windows.Forms.Padding(2);
            this.mdStrobeDuration.Size = new System.Drawing.Size(160, 22);
            this.mdStrobeDuration.TabIndex = 4;
            this.mdStrobeDuration.TextBackColor = System.Drawing.SystemColors.Window;
            this.mdStrobeDuration.UnitsLabel = "Unit";
           
            // 
            // VisionJobRunPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mdStrobeDuration);
            this.Controls.Add(this.lblResultsError);
            this.Controls.Add(this.dgCogJobResult);
            this.Controls.Add(this.flpButtons);
            this.Name = "VisionJobRunPage";
            this.Size = new System.Drawing.Size(677, 309);
            this.flpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCogJobResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnEditJob;
        private System.Windows.Forms.Button btnRunJob;
        private System.Windows.Forms.DataGridView dgCogJobResult;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox cbSuccess;
        private System.Windows.Forms.Button btnAcquisition;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn min;
        private System.Windows.Forms.DataGridViewTextBoxColumn max;
        private System.Windows.Forms.DataGridViewComboBoxColumn mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn actual;
        private System.Windows.Forms.DataGridViewComboBoxColumn visionName;
        private System.Windows.Forms.Label lblResultsError;
        private Controls.MDoubleWithUnits mdStrobeDuration;

    }
}
