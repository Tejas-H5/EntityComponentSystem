using System;
using System.Diagnostics.CodeAnalysis;

namespace ECS
{
    public struct CompTypeIDPair : IComparable<CompTypeIDPair>
    {
        public int ComponentType;
        public int ComponentID;

        public CompTypeIDPair(int componentType, int componentID)
        {
            ComponentType = componentType;
            ComponentID = componentID;
        }

        public int CompareTo(CompTypeIDPair other)
        {
            return ComponentType.CompareTo(other.ComponentType);
        }
    }
}
