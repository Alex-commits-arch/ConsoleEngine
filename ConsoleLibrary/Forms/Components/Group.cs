using ConsoleLibrary.Graphics.Drawing;
using System.Collections.Generic;

namespace ConsoleLibrary.Forms.Components
{
    public class Group : Component
    {
        private List<Component> components;

        public Group(DrawingContext context) : base(context)
        {
            components = new List<Component>();
        }

        public override void Render()
        {
            foreach (var component in components)
            {
                component.Render();
            }
        }

        public void Add(Component component)
        {
            component.Left += Left;
            component.Top += Top;
            components.Add(component);
        }
    }
}
