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
            tbInput.Text += b.Text;
        }

        private void on_click_operand(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            //tbInput.SelectedText += b.Text;
            tbInput.Text += b.Text;
        }

        private void on_click_function(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            tbInput.Text += b.Text + "(";
        }

        private void on_click_clear(object sender, EventArgs e)
        {
            tbInput.Clear();
        }

        private void on_click_equal(object sender, EventArgs e)
        {
            getResult();
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) getResult();
        }

        private void getResult()
        {
            string result = calculator.calculate(tbInput.Text, out Color color);
            lblResult.ForeColor = color;
            lblResult.Text = result;
        }

        private void on_click_copy(object sender, EventArgs e)
        {
            Clipboard.SetText(lblResult.Text);
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            tbInput.Text = tbInput.Text.Remove(tbInput.Text.Length - 1);
        }

        
    }
    // ʃ
}
