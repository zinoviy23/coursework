using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace LessonLibrary.Visualisation3D.Geometry
{
    /// <summary>
    /// Класс для представления грани. (Точки могут быть не на одной плоскости)
    /// </summary>
    public class Face
    {
        /// <summary>
        /// Вершины
        /// </summary>
        private readonly List<Vector3> _vertices;

        /// <summary>
        /// Конструктор для грани. Принимает список вершин(не меньше 3)
        /// </summary>
        /// <param name="first">Первая вершина</param>
        /// <param name="second">Вторая вершина</param>
        /// <param name="third">Третья вершина</param>
        /// <param name="vertices">Остальные вершины</param>
        public Face(Vector3 first, Vector3 second, Vector3 third,  params Vector3[] vertices)
        {
            _vertices = new List<Vector3> {first, second, third};
            _vertices.AddRange(vertices);

            // поиск середины грани

            var centerX = _vertices.Select(vec => vec.X).Aggregate(0f, (f, f1) => f + f1) / _vertices.Count;
            var centerY = _vertices.Select(vec => vec.Y).Aggregate(0f, (f, f1) => f + f1) / _vertices.Count;
            var centerZ = _vertices.Select(vec => vec.Z).Aggregate(0f, (f, f1) => f + f1) / _vertices.Count;

            Center = new Vector3(centerX, centerY, centerZ);
        }

        /// <summary>
        /// Конструктор от коллекции
        /// </summary>
        /// <param name="vertices">Вершины</param>
        /// <exception cref="ArgumentException"></exception>
        public Face(ICollection<Vector3> vertices)
        {
            if (vertices.Count < 3)
                throw new ArgumentException("Кол-во вершин должно быть хотя бы 3.");

            _vertices = new List<Vector3>(vertices.ToArray());

            // поиск середины грани

            var centerX = _vertices.Select(vec => vec.X).Aggregate(0f, (f, f1) => f + f1) / _vertices.Count;
            var centerY = _vertices.Select(vec => vec.Y).Aggregate(0f, (f, f1) => f + f1) / _vertices.Count;
            var centerZ = _vertices.Select(vec => vec.Z).Aggregate(0f, (f, f1) => f + f1) / _vertices.Count;

            Center = new Vector3(centerX, centerY, centerZ);
        }

        /// <summary>
        /// Центр грани
        /// </summary>
        public Vector3 Center { get; }

        /// <summary>
        /// Количество вершин у грани
        /// </summary>
        public int Count => _vertices.Count;

        /// <summary>
        /// Возвращает вершину по её индексу
        /// </summary>
        /// <param name="index">Индекс вершины</param>
        /// <returns>Вершина</returns>
        public Vector3 this[int index] => _vertices[index];

        /// <summary>
        /// Получает ребра грани
        /// </summary>
        public Tuple<Vector3, Vector3>[] Edges
        {
            get
            {
                var res = new Tuple<Vector3, Vector3>[Count];
                for (var i = 0; i < Count; i++)
                {
                    res[i] = new Tuple<Vector3, Vector3>(_vertices[i], _vertices[(i + 1) % Count]);
                }

                return res;
            }
        }
    }
}