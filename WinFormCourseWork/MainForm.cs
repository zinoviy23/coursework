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
using OpenTK.Graphics;
using WinFormCourseWork.Users;
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
        /// отображение 3Д
        /// </summary>
        private readonly GLControl _glControl;

        /// <summary>
        /// Текущий тест
        /// </summary>
        private TestLesson _currentTest;

        /// <summary>
        /// Текущая таблица
        /// </summary>
        private CayleyTableTestLesson _currentTable;

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
        /// Объект для управления визуализациями
        /// </summary>
        private readonly Visualisation3DController _visualisationController;

        /// <summary>
        /// Текущая вершина
        /// </summary>
        private TreeNode _currentNode;

        /// <summary>
        /// Предыдущая вершина
        /// </summary>
        private TreeNode _prevNode;

        /// <summary>
        /// Тэг, который сейчас
        /// </summary>
        private string _currentTag;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <inheritdoc cref="Form"/>
        public MainForm()
        {
            InitializeComponent();

            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;

            _glControl = new GLControl(new GraphicsMode(new ColorFormat(32), 24, 0, 8));
            _glControl.BringToFront();
            _glControl.Enabled = true;
            _glControl.MouseDown += GlControl1_MouseDown;
            _glControl.MouseMove += GlControl1_MouseMove;
            _glControl.MouseUp += GlControl1_MouseUp;
            _glControl.KeyUp += GlControl1_KeyDown;
            _glControl.BorderStyle = BorderStyle.Fixed3D;
            
            
            Controls.Add(_glControl);

            LoadLesson("title_page.xml");

            _tablesFolder = new DirectoryInfo(MainFormPathes.TablesFolderPath);

            _rand = new Random();

            InitVisualisationsDictionary();

            Closing += (sender, args) =>
            {
                MainFormSettingsLoader.WriteSettings();
                MainFormSettingsLoader.SaveUsersTables();
                MainFormSettingsLoader.SaveCurrentUser();
            };

            _glControl.Visible = false;
            cayleyTableGridView.Visible = false;

            _visualisationController = new Visualisation3DController(_glControl, this);
            try
            {
                LessonReader.ReadLessonsTreeInfo(lessonsTreeView, MainFormPathes.LessonsTreeInfoPath);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($@"Ошибка при загрузке уроков! Попробуйте переустановить программу. {'\n'}" + ex.Message);
            }

            lessonsTreeView.Nodes[0].Expand();
            


            SetUiView();  
            EnableButtons(null);
        }

        /// <summary>
        /// Выход ли из пользователя
        /// </summary>
        public bool IsUserExit { get; private set; }

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
            _currentNode = node;

            EnableButtons(node);

            if (_prevNode != null)
            {
                _prevNode.BackColor = lessonsTreeView.BackColor;
            }

            node.BackColor = SystemColors.Highlight;

            _prevNode = node;

            _currentTag = (string) node.Tag;

            if (((string) node.Tag).StartsWith("Visualisation"))
            {
                //htmlView.Hide();
                htmlView.Show();
                PermutationVisualisation.Release();
                PermutationCalculator.Release();
                _glControl.Visible = true;
                _currentTest = null;
                checkTestToolStripMenuItem.Enabled = false;
                checkButton.Visible = false;
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

                LessonReader.ReadPageTemplate(htmlView, MainFormPathes.Visualisation3DMarkupFilePath);

                _visualisationController.IsAnimatingSessionStarted = false;
                _visualisationController.SetButtons(htmlView);

                _uiState = UiState.Visualisation3D;
            }
            else switch ((string) node.Tag)
            {
                case "Cayley Table":
                    PermutationVisualisation.Release();
                    PermutationCalculator.Release();
                    LoadTable();
                    htmlView.Visible = false;
                    cayleyTableGridView.Visible = true;
                    _glControl.Visible = false;
                    checkTestToolStripMenuItem.Enabled = true;
                    checkButton.Visible = true;
                    _visualisationController.HideVertexLabelsAndAxis();

                    _uiState = UiState.CayleyTable;
                    break;
                case "Permutation Visualisation":
                    PermutationCalculator.Release();
                    htmlView.Visible = true;
                    cayleyTableGridView.Visible = false;
                    _glControl.Visible = false;
                    checkTestToolStripMenuItem.Enabled = false;
                    checkButton.Visible = false;
                    _visualisationController.HideVertexLabelsAndAxis();
                    try
                    {
                        LessonReader.ReadPageTemplate(htmlView, MainFormPathes.PermutationVisualisationFilePath);
                        PermutationVisualisation.CreateInstance(htmlView);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    _uiState = UiState.SimpleHtml;
                    break;
                case "Permutation Calculator":
                    PermutationVisualisation.Release();
                    htmlView.Visible = true;
                    cayleyTableGridView.Visible = false;
                    _glControl.Visible = false;
                    checkTestToolStripMenuItem.Enabled = false;
                    checkButton.Visible = false;
                    _visualisationController.HideVertexLabelsAndAxis();
                    try
                    {
                        LessonReader.ReadPageTemplate(htmlView, MainFormPathes.PermutationCalculatorFilePath);
                        PermutationCalculator.Create(htmlView);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    _uiState = UiState.SimpleHtml;

                    break;
                default:
                    PermutationVisualisation.Release();
                    PermutationCalculator.Release();
                    cayleyTableGridView.Visible = false;
                    LoadLesson((string) node.Tag);
                    htmlView.Show();
                    _glControl.Visible = false;
                    _visualisationController.HideVertexLabelsAndAxis();

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
                    checkButton.Visible = true;
                }
                else
                {
                    _currentTest = null;
                    checkTestToolStripMenuItem.Enabled = false;
                    checkButton.Visible = false;
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
            //Log.WriteLine(htmlView.DocumentText);

            if (_currentTest != null && Settings.CurrentUser.Tests.ContainsKey(_currentTag))
            {
                _currentTest.SetEnteredAnswers(Settings.CurrentUser.Tests[_currentTag].Answers);
            }

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
            cayleyTableGridView.EnableHeadersVisualStyles = false;
            cayleyTableGridView.Font = new Font(cayleyTableGridView.Font, FontStyle.Bold);

            for (var column = 1; column < values.GetLength(1); column++)
            {
                cayleyTableGridView.Columns.Add(values[0, column], values[0, column]);
                cayleyTableGridView.Columns[column - 1].HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }

            var colors = new Dictionary<string, Color>();
            for (var i = 1; i < values.GetLength(1); i++)
            {
                if (i < PermutationVisualisation.Colors.Length)
                {
                    colors[values[0, i]] = PermutationVisualisation.Colors[i];
                }
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

                
                foreach (DataGridViewCell cell in cayleyTableGridView.Rows[cayleyTableGridView.RowCount - 1].Cells)
                {
                    
                    if (cell.Value != null)
                    {
                        var cellStyle = new DataGridViewCellStyle { ForeColor = colors[(string)cell.Value]};
                        cell.ReadOnly = true;
                        cell.Style = cellStyle;
                    }

                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                var headerCell = cayleyTableGridView.Rows[cayleyTableGridView.RowCount - 1].HeaderCell;
                headerCell.Style.ForeColor = colors[(string) headerCell.Value];
            }

            foreach (DataGridViewColumn column in cayleyTableGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.HeaderCell.Style.ForeColor = colors[(string) column.HeaderCell.Value];
            }

            SetGridViewCellsSize();
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
        private void CheckTestButtonOnClick(object sender, EventArgs e)
        {
            if (_currentTest != null)
            {
                var mistakes = _currentTest.CheckAnswers();

                // добавление ответов к пользователю
                for (var i = 0; i < _currentTest.Questions.Count; i++)
                {
                    if (!mistakes.Contains(i))
                    {
                        Settings.CurrentUser.AddAnswer(_currentTag, i, _currentTest.Questions[i].Answer);
                    }
                }

                if (mistakes == null)
                {
                    return;
                }

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
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(MainFormPathes.DefultFilesPath + @"\Tetrahedron"));

            _visualisationLessons["Cube"] = new CubeVisualisation();
            _visualisationLessons["Cube"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(MainFormPathes.DefultFilesPath + @"\Cube"));

            _visualisationLessons["Octahedron"] = new OctahedronVisualisation();
            _visualisationLessons["Octahedron"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(MainFormPathes.DefultFilesPath + @"\Octahedron"));

            _visualisationLessons["Icosahedron"] = new IcosahedronVisualisation();
            _visualisationLessons["Icosahedron"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(MainFormPathes.DefultFilesPath + @"\Icosahedron"));

            _visualisationLessons["Dodecahedron"] = new DodecahedronVisualisation();
            _visualisationLessons["Dodecahedron"]
                .SetAnimations(LessonReader.ReadAnimationsFromFolder(MainFormPathes.DefultFilesPath + @"\Dodecahedron"));

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

            buttonsPanel.Size = new Size(ClientSize.Width - lessonsTreeView.Width, buttonsPanel.Height);
            buttonsPanel.Left = lessonsTreeView.Right;
            buttonsPanel.Top = ClientSize.Height - buttonsPanel.Height;

            checkButton.Location = new Point(buttonsPanel.Width / 2 - checkButton.Width / 2,
                buttonsPanel.Height / 2 - checkButton.Height / 2);

            cayleyInfoTableLabel.Visible = false;

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
            htmlView.Size = new Size(Math.Max(250, (ClientSize.Width - lessonsTreeView.Width) / 2),
                ClientSize.Height - menuStrip.Height - buttonsPanel.Height);
            htmlView.Top = menuStrip.Height + 1;
            htmlView.Left = ClientSize.Width - htmlView.Width - 10;

            _glControl.Size =
                new Size(Width - htmlView.Width - lessonsTreeView.Size.Width - 15,
                    ClientSize.Height - buttonsPanel.Height);
            _glControl.Location = new Point(lessonsTreeView.Right + 1, menuStrip.Height + 1);

            _visualisationController?.UpdateGlSettings();
            _visualisationController?.UpdateVerticesIndexies();
        }

        /// <summary>
        /// Располагает элементы при таблице Кэли
        /// </summary>
        private void SetCayleyTableUiView()
        {
            const int tableSize = 500;

            cayleyInfoTableLabel.Visible = true;
            cayleyInfoTableLabel.AutoSize = true;
            cayleyInfoTableLabel.MaximumSize = new Size(ClientSize.Width - lessonsTreeView.Width - 10,
                0);

            var tmpSize = new Size(
                Width - htmlView.Margin.Left - lessonsTreeView.Margin.Right - lessonsTreeView.Size.Width - 15,
                (ClientSize.Height - menuStrip.Height - buttonsPanel.Height - cayleyInfoTableLabel.Height));

            cayleyTableGridView.Size =
                new Size(Math.Min(tmpSize.Width, tableSize),
                    Math.Min(tmpSize.Height, tableSize));

            cayleyTableGridView.Location =
                new Point(
                    lessonsTreeView.Right + (ClientSize.Width - lessonsTreeView.Width - cayleyTableGridView.Width) / 2,
                    menuStrip.Bottom + 1);
            

            cayleyInfoTableLabel.Top = cayleyTableGridView.Bottom + 1;
            cayleyInfoTableLabel.Left = lessonsTreeView.Right + 1;
            
            SetGridViewCellsSize();
        }

        /// <summary>
        /// Располагает элементы при простом html
        /// </summary>
        private void SetSimpleHtmlUiView()
        {
            htmlView.Size =
                new Size(Width - htmlView.Margin.Left - lessonsTreeView.Margin.Right - lessonsTreeView.Size.Width - 15,
                    ClientSize.Height - buttonsPanel.Height - menuStrip.Height);
            htmlView.Location = new Point(lessonsTreeView.Right + 1, menuStrip.Height + 1);
        }

        /// <summary>
        /// Задаёт размеры клеткам таблицы
        /// </summary>
        private void SetGridViewCellsSize()
        {
            foreach (DataGridViewColumn column in cayleyTableGridView.Columns)
            {
                column.Width = (cayleyTableGridView.Width - cayleyTableGridView.RowHeadersWidth - 5)
                               / cayleyTableGridView.ColumnCount;
            }

            foreach (DataGridViewRow row in cayleyTableGridView.Rows)
            {
                row.Height = (cayleyTableGridView.Height - cayleyTableGridView.ColumnHeadersHeight - 5)
                             / cayleyTableGridView.RowCount;
            }
        }

        //TODO: убрать это

        #region Для 3D. Нужно куда-нибудь убрать

        private float _deltaTime;
        private float _prevTime;
        private bool _timeCountingStarted;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Обработчик тика таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (_visualisationController.GlContolLoaded && _glControl.Visible)
            {
                if (_visualisationController.IsPlayingAnimation)
                    _visualisationController.CurrentVisualisation?.CurrentAnimation?.NextStep(_deltaTime);
                UpdateGl();

            }

            _glControl.Refresh();

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
 
        /// <summary>
        /// Обоаботчик изменения размера окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Обработчик клика мыщкой на glcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Обработчик подъема кнопки мышки на glcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Обработчик движения мышки на glcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isRotatingY || _isRotatingX) _delta = new Point(e.X - _clickPoint.X, e.Y - _clickPoint.Y);
        }

        private void GlControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Space || !_visualisationController.GlContolLoaded) return;
            ResetView();
        }

        /// <summary>
        /// Сбрасывает вид
        /// </summary>
        private void ResetView()
        {
            _userPosition = new Vector3(0, 0, 4);
            _userUp = new Vector3(0, 1, 0);
            var modelview = Matrix4.LookAt(_userPosition.X, _userPosition.Y, _userPosition.Z, 0, 0, 0, _userUp.X,
                _userUp.Y, _userUp.Z);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            WorldInfo.ViewMatrix = modelview;
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
            if (e.KeyCode == Keys.Space && _visualisationController.GlContolLoaded && _glControl.Visible)
                ResetView();

        }

        /// <summary>
        /// Обработчик события нажатия на кнопку вперёд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButtonOnClick(object sender, EventArgs e)
        {
            lessonsTreeView.SelectedNode = MainFormUtils.NextNode(_currentNode);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButtonOnClick(object sender, EventArgs e)
        {
            lessonsTreeView.SelectedNode = MainFormUtils.PreviousNode(_currentNode);
        }

        /// <summary>
        /// Разрешает или запрещает кнопки впред, назад
        /// </summary>
        /// <param name="node"></param>
        private void EnableButtons(TreeNode node)
        {
            backButton.Enabled = nextButton.Enabled = false;
            if (node == null) return;

            if (MainFormUtils.NextNode(node) != node)
                nextButton.Enabled = true;

            if (MainFormUtils.PreviousNode(node) != node)
                backButton.Enabled = true;

        }

        /// <summary>
        /// Обработчик события загрузки окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFormOnLoad(object sender, EventArgs e)
        {
            MainFormSettingsLoader.LoadSettings();
            MainFormSettingsLoader.LoadUsersTables();
            if (Settings.CurrentUserName == null)
            {
                var userDialog = new UserForm();
                var result = userDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Settings.CurrentUserName = userDialog.UserName;
                }
                else
                {
                    Close();
                }
            }

            if (!UsersTables.UserContains(Settings.CurrentUserName))
            {
                UsersTables.AddUser(Settings.CurrentUserName);
            }

            MainFormSettingsLoader.LoadUser(MainFormPathes.UserFolderPath +
                                            UsersTables.GetUserFileName(Settings.CurrentUserName));
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку выхода из пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitUserToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            Settings.CurrentUserName = null;
            IsUserExit = true;
            Hide();
            Close();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку информация о пользователе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoUserToolStripMenuItemOnClick(object sender, EventArgs e) => new UserInfoForm().Show();
    }
}
