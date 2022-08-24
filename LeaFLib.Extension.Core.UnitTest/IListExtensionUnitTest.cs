using LeaFLib.Extensions.Core;
using Xunit;

namespace LeaFLib.Extension.Core.UnitTest
{
    public class IListExtensionUnitTest
    {
        public static IEnumerable<object[]> AllSameTestData { get; } = new List<object[]>()
        {
            new object[] { Array.Empty<int>(), true },
            new object[] { new int[] { 1 }, true },
            new object[] { new int[] { 1, 1, 1 }, true },
            new object[] { new int[] { 1, 1, 2 }, false },
        };

        [Theory]
        [MemberData(nameof(AllSameTestData))]
        public void AllSame(int[] source, bool expected)
        {
            bool result = source.AllSame();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void RandomManyRepeatable()
        {
            int[] array = Enumerable.Range(1, 100).ToArray();

            var result = array.RandomManyRepeatable(10);

            Assert.Equal(10, result.Length);
        }

        [Theory]
        [InlineData(RandomManyUniqueAlgorithm.ReservoirSampling, false)]
        [InlineData(RandomManyUniqueAlgorithm.SelectionSampling, false)]
        [InlineData(RandomManyUniqueAlgorithm.Shuffle, true)]
        [InlineData(RandomManyUniqueAlgorithm.SkipDictionary, false)]
        public void RandomManyUnique(RandomManyUniqueAlgorithm algorithm, bool allowSourceModified)
        {
            var origin = Enumerable.Range(1, 10).ToList();
            var source = new List<int>(origin);
            const int randomCount = 5;

            var result = source.RandomManyUnique(randomCount, algorithm);
            int count = result.Distinct().Count();

            Assert.Equal(randomCount, result.Length);
            Assert.Equal(result.Length, count);
            if (!allowSourceModified)
            {
                Assert.True(origin.SequenceEqual(source));
            }
        }
    }
}