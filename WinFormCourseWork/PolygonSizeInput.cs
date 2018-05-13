using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormCourseWork
{
    public partial class PolygonSizeInput : Form
    {
        /// <summary>
        /// Размер полигона
        /// </summary>
        public int PolygonSize { get; private set; }

        /// <summary>
        /// Есть ли ошибка
        /// </summary>
        private bool _hasError;

        public PolygonSizeInput()
        {
            InitializeComponent();
            trackBar.Value = 6;
            textBox.Focus();
        }

        /// <summary>
        /// Обработчик события изменения текста. Проверяет на ошибки
        /// </summary>
        /// <param name="sender">объект</param>
        /// <param name="e">аругменты</param>
        private void TextBoxOnTextChanged(object sender, EventArgs e)
        {
            toolTip.RemoveAll();
            if (textBox.Text == "")
            {
                textBox.BackColor = Color.Red;
                toolTip.SetToolTip(textBox, @"Значение не должно быть пустым!");
                _hasError = true;
                return;
            }

            textBox.BackColor = SystemColors.Window;

            if (!int.TryParse(textBox.Text, out var count))
            {
                textBox.ForeColor = Color.Red;
                toolTip.SetToolTip(textBox, @"Значение должно быть числом!");
                _hasError = true;
            }
            else if (count < 3 || count > 20)
            {
                textBox.ForeColor = Color.Red;
                _hasError = true;
                toolTip.SetToolTip(textBox, @"Значение должно быть от 3 до 20!");
            }
            else
            {
                PolygonSize = count;
                textBox.ForeColor = Color.Black;
                trackBar.Value = count;
                _hasError = false;
            }
        }

        /// <summary>
        /// Изменение значения ползунка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBarOnValueChanged(object sender, EventArgs e)
        {
            textBox.Text = trackBar.Value.ToString();
        }

        /// <summary>
        /// Обработчик события нажатия клавиши в форме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolygonSizeInputOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !_hasError)
                Close();
        }

        /// <summary>
        /// Обработчик события нажатия на клавишу ок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOnClick(object sender, EventArgs e)
        {
            if (!_hasError) Close();
        }

        /// <summary>
        /// Обработчик события нажатия клавиши на текстбокс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !_hasError) Close();
        }

        /// <summary>
        /// Обработчик события нажатия клавиши на ползунок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBarOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !_hasError) Close();
        }
    }
}
