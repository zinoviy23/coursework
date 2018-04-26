using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LessonLibrary;
using System.IO;
using LessonLibrary.Visualisation3D;
using OpenTK;
using GL = OpenTK.Graphics.OpenGL.GL;
using MatrixMode = OpenTK.Graphics.OpenGL.MatrixMode;

namespace WinFormCourseWork
{
    /// <inheritdoc />
    /// <summary>
    /// Главное окно приложения
    /// </summary>
    public partial class MainForm : Form
    {
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
        private readonly Dictionary<string, VisualisationLesson> _visualisationLessons =
            new Dictionary<string, VisualisationLesson>();

        /// <summary>
        /// Путь до файла с деревом уроков
        /// </summary>
        private const string LessonsTreeInfoPath = @"lessons\lessonstree.xml";

        /// <summary>
        /// Путь до шаблона с визуализациями подстановок
        /// </summary>
        private const string PermulationVisualisationFilePath = @"lessons\default\permulation_visualisation.xml";

        /// <summary>
        /// путь до папки со стандартными файлами
        /// </summary>
        private const string DefultFilesPath = @"lessons\default";

        /// <summary>
        /// путь до файла с шаблоном разметки для визуализации 3d
        /// </summary>
        private const string Visualisation3DMarkupFilePath = @"lessons\default\visualisation_3d.xml";

        /// <summary>
        /// Объект для управления визуализациями
        /// </summary>
        private readonly Visualisation3DController _visualisationController;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <inheritdoc cref="Form"/>
        public MainForm()
        {
            InitializeComponent();

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

            Closed += (sender, args) => Log.Close();

            glControl.Visible = false;
            cayleyTableGridView.Visible = false;

            _visualisationController = new Visualisation3DController(glControl, this);

            LessonReader.ReadLessonsTreeInfo(lessonsTreeView, LessonsTreeInfoPath);
            lessonsTreeView.Nodes[0].Expand();

            SetUiView();  
        }

        /// <summary>
        /// Обработчик нажатия на элемент уроков
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Параметры</param>
        private void LessonsView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            if (node.Tag == null) return;
            _currentTest = null;
            _currentTable = null;


            if (((string) node.Tag).StartsWith("Visualisation"))
            {
                //htmlView.Hide();
                PermulationVisualisation.Release();
                glControl.Visible = true;
                _currentTest = null;
                checkTestToolStripMenuItem.Enabled = false;
                cayleyTableGridView.Visible = false;

                var visualisationType = ((string) node.Tag).Substring("Visualisation".Length);
                _visualisationController.CurrentVisualisation =
                    _visualisationLessons[visualisationType];
                if (visualisationType == "Polygon")
                {
                    var sizeInput = new PolygonSizeInput();
                    sizeInput.ShowDialog();
                    ((PolygonVisualisation) _visualisationController.CurrentVisualisation).VerticesCount =
                        sizeInput.PolygonSize;
                }

                //TODO: удалить это
                var tmp = new StreamReader(Visualisation3DMarkupFilePath);
                htmlView.DocumentText = tmp.ReadToEnd();
                tmp.Close();

                _visualisationController.IsAnimatingSessionStarted = false;
                _visualisationController.SetButtons(htmlView);

                _uiState = UiState.Visualisation3D;
            }
            else switch ((string) node.Tag)
            {
                case "Cayley Table":
                    PermulationVisualisation.Release();
                    LoadTable();
                    htmlView.Visible = false;
                    cayleyTableGridView.Visible = true;
                    glControl.Visible = false;
                    checkTestToolStripMenuItem.Enabled = true;
                    _visualisationController.HideVertexLabels();

                    _uiState = UiState.CayleyTable;
                    break;
                case "Permulation Visualisation":
                    htmlView.Visible = true;
                    cayleyTableGridView.Visible = false;
                    glControl.Visible = false;
                    checkTestToolStripMenuItem.Enabled = false;
                    _visualisationController.HideVertexLabels();
                    try
                    {
                        LessonReader.ReadPermulationVisualisationTemplate(htmlView, PermulationVisualisationFilePath);
                        PermulationVisualisation.CreateInstance(htmlView);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    _uiState = UiState.SimpleHtml;
                    break;
                default:
                    PermulationVisualisation.Release();
                    cayleyTableGridView.Visible = false;
                    LoadLesson((string) node.Tag);
                    htmlView.Show();
                    glControl.Visible = false;
                    _visualisationController.HideVertexLabels();

                    _uiState = UiState.SimpleHtml;
                    break;
            }
            SetUiView();
        }

        /// <summary>
        /// Урок, который сейчас загружается
        /// </summary>
        private HtmlViewLesson _currentLoadingLesson;

        /// <summary>
        /// Отображает урок
        /// </summary>
        /// <param name="fileName">Файл урока</param>
        private void LoadLesson(string fileName)
        {
            try
            {
                _currentLoadingLesson = LessonReader.ReadHtmlViewLesson(@"lessons\" + fileName);
                htmlView.DocumentText = _currentLoadingLesson.HtmlString;
                if (fileName.StartsWith("test"))
                {
                    _currentTest = _currentLoadingLesson as TestLesson;
                    checkTestToolStripMenuItem.Enabled = true;
                }
                else
                {
                    _currentTest = null;
                    checkTestToolStripMenuItem.Enabled = false;
                }

                htmlView.DocumentCompleted += HtmlViewOnLoadHandlerBySimpleHtmlLesson;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                htmlView.DocumentText = "";
            }
        }

        /// <summary>
        /// Обрабатывает страницу html через объект урока и удалаят этот обработчик из события DocumentCompleted
        /// </summary>
        /// <param name="sender">объект</param>
        /// <param name="args">Параметры события</param>
        private void HtmlViewOnLoadHandlerBySimpleHtmlLesson(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            _currentLoadingLesson.HtmlView = htmlView;
            Log.WriteLine(htmlView.DocumentText);
            htmlView.DocumentCompleted -= HtmlViewOnLoadHandlerBySimpleHtmlLesson;
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
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Задаёт значения для словоря с объектами визуализаций
        /// </summary>
        private void InitVisualisationsDictionary()
        {
            _visualisationLessons["Tetrahedron"] = new TetrahedronVisualisation();
            _visualisationLessons["Tetrahedron"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(DefultFilesPath + @"\Tetrahedron"));

            _visualisationLessons["Cube"] = new CubeVisualisation();
            _visualisationLessons["Cube"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(DefultFilesPath + @"\Cube"));

            _visualisationLessons["Octahedron"] = new OctahedronVisualisation();
            _visualisationLessons["Octahedron"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(DefultFilesPath + @"\Octahedron"));

            _visualisationLessons["Icosahedron"] = new IcosahedronVisualisation();
            _visualisationLessons["Icosahedron"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(DefultFilesPath + @"\Icosahedron"));

            _visualisationLessons["Dodecahedron"] = new DodecahedronVisualisation();
            _visualisationLessons["Dodecahedron"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(DefultFilesPath + @"\Dodecahedron"));

            _visualisationLessons["Polygon"] = new PolygonVisualisation();
        }

        /// <summary>
        /// Состояние интерфейса
        /// </summary>
        private UiState _uiState = UiState.SimpleHtml;

        /// <summary>
        /// Задаёт нужные элементы и их расположение
        /// </summary>
        private void SetUiView()
        {
            lessonsTreeView.Size = new Size(Math.Min(200, Size.Width / 5), lessonsTreeView.Height);

            switch (_uiState)
            {
                case UiState.Visualisation3D:
                    SetVisualisation3DUiView();
                    return;
                case UiState.SimpleHtml:
                    SetSimpleHtmlUiView();
                    return;
                case UiState.CayleyTable:
                    SetCayleyTableUiView();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }  
        }

        /// <summary>
        /// Располагает элементы при визуализации
        /// </summary>
        private void SetVisualisation3DUiView()
        {
            htmlView.Size = new Size(Width - htmlView.Margin.Left - lessonsTreeView.Margin.Right - lessonsTreeView.Size.Width - 15,
                Math.Max(200, ClientSize.Height / 3));
            htmlView.Top = ClientSize.Height - htmlView.Height;
            htmlView.Left = lessonsTreeView.Right;

            glControl.Size =
                new Size(Width - htmlView.Margin.Left - lessonsTreeView.Margin.Right - lessonsTreeView.Size.Width - 15,
                    ClientSize.Height - htmlView.Height - 5);
            glControl.Location = new Point(lessonsTreeView.Right + 1, 1);

            _visualisationController?.UpdateGlSettings();
            _visualisationController?.UpdateVerticesIndexies();
        }

        /// <summary>
        /// Располагает элементы при таблице Кэли
        /// </summary>
        private void SetCayleyTableUiView()
        {
            cayleyTableGridView.Location = new Point(lessonsTreeView.Location.X + lessonsTreeView.Width + 1, 1);
            cayleyTableGridView.Size =
                new Size(Width - htmlView.Margin.Left - lessonsTreeView.Margin.Right - lessonsTreeView.Size.Width - 15,
                    Height / 2);
            SetGridViewCellsSize();
        }

        /// <summary>
        /// Располагает элементы при простом html
        /// </summary>
        private void SetSimpleHtmlUiView()
        {
            htmlView.Size = new Size(Width - htmlView.Margin.Left - lessonsTreeView.Margin.Right - lessonsTreeView.Size.Width - 15,
                ClientSize.Height);
            htmlView.Location = new Point(lessonsTreeView.Right + 1, 1);
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

        private float _deltaTime;
        private float _prevTime;
        private bool _timeCountingStarted;
        private readonly Stopwatch _stopwatch = new Stopwatch();


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_timeCountingStarted)
            {
                _timeCountingStarted = true;
                _stopwatch.Start();
                return;
            }
            _stopwatch.Stop();

            var currentTime = _stopwatch.ElapsedMilliseconds;
            _deltaTime = currentTime - _prevTime;
            _deltaTime /= 1000;
            _prevTime = currentTime;

            _stopwatch.Start();
            if (_visualisationController == null)
                return;

            if (_visualisationController.GlContolLoaded && glControl.Visible)
            {
                if (_visualisationController.IsPlayingAnimation)
                    _visualisationController.CurrentVisualisation?.CurrentAnimation?.NextStep(_deltaTime);
                UpdateGl();

            }

            glControl.Refresh();

        }

        #endregion

        /// <summary>
        /// Обновление сцены, происходит раз в тик
        /// </summary>
        private void UpdateGl()
        {
            if (_isRotatingY)
            {
                var rotationY =  Matrix4.CreateFromAxisAngle(_userUp, -(float) _delta.X / 2000);

                _userPosition = Vector3.Transform(_userPosition, rotationY);
                _userUp = Vector3.Transform(_userUp, rotationY);

                var modelview = Matrix4.LookAt(_userPosition.X, _userPosition.Y, _userPosition.Z, 0, 0, 0, _userUp.X,
                    _userUp.Y, _userUp.Z);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
                WorldInfo.ViewMatrix = modelview;
            }
            else if (_isRotatingX)
            {
                var axis = -Vector3.Cross(_userPosition, _userUp);
                var rotationX = Matrix4.CreateFromAxisAngle(axis, -(float) _delta.Y / 2000);


                _userPosition = Vector3.Transform(_userPosition, rotationX);
                _userUp = Vector3.Transform(_userUp, rotationX);

                var modelview = Matrix4.LookAt(_userPosition.X, _userPosition.Y, _userPosition.Z, 0, 0, 0, _userUp.X,
                    _userUp.Y, _userUp.Z);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
                WorldInfo.ViewMatrix = modelview;
            }

            _visualisationController.UpdateInfo();
            _visualisationController.CheckHtmlButtons(htmlView);
            _visualisationController.UpdateVerticesIndexies();

            //_currentVisualisation.Transform.Rotate(new Vector3(1f, 1f, 1f));
        }
 
        private void MainForm_Resize(object sender, EventArgs e)
        {
            SetUiView();
        }

        private bool _isRotatingY;
        private bool _isRotatingX;
        private Point _clickPoint;
        private Point _delta;
        private Vector3 _userPosition = new Vector3(0, 0, 4);
        private Vector3 _userUp = new Vector3(0, 1, 0);

        private void GlControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_isRotatingX || _isRotatingY)
                return;

            switch (e.Button)
            {
                case MouseButtons.Right:
                    _isRotatingY = true;
                    break;
                case MouseButtons.Left:
                    _isRotatingX = true;
                    break;
                default:
                    return;
            }

            _clickPoint = new Point(e.X, e.Y);
            _delta = new Point();
        }

        private void GlControl1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    _isRotatingY = false;
                    break;
                case MouseButtons.Left:
                    _isRotatingX = false;
                    break;
            }
        }

        private void GlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isRotatingY || _isRotatingX) _delta = new Point(e.X - _clickPoint.X, e.Y - _clickPoint.Y);
        }

        private void GlControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Space || !_visualisationController.GlContolLoaded) return;
            _userPosition = new Vector3(0, 0, 4);
            _userUp = new Vector3(0, 1, 0);
            var modelview = Matrix4.LookAt(_userPosition.X, _userPosition.Y, _userPosition.Z, 0, 0, 0, _userUp.X,
                _userUp.Y, _userUp.Z);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            WorldInfo.ViewMatrix = modelview;
        }

        private void GlControl1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Запрещает нажимать на F5 в браузере.
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Информация об клавише</param>
        private void HtmlView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                e.IsInputKey = true;
        }

        private void PlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_visualisationController.IsPlayingAnimation)
                _visualisationController.IsPlayingAnimation = true;
            _visualisationController.IsAnimatingSessionStarted = true;
        }
    }
}
