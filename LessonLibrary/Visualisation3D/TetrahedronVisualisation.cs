using System;
using LessonLibrary.Visualisation3D.Animations;
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
    public class TetrahedronVisualisation : VisualisationLesson
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

            Normals = new[]
            {
                -Vector3.Cross(Vertices[2] - Vertices[0], Vertices[1] - Vertices[0]),
                -Vector3.Cross(Vertices[0] - Vertices[3], Vertices[1] - Vertices[3]),
                -Vector3.Cross(Vertices[2] - Vertices[3], Vertices[0] - Vertices[3]),
                -Vector3.Cross(Vertices[1] - Vertices[3], Vertices[2] - Vertices[3])
            };

            InitVertices = VerticesClone;
            //CurrentAnimation = new RotationAnimation(MathHelper.Pi / 3 * 2, Vector3.UnitY, MathHelper.Pi / 4);
            //CurrentAnimation = new SymmetryAnimation(new Plane(Vector3.UnitX, Vertices[2]), 3f);
        }

        /// <inheritdoc cref="VisualisationLesson"/>
        /// <summary>
        /// Отрисовывает тэтраэдр
        /// </summary>
        public override void Render()
        {
            Transform.SetTransform();

            UpdateCurrentAnimationInRender();

            //CurrentAnimation?.NextStep(0.01f);

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
    }
}