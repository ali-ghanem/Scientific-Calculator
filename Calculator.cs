using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculator
{
    class Calculator
    {

        private double prevNumber(String text, int index, out int from)
        {
            double prev = 0.0;
            int i;
            for (i = index; i > -1; i--)

                //p: is number, q: dot, r: minus sign, k: minus in 0
                // p + q + (r * k)
                // ~p * ~q * (~r + ~k)
                if (!Char.IsDigit(text[i]) && text[i] != '.' && (text[i] != '-' || i != 0))
                    break;

            prev = double.Parse(text.Substring(i + 1, index - i));
            from = i + 1;
            return prev;
        }

        private double nextNumber(String text, int index, out int to)
        {
            double next = 0.0;
            int i;
            for (i = index; i < text.Length; i++)
                if (!Char.IsDigit(text[i]) && text[i] != '.' && (text[i] != '-'   //1+-2-5        d+.+(-*p)   -d*-.*(--+)
                || Char.IsDigit(text[i - 1])))
                    break;
            next = double.Parse(text.Substring(index, i - index));
            to = i;
            return next;
        }

        private string getNextOperand(string text)
        {
            if (text.Contains("^")) return "^";
            else if (text.Contains("/")) return "/";
            else if (text.Contains("*")) return "*";
            else if (text.Contains("-") && text.LastIndexOf("-") != 0
                && Char.IsDigit(text[text.LastIndexOf("-") - 1])) return "-";
            else if (text.Contains("+")) return "+";

            else return null;
        }

        private int getIndexOfOperand(string text, string operand)
        {
            if (operand.Equals("-")) return text.IndexOf("-", 1);
            else return text.IndexOf(operand);
        }


        private double applyOperand(string operand, double firstNumber, double secondNumber)
        {
            if (operand.Equals("*")) return firstNumber * secondNumber;
            else if (operand.Equals("/")) return firstNumber / secondNumber;
            else if (operand.Equals("+")) return firstNumber + secondNumber;
            else if (operand.Equals("^")) return Math.Pow(firstNumber, secondNumber);
            else return firstNumber - secondNumber;
        }

        private bool contains(string text, string subText)
        {
            return text.IndexOf(subText, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private string getNextFunction(string text)
        {
            if (contains(text, "√")) return "√";
            else if (contains(text, "log")) return "log";

            else if (contains(text, "arcsin")) return "arcsin";
            else if (contains(text, "arccos")) return "arccos";
            else if (contains(text, "arctan")) return "arctan";

            else if (contains(text, "sin")) return "sin";
            else if (contains(text, "cos")) return "cos";
            else if (contains(text, "tan")) return "tan";

            else return null;
        }

        private int getFunctionEnd(string text, int startIndex)
        {
            int count = 0;
            int endIndex;
            for (endIndex = text.IndexOf('(', startIndex); endIndex < text.Length; endIndex++)
            {
                if (text[endIndex] == '(') count++;
                else if (text[endIndex] == ')') count--;
                if (count == 0) break;
            }
            return endIndex;
        }

        private double executeFunction(string func, string funcText)
        {

            int from = funcText.IndexOf('(') + 1;
            int to = funcText.LastIndexOf(')') - from;
            string textValue = funcText.Substring(from, to);
            double value = double.Parse(calc(textValue));

            if (func.Equals("√"))
            {
                int f = funcText.IndexOf('[') + 1;
                int t = funcText.IndexOf(']') - f;
                double rootBase = double.Parse(funcText.Substring(f, t));
                return Math.Pow(value, 1 / rootBase);
            }

            else if (func.Equals("log"))
            {//e.g: log[10](8+2)
                int f = funcText.IndexOf('[') + 1;
                int t = funcText.IndexOf(']') - f;
                double logBase = double.Parse(funcText.Substring(f, t));
                return Math.Log(value, logBase);
            }

            else if (func.Equals("arcsin")) return Math.Asin(value);
            else if (func.Equals("arccos")) return Math.Acos(value);
            else if (func.Equals("arctan")) return Math.Atan(value);
            else if (func.Equals("sin")) return Math.Sin(value);
            else if (func.Equals("cos")) return Math.Cos(value);
            else if (func.Equals("tan")) return Math.Tan(value);

            else throw new Exception();
        }


        private string calc(string text)
        {
            try
            {
                // clean text:
                text = Regex.Replace(text, @"\s+", "");
                text = text.Replace("--", "+");
                text = text.Replace("-+", "-");
                text = text.Replace("+-", "-");
                text = text.Replace("π", "3.1415926535897932384626433832795");

                string tempText;

                // claculate the base e.g: log[1+1](8) = log[2](8)
                if (text.Contains('['))
                {
                    int startIndex = text.IndexOf('[');
                    int endIndex, count = 1;
                    for (endIndex = startIndex + 1; endIndex < text.Length; endIndex++)
                    {
                        if (text[endIndex] == '[') count++;
                        else if (text[endIndex] == ']') count--;
                        if (count == 0) break;
                    }
                    // log[2+1](5)
                    string baseText = text.Substring(startIndex + 1, endIndex - startIndex - 1);
                    string func = getNextFunction(baseText);
                    string op = getNextOperand(baseText);
                    if (func != null || op != null || baseText.Contains('!'))
                    {
                        tempText = text.Substring(0, startIndex + 1)
                                 + calc(baseText)
                                 + text.Substring(endIndex, text.Length - endIndex);
                        return calc(tempText);
                    }
                }


                string function = getNextFunction(text);
                if (function != null)
                {
                    int startIndex = text.IndexOf(function, StringComparison.OrdinalIgnoreCase);
                    int endIndex = getFunctionEnd(text, startIndex);
                    string funcText = text.Substring(startIndex, endIndex - startIndex + 1);
                    tempText = text.Substring(0, startIndex)
                             + ((startIndex > 0 && Char.IsDigit(text[startIndex - 1])) ? "*" : "") // e.g: 5cos(0) = 5*cos(0)
                             + executeFunction(function, funcText);

                    if (endIndex + 1 < text.Length)
                        tempText += text.Substring(endIndex + 1, text.Length - endIndex - 1);

                    return calc(tempText);
                }

                if (text.Contains('('))
                {
                    int startIndex = text.IndexOf('(');
                    int endIndex = text.IndexOf(')');

                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i] == '(') startIndex = i;

                        else if (text[i] == ')')
                        {
                            endIndex = i;
                            tempText = text.Substring(0, startIndex)
                                     + ((startIndex > 0 && Char.IsDigit(text[startIndex - 1])) ? "*" : "") // 5(1+2) = 5*(1+2)
                                     + calc(text.Substring(startIndex + 1, endIndex - startIndex - 1));
                            if (endIndex + 1 < text.Length)
                                tempText += text.Substring(endIndex + 1, text.Length - endIndex - 1);
                            return calc(tempText);
                        }
                    }
                }

                if (text.Contains("!"))
                {
                    int indx = text.IndexOf("!");
                    double number = prevNumber(text, indx - 1, out int from);
                    long res = factorial((int)number);
                    tempText = text.Substring(0, from) + res + text.Substring(indx + 1, text.Length - indx - 1);
                    return calc(tempText);
                }

                string operand = getNextOperand(text);
                if (operand != null)
                {
                    int index = getIndexOfOperand(text, operand);
                    double firstNumber = prevNumber(text, index - 1, out int from);
                    double secondNumber = nextNumber(text, index + 1, out int to);

                    double result = applyOperand(operand, firstNumber, secondNumber);

                    tempText = text.Substring(0, from) + result + text.Substring(to, text.Length - to);
                    return (calc(tempText));
                }
                return text;
            }
            catch (Exception e)
            {
                // Console.WriteLine(e.StackTrace);
                return "invalid syntax";
            }
        }

        public long factorial(int number)
        {
            long n = 1;
            for (int i = number; i > 1; i--)
                n *= i;
            return n;
        }

        public string calculate(string text, out Color color)
        {
            string result = calc(text);
            if (result.Equals("invalid syntax")) color = Color.Red;
            else color = Color.Green;
            return result;
        }
    }
}
