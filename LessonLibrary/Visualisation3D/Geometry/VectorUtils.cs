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


        /// <summary>
        /// Возвращает лежит ли точка на прямой, заданной точкой, принадлежащей прямой, и направляющим вектором
        /// </summary>
        /// <param name="vertex">Точка</param>
        /// <param name="pointOnLine">Точка на прямой</param>
        /// <param name="direction">Направляющий вектор прямой</param>
        /// <returns>true, если принадлежит, и false иначе</returns>
        public static bool IsVertexOnLineByPointAndDirection(Vector3 vertex, Vector3 pointOnLine, Vector3 direction)
        {
            const float tolerance = 0.000001f;
            return Math.Abs((vertex.X - pointOnLine.X) * direction.Y - (vertex.Y - pointOnLine.Y) * direction.X) <
                   tolerance &&
                   Math.Abs((vertex.X - pointOnLine.X) * direction.Z - (vertex.Z - pointOnLine.Z) * direction.X) <
                   tolerance &&
                   Math.Abs((vertex.Y - pointOnLine.Y) * direction.Z - (vertex.Z - pointOnLine.Z) * direction.Y) <
                   tolerance;
        }

        /// <summary>
        /// Получает середину ребра
        /// </summary>
        /// <param name="edge">Ребро</param>
        /// <returns>Координаты середин</returns>
        public static Vector3 EdgeCenter([NotNull] Tuple<Vector3, Vector3> edge)
        {
            return edge.Item1 / 2 + edge.Item2 / 2;
        }

    }
}