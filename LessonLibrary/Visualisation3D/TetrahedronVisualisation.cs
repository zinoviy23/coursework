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
    /// <inheritdoc cref="VisualisationLesson"/>
    /// <summary>
    /// Класс для визуализации тэтраэдра
    /// </summary>
    public sealed class TetrahedronVisualisation : VisualisationLesson
    {
        /// <summary>
        /// Иницилизирует тэтраэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public TetrahedronVisualisation()
        {
            Vertices = new[]
            {
                new Vector3(-0.5f, (float)-(Math.Sqrt(8.0 / 12) / 4), (float)-Math.Sqrt(3) / 6),
                new Vector3(0.5f, (float)-(Math.Sqrt(8.0 / 12) / 4), (float)-Math.Sqrt(3) / 6),
                new Vector3(0, (float)-(Math.Sqrt(8.0 / 12) / 4), (float)Math.Sqrt(3) / 3),
                new Vector3(0, (float)Math.Sqrt(8.0f / 12) * 3.0f / 4, 0)
            };

            var delta = 1.3f;
            for (var i = 0; i < Vertices.Length; i++)
                Vertices[i] *= delta;

            Normals = new[]
            {
                -Vector3.Cross(Vertices[2] - Vertices[0], Vertices[1] - Vertices[0]),
                -Vector3.Cross(Vertices[0] - Vertices[3], Vertices[1] - Vertices[3]),
                -Vector3.Cross(Vertices[2] - Vertices[3], Vertices[0] - Vertices[3]),
                -Vector3.Cross(Vertices[1] - Vertices[3], Vertices[2] - Vertices[3])
            };

            InitVertices = VerticesClone;
            InitNormals = (Vector3[])Normals.Clone();
            PrevVertices = VerticesClone;
            PrevNormals = (Vector3[]) InitNormals.Clone();

            InitFaces();
            InitEdgesByFaces();
        }

        /// <inheritdoc cref="VisualisationLesson"/>
        /// <summary>
        /// Отрисовывает тэтраэдр
        /// </summary>
        public override void Render()
        {
            Transform.SetTransform();

            ApplyCurrentAnimationInRender();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.OrangeRed);

            GL.Begin(PrimitiveType.Triangles);

            GL.Normal3(Normals[0]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);

            GL.Normal3(Normals[1]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);

            GL.Normal3(Normals[2]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[2]);

            GL.Normal3(Normals[3]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[1]);

            GL.End();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[0]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[2]);

            GL.End();

            DrawVertices();

            Transform.UnsetTransform();
        }

        protected override void InitFaces()
        {
            Faces = new[]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2]),
                new Face(Vertices[3], Vertices[0], Vertices[1]), 
                new Face(Vertices[3], Vertices[0], Vertices[2]), 
                new Face(Vertices[3], Vertices[2], Vertices[1]) 
            };
        }

        public override string UserTutorialHtmlCode =>
            @"<p>
                Повороты <b>тетраэдра</b> составляют группу мощности 12.
            </p>
            <p style=""display: inline"">
                Например, <div style=""display: inline"" id=""rotation"">1</div >
                и <div style=""display: inline"" id=""rotation"">2</div >
                обратные друг другу.
            </p>
            <p style=""display: inline"">
                А<div style=""display: inline"" id=""rotation"">11</div >
                обратен сам себе.
            </p>
            <p style=""display: inline"">
                Также <div style=""display: inline"" id=""rotation"">3</div >
                умножить на <div style=""display: inline"" id=""rotation"">10</div >
                это <div style=""display: inline"" id=""rotation"">5</div >.
                А если применить повороты наоборот получится <div style=""display: inline"" id=""rotation"">7</div >.
            </p>";
    }
}