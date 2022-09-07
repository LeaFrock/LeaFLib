namespace LeaFLib.Common.Core
{
    public partial class TrieTree<TKey, TValue>
    {
        public struct Match
        {
            public Match(int index, int length)
            {
                Index = index;
                Length = length;
            }

            public int Index { get; }

            public int Length { get; }

            public TValue Value { get; init; } = default!;
        }
    }
}