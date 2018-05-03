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
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку ввести. Отображает диалог и отрисовывает введённую подстановку
        /// </summary>
        /// <param name="sender">браузер</param>
        /// <param name="args">аргументы события</param>
        private void InputButtonOnClick(object sender, EventArgs args)
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
    }
}