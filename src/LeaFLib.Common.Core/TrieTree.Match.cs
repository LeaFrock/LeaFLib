namespace LeaFLib.Common.Core
{
    public partial class TrieTree<TKey, TValue>
    {
        public readonly struct Match(int index, int length)
        {
            public int Index { get; } = index;

            public int Length { get; } = length;

            public TValue Value { get; init; } = default!;
        }
    }
}