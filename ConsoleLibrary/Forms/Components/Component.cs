using System.Collections;
using System.Collections.Generic;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Forms.Components
{
    public abstract class Component
    {
        protected Component parent;

        protected int top;
        protected int left;
        protected int width;
        protected int height;
        protected bool visible = true;
        private CharAttribute attributes = CharAttribute.BackgroundBlack | CharAttribute.ForegroundWhite;

        public virtual int Top { get => top; set => top = value; }
        public virtual int Left { get => left; set => left = value; }
        public virtual int Width { get => width; set => width = value; }
        public virtual int Height { get => height; set => height = value; }
        public virtual bool Visible { get => visible; protected set => visible = value; }
        public CharAttribute Attributes { get => attributes; set => attributes = value; }



        public virtual void Draw() { }
        protected virtual void OnHide() { visible = false; }
        protected virtual void OnShow() { visible = true; }
        public void Hide() { OnHide(); }
        public void Show() { OnShow(); }

        public bool ContainsMouse(COORD location)
        {
            int x = location.X;
            int y = location.Y;
            return x >= Left && x < Left + Width
                && y >= Top && y < Top + Height;
        }
    }

    public class InputComponent : Component
    {

    }

    public class ComponentCollection : IEnumerable<Component>
    {
        private List<Component> components;
        public ComponentCollection() { components = new List<Component>(); }
        public void Add(Component component) { components.Add(component); }
        public void Remove(Component component) { components.Remove(component); }
        public void Clear() { components.Clear(); }
        public IEnumerator<Component> GetEnumerator() { return components.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return components.GetEnumerator(); }
    }
}
