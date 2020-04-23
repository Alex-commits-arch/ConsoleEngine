using System.Runtime.InteropServices;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;

        public COORD(short X, short Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public void Deconstruct(out short x, out short y)
        {
            x = X;
            y = Y;
        }



        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
            //return base.ToString();
        }
    }
}
