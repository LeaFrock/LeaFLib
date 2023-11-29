using LeaFLib.Extensions.Core;
using Xunit;

namespace LeaFLib.Extension.Core.UnitTest
{
    public class ArrayExtensionUnitTest
    {
        public static IEnumerable<object[]> MatchAllTestData { get; } = new List<object[]>()
        {
            new object[] { Array.Empty<int>(), Array.Empty<int>() },
            new object[] { new int[] { 18, 18, 18 }, new int[] { 18, 18, 18 } },
            new object[] { new int[] { 18, 35, 65 }, new int[] { 18 } },
            new object[] { new int[] { 65, 65, 65 }, Array.Empty<int>() },
        };

        [Theory]
        [MemberData(nameof(MatchAllTestData))]
        public void MatchAll(int[] source, int[] expected)
        {
            var a = source.MatchAll(p => p < 35);
            var result = a.SequenceEqual(expected);
            Assert.True(result);
        }
    }
}