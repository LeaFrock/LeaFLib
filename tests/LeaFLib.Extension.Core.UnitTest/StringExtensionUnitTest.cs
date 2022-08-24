using LeaFLib.Extensions.Core;
using Xunit;

namespace LeaFLib.Extension.Core.UnitTest
{
    public class StringExtensionUnitTest
    {
        [Theory]
        [InlineData("eeeeeeeeeeeeeeeeeeeeeeee")]
        [InlineData("feeeeeeeeeeeeeeeeeeeeeee")]
        [InlineData("eeeeeeeeeeeeeeeeeeeeeeef")]
        [InlineData("fefefefefefefefefefefefe")]
        public void CountOf_Char(string source)
        {
            const char value = 'f';
            int expectedCount = CountOf_Char_For(source, value);

            int count = source.CountOf(value);

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("eeeeee eeeeeeeeeeeeeeeeee", 0)]
        [InlineData("feeeee eeeeeeeeeeeeeeeeee", 1)]
        [InlineData("fefefe fefefefefefefefefe", 3)]
        public void CountOf_Char_Span(string source, int skipCount)
        {
            const char value = 'f';
            int expectedCount = CountOf_Char_For(source, value) - skipCount;

            int count = source.AsSpan(7).CountOf(value);

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("122132122132122132122132", 0)]
        [InlineData("123eeeeeeeeeeeeeeeeeeeee", 1)]
        [InlineData("321eeeeeeeeeeeeeeeeee123", 1)]
        [InlineData("123123123123123123123123", 8)]
        public void CountOf_String(string source, int expectedCount)
        {
            const string value = "123";

            int count = source.CountOf(value, StringComparison.Ordinal);

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("123123 122132122132122132", 0)]
        [InlineData("123eee 123eeeeeeeeeeeeeee", 1)]
        [InlineData("123123 eeeeeeeeeeeeeee123", 1)]
        [InlineData("123123 123123123123123123", 6)]
        public void CountOf_String_Span(string source, int expectedCount)
        {
            const string value = "123";

            int count = source.AsSpan(7).CountOf(value, StringComparison.Ordinal);

            Assert.Equal(expectedCount, count);
        }

        private static int CountOf_Char_For(string str, char value)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == value)
                {
                    count++;
                }
            }
            return count;
        }
    }
}