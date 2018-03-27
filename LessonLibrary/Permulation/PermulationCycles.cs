using System.Collections.Generic;
using System.Text;

namespace LessonLibrary.Permulation
{
    /// <summary>
    /// Класс для представления подстановок ввиде циклов
    /// </summary>
    public class PermulationCycles
    {
        private readonly List<List<int>> _cycles = new List<List<int>>();

        /// <summary>
        /// Задаёт циклы по данной подстановке
        /// </summary>
        /// <param name="permulation">Подстановка, по которой строятся циклы</param>
        public PermulationCycles(Permulation permulation)
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
        public PermulationCycles(List<int> permulationElements) : this(new Permulation(permulationElements))
        {
        }

        /// <summary>
        /// Переопределённый ToString
        /// </summary>
        /// <returns></returns>
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
    }
}