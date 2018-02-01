using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для представления тестов
    /// </summary>
    /// <inheritdoc cref="HtmlViewLesson"/>
    public class TestLesson : HtmlViewLesson
    {
        /// <summary>
        /// XML представления вопросов
        /// </summary>
        public List<XmlNode> Questions { get; }

        protected override string Source
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var question in Questions)
                {
                    sb.Append(question.InnerXml);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="questions">Лист вопросов</param>
        public TestLesson(List<XmlNode> questions)
        {
            Questions = questions;
        }
    }
}
