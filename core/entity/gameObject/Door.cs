using UnityEngine;

namespace WorldWizards.core.entity.gameObject
{
    public class Door : Interactable
    {
        public Vector3 GetPivot()
        {
            return transform.position;
        }

        public Vector3 GetFacingDirection()
        {
            return resourceMetaData.doorMetaData.facingDirection;
        }

        public float GetWidth()
        {
            return resourceMetaData.doorMetaData.width;
        }

        public float GetHeight()
        {
            return resourceMetaData.doorMetaData.height;
        }
    }
}