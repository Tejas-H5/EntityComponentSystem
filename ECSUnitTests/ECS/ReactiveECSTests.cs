using Common;
using ECS;
using ECSUnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSUnitTests.ECS
{
    [TestClass]
    public class ReactiveECSTests : ECSTests
    {
        protected override IECSSystem CreateSystem(ECSWorld world)
        {
            return new MotionIntergratorSystem2DReactive(world);
        }
    }
}
