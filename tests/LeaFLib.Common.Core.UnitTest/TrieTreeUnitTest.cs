using Xunit;

namespace LeaFLib.Common.Core.UnitTest
{
    public class TrieTreeUnitTest
    {
        private readonly CharTrieTree<int> _tree;

        public TrieTreeUnitTest()
        {
            _tree = new CharTrieTree<int>() { ValueUpdateFactory = UpdateValue }
                .Append("Fuck", 1)
                .Append("FuckU", 2)
                .Append("shit", 4)
                .Append("Shoot", 8);
        }

        [Fact]
        public void MatchFirst()
        {
            var result = _tree.TryMatchFirst("You're a Mother Fucker!", out var m);

            Assert.True(result);
            Assert.Equal(16, m.Index);
            Assert.Equal(4, m.Length);
            Assert.Equal(3, m.Value);
        }

        [Fact]
        public void MatchAll()
        {
            var result = _tree.MatchAll("Fuck you, bull shit!");

            Assert.Equal(2, result.Count);
            Assert.Equal(0, result[0].Index);
            Assert.Equal(4, result[0].Length);
            Assert.Equal(3, result[0].Value);
            Assert.Equal(15, result[1].Index);
            Assert.Equal(4, result[1].Length);
            Assert.Equal(4, result[1].Value);
        }

        private static int UpdateValue(char _, int flag1, int flag2)
            => flag1 | flag2;
    }
}