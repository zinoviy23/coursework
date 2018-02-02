using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Html представление вопроса
        /// </summary>
        public string InnerHtml => _node.InnerXml;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="questionNode">Вершина вопроса</param>
        public QuestionInfo(XmlNode questionNode)
        {
            _node = questionNode;
            var attributes = questionNode.Attributes;
            if (attributes == null)
                return;

            Answer = attributes["answer"].Value;
            Type = attributes["type"].Value;
        }
    }
}
