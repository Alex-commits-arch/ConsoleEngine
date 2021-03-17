using System;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Input.Events
{
    public class MouseEventArgs : EventArgs
    {
        public MouseButton Button { get; set; }
        public COORD Location { get; set; }
        public ControlKeyState ControlKeyState { get; set; }
    }
}
