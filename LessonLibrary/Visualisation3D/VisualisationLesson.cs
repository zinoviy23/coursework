using System;
using JetBrains.Annotations;
using LessonLibrary.Visualisation3D.Animations;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary.Visualisation3D
{
    /// <summary>
    /// Класс для визуализаций
    /// </summary>
    public abstract class VisualisationLesson
    {
        /// <summary>
        /// Вершины, которые отрисовываются сейчас
        /// </summary>
        protected Vector3[] Vertices;

        /// <summary>
        /// Вершины без всяких преобразований
        /// </summary>
        protected Vector3[] InitVertices;

        /// <summary>
        /// Нормали, которые используются сейчас
        /// </summary>
        protected Vector3[] Normals;

        /// <summary>
        /// Нормали без всяких преобразований
        /// </summary>
        protected Vector3[] InitNormals;

        /// <summary>
        /// Отрисовывает фигуру
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Положение в пространствее данной визуализации
        /// </summary>
        public VisualisationTransform Transform { get; set; }

        // Анимация, которая сейчас проигрывается
        public IAnimation CurrentAnimation { get; set; }

        /// <summary>
        /// Конструтор, общий для всех визуализаций
        /// </summary>
        [UsedImplicitly]
        protected VisualisationLesson() => Transform = new VisualisationTransform();

        /// <summary>
        /// Задаёт координатную сетку
        /// </summary>
        public void InitGrid()
        {
            GL.Begin(PrimitiveType.Lines);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Blue);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(3, 0, 0);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Red);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 3, 0);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Green);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, -3);

            GL.End();
        }

        /// <summary>
        /// Радиус вершины
        /// </summary>
        private const float R = 0.07f;

        /// <summary>
        /// Кол-во делений по X
        /// </summary>
        private const int Nx = 7;

        /// <summary>
        /// Кол-во делений по Y
        /// </summary>
        private const int Ny = 7;

        /// <summary>
        /// Рисует вершину
        /// </summary>
        /// <param name="position"></param>
        public static void DrawVertex(Vector3 position)
        {
            GL.Translate(position);

            const double dnx = 1.0 / Nx;
            const double dny = 1.0 / Ny;
            GL.Begin(PrimitiveType.QuadStrip);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.BlueViolet);

            const double piy = Math.PI * dny;
            const double pix = Math.PI * dnx;
            for (var iy = 0; iy < Ny; iy++)
            {
                double diy = iy;
                var ay = diy * piy;
                var sy = Math.Sin(ay);
                var cy = Math.Cos(ay);
                var ay1 = ay + piy;
                var sy1 = Math.Sin(ay1);
                var cy1 = Math.Cos(ay1);
                //int ix;
                for (var ix = 0; ix <= Nx; ix++)
                {
                    var ax = 2.0 * ix * pix;
                    var sx = Math.Sin(ax);
                    var cx = Math.Cos(ax);
                    var x = R * sy * cx;
                    var y = R * sy * sx;
                    var z = -R * cy;
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);
                    x = R * sy1 * cx;
                    y = R * sy1 * sx;
                    z = -R * cy1;
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);
                }
            }
            GL.End();
            GL.Translate(-position);
        }

        /// <summary>
        /// Рисует все вершины
        /// </summary>
        protected void DrawVertices()
        {
            foreach (var vertex in Vertices)
            {
                DrawVertex(vertex);
            }
        }

        /// <summary>
        /// Копия вершин (чтобы не портить)
        /// </summary>
        public Vector3[] VerticesClone => (Vector3[])Vertices.Clone();

        /// <summary>
        /// Преобразует координаты вершины в оконные
        /// </summary>
        /// <param name="vertex">Координаты в пространстве</param>
        /// <returns>Координаты в окне</returns>
        public Vector3 VertexScreenPosition(Vector3 vertex)
        {
            var res = Vector3.Transform(vertex, Transform.Matrix);
            res = Vector3.Transform(res, WorldInfo.ViewMatrix);
            return Vector3.TransformPosition(res, WorldInfo.ProjectionMatrix);
        }

        /// <summary>
        /// Позиции вершин на экране
        /// </summary>
        public Vector3[] ScreenPoints
        {
            get
            {
                var result = new Vector3[Vertices.Length];
                for (var i = 0; i < result.Length; i++)
                {
                    result[i] = VertexScreenPosition(Vertices[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// Сбрасывает все изменения
        /// </summary>
        public void Reset()
        {
            CurrentAnimation?.Reset();
            Vertices = (Vector3[]) InitVertices.Clone();
            Normals = (Vector3[]) InitNormals.Clone();
        }

        /// <summary>
        /// Применяет анимацию к каждой вершине и нормали
        /// </summary>
        protected void ApplyCurrentAnimationInRender()
        {
            if (CurrentAnimation == null) return;

            for (var i = 0; i < Vertices.Length; i++)
                Vertices[i] = CurrentAnimation.Apply(InitVertices[i]);

            for (var i = 0; i < Normals.Length; i++)
            {
                Normals[i] = CurrentAnimation.Apply(InitNormals[i]);
            }
        }
    }
}