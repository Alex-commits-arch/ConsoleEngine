using ConsoleLibrary.Structures;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary
{
    /// <summary>
    /// A wrapper for console functionality
    /// </summary>
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

        public static int Width => GetConsoleSize().X;
        public static int Height => GetConsoleSize().Y;
        public static int MaximumWidth => screenWidth / GetFontSize().X;
        public static int MaximumHeight => screenHeight / GetFontSize().Y;
        public static int TitleBarHeight => titleBarHeight;
        public static int BorderWidth => frameWidth + edgeWidth;

        public static Point GetConsoleSize()
        {
            HandleError(!WinApi.GetConsoleScreenBufferInfo(handleOut, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo));
            return bufferInfo.dwSize;
        }

        public static Point GetClientSize()
        {
            HandleError(!WinApi.GetClientRect(handle, out RECT rect));
            return new Point(rect.Width, rect.Height);
        }

        public static Point GetWindowSize()
        {
            HandleError(!WinApi.GetWindowRect(handle, out RECT rect));
            return new Point(rect.Width, rect.Height);
        }

        private static bool HandleError(bool errorOccurred, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
        {
            if (errorOccurred)
            {
                int errorCode = Marshal.GetLastWin32Error();
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
        }

        public static void Clear(CharAttribute attributes = Drawing.ConsoleRenderer.DefaultAttributes)
        {
            Fill(new CharInfo { Attributes = attributes, UnicodeChar = '\0' });
        }

        public static void MapToScreen(ref System.Drawing.Point[] points)
        {
            WinApi.MapWindowPoints(consoleHandle.DangerousGetHandle(), IntPtr.Zero, ref points, points.Length);
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

        //public static void UpdateMinimumSize()
        //{
        //    (int fw, int fh) = GetFontSize();
        //    if (minConsoleWidth != minWindowWidth / fw)
        //        minConsoleWidth = minWindowWidth / fw;
        //    if (minConsoleHeight != minWindowHeight / fh)
        //        minConsoleHeight = minWindowHeight / fh;
        //}

        public static void SetSize(int width, int height)
        {
            if (width > 0 && width <= MaximumWidth && height > 0 && height <= MaximumHeight)
            {
                WinApi.GetConsoleScreenBufferInfo(handleOut, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

                COORD windowSize = bufferInfo.dwSize;

                SMALL_RECT info;
                if (windowSize.X > width || windowSize.Y > height)
                {
                    info = new SMALL_RECT(
                        0, 0,
                        (short)(Math.Min(width, windowSize.X) - 1),
                        (short)(Math.Min(height, windowSize.Y) - 1)
                    );

                    HandleError(!WinApi.SetConsoleWindowInfo(handleOut, true, ref info));
                }

                COORD size = new COORD((short)width, (short)height);
                HandleError(!WinApi.SetConsoleScreenBufferSize(handleOut, size));

                info = new SMALL_RECT(0, 0, (short)(width - 1), (short)(height - 1));
                HandleError(!WinApi.SetConsoleWindowInfo(handleOut, true, ref info));
            }
        }

        public static void SetTitle(string s)
        {
            HandleError(!WinApi.SetConsoleTitle(s));
        }

        public static void SetIcon(System.Drawing.Icon icon)
        {
            HandleError(!WinApi.SetConsoleIcon(icon.Handle));
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

            HandleError(!WinApi.SetCurrentConsoleFontEx(handleOut, false, ref cfi));
        }

        public static Point GetFontSize()
        {
            HandleError(!WinApi.GetCurrentConsoleFont(handleOut, false, out CONSOLE_FONT_INFO font));
            return WinApi.GetConsoleFontSize(handleOut, font.nFont);
        }
    }
}
