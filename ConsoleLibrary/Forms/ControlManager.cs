using ConsoleLibrary.Drawing;
using ConsoleLibrary.Forms.Controls;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using ConsoleLibrary.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace ConsoleLibrary.Forms
{
    public class ControlManager
    {
        private int prevMouseX;
        private int prevMouseY;
        private bool updating = false;
        private Rectangle oldRectangle;
        private Control controlUnderMouse;
        private readonly List<Control> controls;
        private readonly Dictionary<EventType, EventHandlerList> events;

        public List<Control> Controls => controls;

        public ControlManager()
        {
            controls = new List<Control>();

            events = new Dictionary<EventType, EventHandlerList>
            {
                {EventType.MouseMoved,       new EventHandlerList()},
                {EventType.MouseEnter,       new EventHandlerList()},
                {EventType.MouseLeave,       new EventHandlerList()},
                {EventType.MouseDragged,     new EventHandlerList()},
                {EventType.MousePressed,     new EventHandlerList()},
                {EventType.MouseReleased,    new EventHandlerList()},
                {EventType.MouseDoubleClick, new EventHandlerList()}
            };

            ConsoleInput.MouseMoved += ConsoleInput_MouseMoved;
            ConsoleInput.MouseDragged += ConsoleInput_MouseDragged;
            ConsoleInput.MousePressed += ConsoleInput_MousePressed;
            ConsoleInput.MouseReleased += ConsoleInput_MouseReleased;
            ConsoleInput.MouseDoubleClick += ConsoleInput_MouseDoubleClick;
        }

        public void SubscribeMouseEvent(EventType type, object subscriber, MouseEventHandler handler) => events[type].AddHandler(subscriber, handler);
        public void UnsubscribeMouseEvent(EventType type, object subscriber, MouseEventHandler handler) => events[type].RemoveHandler(subscriber, handler);

        private void ConsoleInput_MouseMoved(object _, MouseEventArgs e)
        {
            (int x, int y) = e.Location;
            if (x != prevMouseX || y != prevMouseY)
            {
                if (controlUnderMouse != null)
                {
                    if (controlUnderMouse.ContainsPoint(x, y))
                    {
                        InvokeMouseEvent(EventType.MouseMoved, controlUnderMouse, e);
                    }
                    else
                    {
                        var control = ControlUnderMouse(x, y);

                        InvokeMouseEvent(EventType.MouseEnter, control, e);
                        InvokeMouseEvent(EventType.MouseLeave, controlUnderMouse, e);

                        controlUnderMouse = control;
                    }
                }
                else
                {
                    controlUnderMouse = ControlUnderMouse(x, y);
                    InvokeMouseEvent(EventType.MouseEnter, controlUnderMouse, e);
                }
                prevMouseX = x;
                prevMouseY = y;
            }
        }

        private void ConsoleInput_MouseDragged(object _, MouseEventArgs e) => HandleMouseEvent(EventType.MouseDragged, e);
        private void ConsoleInput_MousePressed(object _, MouseEventArgs e) => HandleMouseEvent(EventType.MousePressed, e);
        private void ConsoleInput_MouseReleased(object _, MouseEventArgs e) => HandleMouseEvent(EventType.MouseReleased, e);
        private void ConsoleInput_MouseDoubleClick(object _, MouseEventArgs e) => HandleMouseEvent(EventType.MouseDoubleClick, e);

        private void HandleMouseEvent(EventType type, MouseEventArgs e)
        {
            (int x, int y) = e.Location;
            var control = ControlUnderMouse(x, y);
            InvokeMouseEvent(type, control, e);
        }

        private Control ControlUnderMouse(int x, int y)
        {
            for (int i = controls.Count - 1; i >= 0; i--)
            {
                Control control = controls[i];
                if (control.Visible && control.Enabled && control.ContainsPoint(x, y))
                    return control;
            }
            return null;
        }

        private void IntersectingControls(Rectangle rect)
        {

        }

        private void InvokeMouseEvent(EventType type, Control control, MouseEventArgs e)
        {
            (events[type][control] as MouseEventHandler)?.Invoke(control, e);
        }

        public void BeginUpdate(Control control)
        {
            if (control.Visible)
            {
                oldRectangle = control.Rectangle;
                updating = true;
                control.Invalidate();
            }
        }

        public void EndUpdate(Control control)
        {
            if (updating)
            {
                ConsoleRenderer.ActiveBuffer.Clear(oldRectangle);
                foreach (var ctrl in controls)
                    if (ctrl != control && ctrl.IntersectsWith(oldRectangle))
                        ctrl.Draw(ctrl.Rectangle.Intersect(oldRectangle));
                control.Draw();

                int index = controls.IndexOf(control);
                for (int i = index + 1; i < controls.Count; i++)
                {
                    var ctrl = controls[i];
                    if (ctrl.IntersectsWith(control.Rectangle))
                        ctrl.Draw(ctrl.Rectangle.Intersect(control.Rectangle));
                }

                ConsoleRenderer.RenderArea(oldRectangle);
                ConsoleRenderer.RenderArea(control.Rectangle);
                updating = false;
            }
        }

        public void DrawControls()
        {
            foreach (var control in controls)
                if (control.Visible)
                    control.Draw();
        }

        public void Add(Control control) => controls.Add(control);
        public void Remove(Control control) => controls.Remove(control);
    }
}
