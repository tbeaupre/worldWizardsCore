namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Metadata necessary for describing a Configuration of Door Holders.
    /// A World Wizard Tile can have up to four doors, one for each wall.
    /// </summary>
    public class WWDoorHolderConfiguration
    {
        public WWDoorHolderMetaData northWWDoorHolderMetaData;
        public WWDoorHolderMetaData eastWWDoorHolderMetaData;
        public WWDoorHolderMetaData southWWDoorHolderMetaData;
        public WWDoorHolderMetaData westWWDoorHolderMetaData;

        public WWDoorHolderConfiguration(Tile tile) : this(
            tile.resourceMetaData.wwTileMetaData.northWwDoorHolderMetaData,
            tile.resourceMetaData.wwTileMetaData.eastWwDoorHolderMetaData,
            tile.resourceMetaData.wwTileMetaData.southWwDoorHolderMetaData,
            tile.resourceMetaData.wwTileMetaData.westWwDoorHolderMetaData)
        {
        }

        public WWDoorHolderConfiguration(
            WWDoorHolderMetaData northWWDoorHolderMetaData,
            WWDoorHolderMetaData eastWWDoorHolderMetaData,
            WWDoorHolderMetaData southWWDoorHolderMetaData,
            WWDoorHolderMetaData westWWDoorHolderMetaData)
        {
            this.northWWDoorHolderMetaData = northWWDoorHolderMetaData;
            this.eastWWDoorHolderMetaData = eastWWDoorHolderMetaData;
            this.southWWDoorHolderMetaData = southWWDoorHolderMetaData;
            this.westWWDoorHolderMetaData = westWWDoorHolderMetaData;
        }
    }
}