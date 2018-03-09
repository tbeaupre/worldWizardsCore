using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.file.entity;

namespace WorldWizards.core.entity.coordinate
{
    //@author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// A Coordinate is the unit used by World Wizards to describe a WWObject's position.
    /// A coordinate is independent of scale. A coordinate has an a discrete index
    /// and an offset describing a smaller fraction of space within the Coordinate's index space.
    /// </summary>
    public class Coordinate
    {
        // The Index is the Coordinate's Index in 3D space.
        public IntVector3 Index { get; private set; }
        // The Offset is a 3D position within the Coordinate's Index space
        private Vector3 Offset;// all values between (-1,1) not inclusive
        
        public Coordinate(IntVector3 index, Vector3 offset)
        {
            Index = index;
            SetOffset(offset);
        }

        public Coordinate(CoordinateJSONBlob b) : this(
            new IntVector3(b.indexX, b.indexY, b.indexZ),
            new Vector3(b.offsetX, b.offsetY, b.offsetZ))
        {
        }
        /// <summary>
        /// Constructor that only takes in an Index, and defaults Offset to 0,0,0
        /// </summary>
        /// <param name="index"></param>
        public Coordinate(IntVector3 index) : this(index, Vector3.zero)
        {
        }

        public Coordinate(int x, int y, int z) : this(new IntVector3(x, y, z))
        {
        }

        /// <summary>
        /// Returns the Offset, which is clamped between (-1,1) not inclusive
        /// </summary>
        /// <returns></returns>
        public Vector3 GetOffset()
        {
            return Offset;
        }

        /// <summary>
        /// Sets the Offset for this Coordinate. The Offset is a 3D position within the Coordinates Index Space.
        /// The x,y,z values of the Offset are clamped between (-1, 1) and are not inclusive.
        /// A value of (0,0,0) means their is no Offset. Offset is from the 3D center of the coordinate space,
        /// so inside at the center of a cube. This method can also mutate Index.
        /// <param name="offset"> the offset to set. Clamp is applied to be safe.</param>
        public void SetOffset(Vector3 offset)
        {
            offset.x = Mathf.Clamp(offset.x, -1f, 1f);
            offset.y = Mathf.Clamp(offset.y, -1f, 1f);
            offset.z = Mathf.Clamp(offset.z, -1f, 1f);
            
            // NOTE:
            // In order to guarantee that Coordinates can be converted to and from Unity Space to World Wizard Space
            // any number of times and always result in the same conversion, it is necassary to define the behavior
            // for when any axis of the Offset is 1 or -1. If the Offset is this value, there could be multiple possible
            // Coordinates. For example, consider a Coordinate with Index (1,1,1) and offset (0,-1,0). This Coordinate
            // could also have the same Unity Space as a Coordinate with Index (1,0,1) and offset (0,0,0).
            // To avoid this, the convention will be to always increment or decrement the Index if an Offset is -1 or 1
            // and zero out the Offset.
            
            if (offset.x == 1f)
            {
                offset.x = 0;
                Index.x += 1;
            }
            if (offset.x == -1f)
            {
                offset.x = 0;
                Index.x -= 1;
            }
            if (offset.y == 1f)
            {
                offset.y = 0;
                Index.y += 1;
            }
            if (offset.y == -1f)
            {
                offset.y = 0;
                Index.y -= 1;
            }
            if (offset.z == 1f)
            {
                offset.z = 0;
                Index.z += 1;
            }
            if (offset.z == -1f)
            {
                offset.z = 0;
                Index.z -= 1;
            }
            
            Offset = offset;
        }

        public void SnapToGrid()
        {
            Offset = Vector3.zero;
        }
        
        /// <summary>
        /// Convert the Coordinate's values to a readable string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Index x : {0}, y : {1}, z : {2} Offset x : {3}, y : {4}, z : {5}",
                Index.x, Index.y, Index.z, Offset.x, Offset.y, Offset.z);
        }
    }
}