﻿using ConsoleLibrary.Input.Events;
using System;
using System.Diagnostics;
using WindowsWrapper.Constants;
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

        private static bool[] keyStates = new bool[256];

        public static void ReadInput()
        {
            var record = new INPUT_RECORD();
            uint recordLen = 0;
            MouseButton prevMouseState = MouseButton.None;
            while (true)
            {
                MyConsole.ReadInput(ref record, 1, ref recordLen);
                //Console.SetCursorPosition(0, 0);
                switch (record.EventType)
                {
                    case ConsoleConstants.MOUSE_EVENT:
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

                    case ConsoleConstants.KEY_EVENT:
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
                    case ConsoleConstants.RESIZE_EVENT:
                        break;
                    //default:
                    //    Debug.WriteLine(record.EventType);
                    //    break;
                }
            }
        }
    }
}