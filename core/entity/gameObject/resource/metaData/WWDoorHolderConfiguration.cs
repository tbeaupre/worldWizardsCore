namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Metadata necessary for describing a Configuration of Door Holders.
    /// A World Wizard Tile can have up to four doors, one for each wall.
    /// </summary>
    public class WWDoorHolderConfiguration
    {
        public WWDoorHolderMetadata northWwDoorHolderMetadata;
        public WWDoorHolderMetadata eastWwDoorHolderMetadata;
        public WWDoorHolderMetadata southWwDoorHolderMetadata;
        public WWDoorHolderMetadata westWwDoorHolderMetadata;

        public WWDoorHolderConfiguration(Tile tile) : this(
            tile.ResourceMetadata.wwTileMetadata.northWwDoorHolderMetadata,
            tile.ResourceMetadata.wwTileMetadata.eastWwDoorHolderMetadata,
            tile.ResourceMetadata.wwTileMetadata.southWwDoorHolderMetadata,
            tile.ResourceMetadata.wwTileMetadata.westWwDoorHolderMetadata)
        {
        }

        public WWDoorHolderConfiguration(
            WWDoorHolderMetadata northWwDoorHolderMetadata,
            WWDoorHolderMetadata eastWwDoorHolderMetadata,
            WWDoorHolderMetadata southWwDoorHolderMetadata,
            WWDoorHolderMetadata westWwDoorHolderMetadata)
        {
            this.northWwDoorHolderMetadata = northWwDoorHolderMetadata;
            this.eastWwDoorHolderMetadata = eastWwDoorHolderMetadata;
            this.southWwDoorHolderMetadata = southWwDoorHolderMetadata;
            this.westWwDoorHolderMetadata = westWwDoorHolderMetadata;
        }
    }
}