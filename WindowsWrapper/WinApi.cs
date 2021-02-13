using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace WindowsWrapper
{
    //public const uint ENABLE_QUICK_EDIT = 0x0040;
    public static class WinApi
    {
        [DllImport("user32.dll")]
        public static extern bool DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleCP();

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCP(int wCodePageID);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleOutputCP(int wCodePageID);

        [DllImport("kernel32.dll")]
        public static extern int GetConsoleOutputCP();

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int key);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(VirtualKeys vKey);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetCurrentConsoleFontEx(
            ConsoleHandle ConsoleOutput,
            bool MaximumWindow,
            ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static bool GetCurrentConsoleFontEx(
            IntPtr hConsoleOutput,
            bool bMaximumWindow,
            ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFont
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static bool GetCurrentConsoleFont(
            ConsoleHandle hConsoleOutput,
            bool bMaximumWindow,
            out CONSOLE_FONT_INFO lpConsoleCurrentFont
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static COORD GetConsoleFontSize(
            ConsoleHandle hConsoleOutput,
            uint nFont
        );

        [DllImport("Kernel32.dll")]
        static extern IntPtr CreateConsoleScreenBuffer(
            UInt32 dwDesiredAccess,
            UInt32 dwShareMode,
            IntPtr secutiryAttributes,
            UInt32 flags,
            IntPtr screenBufferData
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetNumberOfConsoleInputEvents(
            IntPtr hConsoleInput,
            out uint lpcNumberOfEvents
        );

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll")]
        public static extern int WideCharToMultiByte(
            int CodePage,
            long dwFlags,
            char lpWideCharStr,
            int cchWideChar,
            string lpMultiByteStr,
            int cbMultiByte,
            char lpDefaultChar,
            bool lpUsedDefaultChar
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleWindowInfo(
            IntPtr hConsoleOutput,
            bool bAbsolute,
            [In] ref SmallRect lpConsoleWindow
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleTitle(
            string lpConsoleTitle
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleScreenBufferSize(
            ConsoleHandle hConsoleOutput,
            COORD dwSize
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutputCharacterW(
            ConsoleHandle hConsoleOutput,
            char[] lpCharacter,
            int nLength,
            COORD dwWriteCoord,
            out int lpumberOfCharsWritten
        );

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool WriteConsoleOutputW(
            ConsoleHandle hConsoleOutput,
            [MarshalAs(UnmanagedType.LPArray), In] CharInfo[,] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SmallRect lpWriteRegion
        );

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern ConsoleHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template
        );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetMessage(
            out MSG lpMsg,
            IntPtr hWnd,
            uint wMsgFilterMin,
            uint wMsgFilterMax
        );

        delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr CallWindowProc(
            WndProcDelegate lpPrevWndFunc,
            IntPtr hWnd,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam
        );

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(
            IntPtr hWnd,
            int nIndex,
            int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(
            IntPtr hWnd,
            int nIndex,
            IntPtr dwNewLong);

        public delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList lParam);

        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder buf, int nMaxCount);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean GetConsoleMode(ConsoleHandle hConsoleHandle, ref Int32 lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean ReadConsoleInput(
            ConsoleHandle hConsoleInput,
            ref INPUT_RECORD lpBuffer,
            UInt32 nLength,
            ref UInt32 lpNumberOfEventsRead
        );

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetConsoleMode(ConsoleHandle hConsoleHandle, Int32 dwMode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IDC_STANDARD_CURSORS lpCursorName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, string lpCursorName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetCursor();

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(ref CURSORINFO pci);

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool ReadConsoleOutputCharacter(
            ConsoleHandle hConsoleOutput,
            [Out] char[] lpCharacter,
            uint nLength,
            COORD dwReadCoord,
            out uint lpNumberOfCharsRead
        );

        [DllImport("kernel32.dll")]
        public static extern bool ReadConsoleOutput(
            ConsoleHandle hConsoleOutput,
            [Out] CharInfo[,] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SmallRect lpReadRegion
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FillConsoleOutputAttribute(
            ConsoleHandle hConsoleOutput,
            CharAttribute wAttribute,
            int nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfAttrsWritten
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FillConsoleOutputCharacter(
            ConsoleHandle hConsoleOutput,
            char cCharacter,
            int nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfCharsWritten
        );
    }
    public class ConsoleHandle : SafeHandleMinusOneIsInvalid
    {
        public ConsoleHandle() : base(false) { }

        protected override bool ReleaseHandle()
        {
            return true;
        }
    }
}
