
namespace Receipt_Printing_New
{
    partial class PrinterSetting
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btAddNewMapping = new System.Windows.Forms.Button();
            this.btsave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(498, 637);
            this.panel1.TabIndex = 0;
            // 
            // btAddNewMapping
            // 
            this.btAddNewMapping.Location = new System.Drawing.Point(365, 22);
            this.btAddNewMapping.Name = "btAddNewMapping";
            this.btAddNewMapping.Size = new System.Drawing.Size(134, 23);
            this.btAddNewMapping.TabIndex = 1;
            this.btAddNewMapping.Text = "Add new mapping";
            this.btAddNewMapping.UseVisualStyleBackColor = true;
            this.btAddNewMapping.Click += new System.EventHandler(this.btAddNewMapping_Click);
            // 
            // btsave
            // 
            this.btsave.Location = new System.Drawing.Point(284, 22);
            this.btsave.Name = "btsave";
            this.btsave.Size = new System.Drawing.Size(75, 23);
            this.btsave.TabIndex = 2;
            this.btsave.Text = "Save";
            this.btsave.UseVisualStyleBackColor = true;
            this.btsave.Click += new System.EventHandler(this.btsave_Click);
            // 
            // PrinterSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 761);
            this.Controls.Add(this.btAddNewMapping);
            this.Controls.Add(this.btsave);
            this.Controls.Add(this.panel1);
            this.Name = "PrinterSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrinterSetting";
            this.Load += new System.EventHandler(this.PrinterSetting_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btAddNewMapping;
        private System.Windows.Forms.Button btsave;
    }
}