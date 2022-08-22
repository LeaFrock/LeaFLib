using System.Text;
using Xunit;

namespace LeaFLib.Common.Core.UnitTest
{
    public class Base32ConverterUnitTest
    {
        [Theory]
        [InlineData("Love & Peace", "JRXXMZJAEYQFAZLBMNSQ====")]
        [InlineData("Hello World!", "JBSWY3DPEBLW64TMMQQQ====")]
        public void ConvertToBase32(string source, string expected)
        {
            var bytes = Encoding.UTF8.GetBytes(source);

            var result = Base32Converter.Standard.ToBase32String(bytes);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("JRXXMZJAEYQFAZLBMNSQ====", "Love & Peace")]
        [InlineData("JBSWY3DPEBLW64TMMQQQ====", "Hello World!")]
        public void ConvertFromBase32(string base32Str, string expected)
        {
            var bytes = Base32Converter.Standard.FromBase32String(base32Str);

            var result = Encoding.UTF8.GetString(bytes);

            Assert.Equal(expected, result);
        }
    }
}