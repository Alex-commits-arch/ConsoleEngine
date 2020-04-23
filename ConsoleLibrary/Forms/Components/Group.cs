using ConsoleLibrary.Graphics.Drawing;
using System.Collections.Generic;

namespace ConsoleLibrary.Forms.Components
{
    public class Group : Component
    {
        private List<Component> components;

        public Group() : base()
        {
            components = new List<Component>();
        }

        public override void Draw()
        {
            foreach (var component in components)
            {
                component.Draw();
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
