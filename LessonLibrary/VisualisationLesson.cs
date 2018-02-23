using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для визуализаций
    /// </summary>
    public abstract class VisualisationLesson
    {
        /// <summary>
        /// Отрисовывает фигуру
        /// </summary>
        public abstract void Render();

        public VisualisationTransform Transform { get; set; }

        protected VisualisationLesson()
        {
            Transform = new VisualisationTransform();
        }

        /// <summary>
        /// Задаёт координатную сетку
        /// </summary>
        public void InitGrid()
        {
            GL.Begin(PrimitiveType.Lines);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Blue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(3, 0, 0);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 3, 0);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, -3);

            GL.End();
        }
    }
}