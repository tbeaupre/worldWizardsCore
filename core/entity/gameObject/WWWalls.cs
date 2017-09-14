using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace worldWizards.core.entity.gameObject
{
    // Bit flags for the different tile borders.
    public enum WWWalls : byte
    {
        North = 0x1,    // 0000 0001
        East = 0x2,     // 0000 0010
        South = 0x4,    // 0000 0100
        West = 0x8,     // 0000 1000
        Top = 0x10,     // 0001 0000
        Bottom = 0x20   // 0010 0000
    };
}
