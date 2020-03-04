using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;
using static WindowsWrapper.WinApi;

namespace ConsoleLibrary
{
    internal static class MyConsole
    {
        static IntPtr handle = GetConsoleWindow();
        static IntPtr handleIn = GetStdHandle(ConsoleConstants.STD_INPUT_HANDLE);
        static IntPtr handleOut = GetStdHandle(ConsoleConstants.STD_OUTPUT_HANDLE);
        static IntPtr sysMenu = GetSystemMenu(handle, false);

        private static void HandleError(bool errorOccurred, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
        {
            if (errorOccurred)
            {
                uint errorCode = GetLastError();
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

        public static void GetMessage()
        {
            Debug.WriteLine(handle);
            var test = new HandleRef(new WNDCLASSEX(), handle);
            //SetWindowLongPtr(handle, (int)WindowLongFlags.GWL_EXSTYLE, (IntPtr)0x08000000);
            //WNDCLASSEX wnd = (WNDCLASSEX)Marshal.PtrToStructure(handle, typeof(WNDCLASSEX));
            var sb = new StringBuilder();
            int n = GetClassName(handle, sb, sb.MaxCapacity);
            Debug.WriteLine(n > 0 ? sb.ToString() : "Failed to get class name");
            
        }

        public static void SetMode(short mode)
        {
            if (!SetConsoleMode(handleIn, mode))
                Debug.WriteLine(GetLastError());
        }

        public static void DisableResize()
        {
            if (handleOut != IntPtr.Zero)
            {
                HandleError(!DeleteMenu(sysMenu, Window.SC_MAXIMIZE, 0));
                HandleError(!DeleteMenu(sysMenu, Window.SC_SIZE, 0));
            }
        }

        public static void SetSize(int width, int height)
        {
            //SMALL_RECT window = new SMALL_RECT(0, 0, 1, 1);
            Console.SetWindowSize(width, height);
            HandleError(!SetConsoleScreenBufferSize(handleOut, new COORD((short)width, (short)height)));
            Console.SetWindowPosition(0, 0);
        }

        public static void GetNumberOfInputEvents(out uint events)
        {
            GetNumberOfConsoleInputEvents(handleIn, out events);
        }

        public static void ReadInput(INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsRead)
        {
            ReadConsoleInput(handleIn, lpBuffer, nLength, out lpNumberOfEventsRead);
        }

        public static void SetTitle(string s)
        {
            SetConsoleTitle(s);
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

            if (!SetCurrentConsoleFontEx(handleOut, false, ref cfi))
                Debug.WriteLine(GetLastError());
        }

        public static ArrayList GetWindows()
        {
            ArrayList windowHandles = new ArrayList();
            EnumedWindow callBackPtr = GetWindowHandle;
            EnumWindows(callBackPtr, windowHandles);

            return windowHandles;
        }

        private static bool GetWindowHandle(IntPtr windowHandle, ArrayList windowHandles)
        {
            windowHandles.Add(windowHandle);
            return true;
        }
    }
}
