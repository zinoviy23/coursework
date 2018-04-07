﻿namespace WinFormCourseWork
{
    partial class PermulationInput
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
            this.permulationHead = new System.Windows.Forms.Label();
            this.permulationTextBox = new System.Windows.Forms.TextBox();
            this.permulationLengthComboBox = new System.Windows.Forms.ComboBox();
            this.leftParenthesisLabel = new System.Windows.Forms.Label();
            this.rightParenthesisLabel = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // permulationHead
            // 
            this.permulationHead.AutoSize = true;
            this.permulationHead.Location = new System.Drawing.Point(40, 26);
            this.permulationHead.Name = "permulationHead";
            this.permulationHead.Size = new System.Drawing.Size(16, 17);
            this.permulationHead.TabIndex = 0;
            this.permulationHead.Text = "_";
            // 
            // permulationTextBox
            // 
            this.permulationTextBox.Location = new System.Drawing.Point(43, 46);
            this.permulationTextBox.Name = "permulationTextBox";
            this.permulationTextBox.Size = new System.Drawing.Size(272, 22);
            this.permulationTextBox.TabIndex = 1;
            // 
            // permulationLengthComboBox
            // 
            this.permulationLengthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.permulationLengthComboBox.FormattingEnabled = true;
            this.permulationLengthComboBox.Items.AddRange(new object[] {
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.permulationLengthComboBox.Location = new System.Drawing.Point(371, 44);
            this.permulationLengthComboBox.Name = "permulationLengthComboBox";
            this.permulationLengthComboBox.Size = new System.Drawing.Size(46, 24);
            this.permulationLengthComboBox.TabIndex = 2;
            this.permulationLengthComboBox.SelectedIndexChanged += new System.EventHandler(this.PermulationLengthComboBox_SelectedIndexChanged);
            // 
            // leftParenthesisLabel
            // 
            this.leftParenthesisLabel.AutoSize = true;
            this.leftParenthesisLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftParenthesisLabel.Location = new System.Drawing.Point(12, 29);
            this.leftParenthesisLabel.Name = "leftParenthesisLabel";
            this.leftParenthesisLabel.Size = new System.Drawing.Size(28, 39);
            this.leftParenthesisLabel.TabIndex = 3;
            this.leftParenthesisLabel.Text = "(";
            // 
            // rightParenthesisLabel
            // 
            this.rightParenthesisLabel.AutoSize = true;
            this.rightParenthesisLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightParenthesisLabel.Location = new System.Drawing.Point(321, 31);
            this.rightParenthesisLabel.Name = "rightParenthesisLabel";
            this.rightParenthesisLabel.Size = new System.Drawing.Size(28, 39);
            this.rightParenthesisLabel.TabIndex = 4;
            this.rightParenthesisLabel.Text = ")";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(197, 93);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 5;
            this.buttonOk.Text = "Ок";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // PermulationInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 128);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.rightParenthesisLabel);
            this.Controls.Add(this.leftParenthesisLabel);
            this.Controls.Add(this.permulationLengthComboBox);
            this.Controls.Add(this.permulationTextBox);
            this.Controls.Add(this.permulationHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PermulationInput";
            this.Text = "Введите подстановку";
            this.Load += new System.EventHandler(this.PermulationInput_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label permulationHead;
        private System.Windows.Forms.TextBox permulationTextBox;
        private System.Windows.Forms.ComboBox permulationLengthComboBox;
        private System.Windows.Forms.Label leftParenthesisLabel;
        private System.Windows.Forms.Label rightParenthesisLabel;
        private System.Windows.Forms.Button buttonOk;
    }
}