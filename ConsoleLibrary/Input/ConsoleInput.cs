using ConsoleLibrary.Input.Events;
using System;
using System.Diagnostics;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Input
{
    public static class ConsoleInput
    {
        public static event MouseEventHandler MousePressed;
        public static event MouseEventHandler MouseReleased;
        public static event MouseEventHandler MouseDragged;
        public static event MouseEventHandler MouseDoubleClick;
        public static event MouseEventHandler MouseMoved;

        public delegate void KeyEventHandler(KeyEventArgs keyEventArgs);
        public static event KeyEventHandler KeyPressed;
        public static event KeyEventHandler KeyReleased;
        public static event KeyEventHandler KeyHeld;

        public delegate void ResizedEventHandler(ResizedEventArgs keyEventArgs);
        public static event ResizedEventHandler Resized;

        private static readonly bool[] keyStates = new bool[65535];

        public static void InputLoop()
        {
            var record = new INPUT_RECORD();
            uint recordLen = 0;
            MouseButton prevMouseState = MouseButton.None;
            while (true)
            {
                MyConsole.ReadClientInput(ref record, 1, ref recordLen);
                switch (record.EventType)
                {
                    case EventType.Mouse:
                        {
                            var mouseEvent = record.MouseEvent;
                            var button = mouseEvent.ButtonState;
                            var flags = mouseEvent.EventFlags;

                            bool mousePressed = prevMouseState == MouseButton.None && button != MouseButton.None;
                            bool mouseReleased = prevMouseState != MouseButton.None && button == MouseButton.None;
                            bool mouseHeld = prevMouseState != MouseButton.None && button != MouseButton.None;

                            var args = new MouseEventArgs
                            {
                                Button = button,
                                Location = mouseEvent.MousePosition
                            };

                            if (mousePressed && flags.HasFlag(MouseState.DoubleClick))
                                MouseDoubleClick?.Invoke(null, args);
                            else if (mousePressed)
                                MousePressed?.Invoke(null, args);
                            else if (mouseReleased)
                                MouseReleased?.Invoke(null, args);
                            else if (mouseHeld && flags.HasFlag(MouseState.Moved))
                                MouseDragged?.Invoke(null, args);
                            else if (flags.HasFlag(MouseState.Moved))
                                MouseMoved?.Invoke(null, args);

                            prevMouseState = button;
                        }
                        break;

                    case EventType.Key:
                        {
                            var keyEvent = record.KeyEvent;

                            var eventArgs = new KeyEventArgs
                            {
                                Key = (ConsoleKey)keyEvent.VirtualKeyCode,
                                ControlKeyState = keyEvent.ControlKeyState
                            };

                            bool currState = keyEvent.KeyDown;
                            bool prevState = keyStates[keyEvent.VirtualKeyCode];

                            if (currState && !prevState)
                                KeyPressed?.Invoke(eventArgs);
                            else if (prevState && !currState)
                                KeyReleased?.Invoke(eventArgs);
                            else if (prevState && currState)
                                KeyHeld?.Invoke(eventArgs);

                            keyStates[keyEvent.VirtualKeyCode] = keyEvent.KeyDown;
                        }
                        break;

                    case EventType.Resize:
                        {
                            (int wWidth, int wHeight) = MyConsole.GetWindowSize();
                            (int fWidth, int fHeight) = MyConsole.GetFontSize();
                            int cWidth = wWidth / fWidth;
                            int cHeight = wHeight / fHeight;

                            (int w, int h) = MyConsole.GetConsoleSize();

                            if (w != cWidth || h != cHeight)
                            {
                                MyConsole.SetSize(cWidth, cHeight);
                                Resized?.Invoke(new ResizedEventArgs
                                {
                                    Width = cWidth,
                                    Height = cHeight
                                });
                                MyConsole.HideCursor();
                            }
                        }
                        break;
                    default:
                        Debug.WriteLine("Unhandled event: " + record.EventType);
                        break;
                }
            }
        }
    }
}