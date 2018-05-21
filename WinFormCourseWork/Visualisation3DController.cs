using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using LessonLibrary.Visualisation3D;
using LessonLibrary.Visualisation3D.Animations;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для работы с визуализациями из главного окна
    /// </summary>
    public class Visualisation3DController
    {
        /// <summary>
        /// Элемент для отображения 3D
        /// </summary>
        private readonly GLControl _glControl;

        /// <summary>
        /// Главное окно
        /// </summary>
        private readonly MainForm _mainForm;

        /// <summary>
        /// Массив вершин на экране
        /// </summary>
        private Label[] _vertexLabels;

        /// <summary>
        /// Надпись для оси
        /// </summary>
        private Label _axisLabel;

        /// <summary>
        /// Массив исходных вершин на экране
        /// </summary>
        private Label[] _initVertexLabels;

        /// <summary>
        /// Ссылка на визуализацию
        /// </summary>
        private VisualisationLesson _currentVisualisation;

        /// <summary>
        /// Текущая 3D визуализация
        /// </summary>
        public VisualisationLesson CurrentVisualisation
        {
            get => _currentVisualisation;
            set
            {
                _currentVisualisation = value; 
                _currentVisualisation.Reset();
            }
        }

        /// <summary>
        /// Возвращает, загружен ли GlControl
        /// </summary>
        public bool GlContolLoaded { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="glControl">Элемент для отображения 3D</param>
        /// <param name="mainForm">Главное окно</param>
        public Visualisation3DController([NotNull] GLControl glControl, [NotNull] MainForm mainForm)
        {
            _glControl = glControl;
            _mainForm = mainForm;
            InitVertexLabels(20);
            _glControl.Load += GlControlOnLoad;
            _glControl.Paint += GlControlOnPaint;
            InitAxisLabel();
        }

        /// <summary>
        /// Обработчик события загрузки окна
        /// </summary>
        /// <param name="sender">объект</param>
        /// <param name="args">обработчик события</param>
        /// <remarks>Задаёт различные настройки для OpenGL</remarks>
        private void GlControlOnLoad(object sender, EventArgs args)
        {
            GlContolLoaded = true;
            GL.Viewport(new Point(0 , _glControl.Location.Y), _glControl.Size);

            GL.ClearColor(Color.White);
            
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);

            GL.Light(LightName.Light1, LightParameter.Ambient, new[] { 0.2f, 0.2f, 0.2f, 1.0f });
            GL.Light(LightName.Light1, LightParameter.Diffuse, new[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Light(LightName.Light1, LightParameter.Position, new[] { 0.0f, 3.0f, 0.0f, 1.0f });
            GL.Enable(EnableCap.Light1);

            var p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), _glControl.AspectRatio, 0.1f,
                500);

            //GL.Rotate(40, 1, 0, 0);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);
            WorldInfo.ProjectionMatrix = p;

            Matrix4 modelview = Matrix4.LookAt(0, 0, 4, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            WorldInfo.ViewMatrix = modelview;
        }

        /// <summary>
        /// Обработчик события Paint
        /// </summary>
        /// <param name="sender">объект</param>
        /// <param name="e">аргументы</param>
        /// <remarks>Отрисовывает фигуру, сетку, Label для выршин</remarks>
        private void GlControlOnPaint(object sender, PaintEventArgs e)
        {
            if (!GlContolLoaded)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Translate(-1.2, -1.2, 1.2);

            CurrentVisualisation.InitGrid();

            GL.Translate(1.2, 1.2, -1.2);

            CurrentVisualisation.Render();

            _glControl.SwapBuffers();

            
        }

        /// <summary>
        /// Иницилизирует подпись оси
        /// </summary>
        private void InitAxisLabel()
        {
            _axisLabel = new Label
            {
                Text = @"ОСЬ",
                AutoSize = true,
                BackColor = Color.Transparent,
                Visible = false,
                Size = new Size(20, 17),
                Enabled = true,
                Location = new Point(_mainForm.Width / 2, _mainForm.Height / 2)
            };
            _mainForm.Controls.Add(_axisLabel);
            _axisLabel.BringToFront();
        }

        /// <summary>
        /// Создаёт нужное кол-во вершин
        /// </summary>
        public void InitVertexLabels(int size)
        {
            _vertexLabels = new Label[size];
            for (var i = 0; i < _vertexLabels.Length; i++)
            {
                _vertexLabels[i] = new Label
                {
                    Text = (i + 1).ToString(),
                    AutoSize = true,
                    BackColor = Color.White,
                    Visible = false,
                    Size = new Size(20, 17),
                    Enabled = true,
                    Location = new Point(_mainForm.Width / 2, _mainForm.Height / 2),
                };
                _mainForm.Controls.Add(_vertexLabels[i]);
                _vertexLabels[i].BringToFront();
            }
            InitInitVertexLabels(size);
        }

        /// <summary>
        /// Создаёт нужное кол-во начальных вершин
        /// </summary>
        private void InitInitVertexLabels(int size)
        {
            _initVertexLabels = new Label[size];
            for (var i = 0; i < _initVertexLabels.Length; i++)
            {
                _initVertexLabels[i] = new Label
                {
                    Text = $@"{i + 1}",
                    AutoSize = true,
                    BackColor = Color.DarkSeaGreen,
                    Visible = false,
                    Size = new Size(20, 17),
                    Enabled = true,
                    Location = new Point(_mainForm.Width / 2, _mainForm.Height / 2),
                };
                _mainForm.Controls.Add(_initVertexLabels[i]);
                _initVertexLabels[i].BringToFront();
            }
        }

        /// <summary>
        /// Убирает ненужные вершины, а также ось
        /// </summary>
        public void HideVertexLabelsAndAxis()
        {
            foreach (var vertexLabel in _vertexLabels)
            {
                vertexLabel.Hide();
            }

            foreach (var initVertexLabel in _initVertexLabels)
            {
                initVertexLabel.Hide();
            }

            _axisLabel.Hide();
        }

        /// <summary>
        /// Получает Label с номером вершины по номеру вершины
        /// </summary>
        /// <param name="index">номер Label</param>
        /// <returns>Label с индексом вершины</returns>
        public Label GetVertexLabel(int index)
        {
            if (index < 0 || index >= _vertexLabels.Length) throw new ArgumentOutOfRangeException();

            return _vertexLabels[index];
        }

        /// <summary>
        /// Обновляет размеры gl элемента
        /// </summary>
        public void UpdateGlSettings()
        {
            if (!GlContolLoaded) return;

            GL.Viewport(new Point(0, _glControl.Location.Y),
                _glControl.Size);
            _glControl.Update();

            var p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), _glControl.AspectRatio, 0.1f,
                500);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref p);
            WorldInfo.ProjectionMatrix = p;

            _glControl.Refresh();
        }

        /// <summary>
        /// Прошел ли один шаг с начала сессии
        /// </summary>
        private bool _isOneStepWaited;

        /// <summary>
        /// Началась ли сессия анимаци
        /// </summary>
        private bool _isAnimatingSessionStarted;

        /// <summary>
        /// Обновляет отображение вершин
        /// </summary>
        public void UpdateVerticesIndexies()
        {
            HideVertexLabelsAndAxis();

            var points = CurrentVisualisation.ScreenPoints;
            var indexies = CurrentVisualisation.ImageVerticesIndexies;

            for (var i = 0; i < points.Length; i++)
            {
                if (points[i].Z > 4.2f)
                {
                    GetVertexLabel(i).Visible = false;
                    continue;
                }

                if (!GetVertexLabel(i).Visible)
                {
                    GetVertexLabel(i).Visible = true;
                }

                points[i] = points[i] / points[i].Z;

                var screenPoint = GetScreenCoords(points[i]);
                if (screenPoint.X + _glControl.Location.X >= _glControl.Left &&
                    screenPoint.X + _glControl.Location.X <= _glControl.Right)
                {
                    GetVertexLabel(i).Location =
                        new Point(_glControl.Location.X + screenPoint.X - GetVertexLabel(i).Width,
                            _glControl.Location.Y + screenPoint.Y);
                }
                else
                {
                    GetVertexLabel(i).Visible = false;
                }
                
            }

            var axisCoords = _currentVisualisation.AxisCoordsOnScreen;
            if (axisCoords != null)
            {
                _axisLabel.Visible = true;
                
                var shownVertex = axisCoords.Item1;
                if (shownVertex.Z > axisCoords.Item2.Z)
                {
                    shownVertex = axisCoords.Item2;
                }

                shownVertex = shownVertex / shownVertex.Z;

                var screenPoint = GetScreenCoords(shownVertex);
                if (screenPoint.X + _glControl.Location.X >= _glControl.Left &&
                    screenPoint.X + _glControl.Location.X <= _glControl.Right)
                {
                    _axisLabel.Location = new Point(_glControl.Location.X + screenPoint.X - _axisLabel.Width / 2,
                        _glControl.Location.Y + screenPoint.Y);
                }
                else
                {
                    _axisLabel.Visible = false;
                }
            }

            if (IsAnimatingSessionStarted && !_isOneStepWaited)
            {
                _isOneStepWaited = true;
                return;
            }

            if (indexies == null || !IsAnimatingSessionStarted) return;

            for (var i = 0; i < indexies.Length; i++)
            {
                if (!GetVertexLabel(i).Visible) continue;

                _initVertexLabels[indexies[i]].Show();
                _initVertexLabels[indexies[i]].Top = GetVertexLabel(i).Top;
                _initVertexLabels[indexies[i]].Left = GetVertexLabel(i).Right;
            }
        }

        /// <summary>
        /// Получает координаты точки на экране
        /// </summary>
        /// <param name="point">Точка в пространстве</param>
        /// <returns>Точка на экране</returns>
        private Point GetScreenCoords(Vector3 point)
        {
            return new Point(_glControl.Width / 2 + (int)(point.X * _glControl.Width / 2),
                _glControl.Height - (int)((point.Y + 1) / 2 * _glControl.Height) - _glControl.Top);
        }

        /// <summary>
        /// Возвращает или задаёт началсь ли сессия анимаци
        /// </summary>
        public bool IsAnimatingSessionStarted
        {
            get => _isAnimatingSessionStarted;
            set
            {
                _isAnimatingSessionStarted = value;
                if (value == false)
                    _isOneStepWaited = false;
            }
        }

        /// <summary>
        /// Задаёт кнопки в html
        /// </summary>
        /// <param name="htmlView"></param>
        public void SetButtons([NotNull] WebBrowser htmlView)
        {
            IsAnimatingSessionStarted = false;
            IsPlayingAnimation = false;
            htmlView.DocumentCompleted += HtmlOnDocumentCompleted;
        }

        /// <summary>
        /// Событие при загрузке документа. Добавляет кнопки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void HtmlOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            if (!(sender is WebBrowser htmlView)) return;

            var buttonsDiv = htmlView.Document?.GetElementById("buttons");

            if (buttonsDiv == null)
            {
                htmlView.DocumentCompleted -= HtmlOnDocumentCompleted;
                return;
            }

            if (CurrentVisualisation.ReadOnlyAnimations == null)
            {
                htmlView.DocumentCompleted -= HtmlOnDocumentCompleted;
                return;
            }

            var infoDiv = htmlView.Document?.GetElementById("info");
            if (infoDiv == null)
            {
                htmlView.DocumentCompleted -= HtmlOnDocumentCompleted;
                return;
            }

            AddVisualisationInformation(infoDiv, htmlView);

            // настройка кнопок
            var cntRotation = 0;
            var cntSymmetry = 1;
            foreach (var animation in CurrentVisualisation.ReadOnlyAnimations)
            {
                var div = htmlView.Document.CreateElement("div");
                var button = htmlView.Document.CreateElement("input");
                var p = htmlView.Document.CreateElement("p");
                if (button == null || div == null || p == null)
                    return;
                button.SetAttribute("type", "button");
                button.SetAttribute("value", "animate");
                
                button.Click += (o, eventArgs) =>
                {
                    IsAnimatingSessionStarted = true;
                    IsPlayingAnimation = true;
                    CurrentVisualisation.SetAnimation(animation);
                    CheckHtmlButtons(htmlView);
                };
                div.AppendChild(p);

                switch (animation)
                {
                    case RotationAnimation rotation:
                        p.InnerHtml = RotationAnimationToHtml(rotation, CurrentVisualisation);
                        button.InnerText = $"Поворот {cntRotation}";
                        button.SetAttribute("class", "rotation");
                        button.Style = "background-color: #4CAF50; border-radius: 50px;";
                        div.Id = $"rotation{cntRotation}";
                        cntRotation++;
                        break;
                    case SymmetryAnimation symmetry:
                        p.InnerHtml = SymmetryAnimationToHtml(symmetry, CurrentVisualisation);
                        button.InnerText = $"Симметрия {cntSymmetry}";
                        div.Id = $"symmetry{cntSymmetry}";
                        button.SetAttribute("class", "symmetry");
                        button.Style = "background-color: #008CBA; border-radius: 3px;";
                        cntSymmetry++;
                        break;
                    default:
                        p.InnerHtml = "";
                        break;
                }

                p.InnerHtml += "<div>Подстановка соответсвующая самосовмещению:" +
                    PermutationVisualisation.ListOfTuplesToHtml(CurrentVisualisation
                        .ConvertAnimationToPermutation(animation).TupleList) + "</div>";
                
                div.AppendChild(button);

                div.Style = "border-style: solid; border-width: 1pt; border-color: #f28b71; margin-top: 10pt;" +
                            "border-radius: 20px;" +
                            "padding: 0 5pt 5pt 5pt;";
                
                buttonsDiv.AppendChild(div);
            }

            var resetButton = htmlView.Document?.GetElementById("reset_button");

            if (resetButton == null)
            {
                htmlView.DocumentCompleted -= HtmlOnDocumentCompleted;
                return;
            }

            resetButton.Click += (o, eventArgs) =>
            {
                CurrentVisualisation.Reset();
            };

            htmlView.DocumentCompleted -= HtmlOnDocumentCompleted;
        }

        /// <summary>
        /// Добавляет к визуализации некоторую информацию, которая может содержать ссылки на самосовмещения.
        /// </summary>
        /// <param name="infoDiv">куда добавлять</param>
        /// <param name="htmlView">отображение HTML</param>
        private void AddVisualisationInformation([NotNull] HtmlElement infoDiv, [NotNull] WebBrowser htmlView)
        {
            if (CurrentVisualisation.UserTutorialHtmlCode == null)
                return;

            infoDiv.InnerHtml = CurrentVisualisation.UserTutorialHtmlCode;
            foreach (HtmlElement div in infoDiv.GetElementsByTagName("div"))
            {
                if (div.Id == "rotation")
                {
                    var rotationNumber = int.Parse(div.InnerText);
                    var rotation = CurrentVisualisation.GetRotationByIndex(rotationNumber);
                    var button = infoDiv.Document?.CreateElement("input");
                    var a = infoDiv.Document?.CreateElement("a");

                    div.InnerText = "";

                    if (a == null || button == null)
                        continue;

                    a.SetAttribute("href", $"#rotation{rotationNumber}");
                    button.InnerText = $"Поворот {rotationNumber}";
                    button.SetAttribute("type", "button");

                    button.Click += (sender, args) => {
                        IsAnimatingSessionStarted = true;
                        IsPlayingAnimation = true;
                        CurrentVisualisation.SetAnimation(rotation);
                        CheckHtmlButtons(htmlView);
                    };
                    a.InnerHtml = "[См.]";

                    button.Style = "margin-left: 2pt; margin-right: 1pt; background-color: #4CAF50; border-radius: 50px;";
                    a.Style = "margin-right: 2pt; font-size: 10pt;";

                    div.AppendChild(button);
                    div.AppendChild(a);
                }
                else if (div.Id == "symmetry")
                {
                    var symmetryNumber = int.Parse(div.InnerText);
                    var symmetry = CurrentVisualisation.GetSymmetryByIndex(symmetryNumber - 1);
                    var button = infoDiv.Document?.CreateElement("input");
                    var a = infoDiv.Document?.CreateElement("a");

                    div.InnerText = "";

                    if (a == null || button == null)
                        continue;

                    a.SetAttribute("href", $"#symmetry{symmetryNumber}");
                    button.InnerText = $"Симметрия {symmetryNumber}";
                    button.SetAttribute("type", "button");

                    button.Click += (sender, args) => {
                        IsAnimatingSessionStarted = true;
                        IsPlayingAnimation = true;
                        CurrentVisualisation.SetAnimation(symmetry);
                        CheckHtmlButtons(htmlView);
                    };
                    a.InnerHtml = "[См.]";

                    button.Style = "margin-left: 2pt; margin-right: 1pt; background-color: #008CBA; border-radius: 3px;";
                    a.Style = "margin-right: 2pt; font-size: 10pt;";

                    div.AppendChild(button);
                    div.AppendChild(a);
                }
            }
        }

        /// <summary>
        /// Проигрывается ли анимация
        /// </summary>
        public bool IsPlayingAnimation { get; set; }

        /// <summary>
        /// Обновляет информацию
        /// </summary>
        public void UpdateInfo()
        {
            if (CurrentVisualisation.CurrentAnimation == null || CurrentVisualisation.CurrentAnimation.IsFinish)
            {
                IsPlayingAnimation = false;
            }
        }

        /// <summary>
        /// Проверяет какое состояние должно быть у кнопок и задаёт его
        /// </summary>
        /// <param name="htmlView"></param>
        public void CheckHtmlButtons(WebBrowser htmlView)
        {
            var buttons = htmlView.Document?.GetElementsByTagName("input");
            if (buttons == null)
                return;

            foreach (HtmlElement button in buttons)
            {
                button.Enabled = !IsPlayingAnimation; // если играет анимация, то нельзя выбрать другую кнопку
            }
        }

        /// <summary>
        /// Преобразовывает угол в дробь, домноженную на пи
        /// </summary>
        /// <param name="angle">угол</param>
        /// <returns>пара, где первое - числитель, второе - знаменатель</returns>
        [CanBeNull]
        public static Tuple<int, int> AngleToFracWithPi(float angle)
        {
            var frac = angle / MathHelper.Pi;
            const float delta = 0.00001f;
            for (var denominator = 1; denominator <= 20; denominator++)
            {
                var fracNumerator = denominator * frac;
                for (var numerator = 1; numerator <= denominator * 2; numerator++)
                {
                    if (Math.Abs(numerator - fracNumerator) < delta)
                    {
                        return new Tuple<int, int>(numerator, denominator);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Преобразовавет поворот в html строку
        /// </summary>
        /// <param name="rotation">поворот</param>
        /// <param name="visualisation">визуализация</param>
        /// <returns>html строка</returns>
        [NotNull]
        public static string RotationAnimationToHtml([NotNull] RotationAnimation rotation,
            [NotNull] VisualisationLesson visualisation)
        {
            var frac = AngleToFracWithPi(rotation.Angle);
            if (frac == null)
            {
                return 
                    $@"Поворот на {rotation.Angle} {GetRotationAxisForVisualisation(rotation, visualisation)}";
            }

            if (frac.Item2 == 1)
            { 
                return
                    $@"<span style=""vertical-align: 5pt;"">Поворот на</span>
                    <table style=""display: inline; vertical-align: baseline;"">
                      <tr>
                        <td>
                          {(frac.Item1 == 1 ? "" : frac.Item2.ToString())}
                        </td>
                        <td>
                          &#960;
                        </td>
                      </tr>
                    </table>
                    <span style=""vertical-align: 5pt;"">
                      {GetRotationAxisForVisualisation(rotation, visualisation)}
                    </span>";
            }

            return 
                $@"<span style=""vertical-align: 17pt;"">Поворот на</span>
                <table style=""display: inline; vertical-align: baseline;"">
                  <tr>
                    <td>
                      <table>
                        <tr>
                          <td>
                            {frac.Item1}
                          </td>
                        </tr>
                        <tr>
                          <td style=""border-top-style: solid; border-top-color: black;"">
                            {frac.Item2}
                          </td>
                        </tr>
                      </table>
                    </td>
                    <td>
                      &#960;
                    </td>
                  </tr>
                </table>
                <span style=""vertical-align: 17pt;"">
                  {GetRotationAxisForVisualisation(rotation, visualisation)}
                </span>";
        }

        /// <summary>
        /// Получает информацию о повороте для переданной визуализации.
        /// </summary>
        /// <param name="rotation">Поворот</param>
        /// <param name="visualisation">Визуализауич</param>
        /// <returns>Угол поворота и ось в формате HTML таблицы</returns>
        [NotNull]
        public static string GetRotationAxisForVisualisation([NotNull] RotationAnimation rotation,
            [NotNull] VisualisationLesson visualisation)
        {
            const float tolerance = 0.000001f;
            if (Math.Abs(rotation.Angle) < tolerance)
                return "";

            var firstPointText = "";
            var firstPoint = new Vector3();
            var vertices = visualisation.ReadOnlyInitVertices;
            var edges = visualisation.ReadOnlyEdges;
            var faces = visualisation.ReadOnlyFaces;

            for (var index = 0; index < vertices.Count; index++)
            {
                var vertex = vertices[index];
                if (!VectorUtils.IsVertexOnLineByPointAndDirection(vertex, Vector3.Zero, rotation.Axis)) continue;
                firstPointText = $"вершину {index + 1}";
                firstPoint = vertex;
            }

            if (firstPointText == "")
            {
                foreach (var edge in edges)
                {
                    if (!VectorUtils.IsVertexOnLineByPointAndDirection(VectorUtils.EdgeCenter(edge), Vector3.Zero,
                        rotation.Axis)) continue;
                    firstPointText = $"середину ребра ({visualisation.GetStarterIndexByVertex(edge.Item1) + 1}, " +
                                     $"{visualisation.GetStarterIndexByVertex(edge.Item2) + 1})";
                    firstPoint = VectorUtils.EdgeCenter(edge);
                }
            }

            if (firstPointText == "")
            {
                foreach (var face in faces)
                {
                    if (!VectorUtils.IsVertexOnLineByPointAndDirection(face.Center, Vector3.Zero, rotation.Axis)) continue;
                    firstPoint = face.Center;

                    var sb = new StringBuilder("центр грани (");

                    for (var i = 0; i < face.Count; i++)
                    {
                        sb.AppendFormat("{0}, ", visualisation.GetStarterIndexByVertex(face[i]) + 1);
                    }

                    sb[sb.Length - 2] = ')';

                    firstPointText = sb.ToString();
                }
            }

            var secondPointText = "";

            for (var index = 0; index < vertices.Count; index++)
            {
                if (VectorUtils.AreVectorsEqual(vertices[index], firstPoint)) continue;

                if (VectorUtils.IsVertexOnLineByPointAndDirection(vertices[index], Vector3.Zero, rotation.Axis))
                {
                    secondPointText = $"вершину {index + 1}";
                }
            }

            if (secondPointText == "")
            {
                foreach (var edge in edges)
                {
                    if (VectorUtils.AreVectorsEqual(VectorUtils.EdgeCenter(edge), firstPoint)) continue;

                    if (!VectorUtils.IsVertexOnLineByPointAndDirection(VectorUtils.EdgeCenter(edge), Vector3.Zero,
                        rotation.Axis)) continue;
                    secondPointText = $"середину ребра ({visualisation.GetStarterIndexByVertex(edge.Item1) + 1}, " +
                                     $"{visualisation.GetStarterIndexByVertex(edge.Item2) + 1})";
                }
            }

            if (secondPointText == "")
            {
                foreach (var face in faces)
                {
                    if (VectorUtils.AreVectorsEqual(face.Center, firstPoint)) continue;

                    if (!VectorUtils.IsVertexOnLineByPointAndDirection(face.Center, Vector3.Zero, rotation.Axis)) continue;

                    var sb = new StringBuilder("центр грани (");

                    for (var i = 0; i < face.Count; i++)
                    {
                        sb.AppendFormat("{0}, ", visualisation.GetStarterIndexByVertex(face[i]) + 1);
                    }

                    sb[sb.Length - 2] = ')';

                    secondPointText = sb.ToString();
                }
            }

            if (firstPointText == "" && secondPointText == "")
                return "";

            return $"относительно оси проходящей через {firstPointText}" +
                   $" {(secondPointText == "" ? "" : "и " + secondPointText)}";
        }

        /// <summary>
        /// Представляет симметрию в html
        /// </summary>
        /// <param name="symmetry">симметрия</param>
        /// <param name="visualisation">3д объект</param>
        /// <returns>Представление для html страницы</returns>
        public static string SymmetryAnimationToHtml(SymmetryAnimation symmetry, VisualisationLesson visualisation)
        {
            var vertices = visualisation.ReadOnlyInitVertices;

            if (!(visualisation is PolygonVisualisation))
            {
                return $"Симметрия относительно плоскости {symmetry.Plane}";
            }

            const float comparationConstant = 0.0001f;
            if (vertices.Count % 2 == 1)
            {
                var verIndex = 0;
                for (var i = 0; i < vertices.Count; i++)
                    if (Math.Abs(symmetry.Plane.Value(vertices[i])) < comparationConstant)
                    {
                        verIndex = i;
                        break;
                    }

                return
                    $"Симметрия относительно оси, проходящей через вершину {verIndex + 1}" +
                    $" и середину ребра ({(verIndex + vertices.Count / 2) % vertices.Count + 1}," +
                    $" {(verIndex + vertices.Count / 2 + 1) % vertices.Count + 1})";
            }
            else
            {
                var verIndex = -1;
                for (var i = 0; i < vertices.Count / 2; i++)
                    if (Math.Abs(symmetry.Plane.Value(vertices[i])) < comparationConstant)
                    {
                        verIndex = i;
                        break;
                    }

                if (verIndex != -1)
                {
                    return
                        $"Симметрия относительно оси, проходящей через вершину {verIndex + 1}" +
                        $" и вершину {(verIndex + vertices.Count / 2) % vertices.Count + 1 }";
                }

                verIndex = -1;
                for (var i = 0; i < vertices.Count / 2; i++)
                {
                    if (Math.Abs(symmetry.Plane.Value((vertices[i] + vertices[i + 1]) / 2)) < comparationConstant)
                        verIndex = i;
                }

                return "Симметрия относительно оси, проходящей через середину ребра" +
                       $" ({verIndex + 1}, {verIndex + 2}) и середину ребра" +
                       $" ({(verIndex + vertices.Count / 2) % vertices.Count + 1}," +
                       $" {(verIndex + vertices.Count / 2 + 1) % vertices.Count + 1})";
            }
        }
    }
}