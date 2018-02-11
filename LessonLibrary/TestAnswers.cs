using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для сравнивания ответов
    /// </summary>
    public class TestAnswers
    {
        /// <summary>
        /// Правильные ответы
        /// </summary>
        public List<QuestionInfo> RightAnswers { get; }

        /// <summary>
        /// Ответы пользователя
        /// </summary>
        public List<string> UsersAnswers { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="questionInfos">Информации о вопросах</param>
        public TestAnswers(List<QuestionInfo> questionInfos)
        {
            RightAnswers = questionInfos;
            UsersAnswers = new List<string>(new string[RightAnswers.Count]).ConvertAll(el => "");
        }

        /// <summary>
        /// Задаёт ответ в вопросах с одним ответом
        /// </summary>
        /// <param name="questionIndex"></param>
        /// <param name="answ"></param>
        public void SetRadioAnswer(int questionIndex, string answ)
        {
            UsersAnswers[questionIndex] = answ;
        }

        /// <summary>
        /// Убирает из ответа ответ пользователя
        /// </summary>
        /// <param name="questionIndex">Номер вопроса</param>
        /// <param name="answ">Ответ</param>
        public void UnSetCheckBoxAnswer(int questionIndex, string answ)
        {
            UsersAnswers[questionIndex] = UsersAnswers[questionIndex].Replace(answ, "");
        }

        /// <summary>
        /// Добавляет к ответу
        /// </summary>
        /// <param name="questionIndex">Номер вопроса</param>
        /// <param name="answ">Ответ</param>
        public void SetCheckBoxAnswer(int questionIndex, string answ)
        {
            UsersAnswers[questionIndex] = UsersAnswers[questionIndex] + answ;
        }

        /// <summary>
        /// Проверяет ответы
        /// </summary>
        /// <returns>Лист из неправильных ответов</returns>
        public List<int> CheckAnswers()
        {
            var mistakes = new List<int>();
            for (var i = 0; i < RightAnswers.Count; i++)
            {
                switch (RightAnswers[i].Type)
                {
                    case "text":
                    case "radio":
                        if (RightAnswers[i].Answer != UsersAnswers[i].Trim())
                            mistakes.Add(i);
                        break;
                    case "checkbox":
                        if (!CompareCheckBoxAnswers(RightAnswers[i].Answer, UsersAnswers[i]))
                            mistakes.Add(i);
                        break;
                    default:
                        throw new Exception("Не бывает других элементов!");
                }
            }

            return mistakes;
        }

        /// <summary>
        /// Задаёт ответ для текстового поля
        /// </summary>
        /// <param name="questionIndex">номер вопроса</param>
        /// <param name="answer">ответ</param>
        public void SetTextAnswer(int questionIndex, string answer)
        {
            UsersAnswers[questionIndex] = answer;
        }

        /// <summary>
        /// Проверяет равны ли выбранные элементы правильному ответу
        /// </summary>
        /// <param name="right">Правильный ответ</param>
        /// <param name="users">Пользовательский</param>
        /// <returns>Лист ошибок</returns>
        public static bool CompareCheckBoxAnswers(string right, string users)
        {
            foreach (var answ in users)
            {
                if (!right.Contains(answ))
                    return false;
                right = right.Replace(answ.ToString(), "");
            }

            return right == "";
        }
    }
}
