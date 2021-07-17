using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

namespace ECSBenchmarking
{
    class Program
    {
        static void Main(string[] args)
        {
            //Use this line to quickly turn on Debug mode for BenchmarkDotNet
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());

            BenchmarkRunner.Run<ECSvsOthersBenchmarks>();
        }
    }
}
