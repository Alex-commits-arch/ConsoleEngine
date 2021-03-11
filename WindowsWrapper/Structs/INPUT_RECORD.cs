using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsWrapper.Enums;

namespace WindowsWrapper.Structs
{
    [DebuggerDisplay("EventType: {EventType}")]
    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT_RECORD
    {
        [FieldOffset(0)]
        public EventType EventType;
        [FieldOffset(4)]
        public KEY_EVENT_RECORD KeyEvent;
        [FieldOffset(4)]
        public MOUSE_EVENT_RECORD MouseEvent;
    }
}
