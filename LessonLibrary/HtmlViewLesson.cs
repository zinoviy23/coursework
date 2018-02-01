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
        /// Отображение страницы
        /// </summary>
        public WebBrowser HtmlView { get; set; }

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
