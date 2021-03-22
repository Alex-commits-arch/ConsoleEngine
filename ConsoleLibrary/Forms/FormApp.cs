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

namespace ConsoleLibrary.Forms
{
    public class FormApp : ConsoleApp
    {
        private string title;
        private System.Drawing.Icon icon;
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
            //ConsoleModes mode = 0;
            //MyConsole.GetMode(ref mode);
            //MyConsole.SetMode(ConsoleModes.ENABLE_EXTENDED_FLAGS | ConsoleModes.ENABLE_WINDOW_INPUT | ConsoleModes.ENABLE_MOUSE_INPUT);
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
            MyConsole.SetCursor(IDC_STANDARD_CURSORS.IDC_HAND);
            //WinApi.SendMessage
        }
    }
}
