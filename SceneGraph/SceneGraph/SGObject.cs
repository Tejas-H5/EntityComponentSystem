using System;
using System.Collections.Generic;
using System.Text;

namespace SceneGraph
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> list, int a, int b)
        {
            T temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
    }

    public class SGObject
    {
        private List<SGObject> children = new List<SGObject>();
        private List<SGComponent> components = new List<SGComponent>();

        public void RemoveComponent<T>()
        {
            int index = findComponent(typeof(T));
            if ((index) == -1)
                return;

            components.Swap(index, components.Count - 1);
            components.RemoveAt(components.Count - 1);
        }

        public void AddComponent<T>(T component) where T : SGComponent
        {
            if (findComponent(typeof(T)) != -1)
                return;

            components.Add(component);
        }

        public T GetComponent<T>() where T : SGComponent
        {
            int i = findComponent(typeof(T));
            if(i==-1)
                return null;

            return (T)components[i];
        }

        int findComponent(Type t)
        {
            for (int i = 0; i < components.Count; i++)
            {
                Type ti = components[i].GetType();
                if (ti.IsSubclassOf(t) || ti == t)
                {
                    return i;
                }
            }
            return -1;
        }

        public int ChildCount {
            get {
                return children.Count;
            }
        }

        public SGObject this[int index] {
            get {
                return children[index];
            }
        }

        public void AddChild(SGObject obj)
        {
            children.Add(obj);
            obj.parent = this;
        }

        public void RemoveChild(SGObject obj)
        {
            int i = children.IndexOf(obj);

            children.Swap(i, children.Count - 1);
            children.RemoveAt(children.Count - 1);

            obj.parent = null;
        }

        private SGObject parent;
        public SGObject Parent {
            get {
                return parent;
            }
            set {
                if (parent != null)
                {
                    parent.RemoveChild(this);
                }
                parent = value;
            }
        }


        public void Update(float deltaTime)
        {
            for(int i = 0; i < components.Count; i++)
            {
                components[i].Update(deltaTime);
            }

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update(deltaTime);
            }
        }


        //public void PreUpdate(){ ... }, and so on as seen in SGComponent also need to be defined in a real SceneGraph
        //This could be a naive implementation tho. 
        //If so, I would like to be informed
    }
}
