using UnityEngine;
using worldWizards.core.entity.common;
using Newtonsoft.Json;

namespace worldWizards.core.entity.coordinate
{
    public class Coordinate
    {
        public IntVector3 index { get; set; }
        public Vector3 offset { get; set; } // normalizedOffset [0,1]

        public Coordinate(IntVector3 index, Vector3 offset) {
            this.index = index;
            this.offset = offset;
        }

        public Coordinate(IntVector3 index) : this(index, Vector3.zero) { }

        public Coordinate(int x, int y, int z) : this(new IntVector3(x, y, z)) { }


		[JsonConstructor]
		public Coordinate(){
		}

    }
}
