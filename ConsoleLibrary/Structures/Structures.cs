using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Structures
{
    public struct Location
    {
        public int x, y;
        public bool initialized;

        public Location(int x, int y)
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
            if (obj is Location)
            {
                var other = (Location)obj;
                return other.x == x && other.y == y && other.initialized && initialized;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Location operator +(Location l1, Location l2) => new Location(l1.x + l2.x, l1.y + l2.y);
        public static Location operator -(Location l1, Location l2) => new Location(l1.x - l2.x, l1.y - l2.y);
        public static Location operator *(Location l1, Location l2) => new Location(l1.x * l2.x, l1.y * l2.y);
        public static Location operator /(Location l1, int val) => new Location(l1.x / val, l1.y / val);
        public static Location operator *(Location l1, int val) => new Location(l1.x * val, l1.y * val);
        public static bool operator ==(Location l1, Location l2) => l1.x == l2.x && l1.y == l2.y;
        public static bool operator !=(Location l1, Location l2) => l1.x != l2.x || l1.y != l2.y;
    }
}
