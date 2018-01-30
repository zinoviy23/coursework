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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            htmlView.DocumentText =
            @"<html>
                <body>
                    <h1> Теория групп! </h1>
                    <h1> Теория групп! </h1>
<h1> Теория групп! </h1>
<h1> Теория групп! </h1>
<h1> Теория групп! </h1>
<h1> Теория групп! </h1>
<h1> Теория групп! </h1>

<h1> Теория групп! </h1>
<h1> Теория групп! </h1>
<h1> Теория групп! </h1>
<h1> Теория групп! </h1>
                </body>
            </html>";
        }
    }
}
