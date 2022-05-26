using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System;
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
        protected bool active;
        protected bool pressed;
        protected bool enabled = true;

        public bool Active
        {
            get => active; private set
            {
                if (active != value)
                {
                    active = value;
                    Draw();
                }
            }
        }

        public bool Pressed
        {
            get => pressed; private set
            {
                if (pressed != value)
                {
                    pressed = value;
                    Draw();
                }
            }
        }

        public bool Enabled
        {
            get => enabled; private set
            {
                if (enabled != value)
                {
                    enabled = value;
                    Draw();
                }
            }
        }

        public event MouseEventHandler Click;
        public event MouseEventHandler DoubleClick;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;

        public InputComponent()
        {
            Show();
            Enable();
        }

        public void HideAndDisable()
        {
            Hide();
            Disable();
        }

        public void ShowAndEnable()
        {
            Show();
            Enable();
        }

        public void Enable()
        {
            Enabled = true;
            ConsoleInput.MousePressed += OnMousePressed;
            ConsoleInput.MouseDoubleClick += OnMousePressed;
            ConsoleInput.MouseMoved += OnMouseMoved;
            ConsoleInput.MouseDragged += OnMouseMoved;
            ConsoleInput.MouseReleased += OnMouseReleased;
        }

        public void Disable()
        {
            Enabled = false;
            ConsoleInput.MousePressed -= OnMousePressed;
            ConsoleInput.MouseDoubleClick -= OnMousePressed;
            ConsoleInput.MouseMoved -= OnMouseMoved;
            ConsoleInput.MouseDragged -= OnMouseMoved;
            ConsoleInput.MouseReleased -= OnMouseReleased;
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        protected override void OnShow()
        {
            base.OnShow();
        }

        public void PerformClick(MouseEventArgs args = null)
        {
            OnMousePressed(this, args);
        }

        private void OnMousePressed(object sender, MouseEventArgs args)
        {
            if (visible && ContainsMouse(args.Location))
                Pressed = true;
        }

        private void OnMouseReleased(object sender, MouseEventArgs args)
        {
            if (pressed)
                Click?.Invoke(this, args);
            Pressed = false;
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Active = true;
            MouseEnter?.Invoke(sender, e);
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Active = false;
            Pressed = false;
            MouseLeave?.Invoke(sender, e);
        }

        private void OnMouseMoved(object sender, MouseEventArgs args)
        {
            bool containsMouse = ContainsMouse(args.Location);
            if (!active && containsMouse)
            {
                OnMouseEnter(this, EventArgs.Empty);
            }
            else if (active && !containsMouse)
            {
                OnMouseLeave(this, EventArgs.Empty);
            }
        }
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
