using ConsoleLibrary.Structures;
using System;
using System.Diagnostics;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Interop;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;
//using static WindowsWrapper.WinApi;

namespace ConsoleLibrary
{
    internal static class MyConsole
    {
        static readonly IntPtr handle = WinApi.GetConsoleWindow();
        static readonly IntPtr sysMenu = WinApi.GetSystemMenu(handle, false);
        static readonly ConsoleHandle handleIn = WinApi.GetStdHandle(ConsoleConstants.STD_INPUT_HANDLE);
        static readonly ConsoleHandle handleOut = WinApi.GetStdHandle(ConsoleConstants.STD_OUTPUT_HANDLE);
        static readonly ConsoleHandle consoleHandle = WinApi.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Create, 0, IntPtr.Zero);
        static readonly int minWindowWidth = WinApi.GetSystemMetrics(SystemMetric.SM_CXMIN);
        static readonly int minWindowHeight = WinApi.GetSystemMetrics(SystemMetric.SM_CYMIN);
        static readonly int screenWidth = WinApi.GetSystemMetrics(SystemMetric.SM_CXSCREEN);
        static readonly int screenHeight = WinApi.GetSystemMetrics(SystemMetric.SM_CYSCREEN);
        static readonly int captionHeight = WinApi.GetSystemMetrics(SystemMetric.SM_CYCAPTION);
        static readonly int frameWidth = WinApi.GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME);
        static readonly int frameHeight = WinApi.GetSystemMetrics(SystemMetric.SM_CYSIZEFRAME);
        static readonly int edgeWidth = WinApi.GetSystemMetrics(SystemMetric.SM_CXEDGE);
        static readonly int edgeHeight = WinApi.GetSystemMetrics(SystemMetric.SM_CYEDGE);
        static readonly int titleBarHeight = captionHeight + frameHeight + edgeHeight * 2;
        static int minConsoleWidth = minWindowWidth / GetFontSize().X;
        static int minConsoleHeight = minWindowHeight / GetFontSize().Y;
        static int maxConsoleWidth = screenWidth / GetFontSize().X;
        static int maxConsoleHeight = screenHeight / GetFontSize().Y;
        static int width;
        static int height;

        private static COORD MaxSize => WinApi.GetLargestConsoleWindowSize(handleOut);

        public static int Width { get => width; private set => width = value; }
        public static int Height { get => height; private set => height = value; }
        public static int MaximumWidth => maxConsoleWidth;
        public static int MaximumHeight => maxConsoleHeight;
        public static int TitleBarHeight => titleBarHeight;
        public static int BorderWidth => frameWidth + edgeWidth;

        public static COORD GetConsoleSize()
        {
            return new COORD((short)Width, (short)Height);
        }

        public static COORD GetClientSize()
        {
            WinApi.GetClientRect(handle, out RECT rect);

            return new COORD(
                (short)(rect.Right - rect.Left),
                (short)(rect.Bottom - rect.Top)
            );
        }

        public static COORD GetWindowSize()
        {
            WinApi.GetWindowRect(handle, out RECT rect);

            return new COORD(
                (short)(rect.Right - rect.Left),
                (short)(rect.Bottom - rect.Top)
            );
        }

        private static bool HandleError(bool errorOccurred, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
        {
            if (errorOccurred)
            {
                uint errorCode = WinApi.GetLastError();
                Debug.WriteLine($"Error {errorCode} occurred on line {lineNumber} in file {filePath}");
            }
            return errorOccurred;
        }

        public static void HideCursor()
        {
            CONSOLE_CURSOR_INFO cursorInfo = new CONSOLE_CURSOR_INFO { bVisible = false, dwSize = 1 };
            HandleError(!WinApi.SetConsoleCursorInfo(handleOut, ref cursorInfo));
        }

        public static void ShowCursor()
        {
            CONSOLE_CURSOR_INFO cursorInfo = new CONSOLE_CURSOR_INFO { bVisible = false, dwSize = 25 };
            HandleError(!WinApi.SetConsoleCursorInfo(handleOut, ref cursorInfo));
            //Console.CursorVisible = true;
        }

        public static void Clear(CharAttribute attributes = CharAttribute.ForegroundWhite)
        {
            Fill(new CharInfo { Attributes = attributes, UnicodeChar = '\0' });
        }

        public static void Fill(CharInfo info)
        {
            HandleError(!WinApi.FillConsoleOutputCharacter(handleOut, info.UnicodeChar, Width * Height, new COORD(), out uint _));
            HandleError(!WinApi.FillConsoleOutputAttribute(handleOut, info.Attributes, Width * Height, new COORD(), out uint _));
        }

        public static void SetCursor(IDC_STANDARD_CURSORS cursor) { }

        public static void GetMode(ref int mode) => WinApi.GetConsoleMode(handleIn, ref mode);
        public static void SetMode(int mode) => WinApi.SetConsoleMode(handleIn, mode);

        public static void ReadClientInput(ref INPUT_RECORD record, int length, ref uint recordLen)
        {
            WinApi.ReadConsoleInput(handleIn, ref record, 1, ref recordLen);
        }

        public static void WriteOutputCharacter(char[] chars, int length, COORD coord, out int numCharsWritten)
        {
            WinApi.WriteConsoleOutputCharacterW(handleOut, chars, length, coord, out numCharsWritten);
        }

        public static void WriteOutput(CharInfo[,] chars, COORD bufferSize, COORD coord)
        {
            SMALL_RECT rect = new SMALL_RECT(coord, bufferSize);
            WinApi.WriteConsoleOutputW(consoleHandle, chars, bufferSize, new COORD(), ref rect);
        }

        public static CharInfo GetCharInfo(int x, int y)
        {
            COORD size = new COORD(1, 1);
            SMALL_RECT rect = new SMALL_RECT(new COORD((short)x, (short)y), size);
            CharInfo[,] chars = new CharInfo[1, 1];
            WinApi.ReadConsoleOutput(
                handleOut,
                chars,
                size,
                new COORD(0, 0),
                ref rect
            );
            CharInfo info = chars[0, 0];

            return info;
        }

        public static char GetChar(int x, int y)
        {
            char[] chars = new char[1];
            WinApi.ReadConsoleOutputCharacter(
                handleOut,
                chars,
                1,
                new COORD((short)x, (short)y),
                out uint _
            );
            return chars[0];
        }

        public static void DisableResize()
        {
            if (!handleOut.IsInvalid)
            {
                HandleError(!WinApi.DeleteMenu(sysMenu, Window.SC_MAXIMIZE, 0));
                HandleError(!WinApi.DeleteMenu(sysMenu, Window.SC_SIZE, 0));
            }
        }

        public static void DeleteMenu(int position, int flags)
        {
            WinApi.DeleteMenu(sysMenu, position, flags);
        }

        public static void UpdateMinimumSize()
        {
            (int fw, int fh) = GetFontSize();
            if (minConsoleWidth != minWindowWidth / fw)
                minConsoleWidth = minWindowWidth / fw;
            if (minConsoleHeight != minWindowHeight / fh)
                minConsoleHeight = minWindowHeight / fh;
        }

        public static void SetSize(int width, int height)
        {
            //Debug.WriteLine(width);
            if (width > 0 && width <= MaximumWidth && height > 0 && height <= MaximumHeight)
            {
                WinApi.GetConsoleScreenBufferInfo(handleOut, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);
                SMALL_RECT winInfo = bufferInfo.srWindow;

                COORD windowSize = new COORD((short)(winInfo.Right - winInfo.Left + 1), (short)(winInfo.Bottom - winInfo.Top + 1));

                SMALL_RECT info;
                if (windowSize.X > width || windowSize.Y > height)
                {
                    info = new SMALL_RECT(
                        0, 0,
                        (short)(Math.Min(width, windowSize.X) - 1),
                        (short)(Math.Min(height, windowSize.Y) - 1)
                    );

                    WinApi.SetConsoleWindowInfo(handleOut, true, ref info);
                }

                COORD size = new COORD((short)width, (short)height);
                HandleError(!WinApi.SetConsoleScreenBufferSize(handleOut, size));

                info = new SMALL_RECT(0, 0, (short)(width - 1), (short)(height - 1));
                WinApi.SetConsoleWindowInfo(handleOut, true, ref info);

                MyConsole.width = width;
                MyConsole.height = height;
            }
           //HandleError(!WinApi.ShowScrollBar(handleIn.DangerousGetHandle(), ScrollBar.SB_VERT, false));
        }

        public static void SetSizeDosBox(int width, int height)
        {
            CONSOLE_SCREEN_BUFFER_INFO csbi;
            SMALL_RECT rect;
            COORD window_dims, win_coords;

            WinApi.GetConsoleScreenBufferInfo(handleOut, out csbi);

            win_coords = WinApi.GetLargestConsoleWindowSize(handleOut);
            window_dims = csbi.dwSize;

            rect.Right = (short)(Math.Min(width, win_coords.X) - 1);
            rect.Bottom = (short)(Math.Min(height, win_coords.Y) - 1);
            rect.Left = rect.Top = 0;

            win_coords.X = (short)width;
            win_coords.Y = (short)height;

            if (window_dims.X * window_dims.Y > width * height)
            {
                WinApi.SetConsoleWindowInfo(handleOut, true, ref rect);
                WinApi.SetConsoleScreenBufferSize(handleOut, win_coords);
            }
            if (window_dims.X * window_dims.Y < width * height)
            {
                WinApi.SetConsoleScreenBufferSize(handleOut, win_coords);
                WinApi.SetConsoleWindowInfo(handleOut, true, ref rect);
            }
        }

        public static void SetSizeSaved(int width, int height)
        {
            WinApi.GetConsoleScreenBufferInfo(handleOut, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

            //if (width > bufferInfo.dwMaximumWindowSize.X)
            //    Debug.WriteLine($"Max: {bufferInfo.dwMaximumWindowSize}, Target: {new Point(width, height)}");

            //if (width > bufferInfo.dwMaximumWindowSize.X)
            //    width = bufferInfo.dwMaximumWindowSize.X;
            //if (height > bufferInfo.dwMaximumWindowSize.Y)
            //    height = bufferInfo.dwMaximumWindowSize.Y;

            if (width < minConsoleWidth)
                width = minConsoleWidth;
            if (height < minConsoleHeight)
                height = minConsoleHeight;

            var rect = new SMALL_RECT(0, 0, (short)(width - 1), (short)(height - 1));
            //var smallest = new SMALL_RECT(0, 0, 1, 1);
            var smallest = new SMALL_RECT(0, 0, (short)(rect.Right - 1), (short)(rect.Bottom - 1));
            //if (width < Width)
            //{
            //    HandleError(!WinApi.SetConsoleWindowInfo(handleOut, true, ref rect));
            //    HandleError(!WinApi.SetConsoleScreenBufferSize(handleOut, new COORD((short)width, (short)height)));
            //}
            //else
            //{
            //    HandleError(!WinApi.SetConsoleScreenBufferSize(handleOut, new COORD((short)width, (short)height)));
            //    HandleError(!WinApi.SetConsoleWindowInfo(handleOut, true, ref rect));
            //}
            HandleError(!WinApi.SetConsoleWindowInfo(handleOut, true, ref smallest));
            if (!WinApi.SetConsoleScreenBufferSize(handleOut, new COORD((short)width, (short)height)))
            {
                Debug.WriteLine($"{width}, {height}");
            }
            HandleError(!WinApi.SetConsoleWindowInfo(handleOut, true, ref rect));

            MyConsole.width = width;
            MyConsole.height = height;
            //bufferError = WinApi.SetConsoleScreenBufferSize(handleOut, new COORD((short)width, (short)height));
            //if (bufferError)
            //    HandleError(!(bufferError = WinApi.SetConsoleScreenBufferSize(handleOut, bufferInfo.dwMaximumWindowSize)));
            //if (bufferError)
            //    Debug.WriteLine($"{rect}, {width}, {height}");
        }

        public static void SetTitle(string s)
        {
            WinApi.SetConsoleTitle(s);
        }

        public static void SetIcon(System.Drawing.Icon icon)
        {
            WinApi.SetConsoleIcon(icon.Handle);
            WinApi.SendMessage(handle, WM.SETICON, 0x80, icon.Handle);
        }

        public static void SetFontSize(int width, int height)
        {
            CONSOLE_FONT_INFO_EX cfi = new CONSOLE_FONT_INFO_EX();
            cfi.cbSize = (uint)Marshal.SizeOf(cfi);
            cfi.FaceName = "";

            cfi.cbSize = (uint)Marshal.SizeOf(cfi);
            cfi.nFont = 0;
            cfi.dwFontSize = new COORD((short)width, (short)height);
            cfi.FontFamily = 0;
            cfi.FontWeight = 400;
            cfi.FaceName = "Consolas";

            if (!WinApi.SetCurrentConsoleFontEx(handleOut, false, ref cfi))
                Debug.WriteLine(WinApi.GetLastError());
        }

        public static COORD GetFontSize()
        {
            WinApi.GetCurrentConsoleFont(handleOut, false, out CONSOLE_FONT_INFO font);
            return WinApi.GetConsoleFontSize(handleOut, font.nFont);
        }
    }
}
