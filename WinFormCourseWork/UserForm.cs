using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormCourseWork
{
    /// <inheritdoc />
    /// <summary>
    /// Форма для ввода имени пользователя
    /// </summary>
    public partial class UserForm : Form
    {
        /// <summary>
        /// Есть ли ошибка в форме, по умолчанию есть, так как поле ввода пустое
        /// </summary>
        private bool _isError = true;

        /// <summary>
        /// Выход по кнопке ок или нет
        /// </summary>
        private bool _goodExit;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; private set; }

        public UserForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Abort;
        }

        /// <summary>
        /// Обработчик события выхода из текстового поля. Проверяет есть ли ошибки.
        /// </summary>
        /// <param name="sender">объект</param>
        /// <param name="e">аргументы</param>
        private void UserTextBoxOnLeave(object sender, EventArgs e)
        {
            userTextBox.BackColor = SystemColors.Window;

            if (userTextBox.Text != "") return;
            SetError();
            _isError = true;
        }

        /// <summary>
        /// Задаёт подсказку для информации об ошибке
        /// </summary>
        private void SetError()
        {
            errorTooltip.RemoveAll();
            errorTooltip.SetToolTip(userTextBox, @"Имя не может быть пустым!");
            userTextBox.BackColor = Color.Red;
        }

        /// <summary>
        /// Обработчик события изменения текста в поле ввода
        /// </summary>
        /// <param name="sender">объект</param>
        /// <param name="e">аргументы</param>
        private void UserTextBoxOnTextChanged(object sender, EventArgs e)
        {
            errorTooltip.RemoveAll();
            userTextBox.BackColor = SystemColors.Window;
            _isError = false;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку. Выходит, если всё ок.
        /// </summary>
        /// <param name="sender">объект</param>
        /// <param name="e">аргументы</param>
        private void ButtonOkOnClick(object sender, EventArgs e)
        {
            if (_isError)
            {
                SetError();
                return;
            }

            _goodExit = true;
            UserName = userTextBox.Text;
            Close();
        }

        /// <summary>
        /// Обработчик события выхода из формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserFormOnFormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = _goodExit ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку в поле ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserTextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOkOnClick(null, null);
        }
    }
}
