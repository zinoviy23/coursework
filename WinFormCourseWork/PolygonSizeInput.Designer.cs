namespace WinFormCourseWork
{
    partial class PolygonSizeInput
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
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.textBox = new System.Windows.Forms.TextBox();
            this.label = new System.Windows.Forms.Label();
            this.button = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(12, 42);
            this.trackBar.Maximum = 20;
            this.trackBar.Minimum = 3;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(344, 56);
            this.trackBar.TabIndex = 0;
            this.trackBar.Value = 3;
            this.trackBar.ValueChanged += new System.EventHandler(this.TrackBarOnValueChanged);
            this.trackBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TrackBarOnKeyDown);
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(362, 42);
            this.textBox.MaxLength = 2;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(48, 22);
            this.textBox.TabIndex = 1;
            this.textBox.TextChanged += new System.EventHandler(this.TextBoxOnTextChanged);
            this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxOnKeyDown);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(398, 17);
            this.label.TabIndex = 2;
            this.label.Text = "Выберете количество вершин многоугольника. От 3 до 20.";
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(188, 89);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(48, 23);
            this.button.TabIndex = 3;
            this.button.Text = "ОК";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.ButtonOnClick);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            this.toolTip.ToolTipTitle = "Ошибка ввода!";
            // 
            // PolygonSizeInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 124);
            this.Controls.Add(this.button);
            this.Controls.Add(this.label);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.trackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PolygonSizeInput";
            this.Text = "Ввод";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PolygonSizeInputOnKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button button;
        private System.Windows.Forms.ToolTip toolTip;
    }
}