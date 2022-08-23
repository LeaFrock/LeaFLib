using Xunit;

namespace LeaFLib.Common.Core.UnitTest
{
    public class DemicalBase32ConverterUnitTest
    {
        [Theory]
        [InlineData(31, "v")]
        [InlineData(32, "10")]
        [InlineData(1641385434, "1gtb3eq")]
        [InlineData(int.MaxValue, "1vvvvvv")]
        public void Int32ToString(int num, string expected)
        {
            string str = DemicalBase32Converter.Default.ToString(num);
            Assert.Equal(expected, str);
        }

        [Theory]
        [InlineData(31L, "v")]
        [InlineData(32L, "10")]
        [InlineData(1641385434L, "1gtb3eq")]
        [InlineData(long.MaxValue, "7vvvvvvvvvvvv")]
        public void Int64ToString(long num, string expected)
        {
            string str = DemicalBase32Converter.Default.ToString(num);
            Assert.Equal(expected, str);
        }

        [Theory]
        [InlineData(31, "v")]
        [InlineData(32, "10")]
        [InlineData(1641385434, "1gtb3eq")]
        [InlineData(int.MaxValue, "1vvvvvv")]
        [InlineData(int.MaxValue, "0000001vvvvvv")]
        public void StringToInt32(int expected, string str)
        {
            int num = DemicalBase32Converter.Default.ToInt32(str);
            Assert.Equal(expected, num);
        }

        [Theory]
        [InlineData(31L, "v")]
        [InlineData(32L, "10")]
        [InlineData(1641385434L, "1gtb3eq")]
        [InlineData(long.MaxValue, "7vvvvvvvvvvvv")]
        [InlineData(long.MaxValue, "0000007vvvvvvvvvvvv")]
        public void StringToInt64(long expected, string str)
        {
            long num = DemicalBase32Converter.Default.ToInt64(str);
            Assert.Equal(expected, num);
        }
    }
}