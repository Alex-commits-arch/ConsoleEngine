using ConsoleLibrary.Structures;
using System;
using System.Linq;
using WindowsWrapper.Constants;

namespace ConsoleLibrary
{
    public abstract class ConsoleApp
    {
        public int Width => MyConsole.Width;
        public int Height => MyConsole.Height;
        public Point FontSize => MyConsole.GetFontSize();
        public Point ClientSize => MyConsole.GetClientSize();

        public ConsoleApp(int width = 40, int height = 30)
        {
            MyConsole.SetSize(
                Math.Min(width, MyConsole.MaximumWidth),
                Math.Min(height, MyConsole.MaximumHeight)
            );
        }

        public System.Drawing.Point[] MapToScreen(System.Drawing.Point[] points)
        {
            //System.Drawing.Point[] temp = points.Select(p => (System.Drawing.Point)p).ToArray();
            MyConsole.MapToScreen(ref points);
            return points;
            //return temp.Cast<Point>().ToArray();
        }

        public virtual void Init()
        {
            MyConsole.HideCursor();
        }
    }
}
