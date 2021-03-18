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
        public int Right => Left + Width - 1;
        public int Bottom => Top + Height - 1;
        public Point UpperLeft => new Point(Left, Top);
        public Point LowerRight => new Point(Right, Bottom);
        public Point Size => new Point(Width, Height);

        public Rectangle(int x, int y, int width, int height)
        {
            Left = x;
            Top = y;
            Width = width;
            Height = height;
        }

        public override bool Equals(object obj)
        {
            if (obj is Rectangle rect)
                return Left == rect.Left && Top == rect.Top &&
                       Width == rect.Width && Height == rect.Height;
            return base.Equals(obj);
        }

        public Rectangle(Point p1, Point p2)
        {
            Left = p1.X;
            Top = p1.Y;
            Width = p2.X;
            Height = p2.Y;
        }

        public bool ContainsPoint(Point p)
        {
            return ContainsPoint(p.X, p.Y);
        }

        public bool ContainsPoint(int x, int y)
        {
            return x >= Left && x <= Right &&
                   y >= Top && y <= Bottom;
        }

        public bool IntersectsWith(Rectangle rect)
        {
            return rect.Right >= Left && rect.Left <= Right &&
                   rect.Bottom >= Top && rect.Top <= Bottom;
        }

        public Rectangle Intersect(Rectangle rect)
        {
            int x0 = Math.Max(Left, rect.Left);
            int y0 = Math.Max(Top, rect.Top);
            int x1 = Math.Min(Right, rect.Right);
            int y1 = Math.Min(Bottom, rect.Bottom);
            return new Rectangle(x0, y0, x1 - x0 + 1, y1 - y0 + 1);
        }

        public static bool operator ==(Rectangle self, Rectangle rect) => self.Equals(rect);
        public static bool operator !=(Rectangle self, Rectangle rect) => !self.Equals(rect);

        public static implicit operator SMALL_RECT(Rectangle rect) => new SMALL_RECT((short)rect.Left, (short)rect.Top, (short)rect.Right, (short)rect.Bottom);
        public static implicit operator Rectangle(SMALL_RECT rect) => new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        public static implicit operator Rectangle(RECT rect) => new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
    }
}
