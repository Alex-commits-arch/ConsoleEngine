using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ConsoleLibrary.Input.InputManager;

namespace ConsoleLibrary.Input
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
                
                foreach (var key in Enum.GetValues(typeof(KeyCode)))
                {
                    KeyCode code = (KeyCode)key;
                    KeyState state = KeyState.Released;
                    keyStates[code] = NativeKeyboard.IsKeyDown(code);

                    if (keyStates[code] && !prevKeyStates[code])
                        state = KeyState.Pressed;
                    else if (keyStates[code] && prevKeyStates[code])
                        state = KeyState.Held;

                    if(actionMapping.ContainsKey(code) && actionMapping[code].state == state)
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