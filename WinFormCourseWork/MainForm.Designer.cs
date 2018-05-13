using OpenTK.Graphics;

namespace WinFormCourseWork
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Материалы");
            this.lessonsTreeView = new System.Windows.Forms.TreeView();
            this.htmlView = new System.Windows.Forms.WebBrowser();
            this.glTimer = new System.Windows.Forms.Timer(this.components);
            this.cayleyTableGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.checkButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cayleyTableGridView)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lessonsTreeView
            // 
            this.lessonsTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.lessonsTreeView.Indent = 7;
            this.lessonsTreeView.Location = new System.Drawing.Point(0, 28);
            this.lessonsTreeView.Name = "lessonsTreeView";
            treeNode1.Name = "Узел0";
            treeNode1.Text = "Материалы";
            this.lessonsTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.lessonsTreeView.Size = new System.Drawing.Size(152, 525);
            this.lessonsTreeView.TabIndex = 0;
            this.lessonsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LessonsView_AfterSelect);
            // 
            // htmlView
            // 
            this.htmlView.Location = new System.Drawing.Point(671, 31);
            this.htmlView.MinimumSize = new System.Drawing.Size(20, 20);
            this.htmlView.Name = "htmlView";
            this.htmlView.Size = new System.Drawing.Size(185, 437);
            this.htmlView.TabIndex = 0;
            this.htmlView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.HtmlView_PreviewKeyDown);
            // 
            // glTimer
            // 
            this.glTimer.Enabled = true;
            this.glTimer.Interval = 20;
            this.glTimer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // cayleyTableGridView
            // 
            this.cayleyTableGridView.AllowUserToAddRows = false;
            this.cayleyTableGridView.AllowUserToDeleteRows = false;
            this.cayleyTableGridView.AllowUserToOrderColumns = true;
            this.cayleyTableGridView.AllowUserToResizeColumns = false;
            this.cayleyTableGridView.AllowUserToResizeRows = false;
            this.cayleyTableGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cayleyTableGridView.Location = new System.Drawing.Point(321, 283);
            this.cayleyTableGridView.Name = "cayleyTableGridView";
            this.cayleyTableGridView.RowTemplate.Height = 24;
            this.cayleyTableGridView.Size = new System.Drawing.Size(240, 150);
            this.cayleyTableGridView.TabIndex = 2;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(856, 28);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkTestToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(50, 24);
            this.testToolStripMenuItem.Text = "Тест";
            // 
            // checkTestToolStripMenuItem
            // 
            this.checkTestToolStripMenuItem.Name = "checkTestToolStripMenuItem";
            this.checkTestToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.checkTestToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F5)));
            this.checkTestToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            this.checkTestToolStripMenuItem.Text = "Проверить";
            this.checkTestToolStripMenuItem.Click += new System.EventHandler(this.CheckTestButtonOnClick);
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.buttonsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buttonsPanel.Controls.Add(this.checkButton);
            this.buttonsPanel.Controls.Add(this.nextButton);
            this.buttonsPanel.Controls.Add(this.backButton);
            this.buttonsPanel.Location = new System.Drawing.Point(158, 487);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(698, 41);
            this.buttonsPanel.TabIndex = 4;
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(311, 1);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(124, 38);
            this.checkButton.TabIndex = 2;
            this.checkButton.Text = "Проверить";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Visible = false;
            this.checkButton.Click += new System.EventHandler(this.CheckTestButtonOnClick);
            // 
            // nextButton
            // 
            this.nextButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.nextButton.Location = new System.Drawing.Point(602, 0);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(94, 39);
            this.nextButton.TabIndex = 1;
            this.nextButton.Text = "Вперед >";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.NextButtonOnClick);
            // 
            // backButton
            // 
            this.backButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.backButton.Location = new System.Drawing.Point(0, 0);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 39);
            this.backButton.TabIndex = 0;
            this.backButton.Text = "< Назад";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.BackButtonOnClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 553);
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.cayleyTableGridView);
            this.Controls.Add(this.htmlView);
            this.Controls.Add(this.lessonsTreeView);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainFormOnLoad);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.cayleyTableGridView)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.buttonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView lessonsTreeView;
        private System.Windows.Forms.WebBrowser htmlView;
        private System.Windows.Forms.Timer glTimer;
        private System.Windows.Forms.DataGridView cayleyTableGridView;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkTestToolStripMenuItem;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button checkButton;
    }
}

