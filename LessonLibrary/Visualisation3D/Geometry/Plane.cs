using System;
using System.Runtime.Serialization;
using OpenTK;

namespace LessonLibrary.Visualisation3D.Geometry
{
    /// <summary>
    /// Класс для представления плоскости
    /// </summary>
    [DataContract]
    public struct Plane : IEquatable<Plane>
    {
        /// <summary>
        /// Нормаль
        /// </summary>
        [DataMember]
        public Vector3 Normal;

        /// <summary>
        /// Точка, которая лежит
        /// </summary>
        [DataMember]
        public Vector3 Point;

        /// <summary>
        /// Свободный коэффициент в уравнении плоскости
        /// </summary>
        [DataMember]
        private float _emptyCoef;

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

        [OnDeserialized]
        public void Normalize()
        {
            Normal = Normal.Normalized();
            _emptyCoef = -Vector3.Dot(Normal, Point);
        }

        public bool Equals(Plane other)
        {
            const float tolerance = 0.001f;
            return VectorUtils.AreVectorsEquals(Normal, other.Normal)
                   && Math.Abs(_emptyCoef - other._emptyCoef) < tolerance;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Plane plane && Equals(plane);
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}