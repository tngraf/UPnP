namespace UpnpAnalyzer.UI
{
    partial class ActionInfoControl
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabInput = new System.Windows.Forms.TabPage();
            this.dataGridInputs = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAllowedValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.dataGridOutputs = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rtfStatus = new System.Windows.Forms.RichTextBox();
            this.btnInvokeAction = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInputs)).BeginInit();
            this.tabOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOutputs)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabInput);
            this.tabControl.Controls.Add(this.tabOutput);
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(531, 351);
            this.tabControl.TabIndex = 0;
            // 
            // tabInput
            // 
            this.tabInput.BackColor = System.Drawing.SystemColors.Control;
            this.tabInput.Controls.Add(this.dataGridInputs);
            this.tabInput.Location = new System.Drawing.Point(4, 22);
            this.tabInput.Name = "tabInput";
            this.tabInput.Padding = new System.Windows.Forms.Padding(3);
            this.tabInput.Size = new System.Drawing.Size(523, 325);
            this.tabInput.TabIndex = 0;
            this.tabInput.Text = "Inputs";
            // 
            // dataGridInputs
            // 
            this.dataGridInputs.AllowUserToAddRows = false;
            this.dataGridInputs.AllowUserToDeleteRows = false;
            this.dataGridInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridInputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colType,
            this.colValue,
            this.colAllowedValues});
            this.dataGridInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridInputs.Location = new System.Drawing.Point(3, 3);
            this.dataGridInputs.Name = "dataGridInputs";
            this.dataGridInputs.Size = new System.Drawing.Size(517, 319);
            this.dataGridInputs.TabIndex = 0;
            // 
            // colName
            // 
            this.colName.HeaderText = "Input Argument";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "Data Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colValue
            // 
            this.colValue.HeaderText = "Input Value";
            this.colValue.Name = "colValue";
            // 
            // colAllowedValues
            // 
            this.colAllowedValues.HeaderText = "Allowed Values";
            this.colAllowedValues.Name = "colAllowedValues";
            this.colAllowedValues.ReadOnly = true;
            // 
            // tabOutput
            // 
            this.tabOutput.BackColor = System.Drawing.SystemColors.Control;
            this.tabOutput.Controls.Add(this.dataGridOutputs);
            this.tabOutput.Location = new System.Drawing.Point(4, 22);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabOutput.Size = new System.Drawing.Size(523, 325);
            this.tabOutput.TabIndex = 1;
            this.tabOutput.Text = "Outputs";
            // 
            // dataGridOutputs
            // 
            this.dataGridOutputs.AllowUserToAddRows = false;
            this.dataGridOutputs.AllowUserToDeleteRows = false;
            this.dataGridOutputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridOutputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dataGridOutputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridOutputs.Location = new System.Drawing.Point(3, 3);
            this.dataGridOutputs.Name = "dataGridOutputs";
            this.dataGridOutputs.Size = new System.Drawing.Size(517, 319);
            this.dataGridOutputs.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Output Argument";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Data Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Output Value";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // rtfStatus
            // 
            this.rtfStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfStatus.BackColor = System.Drawing.SystemColors.Window;
            this.rtfStatus.Location = new System.Drawing.Point(7, 353);
            this.rtfStatus.Name = "rtfStatus";
            this.rtfStatus.Size = new System.Drawing.Size(520, 50);
            this.rtfStatus.TabIndex = 2;
            this.rtfStatus.Text = "";
            // 
            // btnInvokeAction
            // 
            this.btnInvokeAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInvokeAction.Location = new System.Drawing.Point(7, 412);
            this.btnInvokeAction.Name = "btnInvokeAction";
            this.btnInvokeAction.Size = new System.Drawing.Size(106, 23);
            this.btnInvokeAction.TabIndex = 1;
            this.btnInvokeAction.Text = "Invoke Action";
            this.btnInvokeAction.UseVisualStyleBackColor = true;
            this.btnInvokeAction.Click += new System.EventHandler(this.BtnInvokeActionClick);
            // 
            // ActionInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnInvokeAction);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.rtfStatus);
            this.Name = "ActionInfoControl";
            this.Size = new System.Drawing.Size(537, 438);
            this.tabControl.ResumeLayout(false);
            this.tabInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInputs)).EndInit();
            this.tabOutput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOutputs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabInput;
        private System.Windows.Forms.TabPage tabOutput;
        private System.Windows.Forms.Button btnInvokeAction;
        private System.Windows.Forms.DataGridView dataGridInputs;
        private System.Windows.Forms.DataGridView dataGridOutputs;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAllowedValues;
        private System.Windows.Forms.RichTextBox rtfStatus;
    }
}
