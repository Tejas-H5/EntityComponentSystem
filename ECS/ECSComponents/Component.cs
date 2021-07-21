using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace ECS.Components
{
    /// <summary>
    /// This struct is an ECS internal data structure that you probably don't need to use.
    /// Ever. If you want to declare a struct as an ECS component, give it an [ECSComponent] Attribute
    /// by putting [ECSComponent(sequentialComponentID)] above the decleration.
    /// More info in the ECSComponentAttribute class documentation
    /// </summary>

    public struct Component<T> where T : struct
    {
        //This ID is an index into an array, and c# uses ints and not ints to index into arrays
        public readonly int ID;

        //The entity that this component is attached to
        public int EntityID;

        public bool IsDestroyed;

        //The actual data. Since structs can't use inheritance, I have used composition like this
        public T Data;

        public Component(int iD, int entityID, T data)
        {
            ID = iD;
            EntityID = entityID;
            Data = data;
            IsDestroyed = false;
        }
    }
}
