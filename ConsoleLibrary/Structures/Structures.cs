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

        public static Location operator +(Location l1, Location l2) => new Location(l1.x + l2.x, l1.y + l2.y);
        public static Location operator -(Location l1, Location l2) => new Location(l1.x - l2.x, l1.y - l2.y);
        public static Location operator *(Location l1, Location l2) => new Location(l1.x * l2.x, l1.y * l2.y);
        public static Location operator /(Location l1, int val) => new Location(l1.x / val, l1.y / val);
        public static Location operator *(Location l1, int val) => new Location(l1.x * val, l1.y * val);
        public static bool operator ==(Location l1, Location l2) => l1.x == l2.x && l1.y == l2.y;
        public static bool operator !=(Location l1, Location l2) => l1.x != l2.x || l1.y != l2.y;
    }
}
