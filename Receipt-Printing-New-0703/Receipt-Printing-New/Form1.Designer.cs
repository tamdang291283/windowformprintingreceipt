namespace Receipt_Printing_New
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.maintab = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btAddNewMapping = new System.Windows.Forms.Button();
            this.btsave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtAddressURL = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.AutoStart = new System.Windows.Forms.CheckBox();
            this.EnableSound = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.traynotification = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnsavesetting = new System.Windows.Forms.Button();
            this.maintab.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // maintab
            // 
            this.maintab.Controls.Add(this.tabPage2);
            this.maintab.Controls.Add(this.tabPage1);
            this.maintab.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maintab.Location = new System.Drawing.Point(3, 2);
            this.maintab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.maintab.Multiline = true;
            this.maintab.Name = "maintab";
            this.maintab.SelectedIndex = 0;
            this.maintab.Size = new System.Drawing.Size(1155, 866);
            this.maintab.TabIndex = 0;
            this.maintab.Tag = "";
            // 
            // tabPage2
            // 
            this.tabPage2.CausesValidation = false;
            this.tabPage2.Controls.Add(this.btAddNewMapping);
            this.tabPage2.Controls.Add(this.btsave);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1147, 828);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Main";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // btAddNewMapping
            // 
            this.btAddNewMapping.Location = new System.Drawing.Point(889, 25);
            this.btAddNewMapping.Margin = new System.Windows.Forms.Padding(4);
            this.btAddNewMapping.Name = "btAddNewMapping";
            this.btAddNewMapping.Size = new System.Drawing.Size(247, 47);
            this.btAddNewMapping.TabIndex = 1;
            this.btAddNewMapping.Text = "Add new mapping";
            this.btAddNewMapping.UseVisualStyleBackColor = true;
            this.btAddNewMapping.Click += new System.EventHandler(this.btAddNewMapping_Click);
            // 
            // btsave
            // 
            this.btsave.Location = new System.Drawing.Point(764, 25);
            this.btsave.Margin = new System.Windows.Forms.Padding(4);
            this.btsave.Name = "btsave";
            this.btsave.Size = new System.Drawing.Size(117, 47);
            this.btsave.TabIndex = 1;
            this.btsave.Text = "Save";
            this.btsave.UseVisualStyleBackColor = true;
            this.btsave.Click += new System.EventHandler(this.btsave_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(0, 74);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1136, 710);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnsavesetting);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.btnStop);
            this.tabPage1.Controls.Add(this.btnStart);
            this.tabPage1.Controls.Add(this.txtAddressURL);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.AutoStart);
            this.tabPage1.Controls.Add(this.EnableSound);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtInterval);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(1147, 828);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Printing Setting";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(65, 270);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(205, 36);
            this.button1.TabIndex = 15;
            this.button1.TabStop = false;
            this.button1.Text = "EOD REPORT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(961, 270);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 36);
            this.btnStop.TabIndex = 14;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(694, 270);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 36);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtAddressURL
            // 
            this.txtAddressURL.Location = new System.Drawing.Point(189, 139);
            this.txtAddressURL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAddressURL.Multiline = true;
            this.txtAddressURL.Name = "txtAddressURL";
            this.txtAddressURL.Size = new System.Drawing.Size(847, 93);
            this.txtAddressURL.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(19, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 25);
            this.label6.TabIndex = 11;
            this.label6.Text = "Url  Order Come";
            // 
            // AutoStart
            // 
            this.AutoStart.AutoSize = true;
            this.AutoStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoStart.Location = new System.Drawing.Point(708, 75);
            this.AutoStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AutoStart.Name = "AutoStart";
            this.AutoStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AutoStart.Size = new System.Drawing.Size(121, 29);
            this.AutoStart.TabIndex = 10;
            this.AutoStart.Text = "Auto Start";
            this.AutoStart.UseVisualStyleBackColor = true;
            // 
            // EnableSound
            // 
            this.EnableSound.AutoSize = true;
            this.EnableSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnableSound.Location = new System.Drawing.Point(477, 77);
            this.EnableSound.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.EnableSound.Name = "EnableSound";
            this.EnableSound.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.EnableSound.Size = new System.Drawing.Size(158, 29);
            this.EnableSound.TabIndex = 9;
            this.EnableSound.Text = "Enable Sound";
            this.EnableSound.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(380, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 25);
            this.label5.TabIndex = 8;
            this.label5.Text = "Sec";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(189, 75);
            this.txtInterval.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(168, 30);
            this.txtInterval.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(43, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 25);
            this.label4.TabIndex = 5;
            this.label4.Text = "Timer Interval";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(380, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Printing Receipt";
            // 
            // traynotification
            // 
            this.traynotification.Icon = ((System.Drawing.Icon)(resources.GetObject("traynotification.Icon")));
            this.traynotification.Text = "Notification";
            this.traynotification.DoubleClick += new System.EventHandler(this.traynotification_DoubleClick);
            // 
            // btnsavesetting
            // 
            this.btnsavesetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsavesetting.Location = new System.Drawing.Point(415, 270);
            this.btnsavesetting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnsavesetting.Name = "btnsavesetting";
            this.btnsavesetting.Size = new System.Drawing.Size(150, 36);
            this.btnsavesetting.TabIndex = 16;
            this.btnsavesetting.Text = "Save Setting";
            this.btnsavesetting.UseVisualStyleBackColor = true;
            this.btnsavesetting.Click += new System.EventHandler(this.btnsavesetting_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 976);
            this.Controls.Add(this.maintab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Printing Receipt";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.maintab.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl maintab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox AutoStart;
        private System.Windows.Forms.CheckBox EnableSound;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAddressURL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NotifyIcon traynotification;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btAddNewMapping;
        private System.Windows.Forms.Button btsave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnsavesetting;
    }
}

