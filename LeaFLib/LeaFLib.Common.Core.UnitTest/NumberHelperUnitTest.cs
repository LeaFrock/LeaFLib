using Xunit;

namespace LeaFLib.Common.Core.UnitTest
{
    public class NumberHelperUnitTest
    {
        [Theory]
        [InlineData(31)]
        [InlineData(32)]
        [InlineData(1641385434)]
        public void ConvertInt32ToBin32(int num)
        {
            string expected = num switch
            {
                31 => "v",
                32 => "10",
                1641385434 => "1gtb3eq",
                _ => throw new System.NotImplementedException()
            };
            string str = NumberHelper.ConvertToBin32(num, "0123456789abcdefghijklmnopqrstuv");
            Assert.Equal(expected, str);
        }
    }
}