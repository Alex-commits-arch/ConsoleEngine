using System.Runtime.InteropServices;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSE_EVENT_RECORD
    {
        [FieldOffset(0)]
        public COORD dwMousePosition;

        public const int DOUBLE_CLICK = 0x0002,
        MOUSE_HWHEELED = 0x0008,
        MOUSE_MOVED = 0x0001,
        MOUSE_WHEELED = 0x0004;
        [FieldOffset(4)]
        public uint dwButtonState;
        [FieldOffset(8)]
        public uint dwControlKeyState;
        [FieldOffset(12)]
        public uint dwEventFlags;
    }
}
