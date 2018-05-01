using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace LessonLibrary.Permutation
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для представления подстановок ввиде циклов
    /// </summary>
    public class PermutationCycles : IEquatable<PermutationCycles>
    {
        /// <summary>
        /// Циклы подстановки
        /// </summary>
        private readonly List<List<int>> _cycles = new List<List<int>>();

        /// <summary>
        /// Задаёт циклы по данной подстановке
        /// </summary>
        /// <param name="permutation">Подстановка, по которой строятся циклы</param>
        public PermutationCycles([NotNull] Permutation permutation)
        {
            var used = new bool[permutation.Size];
            for (var i = 1; i <= permutation.Size; i++)
            {
                if (used[i - 1]) continue;

                var cycle = new List<int>();
                var next = i;
                do
                {
                    cycle.Add(next);
                    used[next - 1] = true;
                    next = permutation[next];
                } while (next != i);

                _cycles.Add(cycle);
            }
            _cycles.Sort((cycle, cycle1) => cycle[0].CompareTo(cycle1[0]));
        }

        /// <inheritdoc  />
        /// <summary>
        /// Конструктор, который принимает лист элементов и по нему строит подстановку, а затем по ней циклы
        /// </summary>
        /// <param name="permutationElements">Лист с элементами подстановки</param>
        public PermutationCycles([NotNull] List<int> permutationElements) : this(new Permutation(permutationElements))
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Принимает лист листов, представляющих циклы
        /// </summary>
        /// <param name="cycles">Циклы подстановки</param>
        public PermutationCycles([NotNull] List<List<int>> cycles) : this(GetPermutationListByCycles(cycles))
        {
        }

        /// <summary>
        /// Проверяет и возвращает список элементов в подстановке, представленной в виде циклов
        /// </summary>
        /// <param name="cycles">Циклы</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Элементы подстановки в правильном порядке</returns>
        public static List<int> GetPermutationListByCycles([NotNull] List<List<int>> cycles)
        {
            if (cycles == null) throw new ArgumentNullException(nameof(cycles));

            if (cycles.Count == 0)
            {
                throw new ArgumentException("Должен быть хотя бы 1 цикл", nameof(cycles));
            }

            var permutationSize = 0;
            foreach (var cycle in cycles)
            {
                if (cycle == null)
                    throw new ArgumentException("Ссылка, ссылающаяся на цикл, не может быть null");
                if (cycle.Count == 0)
                    throw new ArgumentException("Цикл должен содержать хотя бы 1 элемент");

                permutationSize += cycle.Count;
            }

            var isElementUsed = new bool[permutationSize];
            var permutationList = new List<int>(new int[permutationSize]);

            foreach (var cycle in cycles)
            {
                for (var i = 0; i < cycle.Count; i++)
                {
                    if (cycle[i] > permutationSize || cycle[i] < 1)
                        throw new ArgumentException(
                            $"Элементы подстановки должны быть от 1 до {permutationSize}." 
                            + $" Полученный элемент : {cycle[i]}");

                    if (isElementUsed[cycle[i] - 1])
                        throw new ArgumentException(
                            $"Элементы подстановки не могут повторяться. Повторяющийся элемент {cycle[i]}");

                    isElementUsed[cycle[i] - 1] = true;
                    permutationList[cycle[i] - 1] = cycle[(i + 1) % cycle.Count];
                }    
            }

            return permutationList;
        }

        /// <summary>
        /// Переопределённый ToString
        /// </summary>
        /// <returns>Строковое представление циклов</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("");
            foreach (var cycle in _cycles)
            {
                sb.Append("(");
                foreach (var el in cycle)
                {
                    sb.Append(el).Append(" ");
                }

                sb[sb.Length - 1] = ')';
                sb.Append(" ");
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// Возвращает подстановку в нормальном представлении
        /// </summary>
        public Permutation Permutation => new Permutation(GetPermutationListByCycles(_cycles));


        /// <inheritdoc />
        /// <summary>
        /// Сравнивает 2 подстановки ввиде циклов
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true если равны</returns>
        [ContractAnnotation("other:null => false")]
        public bool Equals(PermutationCycles other)
        {
            if (_cycles.Count != other?._cycles.Count)
                return false;

            for (var i = 0; i < _cycles.Count; i++)
            {
                if (_cycles[i].Count != other._cycles[i].Count)
                    return false;
                for (var j = 0; j < _cycles[i].Count; j++)
                    if (_cycles[i][j] != other._cycles[i][j])
                        return false;
            }

            return true;
        }

        /// <summary>
        /// Сравнивает циклы с переданным объектом
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true, если равны</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PermutationCycles) obj);
        }

        /// <summary>
        /// Получает хэшкод, зависимый от циклов
        /// </summary>
        /// <returns>Хэшкод</returns>
        public override int GetHashCode()
        {
            return _cycles != null ? _cycles.GetHashCode() : 0;
        }

        /// <summary>
        /// Группирует элементы подстановки по циклам
        /// </summary>
        /// <param name="p">Подстановка</param>
        /// <returns>Лист пар, где первый элемент число, а второй это результат применения подстановки</returns>
        public static List<Tuple<int, int, int>> GroupPermutationElementsByCycles(Permutation p)
        {
            var result = new List<Tuple<int, int, int>>();

            for (var i = 0; i < p.Cycles._cycles.Count; i++)
            {
                foreach (var element in p.Cycles._cycles[i])
                {
                    result.Add(new Tuple<int, int, int>(element, p[element], i + 1));
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает циклы подстановки ввиде списка списков
        /// </summary>
        public IReadOnlyList<IReadOnlyList<int>> CyclesList => _cycles;
    }
}