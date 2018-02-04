using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LessonLibrary;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WinFormCourseWork
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Файл для отладки
        /// </summary>
        private readonly StreamWriter _debugWriter;

        private TestLesson _currentTest;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <inheritdoc cref="Form"/>
        public MainForm()
        {
            InitializeComponent();
            _debugWriter = new StreamWriter("DebugHelper");
            LoadLesson("title_page.xml");
            splitContainer1.Panel1MinSize = Math.Min(200, Size.Width / 5);
            splitContainer1.Panel2MinSize = Width - splitContainer1.Panel1MinSize - 30;

            Closed += (sender, args) => _debugWriter?.Close();
        }

        private Tmp _tmp;

        /// <summary>
        /// Обработчик нажатия на элемент уроков
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Параметры</param>
        private void LessonsView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            if (node.Tag == null) return;
            _tmp?.Close();
            _tmp = null;
            if ((string) node.Tag != "Cube")
            {
                LoadLesson((string) node.Tag);
                htmlView.Show();
            }
            else
            {
                htmlView.Hide();
                _currentTest = null;
                checkTestButton.Enabled = false;
                checkTestButton.Visible = false;
            }
        }

        /// <summary>
        /// Отображает урок
        /// </summary>
        /// <param name="fileName">Файл урока</param>
        private void LoadLesson(string fileName)
        {
            try
            {
                var tmp = LessonReader.ReadHtmlViewLesson(@"lessons\" + fileName);
                htmlView.DocumentText = tmp.HtmlString;
                if (fileName.StartsWith("test"))
                {
                    _currentTest = tmp as TestLesson;
                    checkTestButton.Enabled = true;
                    checkTestButton.Visible = true;
                }
                else
                {
                    _currentTest = null;
                    checkTestButton.Enabled = false;
                    checkTestButton.Visible = false;
                }

                htmlView.DocumentCompleted += (sender, args) =>
                {
                    tmp.HtmlView = htmlView;
                    _debugWriter.WriteLine(htmlView.DocumentText);
                };
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                htmlView.DocumentText = "";
            }
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку проверки ответов
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Параметры</param>
        private void CheckTestButton_Click(object sender, EventArgs e)
        {
            var mistakes = _currentTest.CheckAnswers();
            if (mistakes == null)
                return;

            if (mistakes.Count == 0)
            {
                MessageBox.Show(@"Всё правильно!", @"Ошибок нет!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var mistakesStringBuilder = new StringBuilder("Ошибки в номерах: \n");
            foreach (var mistake in mistakes)
            {
                mistakesStringBuilder.Append(mistake + 1).Append("\n");
            }

            MessageBox.Show(mistakesStringBuilder.ToString(), @"Ошибки!",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
