using OpenTK;

namespace LessonLibrary.Visualisation3D.Animations
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для анимации поворота
    /// </summary>
    public class RotationAnimation : IAnimation
    {
        /// <summary>
        /// Возвращает угол, на который нужно повернуть
        /// </summary>
        public float Angle { get; }

        /// <summary>
        /// Возвращает ось, вокруг которой нужно повернуть
        /// </summary>
        public Vector3 Axis { get; }

        /// <inheritdoc />
        /// <summary>
        /// Возвращает скорость, с которой надо вращать
        /// </summary>
        public float Speed { get;  }

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

        public void NextStep(float deltaTime)
        {
            _currentAngle += Speed * deltaTime;
            if (_currentAngle > Angle)
                _currentAngle = Angle;
        }

        public Vector3 Apply(Vector3 vertex)
        {
            var rotationMatrix = Matrix4.CreateFromAxisAngle(Axis, _currentAngle);

            return Vector3.Transform(vertex, rotationMatrix);
        }

        public void Reset()
        {
            _currentAngle = 0;
        }
    }
}