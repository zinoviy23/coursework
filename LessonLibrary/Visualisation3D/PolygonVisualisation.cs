using System;
using LessonLibrary.Visualisation3D.Animations;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary.Visualisation3D
{
    /// <inheritdoc />
    /// <summary>
    /// Визуализация многоугольников
    /// </summary>
    public sealed class PolygonVisualisation : VisualisationLesson
    {
        /// <inheritdoc />
        /// <summary>
        /// Конструктор
        /// </summary>
        public PolygonVisualisation()
        {
            VerticesCount = 0;
        }

        /// <summary>
        /// Кол-во вершин в многоугольнике
        /// </summary>
        private int _verticesCount;

        /// <summary>
        /// Возвращает и задаёт кол-во вершин в многоугольнике
        /// </summary>
        public int VerticesCount
        {
            get => _verticesCount;
            set
            {
                _verticesCount = value;
                InitPolygonVertices();
            }
        }

        public override void Render()
        {
            Transform.SetTransform();

            ApplyCurrentAnimationInRender();

            GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse, Color4.Yellow);
            GL.Normal3(Normals[0]);
            GL.Begin(PrimitiveType.Polygon);

            foreach (var vertex in Vertices)
            {
                GL.Vertex3(vertex);
            }

            GL.End();

            GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse, Color4.Yellow);
            GL.Normal3(-Normals[0]);
            GL.Begin(PrimitiveType.Polygon);

            for (var i = VerticesCount - 1; i >= 0; i--)
                GL.Vertex3(Vertices[i] - 0.01f * Normals[0]);

            GL.End();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Normal3(Normals[0]);

            foreach (var vertex in Vertices)
            {
                GL.Vertex3(vertex);
            }

            GL.End();

            DrawVertices();

            Transform.UnsetTransform();
        }

        protected override void InitFaces()
        {
            Faces = new[] {new Face(Vertices) };
        }

        public override string UserTutorialHtmlCode =>
            $@"<p>
            Повороты и симметрии <b>{_verticesCount}-угольника</b> составляют группу мощности {_verticesCount * 2}.
          </p>
          <p style=""display: inline;"">
            Например, <div style=""display: inline;"" id=""rotation"">1</div> и 
            <div style=""display: inline;"" id=""rotation"">{_verticesCount - 1}</div> обратные друг другу.
          </p>
          <p style=""display: inline;"">
            А <div style=""display: inline;"" id=""symmetry"">{(_verticesCount + 1) / 2}</div> обратен сам себе.
          </p>
          <p style=""display: inline;"">
            Рассмотрим теперь композиции движений. Например, <div style=""display: inline;"" id=""rotation"">2</div>
            умножить на <div style=""display: inline;"" id=""symmetry"">1</div> это 
            <div style=""display: inline;"" id=""symmetry"">{(_verticesCount % 2 == 0 ? _verticesCount / 2 : _verticesCount)}</div>.
            А если применить эти повороты наоборот, то получиться <div style=""display: inline;"" id=""symmetry"">2</div>.
          </p>";

        /// <summary>
        /// Задаёт координаты и анимации
        /// </summary>
        private void InitPolygonVertices()
        {
            var angle = 2 * MathHelper.Pi / _verticesCount;
            var r = (float)Math.Sqrt(1.0f / (2 * (1 - (float)Math.Cos(angle))));
            if (_verticesCount > 10)
            {
                r *= 0.7f;
            }

            InitVertices = new Vector3[_verticesCount];

            for (var i = 0; i < _verticesCount; i++)
            {
                InitVertices[i] = new Vector3((float) Math.Cos(angle * i), (float) Math.Sin(angle * i), 0) * r;
            }

            InitNormals = new[] {new Vector3(0, 0, 1)};

            PrevVertices = (Vector3[]) InitVertices.Clone();
            PrevNormals = (Vector3[]) InitNormals.Clone();
            Vertices = (Vector3[]) PrevVertices.Clone();
            Normals = (Vector3[]) PrevNormals.Clone();

            SetAnimations(GenAnimations());

            if (_verticesCount < 3) return;
            InitFaces();
            InitEdgesByFaces();
        }

        /// <summary>
        /// Генерирует анимации по количеству вершин
        /// </summary>
        /// <returns></returns>
        private IAnimation[] GenAnimations()
        {
            var animations = new IAnimation[_verticesCount * 2];

            var angle = 2 * MathHelper.Pi / _verticesCount;

            for (var i = 0; i < _verticesCount; i++)
            {
                animations[i] = new RotationAnimation(angle * i, Vector3.UnitZ, MathHelper.Pi / 5);
            }


            if (_verticesCount % 2 != 0)
            {
                for (var i = 0; i < _verticesCount; i++)
                {
                    animations[i + _verticesCount] =
                        new SymmetryAnimation(new Plane(Vector3.Cross(InitVertices[i], Vector3.UnitZ), Vector3.Zero), 0.6f);
                }
            }
            else
            {
                for (var i = 0; i < _verticesCount / 2; i++)
                {
                    animations[i + _verticesCount] =
                        new SymmetryAnimation(new Plane(Vector3.Cross(InitVertices[i], Vector3.UnitZ), Vector3.Zero),
                            0.6f);
                }

                for (var i = 0; i < _verticesCount / 2; i++)
                {
                    animations[i + _verticesCount * 3 / 2] =
                        new SymmetryAnimation(
                            new Plane(Vector3.Cross(InitVertices[i] + InitVertices[i + 1], Vector3.UnitZ),
                                Vector3.Zero), 0.6f);
                }
            }

            return animations;
        }
        
    }
}