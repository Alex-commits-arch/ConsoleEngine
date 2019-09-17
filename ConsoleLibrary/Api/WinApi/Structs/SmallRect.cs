using System.Runtime.InteropServices;

namespace ConsoleLibrary.Api.WinApi.Structs
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

        public override string ToString()
        {
            return new { Left, Top, Right, Bottom }.ToString();
        }
    };
}
