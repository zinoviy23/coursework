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

            Closed += (sender, args) => _debugWriter?.Close();
            glControl1.Load += glControl1_Load;
            glControl1.Paint +=  glControl1_Paint;
            glControl1.Visible = false;
            
            //TODO: убрать это потом
            for (var i = 1; i <= 3; i++)
            {
                cayleyTableGridView.Columns.Add(i.ToString(), i.ToString());
            }

            for (var i = 1; i <= 3; i++)
            {
                var values = new List<string>();
                for (var j = 1; j <= 3; j++)
                {
                    values.Add((i * j).ToString());
                }

                cayleyTableGridView.Rows.Add(Array.ConvertAll(values.ToArray(), input => (object)input));
                cayleyTableGridView.Rows[cayleyTableGridView.RowCount - 1].HeaderCell.Value = i.ToString();
            }

            cayleyTableGridView.Visible = false;

            SetElementsSizesAndPositions();
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
            _currentTest = null;
            if ((string) node.Tag == "Cube")
            {
                htmlView.Hide();
                glControl1.Visible = true;
                _currentTest = null;
                checkTestButton.Enabled = false;
                checkTestButton.Visible = false;

                cayleyTableGridView.Visible = false;
            }
            else if ((string) node.Tag == "Cayley Table")
            {
                htmlView.Visible = false;
                cayleyTableGridView.Visible = true;
                glControl1.Visible = false;
                checkTestButton.Enabled = true;
                checkTestButton.Visible = true;
            }
            else
            {
                cayleyTableGridView.Visible = false;
                LoadLesson((string) node.Tag);
                htmlView.Show();
                glControl1.Visible = false;
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
            if (_currentTest != null)
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

        /// <summary>
        /// Задаёт размеры и положения всем элементам
        /// </summary>
        private void SetElementsSizesAndPositions()
        {
            treeView1.Size = new Size(Math.Min(200, Size.Width / 7) ,treeView1.Height);
            htmlView.Size = new Size(Width - htmlView.Margin.Left - treeView1.Margin.Right - treeView1.Size.Width - 15,
                htmlView.Height);
            glControl1.Location = new Point(treeView1.Location.X + treeView1.Width + 1, 1);
            glControl1.Size = new Size(Width - htmlView.Margin.Left - treeView1.Margin.Right - treeView1.Size.Width - 15, Height);

            cayleyTableGridView.Location = new Point(treeView1.Location.X + treeView1.Width + 1, 1);
            cayleyTableGridView.Size = new Size(Width - htmlView.Margin.Left - treeView1.Margin.Right - treeView1.Size.Width - 15, Height / 2);

            SetGridViewCellsSize();

            if (loaded)
            {
                GL.Viewport(glControl1.Location, glControl1.Size);
            }
        }

        /// <summary>
        /// Задаёт размеры клеткам таблицы
        /// </summary>
        private void SetGridViewCellsSize()
        {
            foreach (DataGridViewColumn column in cayleyTableGridView.Columns)
            {
                column.Width = (cayleyTableGridView.Width - cayleyTableGridView.RowHeadersWidth)
                               / cayleyTableGridView.ColumnCount;
            }

            foreach (DataGridViewRow row in cayleyTableGridView.Rows)
            {
                row.Height = (cayleyTableGridView.Height - cayleyTableGridView.ColumnHeadersHeight)
                             / cayleyTableGridView.RowCount;
            }
        } 

        //TODO: убрать это
        #region Для 3D. Нужно куда-нибудь убрать
        private bool loaded;
        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.SkyBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.ShadeModel(ShadingModel.Smooth);

            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), 1, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(70, 70, 70, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float width = 20;
            if (_ticked)
            {
                GL.Rotate(_angle, 0, 1, 0);
                _ticked = false;
            }

            //MessageBox.Show(_angle.ToString());
            /*задняя*/
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*левая*/
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*нижняя*/
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, 0, 0);
            GL.End();

            /*верхняя*/
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(0, width, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            /*передняя*/
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();

            /*правая*/
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            /*ребра*/
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(width, 0, 0);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();

            glControl1.SwapBuffers();
        }

        private float _angle = 10;
        private bool _ticked = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            _ticked = true;
            glControl1.Refresh();
        }
        #endregion

        private void MainForm_Resize(object sender, EventArgs e)
        {
            SetElementsSizesAndPositions();
        }
    }
}
