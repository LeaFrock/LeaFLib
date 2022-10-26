using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace LeaFLib.Others.Benchmark
{
    [MemoryDiagnoser]
    public class StringBuilderBenchmark_Append
    {
        // It's from https://github.com/dotnet/aspnetcore/pull/44691/files#diff-85394d9486a1b4a71316a5f8c0859a1024ea7f56c84dd6eae83f3549526034a1

        private readonly byte[] _b = new byte[30];

        [GlobalSetup]
        public void Setup()
        {
            RandomNumberGenerator.Fill(_b);
        }

        [Benchmark]
        public string AppendToString()
        {
            var sb = new StringBuilder();
            foreach (var b in _b)
            {
                sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        [Benchmark]
        public string AppendInterpolated()
        {
            var sb = new StringBuilder();
            foreach (var b in _b)
            {
                sb.Append(CultureInfo.InvariantCulture, $"{b:x2}");
            }
            return sb.ToString();
        }
    }
}