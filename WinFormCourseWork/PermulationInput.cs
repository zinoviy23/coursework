using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// <summary>
        /// Максимальная длина подстановки
        /// </summary>
        private const int MaxPermulationLength = 15;

        /// <summary>
        /// Ширина элемента
        /// </summary>
        private const int ElementWidth = 20;

        /// <summary>
        /// Текущая длина подстаноки
        /// </summary>
        private int _permulationLength;

        /// <summary>
        /// Подстановка, которую ввёл пользователь
        /// </summary>
        public Permulation ResultPermulation { get; private set; }

        /// <summary>
        /// Labelы для верхней части подстановки
        /// </summary>
        private readonly Label[] _headerLabels;

        /// <summary>
        /// TextBoxы для нижней части подстановки
        /// </summary>
        private readonly TextBox[] _bottomTextBoxs;

        public PermulationInput()
        {
            InitializeComponent();
            
            _headerLabels = new Label[MaxPermulationLength];
            for (var i = 0; i < _headerLabels.Length; i++)
            {
                _headerLabels[i] = new Label();
            }

            _bottomTextBoxs = new TextBox[MaxPermulationLength];
            for (var i = 0; i < _bottomTextBoxs.Length; i++)
            {
                _bottomTextBoxs[i] = new TextBox {TextAlign = HorizontalAlignment.Center};
            }
        }

        private void PermulationInput_Load(object sender, EventArgs e)
        {
            permulationLengthComboBox.SelectedItem = "3";
        }

        private void PermulationLengthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReleaseHeadersAndBottoms(_permulationLength);
            _permulationLength = int.Parse(permulationLengthComboBox.SelectedItem.ToString());
            InitPermulationHeaderLabelByLength(_permulationLength);
            InitPermulationBottomByLength(_permulationLength);

            rightParenthesisLabel.Left = permulationPanel.Right;
            permulationLengthComboBox.Left = rightParenthesisLabel.Right + 10;
            ClientSize = new Size(permulationLengthComboBox.Right + 10, ClientSize.Height);
            buttonOk.Left = ClientSize.Width / 2 - buttonOk.Width / 2;

            buttonOk.TabIndex = _permulationLength;
            permulationLengthComboBox.TabIndex = _permulationLength + 1;

            _bottomTextBoxs[0].Select();
        }

        /// <summary>
        /// Используя длину, выводит на экран верхнюю часть подстановки
        /// </summary>
        /// <param name="length"></param>
        private void InitPermulationHeaderLabelByLength(int length)
        {
            var x = 2;
            for (var i = 0; i < length; i++)
            {
                _headerLabels[i].Visible = true;
                _headerLabels[i].Text = (i + 1).ToString();
                _headerLabels[i].Top = 5;
                _headerLabels[i].Left = x;
                _headerLabels[i].Parent = permulationPanel;
                x += ElementWidth;
                _headerLabels[i].BringToFront();
            }

            permulationPanel.Width = x;
        }

        /// <summary>
        /// Используя длину, задаёт TextBoxы для ввода нижней части подстановки
        /// </summary>
        /// <param name="length">Длина подстановки</param>
        private void InitPermulationBottomByLength(int length)
        {
            for (var i = 0; i < length; i++)
            {
                _bottomTextBoxs[i].Visible = true;
                _bottomTextBoxs[i].Parent = permulationPanel;
                _bottomTextBoxs[i].Top = 20;
                _bottomTextBoxs[i].Width = ElementWidth - 1;
                _bottomTextBoxs[i].Left = _headerLabels[i].Left + 1;
                _bottomTextBoxs[i].TabIndex = i;
                _bottomTextBoxs[i].MaxLength = length.ToString().Length;
                _bottomTextBoxs[i].BringToFront();
            }
        }

        /// <summary>
        /// Убирает элементы старой подстановки
        /// </summary>
        /// <param name="prevPermulationLength">Длина старой подстановки</param>
        private void ReleaseHeadersAndBottoms(int prevPermulationLength)
        {
            for (var i = 0; i < prevPermulationLength; i++)
            {
                _headerLabels[i].Visible = false;
                _bottomTextBoxs[i].Visible = false;
                _bottomTextBoxs[i].TabIndex = short.MaxValue;
            }
        }

        /// <summary>
        /// Получает список целых чисел, ввёденных в поля ввода
        /// </summary>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <returns>Нижняя строка подстановки, введённая пользователем</returns>
        private List<int> GetPermulationBottom()
        {
            var result = new List<int>();
            for (var i = 0; i < _permulationLength; i++)
            {
                var tmp = int.Parse(_bottomTextBoxs[i].Text);
                if (tmp < 1 || tmp > _permulationLength)
                    throw new OverflowException();

                result.Add(tmp);
            }

            return result;
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
                ResultPermulation = new Permulation(GetPermulationBottom());
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show(@"В каждом поле должно стоять натуральное число!", @"Ошибка!", MessageBoxButtons.OK,
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
