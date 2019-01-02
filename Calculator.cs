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

        private Tuple<double, int> nextNumber(String text, int index)
        {
            double next = 0.0;
            int i;
            for (i = index; i < text.Length; i++)
                if (!Char.IsDigit(text[i]) && text[i] != '.' && (text[i] != '-'   //1+-2-5        d+.+(-*p)   -d*-.*(--+)
                || Char.IsDigit(text[i - 1])))
                    break;
            Console.WriteLine("in next: " + text.Substring(index, i - index));
            next = double.Parse(text.Substring(index, i - index));
            return Tuple.Create(next, i);
        }

        private Tuple<double, int> prevNumber(String text, int index)
        {
            double prev = 0.0;
            int i;
            for (i = index; i > -1; i--)
                
                //p: is number, q: dot, r: minus sign, k: minus in 0
                // p + q + (r * k)
                // ~p * ~q * (~r + ~k)
                if (!Char.IsDigit(text[i]) && text[i] != '.' && (text[i] != '-' || i != 0))
                    break;

            Console.WriteLine("in prev: " + text.Substring(i + 1, index - i));
            prev = double.Parse(text.Substring(i + 1, index - i));
            return Tuple.Create(prev, i + 1);
        }

        private string getNextOperand(string text)
        {
            if (text.Contains("*")) return "*";
            else if (text.Contains("/")) return "/";
            else if (text.Contains("-") && text.LastIndexOf("-") != 0
                && Char.IsDigit(text[text.LastIndexOf("-") - 1])) return "-";
            else return "+";
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
            else return firstNumber - secondNumber;
        }

        private bool contains(string text, string subText)
        {
            return text.IndexOf(subText, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private string getNextFunction(string text)
        {
            if (contains(text, "sin")) return "sin";
            else if (contains(text, "cos")) return "cos";
            else if (contains(text, "tan")) return "tan";
            else return null;
        }

        private int getFunctionEnd(string text, int startIndex)
        {
            int count = 0;
            int endIndex;
            Console.WriteLine("in func end:  text: {0},  startIndex: {1}", text,startIndex);
            for (endIndex = text.IndexOf('(', startIndex); endIndex < text.Length; endIndex++)
            {
                if (text[endIndex] == '(') count++;
                else if (text[endIndex] == ')') count--;
                if (count == 0) break;
            }
            return endIndex;
        }

        private double executeFunc(string func, string funcText)
        {
            int from = funcText.IndexOf('(') + 1;
            int to = funcText.LastIndexOf(')') - from;
            double value = double.Parse(calc(funcText.Substring(from, to)));
            if (func.Equals("sin")) return Math.Sin(value);
            else if (func.Equals("cos")) return Math.Cos(value);
            else return Math.Tan(value);
        }


        private string calc(string text)
        {
            try
            {
                // clean text
                text = Regex.Replace(text, @"\s+", "");
                text = text.Replace("--", "+");
                text = text.Replace("-+", "-");
                text = text.Replace("+-", "-");
                Console.WriteLine(text);
                string tempText;

                string function = getNextFunction(text);
                if (function != null)
                {
                    int startIndex = text.IndexOf(function, StringComparison.OrdinalIgnoreCase);
                    int endIndex = getFunctionEnd(text, startIndex);
                    string funcText = text.Substring(startIndex, endIndex - startIndex + 1);
                    Console.WriteLine("func: {0}, start: {1}, end: {2}, funcText: {3}",function,startIndex, endIndex, funcText);
                    tempText = text.Substring(0, startIndex)
                             + ((startIndex > 0 && Char.IsDigit(text[startIndex - 1])) ? "*" : "") // e.g: 5cos(0) = 5*cos(0)
                             + executeFunc(function, funcText);

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
                            Console.WriteLine("(: temp: {0}",tempText);
                            if (endIndex + 1 < text.Length)
                                tempText += text.Substring(endIndex + 1, text.Length - endIndex - 1);
                            return calc(tempText);
                        }
                    }
                }

                if (!thereIsOperand(text)) return text;

                string operand = getNextOperand(text);

                int index = getIndexOfOperand(text, operand);
                //Console.WriteLine("op: {0}  in: {1}", operand, index);

                Tuple<double, int> nextElement = nextNumber(text, index + 1);
                Tuple<double, int> previousElement = prevNumber(text, index - 1);

                double firstNumber = previousElement.Item1;
                double secondNumber = nextElement.Item1;

                int from = previousElement.Item2;
                int to = nextElement.Item2;

                double result = applyOperand(operand, firstNumber, secondNumber);

                tempText = text.Substring(0, from) + result + text.Substring(to, text.Length - to);
                // Console.WriteLine("2tT: " + tempText);
                return (calc(tempText));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return "invalid syntax";
            }
        }

        private bool thereIsOperand(string text)
        {
            return text.Contains("*") || text.Contains("+")
                || (text.Contains("-") && text.LastIndexOf("-") != 0) // in (-5): (-) is not operand
                || text.Contains("/");
        }

       

        public Tuple<string, Color> calculate(string text)
        {
            string result = calc(text);
            if (result.Equals("invalid syntax")) return Tuple.Create(result, Color.Red);
            else return Tuple.Create(result, Color.Green);
        }
    }
}
