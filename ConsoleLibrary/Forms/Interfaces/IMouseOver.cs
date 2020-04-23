using ConsoleLibrary.Input.Events;
using System;

namespace ConsoleLibrary.Forms.Interfaces
{
    interface IMouseOver : IMouseAction
    {
        event EventHandler MouseEnter;
        event EventHandler MouseLeave;
    }
}
