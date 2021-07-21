using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public interface IECSSystem
    {
        void Update(float deltaTime);
        protected void Iterate(int[] componentIDs, float deltaTime);
        protected ref T GetComponent<T>(int componentID) where T : struct;
    }
}
