using System.Runtime.InteropServices;

namespace LeaFLib.Common.Core
{
    public partial class TrieTree<TKey, TValue>
    {
        private TreeNode Root = new TreeNode().AsRoot();

        /// <summary>
        /// [key, oldValue, newValue] => finalValue (updated for <see cref="TreeNode.Value"/>)
        /// </summary>
        public Func<TKey, TValue, TValue, TValue>? ValueUpdateFactory { get; init; }

        public TrieTree<TKey, TValue> Append(ReadOnlySpan<TKey> keys, in TValue value)
        {
            EnsureSpanValid(ref keys, nameof(keys));

            bool appendEndChar = true;
            var children = Root.Children;

            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (children!.Count < 1)
                {
                    var temp = new TreeNode();
                    temp.AsNormal(keys[i], value);
                    children.Add(temp.Key, temp);
                    children = temp.Children;
                    continue;
                }

                ref var node = ref CollectionsMarshal.GetValueRefOrAddDefault(children!, keys[i], out var exists);
                if (exists)
                {
                    var updateFactory = ValueUpdateFactory;
                    node.Value = updateFactory is null
                        ? value
                        : updateFactory.Invoke(node.Key, node.Value, value);
                    if (node.IsTail)
                    {
                        appendEndChar = false;
                        break;
                    }
                }
                else
                {
                    node.AsNormal(keys[i], value);
                }
                children = node.Children;
            }

            if (appendEndChar)
            {
                ref var node = ref CollectionsMarshal.GetValueRefOrAddDefault(children!, keys[^1], out var exists);
                if (exists)
                {
                    var updateFactory = ValueUpdateFactory;
                    node.Value = updateFactory is null
                        ? value
                        : updateFactory.Invoke(node.Key, node.Value, value);
                    node.AsTail();
                }
                else
                {
                    node.AsTail(keys[^1], value);
                }
            }

            return this;
        }

        public bool TryMatchFirst(ReadOnlySpan<TKey> sentence, out Match match)
        {
            EnsureRootValid();
            EnsureSpanValid(ref sentence, nameof(sentence));

            for (int i = 0; i < sentence.Length - 1; i++)
            {
                if (Root.Children!.TryGetValue(sentence[i], out var node))
                {
                    var m = MatchCore(ref sentence, i, node);
                    if (m.HasValue)
                    {
                        match = m.Value;
                        return true;
                    }
                }
            }
            match = default;
            return false;
        }

        public List<Match> MatchAll(ReadOnlySpan<TKey> sentence)
        {
            EnsureRootValid();
            EnsureSpanValid(ref sentence, nameof(sentence));

            var matches = new List<Match>();
            for (int i = 0; i < sentence.Length - 1; i++)
            {
                if (Root.Children!.TryGetValue(sentence[i], out var node))
                {
                    var m = MatchCore(ref sentence, i, node);
                    if (m.HasValue)
                    {
                        matches.Add(m.Value);
                        i += m.Value.Length;
                    }
                }
            }
            return matches;
        }

        public void Clear()
        {
            Root = new TreeNode().AsRoot();
        }

        private void EnsureRootValid()
        {
            if (Root.Children!.Count < 1)
            {
                throw new InvalidOperationException("TrieTree is not intialized.");
            }
        }

        private static void EnsureSpanValid(ref ReadOnlySpan<TKey> keys, string paramName)
            => ArgumentOutOfRangeException.ThrowIfLessThan(keys.Length, 2, paramName);

        private static Match? MatchCore(ref ReadOnlySpan<TKey> keys, int start, TreeNode node)
        {
            for (int i = start + 1; i < keys.Length; i++)
            {
                var children = node.Children;
                if (!children!.TryGetValue(keys[i], out node))
                {
                    break;
                }
                if (node.IsTail)
                {
                    return new(start, i - start + 1) { Value = node.Value };
                }
            }
            return default;
        }
    }
}