using UnityEngine;
using worldWizards.core.entity.common;

namespace worldWizards.core.entity.coordinate
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
