using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary.Visualisation3D
{
    /// <summary>
    /// Класс для визуализации икосаэдра
    /// </summary>
    /// <inheritdoc cref="VisualisationLesson"/>
    public class IcosahedronVisualisation : VisualisationLesson
    {
        /// <summary>
        /// Иницилизирует икосаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public IcosahedronVisualisation()
        {
            Vertices = new Vector3[12];

            for (var i = 0; i < 5; i++)
            {
                var angle = 2 * (i + 3)  / 5d * Math.PI;
                Vertices[i] = new Vector3((float)Math.Cos(angle), (float)Math.Sqrt(3) / 4, (float)Math.Sin(angle));
            }

            var height = (float) Math.Sqrt(1 - 1.0f / (4 * Math.Pow(Math.Sin(36 * Math.PI / 180), 2)));
            Vertices[5] = new Vector3(0, (float)Math.Sqrt(3) / 4 + height, 0);

            for (var i = 0; i < 5; i++)
            {
                var angle = (360d * (i + 3) / 5 + 36) * Math.PI / 180;
                Vertices[i + 6] = new Vector3((float)Math.Cos(angle), -(float)Math.Sqrt(3) / 4, (float)Math.Sin(angle));
            }

            Vertices[11] = new Vector3(0, -(float)Math.Sqrt(3) / 4 - height, 0);

            var delta = 1.0f / (Vertices[0] - Vertices[1]).Length;
            for (var i = 0; i < Vertices.Length; i++)
                Vertices[i] *= delta;

            Normals = new[]
            {
                Vector3.Cross(Vertices[1] - Vertices[5], Vertices[0] - Vertices[5]), // верхняя пирамида
                Vector3.Cross(Vertices[2] - Vertices[5], Vertices[1] - Vertices[5]), 
                Vector3.Cross(Vertices[3] - Vertices[5], Vertices[2] - Vertices[5]),
                Vector3.Cross(Vertices[4] - Vertices[5], Vertices[3] - Vertices[5]),
                Vector3.Cross(Vertices[0] - Vertices[5], Vertices[4] - Vertices[5]),

                Vector3.Cross(Vertices[0] - Vertices[6], Vertices[1] - Vertices[6]),  // верхний уровень 
                Vector3.Cross(Vertices[1] - Vertices[7], Vertices[2] - Vertices[7]), 
                Vector3.Cross(Vertices[2] - Vertices[8], Vertices[3] - Vertices[8]),
                Vector3.Cross(Vertices[3] - Vertices[9], Vertices[4] - Vertices[9]),
                Vector3.Cross(Vertices[4] - Vertices[10], Vertices[0] - Vertices[10]), 

                Vector3.Cross(Vertices[7] - Vertices[1], Vertices[6] - Vertices[1]), // нижний уровень
                Vector3.Cross(Vertices[8] - Vertices[2], Vertices[7] - Vertices[2]),
                Vector3.Cross(Vertices[9] - Vertices[3], Vertices[8] - Vertices[3]),
                Vector3.Cross(Vertices[10] - Vertices[4], Vertices[9] - Vertices[4]),
                Vector3.Cross(Vertices[6] - Vertices[0], Vertices[10] - Vertices[0]), 
                
                Vector3.Cross(Vertices[6] - Vertices[11], Vertices[7] - Vertices[11]), // нижняя пирамида
                Vector3.Cross(Vertices[7] - Vertices[11], Vertices[8] - Vertices[11]),
                Vector3.Cross(Vertices[8] - Vertices[11], Vertices[9] - Vertices[11]),
                Vector3.Cross(Vertices[9] - Vertices[11], Vertices[10] - Vertices[11]),
                Vector3.Cross(Vertices[10] - Vertices[11], Vertices[6] - Vertices[11]),
            };

            InitVertices = VerticesClone;
        }

        /// <summary>
        /// Рисует икосаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public override void Render()
        {
            Transform.SetTransform();

            UpdateCurrentAnimationInRender();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Orange);

            GL.Begin(PrimitiveType.Triangles);

            GL.Normal3(Normals[0]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);

            GL.Normal3(Normals[1]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);

            GL.Normal3(Normals[2]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);

            GL.Normal3(Normals[3]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[3]);

            GL.Normal3(Normals[4]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[0]);
            
            GL.Normal3(Normals[5]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[6]);

            GL.Normal3(Normals[6]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[7]);

            GL.Normal3(Normals[7]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[8]);

            GL.Normal3(Normals[8]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[9]);

            GL.Normal3(Normals[9]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[10]);

            GL.Normal3(Normals[10]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[7]);

            GL.Normal3(Normals[11]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[8]);

            GL.Normal3(Normals[12]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[9]);

            GL.Normal3(Normals[13]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[10]);

            GL.Normal3(Normals[14]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[10]);
            GL.Vertex3(Vertices[6]);

            GL.Normal3(Normals[15]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[7]);

            GL.Normal3(Normals[16]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[8]);

            GL.Normal3(Normals[17]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[9]);

            GL.Normal3(Normals[18]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[10]);

            GL.Normal3(Normals[19]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[10]);
            GL.Vertex3(Vertices[6]);

            GL.End();
            
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[4]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[10]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[10]);

            GL.End();

            GL.Begin(PrimitiveType.Lines);

            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[4]);

            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[10]);

            GL.End();

            DrawVertices();

            Transform.UnsetTransform();
        }
    }
}