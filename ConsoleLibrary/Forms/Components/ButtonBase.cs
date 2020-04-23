using ConsoleLibrary.Forms.Interfaces;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Forms.Components
{
    public class ButtonBase : Component, IClickable, IMouseOver
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
                else
                    active = value;
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
                else
                    pressed = value;
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
                else
                    enabled = value;
            }
        }

        public event MouseEventHandler Click;
        public event MouseEventHandler DoubleClick;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;

        public ButtonBase()
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
            if (ContainsMouse(args.Location))
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
}
