namespace Receipt_Printing_New
{
    partial class ucModule1
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
            this.label1 = new System.Windows.Forms.Label();
            this.cklstboxparameter = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cklstboxfullreceipt = new System.Windows.Forms.CheckedListBox();
            this.cbprinter = new System.Windows.Forms.ComboBox();
            this.cbcopy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstboxsize = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupsetting = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbfullreceipt = new System.Windows.Forms.CheckBox();
            this.groupsetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Parameter";
            // 
            // cklstboxparameter
            // 
            this.cklstboxparameter.CheckOnClick = true;
            this.cklstboxparameter.FormattingEnabled = true;
            this.cklstboxparameter.Items.AddRange(new object[] {
            "Online",
            "Local",
            "Use Printing",
            "Split Items"});
            this.cklstboxparameter.Location = new System.Drawing.Point(16, 44);
            this.cklstboxparameter.Name = "cklstboxparameter";
            this.cklstboxparameter.Size = new System.Drawing.Size(120, 79);
            this.cklstboxparameter.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Full Receipt";
            // 
            // cklstboxfullreceipt
            // 
            this.cklstboxfullreceipt.CheckOnClick = true;
            this.cklstboxfullreceipt.FormattingEnabled = true;
            this.cklstboxfullreceipt.Location = new System.Drawing.Point(153, 44);
            this.cklstboxfullreceipt.Name = "cklstboxfullreceipt";
            this.cklstboxfullreceipt.Size = new System.Drawing.Size(120, 79);
            this.cklstboxfullreceipt.TabIndex = 2;
            this.cklstboxfullreceipt.SelectedIndexChanged += new System.EventHandler(this.cklstboxfullreceipt_SelectedIndexChanged);
            // 
            // cbprinter
            // 
            this.cbprinter.FormattingEnabled = true;
            this.cbprinter.Location = new System.Drawing.Point(311, 44);
            this.cbprinter.Name = "cbprinter";
            this.cbprinter.Size = new System.Drawing.Size(121, 21);
            this.cbprinter.TabIndex = 3;
            // 
            // cbcopy
            // 
            this.cbcopy.FormattingEnabled = true;
            this.cbcopy.Location = new System.Drawing.Point(363, 71);
            this.cbcopy.Name = "cbcopy";
            this.cbcopy.Size = new System.Drawing.Size(69, 21);
            this.cbcopy.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(308, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Copies";
            // 
            // lstboxsize
            // 
            this.lstboxsize.FormattingEnabled = true;
            this.lstboxsize.Items.AddRange(new object[] {
            "58 mm",
            "80 mm"});
            this.lstboxsize.Location = new System.Drawing.Point(311, 97);
            this.lstboxsize.Name = "lstboxsize";
            this.lstboxsize.Size = new System.Drawing.Size(120, 30);
            this.lstboxsize.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(356, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupsetting
            // 
            this.groupsetting.Controls.Add(this.cbfullreceipt);
            this.groupsetting.Controls.Add(this.label5);
            this.groupsetting.Controls.Add(this.label4);
            this.groupsetting.Controls.Add(this.cbprinter);
            this.groupsetting.Controls.Add(this.button1);
            this.groupsetting.Controls.Add(this.cklstboxparameter);
            this.groupsetting.Controls.Add(this.lstboxsize);
            this.groupsetting.Controls.Add(this.label3);
            this.groupsetting.Controls.Add(this.cbcopy);
            this.groupsetting.Controls.Add(this.cklstboxfullreceipt);
            this.groupsetting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupsetting.Location = new System.Drawing.Point(0, 21);
            this.groupsetting.Name = "groupsetting";
            this.groupsetting.Size = new System.Drawing.Size(469, 176);
            this.groupsetting.TabIndex = 6;
            this.groupsetting.TabStop = false;
            this.groupsetting.Text = "Setting";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(174, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "FullReceipt";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Parameters";
            // 
            // cbfullreceipt
            // 
            this.cbfullreceipt.AutoSize = true;
            this.cbfullreceipt.Checked = true;
            this.cbfullreceipt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbfullreceipt.Location = new System.Drawing.Point(153, 28);
            this.cbfullreceipt.Name = "cbfullreceipt";
            this.cbfullreceipt.Size = new System.Drawing.Size(15, 14);
            this.cbfullreceipt.TabIndex = 7;
            this.cbfullreceipt.UseVisualStyleBackColor = true;
            this.cbfullreceipt.CheckedChanged += new System.EventHandler(this.cbfullreceipt_CheckedChanged);
            this.cbfullreceipt.Click += new System.EventHandler(this.cbfullreceipt_Click);

            // 
            // ucModule1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupsetting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ucModule1";
            this.Size = new System.Drawing.Size(469, 197);
            this.groupsetting.ResumeLayout(false);
            this.groupsetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox cklstboxparameter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox cklstboxfullreceipt;
        private System.Windows.Forms.ComboBox cbprinter;
        private System.Windows.Forms.ComboBox cbcopy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstboxsize;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupsetting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbfullreceipt;
    }
}
