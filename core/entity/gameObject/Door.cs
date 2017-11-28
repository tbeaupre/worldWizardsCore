using UnityEngine;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.entity.gameObject
{
    public class Door : Interactable
    {
       
        
//        public virtual void Init(WWObjectData objectData, WWResourceMetaData resourceMetaData)
//        {
//            base.Init(objectData, resourceMetaData);
//
//        }
        
        public override void SetPosition(Coordinate coordinate)
        {
            base.SetPosition(coordinate);
        }

        public Vector3 GetPivot()
        {
            return this.transform.position;
        }

        public Vector3 GetFacingDirection()
        {
            return this.resourceMetaData.doorMetaData.facingDirection;
        }
        
        public float GetWidth()
        {
            return this.resourceMetaData.doorMetaData.width;
        }
        
        public float GetHeight()
        {
            return this.resourceMetaData.doorMetaData.height;
        }
    }
}