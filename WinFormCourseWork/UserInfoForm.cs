﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormCourseWork
{
    /// <inheritdoc />
    /// <summary>
    /// Форма для информациии о пользователе
    /// </summary>
    public partial class UserInfoForm : Form
    {
        public UserInfoForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события загрузки окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInfoFormOnLoad(object sender, EventArgs e)
        {
            nameLabel.Text = Settings.CurrentUserName;
            countLabel.Text = $@"{Settings.CurrentUser.AnswersCount}/{Settings.AnswersCount}";

            ClientSize = new Size(Math.Max(nameLabel.Right, countLabel.Right) + 10,  ClientSize.Height);
        }
    }
}
