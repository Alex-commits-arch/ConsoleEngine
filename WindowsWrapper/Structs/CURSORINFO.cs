using System;
using System.Runtime.InteropServices;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CURSORINFO
    {
        public Int32 cbSize;        
        public Int32 flags;
        public IntPtr hCursor;
        public POINT ptScreenPos;

        public override string ToString()
        {
            return $"hCursor: {hCursor}, flags: {flags}, X: {ptScreenPos.X}, Y: {ptScreenPos.Y} ";
        }
    }
}
