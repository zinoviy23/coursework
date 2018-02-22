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
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Глава 1 Введение");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Теория", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Тест 1 Введение");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Таблица Кэли");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Тесты", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode14});
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Тетраэдр");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Куб");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Октаэдр");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Икосаэдр");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Визуализации", new System.Windows.Forms.TreeNode[] {
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.htmlView = new System.Windows.Forms.WebBrowser();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.glControl1 = new OpenTK.GLControl();
            this.cayleyTableGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.cayleyTableGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 28);
            this.treeView1.Name = "treeView1";
            treeNode11.Name = "Узел1";
            treeNode11.Tag = "lesson1.xml";
            treeNode11.Text = "Глава 1 Введение";
            treeNode12.Name = "Узел0";
            treeNode12.Text = "Теория";
            treeNode13.Name = "Узел3";
            treeNode13.Tag = "test_lesson1.xml";
            treeNode13.Text = "Тест 1 Введение";
            treeNode14.Name = "Узел0";
            treeNode14.Tag = "Cayley Table";
            treeNode14.Text = "Таблица Кэли";
            treeNode15.Name = "Узел2";
            treeNode15.Text = "Тесты";
            treeNode16.Name = "Узел0";
            treeNode16.Tag = "VisualisationTetrahedron";
            treeNode16.Text = "Тетраэдр";
            treeNode17.Name = "Узел3";
            treeNode17.Tag = "VisualisationCube";
            treeNode17.Text = "Куб";
            treeNode18.Name = "Узел1";
            treeNode18.Tag = "VisualisationOctahedron";
            treeNode18.Text = "Октаэдр";
            treeNode19.Name = "Узел1";
            treeNode19.Tag = "VisualisationIcosahedron";
            treeNode19.Text = "Икосаэдр";
            treeNode20.Name = "Узел0";
            treeNode20.Text = "Визуализации";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode15,
            treeNode20});
            this.treeView1.Size = new System.Drawing.Size(152, 501);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LessonsView_AfterSelect);
            // 
            // htmlView
            // 
            this.htmlView.Dock = System.Windows.Forms.DockStyle.Right;
            this.htmlView.Location = new System.Drawing.Point(664, 28);
            this.htmlView.MinimumSize = new System.Drawing.Size(20, 20);
            this.htmlView.Name = "htmlView";
            this.htmlView.Size = new System.Drawing.Size(192, 501);
            this.htmlView.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(372, 65);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(200, 185);
            this.glControl1.TabIndex = 1;
            this.glControl1.VSync = false;
            this.glControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
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
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(856, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
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
            this.checkTestToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
            this.checkTestToolStripMenuItem.Text = "Проверить";
            this.checkTestToolStripMenuItem.Click += new System.EventHandler(this.CheckTestButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 529);
            this.Controls.Add(this.cayleyTableGridView);
            this.Controls.Add(this.htmlView);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.cayleyTableGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.WebBrowser htmlView;
        private System.Windows.Forms.Timer timer1;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.DataGridView cayleyTableGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkTestToolStripMenuItem;
    }
}

