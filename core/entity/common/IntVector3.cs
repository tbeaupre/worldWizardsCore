using System;
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

		public override bool Equals(System.Object obj) 
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) 
				return false;
			
			IntVector3 other = (IntVector3)obj;
			return (x == other.x) && (y == other.y) && (z == other.z);
		}
			
		[JsonConstructor]
		public IntVector3(){
		}
    }
}
