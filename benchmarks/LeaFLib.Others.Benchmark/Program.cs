using BenchmarkDotNet.Running;
using LeaFLib.Others.Benchmark;

var _ = BenchmarkRunner.Run<StringBuilderBenchmark_Append>();

// var _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

Console.ReadKey();