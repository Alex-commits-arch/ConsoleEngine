using System;
using WindowsWrapper.Enums;

namespace ConsoleLibrary.Input.Events
{
    public class KeyEventArgs : EventArgs
    {
        public ConsoleKey Key { get; set; }
        public short RepeatCount { get; set; }
        public ControlKeyState ControlKeyState { get; set; }
    }
}
