using OpenTK;

namespace LessonLibrary.Visualisation3D
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

        private readonly float _emptyCoef;

        public Plane(Vector3 normal, Vector3 point)
        {
            Normal = normal.Normalized();
            Point = point;
            _emptyCoef = -Vector3.Dot(Normal, Point);
        }

        public float Value(Vector3 point)
        {
            return Vector3.Dot(point, Normal) + _emptyCoef;
        }
    }
}