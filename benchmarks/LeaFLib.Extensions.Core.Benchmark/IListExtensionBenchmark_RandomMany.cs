using BenchmarkDotNet.Attributes;

namespace LeaFLib.Extensions.Core.Benchmark
{
    [MemoryDiagnoser]
    public class IListExtensionBenchmark_RandomMany
    {
        private readonly int[] Source = Enumerable.Range(1, 1024).ToArray();

        [Params(256, 1000)]
        public int Length { get; set; }

        [ParamsAllValues(Priority = 1)]
        public RandomManyUniqueAlgorithm Algorithm { get; set; }

        [Benchmark]
        public int[] RandomManyUnique()
        {
            return Source.RandomManyUnique(Length, Algorithm);
        }
    }
}