using System;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    [Serializable]
    public class WWTileMetaData
    {
        public WWWallMetaData wwWallMetaData;
        public WWDoorHolderMetaData northWwDoorHolderMetaData;
        public WWDoorHolderMetaData southWwDoorHolderMetaData;
        public WWDoorHolderMetaData eastWwDoorHolderMetaData;
        public WWDoorHolderMetaData westWwDoorHolderMetaData;
    }
}