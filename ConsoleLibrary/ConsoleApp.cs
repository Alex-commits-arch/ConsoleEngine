using ConsoleLibrary.Structures;
using System;
using System.Linq;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;

namespace ConsoleLibrary
{
    public abstract class ConsoleApp
    {
        public int Width => MyConsole.Width;
        public int Height => MyConsole.Height;
        public Point FontSize => MyConsole.GetFontSize();
        public Point ClientSize => MyConsole.GetClientSize();
        protected Rectangle ClientArea => MyConsole.ClientArea;

        public ConsoleApp(int width = 40, int height = 30)
        {
            var styles = MyConsole.WindowStyles;
            bool maximized = styles.HasFlag(WindowStyles.WS_MAXIMIZE) && styles.HasFlag(WindowStyles.WS_OVERLAPPEDWINDOW);
            bool fullscreen = styles.HasFlag(WindowStyles.WS_POPUP);

            int initialBufferHeight = MyConsole.GetConsoleBufferSize().Y;

            width = Math.Min(width, MyConsole.MaximumWidth);
            height = Math.Min(height, MyConsole.MaximumHeight);

            MyConsole.SetSize(width, height);
            MyConsole.Start();
            if (fullscreen)
            {
                MyConsole.SetBufferSize(MyConsole.MaximumWidth, MyConsole.MaximumHeight + 1);
                //MyConsole.SetWindowSize(MyConsole.MaximumWidth - 1, MyConsole.MaximumHeight - 1);
                MyConsole.SetSize(MyConsole.MaximumWidth, MyConsole.MaximumHeight);
            }
            else if (maximized)
            {
                MyConsole.SetBufferSize(MyConsole.MaximumWidth, initialBufferHeight+1);
                //MyConsole.SetWindowSize(MyConsole.MaximumWidth - 1, initialBufferHeight - 1);
                MyConsole.SetSize(MyConsole.MaximumWidth, initialBufferHeight);
            }
            else
            {
                MyConsole.SetSize(width, height);
            }
        }

        ~ConsoleApp()
        {
            Exit();
        }

        protected void Exit()
        {
            MyConsole.Exit();
        }

        public virtual void Init()
        {
            MyConsole.HideCursor();
        }
    }
}
