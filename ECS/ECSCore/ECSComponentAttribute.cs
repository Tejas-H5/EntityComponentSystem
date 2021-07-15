using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    /// <summary>
    /// This attribute tells the ECS that the following struct is a valid ECS component 
    /// for entities in our World(s) to have.
    /// 
    /// The order in which the attributes will be registered is actually very important
    /// for backwards compatibility of loading and saving world states.
    /// 
    /// The easiest way to give a new component type a number and maintain 
    /// the IDs of the other components is by using an enum to keep track of existing components.
    /// As long as the enum is never rearranged and new components are only added to the bottom 
    /// of the enum, their orders and therefore their IDs will remain intact and preserve 
    /// backwards compatibility.
    /// 
    /// speaking of which, 
    /// TODO: implement saving and loading world state
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class ECSComponentAttribute : Attribute
    {
        public readonly int RegisterOrder;

        public ECSComponentAttribute(int registerOrder)
        {
            RegisterOrder = registerOrder;
        }
    }
}
