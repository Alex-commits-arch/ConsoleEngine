using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;
//using static WindowsWrapper.WinApi;

namespace ConsoleLibrary
{
    internal static class MyConsole
    {
        static IntPtr handle = WinApi.GetConsoleWindow();
        static ConsoleHandle handleIn = WinApi.GetStdHandle(ConsoleConstants.STD_INPUT_HANDLE);
        static ConsoleHandle handleOut = WinApi.GetStdHandle(ConsoleConstants.STD_OUTPUT_HANDLE);
        static ConsoleHandle consoleHandle = WinApi.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Create, 0, IntPtr.Zero);
        static int width;
        static int height;

        public static COORD GetWindowSize()
        {
            return new COORD((short)Width, (short)Height);
        }

        //static ConsoleHandle consoleHandle = WinApi.CreateFile("CONOUT$", 0x80000000, 2, IntPtr.Zero, FileMode.Create, 0, IntPtr.Zero);
        static IntPtr sysMenu = WinApi.GetSystemMenu(handle, false);

        public static int Width { get => width; private set => width = value; }
        public static int Height { get => height; private set => height = value; }

        private static void HandleError(bool errorOccurred, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
        {
            if (errorOccurred)
            {
                uint errorCode = WinApi.GetLastError();
                Debug.WriteLine($"Error {errorCode} occurred on line {lineNumber} in file {filePath}");
            }
        }

        public static void HideCursor()
        {
            Console.CursorVisible = false;
        }

        public static void ShowCursor()
        {
            Console.CursorVisible = true;
        }

        public static void Clear()
        {
            Fill(new CharInfo { Attributes = CharAttribute.ForegroundWhite, UnicodeChar = ' ' });
        }

        public static void Fill(CharInfo info)
        {
            WinApi.FillConsoleOutputCharacter(handleOut, info.UnicodeChar, Width * Height, new COORD(), out uint _);
            WinApi.FillConsoleOutputAttribute(handleOut, info.Attributes, Width * Height, new COORD(), out uint _);
        }

        public static void SetCursor(IDC_STANDARD_CURSORS cursor)
        {
            WinApi.SetCursor(WinApi.LoadCursor(handle, cursor));
            WinApi.SetCursor(IntPtr.Zero);
        }

        public static void GetMode(ref int mode)
        {
            WinApi.GetConsoleMode(handleIn, ref mode);
        }

        public static void SetMode(int mode)
        {
            WinApi.SetConsoleMode(handleIn, mode);
        }

        public static void ReadInput(ref INPUT_RECORD record, int length, ref uint recordLen)
        {
            WinApi.ReadConsoleInput(handleIn, ref record, 1, ref recordLen);
        }

        public static void WriteOutputCharacter(char[] chars, int length, COORD coord, out int numCharsWritten)
        {
            WinApi.WriteConsoleOutputCharacterW(handleOut, chars, length, coord, out numCharsWritten);
        }

        public static void WriteOutput(CharInfo[,] chars, COORD bufferSize, COORD coord)
        {
            SmallRect rect = new SmallRect(coord, bufferSize);
            WinApi.WriteConsoleOutputW(consoleHandle, chars, bufferSize, new COORD(), ref rect);
        }

        public static CharInfo GetCharInfo(int x, int y)
        {
            COORD size = new COORD(1, 1);
            SmallRect rect = new SmallRect(new COORD((short)x, (short)y), size);
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

        public static void SetSize(int width, int height)
        {
            MyConsole.width = width;
            MyConsole.height = height;
            Console.SetWindowSize(width, height);
            HandleError(!WinApi.SetConsoleScreenBufferSize(handleOut, new COORD((short)width, (short)height)));
            Console.SetWindowPosition(0, 0);
        }

        public static void SetTitle(string s)
        {
            WinApi.SetConsoleTitle(s);
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
            CONSOLE_FONT_INFO font;
            WinApi.GetCurrentConsoleFont(handleOut, false, out font);
            return WinApi.GetConsoleFontSize(handleOut, font.nFont);
        }
    }
}
