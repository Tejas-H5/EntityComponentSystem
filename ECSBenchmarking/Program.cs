using BenchmarkDotNet.Running;
using System;

namespace ECSBenchmarking
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ECSvsSceneGraph>();

            while (true)
            {

            }
        }
    }
}
