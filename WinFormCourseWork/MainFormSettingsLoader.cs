using System;
using System.IO;
using System.Windows.Forms;
using WinFormCourseWork.Users;

namespace WinFormCourseWork
{
    /// <summary>
    /// ����� ��� �������� ����������
    /// </summary>
    internal static class MainFormSettingsLoader
    {
        /// <summary>
        /// ��������� ���������
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
                    MessageBox.Show(ex.Message + $@"{'\n'}����������� ��������� �� ���������", @"������!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings.CreateEmpty();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}����������� ��������� �� ���������", @"������!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings.CreateEmpty();
                }
            }
            else
            {
                MessageBox.Show(@"���������� ���� ��������. ����������� ��������� �� ���������", @"��������������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Settings.CreateEmpty();
            }
        }

        /// <summary>
        /// ���������� ���������
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
                MessageBox.Show($@"�� ������� �������� ���������. {'\n'}" + ex.Message, @"������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($@"�� ������� �������� ���������. {'\n'}" + ex.Message, @"������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��������� ������ �������������
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
                    MessageBox.Show(ex.Message + $@"{'\n'}������ ������������� ������ ������.", @"������!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UsersTables.CreateEmptyInstance();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}������ ������������� ������ ������.", @"������!",
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
        /// ��������� ������ �������������
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
                MessageBox.Show($@"�� ������� �������� ������ ������������. {'\n'}" + ex.Message, @"������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($@"�� ������� �������� ������ �������������. {'\n'}" + ex.Message, @"������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// �������� ������������
        /// </summary>
        /// <param name="userPath">���� �� �����</param>
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
                    MessageBox.Show(ex.Message + $@"{'\n'}������������ ������ ������", @"������!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings.CurrentUser = new User(Settings.CurrentUserName);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message + $@"{'\n'}������������ ������ ������", @"������!",
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
        /// ��������� ������������
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
                MessageBox.Show($@"�� ������� �������� ������������. {'\n'}" + ex.Message, @"������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($@"�� ������� �������� ������������. {'\n'}" + ex.Message, @"������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}