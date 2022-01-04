using BenchmarkDotNet.Attributes;

namespace LeaFLib.Extensions.Core.Benchmark
{
    [MemoryDiagnoser]
    public class IListExtensionBenchmark
    {
        [Benchmark]
        [ArgumentsSource(nameof(ShuffleItems))]
        public void Shuffle(IList<int> items)
        {
            items.Shuffle();
        }

        public IEnumerable<IList<int>> ShuffleItems()
        {
            yield return Enumerable.Range(1, 1000).ToArray();
            yield return Enumerable.Range(1, 10000).ToArray();
            yield return Enumerable.Range(1, 10000).ToList();
        }
    }
}