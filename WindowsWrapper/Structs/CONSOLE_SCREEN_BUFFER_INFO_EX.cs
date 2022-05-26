using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SCREEN_BUFFER_INFO_EX
    {
        public uint cbSize;
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;

        public ushort wPopupAttributes;
        public bool bFullscreenSupported;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public COLORREF[] ColorTable;

        //internal COLORREF black;
        //internal COLORREF darkBlue;
        //internal COLORREF darkGreen;
        //internal COLORREF darkCyan;
        //internal COLORREF darkRed;
        //internal COLORREF darkMagenta;
        //internal COLORREF darkYellow;
        //internal COLORREF gray;
        //internal COLORREF darkGray;
        //internal COLORREF blue;
        //internal COLORREF green;
        //internal COLORREF cyan;
        //internal COLORREF red;
        //internal COLORREF magenta;
        //internal COLORREF yellow;
        //internal COLORREF white;

        // has been a while since I did this, test before use
        // but should be something like:
        //
        // [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
        // public COLORREF[] ColorTable;
    }
}
