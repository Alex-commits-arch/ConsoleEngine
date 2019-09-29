using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Structs;

namespace ConsoleLibrary
{
    internal static class MyConsole
    {
        static IntPtr handle = WinApi.GetConsoleWindow();
        static IntPtr handleIn = WinApi.GetStdHandle(ConsoleConstants.STD_INPUT_HANDLE);
        static IntPtr handleOut = WinApi.GetStdHandle(ConsoleConstants.STD_OUTPUT_HANDLE);
        static IntPtr sysMenu = WinApi.GetSystemMenu(handle, false);

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

        public static void SetMode(short mode)
        {
            if (!WinApi.SetConsoleMode(handleIn, mode))
                Debug.WriteLine(WinApi.GetLastError());
        }

        public static void DisableResize()
        {
            if (handleOut != IntPtr.Zero)
            {
                HandleError(!WinApi.DeleteMenu(sysMenu, Window.SC_MAXIMIZE, 0));
                HandleError(!WinApi.DeleteMenu(sysMenu, Window.SC_SIZE, 0));
            }
        }

        public static void SetSize(int width, int height)
        {
            SMALL_RECT window = new SMALL_RECT(0, 0, 1, 1);
            Console.SetWindowSize(width, height);
            HandleError(!WinApi.SetConsoleScreenBufferSize(handleOut, new COORD((short)width, (short)height)));
            Console.SetWindowPosition(0, 0);
        }

        public static void GetNumberOfInputEvents(out uint events)
        {
            WinApi.GetNumberOfConsoleInputEvents(handleIn, out events);
        }

        public static void ReadInput(INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsRead)
        {
            WinApi.ReadConsoleInput(handleIn, lpBuffer, nLength, out lpNumberOfEventsRead);
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
    }
}
