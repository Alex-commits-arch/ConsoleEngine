using System.Runtime.InteropServices;
using WindowsWrapper.Enums;

namespace WindowsWrapper.Structs
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
    public struct CharInfo
    {
        [FieldOffset(0)]
        public char UnicodeChar;
        [FieldOffset(0)]
        public byte AsciiChar;
        [FieldOffset(2)]
        public CharAttribute Attributes;

        public override string ToString()
        {
            return string.Format("{0}, {1}", UnicodeChar == 0 ? ' ' : UnicodeChar, Attributes);
        }
    }
}
