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
        int caretIndex;
        string newText;

        public MainView()
        {
            InitializeComponent();
            ActiveControl = tbInput;
            calculator = new Calculator();
        }

        private void on_click_number(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            caretIndex = tbInput.SelectionStart;
            newText = tbInput.Text.Substring(0, caretIndex) + b.Text + tbInput.Text.Substring(caretIndex, tbInput.Text.Length - caretIndex);
            tbInput.Text = newText;
            ActiveControl = tbInput;
            tbInput.SelectionStart = caretIndex + 1;
            tbInput.SelectionLength = 0;
        }

        private void on_click_operand(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            caretIndex = tbInput.SelectionStart;
            newText = tbInput.Text.Substring(0, caretIndex) + b.Text + tbInput.Text.Substring(caretIndex, tbInput.Text.Length - caretIndex);
            tbInput.Text = newText;
            ActiveControl = tbInput;
            tbInput.SelectionStart = caretIndex + 1;
            tbInput.SelectionLength = 0;
        }

        private void on_click_function(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            caretIndex = tbInput.SelectionStart;
            newText = tbInput.Text.Substring(0, caretIndex) + b.Text + '(' + tbInput.Text.Substring(caretIndex, tbInput.Text.Length - caretIndex);
            tbInput.Text = newText;
            ActiveControl = tbInput;
            tbInput.SelectionStart = caretIndex + 4;
            tbInput.SelectionLength = 0;
        }

        private void on_click_inverse_function(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string func = b.Text.Substring(0, 3);
            caretIndex = tbInput.SelectionStart;
            newText = tbInput.Text.Substring(0, caretIndex) + "arc" + func + "(" + tbInput.Text.Substring(caretIndex, tbInput.Text.Length - caretIndex);
            tbInput.Text = newText;
            ActiveControl = tbInput;
            tbInput.SelectionStart = caretIndex + 7;
            tbInput.SelectionLength = 0;
        }

        private void on_click_backspace(object sender, EventArgs e)
        {
            tbInput.Text = tbInput.Text.Remove(tbInput.Text.Length - 1);
        }

        private void on_click_log(object sender, EventArgs e)
        {
            caretIndex = tbInput.SelectionStart;
            newText = tbInput.Text.Substring(0, caretIndex) + "log[10](" + tbInput.Text.Substring(caretIndex, tbInput.Text.Length - caretIndex);
            tbInput.Text = newText;
            ActiveControl = tbInput;
            tbInput.SelectionStart = caretIndex + 8;
            tbInput.SelectionLength = 0;
        }

        private void on_click_root(object sender, EventArgs e)
        {
            caretIndex = tbInput.SelectionStart;
            newText = tbInput.Text.Substring(0, caretIndex)+ "√[2]("+tbInput.Text.Substring(caretIndex,tbInput.Text.Length-caretIndex);
            tbInput.Text = newText;
            ActiveControl = tbInput;
            tbInput.SelectionStart = caretIndex + 5;
            tbInput.SelectionLength = 0;
        }

        private void on_click_factorial(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            caretIndex = tbInput.SelectionStart;
            newText = tbInput.Text.Substring(0, caretIndex) + "!" + tbInput.Text.Substring(caretIndex, tbInput.Text.Length - caretIndex);
            tbInput.Text = newText;
            ActiveControl = tbInput;
            tbInput.SelectionStart = caretIndex + 1;
            tbInput.SelectionLength = 0;
        }

        private void on_click_equal(object sender, EventArgs e)
        {
            getResult();
        }

        private void on_click_ans(object sender, EventArgs e)
        {
            caretIndex = tbInput.SelectionStart;
            newText = tbInput.Text.Substring(0, caretIndex) + tbResult.Text + tbInput.Text.Substring(caretIndex, tbInput.Text.Length - caretIndex);
            tbInput.Text = newText;
            ActiveControl = tbInput;
            tbInput.SelectionStart = caretIndex + tbResult.Text.Length;
            tbInput.SelectionLength = 0;
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

        private void tbResult_KeyPress(object sender, KeyPressEventArgs e)
        {
            // preventing user from editing the result text box
            if (e.KeyChar != 'c' - 96) // except: CTRL + C (copy result)
            {
                e.Handled = true; 
            }
        }

        private void getResult()
        {
            string result = calculator.calculate(tbInput.Text, out Color color);
            tbResult.ForeColor = color;
            tbResult.Text = result;
        }
  
    }
    // ʃ
}
