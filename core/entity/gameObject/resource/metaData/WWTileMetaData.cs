using System;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    [Serializable]
    public class WWTileMetaData
    {
        public WWDoorHolderMetaData eastWwDoorHolderMetaData;
        public WWDoorHolderMetaData northWwDoorHolderMetaData;
        public WWDoorHolderMetaData southWwDoorHolderMetaData;
        public WWDoorHolderMetaData westWwDoorHolderMetaData;
        public WWWallMetaData wwWallMetaData;
    }
}