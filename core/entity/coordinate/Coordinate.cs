using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.file.entity;

namespace WorldWizards.core.entity.coordinate
{
    public class Coordinate
    {
        public Coordinate(IntVector3 index, Vector3 offset, int rotation)
        {
            this.Index = index;
            SetOffset(offset);
            this.Rotation = rotation;
        }

        public Coordinate(CoordinateJSONBlob b) : this(
            new IntVector3(b.indexX, b.indexY, b.indexZ),
            new Vector3(b.offsetX, b.offsetY, b.offsetZ), b.rotation)
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

        public IntVector3 Index { get; private set; }
        public Vector3 Offset { get; set; } // (-1,1)

        public int Rotation { get; set; } // y rotation

        private void SetOffset(Vector3 offset)
        {
            offset.x = Mathf.Clamp(offset.x, -1, 1f);
            offset.y = Mathf.Clamp(offset.y, -1, 1f);
            offset.z = Mathf.Clamp(offset.z, -1, 1f);
            this.Offset = offset;
        }

        public void SnapToGrid()
        {
            Offset = Vector3.zero;
        }
        
        public override string ToString()
        {
            return string.Format("Index x : {0}, y : {1}, z : {2} Offset x : {3}, y : {4}, z : {5} Rotation : {6}",
                Index.x, Index.y, Index.z, Offset.x, Offset.y, Offset.z, Rotation);
        }
    }
}