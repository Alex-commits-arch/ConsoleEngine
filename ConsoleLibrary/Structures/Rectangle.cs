using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Structures
{
    public struct Rectangle
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Right => Left + Width;
        public int Bottom => Top + Height;
        public Point UpperLeft => new Point(Left, Top);
        public Point LowerRight => new Point(Left + Width, Top + Height);
        public Point Size => new Point(Width, Height);

        public Rectangle(int x, int y, int width, int height)
        {
            Left = x;
            Top = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Point p1, Point p2)
        {
            Left = p1.X;
            Top = p1.Y;
            Width = p2.X;
            Height = p2.Y;
        }

        public static implicit operator SMALL_RECT(Rectangle rect) => new SMALL_RECT((short)rect.Left, (short)rect.Top, (short)rect.Right, (short)rect.Bottom);
        public static implicit operator Rectangle(SMALL_RECT rect) => new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        public static implicit operator Rectangle(RECT rect) => new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
    }
}
