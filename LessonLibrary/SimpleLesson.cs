using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для представления простых уроков
    /// </summary>
    /// <inheritdoc cref="HtmlViewLesson"/>
    public class SimpleLesson : HtmlViewLesson
    {
        /// <summary>
        /// Элементы урока
        /// </summary>
        private readonly List<string> _elements;

        protected override string Source
        {
            get
            {
                var result = new StringBuilder("");
                foreach (var element in _elements)
                {
                    result.Append(element);
                }

                return result.ToString();
            }
        }

        protected override void SetHtmlView(WebBrowser htmlView)
        {
            CurrenHtmlView = htmlView;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="elements">Массив обзацев и тд.</param>
        public SimpleLesson(List<string> elements)
        {
            _elements = elements;
        }
    }
}
