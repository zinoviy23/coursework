﻿using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary.Visualisation3D
{
    /// <summary>
    /// Класс для визуализации додэкаэдра
    /// </summary>
    /// <inheritdoc cref="VisualisationLesson"/>
    public sealed class DodecahedronVisualisation : VisualisationLesson
    {
        /// <summary>
        /// Иницилизирует додэкаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public DodecahedronVisualisation()
        {
            var icosahedronVertices = new IcosahedronVisualisation().VerticesClone;
            Vertices = new Vector3[20];

            for (var i = 0; i < 5; i++)
            {
                Vertices[i] = (icosahedronVertices[i] + icosahedronVertices[(i + 1) % 5] + icosahedronVertices[5]) / 3;
            }

            for (var i = 0; i < 5; i++)
            {
                Vertices[5 + i] = (icosahedronVertices[i] + icosahedronVertices[(i + 1) % 5] +
                                   icosahedronVertices[i + 6]) / 3;
            }

            for (var i = 0; i < 5; i++)
            {
                Vertices[i + 10] = (icosahedronVertices[(i + 6)] + icosahedronVertices[(i + 1) % 5] +
                                    icosahedronVertices[(i + 1) % 5 + 6]) / 3;
            }

            for (var i = 0; i < 5; i++)
            {
                Vertices[i + 15] = (icosahedronVertices[11] + icosahedronVertices[i + 6] +
                                    icosahedronVertices[(i + 1) % 5 + 6]) / 3;
            }

            var delta = 1.0f / (Vertices[0] - Vertices[1]).Length;

            for (var i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] *= delta;
            }

            Normals = new[]
            {
                Vector3.Cross(Vertices[0] - Vertices[1], Vertices[2] - Vertices[1]), 

                Vector3.Cross(Vertices[6] - Vertices[1], Vertices[0] - Vertices[1]),
                Vector3.Cross(Vertices[7] - Vertices[2], Vertices[1] - Vertices[2]),
                Vector3.Cross(Vertices[8] - Vertices[3], Vertices[2] - Vertices[3]),
                Vector3.Cross(Vertices[9] - Vertices[4], Vertices[3] - Vertices[4]),
                Vector3.Cross(Vertices[5] - Vertices[0], Vertices[4] - Vertices[0]),

                Vector3.Cross(Vertices[17] - Vertices[16], Vertices[15] - Vertices[16]), 

                Vector3.Cross(Vertices[10] - Vertices[15], Vertices[16] - Vertices[15]),
                Vector3.Cross(Vertices[11] - Vertices[16], Vertices[17] - Vertices[16]),
                Vector3.Cross(Vertices[12] - Vertices[17], Vertices[18] - Vertices[17]),
                Vector3.Cross(Vertices[13] - Vertices[18], Vertices[19] - Vertices[18]),
                Vector3.Cross(Vertices[14] - Vertices[19], Vertices[15] - Vertices[19]),
            };
            InitVertices = VerticesClone;
            InitNormals = (Vector3[])Normals.Clone();
            PrevVertices = (Vector3[]) InitVertices.Clone();
            PrevNormals = (Vector3[]) InitNormals.Clone();

            InitFaces();
            InitEdgesByFaces();
        }

        /// <summary>
        /// Рисует додэкаэдр
        /// </summary>
        /// <inheritdoc cref="VisualisationLesson"/>
        public override void Render()
        {
            Transform.SetTransform();

            ApplyCurrentAnimationInRender();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.SkyBlue);

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[0]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[4]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[1]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[10]);
            GL.Vertex3(Vertices[5]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[2]);

            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[6]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[3]);

            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[12]);
            GL.Vertex3(Vertices[7]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[4]);

            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[13]);
            GL.Vertex3(Vertices[8]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[5]);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[14]);
            GL.Vertex3(Vertices[9]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[6]);

            GL.Vertex3(Vertices[15]);
            GL.Vertex3(Vertices[16]);
            GL.Vertex3(Vertices[17]);
            GL.Vertex3(Vertices[18]);
            GL.Vertex3(Vertices[19]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[7]);

            GL.Vertex3(Vertices[15]);
            GL.Vertex3(Vertices[16]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[10]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[8]);

            GL.Vertex3(Vertices[16]);
            GL.Vertex3(Vertices[17]);
            GL.Vertex3(Vertices[12]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[11]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[9]);

            GL.Vertex3(Vertices[17]);
            GL.Vertex3(Vertices[18]);
            GL.Vertex3(Vertices[13]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[12]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[10]);

            GL.Vertex3(Vertices[18]);
            GL.Vertex3(Vertices[19]);
            GL.Vertex3(Vertices[14]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[13]);

            GL.End();

            GL.Begin(PrimitiveType.Polygon);

            GL.Normal3(Normals[11]);

            GL.Vertex3(Vertices[19]);
            GL.Vertex3(Vertices[15]);
            GL.Vertex3(Vertices[10]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[14]);

            GL.End();

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[0]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[4]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[1]);

            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[10]);
            GL.Vertex3(Vertices[5]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[2]);

            GL.Vertex3(Vertices[1]);
            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[6]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[3]);

            GL.Vertex3(Vertices[2]);
            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[12]);
            GL.Vertex3(Vertices[7]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[4]);

            GL.Vertex3(Vertices[3]);
            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[13]);
            GL.Vertex3(Vertices[8]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[5]);

            GL.Vertex3(Vertices[4]);
            GL.Vertex3(Vertices[0]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[14]);
            GL.Vertex3(Vertices[9]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[6]);

            GL.Vertex3(Vertices[15]);
            GL.Vertex3(Vertices[16]);
            GL.Vertex3(Vertices[17]);
            GL.Vertex3(Vertices[18]);
            GL.Vertex3(Vertices[19]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[7]);

            GL.Vertex3(Vertices[15]);
            GL.Vertex3(Vertices[16]);
            GL.Vertex3(Vertices[11]);
            GL.Vertex3(Vertices[6]);
            GL.Vertex3(Vertices[10]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[8]);

            GL.Vertex3(Vertices[16]);
            GL.Vertex3(Vertices[17]);
            GL.Vertex3(Vertices[12]);
            GL.Vertex3(Vertices[7]);
            GL.Vertex3(Vertices[11]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[9]);

            GL.Vertex3(Vertices[17]);
            GL.Vertex3(Vertices[18]);
            GL.Vertex3(Vertices[13]);
            GL.Vertex3(Vertices[8]);
            GL.Vertex3(Vertices[12]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[10]);

            GL.Vertex3(Vertices[18]);
            GL.Vertex3(Vertices[19]);
            GL.Vertex3(Vertices[14]);
            GL.Vertex3(Vertices[9]);
            GL.Vertex3(Vertices[13]);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            GL.Normal3(Normals[11]);

            GL.Vertex3(Vertices[19]);
            GL.Vertex3(Vertices[15]);
            GL.Vertex3(Vertices[10]);
            GL.Vertex3(Vertices[5]);
            GL.Vertex3(Vertices[14]);

            GL.End();

            DrawVertices();

            Transform.UnsetTransform();
        }

        protected override void InitFaces()
        {
            Faces = new[]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2], Vertices[3], Vertices[4]), 
                new Face(Vertices[0], Vertices[1], Vertices[6], Vertices[10], Vertices[5]), 
                new Face(Vertices[1], Vertices[2], Vertices[7], Vertices[11], Vertices[6]), 
                new Face(Vertices[2], Vertices[3], Vertices[8], Vertices[12], Vertices[7]), 
                new Face(Vertices[3], Vertices[4], Vertices[9], Vertices[13], Vertices[8]), 
                new Face(Vertices[4], Vertices[0], Vertices[5], Vertices[14], Vertices[9]),
                new Face(Vertices[15], Vertices[16], Vertices[17], Vertices[18], Vertices[19] ), 
                new Face(Vertices[15], Vertices[16], Vertices[11], Vertices[6], Vertices[10]), 
                new Face(Vertices[16], Vertices[17], Vertices[12], Vertices[7], Vertices[11]), 
                new Face(Vertices[17], Vertices[18], Vertices[13], Vertices[8], Vertices[12]), 
                new Face(Vertices[18], Vertices[19], Vertices[14], Vertices[9], Vertices[13]), 
                new Face(Vertices[19], Vertices[15], Vertices[10], Vertices[5], Vertices[14]), 
            };
        }

        public override string UserTutorialHtmlCode =>
            @"<p>
            Повороты <b>додекаэдра</b> составляют группу мощности 60, так как <b>додекаэдр</b> изоморфен <b>икосаэдру</b>.
          </p>
          <p style=""display: inline;"">
            Например, <div style=""display: inline;"" id=""rotation"">1</div> и 
            <div style=""display: inline;"" id=""rotation"">4</div> обратные друг другу.
            Или <div style=""display: inline;"" id=""rotation"">25</div> и 
            <div style=""display: inline;"" id=""rotation"">26</div> обратные друг другу.
          </p>
          <p style=""display: inline;"">
            А <div style=""display: inline;"" id=""rotation"">45</div> обратен сам себе.
          </p>
          <p style=""display: inline;"">
            Рассмотрим теперь композиции поворотов. Например, <div style=""display: inline;"" id=""rotation"">3</div>
            умножить на <div style=""display: inline;"" id=""rotation"">58</div> это 
            <div style=""display: inline;"" id=""rotation"">57</div>.
            А если применить эти повороты наоборот, то получиться <div style=""display: inline;"" id=""rotation"">59</div>.
          </p>";
    }
}