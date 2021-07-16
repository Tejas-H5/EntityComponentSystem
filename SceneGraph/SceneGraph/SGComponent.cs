using System;
using System.Collections.Generic;
using System.Text;

namespace SceneGraph
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

        /*
        //Such functions are not needed in an ECS paradigm but may needed here if required by any of the components
        //This may be balanced by the fact that components have inheritance, which might make them easier to program

        //public virtual void PreRender() { }
        //public virtual void Render() { }
        //public virtual void PostRender() { }

        //public virtual void PreUpdate() { }
        //public virtual void PostUpdate() { }
        */
    }
}
