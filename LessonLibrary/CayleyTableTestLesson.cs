using System;
using System.Collections.Generic;

namespace LessonLibrary
{
    /// <summary>
    /// Класс, представляющий тест с таблицей Кэли
    /// </summary>
    public class CayleyTableTestLesson
    {
        /// <summary>
        /// Результаты проверки на группу
        /// </summary>
        public enum CheckResult
        {
            Success,
            NotAssociativity,
            DontContainsNeutral,
            DontContainsInverts
        }

        /// <summary>
        /// Проверяет таблицу на то, является ли она группой
        /// </summary>
        /// <param name="cayleyTable">Таблица Кэли</param>
        /// <returns>Истину, если группа, ложь иначе</returns>
        public static CheckResult CheckTableOnGroup(string[,] cayleyTable)
        {
            if (cayleyTable.GetLength(0) != cayleyTable.GetLength(1))
                throw new Exception("Таблица должна быть квадратной!");

            var columnIndexies = new Dictionary<string, int>();
            for (var i = 1; i < cayleyTable.GetLength(1); i++)
            {
                if (columnIndexies.ContainsKey(cayleyTable[0, i]))
                    throw new Exception("Элементы в таблице не должны повторяться");

                columnIndexies[cayleyTable[0, i]] = i;
            }

            var rowIndexies = new Dictionary<string, int>();
            for (var i = 1; i < cayleyTable.GetLength(0); i++)
            {
                if (rowIndexies.ContainsKey(cayleyTable[i, 0]))
                    throw new Exception("Элементы в таблице не должны повторяться");

                rowIndexies[cayleyTable[i, 0]] = i;
            }

            foreach (var element in rowIndexies.Keys)
            {
                if (!columnIndexies.ContainsKey(element))
                    throw new Exception("В столбцах и строках дожны быть одинаковые элементы");
            }

            foreach (var element in columnIndexies.Keys)
            {
                if (!rowIndexies.ContainsKey(element))
                    throw new Exception("В столбцах и строках дожны быть одинаковые элементы");
            }

            if (!CheckAssociativity(cayleyTable, columnIndexies, rowIndexies))
                return CheckResult.NotAssociativity;

            if (!CheckNeutralElement(cayleyTable, columnIndexies, rowIndexies, out var neutralElement))
                return CheckResult.DontContainsNeutral;

            return !CheckInverts(cayleyTable, columnIndexies, rowIndexies, neutralElement)
                ? CheckResult.DontContainsInverts : CheckResult.Success;
        }

        /// <summary>
        /// Проверяет на присутствие нейтрального элемента
        /// </summary>
        /// <param name="cayleyTable">таблица кэли</param>
        /// <param name="columnIndexies">Индексы элементов в столбцах</param>
        /// <param name="rowIndexies">Индексы элементов строках</param>
        /// <param name="neutralElement">Нейтральный элемент</param>
        /// <returns>Истина, если есть нейтральный</returns>
        private static bool CheckNeutralElement(string[,] cayleyTable, IReadOnlyDictionary<string, int> columnIndexies, 
            IReadOnlyDictionary<string, int> rowIndexies, out string neutralElement)
        {
            neutralElement = null;
            for (var neutralIndex = 1; neutralIndex < cayleyTable.GetLength(0); neutralIndex++)
            {
                var isNeutral = true;
                for (var elementIndex = 1; elementIndex < cayleyTable.GetLength(1); elementIndex++)
                {
                    if (cayleyTable[neutralIndex, elementIndex] != cayleyTable[0, elementIndex])
                    {
                        isNeutral = false;
                        break;
                    }

                    var elementRowIndex = rowIndexies[cayleyTable[0, elementIndex]];
                    var neutralColumnIndex = columnIndexies[cayleyTable[neutralIndex, 0]];
                    if (cayleyTable[elementRowIndex, neutralColumnIndex] == cayleyTable[neutralIndex, elementIndex]) continue;

                    isNeutral = false;
                    break;
                }

                if (!isNeutral) continue;

                neutralElement = cayleyTable[neutralIndex, 0];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет таблицу на ассоциативность
        /// </summary>
        /// <param name="cayleyTable">Таблица кэли</param>
        /// <param name="columnIndexies">Индексы элементов в колонках</param>
        /// <param name="rowIndexies">Индексы элементов в строках</param>
        /// <returns>Истина, если таблица ассоциативна, ложь иначе</returns>
        private static bool CheckAssociativity(string[,] cayleyTable, IReadOnlyDictionary<string, int> columnIndexies,
            IReadOnlyDictionary<string, int> rowIndexies)
        {
            for (var firstIndex = 1; firstIndex < cayleyTable.GetLength(0); firstIndex++)
            {
                for (var secondIndex = 1; secondIndex < cayleyTable.GetLength(0); secondIndex++)
                {
                    for (var thirdIndex = 1; thirdIndex < cayleyTable.GetLength(0); thirdIndex++)
                    {
                        var firstSecondResult = cayleyTable[firstIndex, columnIndexies[cayleyTable[secondIndex, 0]]];
                        var leftPartResult = cayleyTable[rowIndexies[firstSecondResult], columnIndexies[cayleyTable[thirdIndex, 0]]];

                        var secondThirdResult = cayleyTable[secondIndex, columnIndexies[cayleyTable[thirdIndex, 0]]];
                        var rightPartResult = cayleyTable[firstIndex, columnIndexies[secondThirdResult]];

                        if (leftPartResult != rightPartResult)
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Проверяет таблицу на наличее обратных
        /// </summary>
        /// <param name="cayleyTable">Таблица Кэли</param>
        /// <param name="columnIndexies">Индексы элементов в колонке</param>
        /// <param name="rowIndexies">Индексы элементов в строках</param>
        /// <param name="neutralElement">Нейтральный элемент</param>
        /// <returns>Истина, если для всех есть обратный, ложь иначе</returns>
        private static bool CheckInverts(string[,] cayleyTable, IReadOnlyDictionary<string, int> columnIndexies,
            IReadOnlyDictionary<string, int> rowIndexies, string neutralElement)
        {
            for (var elementIndex = 1; elementIndex < cayleyTable.GetLength(0); elementIndex++)
            {
                var isHasInvert = false;
                for (var invertIndex = 1; invertIndex < cayleyTable.GetLength(1); invertIndex++)
                {
                    if (cayleyTable[elementIndex, invertIndex] != neutralElement) continue;

                    var elementColumnIndex = columnIndexies[cayleyTable[elementIndex, 0]];
                    var invertRowIndex = rowIndexies[cayleyTable[0, invertIndex]];

                    if (cayleyTable[invertRowIndex, elementColumnIndex] == neutralElement)
                        isHasInvert = true;
                }

                if (!isHasInvert)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Изначальная таблица
        /// </summary>
        public string[,] StartTable { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="table">таблица</param>
        public CayleyTableTestLesson(string[,] table)
        {
            StartTable = table;
        }
    }
}