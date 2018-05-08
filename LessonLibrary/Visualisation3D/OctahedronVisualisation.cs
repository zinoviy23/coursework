using System;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary.Visualisation3D
{
    /// <summary>
    /// Класс для визуализации октаэдра
    /// </summary>
    /// <inheritdoc cref="VisualisationLesson"/>
    public sealed class OctahedronVisualisation : VisualisationLesson
    {
        /// <summary>
        /// Иницилизирует октаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public OctahedronVisualisation()
        {
            Vertices = new[]
            {
                new Vector3(-0.5f, 0.0f, 0.5f),
                new Vector3(0.5f, 0.0f, 0.5f),
                new Vector3(0.5f, 0.0f, -0.5f),
                new Vector3(-0.5f, 0.0f, -0.5f),
                new Vector3(0.0f, (float)Math.Sqrt(2) / 2.0f, 0.0f),
                new Vector3(0.0f, -(float)Math.Sqrt(2) / 2.0f, 0.0f)
            };

            Normals = new[]
            {
                Vector3.Cross(Vertices[0] - Vertices[4], Vertices[1] - Vertices[4]),
                Vector3.Cross(Vertices[1] - Vertices[4], Vertices[2] - Vertices[4]),
                Vector3.Cross(Vertices[2] - Vertices[4], Vertices[3] - Vertices[4]), 
                Vector3.Cross(Vertices[3] - Vertices[4], Vertices[0] - Vertices[4]),
                Vector3.Cross(Vertices[1] - Vertices[5], Vertices[0] - Vertices[5]),
                Vector3.Cross(Vertices[2] - Vertices[5], Vertices[1] - Vertices[5]), 
                Vector3.Cross(Vertices[3] - Vertices[5], Vertices[2] - Vertices[5]),
                Vector3.Cross(Vertices[0] - Vertices[5], Vertices[3] - Vertices[5]), 
            };

            InitVertices = VerticesClone;
            InitNormals = (Vector3[]) Normals.Clone();
            PrevVertices = (Vector3[]) InitVertices.Clone();
            PrevNormals = (Vector3[]) InitNormals.Clone();

            InitFaces();
            InitEdgesByFaces();
        }

        /// <summary>
        /// Отрисовывает октаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public override void Render()
        {
            Transform.SetTransform();

            ApplyCurrentAnimationInRender();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.LawnGreen);

            GL.Begin(PrimitiveType.Triangles);

            GL.Normal3(Normals[0]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[4]);

            GL.Normal3(Normals[1]);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);

            GL.Normal3(Normals[2]);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[2]);

            GL.Normal3(Normals[3]);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[0]);

            GL.Normal3(Normals[4]);

            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);

            GL.Normal3(Normals[5]);
            
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[1]);

            GL.Normal3(Normals[6]);

            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);

            GL.Normal3(Normals[7]);

            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[0]);

            GL.End();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[2]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[3]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);

            GL.End();

            DrawVertices();

            Transform.UnsetTransform();
        }

        protected override void InitFaces()
        {
            Faces = new[]
            {
                new Face(Vertices[0], Vertices[1], Vertices[4]),
                new Face(Vertices[4], Vertices[1], Vertices[2]), 
                new Face(Vertices[4], Vertices[3], Vertices[2]), 
                new Face(Vertices[4], Vertices[3], Vertices[0]), 
                new Face(Vertices[5], Vertices[0], Vertices[1]), 
                new Face(Vertices[5], Vertices[2], Vertices[1]), 
                new Face(Vertices[5], Vertices[2], Vertices[3]),
                new Face(Vertices[5], Vertices[3], Vertices[0])
            };
        }

        public override string UserTutorialHtmlCode =>
            @"   <p>
            Повороты <b>октаэдра</b> составляют группу мощности 24, так как <b>октаэдр</b> изоморфен <b>кубу</b>.
          </p>
          <p style=""display: inline;"">
            Например, <div style=""display: inline;"" id=""rotation"">1</div>
            и <div style=""display: inline;"" id=""rotation"">3</div> обратные друг другу.
            Или <div style=""display: inline;"" id=""rotation"">16</div> и 
            <div style=""display: inline;"" id=""rotation"">17</div> обратные друг другу.
          </p>
          <p style=""display: inline;"">
            А <div style=""display: inline;"" id=""rotation"">18</div> обратен сам себе.
          </p>
          <p style=""display: inline;"">
            Рассмотрим теперь композиции поворотов. Например, <div style=""display: inline;"" id=""rotation"">4</div>
            умножить на <div style=""display: inline;"" id=""rotation"">18</div> это 
            <div style=""display: inline;"" id=""rotation"">8</div>.
            А если применить эти повороты наоборот, то получиться
            <div style=""display: inline;"" id=""rotation"">2</div>.
          </p>";
    }
}