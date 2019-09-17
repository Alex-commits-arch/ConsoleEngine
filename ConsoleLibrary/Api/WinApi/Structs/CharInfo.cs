using System.Runtime.InteropServices;

namespace ConsoleLibrary.Api.WinApi.Structs
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
    public struct CharInfo
    {
        [FieldOffset(0)]
        public char UnicodeChar;
        [FieldOffset(0)]
        public byte AsciiChar;
        [FieldOffset(2)]
        public ushort Attributes;
    }
}
