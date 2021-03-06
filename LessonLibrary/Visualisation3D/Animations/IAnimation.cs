﻿using OpenTK;

namespace LessonLibrary.Visualisation3D.Animations
{
    /// <summary>
    /// Интерфейс для анимаций
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// делает следующий щаг
        /// </summary>
        /// <param name="deltaTime">время, с применения предыдущего шага</param>
        void NextStep(float deltaTime);

        /// <summary>
        /// Преобразование вершины
        /// </summary>
        /// <param name="vertex">координаты вершины</param>
        /// <returns>её образ</returns>
        Vector3 Apply(Vector3 vertex);

        /// <summary>
        /// Задаёт заново анимацию
        /// </summary>
        void Reset();

        /// <summary>
        /// Скорость анимации
        /// </summary>
        float Speed { get; }

        /// <summary>
        /// Возвращает, завершилась ли анимация
        /// </summary>
        bool IsFinish { get; }

        /// <summary>
        /// Применяет к вершине конечный вариант анимации
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <returns>Её образ</returns>
        Vector3 ApplyToEnd(Vector3 vertex);
    }
}