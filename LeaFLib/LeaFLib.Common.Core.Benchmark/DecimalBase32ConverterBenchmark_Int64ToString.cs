using BenchmarkDotNet.Attributes;

namespace LeaFLib.Common.Core.Benchmark
{
    [MemoryDiagnoser]
    public class DecimalBase32ConverterBenchmark_Int64ToString
    {
        private readonly long _num;
        private readonly DecimalBase32Converter _converter;

        public DecimalBase32ConverterBenchmark_Int64ToString()
        {
            _num = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _converter = DecimalBase32Converter.Default;
        }

        [Benchmark]
        public string Int64ToString_32Base()
        {
            return _converter.ToString(_num);
        }

        [Benchmark(Baseline = true)]
        public string Int64ToString_Hex()
        {
            return _num.ToString("x");
        }
    }
}