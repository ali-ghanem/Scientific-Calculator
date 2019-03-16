using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class MainView : Form
    {
        Calculator calculator;


        public MainView()
        {
            InitializeComponent();
            ActiveControl = tbInput;
            calculator = new Calculator();
        }

        private void on_click_number(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            tbInput.AppendText(b.Text);

        }

        private void on_click_operand(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            tbInput.AppendText(b.Text);
            ActiveControl = tbInput;
        }

        private void on_click_function(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            tbInput.AppendText(b.Text + '(');
            ActiveControl = tbInput;
        }

        private void on_click_inverse_function(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string func = b.Text.Substring(0, 3);
            tbInput.AppendText("arc" + func + "(");
            ActiveControl = tbInput;
        }

        private void on_click_backspace(object sender, EventArgs e)
        {
            tbInput.Text = tbInput.Text.Remove(tbInput.Text.Length - 1);
        }

        private void on_click_log(object sender, EventArgs e)
        {
            tbInput.AppendText("log[10](");
            ActiveControl = tbInput;
        }

        private void on_click_root(object sender, EventArgs e)
        {
            tbInput.AppendText("√[2](");
            ActiveControl = tbInput;
        }

        private void on_click_factorial(object sender, EventArgs e)
        {
            tbInput.AppendText("!");
            ActiveControl = tbInput;
        }

        private void on_click_equal(object sender, EventArgs e)
        {
            getResult();
        }

        private void on_click_ans(object sender, EventArgs e)
        {
            tbInput.AppendText(tbResult.Text);
            ActiveControl = tbInput;
        }

        private void on_click_clear(object sender, EventArgs e)
        {
            tbInput.Clear();
            ActiveControl = tbInput;
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true; // disable the beep sound 
                getResult();
            }
        }

        private void getResult()
        {
            string result = calculator.calculate(tbInput.Text, out Color color);
            tbResult.ForeColor = color;
            tbResult.Text = result;

        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
      
        }

    }
    // ʃ
}
