using System;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Metadata required to describe a Tile.
    /// </summary>
    [Serializable]
    public class WWTileMetadata
    {
        public WWDoorHolderMetadata eastWwDoorHolderMetadata;
        public WWDoorHolderMetadata northWwDoorHolderMetadata;
        public WWDoorHolderMetadata southWwDoorHolderMetadata;
        public WWDoorHolderMetadata westWwDoorHolderMetadata;
        public WWWallMetadata wwWallMetadata;
    }
}