using System.Runtime.InteropServices;
using WindowsWrapper.Enums;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWINFO
    {
        public uint cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public WindowStyles dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;

        public WINDOWINFO(bool? _) : this()
        {
            cbSize = (uint)(Marshal.SizeOf(typeof(WINDOWINFO)));
        }

        public override string ToString()
        {
            return $"Styles={dwStyle}\nStatus={dwWindowStatus}";
        }
    }
}
