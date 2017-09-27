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
			this.x = (int) Mathf.Floor(vector.x);
			this.y = (int) Mathf.Floor(vector.y);
			this.z = (int) Mathf.Floor(vector.z);
        }

		public override string ToString(){
			return string.Format ("x : {0}, y : {1}, z : {2}",x,y,z);
		}  

		[JsonConstructor]
		public IntVector3(){
		}
    }
}
