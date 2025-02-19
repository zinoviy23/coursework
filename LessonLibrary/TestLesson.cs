﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

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

        /// <summary>
        /// Ответы на вопросы
        /// </summary>
        private readonly TestAnswers _answers;

        /// <summary>
        /// Скрипты для показывание ответов в тестах
        /// </summary>
        private const string AnswersScriptHtml = @"
        <script>
          function showAnswer(n) {
            document.getElementById('answer' + n).style.display = ""block"";
            document.getElementById('answerButton' + n).style.display = ""none"";
          }
          function hideAnswer(n) {
            document.getElementById('answer' + n).style.display = ""none"";
            document.getElementById('answerButton' + n).style.display = ""inline"";
          }
        </script>";

        /// <summary>
        /// Переопределённое свойство для получения HTML
        /// </summary>
        /// <inheritdoc cref="HtmlViewLesson"/>
        protected override string Source
        {
            get
            {
                var sb = new StringBuilder(AnswersScriptHtml);
                for (var index = 0; index < Questions.Count; index++)
                {
                    var question = Questions[index];
                    sb.Append(question.InnerHtml).Append(question.AnswerDivHtml(index));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Задаёт обработчики событий для ввода
        /// </summary>
        /// <param name="htmlView">Отображение html</param>
        /// <inheritdoc cref="HtmlViewLesson"/>
        protected override void SetHtmlView(WebBrowser htmlView)
        {
            CurrenHtmlView = htmlView;
            var inputs = htmlView.Document?.GetElementsByTagName("input");
            if (inputs == null)
                return;
            foreach (HtmlElement input in inputs)
            {
                //Debug.Print(input.GetAttribute("className"));
                if (input.GetAttribute("className") == QuestionInfo.AnswersInfoClass)
                {
                    continue;
                }

                if (input.GetAttribute("type") != "text")
                {
                    input.Click += OnAnswerClicked;
                }
                else
                    input.KeyUp += OnAnswerClicked;
            }
        }

        /// <summary>
        /// Сразу отвечает на ответы, которые переданы в метод
        /// </summary>
        /// <param name="answers">Словарь ответорв, где номеру ответа соответвует ответ</param>
        public void SetEnteredAnswers(IReadOnlyDictionary<int, string> answers)
        {
            var inputs = CurrenHtmlView.Document?.GetElementsByTagName("input");

            if (inputs == null)
                return;

            foreach (var answer in answers)
            {
                foreach (HtmlElement input in inputs)
                {
                    if (input.GetAttribute("name") != (answer.Key + 1).ToString()) continue;
                    switch (input.GetAttribute("type"))
                    {
                        case "radio":
                            if (input.GetAttribute("value") == answer.Value.Trim())
                                input.SetAttribute("checked", "True");
                            break;
                        case "checkbox":
                            if (answer.Value.Contains(input.GetAttribute("value")))
                                input.SetAttribute("checked", "True");

                            break;
                        case "text":
                            input.SetAttribute("value", answer.Value);
                            break;
                    }
                }
                _answers.UsersAnswers[answer.Key] = answer.Value;
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
            var questionNumber = int.Parse(element.GetAttribute("name")) - 1;
            switch (Questions[questionNumber].Type)
            {
                case "radio":
                    _answers.SetRadioAnswer(questionNumber, usersAnswer);
                    break;
                case "checkbox":
                    Debug.WriteLine("Lel");
                    if (element.GetAttribute("checked") != "False")
                        _answers.SetCheckBoxAnswer(questionNumber, usersAnswer);
                    else
                        _answers.UnSetCheckBoxAnswer(questionNumber, usersAnswer);
                    break;
                case "text":
                    _answers.SetTextAnswer(questionNumber, usersAnswer);
                    break;
                default:
                    throw new Exception("Такого нет!");
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="questions">Лист вопросов</param>
        public TestLesson(List<QuestionInfo> questions)
        {
            Questions = questions;
            _answers = new TestAnswers(Questions);
        }

        /// <summary>
        /// Проверяет тест
        /// </summary>
        /// <returns>Лист с ошибками</returns>
        public List<int> CheckAnswers() => _answers.CheckAnswers();
    }
}
