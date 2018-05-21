using System;
using System.IO;
using System.Windows.Forms;
using WinFormCourseWork.Users;

namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для настроек приложения
    /// </summary>
    internal static class MainFormSettingsLoader
    {
        /// <summary>
        /// Загружает настройки
        /// </summary>
        public static void LoadSettings()
        {
            if (File.Exists(MainFormPathes.SettingsFilePath))
            {
                try
                {
                    using (var settingsFileStream =
                        new FileStream(MainFormPathes.SettingsFilePath, FileMode.Open, FileAccess.Read))
                    {
                        Settings.ReadSettingsFromStream(settingsFileStream);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}Установлены настройки по умолчанию", @"Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings.CreateEmpty();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}Установлены настройки по умолчанию", @"Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings.CreateEmpty();
                }
            }
            else
            {
                MessageBox.Show(@"Отсутсвует файл настроек. Установлены настройки по умолчанию", @"Предупреждение!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Settings.CreateEmpty();
            }
        }

        /// <summary>
        /// Записывает настройки
        /// </summary>
        public static void WriteSettings()
        {
            try
            {
                using (var settingsFileStream =
                    new FileStream(MainFormPathes.SettingsFilePath, FileMode.Create, FileAccess.Write))
                {
                    Settings.WriteToStream(settingsFileStream);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($@"Не удалось записать настройки. {'\n'}" + ex.Message, @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($@"Не удалось записать настройки. {'\n'}" + ex.Message, @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загружает список пользователей
        /// </summary>
        public static void LoadUsersTables()
        {
            if (File.Exists(MainFormPathes.UsersFile))
            {
                try
                {
                    using (var usersFileStream = new FileStream(MainFormPathes.UsersFile, FileMode.Open, FileAccess.Read))
                    {
                        UsersTables.ReadUsersFromFile(usersFileStream);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}Список пользователей создан заново.", @"Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UsersTables.CreateEmptyInstance();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}Список пользователей создан заново.", @"Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UsersTables.CreateEmptyInstance();
                }
            }
            else
            {
                UsersTables.CreateEmptyInstance();
            }
        }

        /// <summary>
        /// Сохраняет список пользователей
        /// </summary>
        public static void SaveUsersTables()
        {
            try
            {
                using (var usersFileStream =
                    new FileStream(MainFormPathes.UsersFile, FileMode.Create, FileAccess.Write))
                {
                    UsersTables.WriteUsersInfo(usersFileStream);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($@"Не удалось записать список пользоватлей. {'\n'}" + ex.Message, @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($@"Не удалось записать список пользователей. {'\n'}" + ex.Message, @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загрузка пользователя
        /// </summary>
        /// <param name="userPath">путь до файла</param>
        public static void LoadUser(string userPath)
        {
            if (userPath != null && File.Exists(userPath))
            {
                try
                {
                    using (var userFileStream = new FileStream(userPath, FileMode.Open, FileAccess.Read))
                    {
                        Settings.CurrentUser = new User(userFileStream);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}Пользователь создан заново", @"Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings.CurrentUser = new User(Settings.CurrentUserName);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}Пользователь создан заново", @"Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings.CurrentUser = new User(Settings.CurrentUserName);
                }
            }
            else
            {
                Settings.CurrentUser = new User(Settings.CurrentUserName);
            }
        }

        /// <summary>
        /// Сохраняет пользователя
        /// </summary>
        public static void SaveCurrentUser()
        {
            try
            {
                var userPath = UsersTables.GetUserFileName(Settings.CurrentUserName);
                if (userPath == null)
                    return;

                using (var userFileStream =
                    new FileStream(MainFormPathes.UserFolderPath + userPath, FileMode.Create, FileAccess.Write))
                {
                    Settings.CurrentUser.WriteUser(userFileStream);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($@"Не удалось записать пользователя. {'\n'}" + ex.Message, @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($@"Не удалось записать пользователя. {'\n'}" + ex.Message, @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}