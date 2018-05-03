using System;
using System.Windows.Forms;
using JetBrains.Annotations;
using LessonLibrary.Permutations;

namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для калькулятора подстановок
    /// </summary>
    public sealed class PermutationCalculator
    {
        /// <summary>
        /// Единственный экземпляр калькулятора
        /// </summary>
        private static PermutationCalculator _instance;

        /// <summary>
        /// Ссылка на браузер
        /// </summary>
        private readonly WebBrowser _htmlView;

        /// <summary>
        /// Сохранённая подстановка
        /// </summary>
        private Permutation _savedPermutation;

        /// <summary>
        /// Подстановка, которая сейчас отображается
        /// </summary>
        private Permutation _showingPermutation;

        /// <summary>
        /// Приватный коструктор
        /// </summary>
        /// <param name="htmlView">Ссылка на браузер</param>
        private PermutationCalculator([NotNull] WebBrowser htmlView)
        {
            _htmlView = htmlView;
            _instance = this;

            htmlView.DocumentCompleted += HtmlViewOnDocumentCompleted;
        }

        /// <summary>
        /// Создаёт калькулятор
        /// </summary>
        /// <param name="htmlView">Ссылка на браузер</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Create([NotNull] WebBrowser htmlView)
        {
            if (_instance != null)
                throw new InvalidOperationException("Нельзя создавать более одного объекта!");

            _instance = new PermutationCalculator(htmlView);
        }

        /// <summary>
        /// Удаляет калькулятор
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Release()
        {
            if (_instance != null)
                _instance._htmlView.DocumentCompleted -= _instance.HtmlViewOnDocumentCompleted;
            _instance = null;
        }

        private void HtmlViewOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs agrs)
        {
            var inputButton = _htmlView.Document?.GetElementById("input_button");
            if (inputButton == null) 
                return;
            inputButton.Click += InputButtonOnClick;

            var deleteButton = _htmlView.Document?.GetElementById("delete_button");
            if (deleteButton == null)
                return;
            deleteButton.Click += DeleteButtonOnClick;

            var negationButton = _htmlView.Document?.GetElementById("negation_button");
            if (negationButton == null)
                return;
            negationButton.Click += NegationButtonOnClick;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку обратной подстановки. Отрисовывает обратную подстановку.
        /// </summary>
        /// <param name="sender">Кнопка</param>
        /// <param name="e">Аргументы события</param>
        private void NegationButtonOnClick(object sender, HtmlElementEventArgs e)
        {
            if (_showingPermutation == null)
            {
                MessageBox.Show(@"Введите подстановку!", @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var negPerm = -_showingPermutation;
            WriteNegationInformation(_showingPermutation, negPerm);
            _showingPermutation = negPerm;
            ShowCurrentPermutation();
        }

        /// <summary>
        /// Сбрасывает всю информацию о подстановках
        /// </summary>
        /// <param name="sender">Кнопка</param>
        /// <param name="e">Аргументы события</param>
        private void DeleteButtonOnClick(object sender, HtmlElementEventArgs e)
        {
            _savedPermutation = null;
            _showingPermutation = null;
            ShowCurrentPermutation();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку ввести. Отображает диалог и отрисовывает введённую подстановку
        /// </summary>
        /// <param name="sender">Кнопка</param>
        /// <param name="args">аргументы события</param>
        private void InputButtonOnClick(object sender, HtmlElementEventArgs args)
        {
            var permutationInputDialog = new PermutationInput();
            permutationInputDialog.ShowDialog();
            var currentPermutation = permutationInputDialog.ResultPermutation;

            if (currentPermutation == null) return;
            _showingPermutation = currentPermutation;
            ShowCurrentPermutation();
        }

        /// <summary>
        /// Отрисовывает конкретную подстановку
        /// </summary>
        private void ShowCurrentPermutation()
        {
            var permutationElement =  _htmlView.Document?.GetElementById("permutation");
            if (permutationElement == null)
                return;

            permutationElement.InnerHtml = _showingPermutation == null
                ? ""
                : PermutationVisualisation.ListOfTuplesToHtml(_showingPermutation.TupleList);
        }

        /// <summary>
        /// Отрисовывает информацию о действии "Обратная подстановка".
        /// </summary>
        /// <param name="prev">подстановка</param>
        /// <param name="newPerm">обратная к ней</param>
        private void WriteNegationInformation([NotNull] Permutation prev, [NotNull] Permutation newPerm)
        {
            var hisotoryDiv = _htmlView.Document?.GetElementById("history");
            var newDiv = _htmlView.Document?.CreateElement("div");
            if (hisotoryDiv == null || newDiv == null)
                return;

            newDiv.InnerHtml =
                "<table><tr><td>Обратная подстановка к</td>" +
                $"<td>{PermutationVisualisation.ListOfTuplesToHtml(prev.TupleList)}</td>" +
                "<td>это</td>" +
                $"<td>{PermutationVisualisation.ListOfTuplesToHtml(newPerm.TupleList)}</td></tr></table>";

            hisotoryDiv.AppendChild(newDiv);
        }
    }
}