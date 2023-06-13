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
        private class KeyState
        {
            public bool Current { get; set; }
            public bool Previous { get; set; }
        }

        private static KeyState[] CreateKeyStates()
        {
            KeyState[] states = new KeyState[256];
            for (int i = 0; i < 256; i++)
                states[i] = new KeyState();
            return states;
        }

        private static KeyState[] buffer = CreateKeyStates();
        private static bool[] stateBuffer = new bool[256];
        private static Point mousePosition;

        private static void UpdateKeyState(VirtualKey key)
        {
            KeyState keyState = GetKeyState(key);
            keyState.Previous = keyState.Current;
            keyState.Current = (WinApi.GetAsyncKeyState(key) & 0x8000) != 0;
        }

        private static KeyState GetKeyState(VirtualKey key)
        {
            return buffer[(int)key];
        }

        public static bool IsPressed(VirtualKey key)
        {
            return GetKeyState(key).Current;
            //var state = (WinApi.GetAsyncKeyState(key) & 0x8000) != 0;
            //stateBuffer[(int)key] = state;
            //return state;
        }

        public static bool IsReleased(VirtualKey key)
        {
            return !GetKeyState(key).Current;
            //return !IsPressed(key);
        }

        public static bool IsFirstPressed(VirtualKey key)
        {
            KeyState state = GetKeyState(key);
            return state.Current && !state.Previous;
            //var prevState = stateBuffer[(int)key];
            //var state = (WinApi.GetAsyncKeyState(key) & 0x8000) != 0;
            //return IsPressed(key) && !prevState;
        }

        public static bool IsFirstReleased(VirtualKey key)
        {
            KeyState state = GetKeyState(key);
            return state.Previous && !state.Current;
            //var prevState = stateBuffer[(int)key];
            //var state = (WinApi.GetAsyncKeyState(key) & 0x8000) != 0;
            //return IsReleased(key) && prevState;
        }

        public static void Update()
        {
            mousePosition = GetMousePosition_();
            foreach (VirtualKey key in Enum.GetValues(typeof(VirtualKey)))
                UpdateKeyState(key);
        }

        public static Point GetMousePosition()
        {
            return mousePosition;
        }
        private static Point GetMousePosition_()
        {
            WinApi.GetCursorPos(out POINT mousePos);

            int inWindowX = mousePos.X - MyConsole.ClientArea.Left;
            int inWindowY = mousePos.Y - MyConsole.ClientArea.Top;

            Point fontSize = MyConsole.GetFontSize();

            return new Point(inWindowX, inWindowY) / fontSize;
        }
    }
}
