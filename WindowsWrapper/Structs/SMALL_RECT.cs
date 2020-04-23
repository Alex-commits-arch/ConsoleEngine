using System.Runtime.InteropServices;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SmallRect
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;

        public SmallRect(short Left, short Top, short Right, short Bottom)
        {
            this.Left = Left;
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
        }

        public SmallRect(COORD pos, COORD size)
        {
            Left = pos.X;
            Top = pos.Y;
            Right =  size.X;
            Bottom = size.Y;
            Right = (short)(Left + size.X);
            Bottom = (short)(Top + size.Y);
        }

        public override string ToString()
        {
            return new { Left, Top, Right, Bottom }.ToString();
        }
    };
}
