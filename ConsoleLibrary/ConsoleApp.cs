using ConsoleLibrary.Structures;
using System;
using WindowsWrapper.Constants;

namespace ConsoleLibrary
{
    public abstract class ConsoleApp
    {
        public int Width => MyConsole.Width;
        public int Height => MyConsole.Height;
        public Point FontSize => new Point(MyConsole.GetFontSize());

        public ConsoleApp(int width = 40, int height = 30)
        {
            MyConsole.SetSize(
                Math.Min(width, MyConsole.MaximumWidth),
                Math.Min(height, MyConsole.MaximumHeight)
            );
        }

        public virtual void Init()
        {
            MyConsole.HideCursor();
        }
    }
}
