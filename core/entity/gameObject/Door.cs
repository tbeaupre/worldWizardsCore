using UnityEngine;

namespace WorldWizards.core.entity.gameObject
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    public class Door : Interactable
    {
        public Vector3 GetPivot()
        {
            return transform.position;
        }

        public Vector3 GetFacingDirection()
        {
            return ResourceMetadata.doorMetadata.facingDirection;
        }

        public float GetWidth()
        {
            return ResourceMetadata.doorMetadata.width;
        }

        public float GetHeight()
        {
            return ResourceMetadata.doorMetadata.height;
        }
    }
}