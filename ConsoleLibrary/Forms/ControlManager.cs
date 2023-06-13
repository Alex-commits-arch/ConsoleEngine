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
        private Control activeControl;
        private Control controlUnderMouse;

        public override BufferArea Buffer => ConsoleRenderer.ActiveBuffer;

        public ControlManager() : base(null)
        {
            name = "Control manager";
            controls = new List<Control>();

            ConsoleInput.MouseMoved += ConsoleInput_MouseMoved;
            ConsoleInput.MouseDragged += ConsoleInput_MouseDragged;
            ConsoleInput.MousePressed += ConsoleInput_MousePressed;
            ConsoleInput.MouseReleased += ConsoleInput_MouseReleased;
            ConsoleInput.MouseDoubleClick += ConsoleInput_MouseDoubleClick;
            ConsoleInput.Resized += ConsoleInput_Resized;
        }

        private void ConsoleInput_Resized(ResizedEventArgs args)
        {
            //controls.ForEach(control =>
            //{
            //    control.HandleResized(args.Width, args.Height);
            //    control.Invalidate();
            //});
            //controls.ForEach(control => control.Refre);
        }

        private void ConsoleInput_MouseDragged(object _, MouseEventArgs e)
        {
            if (activeControl != null)
                activeControl.HandleMouseDragged(e);
            else
                controlUnderMouse?.HandleMouseDragged(e);
        }

        private void ConsoleInput_MousePressed(object _, MouseEventArgs e)
        {
            activeControl = controlUnderMouse;
            controlUnderMouse?.HandleMousePressed(e);
        }

        private void ConsoleInput_MouseReleased(object _, MouseEventArgs e)
        {
            activeControl?.HandleMouseReleased(e);
            if (controlUnderMouse != activeControl)
                controlUnderMouse?.HandleMouseReleased(e);
            activeControl = null;
        }

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
                oldRectangle = control.ClientRectangle;
                //ConsoleRenderer.ActiveBuffer.FillRect(oldRectangle, WindowsWrapper.Enums.CharAttribute.BackgroundYellow);
                //ConsoleRenderer.RenderArea(oldRectangle);
                updating = true;
                control.Invalidate();
            }
        }

        protected override void EndUpdate(Control control)
        {
            if (updating)
            {
                // Redraw old area
                ConsoleRenderer.ActiveBuffer.Clear(oldRectangle);
                foreach (var ctrl in controls)
                    if (ctrl != control && ctrl.IntersectsWith(oldRectangle))
                        ctrl.Draw(ctrl.ClientRectangle.Intersect(oldRectangle));
                //control.Draw(ConsoleRenderer.ActiveBuffer);
                control.Draw(control.Parent.Buffer);

                int index = controls.IndexOf(control);
                for (int i = index + 1; i < controls.Count; i++)
                {
                    var ctrl = controls[i];
                    if (ctrl.IntersectsWith(control.ClientRectangle))
                        ctrl.Draw(ctrl.ClientRectangle.Intersect(control.ClientRectangle));
                }

                ConsoleRenderer.RenderArea(oldRectangle);
                ConsoleRenderer.RenderArea(control.ClientRectangle);
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
