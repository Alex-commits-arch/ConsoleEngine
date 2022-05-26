using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWrapper.Enums
{
    enum FormatMessage : uint
    {
        ALLOCATE_BUFFER = 0x00000100,
        IGNORE_INSERTS = 0x00000200,
        FROM_SYSTEM = 0x00001000,
        ARGUMENT_ARRAY = 0x00002000,
        FROM_HMODULE = 0x00000800,
        FROM_STRING = 0x00000400
    }
}
