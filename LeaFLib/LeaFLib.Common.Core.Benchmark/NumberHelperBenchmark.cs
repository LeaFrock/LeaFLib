using BenchmarkDotNet.Attributes;

namespace LeaFLib.Common.Core.Benchmark
{
    [MemoryDiagnoser]
    public class NumberHelperBenchmark
    {
        private readonly int _num;

        public NumberHelperBenchmark()
        {
            _num = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        [Benchmark]
        public string ConvertInt32ToBin32()
        {
            return NumberHelper.ConvertToBin32(_num);
        }
    }
}