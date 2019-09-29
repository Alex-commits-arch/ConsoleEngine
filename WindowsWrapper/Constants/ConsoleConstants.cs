using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWrapper.Constants
{
    public static class ConsoleConstants
    {
        public const short ENABLE_EXTENDED_FLAGS = 0x0080;
        public const short ENABLE_MOUSE_INPUT = 0x0010;
        public const short ENABLE_WINDOW_INPUT = 0x0008;

        public const short STD_INPUT_HANDLE = -10;
        public const short STD_OUTPUT_HANDLE = -11;
    }
}
