using System;
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
    /// Класс для визуализации тэтраэдра
    /// </summary>
    public class TetrahedronVisualisation : VisualisationLesson
    {
        private readonly Vector3[] _vertices;
        private readonly Vector3[] _normals;

        /// <summary>
        /// Иницилизирует тэтраэдр
        /// </summary>
        public TetrahedronVisualisation()
        {
            _vertices = new[]
            {
                new Vector3(-0.5f, (float)-(Math.Sqrt(8.0 / 12) / 4), (float)-Math.Sqrt(3) / 6),
                new Vector3(0.5f, (float)-(Math.Sqrt(8.0 / 12) / 4), (float)-Math.Sqrt(3) / 6),
                new Vector3(0, (float)-(Math.Sqrt(8.0 / 12) / 4), (float)Math.Sqrt(3) / 3),
                new Vector3(0, (float)Math.Sqrt(8.0f / 12) * 3.0f / 4, 0)
            };

            _normals = new[]
            {
                -Vector3.Cross(_vertices[2] - _vertices[0], _vertices[1] - _vertices[0]),
                -Vector3.Cross(_vertices[0] - _vertices[3], _vertices[1] - _vertices[3]),
                -Vector3.Cross(_vertices[2] - _vertices[3], _vertices[0] - _vertices[3]),
                -Vector3.Cross(_vertices[1] - _vertices[3], _vertices[2] - _vertices[3])
            };

        }

        /// <inheritdoc cref="VisualisationLesson"/>
        /// <summary>
        /// Отрисовывает тэтраэдр
        /// </summary>
        public override void Render()
        {
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Red);

            GL.Begin(PrimitiveType.Triangles);

            GL.Normal3(_normals[0]);
            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[2]);

            GL.Normal3(_normals[1]);
            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);

            GL.Normal3(_normals[2]);
            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[2]);

            GL.Normal3(_normals[3]);
            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[2]);
            GL.Vertex3(_vertices[1]);

            GL.End();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[2]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[2]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[0]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[2]);

            GL.End();
        }
    }
}