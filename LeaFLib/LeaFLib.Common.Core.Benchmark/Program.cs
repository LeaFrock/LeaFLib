using BenchmarkDotNet.Running;
using LeaFLib.Common.Core.Benchmark;

var _ = BenchmarkRunner.Run<DecimalBase32ConverterBenchmark_Int64ToString>();

Console.ReadKey();
