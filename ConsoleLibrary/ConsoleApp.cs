using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Input;
using System;
using WindowsWrapper;
using WindowsWrapper.Constants;

namespace ConsoleLibrary
{
    public abstract class ConsoleApp
    {
        private const int MF_BYCOMMAND = 0x00000000;

        public DrawingContext context;
        public InputManager inputManager;

        public int width, height;
        public bool running = true;
        
        public ConsoleApp(int width = 40, int height = 30)
        {
            this.width = width;
            this.height = height;
            SetupSize(width, height);
        }

        public virtual void Init()
        {
            IntPtr handle = WinApi.GetConsoleWindow();
            IntPtr sysMenu = WinApi.GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                WinApi.DeleteMenu(sysMenu, Window.SC_MAXIMIZE, MF_BYCOMMAND);
                WinApi.DeleteMenu(sysMenu, Window.SC_SIZE, MF_BYCOMMAND);
            }
            context = new DrawingContext(width, height);
            inputManager = new InputManager();

            inputManager.Init();
        }

        public virtual void Loop(Action update)
        {
            while (running)
                update();
        }

        public void Exit()
        {
            running = false;
            inputManager.Exit();
        }

        private void SetupSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            Console.SetWindowPosition(0, 0);
        }
    }
}
