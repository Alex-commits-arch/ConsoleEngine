using ConsoleLibrary.Forms.Components;
using ConsoleLibrary.Forms.Interfaces;
using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System.Diagnostics;
using System.Linq;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Forms
{
    public class FormApp : ConsoleApp
    {
        private string title;
        protected ControlManager controlManager;

        public ControlManager ControlManager { get => controlManager; set => controlManager = value; }

        public string Title
        {
            get => title;
            set
            {
                MyConsole.SetTitle(value);
                title = value;
            }
        }

        public FormApp(int width, int height) : base(width, height)
        {
            MyConsole.SetMode(ConsoleConstants.ENABLE_EXTENDED_FLAGS | ConsoleConstants.ENABLE_WINDOW_INPUT | ConsoleConstants.ENABLE_MOUSE_INPUT);
        }

        public override void Init()
        {
            base.Init();
            controlManager = new ControlManager();
            ConsoleInput.MouseMoved += OnMouseMoved;
            ConsoleInput.KeyPressed += OnKeyPressed;
        }

        //protected void DrawComponents()
        //{
        //    foreach (var component in Components)
        //    {
        //        if (component.Visible)
        //            component.Draw();
        //    }
        //}

        private void OnKeyPressed(KeyEventArgs keyEventArgs)
        {

        }

        private void OnMouseMoved(object sender, MouseEventArgs args)
        {
            //if (_cursor)
            //{
            //    var (currX, currY) = args.Location;
            //    if (_prevMouseX != currX || _prevMouseY != currY)
            //    {
            //        System.Threading.Thread.Sleep(40);
            //        ConsoleRenderer.DrawCursor(_prevMouseX, _prevMouseY, currX, currY);
            //        _prevMouseX = currX;
            //        _prevMouseY = currY;
            //    }
            //}
        }
    }
}
