using System;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    [Serializable]
    public class WWTileMetaData
    {
        public WWWallMetaData wwWallMetaData;
        public WWDoorHolder northWwDoorHolder;
        public WWDoorHolder southWwDoorHolder;
        public WWDoorHolder eastWwDoorHolder;
        public WWDoorHolder westWwDoorHolder;
    }
}