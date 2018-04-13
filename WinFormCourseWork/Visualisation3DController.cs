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
        private MainForm _mainForm;

        private bool _glControlLoaded;

        private TreeView _lessonTreeView;

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
        }

        private void GlControlOnLoad(object sender, EventArgs args)
        {
            _glControlLoaded = true;
            GL.Viewport(new Point(_glControl.Location.X - _lessonTreeView.Width, _glControl.Location.Y), _glControl.Size);

            GL.ClearColor(Color.DarkGray);
            GL.Enable(EnableCap.DepthTest);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.LineSmooth);

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
    }
}