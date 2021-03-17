using ConsoleLibrary.Input.Events;
using System;
using System.ComponentModel;
using WindowsWrapper.Enums;
using ConsoleLibrary.Structures;
using System.Runtime.CompilerServices;
using WindowsWrapper.Structs;
using ConsoleLibrary.Drawing;

namespace ConsoleLibrary.Forms.Controls
{
    public abstract class Control// : Component
    {
        protected ControlManager controlManager;

        protected Rectangle rectangle;
        protected string name;
        protected bool visible = true;
        protected bool enabled = true;
        protected bool isInvalid = true;
        protected CharAttribute attributes = CharAttribute.BackgroundBlack;
        protected BufferArea buffer;

        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }

        public int Left
        {
            get => rectangle.Left;
            set
            {
                //if (value != rectangle.Left) Invalidate();
                rectangle.Left = value;
            }
        }
        public int Top
        {
            get => rectangle.Top;
            set
            {
                //if (value != rectangle.Top) Invalidate();
                rectangle.Top = value;
            }
        }
        public int Width
        {
            get => rectangle.Width;
            set
            {
                //if (value != rectangle.Width) Invalidate();
                rectangle.Width = Math.Max(0, value);
            }
        }
        public int Height
        {
            get => rectangle.Height;
            set
            {
                //if (value != rectangle.Height) Invalidate();
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
            add => controlManager.SubscribeMouseEvent(EventType.MousePressed, this, value);
            remove => controlManager.UnsubscribeMouseEvent(EventType.MousePressed, this, value);
        }

        public event MouseEventHandler MouseReleased
        {
            add => controlManager.SubscribeMouseEvent(EventType.MouseReleased, this, value);
            remove => controlManager.UnsubscribeMouseEvent(EventType.MouseReleased, this, value);
        }

        public event MouseEventHandler MouseMoved
        {
            add => controlManager.SubscribeMouseEvent(EventType.MouseMoved, this, value);
            remove => controlManager.UnsubscribeMouseEvent(EventType.MouseMoved, this, value);
        }

        public event MouseEventHandler MouseEnter
        {
            add => controlManager.SubscribeMouseEvent(EventType.MouseEnter, this, value);
            remove => controlManager.UnsubscribeMouseEvent(EventType.MouseEnter, this, value);
        }

        public event MouseEventHandler MouseLeave
        {
            add => controlManager.SubscribeMouseEvent(EventType.MouseLeave, this, value);
            remove => controlManager.UnsubscribeMouseEvent(EventType.MouseLeave, this, value);
        }

        public Control(ControlManager manager)
        {
            controlManager = manager;
            manager.Add(this);
        }

        public void Invalidate()
        {
            isInvalid = true;
            Draw();
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

        /// <summary>
        /// Reinitializes the buffer with the current width and height
        /// </summary>
        protected virtual void RefreshBuffer()
        {
            if (buffer == null)
                buffer = new BufferArea(Width, Height);
            else
                buffer.Resize(Width, Height);
        }

        public virtual void Draw()
        {
            if (isInvalid)
            {
                RefreshBuffer();
                isInvalid = false;
            }
            ConsoleRenderer.ActiveBuffer.Draw(buffer, Left, Top);
        }
    }
}
