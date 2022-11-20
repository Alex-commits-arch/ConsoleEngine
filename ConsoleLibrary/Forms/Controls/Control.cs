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
        protected bool propagateEvents = true;
        protected CharAttribute attributes = CharAttribute.BackgroundBlack;
        protected BufferArea buffer;
        protected Control parent;
        protected List<Control> controls;

        public Control Parent => parent;

        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }
        public Rectangle ClientRectangle => new Rectangle
        {
            Left = Left + parent.Left,
            Top = Top + parent.Top,
            Width = Width,
            Height = Height
        };
        public virtual BufferArea Buffer => buffer;

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

        public event MouseEventHandler MousePressed;
        public event MouseEventHandler MouseReleased;
        public event MouseEventHandler MouseDoubleClick;
        public event MouseEventHandler MouseMoved;
        public event MouseEventHandler MouseDragged;
        public event MouseEventHandler MouseEnter;
        public event MouseEventHandler MouseLeave;

        //public event ResizedEventHandler Resized;

        private Control()
        {
            //buffer = new BufferArea(0, 0);
            controls = new List<Control>();
        }

        public Control(Control parent) : this()
        {
            this.parent = parent;
            if (parent != null)
            {
                buffer = parent.buffer;
                parent.controls.Add(this);
            }
        }

        public void Invalidate()
        {
            isInvalid = true;
        }

        protected virtual void BeginUpdate(Control control) => parent?.BeginUpdate(control);
        protected virtual void EndUpdate(Control control) => parent?.EndUpdate(control);

        public void BeginUpdate()
        {
            BeginUpdate(this);
        }

        public void EndUpdate()
        {
            EndUpdate(this);
        }

        public bool ContainsPoint(Point p) => ContainsPoint(p.X, p.Y);
        public bool ContainsPoint(int mx, int my) => Rectangle.ContainsPoint(mx, my);
        public bool IntersectsWith(Rectangle rectangle) => Rectangle.IntersectsWith(rectangle);
        //public Point GetRelativeLocation(Point p) => new Point(p.X - Left, p.Y - Top);
        public Point GetRelativeLocation(Point p) => new Point(p.X - Left - parent.Left, p.Y - Top - parent.Top);

        protected internal void HandleMouseEnter(MouseEventArgs args)
        {
            MouseEnter?.Invoke(this, args);
            if (propagateEvents)
                ControlUnderMouse(args.Location)?.HandleMouseEnter(args);
        }

        protected internal void HandleMouseLeave(MouseEventArgs args)
        {
            MouseLeave?.Invoke(this, args);
            if (propagateEvents)
                ControlUnderMouse(args.Location)?.HandleMouseLeave(args);
        }

        protected internal void HandleMouseMoved(MouseEventArgs args)
        {
            MouseMoved?.Invoke(this, args);
            if (propagateEvents)
                ControlUnderMouse(args.Location)?.HandleMouseMoved(args);
        }

        protected internal void HandleMouseDragged(MouseEventArgs args)
        {
            MouseDragged?.Invoke(this, args);
            if (propagateEvents)
                ControlUnderMouse(args.Location)?.HandleMouseDragged(args);
        }

        protected internal void HandleMouseReleased(MouseEventArgs args)
        {
            MouseReleased?.Invoke(this, args);
            if (propagateEvents)
                ControlUnderMouse(args.Location)?.HandleMouseReleased(args);
        }

        protected internal void HandleMouseDoubleClick(MouseEventArgs args)
        {
            MouseDoubleClick?.Invoke(this, args);
            if (propagateEvents)
                ControlUnderMouse(args.Location)?.HandleMouseDoubleClick(args);
        }

        protected internal void HandleMousePressed(MouseEventArgs args)
        {
            MousePressed?.Invoke(this, args);
            if (propagateEvents)
                ControlUnderMouse(args.Location)?.HandleMousePressed(args);
        }

        protected Control ControlUnderMouse(Point point)
        {
            return ControlUnderMouse(point.X, point.Y);
        }

        protected Control ControlUnderMouse(int x, int y)
        {
            for (int i = controls.Count - 1; i >= 0; i--)
            {
                Control control = controls[i];
                if (control.Visible && control.Enabled && control.ContainsPoint(x - Left, y - Top))
                    return control;
            }
            return null;
        }

        protected virtual internal void HandleResized(int width, int height)
        {

            //Resized?.Invoke(args);
        }


        /// <summary>
        /// Reinitializes the buffer with the current width and height
        /// </summary>
        protected virtual void RefreshBuffer()
        {
            if (buffer == null)
                buffer = new BufferArea(Width, Height);
            else if (buffer.Width != Width || buffer.Height != Height)
                buffer.Resize(Width, Height);
            else
                buffer.Clear();
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
