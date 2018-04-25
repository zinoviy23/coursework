using System;
using OpenTK;

namespace LessonLibrary.Visualisation3D.Geometry
{
    /// <summary>
    /// Класс для методов работы с векторами
    /// </summary>
    public static class VectorUtils
    {
        /// <summary>
        /// Сравнивает 2 вектора на равенство с погрешностью
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>true, если вектора равны</returns>
        public static bool AreVectorsEquals(Vector3 a, Vector3 b)
        {
            const float comparationError = 0.001f;
            return Math.Abs(a.X - b.X) < comparationError && Math.Abs(a.Y - b.Y) < comparationError &&
                   Math.Abs(a.Z - b.Z) < comparationError;
        }
    }
}