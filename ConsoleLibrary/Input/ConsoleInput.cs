using ConsoleLibrary.Input.Events;
using System;
using System.Diagnostics;
using WindowsWrapper;
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

        public delegate void ResizedEventHandler(ResizedEventArgs keyEventArgs);
        public static event ResizedEventHandler Resized;

        private static readonly bool[] keyStates = new bool[65535];

        private static readonly ConsoleHandle inputHandle = WinApi.GetStdHandle(ConsoleConstants.STD_INPUT_HANDLE);

        public static void InputLoop()
        {
            uint toRead = 128;
            var records = new INPUT_RECORD[toRead];
            int prevWidth = MyConsole.Width;
            int prevHeight = MyConsole.Height;
            MouseButton prevMouseState = MouseButton.None;
            COORD prevMouseLocation = new COORD();

            while (!MyConsole.Exiting)
            {
                WinApi.ReadConsoleInput(inputHandle, records, toRead, out uint recordLen);

                for (int i = 0; i < recordLen; i++)
                {
                    var record = records[i];

                    switch (record.EventType)
                    {
                        case EventType.Mouse:
                            {
                                var mouseEvent = record.Event.MouseEvent;
                                var button = mouseEvent.ButtonState;
                                var flags = mouseEvent.EventFlags;
                                var location = mouseEvent.MousePosition;

                                bool mousePressed = prevMouseState == MouseButton.None && button != MouseButton.None;
                                bool mouseReleased = prevMouseState != MouseButton.None && button == MouseButton.None;
                                bool mouseHeld = prevMouseState != MouseButton.None && button != MouseButton.None;

                                var args = new MouseEventArgs
                                {
                                    Button = button,
                                    Location = location,
                                    ControlKeyState = mouseEvent.ControlKeyState
                                };

                                bool sameLocation = location.Equals(prevMouseLocation);

                                if (mousePressed && flags.HasFlag(MouseState.DoubleClick))
                                    MouseDoubleClick?.Invoke(null, args);
                                else if (mousePressed)
                                    MousePressed?.Invoke(null, args);
                                else if (mouseReleased)
                                    MouseReleased?.Invoke(null, args);
                                else if (mouseHeld && flags.HasFlag(MouseState.Moved) && !sameLocation)
                                    MouseDragged?.Invoke(null, args);
                                else if (flags.HasFlag(MouseState.Moved) && !sameLocation)
                                    MouseMoved?.Invoke(null, args);

                                prevMouseState = button;
                                prevMouseLocation = location;
                            }
                            break;

                        case EventType.Key:
                            {
                                var keyEvent = record.Event.KeyEvent;

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
                                var clientSize = MyConsole.GetClientSize();
                                var fontSize = MyConsole.GetFontSize();
                                int w = clientSize.X / fontSize.X;
                                int h = clientSize.Y / fontSize.Y;

                                if (prevWidth != w || prevHeight != h)
                                {
                                    MyConsole.SetSize(w, h);
                                    MyConsole.HideCursor();
                                    Drawing.ConsoleRenderer.Resize(w, h);
                                    Resized?.Invoke(new ResizedEventArgs
                                    {
                                        Width = w,
                                        Height = h
                                    });
                                    prevWidth = w;
                                    prevHeight = h;
                                }
                            }
                            break;

                        case EventType.Menu:
                            {
                                var id = record.Event.MenuEvent.dwCommandId;
                                Debug.WriteLine(id);
                            }
                            break;
                        case EventType.Focus:
                            {
                                var focused = record.Event.FocusEvent.bSetFocus;
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
}