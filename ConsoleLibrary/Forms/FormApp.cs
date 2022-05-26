using ConsoleLibrary.Forms.Components;
using ConsoleLibrary.Forms.Interfaces;
using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System.Diagnostics;
using System.Linq;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;
using System.Drawing;

namespace ConsoleLibrary.Forms
{
    public class FormApp : ConsoleApp
    {
        private string title;
        private System.Drawing.Icon icon;
        protected ControlManager controlManager;

        public ControlManager ControlManager { get => controlManager; set => controlManager = value; }

        public System.Drawing.Icon Icon
        {
            get => Icon;
            set
            {
                MyConsole.SetIcon(value);
                icon = value;
            }
        }

        public FormApp(int width, int height) : base(width, height)
        {
            MyConsole.SetMode(ConsoleModes.ENABLE_EXTENDED_FLAGS | ConsoleModes.ENABLE_MOUSE_INPUT);
        }

        public override void Init()
        {
            base.Init();
            controlManager = new ControlManager();
            ConsoleInput.MouseMoved += OnMouseMoved;
            ConsoleInput.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(KeyEventArgs keyEventArgs)
        {
            //MyConsole.SetSize(Width, Height);
        }

        private void OnMouseMoved(object sender, MouseEventArgs args)
        {
            //MyConsole.Test();

            //Icon = bmp.GetHicon();
            //MyConsole.SetIcon()
            //MyConsole.SetCursor(IDC_STANDARD_CURSORS.IDC_HAND);
            //WinApi.SendMessage
        }
    }
}
