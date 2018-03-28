using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace LessonLibrary.Permulation
{
    /// <summary>
    /// Класс для представления подстановок ввиде циклов
    /// </summary>
    public class PermulationCycles : IEquatable<PermulationCycles>
    {
        /// <summary>
        /// Циклы подстановки
        /// </summary>
        private readonly List<List<int>> _cycles = new List<List<int>>();

        /// <summary>
        /// Задаёт циклы по данной подстановке
        /// </summary>
        /// <param name="permulation">Подстановка, по которой строятся циклы</param>
        public PermulationCycles([NotNull] Permulation permulation)
        {
            var used = new bool[permulation.Size];
            for (var i = 1; i <= permulation.Size; i++)
            {
                if (used[i - 1]) continue;

                var cycle = new List<int>();
                var next = i;
                do
                {
                    cycle.Add(next);
                    used[next - 1] = true;
                    next = permulation[next];
                } while (next != i);
                cycle.Sort();
                _cycles.Add(cycle);
            }
            _cycles.Sort((cycle, cycle1) => cycle[0].CompareTo(cycle1[0]));
        }

        /// <inheritdoc  />
        /// <summary>
        /// Конструктор, который принимает лист элементов и по нему строит подстановку, а затем по ней циклы
        /// </summary>
        /// <param name="permulationElements">Лист с элементами подстановки</param>
        public PermulationCycles([NotNull] List<int> permulationElements) : this(new Permulation(permulationElements))
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Принимает лист листов, представляющих циклы
        /// </summary>
        /// <param name="cycles">Циклы подстановки</param>
        public PermulationCycles([NotNull] List<List<int>> cycles) : this(GetPermulationListByCycles(cycles))
        {
        }

        /// <summary>
        /// Проверяет и возвращает список элементов в подстановке, представленной в виде циклов
        /// </summary>
        /// <param name="cycles">Циклы</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Элементы подстановки в правильном порядке</returns>
        public static List<int> GetPermulationListByCycles([NotNull] List<List<int>> cycles)
        {
            if (cycles == null) throw new ArgumentNullException(nameof(cycles));

            if (cycles.Count == 0)
            {
                throw new ArgumentException("Должен быть хотя бы 1 цикл", nameof(cycles));
            }

            var permulationSize = 0;
            foreach (var cycle in cycles)
            {
                if (cycle == null)
                    throw new ArgumentException("Ссылка, ссылающаяся на цикл, не может быть null");
                if (cycle.Count == 0)
                    throw new ArgumentException("Цикл должен содержать хотя бы 1 элемент");

                permulationSize += cycle.Count;
            }

            var isElementUsed = new bool[permulationSize];
            var permulationList = new List<int>(new int[permulationSize]);

            foreach (var cycle in cycles)
            {
                for (var i = 0; i < cycle.Count; i++)
                {
                    if (cycle[i] > permulationSize || cycle[i] < 1)
                        throw new ArgumentException(
                            $"Элементы подстановки должны быть от 1 до {permulationSize}." 
                            + $" Полученный элемент : {cycle[i]}");

                    if (isElementUsed[cycle[i] - 1])
                        throw new ArgumentException(
                            $"Элементы подстановки не могут повторяться. Повторяющийся элемент {cycle[i]}");

                    isElementUsed[cycle[i] - 1] = true;
                    permulationList[cycle[i] - 1] = cycle[(i + 1) % cycle.Count];
                }    
            }

            return permulationList;
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
        public Permulation Permulation => new Permulation(GetPermulationListByCycles(_cycles));


        /// <inheritdoc />
        /// <summary>
        /// Сравнивает 2 подстановки ввиде циклов
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true если равны</returns>
        [ContractAnnotation("other:null => false")]
        public bool Equals(PermulationCycles other)
        {
            if (_cycles.Count != other?._cycles.Count)
                return false;

            for (int i = 0; i < _cycles.Count; i++)
            {
                if (_cycles[i].Count != other._cycles[i].Count)
                    return false;
                for (int j = 0; j < _cycles[i].Count; j++)
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
            return Equals((PermulationCycles) obj);
        }

        /// <summary>
        /// Получает хэшкод, зависимый от циклов
        /// </summary>
        /// <returns>Хэшкод</returns>
        public override int GetHashCode()
        {
            return (_cycles != null ? _cycles.GetHashCode() : 0);
        }
    }
}