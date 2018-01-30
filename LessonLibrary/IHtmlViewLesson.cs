namespace LessonLibrary
{
    /// <summary>
    /// Интерфейс для уроков, которые конвертируются в HTML
    /// </summary>
    public interface IHtmlViewLesson
    {
        /// <summary>
        /// HTML разметка урока
        /// </summary>
        string HtmlString { get; }
    }
}
