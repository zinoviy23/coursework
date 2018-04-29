using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
        public static bool AreVectorsEqual(Vector3 a, Vector3 b)
        {
            const float comparationError = 0.001f;
            return Math.Abs(a.X - b.X) < comparationError && Math.Abs(a.Y - b.Y) < comparationError &&
                   Math.Abs(a.Z - b.Z) < comparationError;
        }

        /// <summary>
        /// Проверяет, есть ли пара векторов в последовательности пар
        /// </summary>
        /// <param name="vectorPairs">Последовательность пар</param>
        /// <param name="pair">Пара векторов</param>
        /// <returns>Принадлежит ли пара векторов последовательности</returns>
        public static bool IsVectorPairContainsInPairSequence(
            [ItemNotNull] [NotNull] IEnumerable<Tuple<Vector3, Vector3>> vectorPairs,
            [NotNull] Tuple<Vector3, Vector3> pair) => vectorPairs.Any(tuple =>
            AreVectorsEqual(tuple.Item1, pair.Item1) && AreVectorsEqual(tuple.Item2, pair.Item2));
    }
}