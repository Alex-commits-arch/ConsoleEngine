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
    public class ControlManager : Control
    {
        private int prevMouseX;
        private int prevMouseY;
        private bool updating = false;
        private Rectangle oldRectangle;
        private Control controlUnderMouse;

        public ControlManager() : base(null)
        {
            name = "Control manager";
            controls = new List<Control>();

            ConsoleInput.MouseMoved += ConsoleInput_MouseMoved;
            ConsoleInput.MouseDragged += ConsoleInput_MouseDragged;
            ConsoleInput.MousePressed += ConsoleInput_MousePressed;
            ConsoleInput.MouseReleased += ConsoleInput_MouseReleased;
            ConsoleInput.MouseDoubleClick += ConsoleInput_MouseDoubleClick;
        }

        private void ConsoleInput_MouseDragged(object _, MouseEventArgs e) => controlUnderMouse?.HandleMouseDragged(e);
        private void ConsoleInput_MousePressed(object _, MouseEventArgs e) => controlUnderMouse?.HandleMousePressed(e);
        private void ConsoleInput_MouseReleased(object _, MouseEventArgs e) => controlUnderMouse?.HandleMouseReleased(e);
        private void ConsoleInput_MouseDoubleClick(object _, MouseEventArgs e) => controlUnderMouse?.HandleMouseDoubleClick(e);

        private void ConsoleInput_MouseMoved(object _, MouseEventArgs e)
        {
            (int x, int y) = e.Location;
            if (x != prevMouseX || y != prevMouseY)
            {
                if (controlUnderMouse != null)
                {
                    if (controlUnderMouse.ContainsPoint(x, y))
                    {
                        controlUnderMouse.HandleMouseMoved(e);
                    }
                    else
                    {
                        var control = ControlUnderMouse(x, y);

                        control?.HandleMouseEnter(e);
                        controlUnderMouse.HandleMouseLeave(e);

                        controlUnderMouse = control;
                    }
                }
                else
                {
                    controlUnderMouse = ControlUnderMouse(x, y);
                    controlUnderMouse?.HandleMouseEnter(e);
                }
                prevMouseX = x;
                prevMouseY = y;
            }
        }

        protected override void BeginUpdate(Control control)
        {
            if (updating)
                throw new Exception("EndUpdate must be called before calling BeginUpdate again.");

            if (control.Visible)
            {
                oldRectangle = control.Rectangle;
                updating = true;
                control.Invalidate();
            }
        }

        protected override void EndUpdate(Control control)
        {
            if (updating)
            {
                ConsoleRenderer.ActiveBuffer.Clear(oldRectangle);
                foreach (var ctrl in controls)
                    if (ctrl != control && ctrl.IntersectsWith(oldRectangle))
                        ctrl.Draw(ctrl.Rectangle.Intersect(oldRectangle));
                control.Draw(ConsoleRenderer.ActiveBuffer);

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

        public void Draw()
        {
            foreach (var control in controls)
                control.Draw(ConsoleRenderer.ActiveBuffer);
        }

        public void DrawControls()
        {
            foreach (var control in controls)
                if (control.Visible)
                    control.Draw(ConsoleRenderer.ActiveBuffer);
        }

        public void Add(Control control) => controls.Add(control);
        public void Remove(Control control) => controls.Remove(control);
    }
}
