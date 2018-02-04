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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Глава 1 Введение");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Теория", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Тест 1 Введение");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Тесты", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Куб");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Визуализации", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.checkTestButton = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.htmlView = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.checkTestButton);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.htmlView);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(856, 529);
            this.splitContainer1.SplitterDistance = 152;
            this.splitContainer1.TabIndex = 0;
            // 
            // checkTestButton
            // 
            this.checkTestButton.Location = new System.Drawing.Point(0, 494);
            this.checkTestButton.Name = "checkTestButton";
            this.checkTestButton.Size = new System.Drawing.Size(146, 32);
            this.checkTestButton.TabIndex = 1;
            this.checkTestButton.Text = "Проверить";
            this.checkTestButton.UseVisualStyleBackColor = true;
            this.checkTestButton.Click += new System.EventHandler(this.CheckTestButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Узел1";
            treeNode1.Tag = "lesson1.xml";
            treeNode1.Text = "Глава 1 Введение";
            treeNode2.Name = "Узел0";
            treeNode2.Text = "Теория";
            treeNode3.Name = "Узел3";
            treeNode3.Tag = "test_lesson1.xml";
            treeNode3.Text = "Тест 1 Введение";
            treeNode4.Name = "Узел2";
            treeNode4.Text = "Тесты";
            treeNode5.Name = "Узел3";
            treeNode5.Tag = "Cube";
            treeNode5.Text = "Куб";
            treeNode6.Name = "Узел0";
            treeNode6.Text = "Визуализации";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode4,
            treeNode6});
            this.treeView1.Size = new System.Drawing.Size(152, 529);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LessonsView_AfterSelect);
            // 
            // htmlView
            // 
            this.htmlView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlView.Location = new System.Drawing.Point(0, 0);
            this.htmlView.MinimumSize = new System.Drawing.Size(20, 20);
            this.htmlView.Name = "htmlView";
            this.htmlView.Size = new System.Drawing.Size(700, 529);
            this.htmlView.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 529);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.WebBrowser htmlView;
        private System.Windows.Forms.Button checkTestButton;
    }
}

