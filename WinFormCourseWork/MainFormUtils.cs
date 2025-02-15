using System.Windows.Forms;
using JetBrains.Annotations;

namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для всяких полезных методов
    /// </summary>
    internal static class MainFormUtils
    {
        /// <summary>
        /// Получение следующей вершиы
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [NotNull]
        public static TreeNode NextNode([NotNull] TreeNode node)
        {
            var current = node;
            while (current.Parent != null && current.NextNode == null)
            {
                current = current.Parent;
            }

            if (current.Parent == null)
                return node;

            current = current.NextNode;
            while (current.FirstNode != null)
            {
                current = current.FirstNode;
            }

            return current;
        }

        /// <summary>
        /// Предыдущая вершина
        /// </summary>
        /// <param name="node">вершина</param>
        /// <returns>Предыдущая вершина</returns>
        [NotNull]
        public static TreeNode PreviousNode([NotNull] TreeNode node)
        {
            var current = node;
            while (current.Parent != null && current.PrevNode == null)
            {
                current = current.Parent;
            }

            if (current.Parent == null)
                return node;

            current = current.PrevNode;
            while (current.LastNode != null)
            {
                current = current.LastNode;
            }

            return current;
        }
    }
}