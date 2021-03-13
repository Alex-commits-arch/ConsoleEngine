using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsWrapper.Enums;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT_RECORD_UNION
    {
        [FieldOffset(0)]
        public KEY_EVENT_RECORD KeyEvent;
        [FieldOffset(0)]
        public MOUSE_EVENT_RECORD MouseEvent;
        [FieldOffset(0)]
        public WINDOW_BUFFER_SIZE_RECORD ResizeEvent;
        [FieldOffset(0)]
        public MENU_EVENT_RECORD MenuEvent;
        [FieldOffset(0)]
        public FOCUS_EVENT_RECORD FocusEvent;
    };

    //[DebuggerDisplay("EventType: {EventType}")]
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT_RECORD
    {
        public EventType EventType;
        public INPUT_RECORD_UNION Event;
    }
}
