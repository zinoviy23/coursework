using System;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;

namespace LessonLibrary.Visualisation3D.Animations
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для анимации симметрии
    /// </summary>
    public class SymmetryAnimation : IAnimation
    {
        /// <summary>
        /// Ось симмертии
        /// </summary>
        public Plane Plane { get; }

        public float Speed { get; }

        public bool IsFinish => Math.Abs(_currentCoef - (-1)) < 0.0001f;

        /// <summary>
        /// Текущий коэффициент применения анимации
        /// </summary>
        private float _currentCoef;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="plane">Ось</param>
        /// <param name="speed">Скорость</param>
        public SymmetryAnimation(Plane plane, float speed)
        {
            Plane = plane;
            Speed = speed;
            _currentCoef = 1;
        }

        public void NextStep(float deltaTime)
        {
            _currentCoef -= deltaTime * Speed;
            if (_currentCoef < -1)
                _currentCoef = -1;
        }

        public Vector3 Apply(Vector3 vertex)
        {
            return vertex - Plane.Normal * Plane.Value(vertex) * (1 - _currentCoef);
        }

        public void Reset()
        {
            _currentCoef = 1;
        }
    }
}