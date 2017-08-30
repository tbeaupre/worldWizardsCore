using UnityEngine;

namespace worldWizards.core.entity.common
{
    public class IntVector3
    {
        public int x { get; }
        public int y { get; }
        public int z { get; }

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
    }
}
