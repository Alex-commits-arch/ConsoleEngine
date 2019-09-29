using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWrapper.Constants
{
    public class Colors
    {
        #region Base colors
        public static readonly ushort FOREGROUND_BLUE = 0x0001;
        public static readonly ushort FOREGROUND_GREEN = 0x0002;
        public static readonly ushort FOREGROUND_RED = 0x0004;
        public static readonly ushort FOREGROUND_INTENSITY = 0x0008;
        public static readonly ushort BACKGROUND_BLUE = 0x0010;
        public static readonly ushort BACKGROUND_GREEN = 0x0020;
        public static readonly ushort BACKGROUND_RED = 0x0040;
        public static readonly ushort BACKGROUND_INTENSITY = 0x0080;
        #endregion
        public static readonly ushort FOREGROUND_WHITE = (ushort)(FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
        public static readonly ushort FOREGROUND_CYAN = (ushort)(FOREGROUND_BLUE | FOREGROUND_GREEN);
        public static readonly ushort FOREGROUND_MAGENTA = (ushort)(FOREGROUND_BLUE | FOREGROUND_RED);
        public static readonly ushort FOREGROUND_YELLOW = (ushort)(FOREGROUND_GREEN | FOREGROUND_RED);
        public static readonly ushort BACKGROUND_WHITE = (ushort)(BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE);
        public static readonly ushort BACKGROUND_CYAN = (ushort)(BACKGROUND_BLUE | BACKGROUND_GREEN);
        public static readonly ushort BACKGROUND_MAGENTA = (ushort)(BACKGROUND_BLUE | BACKGROUND_RED);
        public static readonly ushort BACKGROUND_YELLOW = (ushort)(BACKGROUND_GREEN | BACKGROUND_RED);
        #region Composite colors

        #endregion

        public static readonly ushort COMMON_LVB_LEADING_BYTE = 0x0100;
        public static readonly ushort COMMON_LVB_TRAILING_BYTE = 0x0200;
        public static readonly ushort COMMON_LVB_GRID_HORIZONTAL = 0x0400;
        public static readonly ushort COMMON_LVB_GRID_LVERTICAL = 0x0800;
        public static readonly ushort COMMON_LVB_GRID_RVERTICAL = 0x1000;
        public static readonly ushort COMMON_LVB_REVERSE_VIDEO = 0x4000;
        public static readonly ushort COMMON_LVB_UNDERSCORE = 0x8000;
    }
}
