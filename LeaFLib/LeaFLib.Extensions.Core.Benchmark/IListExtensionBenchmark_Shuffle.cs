using BenchmarkDotNet.Attributes;

namespace LeaFLib.Extensions.Core.Benchmark
{
    [MemoryDiagnoser]
    public class IListExtensionBenchmark_Shuffle
    {
        [Benchmark]
        [ArgumentsSource(nameof(ShuffleItems))]
        public IList<int> Shuffle(IList<int> items)
        {
            items.Shuffle();
            return items;
        }

        public static IEnumerable<IList<int>> ShuffleItems()
        {
            yield return Enumerable.Range(1, 1000).ToArray();
            yield return Enumerable.Range(1, 10000).ToArray();
            yield return Enumerable.Range(1, 10000).ToList();
        }
    }
}