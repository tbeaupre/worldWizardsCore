using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject.resource.metaData;

namespace WorldWizards.core.entity.gameObject
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    public class Tile : WWObject
    {
//        public override void SetPosition(Vector3 position)
//        {
//            if (snapToGrid)
//            {
//                position = CoordinateHelper.GetTileCenter(position);
//            }
//            base.SetPosition(position, snapToGrid);
//        }

//        protected override Vector3 GetPositionOffset()
//        {
////            return new Vector3(
////                0,
////                0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
////                0);
//
//            ///return CoordinateHelper.GetOffset(this.GetCoordinate());
//            return Vector3.zero;
//        }

        public List<WWDoorHolderMetadata> GetDoorHolders()
        {
            var result = new List<WWDoorHolderMetadata>();
            if (ResourceMetadata.wwTileMetadata.northWwDoorHolderMetadata.hasDoorHolder)
            {
                result.Add(ResourceMetadata.wwTileMetadata.northWwDoorHolderMetadata);
            }
            if (ResourceMetadata.wwTileMetadata.eastWwDoorHolderMetadata.hasDoorHolder)
            {
                result.Add(ResourceMetadata.wwTileMetadata.eastWwDoorHolderMetadata);
            }
            if (ResourceMetadata.wwTileMetadata.southWwDoorHolderMetadata.hasDoorHolder)
            {
                result.Add(ResourceMetadata.wwTileMetadata.southWwDoorHolderMetadata);
            }
            if (ResourceMetadata.wwTileMetadata.westWwDoorHolderMetadata.hasDoorHolder)
            {
                result.Add(ResourceMetadata.wwTileMetadata.westWwDoorHolderMetadata);
            }
            return result;
        }
    }
}