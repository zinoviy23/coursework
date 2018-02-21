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
    /// Класс для визуализации икосаэдра
    /// </summary>
    /// <inheritdoc cref="VisualisationLesson"/>
    public class IcosahedronVisualisation : VisualisationLesson
    {
        private readonly Vector3[] _verticies;
        private readonly Vector3[] _normals;

        /// <summary>
        /// Иницилизирует икосаэдр
        /// </summary>
        public IcosahedronVisualisation()
        {
            _verticies = new Vector3[12];

            for (var i = 0; i < 5; i++)
            {
                var angle = 2 * (i + 3)  / 5d * Math.PI;
                _verticies[i] = new Vector3((float)Math.Cos(angle), (float)Math.Sqrt(3) / 4, (float)Math.Sin(angle));
            }

            var height = (float) Math.Sqrt(1 - 1.0f / (4 * Math.Pow(Math.Sin(36 * Math.PI / 180), 2)));
            _verticies[5] = new Vector3(0, (float)Math.Sqrt(3) / 4 + height, 0);

            for (var i = 0; i < 5; i++)
            {
                var angle = (360d * (i + 3) / 5 + 36) * Math.PI / 180;
                _verticies[i + 6] = new Vector3((float)Math.Cos(angle), -(float)Math.Sqrt(3) / 4, (float)Math.Sin(angle));
            }

            _verticies[11] = new Vector3(0, -(float)Math.Sqrt(3) / 4 - height, 0);
        }

        /// <summary>
        /// Рисует икосаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public override void Render()
        {
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Red);

            GL.Begin(PrimitiveType.Triangles);

            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[1]);


            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[3]);

            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[6]);

            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[7]);

            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[6]);
            GL.Vertex3(_verticies[7]);

            GL.End();
        }
    }
}