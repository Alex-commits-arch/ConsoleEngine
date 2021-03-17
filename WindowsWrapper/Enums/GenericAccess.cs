using System;

namespace WindowsWrapper.Enums
{
    [Flags]
    public enum GenericAccess : uint
    {
        GENERIC_ALL = 0x10000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_WRITE = 0x40000000,
        GENERIC_READ = 0x80000000,
    }
}
