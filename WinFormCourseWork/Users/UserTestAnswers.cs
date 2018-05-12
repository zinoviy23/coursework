using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WinFormCourseWork.Users
{
    /// <summary>
    /// Класс для ответов к тесту
    /// </summary>
    [DataContract(Name = "Test Answers")]
    public class UserTestAnswers
    {
        /// <summary>
        /// Ответы теста
        /// </summary>
        [DataMember(Name = "Answers")]
        private Dictionary<int, string> _answers = new Dictionary<int, string>();

        /// <summary>
        /// Возвращает ответы теста
        /// </summary>
        public IReadOnlyDictionary<int, string> Answers => _answers;

        /// <summary>
        /// Добавляет ответ к тесту
        /// </summary>
        /// <param name="number"></param>
        /// <param name="answer"></param>
        public void Add(int number, string answer)
        {
            _answers.Add(number, answer);
        }

        /// <summary>
        /// Количество ответов
        /// </summary>
        public int AnswersCount => _answers.Count;
    }
}