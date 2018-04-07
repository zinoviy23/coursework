using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using LessonLibrary.Permulation;

namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для урока визуализации подстановок. Может быть только один объект.
    /// </summary>
    public class PermulationVisualisation
    {
        private static PermulationVisualisation _instance;

        /// <summary>
        /// WebBrowser для отображения уроков
        /// </summary>
        private readonly WebBrowser _htmlView;

        /// <summary>
        /// Конструктор визуализации
        /// </summary>
        /// <param name="htmlView">WebBrowser для отображения уроков</param>
        private PermulationVisualisation([NotNull] WebBrowser htmlView)
        {
            _htmlView = htmlView;

            _htmlView.DocumentCompleted += HtmlViewOnDocumentLoaded;
        }

        private void HtmlViewOnDocumentLoaded(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            var inputButton = _htmlView.Document?.GetElementById("input_button");

            if (inputButton == null)
                throw new ArgumentException("У урока должна быть кнопка для ввода с id input_button");

            inputButton.Click += InputButtonOnClick;
        }

        private void InputButtonOnClick(object sender, HtmlElementEventArgs htmlElementEventArgs)
        {
            var permulationInput = new PermulationInput();
            permulationInput.ShowDialog();
            var permulationDiv = _htmlView.Document?.GetElementById("permulation");
            var permulationCyclesDiv = _htmlView.Document?.GetElementById("permulation_cycles");
            var permulationGroupedByCyclesDiv = _htmlView.Document?.GetElementById("permulatio_grouped_by_cycles");

            if (permulationDiv == null)
            {
                MessageBox.Show(@"У файла отсутсвует div с id permulation!", @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (permulationCyclesDiv == null)
            {
                MessageBox.Show(@"У файла отсутсвует div с id permulation_cycles!", @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (permulationGroupedByCyclesDiv == null)
            {
                MessageBox.Show(@"У файла отсутсвует div с id permulatio_grouped_by_cycles!", @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            var p = permulationInput.ResulPermulation;

            if (p == null) return;
            permulationDiv.InnerHtml = ListOfTuplesToHtml(p.TupleList);
            permulationCyclesDiv.InnerHtml = p.Cycles.ToString();
            permulationGroupedByCyclesDiv.InnerHtml =
                ListOfTuplesToHtml(PermulationCycles.GroupPermulationElementsByCycles(p));
        }

        /// <summary>
        /// Создаёт объект визуализации.
        /// </summary>
        /// <param name="htmlView"></param>
        public static void CreateInstance([NotNull] WebBrowser htmlView)
        {
            if (_instance == null)
                _instance = new PermulationVisualisation(htmlView);
        }

        /// <summary>
        /// Удаляет объект, если он есть.
        /// </summary>
        public static void Release()
        {
            if (_instance != null)
            {
                _instance._htmlView.DocumentCompleted -= _instance.HtmlViewOnDocumentLoaded;
            }
            _instance = null;
        }

        /// <summary>
        /// Возвращает HTML представление листа пар, со скобками по бокам
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns>HTML разметка для подстановки</returns>
        [NotNull]
        private static string ListOfTuplesToHtml([NotNull] IReadOnlyList<Tuple<int, int>> pairs)
        {
            var sb = new StringBuilder(@"<table>
                        <tr><td>
                            <span style=""font-size:2.5em; "">(</span>
                        </td ><td >
                        <table ><tr >");

            foreach (var pair in pairs)
            {
                sb.Append("<td align=\"center\">").Append(pair.Item1).Append("</td>");
            }

            sb.Append("</tr ><tr >");

            foreach (var pair in pairs)
            {
                sb.Append("<td align=\"center\">").Append(pair.Item2).Append("</td>");
            }

            sb.Append(@"</tr ></table >
                       </td ><td >
                <span style = ""font-size:2.5em;"" >) </span > </td> </tr> </table> ");
            return sb.ToString();
        }
    }
}