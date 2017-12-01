namespace WorldWizards.core.entity.gameObject.resource.metaData
{
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