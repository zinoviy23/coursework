using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using JetBrains.Annotations;
using LessonLibrary.Visualisation3D.Animations;

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
                        var name = 1;
                        foreach (XmlNode node in questions)
                        {
                            elements.Add(new QuestionInfo(node, name++));
                        }
                        return new TestLesson(elements);
                    }
                    default:
                    {
                        throw new ArgumentException("Так пока нельзя!");
                    }
                }

                
            }
            catch (XmlException exception)
            {
                throw new ArgumentException($"Ошибка в формате урока! {exception.Message}");
            }
            catch (DirectoryNotFoundException exception)
            {
                throw new ArgumentException($"Ошибка доступа к уроку! {exception.Message}");
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
                    throw new ArgumentException("Неправильный формат таблицы");
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
                throw new ArgumentException($"Ошибка в формате таблицы! {exception.Message}");
            }
            catch (DirectoryNotFoundException exception)
            {
                throw new ArgumentException($"Ошибка доступа к таблице! {exception.Message}");
            }
        }

        /// <summary>
        /// Считывает информацию об уроках в TreeView
        /// </summary>
        /// <param name="treeView">TreeView для отображения уроков</param>
        /// <param name="path">Путь до информации о дереве</param>
        public static void ReadLessonsTreeInfo([NotNull] TreeView treeView, [NotNull] string path)
        {
            var lessonsTreeInfo = new XmlDocument();
            try
            {
                lessonsTreeInfo.Load(new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read),
                    Encoding.UTF8));

                var root = lessonsTreeInfo.DocumentElement;
                if (root == null || root.Name != "lessonstree")
                    throw new ArgumentException("Неправильный формат файла с информацией об дереве уроков!");

                InitXmlElement(root, treeView.Nodes[0]);
            }
            catch (XmlException ex)
            {
                throw new ArgumentException($"Неправильный формат таблицы с деревом уроков. {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new ArgumentException($"Ошибка доступа к файлу с деревом уроков. {ex.Message}");
            }
        }

        /// <summary>
        /// Рекурсивно иницилизирует дерево уроков
        /// </summary>
        /// <param name="node">Вершина, которую надо обработать</param>
        /// <param name="treeViewNode">Элемент TreeView, куда надо добавлять новые вершины</param>
        private static void InitXmlElement(XmlNode node, TreeNode treeViewNode)
        {
            foreach (XmlNode nodeChild in node.ChildNodes)
            {
                switch (nodeChild.Name)
                {
                    case "comment":
                        continue;
                    case "root":
                    {
                        var rootNode = new TreeNode();
                        treeViewNode.Nodes.Add(rootNode);

                        if (nodeChild.Attributes?["name"]?.Value != null)
                            rootNode.Text = nodeChild.Attributes["name"].Value;
                        else
                            throw new ArgumentException("У тега root должен быть аттрибут name!");
                        

                        InitXmlElement(nodeChild, rootNode);
                        break;
                    }
                    case "table":
                    case "visualisation":
                    case "leaf":
                    {
                        var leafNode = new TreeNode(nodeChild.InnerText);
                        if (nodeChild.Attributes?["tag"]?.Value != null)
                            leafNode.Tag = nodeChild.Attributes["tag"].Value;
                        else
                            throw new ArgumentException("У листовых элементов должен быть аттрибут tag");

                        treeViewNode.Nodes.Add(leafNode);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Считывает шаблон для визуализации подстановок
        /// </summary>
        /// <param name="htmlView">WebBrowser для отображения уроков</param>
        /// <param name="permulationVisualisationFilePath">Путь до шаблона</param>
        /// <exception cref="ArgumentException"></exception>
        public static void ReadPermulationVisualisationTemplate([NotNull] WebBrowser htmlView,
            string permulationVisualisationFilePath)
        {
            try
            {
                using (var streamReader = new StreamReader(permulationVisualisationFilePath))
                {
                    htmlView.DocumentText = streamReader.ReadToEnd();
                }
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException($@"По нет файла по переданному пути {permulationVisualisationFilePath}");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException($@"По нет файла по переданному пути {permulationVisualisationFilePath}");
            }
        }

        /// <summary>
        /// Считывает все анимации из папки
        /// </summary>
        /// <param name="folderPath">папка с анимациями </param>
        /// <returns>Массив анимаций</returns>
        public static IAnimation[] ReadAnimationsFromFolder([NotNull] string folderPath)
        {
            var dirInfo = new DirectoryInfo(folderPath);
            var res = new List<IAnimation>();
            var serializerRotation = new DataContractSerializer(typeof(RotationAnimation));
            var serializerSymmetry = new DataContractSerializer(typeof(SymmetryAnimation));

            foreach (var fileInfo in dirInfo.EnumerateFiles())
            {
                try
                {
                    using (var reader = new FileStream(fileInfo.FullName, FileMode.Open))
                    {
                        if (fileInfo.Name.StartsWith("r"))
                            res.Add((RotationAnimation) serializerRotation.ReadObject(reader));
                        else
                            res.Add((SymmetryAnimation) serializerSymmetry.ReadObject(reader));
                    }
                }
                catch (SerializationException)
                {
                    throw new ArgumentException("Невозможно корректно считать файлы из папки");
                }
                catch (IOException)
                {
                    throw new ArgumentException("Проблемы с доступом к папке!");
                }
            }

            return res.ToArray();
        }

    }
}
