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
        private readonly StreamWriter _debugWriter;

        public MainForm()
        {
            InitializeComponent();
            try
            {
                var tmp = LessonReader.ReadHtmlViewLesson(@"lessons\lesson1.xml").HtmlString;
                htmlView.DocumentText = tmp;
                _debugWriter = new StreamWriter("DebugHelper");
                _debugWriter.Write(tmp);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                htmlView.DocumentText = "";
            }

            splitContainer1.Panel1MinSize = Math.Min(200, Size.Width / 5);
            splitContainer1.Panel2MinSize = Size.Width  / 5 * 4;

            Closed += (sender, args) => _debugWriter?.Close();
        }
    }
}
