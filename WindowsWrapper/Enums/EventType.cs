using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWrapper.Enums
{
    public enum EventType
    {
        Key = 0x0001,
        Mouse = 0x0002,
        Resize = 0x0004,
        Menu = 0x0008,
        Focus = 0x0010
    }
}
