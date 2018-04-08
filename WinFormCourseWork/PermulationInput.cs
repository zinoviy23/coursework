using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using LessonLibrary.Permulation;

namespace WinFormCourseWork
{
    /// <inheritdoc />
    /// <summary>
    /// Форма для ввода подстановки
    /// </summary>
    public partial class PermulationInput : Form
    {
        private int _permulationLength;

        /// <summary>
        /// Подстановка, которую ввёл пользователь
        /// </summary>
        public Permulation ResulPermulation { get; private set; }

        public PermulationInput()
        {
            InitializeComponent();
        }

        private void PermulationInput_Load(object sender, EventArgs e)
        {
            permulationLengthComboBox.SelectedItem = "3";
        }

        private void PermulationLengthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _permulationLength = int.Parse(permulationLengthComboBox.SelectedItem.ToString());
            InitPermulationHeaderLabelByLength(_permulationLength);
            permulationTextBox.Width = permulationHead.Width;
            rightParenthesisLabel.Left = permulationTextBox.Right + 3;
        }

        /// <summary>
        /// Используя длину, выводит на экран верхнюю часть подстановки
        /// </summary>
        /// <param name="length"></param>
        private void InitPermulationHeaderLabelByLength(int length)
        {
            var sb = new StringBuilder("");
            for (var i = 0; i < length; i++)
            {
                sb.Append(i + 1).Append(" ");
            }

            sb.Remove(sb.Length - 1, 1);
            permulationHead.Text = sb.ToString();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку ОК
        /// </summary>
        /// <param name="sender">Кнопка</param>
        /// <param name="e">Параметры события</param>
        /// <remarks>Достаёт текст из TextBox и преобразует его в подстановку,
        ///  или выводит сообщение об ошибке</remarks>
        private void ButtonOk_Click(object sender, EventArgs e)
        {
            try
            {
                var permulation = new Permulation(new List<string>(permulationTextBox.Text.Split(' '))
                    .FindAll(s => s.Trim() != "")
                    .ConvertAll(int.Parse));
                ResulPermulation = permulation;
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Введите натуральные числа через пробел!", @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (OverflowException)
            {
                MessageBox.Show($@"Числа должны быть от 1 до {_permulationLength}!", @"Ошибка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
