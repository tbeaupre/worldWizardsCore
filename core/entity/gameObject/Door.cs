using WorldWizards.core.controller.builder;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject.resource.metaData;

namespace WorldWizards.core.entity.gameObject
{
    public class Door : Interactable
    {
        public virtual void Init(WWObjectData objectData, WWResourceMetaData resourceMetaData)
        {
            base.Init(objectData, resourceMetaData);
        }
        
        public override void SetPosition(Coordinate coordinate)
        {
            base.SetPosition(coordinate);
        }
    }
}