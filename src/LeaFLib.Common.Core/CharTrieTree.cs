namespace LeaFLib.Common.Core
{
    public partial class CharTrieTree<TValue> : TrieTree<char, TValue>
    {
        public CharTrieTree<TValue> Append(string keyword, in TValue value)
        {
            ArgumentNullException.ThrowIfNull(keyword);

            _ = Append(keyword.AsSpan(), value);

            return this;
        }

        public bool TryMatchFirst(string sentence, out Match match)
        {
            ArgumentNullException.ThrowIfNull(sentence);

            return TryMatchFirst(sentence.AsSpan(), out match);
        }

        public List<Match> MatchAll(string sentence)
        {
            ArgumentNullException.ThrowIfNull(sentence);

            return MatchAll(sentence.AsSpan());
        }
    }
}