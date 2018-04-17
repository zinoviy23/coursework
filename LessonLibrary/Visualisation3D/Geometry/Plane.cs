using OpenTK;

namespace LessonLibrary.Visualisation3D.Geometry
{
    /// <summary>
    /// Класс для представления плоскости
    /// </summary>
    public struct Plane
    {
        /// <summary>
        /// Нормаль
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Точка, которая лежит
        /// </summary>
        public Vector3 Point;

        /// <summary>
        /// Свободный коэффициент в уравнении плоскости
        /// </summary>
        private readonly float _emptyCoef;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="normal">нормаль плоскости</param>
        /// <param name="point">точка на плоскости</param>
        public Plane(Vector3 normal, Vector3 point)
        {
            Normal = normal.Normalized();
            Point = point;
            _emptyCoef = -Vector3.Dot(Normal, Point);
        }

        /// <summary>
        /// Подставляет точку в уравнение плоскости. Результат равен расстоянию со знаком
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns>Расстояние со знаком до плоскости</returns>
        public float Value(Vector3 point)
        {
            return Vector3.Dot(point, Normal) + _emptyCoef;
        }
    }
}