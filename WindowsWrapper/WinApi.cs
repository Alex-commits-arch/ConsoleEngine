using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        #region KERNEL32.DLL
        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleCP();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCP(int wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleOutputCP(int wCodePageID);

        [DllImport("kernel32.dll")]
        public static extern int GetConsoleOutputCP();

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int key);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(VirtualKeys vKey);

        [DllImport("user32.dll", SetLastError = true)]
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

        [DllImport("kernel32.dll")]
        public static extern ConsoleHandle CreateConsoleScreenBuffer(
            GenericAccess dwDesiredAccess,
            uint dwShareMode,
            IntPtr secutiryAttributes,
            uint flags,
            IntPtr screenBufferData
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DuplicateHandle(
            IntPtr hSourceProcessHandle,
            IntPtr hSourceHandle,
            IntPtr hTargetProcessHandle,
            out ConsoleHandle lpTargetHandle,
            short dwDesiredAccess,
            bool bInheritHandle,
            DuplicateOptions dwOptions
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleActiveScreenBuffer(ConsoleHandle hConsoleOutput);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetNumberOfConsoleInputEvents(
            IntPtr hConsoleInput,
            out uint lpcNumberOfEvents
        );

        //[DllImport("kernel32.dll")]
        //public static extern uint GetLastError();

        [DllImport("kernel32.dll")]
        static extern int FormatMessage(
            FormatMessage dwFlags,
            IntPtr lpSource,
            int dwMessageId,
            uint dwLanguageId,
            out StringBuilder msgOut,
            int nSize,
            IntPtr Arguments
        );

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
            ConsoleHandle hConsoleOutput,
            bool bAbsolute,
            ref SMALL_RECT lpConsoleWindow
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

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern bool WriteConsoleOutputW(
            ConsoleHandle hConsoleOutput,
            [MarshalAs(UnmanagedType.LPArray), In] CharInfo[,] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpWriteRegion
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

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetConsoleMode(ConsoleHandle hConsoleHandle, ref ConsoleModes lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern ConsoleHandle GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadConsoleInput(
            ConsoleHandle hConsoleInput,
            ref INPUT_RECORD lpBuffer,
            uint nLength,
            ref uint lpNumberOfEventsRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadConsoleOutputCharacter(
            ConsoleHandle hConsoleOutput,
            [Out] char[] lpCharacter,
            uint nLength,
            COORD dwReadCoord,
            out uint lpNumberOfCharsRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadConsoleOutput(
            ConsoleHandle hConsoleOutput,
            [Out] CharInfo[,] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpReadRegion
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

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            ProcessAccessFlags processAccess,
            bool bInheritHandle,
            int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfo(
            ConsoleHandle hConsoleOutput,
            out CONSOLE_SCREEN_BUFFER_INFO ConsoleScreenBufferInfo
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfoEx(
            ConsoleHandle hConsoleOutput,
            ref CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfo
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCursorInfo(
            ConsoleHandle hConsoleOutput,
            [In] ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleCursorInfo(
            ConsoleHandle hConsoleOutput,
            out CONSOLE_CURSOR_INFO lpConsoleCursorInfo
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern COORD GetLargestConsoleWindowSize(
            ConsoleHandle hConsoleOutput
        );

        public static IntPtr OpenProcess(Process proc, ProcessAccessFlags flags)
        {
            return OpenProcess(flags, false, proc.Id);
        }

        #endregion

        #region USER32.DLL
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

        public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(
            WndProcDelegate lpPrevWndFunc,
            IntPtr hWnd,
            WM Msg,
            IntPtr wParam,
            IntPtr lParam
        );

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(
            IntPtr lpPrevWndFunc,
            IntPtr hWnd,
            WM Msg,
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
        public static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList lParam);

        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder buf, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageW(IntPtr hWnd, WM Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleIcon(IntPtr hIcon);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetConsoleMode(ConsoleHandle hConsoleHandle, ConsoleModes dwMode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IDC_STANDARD_CURSORS lpCursorName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, string lpCursorName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetCursor();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetCursorInfo(ref CURSORINFO pci);

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClassInfoExA(IntPtr hInstance, string lpszClass, out WNDCLASSEX lpwcx);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowScrollBar(IntPtr hWnd, ScrollBar wBar, bool bShow);

        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref System.Drawing.Point[] pt, [MarshalAs(UnmanagedType.U4)] int cPoints);

        [DllImport("user32", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);
        #endregion

        #region GDI32.DLL
        [DllImport("gdi32.dll")]
        public static extern uint SetPixel(IntPtr hdc, int X, int Y, COLORREF crColor);
        #endregion

        #region UTILITY
        public static int GetLastError()
        {
            return (Marshal.GetLastWin32Error());
        }

        public static string GetLastErrorMessage()
        {
            int lastError = GetLastError();

            if (lastError == 0)
                return "";

            uint langId = 0x0409;

            StringBuilder msgOut = new StringBuilder(256);
            var flags = Enums.FormatMessage.ALLOCATE_BUFFER | Enums.FormatMessage.FROM_SYSTEM | Enums.FormatMessage.IGNORE_INSERTS;
            FormatMessage(flags, IntPtr.Zero, lastError, langId, out msgOut, msgOut.Capacity, IntPtr.Zero);
            return msgOut.ToString().Trim();
        }

        public static string GetErrorMessage(int errorCode)
        {
            uint langId = 0x0409;

            StringBuilder msgOut = new StringBuilder(256);
            var flags = Enums.FormatMessage.ALLOCATE_BUFFER | Enums.FormatMessage.FROM_SYSTEM | Enums.FormatMessage.IGNORE_INSERTS;
            FormatMessage(flags, IntPtr.Zero, errorCode, langId, out msgOut, msgOut.Capacity, IntPtr.Zero);
            return msgOut.ToString().Trim();
        }
        #endregion
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
