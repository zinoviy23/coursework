using OpenTK;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;


namespace LessonLibrary.Visualisation3D
{
    /// <summary>
    /// Информация об объекте
    /// </summary>
    public class VisualisationTransform
    {
        /// <summary>
        /// Положение в пространстве
        /// </summary>
        public Vector3 Position { get; } = new Vector3(0, 0, 0);

        /// <summary>
        /// Поворот в пространстве
        /// </summary>
        public Vector3 Rotation { get; } = new Vector3(0, 0, 0);

        /// <summary>
        /// Расширение в пространстве
        /// </summary>
        public Vector3 Scale { get; } = new Vector3(1, 1, 1);

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="rotation">Поворот</param>
        /// <param name="scale">Расширение</param>
        public VisualisationTransform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public VisualisationTransform()
        {
        }

        /// <summary>
        /// Задаёт положение объекту для отрисовки
        /// </summary>
        public void SetTransform()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(Rotation.X, 1, 0, 0);
            GL.Rotate(Rotation.Y, 0, 1, 0);
            GL.Rotate(Rotation.Z, 0, 0, 1);
        }

        /// <summary>
        /// Отменяет положение для отриски других объектов
        /// </summary>
        public void UnsetTransform()
        {
            GL.PopMatrix();
        }

        /// <summary>
        /// Матрица преобразования
        /// </summary>
        public Matrix4 Matrix
        {
            get
            {
                var xRot = Matrix4.CreateRotationX(Rotation.X / 180 * MathHelper.Pi);
                var yRot = Matrix4.CreateRotationY(Rotation.Y / 180 * MathHelper.Pi);
                var zRot = Matrix4.CreateRotationZ(Rotation.Z / 180 * MathHelper.Pi);
                var translate = Matrix4.CreateTranslation(Position);

                return zRot * yRot * xRot * translate;
            }
        }
    }
}