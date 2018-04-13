using OpenTK;

namespace LessonLibrary.Visualisation3D
{
    /// <summary>
    /// Информация о мире
    /// </summary>
    public static class WorldInfo
    {
        /// <summary>
        /// Матрица проекции
        /// </summary>
        public static Matrix4 ProjectionMatrix { get; set; }

        /// <summary>
        /// Матрица вида
        /// </summary>
        public static Matrix4 ViewMatrix { get; set; }
    }
}