using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Enums;
using static WindowsWrapper.WinApi;

namespace WindowsWrapper.Structs
{
    //[StructLayout(LayoutKind.Sequential)]
    //public struct WNDCLASSEX
    //{
    //    public uint cbSize;
    //    public ClassStyles style;
    //    [MarshalAs(UnmanagedType.FunctionPtr)]
    //    public WndProc lpfnWndProc;
    //    public int cbClsExtra;
    //    public int cbWndExtra;
    //    public IntPtr hInstance;
    //    public IntPtr hIcon;
    //    public IntPtr hCursor;
    //    public IntPtr hbrBackground;
    //    public string lpszMenuName;
    //    public string lpszClassName;
    //    public IntPtr hIconSm;
    //}

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WNDCLASSEX
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public int style;
        public IntPtr lpfnWndProc; // not WndProc
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
        public IntPtr hIconSm;

        public static WNDCLASSEX Build()
        {
            var nw = new WNDCLASSEX
            {
                cbSize = Marshal.SizeOf(typeof(WNDCLASSEX))
            };
            return nw;
        }
    }
}
