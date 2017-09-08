using UnityEngine;
using Newtonsoft.Json;

namespace worldWizards.core.entity.common
{
    public class IntVector3
    {
		public int x { get; set; }
		public int y { get; set; }
		public int z { get; set; }

        public IntVector3(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public IntVector3(Vector3 vector) {
            this.x = (int)vector.x;
            this.y = (int)vector.y;
            this.z = (int)vector.z;
        }


		[JsonConstructor]
		public IntVector3(){
		}
    }
}
