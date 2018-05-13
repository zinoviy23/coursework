namespace WinFormCourseWork
{
    partial class UserForm
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
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.userLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.errorTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(149, 48);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(140, 22);
            this.userTextBox.TabIndex = 0;
            this.userTextBox.TextChanged += new System.EventHandler(this.UserTextBoxOnTextChanged);
            this.userTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserTextBoxOnKeyDown);
            this.userTextBox.Leave += new System.EventHandler(this.UserTextBoxOnLeave);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(295, 47);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOkOnClick);
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(12, 54);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(131, 17);
            this.userLabel.TabIndex = 2;
            this.userLabel.Text = "Имя пользователя";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(12, 21);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(363, 17);
            this.infoLabel.TabIndex = 3;
            this.infoLabel.Text = "Введите своё имя. При отмене программа закроется.";
            // 
            // errorTooltip
            // 
            this.errorTooltip.AutoPopDelay = 5000;
            this.errorTooltip.InitialDelay = 100;
            this.errorTooltip.ReshowDelay = 100;
            this.errorTooltip.ShowAlways = true;
            this.errorTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            this.errorTooltip.ToolTipTitle = "Ошибка!";
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 99);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.userTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UserForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Пользователь";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserFormOnFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.ToolTip errorTooltip;
    }
}