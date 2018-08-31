using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormCourseWork
{
    /// <inheritdoc />
    /// <summary>
    /// Форма для контроля главных форм
    /// </summary>
    public partial class ControlForm : Form
    {
        /// <summary>
        /// Текущая форма
        /// </summary>
        private MainForm _currentMainForm;

        public ControlForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события закрытия главного окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentMainFormOnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (_currentMainForm.IsUserExit)
            {
                _currentMainForm = new MainForm(CurrentMainFormOnFormClosed);
                _currentMainForm.Show();
            }
            else
            {
                Close();
            }
        }

        private void ControlForm_Load(object sender, EventArgs e)
        { 
            Opacity = 0.0;
            Hide();
            Size = new Size(0, 0);
            _currentMainForm = new MainForm(CurrentMainFormOnFormClosed);
            _currentMainForm.Show();
        }

        private void ControlFormOnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_currentMainForm != null && _currentMainForm.Disposing)
                _currentMainForm.Close();
        }
    }
}
