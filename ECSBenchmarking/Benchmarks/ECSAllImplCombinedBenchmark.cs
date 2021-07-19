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

        ECSEventBasedBenchmark eventBased = new ECSEventBasedBenchmark();
        ECSNormalBenchmark normal = new ECSNormalBenchmark();

        [GlobalSetup]
        public void GlobalSetup()
        {
            //Order is important
            eventBased.NumberOfElements = NumElements;
            eventBased.GlobalSetup();

            normal.NumberOfElements = NumElements;
            normal.GlobalSetup();
        }


        [Benchmark]
        public void NormalECSUpdate()
        {
            normal.ECSUpdate();
        }


        [Benchmark]
        public void EventBasedECSUpdate()
        {
            eventBased.EventBasedECSUpdate();
        }
    }
}
