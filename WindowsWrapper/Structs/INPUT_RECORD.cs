using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WindowsWrapper.Structs
{
    [DebuggerDisplay("EventType: {EventType}")]
    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT_RECORD
    {
        [FieldOffset(0)]
        public int EventType;
        [FieldOffset(4)]
        public KEY_EVENT_RECORD KeyEvent;
        [FieldOffset(4)]
        public MOUSE_EVENT_RECORD MouseEvent;
    }
}
