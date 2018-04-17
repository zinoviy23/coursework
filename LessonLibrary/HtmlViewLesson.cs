using System.Text;
using System.Windows.Forms;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для уроков, которые конвертируются в HTML
    /// </summary>
    public abstract class HtmlViewLesson
    {
        /// <summary>
        /// Представление страницы
        /// </summary>
        protected WebBrowser CurrenHtmlView;

        /// <summary>
        /// Отображение страницы
        /// </summary>
        public WebBrowser HtmlView
        {
            get => CurrenHtmlView;
            set => SetHtmlView(value);
        }

        /// <summary>
        /// Задаёт отображение страницы
        /// </summary>
        /// <param name="htmlView">Отображение страницы</param>
        protected abstract void SetHtmlView(WebBrowser htmlView);

        /// <summary>
        /// Размеченный HTML урок
        /// </summary>
        public string HtmlString => new StringBuilder("<html><head></head><body>").Append(Source)
            .Append("</body></html>").ToString();

        /// <summary>
        /// Представление для конкретного типа файла
        /// </summary>
        protected abstract string Source { get; } 
    }
}
