using System.Xml;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для представления вопроса
    /// </summary>
    public class QuestionInfo
    {
        /// <summary>
        /// Ссылка на вершину вопроса
        /// </summary>
        private readonly XmlNode _node; 

        /// <summary>
        /// Ответ на вопрос
        /// </summary>
        public string Answer { get; }

        /// <summary>
        /// Тип вопроса
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Номер вопроса
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Html представление вопроса
        /// </summary>
        public string InnerHtml => _node.InnerXml;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="questionNode">Вершина вопроса</param>
        /// <param name="name">Номер вопроса</param>
        public QuestionInfo(XmlNode questionNode, int name)
        {
            _node = questionNode;
            var attributes = questionNode.Attributes;
            if (attributes == null)
                return;

            Answer = attributes["answer"].Value;
            Type = attributes["type"].Value;
            Name = name.ToString();
            InitHtmlAttributes();
        }

        /// <summary>
        /// Задаёт элементам нужные атрибуты
        /// </summary>
        private void InitHtmlAttributes()
        {
            var inputs = (_node as XmlElement)?.GetElementsByTagName("input");
            if (inputs == null) return;

            foreach (XmlElement input in inputs)
            {
                input.SetAttribute("name", Name);
                input.SetAttribute("type", Type);
            }
        }
    }
}
