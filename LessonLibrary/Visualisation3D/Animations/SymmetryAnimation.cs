using OpenTK;

namespace LessonLibrary.Visualisation3D.Animations
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для анимации симметрии
    /// </summary>
    public class SymmetryAnimation : IAnimation
    {
        public Plane Plane { get; }

        public float Speed { get; }

        private float _currentCoef;

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