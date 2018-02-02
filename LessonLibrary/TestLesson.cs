using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public List<QuestionInfo> Questions { get; }

        protected override string Source
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var question in Questions)
                {
                    sb.Append(question.InnerHtml);
                }

                return sb.ToString();
            }
        }

        protected override void SetHtmlView(WebBrowser htmlView)
        {
            _htmlView = htmlView;
            var inputs = htmlView.Document?.GetElementsByTagName("input");
            if (inputs == null)
                return;
            foreach (HtmlElement input in inputs)
            {
                input.Click += OnAnswerClicked;
            }
        }

        /// <summary>
        /// Обработчик события клика на элемент
        /// </summary>
        /// <param name="sender">объект</param>
        /// <param name="args">аргументы</param>
        private void OnAnswerClicked(object sender, HtmlElementEventArgs args)
        {
            var element = (HtmlElement) sender;
            var usersAnswer = element.GetAttribute("value");
            var questionNumber = int.Parse(element.GetAttribute("name"));
            if (usersAnswer != Questions[questionNumber - 1].Answer)
            {
                MessageBox.Show("Неправильно!");
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="questions">Лист вопросов</param>
        public TestLesson(List<QuestionInfo> questions)
        {
            Questions = questions;
        }
    }
}
