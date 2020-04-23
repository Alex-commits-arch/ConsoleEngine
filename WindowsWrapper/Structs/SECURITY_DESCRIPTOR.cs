using System;
using System.Runtime.InteropServices;

namespace WindowsWrapper.Constants
{
    [StructLayout(LayoutKind.Sequential)]
    struct SECURITY_DESCRIPTOR
    {
        public byte revision;
        public byte size;
        public short control;
        public IntPtr owner;
        public IntPtr group;
        public IntPtr sacl;
        public IntPtr dacl;
    }
}
