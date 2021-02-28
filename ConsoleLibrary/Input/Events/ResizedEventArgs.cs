using System;
using WindowsWrapper.Enums;

namespace ConsoleLibrary.Input.Events
{
    public class ResizedEventArgs : EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
