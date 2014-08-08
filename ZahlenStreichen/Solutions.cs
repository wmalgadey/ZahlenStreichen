using System;

namespace ZahlenStreichen
{
    [Flags]
    public enum Solutions
    {
        None = 0x00000,

        EqualTop = 0x00001,
        EqualDown = 0x00002,
        EqualNext = 0x00004,
        EqualPrevious = 0x00008,

        TenTop = 0x00010,
        TenDown = 0x00020,
        TenNext = 0x00040,
        TenPrevious = 0x00080,
    }
}
