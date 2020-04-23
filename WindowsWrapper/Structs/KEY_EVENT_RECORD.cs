using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsWrapper.Enums;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct KEY_EVENT_RECORD
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.Bool)]
        public bool KeyDown;
        [FieldOffset(4)]
        public short RepeatCount;
        [FieldOffset(6)]
        public short VirtualKeyCode;
        [FieldOffset(8)]
        public short VirtualScanCode;
        [FieldOffset(10)]
        public char UnicodeChar;
        [FieldOffset(10)]
        public byte AsciiChar;
        [FieldOffset(12)]
        public ControlKeyState ControlKeyState;
    };
}
