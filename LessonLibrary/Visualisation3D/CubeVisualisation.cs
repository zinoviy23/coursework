﻿using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary.Visualisation3D
{
    /// <inheritdoc cref="VisualisationLesson"/>
    /// <summary>
    /// Класс для визуализации куба
    /// </summary>
    public sealed class CubeVisualisation : VisualisationLesson
    {
        /// <summary>
        /// Иницилизирует куб
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public CubeVisualisation()
        {
            Vertices = new[]
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

            Normals = new[]
            {
                Vector3.Cross(Vertices[3] - Vertices[0], Vertices[1] - Vertices[0]), // низ
                Vector3.Cross(Vertices[4] - Vertices[0], Vertices[3] - Vertices[0]), // право
                Vector3.Cross(Vertices[2] - Vertices[1], Vertices[5] - Vertices[1]), // лево
                Vector3.Cross(Vertices[5] - Vertices[4], Vertices[7] - Vertices[4]), // верх 
                Vector3.Cross(Vertices[1] - Vertices[0], Vertices[4] - Vertices[0]), // перед
                Vector3.Cross(Vertices[2] - Vertices[6], Vertices[7] - Vertices[6]), // зад 
            };

            InitVertices = VerticesClone;
            InitNormals = (Vector3[]) Normals.Clone();
            PrevVertices = (Vector3[]) InitVertices.Clone();
            PrevNormals = (Vector3[]) InitNormals.Clone();

            InitFaces();
            InitEdgesByFaces();
        }

        /// <summary>
        /// Рисует куб
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public override void Render()
        {
            Transform.SetTransform();

            ApplyCurrentAnimationInRender();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Aqua);

            GL.Begin(PrimitiveType.Triangles);
            
            // нижняя грань
            GL.Normal3(Normals[0]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[2]);

            //правая грань
            GL.Normal3(Normals[1]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[7]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[7]);

            //левая грань
            GL.Normal3(Normals[2]);

            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[6]);

            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[6]);

            //Верхняя грань
            GL.Normal3(Normals[3]);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[6]);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[6]);

            // Передняя грань
            GL.Normal3(Normals[4]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[5]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[5]);

            // задняя грань
            GL.Normal3(Normals[5]);

            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[6]);

            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[6]);

            GL.End();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[7]);

            GL.End();

            GL.Begin(PrimitiveType.Lines);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[4]);

            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[5]);

            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[6]);

            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[7]);

            GL.End();

            DrawVertices();

            Transform.UnsetTransform();
        }

        protected override void InitFaces()
        {
            Faces = new[]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2], Vertices[3]),
                new Face(Vertices[0], Vertices[3], Vertices[7], Vertices[4]), 
                new Face(Vertices[1], Vertices[2], Vertices[6], Vertices[5]),
                new Face(Vertices[4], Vertices[5], Vertices[6], Vertices[7]),
                new Face(Vertices[0], Vertices[1], Vertices[5], Vertices[4]),
                new Face(Vertices[3], Vertices[7], Vertices[6], Vertices[2])
            };
        }

        public override string UserTutorialHtmlCode =>
            @"<p>
            Повороты <b>куба</b> составляют группу мощности 24.
          </p>
          <p style=""display: inline;"">
            Например, <div style=""display: inline;"" id=""rotation"">1</div>
            и <div style=""display: inline;"" id=""rotation"">3</div>
            обратные друг другу. Или <div style=""display: inline;"" id=""rotation"">10</div>
            и <div style=""display: inline;"" id=""rotation"">11</div> обратные друг другу.
          </p>
          <p style=""display: inline;"">
            A <div style=""display: inline;"" id=""rotation"">18</div> обратен сам себе.
          </p>
          <p style=""display: inline;"">
            Рассмотрим теперь композиции. Например, <div style=""display: inline;"" id=""rotation"">23</div>
            умножить на <div style=""display: inline;"" id=""rotation"">13</div> 
            это <div style=""display: inline;"" id=""rotation"">3</div>.
            А если применить эти повороты наоборот получится <div style=""display: inline;"" id=""rotation"">4</div>.
          </p>";
    }
}