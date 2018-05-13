using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormCourseWork;
using WinFormCourseWork.Users;

namespace UnitTests
{
    [TestClass]
    public class UsersTests
    {
        [TestMethod]
        public void UserWritingReadingTest()
        {
            var user = new User("Sanya");

            user.AddAnswer("lol", 1, "kek");
            user.AddAnswer("kek", 3, "bib");
            user.AddAnswer("lol", 2, "kek");

            using (var stream = new FileStream("user.json", FileMode.Create, FileAccess.Write))
            {
                user.WriteUser(stream);
            }

            User user1;
            using (var stream = new FileStream("user.json", FileMode.Open, FileAccess.Read))
            {
                user1 = new User(stream);
            }

            foreach (var test in user.Tests)
            {
                Assert.IsTrue(user1.Tests.ContainsKey(test.Key));

                var otherUserTest = user1.Tests[test.Key];
                var tmpTest = test.Value;

                Assert.IsTrue((tmpTest.AnswersCount == otherUserTest.AnswersCount)
                              && !tmpTest.Answers.Except(otherUserTest.Answers).Any());
            }
        }

        [TestMethod]
        public void UsersTableTest()
        {
            UsersTables.CreateEmptyInstance();

            UsersTables.AddUser("sanya");
            UsersTables.AddUser("petya");

            using (var stream = new FileStream("users.json", FileMode.Create, FileAccess.Write))
            {
                UsersTables.WriteUsersInfo(stream);
            }

            using (var stream = new FileStream("users.json", FileMode.Open, FileAccess.Read))
            {
                UsersTables.ReadUsersFromFile(stream);
            }

            Assert.AreEqual(UsersTables.GetUserFileName("sanya"), "0.json");
        }

        [TestMethod]
        public void SettingsTest()
        {
            Settings.CreateEmpty();
            using (var stream = new FileStream("settings.json", FileMode.Create, FileAccess.Write))
            {
                Settings.WriteToStream(stream);
            }

            using (var stream = new FileStream("settings.json", FileMode.Open, FileAccess.Read))
            {
                Settings.ReadSettingsFromStream(stream);   
            }

            Assert.AreEqual(0, Settings.AnswersCount);
        }
    }

    

}
