using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace LessonLibrary.Permulation
{
    /// <inheritdoc cref="IEquatable{T}" />
    /// <summary>
    /// Класс для подстановок
    /// </summary>
    public class Permulation : IEquatable<Permulation>
    {
        /// <summary>
        /// Элементы перестановки
        /// </summary>
        private List<int> Elements { get; }

        /// <summary>
        /// Конструктор для подстановки созданной из списка
        /// </summary>
        /// <param name="elements">Список неповторяющихся элементов от 1 до n</param>
        /// <exception cref="ArgumentException"></exception>
        public Permulation(List<int> elements)
        {
            if (!CheckListOnPermulation(elements))
                throw new ArgumentException(
                    "Список должен быть перестановкой натуральных чисел от 1 до n без повторений", nameof(elements));
            Elements = elements;
        }

        /// <summary>
        /// Конструктор для тривиальной подстановки
        /// </summary>
        /// <param name="n">длина подстановки</param>
        public Permulation(int n)
        {
            if (n <= 0)
                throw new ArgumentException("Количество элементов в подстановке должно быть больше 0.", nameof(n));
            Elements = new List<int>(n);
            for (var i = 1; i <= n; i++)
                Elements.Add(n);
        }

        /// <summary>
        /// Применяет перестановку к элементу
        /// </summary>
        /// <param name="el">Элемент, к которому применить</param>
        /// <returns>результат применения перестановки</returns>
        public int this[int el]
        {
            get
            {
                if (el < 1 || el > Elements.Count)
                    throw new ArgumentOutOfRangeException(nameof(el),
                        $"Неправильный элемент перестановки от 1 до {Elements.Count} : {el}");
                return Elements[el - 1];
            }
        }

        /// <summary>
        /// Проверяет переданный список на перестановку чисел от 1 до n
        /// </summary>
        /// <param name="perm">список, который нужно проверить</param>
        /// <returns>true если список перестановка, false иначе</returns>
        public static bool CheckListOnPermulation([NotNull] List<int> perm)
        {
            if (perm.Count == 0)
                return false;

            var contains = new bool[perm.Count];
            foreach (var el in perm)
            {
                if (el < 1 || el > perm.Count)
                    return false;
                if (contains[el - 1])
                    return false;
                contains[el - 1] = true;
            }

            return true;
        }


        public bool Equals(Permulation other)
        {
            throw new NotImplementedException();
        }
    }
}
