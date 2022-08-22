using Xunit;

namespace LeaFLib.Common.Core.UnitTest
{
    public class NumberHelperUnitTest
    {
        [Theory]
        [InlineData(31, "v")]
        [InlineData(32, "10")]
        [InlineData(1641385434, "1gtb3eq")]
        [InlineData(int.MaxValue, "1vvvvvv")]
        public void FormatInt32To32Base(int num, string expected)
        {
            const string customAlphabet = "0123456789abcdefghijklmnopqrstuv";
            string str = NumberHelper.FormatInt32To32Base(num, customAlphabet);
            Assert.Equal(expected, str);
        }

        [Theory]
        [InlineData(31L, "v")]
        [InlineData(32L, "10")]
        [InlineData(1641385434L, "1gtb3eq")]
        [InlineData(long.MaxValue, "7vvvvvvvvvvvv")]
        public void FormatInt64To32Base(long num, string expected)
        {
            const string customAlphabet = "0123456789abcdefghijklmnopqrstuv";
            string str = NumberHelper.FormatInt64To32Base(num, customAlphabet);
            Assert.Equal(expected, str);
        }
    }
}