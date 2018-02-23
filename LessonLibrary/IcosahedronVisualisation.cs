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

            _normals = new[]
            {
                Vector3.Cross(_verticies[1] - _verticies[5], _verticies[0] - _verticies[5]), // верхняя пирамида
                Vector3.Cross(_verticies[2] - _verticies[5], _verticies[1] - _verticies[5]), 
                Vector3.Cross(_verticies[3] - _verticies[5], _verticies[2] - _verticies[5]),
                Vector3.Cross(_verticies[4] - _verticies[5], _verticies[3] - _verticies[5]),
                Vector3.Cross(_verticies[0] - _verticies[5], _verticies[4] - _verticies[5]),

                Vector3.Cross(_verticies[0] - _verticies[6], _verticies[1] - _verticies[6]),  // верхний уровень 
                Vector3.Cross(_verticies[1] - _verticies[7], _verticies[2] - _verticies[7]), 
                Vector3.Cross(_verticies[2] - _verticies[8], _verticies[3] - _verticies[8]),
                Vector3.Cross(_verticies[3] - _verticies[9], _verticies[4] - _verticies[9]),
                Vector3.Cross(_verticies[4] - _verticies[10], _verticies[0] - _verticies[10]), 

                Vector3.Cross(_verticies[7] - _verticies[1], _verticies[6] - _verticies[1]), // нижний уровень
                Vector3.Cross(_verticies[8] - _verticies[2], _verticies[7] - _verticies[2]),
                Vector3.Cross(_verticies[9] - _verticies[3], _verticies[8] - _verticies[3]),
                Vector3.Cross(_verticies[10] - _verticies[4], _verticies[9] - _verticies[4]),
                Vector3.Cross(_verticies[6] - _verticies[0], _verticies[10] - _verticies[0]), 
                
                Vector3.Cross(_verticies[6] - _verticies[11], _verticies[7] - _verticies[11]), // нижняя пирамида
                Vector3.Cross(_verticies[7] - _verticies[11], _verticies[8] - _verticies[11]),
                Vector3.Cross(_verticies[8] - _verticies[11], _verticies[9] - _verticies[11]),
                Vector3.Cross(_verticies[9] - _verticies[11], _verticies[10] - _verticies[11]),
                Vector3.Cross(_verticies[10] - _verticies[11], _verticies[6] - _verticies[11]),
            };
        }

        /// <summary>
        /// Рисует икосаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public override void Render()
        {
            Transform.SetTransform();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Red);

            GL.Begin(PrimitiveType.Triangles);

            GL.Normal3(_normals[0]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[1]);

            GL.Normal3(_normals[1]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[2]);

            GL.Normal3(_normals[2]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[3]);

            GL.Normal3(_normals[3]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[4]);
            GL.Vertex3(_verticies[3]);

            GL.Normal3(_normals[4]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[4]);
            GL.Vertex3(_verticies[0]);
            
            GL.Normal3(_normals[5]);
            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[6]);

            GL.Normal3(_normals[6]);
            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[7]);

            GL.Normal3(_normals[7]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[3]);
            GL.Vertex3(_verticies[8]);

            GL.Normal3(_normals[8]);
            GL.Vertex3(_verticies[3]);
            GL.Vertex3(_verticies[4]);
            GL.Vertex3(_verticies[9]);

            GL.Normal3(_normals[9]);
            GL.Vertex3(_verticies[4]);
            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[10]);

            GL.Normal3(_normals[10]);
            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[6]);
            GL.Vertex3(_verticies[7]);

            GL.Normal3(_normals[11]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[7]);
            GL.Vertex3(_verticies[8]);

            GL.Normal3(_normals[12]);
            GL.Vertex3(_verticies[3]);
            GL.Vertex3(_verticies[8]);
            GL.Vertex3(_verticies[9]);

            GL.Normal3(_normals[13]);
            GL.Vertex3(_verticies[4]);
            GL.Vertex3(_verticies[9]);
            GL.Vertex3(_verticies[10]);

            GL.Normal3(_normals[14]);
            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[10]);
            GL.Vertex3(_verticies[6]);

            GL.Normal3(_normals[15]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[6]);
            GL.Vertex3(_verticies[7]);

            GL.Normal3(_normals[16]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[7]);
            GL.Vertex3(_verticies[8]);

            GL.Normal3(_normals[17]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[8]);
            GL.Vertex3(_verticies[9]);

            GL.Normal3(_normals[18]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[9]);
            GL.Vertex3(_verticies[10]);

            GL.Normal3(_normals[19]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[10]);
            GL.Vertex3(_verticies[6]);

            GL.End();
            
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[3]);
            GL.Vertex3(_verticies[4]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_verticies[6]);
            GL.Vertex3(_verticies[7]);
            GL.Vertex3(_verticies[8]);
            GL.Vertex3(_verticies[9]);
            GL.Vertex3(_verticies[10]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[6]);
            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[7]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[8]);
            GL.Vertex3(_verticies[3]);
            GL.Vertex3(_verticies[9]);
            GL.Vertex3(_verticies[4]);
            GL.Vertex3(_verticies[10]);

            GL.End();

            GL.Begin(PrimitiveType.Lines);

            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[0]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[1]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[2]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[3]);
            GL.Vertex3(_verticies[5]);
            GL.Vertex3(_verticies[4]);

            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[6]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[7]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[8]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[9]);
            GL.Vertex3(_verticies[11]);
            GL.Vertex3(_verticies[10]);

            GL.End();

            Transform.UnsetTransform();
        }
    }
}