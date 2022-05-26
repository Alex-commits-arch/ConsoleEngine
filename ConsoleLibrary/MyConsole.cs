using ConsoleLibrary.Structures;
using System;
using System.ComponentModel;
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
        static readonly IntPtr windowHandle = WinApi.GetConsoleWindow();
        static readonly IntPtr sysMenu = WinApi.GetSystemMenu(windowHandle, false);
        static readonly ConsoleHandle inputHandle = WinApi.GetStdHandle(ConsoleConstants.STD_INPUT_HANDLE);
        static ConsoleHandle bufferHandle = WinApi.CreateConsoleScreenBuffer(GenericAccess.GENERIC_READ | GenericAccess.GENERIC_WRITE, 0x00000004, IntPtr.Zero, 1, IntPtr.Zero);

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
        static readonly Point originalBufferSize = GetConsoleBufferSize();
        static readonly Point originalWindowSize = GetConsoleWindowSize();
        static readonly bool startedMaximized = GetWindowInfo().dwStyle.HasFlag(WindowStyles.WS_MAXIMIZE) || !GetWindowInfo().dwStyle.HasFlag(WindowStyles.WS_OVERLAPPEDWINDOW);

        static bool exiting = false;

        public static string Title { get => GetTitle(); set => SetTitle(value); }
        public static int Width => GetConsoleBufferSize().X;
        public static int Height => GetConsoleBufferSize().Y;
        public static int MaximumWidth => screenWidth / Math.Max(GetFontSize().X, 1);
        public static int MaximumHeight => screenHeight / Math.Max(GetFontSize().Y, 1);
        public static int TitleBarHeight => titleBarHeight;
        public static int BorderWidth => frameWidth + edgeWidth;
        public static bool Exiting => exiting;
        public static bool StartedMaximized => startedMaximized;
        public static WindowStyles WindowStyles => GetWindowInfo().dwStyle;
        public static Rectangle ClientArea => GetWindowInfo().rcClient;
        public static bool ScrollBarVisible => WindowStyles.HasFlag(WindowStyles.WS_VSCROLL);

        public static void Start()
        {
            HandleError(!WinApi.SetConsoleActiveScreenBuffer(bufferHandle));
        }

        public static void Exit()
        {
            Clear();
            SetTitle("Exiting...");
            WinApi.SetConsoleActiveScreenBuffer(WinApi.GetStdHandle(ConsoleConstants.STD_OUTPUT_HANDLE));
            exiting = true;
        }

        private static WINDOWINFO GetWindowInfo()
        {
            WINDOWINFO info = new WINDOWINFO(true);
            HandleError(!WinApi.GetWindowInfo(windowHandle, ref info));
            return info;
        }
        //public static 

        /// <summary>
        /// Returns the size of the visible console window in rows and columns
        /// </summary>
        public static Point GetConsoleWindowSize()
        {
            HandleError(!WinApi.GetConsoleScreenBufferInfo(bufferHandle, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo));
            return bufferInfo.srWindow.LowerRight;
        }

        /// <summary>
        /// Returns the size of the console buffer in rows and columns
        /// </summary>
        public static Point GetConsoleBufferSize()
        {
            HandleError(!WinApi.GetConsoleScreenBufferInfo(bufferHandle, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo));
            return bufferInfo.dwSize;
        }

        /// <summary>
        /// Returns the size of the client area in pixels
        /// </summary>
        public static Point GetClientSize()
        {
            HandleError(!WinApi.GetClientRect(windowHandle, out RECT rect));
            return new Point(rect.Width, rect.Height);
        }

        /// <summary>
        /// Returns the size of the window in pixels
        /// </summary>
        public static Point GetWindowSize()
        {
            HandleError(!WinApi.GetWindowRect(windowHandle, out RECT rect));
            return new Point(rect.Width, rect.Height);
        }

        public static void UpdateColor()
        {
            var bufferInfo = GetExtendedBufferInfo();
            bufferInfo.ColorTable[0] = new COLORREF(System.Drawing.Color.CornflowerBlue);
            SetExtendedBufferInfo(bufferInfo);
        }

        public static CONSOLE_SCREEN_BUFFER_INFO_EX GetExtendedBufferInfo()
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX bufferInfo = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            bufferInfo.cbSize = (uint)Marshal.SizeOf(bufferInfo);
            HandleError(!WinApi.GetConsoleScreenBufferInfoEx(bufferHandle, ref bufferInfo));
            return bufferInfo;
        }

        public static void SetExtendedBufferInfo(CONSOLE_SCREEN_BUFFER_INFO_EX bufferInfo)
        {
            HandleError(!WinApi.SetConsoleScreenBufferInfoEx(bufferHandle, ref bufferInfo));
        }

        public delegate bool SystemErrorHandler(bool errorOccurred, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "");

        //private static bool HandleError(bool errorOccurred, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
        private static bool HandleError(bool errorOccurred, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = "")
        {
            if (errorOccurred)
            {
                int errorCode = WinApi.GetLastError();
                string message = WinApi.GetErrorMessage(errorCode).TrimEnd('.');
                Debug.WriteLine($"System error {errorCode} ({message}) occurred on line {lineNumber} in file {filePath}");
                Console.WriteLine($"System error {errorCode} ({message}) occurred on line {lineNumber} in file {filePath}");
            }
            return errorOccurred;
        }

        private static ConsoleHandle HandleInvalidHandle(ConsoleHandle handle, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
        {
            if (handle.IsInvalid)
            {
                int errorCode = Marshal.GetLastWin32Error();
                string message = WinApi.GetErrorMessage(errorCode);
                Debug.WriteLine($"System error {errorCode} ({message}) occurred on line {lineNumber} in file {filePath}");
            }
            return handle;
        }


        public static void HideCursor()
        {
            CONSOLE_CURSOR_INFO cursorInfo = new CONSOLE_CURSOR_INFO { bVisible = false, dwSize = 1 };
            HandleError(!WinApi.SetConsoleCursorInfo(bufferHandle, ref cursorInfo));
        }

        public static void ShowCursor()
        {
            CONSOLE_CURSOR_INFO cursorInfo = new CONSOLE_CURSOR_INFO { bVisible = false, dwSize = 25 };
            HandleError(!WinApi.SetConsoleCursorInfo(bufferHandle, ref cursorInfo));
        }

        //TODO: Maybe move to ConsoleRenderer
        public static void Clear(CharAttribute attributes = Drawing.ConsoleRenderer.DefaultAttributes)
        {
            Fill(new CharInfo { Attributes = attributes, UnicodeChar = '\0' });
        }

        //TODO: Maybe move to ConsoleRenderer
        public static void Fill(CharInfo info)
        {
            HandleError(!WinApi.FillConsoleOutputCharacter(bufferHandle, info.UnicodeChar, Width * Height, new COORD(), out uint _));
            HandleError(!WinApi.FillConsoleOutputAttribute(bufferHandle, info.Attributes, Width * Height, new COORD(), out uint _));
        }

        public static void SetCursor(IDC_STANDARD_CURSORS cursor) { }

        //TODO: Maybe move to ConsoleInput
        public static void GetMode(ref ConsoleModes mode) => WinApi.GetConsoleMode(inputHandle, ref mode);
        public static void SetMode(ConsoleModes mode) => WinApi.SetConsoleMode(inputHandle, mode);

        public static void WriteOutputCharacter(char[] chars, int length, COORD coord, out int numCharsWritten)
        {
            WinApi.WriteConsoleOutputCharacterW(bufferHandle, chars, length, coord, out numCharsWritten);
        }

        public static void WriteOutput(CharInfo[,] chars, COORD bufferSize, COORD coord)
        {
            SMALL_RECT rect = new SMALL_RECT(coord, bufferSize);
            WinApi.WriteConsoleOutputW(bufferHandle, chars, bufferSize, new COORD(), ref rect);
        }

        public static CharInfo GetCharInfo(int x, int y)
        {
            COORD size = new COORD(1, 1);
            SMALL_RECT rect = new SMALL_RECT(new COORD((short)x, (short)y), size);
            CharInfo[,] chars = new CharInfo[1, 1];
            WinApi.ReadConsoleOutput(
                bufferHandle,
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
                bufferHandle,
                chars,
                1,
                new COORD((short)x, (short)y),
                out uint _
            );
            return chars[0];
        }

        public static void DisableResize()
        {
            HandleError(!WinApi.DeleteMenu(sysMenu, Window.SC_MAXIMIZE, 0));
            HandleError(!WinApi.DeleteMenu(sysMenu, Window.SC_SIZE, 0));
        }

        public static void DeleteMenu(int position, int flags)
        {
            WinApi.DeleteMenu(sysMenu, position, flags);
        }

        public static void SetSize(int width, int height, int bufferHeight = -1)
        {
            if (width > 0 && width <= MaximumWidth && height > 0 && height <= MaximumHeight)
            {
                COORD bufferSize = GetConsoleBufferSize();

                if (bufferSize.X > width || bufferSize.Y > height)
                {
                    SetWindowSize(
                        Math.Min(width, bufferSize.X) - 1,
                        Math.Min(height, bufferSize.Y) - 1
                    );
                }

                SetBufferSize(width, bufferHeight > 0 ? bufferHeight : height);
                SetWindowSize(width - 1, height - 1);
            }
        }

        public static void SetWindowSize(int width, int height, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = "")
        {
            SMALL_RECT info = new SMALL_RECT(0, 0, (short)width, (short)height);
            HandleError(!WinApi.SetConsoleWindowInfo(bufferHandle, true, ref info), lineNumber, filePath);
        }

        public static void SetBufferSize(int width, int height, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = "")
        {
            COORD size = new COORD((short)width, (short)height);
            HandleError(!WinApi.SetConsoleScreenBufferSize(bufferHandle, size), lineNumber, filePath);
        }

        public static string GetTitle()
        {
            var sb = new System.Text.StringBuilder(128);
            HandleError(WinApi.GetConsoleTitle(sb, sb.Capacity) == 0);
            return sb.ToString();
        }

        public static void SetTitle(string s)
        {
            HandleError(!WinApi.SetConsoleTitle(s));
        }

        public static void SetIcon(System.Drawing.Icon icon)
        {
            HandleError(!WinApi.SetConsoleIcon(icon.Handle));
            WinApi.SendMessage(windowHandle, WM.SETICON, 0x80, icon.Handle);
        }

        static System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(16, 16);
        public static void Test()
        {
            //while (WinApi.ShowCursor(false) >= 0) { }
            //WinApi.SetCursor(IntPtr.Zero);
            IntPtr hCursor = WinApi.LoadCursor(IntPtr.Zero, IDC_STANDARD_CURSORS.IDC_CROSS);
            if (hCursor == IntPtr.Zero)
                Debug.WriteLine("Oj");
            HandleError(WinApi.SetClassLong(windowHandle, ClassLongFlags.GCLP_HCURSOR, hCursor) == IntPtr.Zero);
            return;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                System.Drawing.Icon icon = new System.Drawing.Icon(@"D:\Dokument\GIT\ConsoleEngine\ConsoleApiTest\Treetog-Junior-Monitor-test.ico");
                WinApi.SendMessage(windowHandle, WM.SETICON, 0, icon.Handle);
                WinApi.SendMessage(windowHandle, WM.SETICON, 1, icon.Handle);
            }
        }

        //TODO: Add SetFont method
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

            HandleError(!WinApi.SetCurrentConsoleFontEx(bufferHandle, false, ref cfi));
        }

        public static Point GetFontSize()
        {
            HandleError(!WinApi.GetCurrentConsoleFont(bufferHandle, false, out CONSOLE_FONT_INFO font));
            return WinApi.GetConsoleFontSize(bufferHandle, font.nFont);
        }
    }

}
