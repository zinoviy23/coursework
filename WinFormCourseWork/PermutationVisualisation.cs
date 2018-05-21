using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using LessonLibrary.Permutations;

namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для урока визуализации подстановок. Может быть только один объект.
    /// </summary>
    public class PermutationVisualisation
    {
        /// <summary>
        /// Единственный объект
        /// </summary>
        private static PermutationVisualisation _instance;

        /// <summary>
        /// Цвета
        /// </summary>
        public static readonly Color[] Colors =
        {
            Color.Black, Color.Blue, Color.Red, Color.Green, Color.DarkCyan, Color.BlueViolet, Color.Sienna,
            Color.Chartreuse, Color.Yellow, Color.DeepPink, Color.Gray, Color.Indigo, Color.MidnightBlue,
            Color.SpringGreen, Color.Tomato, Color.Olive
        };

        /// <summary>
        /// WebBrowser для отображения уроков
        /// </summary>
        private readonly WebBrowser _htmlView;

        /// <summary>
        /// Конструктор визуализации
        /// </summary>
        /// <param name="htmlView">WebBrowser для отображения уроков</param>
        private PermutationVisualisation([NotNull] WebBrowser htmlView)
        {
            _htmlView = htmlView;

            _htmlView.DocumentCompleted += HtmlViewOnDocumentLoaded;
        }

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void HtmlViewOnDocumentLoaded(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            var inputButton = _htmlView.Document?.GetElementById("input_button");

            if (inputButton == null)
                throw new ArgumentException("У урока должна быть кнопка для ввода с id input_button");

            inputButton.Click += InputButtonOnClick;
            //_htmlView.DocumentCompleted -= HtmlViewOnDocumentLoaded;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку ввести
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="htmlElementEventArgs"></param>
        private void InputButtonOnClick(object sender, HtmlElementEventArgs htmlElementEventArgs)
        {
            var permutationInput = new PermutationInput();
            permutationInput.ShowDialog();
            var permutationDiv = _htmlView.Document?.GetElementById("permutation");
            var permutationCyclesDiv = _htmlView.Document?.GetElementById("permutation_cycles");
            var permutationGroupedByCyclesDiv = _htmlView.Document?.GetElementById("permutation_grouped_by_cycles");

            if (permutationDiv == null)
            {
                MessageBox.Show(@"У файла отсутсвует div с id permutation!", @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (permutationCyclesDiv == null)
            {
                MessageBox.Show(@"У файла отсутсвует div с id permutation_cycles!", @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (permutationGroupedByCyclesDiv == null)
            {
                MessageBox.Show(@"У файла отсутсвует div с id permutation_grouped_by_cycles!", @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            var p = permutationInput.ResultPermutation;

            if (p == null) return;
            permutationDiv.InnerHtml = "Подстановка:" + ListOfTuplesToHtml(p.TupleList);
            permutationCyclesDiv.InnerHtml = "Подстановка в виде циклов:" + CyclesListToHtml(p.Cycles.CyclesList);
            permutationGroupedByCyclesDiv.InnerHtml = "Подстановка, где элементы сгруппированы по циклам:" +
                ListOfTuplesToHtml(PermutationCycles.GroupPermutationElementsByCycles(p));
        }

        /// <summary>
        /// Создаёт объект визуализации.
        /// </summary>
        /// <param name="htmlView"></param>
        public static void CreateInstance([NotNull] WebBrowser htmlView)
        {
            if (_instance == null)
                _instance = new PermutationVisualisation(htmlView);
        }

        /// <summary>
        /// Удаляет объект, если он есть.
        /// </summary>
        public static void Release()
        {
            if (_instance != null)
                _instance._htmlView.DocumentCompleted -= _instance.HtmlViewOnDocumentLoaded;
            _instance = null;
        }

        /// <summary>
        /// Возвращает HTML представление листа пар, со скобками по бокам
        /// </summary>
        /// <param name="pairs">Лист пар с дополнительным параметром</param>
        /// <returns>HTML разметка для подстановки</returns>
        [NotNull]
        public static string ListOfTuplesToHtml([NotNull] IReadOnlyList<Tuple<int, int, int>> pairs)
        {
            var sb = new StringBuilder(@"<table>
                        <tr><td>
                            <span style=""font-size:2.5em; "">(</span>
                        </td ><td >
                        <table ><tr >");

            foreach (var pair in pairs)
            {
                sb.AppendFormat("<td align=\"center\" style=\"color: {0} \">",
                        ColorTranslator.ToHtml(Colors[pair.Item3]))
                    .Append(pair.Item1)
                    .Append("</span>").Append("</td>");
            }

            sb.Append("</tr ><tr >");

            foreach (var pair in pairs)
            {
                sb.AppendFormat("<td align=\"center\" style=\"color: {0} \">",
                        ColorTranslator.ToHtml(Colors[pair.Item3]))
                    .Append(pair.Item2)
                    .Append("</span>").Append("</td>");
            }

            sb.Append(@"</tr ></table >
                       </td ><td >
                <span style = ""font-size:2.5em;"" >) </span > </td> </tr> </table> ");
            return sb.ToString();
        }

        /// <summary>
        /// Представляет циклы подстановки в виде HTML
        /// </summary>
        /// <param name="cycles">список списков, представляющий циклы</param>
        /// <returns>HTML представление</returns>
        [NotNull]
        private static string CyclesListToHtml([NotNull] IReadOnlyList<IReadOnlyList<int>> cycles)
        {
            var sb = new StringBuilder("<table><tr>");

            for (var i = 0; i < cycles.Count; i++)
            {
                sb.AppendFormat("<td style=\"color: {0}\"> (", ColorTranslator.ToHtml(Colors[i + 1]));
                foreach (var element in cycles[i])
                {
                    sb.Append(element).Append(" ");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(")</td>");
            }

            sb.Append("</tr></table>");
            return sb.ToString();
        }
    }
}