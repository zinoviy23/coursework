using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormCourseWork
{
    public partial class PolygonSizeInput : Form
    {
        public int PolygonSize { get; private set; }

        private bool _hasError;

        public PolygonSizeInput()
        {
            InitializeComponent();
            trackBar.Value = 6;
            textBox.Focus();
        }

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

        private void TrackBarOnValueChanged(object sender, EventArgs e)
        {
            textBox.Text = trackBar.Value.ToString();
        }

        private void PolygonSizeInputOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !_hasError)
                Close();
        }

        private void ButtonOnClick(object sender, EventArgs e)
        {
            if (!_hasError) Close();
        }

        private void TextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !_hasError) Close();
        }

        private void TrackBarOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !_hasError) Close();
        }
    }
}
