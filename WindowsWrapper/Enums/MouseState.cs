using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWrapper.Enums
{
    [Flags]
    public enum MouseState
    {
        None = 0x0,
        Moved = 0x1,
        DoubleClick = 0x2,
        Scroll = 0x4,
        HorizontalScroll = 0x8
    }
}
