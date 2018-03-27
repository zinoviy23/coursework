using System;
using System.Collections.Generic;
using System.Text;
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
        public Permulation([NotNull] List<int> elements)
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
                Elements.Add(i);
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
        /// Длинна подстановки
        /// </summary>
        public int Size => Elements.Count;

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

        /// <summary>
        /// Сравнивает на равенство две подстановки
        /// </summary>
        /// <param name="other">Подстановка, с которой нужно сравнить данную</param>
        /// <returns>true если подстановки одной длины и равны, false иначе</returns>
        /// <inheritdoc cref="IEquatable{T}"/>
        [ContractAnnotation("other:null => false")]
        public bool Equals(Permulation other)
        {
            if (Size != other?.Size)
                return false;

            for (var i = 1; i <= Size; i++)
            {
                if (this[i] != other[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Сравнивает данную подстановку с объектом. Использует Equals.
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns>true - если переданный объект подстановка, равная данной, false иначе</returns>
        /// <seealso cref="Equals(Permulation)"/>
        public override bool Equals(object obj)
        {
            if (obj is Permulation p)
            {
                return Equals(p);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Elements != null ? Elements.GetHashCode() : 0;
        }

        /// <summary>
        /// Переопределённый ToString. Возвращает нижнюю строку подстановки.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder("(");
            foreach (var el in Elements)
            {
                sb.Append(el).Append(" ");
            }

            sb[sb.Length - 1] = ')';
            return sb.ToString();
        }
    }
}
