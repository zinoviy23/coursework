using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LessonLibrary;
using System.IO;
using OpenTK;
using ClearBufferMask = OpenTK.Graphics.OpenGL.ClearBufferMask;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using GL = OpenTK.Graphics.OpenGL.GL;
using LightName = OpenTK.Graphics.OpenGL.LightName;
using LightParameter = OpenTK.Graphics.OpenGL.LightParameter;
using MatrixMode = OpenTK.Graphics.OpenGL.MatrixMode;
using ShadingModel = OpenTK.Graphics.OpenGL.ShadingModel;

namespace WinFormCourseWork
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Файл для отладки
        /// </summary>
        private readonly StreamWriter _debugWriter;

        /// <summary>
        /// Текущий тест
        /// </summary>
        private TestLesson _currentTest;

        /// <summary>
        /// Текущая таблица
        /// </summary>
        private CayleyTableTestLesson _currentTable;

        /// <summary>
        /// Временный путь до таблицы
        /// </summary>
        private const string TablesFolderPath = @"lessons\CayleyTables";

        /// <summary>
        /// Папка с таблицами Кэли
        /// </summary>
        private readonly DirectoryInfo _tablesFolder;

        /// <summary>
        /// Рандом
        /// </summary>
        private readonly Random _rand;

        /// <summary>
        /// Словарь с визуализациями
        /// </summary>
        private Dictionary<string, VisualisationLesson> _visualisationLessons =
            new Dictionary<string, VisualisationLesson>();

        private VisualisationLesson _currentVisualisation;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <inheritdoc cref="Form"/>
        public MainForm()
        {
            InitializeComponent();
            _debugWriter = new StreamWriter("DebugHelper");
            LoadLesson("title_page.xml");

            _tablesFolder = new DirectoryInfo(TablesFolderPath);

            _rand = new Random();

            InitGrid(new[,]
            {
                {"\\", "1", "2", "3"},
                {"1", "1", "*", "3"},
                {"2", "2", "3", "6"},
                {"3", "3", "6", "*"},
            });

            InitVisualisationsDictionary();

            Closed += (sender, args) => _debugWriter?.Close();
            glControl1.Load += glControl1_Load;
            glControl1.Paint +=  glControl1_Paint;

            glControl1.Visible = false;
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
            _currentTable = null;

            if (((string) node.Tag).StartsWith("Visualisation"))
            {
                htmlView.Hide();
                glControl1.Visible = true;
                _currentTest = null;
                checkTestToolStripMenuItem.Enabled = false;
                cayleyTableGridView.Visible = false;

                _currentVisualisation = _visualisationLessons[((string) node.Tag).Substring("Visualisation".Length)];
            }
            else if ((string) node.Tag == "Cayley Table")
            {
                LoadTable();
                htmlView.Visible = false;
                cayleyTableGridView.Visible = true;
                glControl1.Visible = false;
                checkTestToolStripMenuItem.Enabled = true;
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
                    checkTestToolStripMenuItem.Enabled = true;
                }
                else
                {
                    _currentTest = null;
                    checkTestToolStripMenuItem.Enabled = false;
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
        /// Загружает таблицу
        /// </summary>
        private void LoadTable()
        {
            cayleyTableGridView.Columns.Clear();
            try
            {
                var files = _tablesFolder.GetFiles();

                _currentTable = LessonReader.ReadCayleyTableTestLesson(files[_rand.Next(0, files.Length)].FullName);
                InitGrid(_currentTable.StartTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Задаёт значение DataGridView
        /// </summary>
        /// <param name="values">Значения</param>
        private void InitGrid(string[,] values)
        {
            for (var column = 1; column < values.GetLength(1); column++)
            {
                cayleyTableGridView.Columns.Add(values[0, column], values[0, column]);
                cayleyTableGridView.Columns[column - 1].HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }

            for (var row = 1; row < values.GetLength(0); row++)
            {
                var line = new object[values.GetLength(1) - 1];
                for (var elementIndex = 1; elementIndex < values.GetLength(1); elementIndex++)
                {
                    if (values[row, elementIndex] != "*")
                        line[elementIndex - 1] = values[row, elementIndex];
                }

                cayleyTableGridView.Rows.Add(line);
                cayleyTableGridView.Rows[cayleyTableGridView.RowCount - 1].HeaderCell.Value = values[row, 0];
                cayleyTableGridView.Rows[cayleyTableGridView.RowCount - 1].HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                var cellStyle = new DataGridViewCellStyle {ForeColor = Color.DarkGreen};
                foreach (DataGridViewCell cell in cayleyTableGridView.Rows[cayleyTableGridView.RowCount - 1].Cells)
                {
                    if (cell.Value != null)
                    {
                        cell.ReadOnly = true;
                        cell.Style = cellStyle;
                    }

                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                SetGridViewCellsSize();
            }
        }

        /// <summary>
        /// Получает строковое представление таблицы
        /// </summary>
        /// <returns>Двумерный массив - таблица</returns>
        private string[,] GetValuesFromCayleyTable()
        {
            var res = new string[cayleyTableGridView.RowCount + 1, cayleyTableGridView.ColumnCount + 1];

            for (var columnIndex = 0; columnIndex < cayleyTableGridView.ColumnCount; columnIndex++)
            {
                res[0, columnIndex + 1] = (cayleyTableGridView.Columns[columnIndex].HeaderText ?? "").Trim();
            }

            for (var rowIndex = 0; rowIndex < cayleyTableGridView.RowCount; rowIndex++)
            {
                res[rowIndex + 1, 0] = ((string) (cayleyTableGridView.Rows[rowIndex].HeaderCell.Value ?? "")).Trim();

                for (var elementIndex = 0;
                    elementIndex < cayleyTableGridView.Rows[rowIndex].Cells.Count;
                    elementIndex++)
                {
                    res[rowIndex + 1, elementIndex + 1] =
                        ((string) (cayleyTableGridView.Rows[rowIndex].Cells[elementIndex].Value ?? "")).Trim();
                }
            }

            return res;
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
            else if (_currentTable != null)
            {
                var usersTable = GetValuesFromCayleyTable();
                try
                {
                    var result = CayleyTableTestLesson.CheckTableOnGroup(usersTable);
                    switch (result)
                    {
                        case CayleyTableTestLesson.CheckResult.DontContainsInverts:
                            MessageBox.Show(@"Не для всех элементов есть обратный!", @"Неправильно!",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case CayleyTableTestLesson.CheckResult.Success:
                            MessageBox.Show(@"Это группа!", @"Правильно!", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            break;
                        case CayleyTableTestLesson.CheckResult.NotAssociativity:
                            MessageBox.Show(@"Операция в группе не ассоциативна!", @"Неправильно!",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case CayleyTableTestLesson.CheckResult.DontContainsNeutral:
                            MessageBox.Show(@"В группе нет нейтрального элемента!", @"Неправильно!",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitVisualisationsDictionary()
        {
            _visualisationLessons["Tetrahedron"] = new TetrahedronVisualisation();
            _visualisationLessons["Cube"] = new CubeVisualisation();
            _visualisationLessons["Octahedron"] = new OctahedronVisualisation();
            _visualisationLessons["Icosahedron"] = new IcosahedronVisualisation();
        }

        /// <summary>
        /// Задаёт размеры и положения всем элементам
        /// </summary>
        private void SetElementsSizesAndPositions()
        {
            treeView1.Size = new Size(Math.Min(200, Size.Width / 7) ,treeView1.Height);

            //MessageBox.Show(checkTestButton.Location.ToString());

            htmlView.Size = new Size(Width - htmlView.Margin.Left - treeView1.Margin.Right - treeView1.Size.Width - 15,
                htmlView.Height);

            glControl1.Location = new Point(treeView1.Location.X + treeView1.Width + 1, 1);
            glControl1.Size =
                new Size(Width - htmlView.Margin.Left - treeView1.Margin.Right - treeView1.Size.Width - 15, Height);

            cayleyTableGridView.Location = new Point(treeView1.Location.X + treeView1.Width + 1, 1);
            cayleyTableGridView.Size =
                new Size(Width - htmlView.Margin.Left - treeView1.Margin.Right - treeView1.Size.Width - 15, Height / 2);

            SetGridViewCellsSize();

            if (!loaded) return;

            GL.Viewport(new Point(glControl1.Location.X - treeView1.Width, glControl1.Location.Y),
                glControl1.Size);
            glControl1.Update();

            GL.LoadIdentity();
            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), glControl1.AspectRatio, 0.1f, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(0, 0, 4, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            //glControl1.Refresh();
        }

        /// <summary>
        /// Задаёт размеры клеткам таблицы
        /// </summary>
        private void SetGridViewCellsSize()
        {
            foreach (DataGridViewColumn column in cayleyTableGridView.Columns)
            {
                column.Width = (cayleyTableGridView.Width - cayleyTableGridView.RowHeadersWidth - 10)
                               / cayleyTableGridView.ColumnCount;
            }

            foreach (DataGridViewRow row in cayleyTableGridView.Rows)
            {
                row.Height = (cayleyTableGridView.Height - cayleyTableGridView.ColumnHeadersHeight - 10)
                             / cayleyTableGridView.RowCount;
            }
        } 

        //TODO: убрать это
        #region Для 3D. Нужно куда-нибудь убрать
        private bool loaded;
        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.Viewport(new Point(glControl1.Location.X - treeView1.Width, glControl1.Location.Y), glControl1.Size);

            _currentVisualisation = new OctahedronVisualisation();
            

            GL.ClearColor(Color.DarkGray);
            GL.Enable(EnableCap.DepthTest);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.LineSmooth);

            GL.Light(LightName.Light1, LightParameter.Ambient, new []{0.2f, 0.2f, 0.2f, 1.0f});
            GL.Light(LightName.Light1, LightParameter.Diffuse, new []{1.0f, 1.0f, 1.0f, 1.0f});
            GL.Light(LightName.Light1, LightParameter.Position, new []{0.0f, 3.0f, 0.0f, 1.0f});
            GL.Enable(EnableCap.Light1);
            

            var p = Matrix4.CreatePerspectiveFieldOfView((float) (80 * Math.PI / 180), glControl1.AspectRatio, 0.1f,
                500);

            //GL.Rotate(40, 1, 0, 0);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(0, 0, 4, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;

            //GL.Rotate(10, 0, 1, 0);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _currentVisualisation.Render();


            glControl1.SwapBuffers();
        }

        private float _angle = 1;
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

        private float tmp = 0;
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            tmp -= 0.5f;
            var modelview = Matrix4.LookAt(tmp, 0, 4, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }
    }
}
