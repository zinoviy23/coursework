using System.Text;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для уроков, которые конвертируются в HTML
    /// </summary>
    public abstract class HtmlViewLesson
    {
        /// <summary>
        /// Размеченный HTML урок
        /// </summary>
        public string HtmlString
        {
            get => new StringBuilder("<html><head></head><body>").Append(Source)
                .Append("</body></html>").ToString();
        }

        /// <summary>
        /// Представление для конкретного типа файла
        /// </summary>
        protected abstract string Source { get; } 
    }
}
