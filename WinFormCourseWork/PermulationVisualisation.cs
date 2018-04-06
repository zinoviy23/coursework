using System;
using System.Windows.Forms;
using JetBrains.Annotations;

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
            var permulationDiv = _htmlView.Document.GetElementById("permulation");

            permulationDiv.InnerHtml = @"<table>
                <tr><td>
                    <span style=""font - size:2.5em; "">( </span>
                </td ><td >
   
                <table ><tr ><td > 1 2 3 </td ></tr ><tr ><td > 1 2 3 </td ></tr ></table >
                    
                </td ><td >
                    
                <span style = ""font-size:2.5em;"" >) </span > </td> </tr> </table> ";
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
    }
}