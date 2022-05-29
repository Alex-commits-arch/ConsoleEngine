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
        private const int KeyPressed = 0x8000;

        private static bool[] stateBuffer = new bool[256];
        private static bool[] m_mouseNewState = new bool[5];
        private static INPUT_RECORD[] inBuffer = new INPUT_RECORD[32];
        private static COORD mousePos;
        private static bool m_bConsoleInFocus = true;

        public static void HandleEvents()
        {
            uint events = 0;
            //MyConsole.GetNumberOfInputEvents(out events);

            for (uint i = 0; i < events; i++)
            {
                switch ((EventType)inBuffer[i].EventType)
                {
                    case EventType.Focus:
                        {
                            //m_bConsoleInFocus = inBuffer[i].FocusEvent.bSetFocus;
                        }
                        break;

                    case EventType.Mouse:
                        {
                            //switch (inBuffer[i].MouseEvent.dwEventFlags)
                            //{
                            //    case MOUSE_EVENT_RECORD.MOUSE_MOVED:
                            //        {
                            //            mousePos = inBuffer[i].MouseEvent.dwMousePosition;
                            //            //m_mousePosY = inBuffer[i].MouseEvent.dwMousePosition.Y;
                            //        }
                            //        break;

                            //    case 0:
                            //        {
                            //            for (int m = 0; m < 5; m++)
                            //                //m_mouseNewState[m] = (inBuffer[i].MouseEvent.dwButtonState & (1 << m)) > 0;

                            //        }
                            //        break;

                            //    default:
                            //        break;
                            //}
                        }
                        break;

                    default:
                        break;
                }
            }
        }

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

        public static Point GetMousePosition()
        {
            WinApi.GetCursorPos(out POINT mousePos);
            //WinApi.GetWindowRect(WinApi.GetConsoleWindow(), out RECT rect);

            int inWindowX = mousePos.X - MyConsole.ClientArea.Left;
            int inWindowY = mousePos.Y - MyConsole.ClientArea.Top;

            Point fontSize = MyConsole.GetFontSize();

            return new Point(inWindowX, inWindowY) / fontSize;

            return new Point(mousePos.X, mousePos.Y);
        }
    }
}
