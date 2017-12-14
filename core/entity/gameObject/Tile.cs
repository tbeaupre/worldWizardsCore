using System.Collections.Generic;
 using UnityEngine;
 using WorldWizards.core.entity.coordinate;
 using WorldWizards.core.entity.coordinate.utils;
 using WorldWizards.core.entity.gameObject.resource.metaData;

namespace WorldWizards.core.entity.gameObject
{
    public class Tile : WWObject
    {
        public override void SetPosition(Vector3 position, bool snapToGrid)
        {
            if (snapToGrid)
            {
                position = CoordinateHelper.GetTileCenter(position);
            }
            base.SetPosition(position, snapToGrid);
        }

        protected override Vector3 GetPositionOffset()
        {
            return new Vector3(
                0,
                0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                0);
        }

        public List<WWDoorHolderMetaData> GetDoorHolders()
        {
            var result = new List<WWDoorHolderMetaData>();
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