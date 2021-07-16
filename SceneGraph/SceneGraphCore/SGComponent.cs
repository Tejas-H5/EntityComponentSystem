using System;
using System.Collections.Generic;
using System.Text;

namespace SceneGraph.SceneGraphCore
{
    //Highly simplified
    public abstract class SGComponent
    {
        protected SGObject attachedObject;

        public void Attach(SGObject obj)
        {
            attachedObject = obj;
            Init();
        }

        public virtual void Init() { }
        public virtual void Update(float deltaTime) { }

        public abstract Type GetActualType();
        /*
        //Such functions are not needed in an ECS paradigm but may needed here if required by any of the components

        //public virtual void PreRender() { }
        //public virtual void Render() { }
        //public virtual void PostRender() { }

        //public virtual void PreUpdate() { }
        //public virtual void PostUpdate() { }
        */
    }
}
