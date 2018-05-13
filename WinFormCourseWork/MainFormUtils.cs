using System.Windows.Forms;
using JetBrains.Annotations;

namespace WinFormCourseWork
{
    /// <summary>
    /// ����� ��� ������ �������� �������
    /// </summary>
    internal static class MainFormUtils
    {
        /// <summary>
        /// ��������� ��������� ������
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
        /// ���������� �������
        /// </summary>
        /// <param name="node">�������</param>
        /// <returns>���������� �������</returns>
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