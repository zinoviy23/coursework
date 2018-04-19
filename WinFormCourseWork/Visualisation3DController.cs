using System;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;
using LessonLibrary.Visualisation3D;
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
        /// Массив исходных вершин на экране
        /// </summary>
        private Label[] _initVertexLabels;

        /// <summary>
        /// Ссылка на объект элемента TreeView
        /// </summary>
        private readonly TreeView _lessonTreeView;

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
        /// <param name="lessonTreeView">TreeView для выбора уроков</param>
        public Visualisation3DController([NotNull] GLControl glControl, [NotNull] MainForm mainForm,
            [NotNull] TreeView lessonTreeView)
        {
            _glControl = glControl;
            _mainForm = mainForm;
            _lessonTreeView = lessonTreeView;
            InitVertexLabels(20);
            _glControl.Load += GlControlOnLoad;
            _glControl.Paint += GlControlOnPaint;
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
            GL.Viewport(new Point(_glControl.Location.X - _lessonTreeView.Width, _glControl.Location.Y), _glControl.Size);

            GL.ClearColor(Color.DarkGray);
            
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
                    BackColor = Color.Transparent,
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
        /// Убирает ненужные вершины
        /// </summary>
        public void HideVertexLabels()
        {
            foreach (var vertexLabel in _vertexLabels)
            {
                vertexLabel.Hide();
            }

            foreach (var initVertexLabel in _initVertexLabels)
            {
                initVertexLabel.Hide();
            }
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

            GL.Viewport(new Point(_glControl.Location.X - _lessonTreeView.Width, _glControl.Location.Y),
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
            HideVertexLabels();

            var points = CurrentVisualisation.ScreenPoints;
            var indexies = CurrentVisualisation.ImageVerticesIndexies;

            for (var i = 0; i < points.Length; i++)
            {
                if (points[i].Z > 4)
                {
                    GetVertexLabel(i).Visible = false;
                    continue;
                }

                if (!GetVertexLabel(i).Visible)
                {
                    GetVertexLabel(i).Visible = true;
                }

                points[i] = points[i] / points[i].Z;

                var x = _glControl.Width / 2 + (int)(points[i].X * _glControl.Width / 2);
                var y = _glControl.Height - (int)((points[i].Y + 1) / 2 * _glControl.Height);
                GetVertexLabel(i).Location =
                    new Point(_glControl.Location.X + x, _glControl.Location.Y + y);
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
    }
}