﻿using System;
using System.Runtime.Serialization;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;

namespace LessonLibrary.Visualisation3D.Animations
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для анимации симметрии
    /// </summary>
    [DataContract(Name = "Symmetry")]
    public class SymmetryAnimation : IAnimation, IEquatable<SymmetryAnimation>
    {
        /// <summary>
        /// Ось симмертии
        /// </summary>
        [DataMember]
        public Plane Plane { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Скорость анимации
        /// </summary>
        [DataMember]
        public float Speed { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Возвращает завершилась ли анимация
        /// </summary>
        public bool IsFinish => Math.Abs(_currentCoef - (-1)) < 0.0001f;

        /// <summary>
        /// Текущий коэффициент применения анимации
        /// </summary>
        // ReSharper disable once MemberInitializerValueIgnored
        private float _currentCoef = 1;

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

        /// <inheritdoc />
        /// делает следующий шаг
        public void NextStep(float deltaTime)
        {
            _currentCoef -= deltaTime * Speed;
            if (_currentCoef < -1)
                _currentCoef = -1;
        }
        
        /// <inheritdoc />
        /// применяет анимацию к точке
        public Vector3 Apply(Vector3 vertex)
        {
            return vertex - Plane.Normal * Plane.Value(vertex) * (1 - _currentCoef);
        }

        /// <inheritdoc/>
        /// Сбрасывает анимацию
        public void Reset()
        {
            _currentCoef = 1;
        }

        public bool Equals(SymmetryAnimation other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            const float tolerance = 0.0001f;
            return Plane.Equals(other.Plane) && Math.Abs(Speed - other.Speed) < tolerance;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SymmetryAnimation) obj);
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}