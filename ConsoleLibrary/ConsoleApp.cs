using ConsoleLibrary.Graphics.Drawing;
using System;
using System.Diagnostics;
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
            MyConsole.SetSize(width, height);
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

        protected void Loop(Action update)
        {
            MyConsole.GetMessage();
            //var windows = MyConsole.GetWindows();
            
            //foreach (var window in windows)
            //{
            //    //Debug.Write()
            //    Debug.WriteLine(window);

            //}
            //while (running)
            //{

            //}
        }

        public void Exit()
        {
            running = false;
            inputManager.Exit();
        }
    }
}
