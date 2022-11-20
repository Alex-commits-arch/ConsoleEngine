using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Structures
{
    public struct Point
    {
        public int X;
        public int Y;
        public bool initialized;


        public Point(int x, int y)
        {
            X = x;
            Y = y;
            initialized = true;
        }

        public Point(COORD coord)
        {
            X = coord.X;
            Y = coord.Y;
            initialized = true;
        }

        public Point(System.Drawing.Point point)
        {
            X = point.X;
            Y = point.Y;
            initialized = true;
        }

        public static int Distance(Point p1, Point p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            return (int)Math.Round(Math.Sqrt(dx * dx + dy * dy));
        }

        public Point Bound(Rectangle rect)
        {
            X = Math.Min(rect.Left + rect.Width - 1, Math.Max(rect.Left, X));
            Y = Math.Min(rect.Top + rect.Height - 1, Math.Max(rect.Top, Y));
            return this;
        }

        public void OnBorder(Rectangle rect)
        {
            Point center = rect.UpperLeft + rect.Size / 2;
            double angle = Math.Atan2(center.X, center.Y);
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point other)
                return other.X == X && other.Y == Y && other.initialized && initialized;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public static implicit operator System.Drawing.Point(Point p) => new System.Drawing.Point(p.X, p.Y);
        public static implicit operator Point(System.Drawing.Point p) => new Point(p);

        public static implicit operator COORD(Point p) => new COORD((short)p.X, (short)p.Y);
        public static implicit operator Point(COORD c) => new Point(c);

        public static Point operator +(Point l1, Point l2) => new Point(l1.X + l2.X, l1.Y + l2.Y);
        public static Point operator -(Point l1, Point l2) => new Point(l1.X - l2.X, l1.Y - l2.Y);
        public static Point operator *(Point l1, Point l2) => new Point(l1.X * l2.X, l1.Y * l2.Y);
        public static Point operator /(Point l1, Point l2) => new Point(l1.X / l2.X, l1.Y / l2.Y);
        public static Point operator /(Point l1, int val) => new Point(l1.X / val, l1.Y / val);
        public static Point operator *(Point l1, int val) => new Point(l1.X * val, l1.Y * val);
        public static bool operator ==(Point l1, Point l2) => l1.X == l2.X && l1.Y == l2.Y;
        public static bool operator !=(Point l1, Point l2) => l1.X != l2.X || l1.Y != l2.Y;
    }
}
