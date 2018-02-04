using System;
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
    }
}
