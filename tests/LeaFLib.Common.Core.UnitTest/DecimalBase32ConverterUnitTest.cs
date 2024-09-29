using Xunit;

namespace LeaFLib.Common.Core.UnitTest;

public class DecimalBase32ConverterUnitTest
{
    [Theory]
    [InlineData(31, "v")]
    [InlineData(32, "10")]
    [InlineData(1641385434, "1gtb3eq")]
    [InlineData(uint.MaxValue, "3vvvvvv")]
    public void Int32ToString(uint num, string expected)
    {
        string str = DecimalBase32Converter.Default.ToString(num);
        Assert.Equal(expected, str);
    }

    [Theory]
    [InlineData(31L, "v")]
    [InlineData(32L, "10")]
    [InlineData(1641385434L, "1gtb3eq")]
    [InlineData(ulong.MaxValue, "fvvvvvvvvvvvv")]
    public void Int64ToString(ulong num, string expected)
    {
        string str = DecimalBase32Converter.Default.ToString(num);
        Assert.Equal(expected, str);
    }

    [Fact]
    public void DateTimeOffsetToString()
    {
        var dto = new DateTimeOffset(2022, 10, 1, 0, 0, 0, TimeSpan.FromHours(8));

        string str1 = DecimalBase32Converter.Default.ToString(dto, false);
        string str2 = DecimalBase32Converter.Default.ToString(dto, true);

        Assert.Equal("1hje4k0", str1);
        Assert.Equal("1ge7i0h00", str2);
    }

    [Theory]
    [InlineData(31, "v")]
    [InlineData(32, "10")]
    [InlineData(1641385434, "1gtb3eq")]
    [InlineData(int.MaxValue, "1vvvvvv")]
    [InlineData(int.MaxValue, "0000001vvvvvv")]
    public void StringToInt32(uint expected, string str)
    {
        var num = DecimalBase32Converter.Default.ToUInt32(str);
        Assert.Equal(expected, num);
    }

    [Theory]
    [InlineData(31L, "v")]
    [InlineData(32L, "10")]
    [InlineData(1641385434L, "1gtb3eq")]
    [InlineData(long.MaxValue, "7vvvvvvvvvvvv")]
    [InlineData(long.MaxValue, "0000007vvvvvvvvvvvv")]
    public void StringToInt64(ulong expected, string str)
    {
        var num = DecimalBase32Converter.Default.ToUInt64(str);
        Assert.Equal(expected, num);
    }

    [Theory]
    [InlineData(31, true, "v")]
    [InlineData(32, true, "10")]
    [InlineData(1641385434, true, "1gtb3eq")]
    [InlineData(int.MaxValue, true, "0000001vvvvvv")]
    [InlineData(0, false, "0011234567")]
    [InlineData(0, false, "1-vvvvv")]
    [InlineData(0, false, "2vvvvvv")]
    public void StringTryToInt32(uint expectedNum, bool canParse, string str)
    {
        bool parseResult = DecimalBase32Converter.Default.TryToUInt32(str, out var num);
        Assert.Equal(canParse, parseResult);
        if (canParse)
        {
            Assert.Equal(expectedNum, num);
        }
    }

    [Theory]
    [InlineData(31L, true, "v")]
    [InlineData(32L, true, "10")]
    [InlineData(1641385434L, true, "1gtb3eq")]
    [InlineData(long.MaxValue, true, "0000007vvvvvvvvvvvv")]
    [InlineData(0, false, "0011234567890abc")]
    [InlineData(0, false, "8vvvvvvvvvvvv")]
    [InlineData(0, false, "7-vvvvvvvvvvv")]
    public void StringTryToInt64(ulong expectedNum, bool canParse, string str)
    {
        bool parseResult = DecimalBase32Converter.Default.TryToUInt64(str, out var num);
        Assert.Equal(canParse, parseResult);
        if (canParse)
        {
            Assert.Equal(expectedNum, num);
        }
    }
}