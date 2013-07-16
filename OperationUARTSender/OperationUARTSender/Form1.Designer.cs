﻿namespace OperationUARTSender
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonDigSwitch = new System.Windows.Forms.Button();
            this.buttonTemp = new System.Windows.Forms.Button();
            this.buttonHum = new System.Windows.Forms.Button();
            this.buttonDateTime = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBoxConfigAddress = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.buttonMAC = new System.Windows.Forms.Button();
            this.buttonFirm = new System.Windows.Forms.Button();
            this.buttonCheckSum = new System.Windows.Forms.Button();
            this.buttonDigRead = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSwitchTime = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(13, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.comboBox1_MouseClick);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(140, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(586, 290);
            this.listBox1.TabIndex = 2;
            // 
            // buttonDigSwitch
            // 
            this.buttonDigSwitch.Enabled = false;
            this.buttonDigSwitch.Location = new System.Drawing.Point(605, 50);
            this.buttonDigSwitch.Name = "buttonDigSwitch";
            this.buttonDigSwitch.Size = new System.Drawing.Size(99, 37);
            this.buttonDigSwitch.TabIndex = 3;
            this.buttonDigSwitch.Text = "DIG SWITCH";
            this.buttonDigSwitch.UseVisualStyleBackColor = true;
            this.buttonDigSwitch.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // buttonTemp
            // 
            this.buttonTemp.Enabled = false;
            this.buttonTemp.Location = new System.Drawing.Point(815, 111);
            this.buttonTemp.Name = "buttonTemp";
            this.buttonTemp.Size = new System.Drawing.Size(99, 37);
            this.buttonTemp.TabIndex = 3;
            this.buttonTemp.Text = "TEMPERATURE";
            this.buttonTemp.UseVisualStyleBackColor = true;
            this.buttonTemp.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // buttonHum
            // 
            this.buttonHum.Enabled = false;
            this.buttonHum.Location = new System.Drawing.Point(815, 50);
            this.buttonHum.Name = "buttonHum";
            this.buttonHum.Size = new System.Drawing.Size(99, 37);
            this.buttonHum.TabIndex = 3;
            this.buttonHum.Text = "HUMIDITY";
            this.buttonHum.UseVisualStyleBackColor = true;
            this.buttonHum.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // buttonDateTime
            // 
            this.buttonDateTime.Enabled = false;
            this.buttonDateTime.Location = new System.Drawing.Point(605, 227);
            this.buttonDateTime.Name = "buttonDateTime";
            this.buttonDateTime.Size = new System.Drawing.Size(99, 37);
            this.buttonDateTime.TabIndex = 3;
            this.buttonDateTime.Text = "DATETIME";
            this.buttonDateTime.UseVisualStyleBackColor = true;
            this.buttonDateTime.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBoxConfigAddress
            // 
            this.textBoxConfigAddress.Location = new System.Drawing.Point(263, 12);
            this.textBoxConfigAddress.Name = "textBoxConfigAddress";
            this.textBoxConfigAddress.Size = new System.Drawing.Size(53, 20);
            this.textBoxConfigAddress.TabIndex = 4;
            this.textBoxConfigAddress.Text = "0000";
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(334, 11);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(92, 22);
            this.button7.TabIndex = 5;
            this.button7.Text = "Send Config";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Enabled = false;
            this.button8.Location = new System.Drawing.Point(530, 10);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(92, 22);
            this.button8.TabIndex = 5;
            this.button8.Text = "Send DateTime";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Enabled = false;
            this.button9.Location = new System.Drawing.Point(432, 10);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(92, 22);
            this.button9.TabIndex = 5;
            this.button9.Text = "Read Config";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // buttonMAC
            // 
            this.buttonMAC.Enabled = false;
            this.buttonMAC.Location = new System.Drawing.Point(710, 50);
            this.buttonMAC.Name = "buttonMAC";
            this.buttonMAC.Size = new System.Drawing.Size(99, 37);
            this.buttonMAC.TabIndex = 3;
            this.buttonMAC.Text = "MAC";
            this.buttonMAC.UseVisualStyleBackColor = true;
            this.buttonMAC.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // buttonFirm
            // 
            this.buttonFirm.Enabled = false;
            this.buttonFirm.Location = new System.Drawing.Point(710, 109);
            this.buttonFirm.Name = "buttonFirm";
            this.buttonFirm.Size = new System.Drawing.Size(99, 37);
            this.buttonFirm.TabIndex = 3;
            this.buttonFirm.Text = "FIRMWARE";
            this.buttonFirm.UseVisualStyleBackColor = true;
            this.buttonFirm.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // buttonCheckSum
            // 
            this.buttonCheckSum.Enabled = false;
            this.buttonCheckSum.Location = new System.Drawing.Point(710, 172);
            this.buttonCheckSum.Name = "buttonCheckSum";
            this.buttonCheckSum.Size = new System.Drawing.Size(99, 37);
            this.buttonCheckSum.TabIndex = 3;
            this.buttonCheckSum.Text = "CHECKSUM";
            this.buttonCheckSum.UseVisualStyleBackColor = true;
            this.buttonCheckSum.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // buttonDigRead
            // 
            this.buttonDigRead.Enabled = false;
            this.buttonDigRead.Location = new System.Drawing.Point(605, 172);
            this.buttonDigRead.Name = "buttonDigRead";
            this.buttonDigRead.Size = new System.Drawing.Size(99, 37);
            this.buttonDigRead.TabIndex = 3;
            this.buttonDigRead.Text = "DIG READ";
            this.buttonDigRead.UseVisualStyleBackColor = true;
            this.buttonDigRead.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Enabled = false;
            this.buttonReset.Location = new System.Drawing.Point(710, 286);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(99, 37);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "RESET";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // buttonSwitchTime
            // 
            this.buttonSwitchTime.Enabled = false;
            this.buttonSwitchTime.Location = new System.Drawing.Point(605, 111);
            this.buttonSwitchTime.Name = "buttonSwitchTime";
            this.buttonSwitchTime.Size = new System.Drawing.Size(99, 37);
            this.buttonSwitchTime.TabIndex = 3;
            this.buttonSwitchTime.Text = "DIG SWITCH TIME";
            this.buttonSwitchTime.UseVisualStyleBackColor = true;
            this.buttonSwitchTime.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 341);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.textBoxConfigAddress);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonDigRead);
            this.Controls.Add(this.buttonDateTime);
            this.Controls.Add(this.buttonCheckSum);
            this.Controls.Add(this.buttonHum);
            this.Controls.Add(this.buttonFirm);
            this.Controls.Add(this.buttonTemp);
            this.Controls.Add(this.buttonMAC);
            this.Controls.Add(this.buttonSwitchTime);
            this.Controls.Add(this.buttonDigSwitch);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "UART Operation Sender";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonDigSwitch;
        private System.Windows.Forms.Button buttonTemp;
        private System.Windows.Forms.Button buttonHum;
        private System.Windows.Forms.Button buttonDateTime;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBoxConfigAddress;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonMAC;
        private System.Windows.Forms.Button buttonFirm;
        private System.Windows.Forms.Button buttonCheckSum;
        private System.Windows.Forms.Button buttonDigRead;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSwitchTime;
    }
}
