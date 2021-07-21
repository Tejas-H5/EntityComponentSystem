using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSBenchmarking
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ECSAllImplCombinedBenchmark
    {
        [Params(10000)]
        public int NumElements { get; set; }

        ECSReactiveBenchmark reactive = new ECSReactiveBenchmark();
        ECSNormalBenchmark normal = new ECSNormalBenchmark();

        [GlobalSetup]
        public void GlobalSetup()
        {
            //Order is important
            normal.NumberOfElements = NumElements;
            normal.GlobalSetup();

            reactive.NumberOfElements = NumElements;
            reactive.GlobalSetup();
        }


        [Benchmark]
        public void NormalECSUpdate()
        {
            normal.ECSUpdate();
        }

        [Benchmark]
        public void ReactiveECSUpdate()
        {
            reactive.ECSUpdate();
        }
    }
}
