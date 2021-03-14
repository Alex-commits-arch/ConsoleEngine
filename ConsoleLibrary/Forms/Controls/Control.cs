using ConsoleLibrary.Input.Events;
using System;
using System.ComponentModel;
using WindowsWrapper.Enums;
using ConsoleLibrary.Structures;
using System.Runtime.CompilerServices;

namespace ConsoleLibrary.Forms.Controls
{
    public abstract class Control// : Component
    {
        protected ControlManager container;

        protected Rectangle rectangle;
        protected string name;
        protected bool visible = true;
        protected bool enabled = true;
        protected bool isInvalid = true;
        protected CharAttribute attributes = CharAttribute.BackgroundBlack;

        protected Rectangle Rectangle { get => rectangle; set => rectangle = value; }

        public int Left
        {
            get => rectangle.Left;
            set
            {
                if (value != rectangle.Left) Invalidate();
                rectangle.Left = value;
            }
        }
        public int Top
        {
            get => rectangle.Top;
            set
            {
                if (value != rectangle.Top) Invalidate();
                rectangle.Top = value;
            }
        }
        public int Width
        {
            get => rectangle.Width;
            set
            {
                if (value != rectangle.Width) Invalidate();
                rectangle.Width = Math.Max(0, value);
            }
        }
        public int Height
        {
            get => rectangle.Height;
            set
            {
                if (value != rectangle.Height) Invalidate();
                rectangle.Height = Math.Max(0, value);
            }
        }

        public int Right => rectangle.Right;
        public int Bottom => rectangle.Bottom;

        public string Name { get => name; set => name = value; }
        public bool Visible { get => visible; set => visible = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
        public CharAttribute Attributes { get => attributes; set => attributes = value; }

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

        public void Invalidate()
        {
            isInvalid = true;
        }

        public bool ContainsPoint(Point p)
        {
            return ContainsPoint(p.X, p.Y);
        }

        public bool ContainsPoint(int mx, int my)
        {
            return mx >= rectangle.Left && mx < rectangle.Right &&
                   my >= rectangle.Top && my < rectangle.Bottom;
        }

        public virtual void Draw() { }
    }
}
