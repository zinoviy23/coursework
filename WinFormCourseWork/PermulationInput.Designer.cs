namespace WinFormCourseWork
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
            this.SuspendLayout();
            // 
            // permulationHead
            // 
            this.permulationHead.AutoSize = true;
            this.permulationHead.Location = new System.Drawing.Point(13, 13);
            this.permulationHead.Name = "permulationHead";
            this.permulationHead.Size = new System.Drawing.Size(16, 17);
            this.permulationHead.TabIndex = 0;
            this.permulationHead.Text = "_";
            // 
            // permulationTextBox
            // 
            this.permulationTextBox.Location = new System.Drawing.Point(12, 46);
            this.permulationTextBox.Name = "permulationTextBox";
            this.permulationTextBox.Size = new System.Drawing.Size(272, 22);
            this.permulationTextBox.TabIndex = 1;
            // 
            // permulationLengthComboBox
            // 
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
            // 
            // PermulationInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 105);
            this.Controls.Add(this.permulationLengthComboBox);
            this.Controls.Add(this.permulationTextBox);
            this.Controls.Add(this.permulationHead);
            this.Name = "PermulationInput";
            this.Text = "Введите подстановку";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label permulationHead;
        private System.Windows.Forms.TextBox permulationTextBox;
        private System.Windows.Forms.ComboBox permulationLengthComboBox;
    }
}