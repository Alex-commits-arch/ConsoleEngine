using System;

namespace WindowsWrapper.Enums
{
    [Flags]
    public enum CharAttribute : ushort
    {
        ForegroundBlack = 0x0000,
        ForegroundDarkBlue = 0x0001,
        ForegroundDarkGreen = 0x0002,
        ForegroundDarkCyan = 0x0003,
        ForegroundDarkRed = 0x0004,
        ForegroundDarkMagenta = 0x0005,
        ForegroundDarkYellow = 0x0006,
        ForegroundGrey = 0x0007,
        ForegroundDarkGrey = 0x0008,
        ForegroundBlue = 0x0009,
        ForegroundGreen = 0x000A,
        ForegroundCyan = 0x000B,
        ForegroundRed = 0x000C,
        ForegroundMagenta = 0x000D,
        ForegroundYellow = 0x000E,
        ForegroundWhite = 0x000F,
        BackgroundBlack = 0x0000,
        BackgroundDarkBlue = 0x0010,
        BackgroundDarkGreen = 0x0020,
        BackgroundDarkCyan = 0x0030,
        BackgroundDarkRed = 0x0040,
        BackgroundDarkMagenta = 0x0050,
        BackgroundDARK_Yellow = 0x0060,
        BackgroundGrey = 0x0070,
        BackgroundDarkGrey = 0x0080,
        BackgroundBlue = 0x0090,
        BackgroundGreen = 0x00A0,
        BackgroundCyan = 0x00B0,
        BackgroundRed = 0x00C0,
        BackgroundMagenta = 0x00D0,
        BackgroundYellow = 0x00E0,
        BackgroundWhite = 0x00F0,
        LeadingByte = 0x0100,
        TrailingByte = 0x0200,
        TopHorizontal = 0x0400,
        LeftVertical = 0x0800,
        RightVertical = 0x1000,
        Reverse = 0x4000,
        Underscore = 0x8000
    };
}
