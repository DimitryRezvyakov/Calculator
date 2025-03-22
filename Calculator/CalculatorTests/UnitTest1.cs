using HomeWork;

namespace CalculatorTests
{
    public class OperationsTest
    {
        [Theory]
        [MemberData(nameof(OperationsTestsData))]
        public void OperationTest(double a, double b, double expected,  ICalculator oper)
        {
            var calc = new Calculator(a, b, oper);

            var result = calc.Calculate();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void QuotienCalculateTest_OnZeroDivision_ReturnExeption()
        {
            var calc = new Calculator(1, 0, new QuotienCalculate());

            Assert.Throws<DivideByZeroException>(() => calc.Calculate());
        }

        public static IEnumerable<object[]> OperationsTestsData()
        {
            yield return new object[] {1, 2, 3, new SumCalculate() };
            yield return new object[] { 4, 2, 2, new DifferCalculate() };
            yield return new object[] { 3, 2, 6, new ProductCalculate() };
            yield return new object[] { 6, 2, 3, new QuotienCalculate() };
        }
    }



    public class ParserTest
    {

        [Theory]
        [MemberData(nameof(ParserData))]
        public void ParseTests(string inp, string[] expected)
        {
            var result = Parser.Parse(inp);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ParserData()
        {
            yield return new object[] { "1+ 2/ 3 -3*4-999", new string[] {"1", "+", "2", "/", "3", "-", "3", "*", "4", "-", "999" } };
            yield return new object[] { "-1 -4", new string[] { "-1", "-", "4"} };
            yield return new object[] { "1 +9*-5/-5-5-5", new string[] {"1", "+", "9", "*", "-5", "/", "-5", "-", "5", "-", "5" }  };
        }
    }

    public class RunnerTest
    {

        [Theory]
        [MemberData(nameof(RunnerData))]
        public void RunnerTests(string input, double expected)
        {
            var arr = Parser.Parse(input);
            var nums = new List<double>();
            var opers = new List<string>();
            Programm.DivideArr(nums, opers, arr);
            var runner = new MultiCalculator(opers, nums);

            var result = runner.Runner(0);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> RunnerData()
        {
            yield return new object[] { "400 / 20 + 50 * 20 / 5 + 100 - 500 * 100", -49680};
            yield return new object[] { "55+20 / 2 +50 * 4-50 *2/2", 215 };
            yield return new object[] { "5 - 3 + -10 * 2 / 5", -2 };
        }
    }
}
