using ConsoleLibrary.Input.Events;
using System;
using System.ComponentModel;
using WindowsWrapper.Enums;
using ConsoleLibrary.Structures;
using System.Runtime.CompilerServices;
using WindowsWrapper.Structs;
using ConsoleLibrary.Drawing;
using System.Collections.Generic;

namespace ConsoleLibrary.Forms.Controls
{
    public abstract class Control
    {
        //protected ControlManager controlManager;

        protected Rectangle rectangle;
        protected string name;
        protected bool visible = true;
        protected bool enabled = true;
        protected bool isInvalid = true;
        protected CharAttribute attributes = CharAttribute.BackgroundBlack;
        protected BufferArea buffer;
        protected Control parent;
        protected List<Control> controls;

        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }
        public BufferArea Buffer => buffer;

        public int Left
        {
            get => rectangle.Left;
            set => rectangle.Left = value;
        }
        public int Top
        {
            get => rectangle.Top;
            set => rectangle.Top = value;
        }
        public virtual int Width
        {
            get => rectangle.Width;
            set => rectangle.Width = Math.Max(0, value);
        }
        public virtual int Height
        {
            get => rectangle.Height;
            set => rectangle.Height = Math.Max(0, value);
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

        public event MouseEventHandler MouseDoubleClick
        {
            add => controlManager.SubscribeMouseEvent(EventType.MouseDoubleClick, this, value);
            remove => controlManager.UnsubscribeMouseEvent(EventType.MouseDoubleClick, this, value);
        }

        public event MouseEventHandler MouseMoved
        {
            add => controlManager.SubscribeMouseEvent(EventType.MouseMoved, this, value);
            remove => controlManager.UnsubscribeMouseEvent(EventType.MouseMoved, this, value);
        }

        public event MouseEventHandler MouseDragged
        {
            add => controlManager.SubscribeMouseEvent(EventType.MouseDragged, this, value);
            remove => controlManager.UnsubscribeMouseEvent(EventType.MouseDragged, this, value);
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

        private Control()
        {
            buffer = new BufferArea(0, 0);
            controls = new List<Control>();
            MousePressed += OnMousePressed;
        }

        //public Control(ControlManager manager) : this()
        //{
        //    controlManager = manager;
        //    manager.Add(this);
        //}

        public Control(Control parent) : this()
        {
            this.parent = parent;
            //parent.controls
        }

        public void Invalidate()
        {
            isInvalid = true;
        }

        public void BeginUpdate() => parent.BeginUpdate();
        public void EndUpdate() => parent.EndUpdate();

        public bool ContainsPoint(Point p) => ContainsPoint(p.X, p.Y);
        public bool ContainsPoint(int mx, int my) => Rectangle.ContainsPoint(mx, my);
        public bool IntersectsWith(Rectangle rectangle) => Rectangle.IntersectsWith(rectangle);
        public Point GetRelativeLocation(Point p) => new Point(p.X - Left, p.Y - Top);

        private void OnMousePressed(object obj, MouseEventArgs args)
        {
            foreach (var control in controls)
            {
                if (control.ContainsPoint(args.Location))
                    control.OnMousePressed(obj, args);
            }
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

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Requests drawing to main buffer
        /// </summary>
        public void Draw(Rectangle rect)
        {
            if (Visible)
            {
                rect.Left -= Left;
                rect.Top -= Top;
                var area = buffer.GetArea(rect);
                ConsoleRenderer.ActiveBuffer.Draw(area, rect.Left + Left, rect.Top + Top);
            }
        }

        /// <summary>
        /// Requests drawing to given buffer
        /// </summary>
        public virtual void Draw(BufferArea drawingBuffer)
        {
            if (Visible)
            {
                if (isInvalid)
                {
                    RefreshBuffer();
                    isInvalid = false;
                }

                foreach (var control in controls)
                    control.Draw(buffer);

                drawingBuffer.Draw(buffer, Left, Top);
            }
        }
    }
}
