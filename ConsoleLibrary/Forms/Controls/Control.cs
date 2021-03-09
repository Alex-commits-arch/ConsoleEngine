using ConsoleLibrary.Forms;
using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WindowsWrapper.Enums;
namespace ConsoleLibrary.Forms.Controls
{
    [DesignerCategory("")]
    public abstract class Control : Component
    {
        //private static readonly Dictionary<BackgroundShade, char> shades = new Dictionary<BackgroundShade, char>
        //{
        //    { BackgroundShade.None, '\u0020' },
        //    { BackgroundShade.Light, '\u2591' },
        //    { BackgroundShade.Medium, '\u2592' },
        //    { BackgroundShade.Dark, '\u2593' },
        //};

        protected int left;
        protected int top;
        protected int width;
        protected int height;
        protected CharAttribute attributes = CharAttribute.BackgroundBlack;
        private bool focused;
        private bool visible = true;
        private bool enabled = true;
        private string name;
        protected ControlManager container;

        public int Left { get => left; set => left = value; }
        public int Top { get => top; set => top = value; }
        public int Width { get => width; set => width = Math.Max(0, value); }
        public int Height { get => height; set => height = Math.Max(0, value); }
        public int Right => left + width;
        public int Bottom => top + height;
        public CharAttribute Attributes { get => attributes; set => attributes = value; }
        public string Name { get => name; set => name = value; }
        public bool Visible { get => visible; set => visible = value; }
        public bool Enabled { get => enabled; set => enabled = value; }

        public event MouseEventHandler MousePressed
        {
            add => container.SubscribeMouseEvent(EventType.MousePressed, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MousePressed, this, value);
        }

        public event MouseEventHandler MouseReleased
        {
            add => container.SubscribeMouseEvent(EventType.MouseReleased, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MouseReleased, this, value);
        }

        public event MouseEventHandler MouseMoved
        {
            add => container.SubscribeMouseEvent(EventType.MouseMoved, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MouseMoved, this, value);
        }

        public event MouseEventHandler MouseEnter
        {
            add => container.SubscribeMouseEvent(EventType.MouseEnter, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MouseEnter, this, value);
        }

        public event MouseEventHandler MouseLeave
        {
            add => container.SubscribeMouseEvent(EventType.MouseLeave, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MouseLeave, this, value);
        }

        public Control(ControlManager container)
        {
            this.container = container;
            container.Add(this);
        }

        public bool ContainsPoint(int mx, int my)
        {
            return width > 0 && height > 0 &&
                   mx >= left && mx < left + width &&
                   my >= top && my < top + height;
        }

        public virtual void Draw()
        {
            ConsoleRenderer.FillRect(' ', left, top, width, height, attributes);
        }
    }
}
