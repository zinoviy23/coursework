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
            try
            {
                var tmp = LessonReader.ReadHtmlViewLesson(@"lessons\test_lesson1.xml");
                tmp.HtmlView = htmlView;
                htmlView.DocumentText = tmp.HtmlString;
                _debugWriter = new StreamWriter("DebugHelper");
                _debugWriter.Write(htmlView.DocumentText);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                htmlView.DocumentText = "";
            }

            splitContainer1.Panel1MinSize = Math.Min(200, Size.Width / 5);
            splitContainer1.Panel2MinSize = Width - splitContainer1.Panel1MinSize - 30;

            Closed += (sender, args) => _debugWriter?.Close();
        }
    }
}
