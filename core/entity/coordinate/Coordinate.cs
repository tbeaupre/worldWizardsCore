using Newtonsoft.Json;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.coordinate
{
    public class Coordinate
    {
        public Coordinate(IntVector3 index, Vector3 offset, int rotation)
        {
            this.index = index;
            this.offset = offset;
            this.rotation = rotation;
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


        [JsonConstructor]
        public Coordinate()
        {
        }

        public IntVector3 index { get; set; }
        public Vector3 offset { get; set; } // normalizedOffset [0,1]
        public int rotation { get; set; } // y rotation
        
    }
}