using System.IO;

namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для вывода отладки
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// адаптер вывода в файл отладки
        /// </summary>
        private static readonly StreamWriter DebugWriter;

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static Log()
        {
            DebugWriter = new StreamWriter("DebugHelper.debug");
        }

        /// <summary>
        /// Выводит объект без переноса строки
        /// </summary>
        /// <param name="o">объект</param>
        public static void Write(object o)
        {
            DebugWriter.Write(o);
        }

        /// <summary>
        /// Выводит объект с переносом строки
        /// </summary>
        /// <param name="o"></param>
        public static void WriteLine(object o)
        {
            DebugWriter.WriteLine(o);
        }

        /// <summary>
        /// Закрывает поток
        /// </summary>
        public static void Close()
        {
            DebugWriter.Close();
        }
    }
}