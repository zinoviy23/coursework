using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using WinFormCourseWork.Users;

namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для настроек
    /// </summary>
    [DataContract(Name = "Settings")]
    public class Settings
    {
        /// <summary>
        /// Объект
        /// </summary>
        private static Settings _instance;

        /// <summary>
        /// кол-во вопросов
        /// </summary>
        [DataMember(Name = "AnswersCount")]
        private int _answersCount;

        /// <summary>
        /// имя пользователя
        /// </summary>
        [DataMember(Name = "CurrentUserName")]
        private string _currentUserName;

        /// <summary>
        /// Количество вопросов всего
        /// </summary>
        public static int AnswersCount => _instance._answersCount;

        /// <summary>
        /// Имя текущего пользователя
        /// </summary>
        public static string CurrentUserName
        {
            get =>_instance. _currentUserName;
            set => _instance._currentUserName = value;
        }

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public static User CurrentUser { get; set; }

        /// <summary>
        /// Считывает настройки из потока
        /// </summary>
        /// <param name="stream"></param>
        public static void ReadSettingsFromStream(Stream stream)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Settings));
                _instance = (Settings) serializer.ReadObject(stream);
            }
            catch (IOException ex)
            {
                throw new ArgumentException($"Из данного потока нельзя считать\n{ex.Message}");
            }
            catch (SerializationException ex)
            {
                throw new ArgumentException($"Ошибка считывания настроек.\n{ex.Message}");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        /// <summary>
        /// Записывает настройки в поток
        /// </summary>
        /// <param name="stream">поток</param>
        public static void WriteToStream(Stream stream)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Settings));
                    serializer.WriteObject(writer, _instance);
                }
            }
            catch (IOException ex)
            {
                throw new ArgumentException($"В переданный поток нельзя записать.\n{ex.Message}");
            }
            catch (SerializationException ex)
            {
                throw new ArgumentException($"Ошибка записи настроек.\n{ex.Message}");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        /// <summary>
        /// Создаёт пустые настройки
        /// </summary>
        public static void CreateEmpty()
        {
            _instance = new Settings();
        }
    }
}