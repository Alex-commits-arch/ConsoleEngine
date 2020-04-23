using System;
using System.Diagnostics;
using WindowsWrapper.Enums;

namespace WindowsWrapper.Structs
{
    [DebuggerDisplay("{dwMousePosition.X}, {dwMousePosition.Y}")]
    public struct MOUSE_EVENT_RECORD
    {
        public COORD MousePosition;
        public MouseButton ButtonState;
        public ControlKeyState ControlKeyState;
        public MouseState EventFlags;
    }
}
