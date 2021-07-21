namespace ECS
{
    public struct CompTypeIDPair
    {
        public int ComponentType;
        public int ComponentID;

        public CompTypeIDPair(int componentType, int componentID)
        {
            ComponentType = componentType;
            ComponentID = componentID;
        }
    }
}
