using LeaFLib.Extensions.Core;
using Xunit;

namespace LeaFLib.Extension.Core.UnitTest
{
    public class IListExtensionUnitTest
    {
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
        [InlineData(RandomManyUniqueAlgorithm.HashSet, false)]
        public void RandomManyUnique(RandomManyUniqueAlgorithm algorithm, bool allowSourceModified)
        {
            var origin = Enumerable.Range(1, 10).ToList();
            var source = new List<int>(origin);
            const int targetLength = 5;

            var result = source.RandomManyUnique(targetLength, algorithm);
            int count = result.Distinct().Count();

            Assert.Equal(targetLength, result.Length);
            Assert.Equal(result.Length, count);
            if (!allowSourceModified)
            {
                Assert.True(origin.SequenceEqual(source));
            }
        }
    }
}