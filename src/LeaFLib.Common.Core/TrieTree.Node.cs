namespace LeaFLib.Common.Core
{
    public partial class TrieTree<TKey, TValue> where TKey : notnull
    {
        private struct TreeNode
        {
            public TKey Key { get; private set; }

            public TValue Value { get; set; }

            public Dictionary<TKey, TreeNode>? Children { get; private set; }

            public bool IsTail { get; private set; }

            public void AsNormal(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                Children = new();
            }

            public TreeNode AsRoot()
            {
                Children = new();
                return this;
            }

            public void AsTail(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                IsTail = true;
            }

            public void AsTail()
            {
                IsTail = true;
                Children = default;
            }
        }
    }
}