using ConsoleLibrary.Structures;
using System;
using WindowsWrapper.Constants;

namespace ConsoleLibrary
{
    public abstract class ConsoleApp
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Point FontSize => new Point(MyConsole.GetFontSize());

        public ConsoleApp(int width = 40, int height = 30)
        {
            MyConsole.HideCursor();
            MyConsole.SetSize(
                Math.Min(width, Console.LargestWindowWidth),
                Math.Min(height, Console.LargestWindowHeight)
            );

            (Width, Height) = MyConsole.GetConsoleSize();
        }

        ~ConsoleApp()
        {
            Console.WriteLine("Hello");
        }

        public virtual void Init()
        {
            MyConsole.HideCursor();
            //MyConsole.DeleteMenu(Window.SC_MAXIMIZE, 0x0);
            //MyConsole.DeleteMenu(Window.SC_SIZE, 0x0);
        }
    }
}
