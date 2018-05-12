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
    [DataContract(Name = "Settings")]
    public class Settings
    {
        private static Settings _instance;

        [DataMember(Name = "AnswersCount")]
        private int _answersCount;

        [DataMember(Name = "CurrentUser")]
        private string _currentUser;

        public static int AnswersCount => _instance._answersCount;

        public static string CurrentUser => _instance._currentUser;

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
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

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
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        /// <summary>
        /// Только для тестов
        /// </summary>
        [Obsolete("Используется только для тестов")]
        public static void CreateEmpty()
        {
            _instance = new Settings();
        }
    }
}