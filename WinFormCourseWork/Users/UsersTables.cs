using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using JetBrains.Annotations;

namespace WinFormCourseWork.Users
{
    /// <summary>
    /// Класс для всех хранения всех пользователей
    /// </summary>
    [DataContract(Name = "Users")]
    public class UsersTables
    {
        /// <summary>
        /// Ссылка на объект с информацией о пользователях
        /// </summary>
        private static UsersTables _instance;

        /// <summary>
        /// Пользователи
        /// </summary>
        [DataMember(Name = "UsersList")]
        private List<string> _users;

        /// <summary>
        /// Считывает информацию о пользователях из потока
        /// </summary>
        /// <param name="stream">Поток</param>
        public static void ReadUsersFromFile([NotNull] Stream stream)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(UsersTables));
                _instance = (UsersTables) serializer.ReadObject(stream);
                
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

        public static void AddUser(string userName)
        {
            _instance._users.Add(userName);
        }

        public static void CreateEmptyInstance()
        {
            _instance = new UsersTables();
        }

        private UsersTables()
        {
            _users = new List<string>();
        }

        public static void WriteUsersInfo(Stream stream)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true))
                {
                    var serializer = new DataContractJsonSerializer(typeof(UsersTables));
                    serializer.WriteObject(writer, _instance);
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

        /// <summary>
        /// Возвращает имя файла для сохранения пользователей
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>имя файла вида N.json, где N индекс пользователя в массиве всех пользователей</returns>
        [CanBeNull]
        public static string GetUserFileName(string userName)
        {
            var userIndex = _instance._users.FindIndex(s => string.Equals(s, userName, StringComparison.InvariantCulture));
            return userIndex == -1 ? null : $"{userIndex}.json";
        }


    }
}