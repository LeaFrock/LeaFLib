using BenchmarkDotNet.Attributes;

namespace LeaFLib.Extensions.Core.Benchmark
{
    public class StringExtensionBenchmark_CountOf_Char
    {
        private const char Value = 'f';

        private const string Text1 = "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
        private const string Text2 = "feeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
        private const string Text3 = "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeef";
        private const string Text4 = "fefefefefefefefefefefefefefefefefefefefefefefefefefefefefefefefe";
        private const string Text5 = "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff";

        [Params(Text1, Text2, Text3, Text4, Text5)]
        public string Source { get; set; } = string.Empty;

        [Benchmark]
        public int CountOf()
        {
            return Source.CountOf(Value);
        }

        [Benchmark(Baseline = true)]
        public int CountOf_For()
        {
            return CountOf_For(Source, Value);
        }

        private static int CountOf_For(string str, char value)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == value)
                {
                    count++;
                }
            }
            return count;
        }
    }
}