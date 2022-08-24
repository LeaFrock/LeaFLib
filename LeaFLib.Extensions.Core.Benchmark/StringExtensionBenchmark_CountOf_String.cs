using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace LeaFLib.Extensions.Core.Benchmark
{
    [MemoryDiagnoser]
    public class StringExtensionBenchmark_CountOf_String
    {
        private const string Value = "fefe";

        private const string Text1 = "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeefefe";
        private const string Text2 = "fefeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
        private const string Text3 = "fefefefefefefefefefefefefefefefefefefefefefefefefefefefefefefefe";
        private const string Text4 = "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
        private const string Text5 = "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff";

        [Params(Text1, Text2, Text3, Text4, Text5)]
        public string Source { get; set; } = string.Empty;

        [Benchmark]
        public int CountOf()
        {
            return Source.CountOf(Value, StringComparison.Ordinal);
        }

        [Benchmark(Baseline = true)]
        public int CountOf_Replace()
        {
            string strReplaced = Source.Replace(Value, string.Empty, StringComparison.Ordinal);
            return (Source.Length - strReplaced.Length) / Value.Length;
        }

        [Benchmark]
        public int CountOf_RegexMatch()
        {
            return Regex.Matches(Source, Value).Count;
        }
    }
}