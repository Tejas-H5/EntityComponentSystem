using ECS.CustomDataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public interface IECSListner
    {
        void OnEntityCreated(MutableList<CompTypeIDPair> components, int entityID);
        void OnEntityRemoved(MutableList<CompTypeIDPair> components, int entityID);
        void OnAddComponent(MutableList<CompTypeIDPair> components, int entityID, int indexIntoComponentsList);
        void OnRemoveComponent(MutableList<CompTypeIDPair> components, int entityID, int indexIntoComponentsList);
    }
}
