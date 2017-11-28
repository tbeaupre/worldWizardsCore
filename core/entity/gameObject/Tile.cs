using System.Collections.Generic;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject.resource.metaData;

namespace WorldWizards.core.entity.gameObject
{
    public class Tile : WWObject
    {   
        public override void SetPosition(Coordinate coordinate)
        {
            var coordinateNoOffset = new Coordinate(coordinate.index, coordinate.rotation);
            base.SetPosition(coordinateNoOffset);
        }

        public List<WWDoorHolderMetaData> GetDoorHolders()
        {
            List<WWDoorHolderMetaData> result = new List<WWDoorHolderMetaData>();
            if (resourceMetaData.wwTileMetaData.northWwDoorHolderMetaData.hasDoorHolder)
            {
                result.Add(resourceMetaData.wwTileMetaData.northWwDoorHolderMetaData);
            }
            if (resourceMetaData.wwTileMetaData.eastWwDoorHolderMetaData.hasDoorHolder)
            {
                result.Add(resourceMetaData.wwTileMetaData.eastWwDoorHolderMetaData);
            }
            if (resourceMetaData.wwTileMetaData.southWwDoorHolderMetaData.hasDoorHolder)
            {
                result.Add(resourceMetaData.wwTileMetaData.southWwDoorHolderMetaData);
            }
            if (resourceMetaData.wwTileMetaData.westWwDoorHolderMetaData.hasDoorHolder)
            {
                result.Add(resourceMetaData.wwTileMetaData.westWwDoorHolderMetaData);
            }
            return result;
        }
        
    }
}