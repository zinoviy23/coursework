using Microsoft.VisualStudio.TestTools.UnitTesting;
using LessonLibrary;

namespace UnitTests
{
    [TestClass]
    public class LessonLibraryTests
    {
        /// <summary>
        /// Проверяет на правильные ответы в checkbox
        /// </summary>
        [TestMethod]
        public void CompareCheckBoxAnswersTestTrue()
        {
            Assert.IsTrue(TestAnswers.CompareCheckBoxAnswers("132", "123"),
                "TestAnswers.CompareCheckBoxAnswers('132', '123') не работает");
            Assert.IsTrue(TestAnswers.CompareCheckBoxAnswers("12345", "31425"),
                "TestAnswers.CompareCheckBoxAnswers('12345', '31425') не работает");
        }

        /// <summary>
        /// Проверяет на неправильные ответы в checkbox
        /// </summary>
        [TestMethod]
        public void CompareCheckBoxAnswersTestFalse()
        {
            Assert.IsFalse(TestAnswers.CompareCheckBoxAnswers("12345", "324"),
                "TestAnswers.CompareCheckBoxAnswers('12345', '324') не работает");
            Assert.IsFalse(TestAnswers.CompareCheckBoxAnswers("123", "3214"), 
                "TestAnswers.CompareCheckBoxAnswers('123', '3214') не работает");
        }

        /// <summary>
        /// Проверяет "проверку" на группу(правилтные таблицы)
        /// </summary>
        [TestMethod]
        public void CheckTableOnGroupTestSuccess()
        {
            var table = new[,]
            {
                {" ", "0", "1", "2" },
                {"0", "0", "1", "2" },
                {"1", "1", "2", "0" },
                {"2", "2", "0", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.Success,
                CayleyTableTestLesson.CheckTableOnGroup(table));

         
            var table2 = new[,]
            {
                {" ", "0", "1"},
                {"1", "1", "0" },
                {"0", "0", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.Success,
                CayleyTableTestLesson.CheckTableOnGroup(table2));
        }

        /// <summary>
        /// Проверяет "проверку" на группу(не выполняется ассоциативность)
        /// </summary>
        [TestMethod]
        public void CheckTableOnGroupTestNotAssociativity()
        {
            var table1 = new[,]
            {
                {" ", "0", "1", "2" },
                {"0", "0", "2", "1" },
                {"1", "1", "2", "0" },
                {"2", "2", "0", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.NotAssociativity,
                CayleyTableTestLesson.CheckTableOnGroup(table1));
        }
        
        /// <summary>
        /// Проверяет "проверку" на группу(нет нейтрального элемента)
        /// </summary>
        [TestMethod]
        public void CheckTableOnGroupTestDontContainsNeutral()
        {
            var table3 = new[,]
            {
                {" ", "0", "1"},
                {"1", "1", "1" },
                {"0", "1", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.DontContainsNeutral,
                CayleyTableTestLesson.CheckTableOnGroup(table3));
        }

        /// <summary>
        /// Проверяет "проверку" на группу(не для всех есть обратные)
        /// </summary>
        [TestMethod]
        public void CheckTableOnGroupTestDontContainsInverts()
        {
            var table4 = new[,]
            {
                {" ", "0", "1"},
                {"0", "0", "1" },
                {"1", "1", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.DontContainsInverts,
                CayleyTableTestLesson.CheckTableOnGroup(table4));
        }

        /// <summary>
        /// Проверяет считывание таблицы
        /// </summary>
        [TestMethod]
        public void ReadCayleyTableTestLessonTest()
        {
            var expectedTable = new[,]
            {
                {"\\", "0", "1"},
                {"0", "0", "1"},
                {"1", "1", "0"}
            };
            var cayleyTable = LessonReader.ReadCayleyTableTestLesson(@"lessons\CayleyTables\table_test.xml");
            for (var i = 0; i < expectedTable.GetLength(0); i++)
                for (var j = 0; j < expectedTable.GetLength(1); j++)
                    Assert.AreEqual(expectedTable[i, j], cayleyTable.StartTable[i, j]);
        }
    }
}
