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
                switch (root?.Name)
                {
                    case "text":
                    {
                        var elements = new List<string>()
                        {
                            root.InnerXml
                        };
                        return new SimpleLesson(elements);
                    }
                    case "test":
                    {
                        var elements = new List<QuestionInfo>();
                        var questions = root.GetElementsByTagName("q");
                        foreach (XmlNode node in questions)
                        {
                            elements.Add(new QuestionInfo(node));
                        }
                        return new TestLesson(elements);
                    }
                    default:
                    {
                        throw new Exception("Так пока нельзя!");
                    }
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
        }

        /// <summary>
        /// Считывает задание с таблицей кэли
        /// </summary>
        /// <param name="path">Путь до таблицы</param>
        /// <returns></returns>
        public static CayleyTableTestLesson ReadCayleyTableTestLesson(string path)
        {
            var table = new XmlDocument();
            try
            {
                table.Load(path);
                var root = table.DocumentElement;
                if (root == null || root.Name != "table")
                {
                    throw new Exception("Неправильный формат таблицы");
                }
                var elements = new List<string[]>();
                var lines = root.GetElementsByTagName("line");
                foreach (XmlNode line in lines)
                {
                    elements.Add(line.InnerText.Split(' '));
                }

                var resultTable = new string[elements.Count, elements[0].Length];
                for (var lineIndex = 0; lineIndex < elements.Count; lineIndex++)
                {
                    for (var lineElementIndex = 0; lineElementIndex < elements[0].Length; lineElementIndex++)
                    {
                        resultTable[lineIndex, lineElementIndex] = elements[lineIndex][lineElementIndex];
                    }
                }

                return new CayleyTableTestLesson(resultTable);
            }
            catch (XmlException exception)
            {
                throw new XmlException(exception.Message);
            }
            catch (DirectoryNotFoundException exception)
            {
                throw new DirectoryNotFoundException(exception.Message);
            }
        }
    }
}
