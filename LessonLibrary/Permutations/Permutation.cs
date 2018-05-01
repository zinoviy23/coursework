using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace LessonLibrary.Permutations
{
    /// <inheritdoc cref="IEquatable{T}" />
    /// <summary>
    /// Класс для подстановок
    /// </summary>
    public class Permutation : IEquatable<Permutation>
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
        public Permutation([NotNull] List<int> elements)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            if (!CheckListOnPermutation(elements))
                throw new ArgumentException(
                    "Список должен быть перестановкой натуральных чисел от 1 до n без повторений", nameof(elements));
            Elements = elements;
        }

        /// <summary>
        /// Конструктор для тривиальной подстановки
        /// </summary>
        /// <param name="n">длина подстановки</param>
        /// <exception cref="ArgumentException"></exception>
        public Permutation(int n)
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
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>true если список перестановка, false иначе</returns>
        public static bool CheckListOnPermutation([NotNull] List<int> perm)
        {
            if (perm == null) throw new ArgumentNullException(nameof(perm));
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
        public bool Equals(Permutation other)
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
        /// <seealso cref="Equals(Permutation)"/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Permutation)obj);
        }

        /// <summary>
        /// Получает хэш подстановки взависимости от ее элементов
        /// </summary>
        /// <returns>Хэш код</returns>
        /// <seealso cref="List{T}.GetHashCode"/>
        public override int GetHashCode()
        {
            return Elements != null ? Elements.GetHashCode() : 0;
        }

        /// <summary>
        /// Переопределённый ToString. Возвращает нижнюю строку подстановки.
        /// </summary>
        /// <returns>Нижняя строка подстановки</returns>
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

        /// <summary>
        /// Берет композицию подстановок. Применяет сначала вторую, потом первую.
        /// </summary>
        /// <param name="a">Первая подстановка</param>
        /// <param name="b">Вторая подстановка</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Результат композиции двух подстановок</returns>
        public static Permutation operator *([NotNull] Permutation a, [NotNull] Permutation b)
        {
            if (a.Size != b.Size)
                throw new ArgumentException(
                    $"Размеры подстановок должны быть равны! Переданные размеры: {a.Size} {b.Size}");

            var resList = new List<int>(a.Size);
            for (var i = 1; i <= a.Size; i++)
            {
                resList.Add(a[b[i]]);
            }
            
            return new Permutation(resList);
        }

        /// <summary>
        /// Получает обратную подстановку
        /// </summary>
        /// <param name="a">Подстановка</param>
        /// <returns>Обратная подстановка</returns>
        [NotNull]
        public static Permutation operator -([NotNull] Permutation a)
        {
            var res = new List<int>(new int[a.Size]);

            for (var i = 1; i <= a.Size; i++)
            {
                res[a[i] - 1] = i;
            }

            return new Permutation(res);
        }
        
        /// <summary>
        /// Возвращает представление подстановки в виде циклов
        /// </summary>
        public PermutationCycles Cycles => new PermutationCycles(this);

        /// <summary>
        /// Возвращает представление подстановки в виде листа пар
        /// </summary>
        public List<Tuple<int, int, int>> TupleList
        {
            get
            {
                var result = new List<Tuple<int, int, int>>();

                for (var i = 1; i <= Size; i++)
                    result.Add(new Tuple<int, int, int>(i, this[i], 0));

                return result;
            }
        }
    }
}
