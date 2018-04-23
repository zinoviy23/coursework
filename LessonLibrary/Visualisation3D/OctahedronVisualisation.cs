using System;
using LessonLibrary.Visualisation3D.Animations;
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
    public class OctahedronVisualisation : VisualisationLesson
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

            CurrentAnimation = new SymmetryAnimation(new Plane(Vector3.UnitY, Vector3.Zero), 1f);
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
    }
}