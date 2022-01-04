using BenchmarkDotNet.Attributes;

namespace LeaFLib.Extensions.Core.Benchmark
{
    public class IListExtensionBenchmark
    {
        private readonly IList<int> _items;

        public IListExtensionBenchmark()
        {
            _items = Enumerable.Range(1, 10000).ToArray();
        }

        [Benchmark]
        public void Shuffle()
        {
            _items.Shuffle();
        }
    }
}