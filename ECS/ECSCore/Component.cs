using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    /// <summary>
    /// This struct is an ECS internal data structure that you probably don't need to use.
    /// Ever. If you want to declare a struct as an ECS component, give it an [ECSComponent] Attribute
    /// by putting [ECSComponent(sequentialComponentID)] above the decleration.
    /// More info in the ECSComponentAttribute class documentation
    /// </summary>
    public struct Component<T>
    {
        //This ID is an index into an array, and c# uses ints and not uints to index into arrays
        public readonly int ID;

        //The entity that this component is attached to
        public uint EntityID;

        public bool IsDestroyed;

        //The actual data. Since structs can't use inheritance, I have used composition like this
        public T Data;

        public Component(int iD, uint entityID, T data)
        {
            ID = iD;
            EntityID = entityID;
            Data = data;
            IsDestroyed = false;
        }
    }
}
