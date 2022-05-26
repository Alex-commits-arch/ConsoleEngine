using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using WindowsWrapper.Structs;
using static ConsoleLibrary.Input.ConsoleInput;

namespace ConsoleLibrary.Forms.Interfaces
{
    interface IClickable : IMouseAction
    {
        event MouseEventHandler Click;
        event MouseEventHandler DoubleClick;
        void PerformClick(MouseEventArgs args = null);
    }
}
