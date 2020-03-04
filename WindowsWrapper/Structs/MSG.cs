using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        IntPtr hwnd;
        uint message;
        UIntPtr wParam; 
        IntPtr lParam;
        int time;
        POINT pt;
        //uint lPrivate;
    }
}
