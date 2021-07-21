using System;

namespace ECS
{
    public abstract class BaseECSSystem 
    {
        //Simply to enfore constructor arguments
        protected BaseECSSystem(ECSWorld world) { }
    }
}
