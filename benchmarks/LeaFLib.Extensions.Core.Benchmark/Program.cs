using BenchmarkDotNet.Running;
using LeaFLib.Extensions.Core.Benchmark;

var _ = BenchmarkRunner.Run<StringExtensionBenchmark_CountOf_Char>();

// var _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

Console.ReadKey();