using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для визуализации октаэдра
    /// </summary>
    /// <inheritdoc cref="VisualisationLesson"/>
    public class OctahedronVisualisation : VisualisationLesson
    {
        private readonly Vector3[] _vertices;
        private readonly Vector3[] _normals;

        /// <summary>
        /// Иницилизирует октаэдр
        /// </summary>
        public OctahedronVisualisation()
        {
            _vertices = new[]
            {
                new Vector3(-0.5f, 0.0f, 0.5f),
                new Vector3(0.5f, 0.0f, 0.5f),
                new Vector3(0.5f, 0.0f, -0.5f),
                new Vector3(-0.5f, 0.0f, -0.5f),
                new Vector3(0.0f, (float)Math.Sqrt(2) / 2.0f, 0.0f),
                new Vector3(0.0f, -(float)Math.Sqrt(2) / 2.0f, 0.0f)
            };

            _normals = new[]
            {
                Vector3.Cross(_vertices[0] - _vertices[4], _vertices[1] - _vertices[4]),
                Vector3.Cross(_vertices[1] - _vertices[4], _vertices[2] - _vertices[4]),
                Vector3.Cross(_vertices[2] - _vertices[4], _vertices[3] - _vertices[4]), 
                Vector3.Cross(_vertices[3] - _vertices[4], _vertices[0] - _vertices[4]),
                Vector3.Cross(_vertices[1] - _vertices[5], _vertices[0] - _vertices[5]),
                Vector3.Cross(_vertices[2] - _vertices[5], _vertices[1] - _vertices[5]), 
                Vector3.Cross(_vertices[3] - _vertices[5], _vertices[2] - _vertices[5]),
                Vector3.Cross(_vertices[0] - _vertices[5], _vertices[3] - _vertices[5]), 
            };
        }

        /// <summary>
        /// Отрисовывает октаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public override void Render()
        {
            Transform.SetTransform();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Red);

            GL.Begin(PrimitiveType.Triangles);

            GL.Normal3(_normals[0]);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[4]);

            GL.Normal3(_normals[1]);

            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[2]);

            GL.Normal3(_normals[2]);

            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[2]);

            GL.Normal3(_normals[3]);

            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[0]);

            GL.Normal3(_normals[4]);

            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);

            GL.Normal3(_normals[5]);
            
            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[2]);
            GL.Vertex3(_vertices[1]);

            GL.Normal3(_normals[6]);

            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[2]);
            GL.Vertex3(_vertices[3]);

            GL.Normal3(_normals[7]);

            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[3]);
            GL.Vertex3(_vertices[0]);

            GL.End();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[2]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[4]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[5]);
            GL.Vertex3(_vertices[3]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_vertices[0]);
            GL.Vertex3(_vertices[1]);
            GL.Vertex3(_vertices[2]);
            GL.Vertex3(_vertices[3]);

            GL.End();

            Transform.UnsetTransform();
        }
    }
}