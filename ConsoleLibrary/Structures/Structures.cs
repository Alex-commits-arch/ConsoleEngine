using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Structures
{
    public struct Point
    {
        public int x, y;
        public bool initialized;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
            initialized = true;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", x, y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                var other = (Point)obj;
                return other.x == x && other.y == y && other.initialized && initialized;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Point operator +(Point l1, Point l2) => new Point(l1.x + l2.x, l1.y + l2.y);
        public static Point operator -(Point l1, Point l2) => new Point(l1.x - l2.x, l1.y - l2.y);
        public static Point operator *(Point l1, Point l2) => new Point(l1.x * l2.x, l1.y * l2.y);
        public static Point operator /(Point l1, int val) => new Point(l1.x / val, l1.y / val);
        public static Point operator *(Point l1, int val) => new Point(l1.x * val, l1.y * val);
        public static bool operator ==(Point l1, Point l2) => l1.x == l2.x && l1.y == l2.y;
        public static bool operator !=(Point l1, Point l2) => l1.x != l2.x || l1.y != l2.y;
    }
}
