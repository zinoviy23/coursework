using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;

namespace LessonLibrary.Visualisation3D.Animations
{
    /// <inheritdoc cref="IAnimation" />
    /// <summary>
    /// Класс для анимации поворота
    /// </summary>
    [DataContract(Name = "Rotation")]
    public class RotationAnimation : IAnimation, IEquatable<RotationAnimation>
    {
        /// <summary>
        /// Возвращает угол, на который нужно повернуть
        /// </summary>
        [DataMember]
        public float Angle { get; private set; }

        /// <summary>
        /// Возвращает ось, вокруг которой нужно повернуть
        /// </summary>
        [DataMember]
        public Vector3 Axis { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Возвращает скорость, с которой надо вращать
        /// </summary>
        [DataMember]
        public float Speed { get; private set; }

        // Возвращает закончилась ли анимация
        public bool IsFinish => Math.Abs(Angle - _currentAngle) < 0.0001f;

        /// <summary>
        /// Текущий угол поворота
        /// </summary>
        private float _currentAngle;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="angle">Угол</param>
        /// <param name="axis">Ось</param>
        /// <param name="speed">Скорость поворота</param>
        public RotationAnimation(float angle, Vector3 axis, float speed)
        {
            Angle = angle;
            Axis = axis;
            Speed = speed;

            _currentAngle = 0;
        }

        // следующий шаг
        public void NextStep(float deltaTime)
        {
            _currentAngle += Speed * deltaTime;
            if (_currentAngle > Angle)
                _currentAngle = Angle;
        }

        // применяет анимацию к точке
        public Vector3 Apply(Vector3 vertex)
        {
            var rotationMatrix = Matrix4.CreateFromAxisAngle(Axis, _currentAngle);

            return Vector3.Transform(vertex, rotationMatrix);
        }

        // сбрасывает анимацию
        public void Reset()
        {
            _currentAngle = 0;
        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public RotationAnimation() : this(0, Vector3.Zero, 0)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Сравнивает два объекта RotationAnimation на равенство 
        /// </summary>
        /// <param name="other">другой объект</param>
        /// <returns>равны ли</returns>
        public bool Equals(RotationAnimation other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            const float tolerance = 0.0001f;
            return Math.Abs(Angle - other.Angle) < tolerance
                   && VectorUtils.AreVectorsEquals(Axis, other.Axis)
                   && Math.Abs(Speed - other.Speed) < tolerance;
        }

        /// <summary>
        /// Сравнивает с переданным объектом
        /// </summary>
        /// <param name="obj">объект</param>
        /// <returns>Равны ли типы и сами объекты</returns>
        [ContractAnnotation("null=>false")]
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RotationAnimation) obj);
        }

        /// <summary>
        /// Хэш код, лучше не использовать
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => 1;
    }
}