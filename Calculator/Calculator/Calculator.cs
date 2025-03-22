using System;
using System.Drawing;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace HomeWork
{
    public class Programm
    {
        public static void Main()
        {
            var input = Console.ReadLine();
            try
            {
                var arr = Parser.Parse(input);
                var opers = new List<string>();
                var nums = new List<double>();

                DivideArr(nums, opers, arr);

                var n = new MultiCalculator(opers, nums);

                Console.WriteLine(n.Runner());
            }
            catch(Exception)
            {
                Console.WriteLine("Неправильный ввод");
            }


        }

        public static void DivideArr(List<double> nums, List<string> opers, List<string> arr)
        {
            foreach (string elem in arr)
            {
                if (new string[] { "+", "-", "*", "/" }.Contains(elem))
                    opers.Add(elem);
                else
                    nums.Add(double.Parse(elem));
            }
        }
    }

    public interface ICalculator
    {
        double Calculate(double fst, double scd);
    }

    public class SumCalculate : ICalculator
    {
        public double Calculate(double fst, double scd) => fst + scd;
    }

    public class DifferCalculate : ICalculator
    {
        public double Calculate(double fst, double scd) => fst - scd;
    }

    public class ProductCalculate : ICalculator
    {
        public double Calculate(double fst, double scd) => fst * scd;
    }

    public class QuotienCalculate : ICalculator
    {
        public double Calculate(double fst, double scd) => scd == 0 ? throw new DivideByZeroException() : fst / scd;
    }

    public class Calculator(double fst, double scd, ICalculator calc)
    {
        protected double fst = fst;
        protected double scd = scd;
        private static Dictionary<string, ICalculator> _dict = new Dictionary<string, ICalculator>()
        {
            {"+", new SumCalculate() },
            {"-", new DifferCalculate() },
            {"*", new ProductCalculate() },
            {"/", new QuotienCalculate() }

        };
        private ICalculator _Calc { get; set; } = calc;

        public double Calculate() => calc.Calculate(fst, scd);

        static public ICalculator ChooseOperation(string oper)
        {
            return _dict[oper];
        }

    }

    public static class Parser
    {
        public static List<string> Parse(string input)
        {
            var result = new List<string>();
            var opersWithoutMinus = new List<char>() { '+', '/', '*' };
            input = input.Replace(" ", "");
            int i = 0;
            while (i < input.Length)
            {
                if (input[i] == '-' && i == 0) {
                    result.Add(input.Substring(0, 2));
                    i += 2;
                }
                else if (input[i] == '-' && !opersWithoutMinus.Contains(input[i + 1]))
                {
                    if (opersWithoutMinus.Contains(input[i - 1]))
                    {
                        var r = GetIndex(input, i + 1) - i;
                        result.Add(input.Substring(i, r));
                        i += r;
                    }
                    else
                    {
                        result.Add(input.Substring(i, 1));
                        i += 1;
                    }
                        //result.Add(opersWithoutMinus.Contains(input[i - 1]) ? input.Substring(i, 2) : input.Substring(i, 1));
                }
                else if (opersWithoutMinus.Contains(input[i])) {
                    result.Add(input.Substring(i, 1));
                    i += 1;
                }
                else
                {
                    var r = GetIndex(input, i + 1) - i;
                    result.Add(input.Substring(i, r));
                    i += r;
                }
            }

            return result;
        }

        public static int GetIndex(string str, int l)
        {
            var r = l;
            while (r < str.Length)
            {
                if (new char[] { '-', '+', '*', '/' }.Contains(str[r])) break;
                r += 1;
            }
            return r;
        }
    }

    public class MultiCalculator(List<string> opoperations, List<double> numbers)
    {
        protected List<string> operations = opoperations;
        protected List<double> numbers = numbers;

        public double Runner(int point = 0)
        {
            while (operations.Count != 1) {

            if ((operations[point] == "*" || operations[point] == "/") || (operations[point] == "+" || operations[point] == "-") &&
                    (operations[point + 1] != "*" && operations[point + 1] != "/"))
                {
                    string oper = operations[point];
                    double fst = numbers[point];
                    double scd = numbers[point + 1];

                    Govno(point, oper, fst, scd);

                    point = 0;
                }

                else
                {
                    point += 1;
                }
            }
            return new Calculator(numbers[0], numbers[1], Calculator.ChooseOperation(operations[0])).Calculate();
        }

        public void Govno(int point, string oper, double fst, double scd)
        {
            operations.RemoveAt(point);
            numbers.RemoveAt(point);
            numbers.RemoveAt(point);
            numbers.Insert(point, new Calculator(fst, scd, Calculator.ChooseOperation(oper)).Calculate());
        }
    }
}
