using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LessonLibrary
{
    /// <summary>
    /// Класс для считываения уроков
    /// </summary>
    public static class LessonReader
    {
        /// <summary>
        /// Считывает текстовый урок
        /// </summary>
        /// <param name="lessonPath">Путь до урока</param>
        /// <returns>HTML представление урока</returns>
        public static HtmlViewLesson ReadHtmlViewLesson(string lessonPath)
        {
            var lesson = new XmlDocument();
            try
            {
                lesson.Load(lessonPath);
                var root = lesson.DocumentElement;
                if (root?.Name == "text")
                {
                    var elements = new List<string>()
                    {
                        root.InnerXml
                    };
                    return new SimpleLesson(elements);
                }
                else if (root?.Name == "test")
                {
                    var elements = new List<XmlNode>();
                    var questions = root.GetElementsByTagName("q");
                    foreach (XmlNode node in questions)
                    {
                        elements.Add(node);
                    }
                    return new TestLesson(elements);
                }
                else
                {
                    throw new Exception("Так пока нельзя!");
                }

            }
            catch (XmlException exception)
            {
                throw new XmlException(exception.Message);
            }
            catch (DirectoryNotFoundException exception)
            {
                throw new DirectoryNotFoundException(exception.Message);
            }

            return null;
        }
    }
}
