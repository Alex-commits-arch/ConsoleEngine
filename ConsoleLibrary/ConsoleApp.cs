using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Input;
using System;
using System.Diagnostics;
using WindowsWrapper;
using WindowsWrapper.Constants;

namespace ConsoleLibrary
{
    public abstract class ConsoleApp
    {
        //public int width, height;

        public ConsoleApp(int width = 40, int height = 30)
        {
            MyConsole.SetSize(width, height);
            //this.width = MyConsole.Width;
            //this.height = MyConsole.Height;
        }

        public virtual void Init()
        {
            MyConsole.HideCursor();
            MyConsole.DeleteMenu(Window.SC_MAXIMIZE, 0x0);
            MyConsole.DeleteMenu(Window.SC_SIZE, 0x0);

            //int mode = 0;
            //MyConsole.GetMode(ref mode);
            //mode |= ConsoleConstants.ENABLE_MOUSE_INPUT;
            //mode &= ~ConsoleConstants.ENABLE_QUICK_EDIT_MODE;
            //mode |= ConsoleConstants.ENABLE_EXTENDED_FLAGS;
            //MyConsole.SetMode(mode);
        }
    }
}
