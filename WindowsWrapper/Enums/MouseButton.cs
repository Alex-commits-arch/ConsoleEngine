using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWrapper.Enums
{
    [Flags]
    public enum MouseButton
    {
        None = 0x0,
        Left = 0x1,
        Right = 0x2,
        Middle = 0x4
    }
}
