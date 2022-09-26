using BenchmarkDotNet.Attributes;

namespace LeaFLib.Common.Core.Benchmark
{
    [MemoryDiagnoser]
    public class StringHelperBenchmark_RandomBase64String
    {
        [Params(128, 1024, 1024 * 1024)]
        public int StringLength { get; set; }

        [Benchmark]
        public string UsingArrayPool()
            => StringHelper.RandomBase64String(StringLength);

        [Benchmark]
        public string NotUsingArrayPool()
            => RandomBase64String_Old(StringLength);

        [Benchmark(Baseline = true)]
        public string Basic()
            => RandomBase64String_Base(StringLength);

        private static string RandomBase64String_Old(int length)
        {
            if (length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The length is less than 1");
            }
            if (length % 4 != 0)
            {
                throw new ArgumentException("The length is not a multiple of 4", nameof(length));
            }

            int byteCount = length * 6 / 8;
            Span<byte> bytes = byteCount <= 512
                ? stackalloc byte[byteCount]
                : new byte[byteCount];
            Random.Shared.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string RandomBase64String_Base(int length)
        {
            if (length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The length is less than 1");
            }
            if (length % 4 != 0)
            {
                throw new ArgumentException("The length is not a multiple of 4", nameof(length));
            }

            int byteCount = length * 6 / 8;
            var bytes = new byte[byteCount];
            Random.Shared.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}