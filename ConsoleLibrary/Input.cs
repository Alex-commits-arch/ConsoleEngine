using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsWrapper;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;
using static ConsoleLibrary.InputManager;

namespace ConsoleLibrary
{
    public struct KeyHandler
    {
        public Action<double> action;
        public KeyState state;

        public KeyHandler(KeyState state, Action<double> action)
        {
            this.state = state;
            this.action = action;
        }
    }

    public static class Input
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
            MyConsole.GetNumberOfInputEvents(out events);

            for (uint i = 0; i < events; i++)
            {
                switch ((EventType)inBuffer[i].EventType)
                {
                    case EventType.FOCUS_EVENT:
                        {
                            m_bConsoleInFocus = inBuffer[i].FocusEvent.bSetFocus;
                        }
                        break;

                    case EventType.MOUSE_EVENT:
                        {
                            switch (inBuffer[i].MouseEvent.dwEventFlags)
                            {
                                case MOUSE_EVENT_RECORD.MOUSE_MOVED:
                                    {
                                        mousePos = inBuffer[i].MouseEvent.dwMousePosition;
                                        //m_mousePosY = inBuffer[i].MouseEvent.dwMousePosition.Y;
                                    }
                                    break;

                                case 0:
                                    {
                                        for (int m = 0; m < 5; m++)
                                            m_mouseNewState[m] = (inBuffer[i].MouseEvent.dwButtonState & (1 << m)) > 0;

                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;

                    default:
                        break;
                        // We don't care just at the moment
                }
            }
            //uint events = WinApi.GetNumberOfConsoleInputEvents()
        }

        public static bool IsPressed(VirtualKeys key)
        {
            return (WinApi.GetAsyncKeyState(key) & (1 << 15)) != 0;
        }

        public static bool IsReleased(VirtualKeys key)
        {
            return (WinApi.GetAsyncKeyState(key) & (1 << 15)) == 0;
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

        public static POINT GetMousePosition()
        {
            POINT mousePos;
            WinApi.GetCursorPos(out mousePos);
            return mousePos;
        }
    }

    public class InputManager
    {
        Dictionary<KeyCode, bool> keyStates;
        Dictionary<KeyCode, bool> prevKeyStates;
        Dictionary<KeyCode, KeyHandler> actionMapping;
        private bool running = true;

        public InputManager()
        {
            keyStates = new Dictionary<KeyCode, bool>();
            prevKeyStates = new Dictionary<KeyCode, bool>();
            actionMapping = new Dictionary<KeyCode, KeyHandler>();
        }

        public void Register(KeyCode key, KeyHandler keyHandler)
        {
            actionMapping[key] = keyHandler;
        }

        public void Init()
        {
            Thread t = new Thread(MainLoop);
            t.Start();
        }

        DateTime currentTime = DateTime.Now;
        DateTime prevTime = DateTime.Now;
        private void MainLoop()
        {
            while (running)
            {
                currentTime = DateTime.Now;
                TimeSpan deltaTime = currentTime - prevTime;
                //Debug.WriteLine(1.0f / deltaTime.TotalMilliseconds);
                //Debug.Print("hello");
                //MyConsole.GetMessage();
                foreach (var key in Enum.GetValues(typeof(KeyCode)))
                {
                    KeyCode code = (KeyCode)key;
                    KeyState state = KeyState.Released;
                    keyStates[code] = NativeKeyboard.IsKeyDown(code);

                    if (keyStates[code] && !prevKeyStates[code])
                        state = KeyState.Pressed;
                    else if (keyStates[code] && prevKeyStates[code])
                        state = KeyState.Held;

                    if (actionMapping.ContainsKey(code) && actionMapping[code].state == state)
                        actionMapping[code].action(deltaTime.TotalSeconds);

                    prevKeyStates[code] = keyStates[code];
                }
                prevTime = currentTime;
            }
        }

        public void Exit()
        {
            running = false;
        }

        public enum KeyState
        {
            Released,
            Pressed,
            Held
        }

        public enum KeyCode : int
        {
            Left = 0x25,
            Up = 0x26,
            Right = 0x27,
            Down = 0x28,
            Space = 0x20,
            PageUp = 0x21,
            PageDown = 0x22,
            Escape = 0x1B,
        }

        private static class NativeKeyboard
        {
            private const int KeyPressed = 0x8000;

            public static bool IsKeyDown(KeyCode key)
            {
                return (GetKeyState((int)key) & KeyPressed) != 0;
            }

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern short GetKeyState(int key);
        }
    }
}