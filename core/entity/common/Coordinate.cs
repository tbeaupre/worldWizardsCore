using UnityEngine;
using worldWizards.core.entity.utils;

namespace worldWizards.core.entity.common
{
    public class Coordinate
    {
        IntVector3 index { get; set; }
        Vector3 offset { get; set; } // normalizedOffset [0,1]

        public Coordinate(IntVector3 index, Vector3 offset) {
            this.index = index;
            this.offset = offset;
        }
    }
}
