using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary
{
    /// <inheritdoc cref="VisualisationLesson"/>
    /// <summary>
    /// Класс для визуализации куба
    /// </summary>
    public class CubeVisualisation : VisualisationLesson
    {
        private readonly Vector3[] _vertices;
        private readonly Vector3[] _normals;

        /// <summary>
        /// Иницилизирует куб
        /// </summary>
        public CubeVisualisation()
        {
            _vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f)
            };

            _normals = new[]
            {
                Vector3.Cross(_vertices[3] - _vertices[0], _vertices[1] - _vertices[0]), // низ
                Vector3.Cross(_vertices[4] - _vertices[0], _vertices[3] - _vertices[0]), // право
                Vector3.Cross(_vertices[2] - _vertices[1], _vertices[5] - _vertices[1]), // лево
                Vector3.Cross(_vertices[5] - _vertices[4], _vertices[7] - _vertices[4]), // верх 
                Vector3.Cross(_vertices[1] - _vertices[0], _vertices[4] - _vertices[0]), // перед
                Vector3.Cross(_vertices[2] - _vertices[6], _vertices[7] - _vertices[6]), // зад 
            };
        }

        /// <summary>
        /// Рисует куб
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public override void Render()
        {
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Red);

            GL.Begin(PrimitiveType.Triangles);
            
            // нижняя грань
            GL.Normal3(_normals[0]);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[2]);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[2]);

            //правая грань
            GL.Normal3(_normals[1]);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[7]);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[7]);

            //левая грань
            GL.Normal3(_normals[2]);

            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[2]);
            GL.Vertex3(_vertices[6]);

            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[6]);

            //Верхняя грань
            GL.Normal3(_normals[3]);

            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[6]);

            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[7]);
            GL.Vertex3(_vertices[6]);

            // Передняя грань
            GL.Normal3(_normals[4]);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[5]);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[5]);

            // задняя грань
            GL.Normal3(_normals[5]);

            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[7]);
            GL.Vertex3(_vertices[6]);

            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[2]);
            GL.Vertex3(_vertices[6]);

            GL.End();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[2]);
            GL.Vertex3(_vertices[3]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[6]);
            GL.Vertex3(_vertices[7]);

            GL.End();

            GL.Begin(PrimitiveType.Lines);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[4]);

            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[5]);

            GL.Vertex3(_vertices[2]);
            GL.Vertex3(_vertices[6]);

            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[7]);

            GL.End();
        }
    }
}