using System.Runtime.InteropServices;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int Left, int Top, int Right, int Bottom)
        {
            this.Left = Left;
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
        }

        public RECT(COORD pos, COORD size)
        {
            Left = pos.X;
            Top = pos.Y;
            Right = (Left + size.X);
            Bottom = (Top + size.Y);
        }

        public override string ToString()
        {
            return new { Left, Top, Right, Bottom }.ToString();
        }
    };
}
