using BenchmarkDotNet.Running;
using LeaFLib.Extensions.Core.Benchmark;

var _ = BenchmarkRunner.Run<IListExtensionBenchmark_RandomMany>();

// var _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

Console.ReadKey();