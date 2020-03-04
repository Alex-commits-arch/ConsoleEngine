using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Structures;
using System.Collections.Generic;

namespace ConsoleLibrary.Forms.Components
{
    public abstract class Component
    {
        //public Location pos;
        protected DrawingContext context;
        protected Component parent;

        private int top;
        private int left;
        private int width;
        private int height;

        public virtual int Top { get => top; set => top = value; }
        public virtual int Left { get => left; set => left = value; }
        public virtual int Width { get => width; set => width = value; }
        public virtual int Height { get => height; set => height = value; }

        public Component() { }

        public Component(DrawingContext context) : this()
        {
            this.context = context;
        }

        public virtual void Render() { }
    }

    public class ComponentCollection
    {
        private List<Component> components;

        public ComponentCollection()
        {
            components = new List<Component>();
        }
    }
}
