using ConsoleLibrary.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Game
{
    public static class InputManager
    {
        private static bool[] stateBuffer = new bool[256];

        public static bool IsPressed(VirtualKeys key)
        {
            return (WinApi.GetAsyncKeyState(key) & 0x8000) != 0;
        }

        public static bool IsReleased(VirtualKeys key)
        {
            return (WinApi.GetAsyncKeyState(key) & 0x8000) == 0;
        }

        public static bool IsFirstPressed(VirtualKeys key)
        {
            bool prevState = stateBuffer[(int)key];

            stateBuffer[(int)key] = IsPressed(key);

            return stateBuffer[(int)key] && !prevState;
        }

        public static bool IsFirstReleased(VirtualKeys key)
        {
            bool prevState = stateBuffer[(int)key];

            stateBuffer[(int)key] = IsPressed(key);

            return !stateBuffer[(int)key] && prevState;
        }

        public static bool IsPressed(MouseButton mouseButton)
        {
            return false;
            //return MouseButton
        }

        public static Point GetMousePosition()
        {
            WinApi.GetCursorPos(out POINT mousePos);

            int inWindowX = mousePos.X - MyConsole.ClientArea.Left;
            int inWindowY = mousePos.Y - MyConsole.ClientArea.Top;

            Point fontSize = MyConsole.GetFontSize();

            return new Point(inWindowX, inWindowY) / fontSize;
        }
    }
}
