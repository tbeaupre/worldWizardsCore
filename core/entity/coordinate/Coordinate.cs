using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.file.entity;

namespace WorldWizards.core.entity.coordinate
{
    public class Coordinate
    {
        public Coordinate(IntVector3 index, Vector3 offset, int rotation)
        {
            this.index = index;
            SetOffset(offset);
            this.rotation = rotation;
        }
        
        public Coordinate(CoordinateJSONBlob b) : this(
            new IntVector3(b.indexX,b.indexY,b.indexZ),
            new Vector3(b.offsetX,b.offsetY,b.offsetZ), b.rotation)
        {
        }

        public Coordinate(IntVector3 index) : this(index, Vector3.zero, 0)
        {
        }

        public Coordinate(IntVector3 index, int rotation) : this(index, Vector3.zero, rotation)
        {
        }


        public Coordinate(int x, int y, int z) : this(new IntVector3(x, y, z))
        {
        }

        public IntVector3 index { get; private set; }
        public Vector3 offset { get; private set; } // normalizedOffset [0,1]

        private void SetOffset(Vector3 offset)
        {
            this.offset = offset.normalized;
        }

        public int rotation { get; private set; } // y rotation
        
    }
}