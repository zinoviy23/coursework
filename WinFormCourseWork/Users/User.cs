using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using JetBrains.Annotations;

namespace WinFormCourseWork.Users
{
    /// <summary>
    /// Класс для информации о пользователе
    /// </summary>
    [DataContract(Name = "User")]
    public class User
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember(Name = "Name")]
        public string Name { get; private set; }

        /// <summary>
        /// Информация о тестах пользователя
        /// </summary>
        [DataMember(Name = "Tests")]
        private Dictionary<string, UserTestAnswers> _tests = new Dictionary<string, UserTestAnswers>();

        [NotNull] public IReadOnlyDictionary<string, UserTestAnswers> Tests => _tests;

        /// <summary>
        /// Количество ответов пользователя
        /// </summary>
        public int AnswersCount => _tests.Aggregate(0, (i, pair) => i + pair.Value.AnswersCount);

        public User([NotNull] Stream stream)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(User));
                var tmpUser = (User) serializer.ReadObject(stream);
                Name = tmpUser.Name;
                _tests = tmpUser._tests;
            }
            catch (IOException ex)
            {
                throw new ArgumentException($"Из данного потока нельзя считать\n{ex.Message}");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        public User([NotNull] string name)
        {
            Name = name;
        }

        public void WriteUser([NotNull] Stream stream)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true))
                {
                    var serializer = new DataContractJsonSerializer(typeof(User));
                    serializer.WriteObject(writer, this);
                }
            }
            catch (IOException ex)
            {
                throw new ArgumentException($"В переданный поток нельзя записать.\n{ex.Message}");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        public void AddAnswer(string tag, int number, string answer)
        {
            if (!_tests.ContainsKey(tag))
                _tests[tag] = new UserTestAnswers();
            _tests[tag].Add(number, answer);
        }
    }
}