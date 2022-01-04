using BenchmarkDotNet.Running;
using LeaFLib.Extensions.Core.Benchmark;

var _ = BenchmarkRunner.Run<IListExtensionBenchmark>();

Console.ReadKey();