using UnityEngine;

namespace WorldWizards.core.entity.common
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// A Vector3 data type with integers instead of floating pointer numbers.
    /// </summary>
    public class IntVector3
    {
        public IntVector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public IntVector3(Vector3 vector)
        {
            x = (int) Mathf.Floor(vector.x);
            y = (int) Mathf.Floor(vector.y);
            z = (int) Mathf.Floor(vector.z);
        }

        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public override string ToString()
        {
            return string.Format("x : {0}, y : {1}, z : {2}", x, y, z);
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (IntVector3) obj;
            return x == other.x && y == other.y && z == other.z;
        }

        /// <summary>
        /// from https://stackoverflow.com/questions/2634690/good-hash-function-for-a-2d-index
        /// Generates a unique hash code even when the sum of x y and z is the same.'
        /// Neccassary in order to use IntVector3 as a key for a Dictionary.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((199 + x.GetHashCode()) * 199 + y.GetHashCode()) * 199 + z.GetHashCode();
        }
    }
}