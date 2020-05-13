namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    partial class NewItemForm
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
            this.gbFlowItems = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rbActTrans = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.rbTransition = new System.Windows.Forms.RadioButton();
            this.tbText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbSubroutine = new System.Windows.Forms.RadioButton();
            this.rbStopReturn = new System.Windows.Forms.RadioButton();
            this.rbStop = new System.Windows.Forms.RadioButton();
            this.rbDecision = new System.Windows.Forms.RadioButton();
            this.rbAction = new System.Windows.Forms.RadioButton();
            this.gbFlowItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFlowItems
            // 
            this.gbFlowItems.Controls.Add(this.rbActTrans);
            this.gbFlowItems.Controls.Add(this.rbTransition);
            this.gbFlowItems.Controls.Add(this.rbSubroutine);
            this.gbFlowItems.Controls.Add(this.rbStopReturn);
            this.gbFlowItems.Controls.Add(this.rbStop);
            this.gbFlowItems.Controls.Add(this.rbDecision);
            this.gbFlowItems.Controls.Add(this.rbAction);
            this.gbFlowItems.Location = new System.Drawing.Point(30, 25);
            this.gbFlowItems.Name = "gbFlowItems";
            this.gbFlowItems.Size = new System.Drawing.Size(236, 329);
            this.gbFlowItems.TabIndex = 2;
            this.gbFlowItems.TabStop = false;
            this.gbFlowItems.Text = "Choose the new flow item";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(426, 59);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 50);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // rbActTrans
            // 
            this.rbActTrans.Image = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.ActTransFlow;
            this.rbActTrans.Location = new System.Drawing.Point(19, 284);
            this.rbActTrans.Name = "rbActTrans";
            this.rbActTrans.Size = new System.Drawing.Size(126, 33);
            this.rbActTrans.TabIndex = 6;
            this.rbActTrans.Tag = "";
            this.rbActTrans.Text = "Act Trans";
            this.rbActTrans.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbActTrans.UseVisualStyleBackColor = true;
            this.rbActTrans.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(272, 59);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(86, 50);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // rbTransition
            // 
            this.rbTransition.Image = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.TransitionFlow;
            this.rbTransition.Location = new System.Drawing.Point(19, 246);
            this.rbTransition.Name = "rbTransition";
            this.rbTransition.Size = new System.Drawing.Size(126, 33);
            this.rbTransition.TabIndex = 5;
            this.rbTransition.Tag = "";
            this.rbTransition.Text = "Transition";
            this.rbTransition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbTransition.UseVisualStyleBackColor = true;
            this.rbTransition.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(272, 33);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(240, 20);
            this.tbText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(269, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Label text";
            // 
            // rbSubroutine
            // 
            this.rbSubroutine.Image = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.Subroutine;
            this.rbSubroutine.Location = new System.Drawing.Point(19, 120);
            this.rbSubroutine.Name = "rbSubroutine";
            this.rbSubroutine.Size = new System.Drawing.Size(126, 33);
            this.rbSubroutine.TabIndex = 2;
            this.rbSubroutine.Tag = "";
            this.rbSubroutine.Text = "Subroutine";
            this.rbSubroutine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbSubroutine.UseVisualStyleBackColor = true;
            this.rbSubroutine.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // rbStopReturn
            // 
            this.rbStopReturn.Image = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.StartStop;
            this.rbStopReturn.Location = new System.Drawing.Point(19, 207);
            this.rbStopReturn.Name = "rbStopReturn";
            this.rbStopReturn.Size = new System.Drawing.Size(126, 33);
            this.rbStopReturn.TabIndex = 4;
            this.rbStopReturn.Tag = "";
            this.rbStopReturn.Text = "Return Stop";
            this.rbStopReturn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbStopReturn.UseVisualStyleBackColor = true;
            this.rbStopReturn.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // rbStop
            // 
            this.rbStop.Image = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.StartStop;
            this.rbStop.Location = new System.Drawing.Point(19, 165);
            this.rbStop.Name = "rbStop";
            this.rbStop.Size = new System.Drawing.Size(126, 33);
            this.rbStop.TabIndex = 3;
            this.rbStop.Tag = "";
            this.rbStop.Text = "Stop";
            this.rbStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbStop.UseVisualStyleBackColor = true;
            this.rbStop.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // rbDecision
            // 
            this.rbDecision.Image = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.Decision;
            this.rbDecision.Location = new System.Drawing.Point(19, 78);
            this.rbDecision.Name = "rbDecision";
            this.rbDecision.Size = new System.Drawing.Size(126, 33);
            this.rbDecision.TabIndex = 1;
            this.rbDecision.Tag = "";
            this.rbDecision.Text = "Decision";
            this.rbDecision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbDecision.UseVisualStyleBackColor = true;
            this.rbDecision.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // rbAction
            // 
            this.rbAction.Image = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.ActionFlow;
            this.rbAction.Location = new System.Drawing.Point(19, 35);
            this.rbAction.Name = "rbAction";
            this.rbAction.Size = new System.Drawing.Size(126, 33);
            this.rbAction.TabIndex = 0;
            this.rbAction.Tag = "";
            this.rbAction.Text = "Action";
            this.rbAction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbAction.UseVisualStyleBackColor = true;
            this.rbAction.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // NewItemForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(528, 366);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbFlowItems);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewItemForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewItemForm";
            this.gbFlowItems.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFlowItems;
        private System.Windows.Forms.RadioButton rbAction;
        private System.Windows.Forms.RadioButton rbStop;
        private System.Windows.Forms.RadioButton rbDecision;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbStopReturn;
        private System.Windows.Forms.RadioButton rbSubroutine;
        private System.Windows.Forms.RadioButton rbTransition;
        private System.Windows.Forms.RadioButton rbActTrans;
    }
}