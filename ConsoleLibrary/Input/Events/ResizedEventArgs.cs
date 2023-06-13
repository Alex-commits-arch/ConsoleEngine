using System;
using WindowsWrapper.Enums;

namespace ConsoleLibrary.Input.Events
{
    public class ResizedEventArgs : EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public ResizedEventArgs() { }

        public ResizedEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
