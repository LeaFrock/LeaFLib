using BenchmarkDotNet.Running;
using LeaFLib.Common.Core.Benchmark;

var _ = BenchmarkRunner.Run<NumberHelperBenchmark>();

Console.ReadKey();
