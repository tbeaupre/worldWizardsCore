using System;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Metadata required to describe a Tile.
    /// </summary>
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