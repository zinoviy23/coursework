using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LessonLibrary;
using System.IO;

namespace WinFormCourseWork
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Файл для отладки
        /// </summary>
        private readonly StreamWriter _debugWriter;

        public MainForm()
        {
            InitializeComponent();
            _debugWriter = new StreamWriter("DebugHelper");
            LoadLesson("title_page.xml");
            splitContainer1.Panel1MinSize = Math.Min(200, Size.Width / 5);
            splitContainer1.Panel2MinSize = Width - splitContainer1.Panel1MinSize - 30;

            Closed += (sender, args) => _debugWriter?.Close();
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
            }
            else
            {
                _tmp = new Tmp();
                _tmp.Show();
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
    }
}
