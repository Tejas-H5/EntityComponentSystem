using BenchmarkDotNet.Running;
using IterationPatternBenchmarking.Benchmarks;
using System;

namespace IterationPatternBenchmarking
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<IterationPatternBenchmarks>();
        }
    }
}
