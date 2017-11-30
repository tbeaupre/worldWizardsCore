using UnityEngine.Assertions;
using WorldWizards.core.entity.gameObject.resource.metaData;

namespace WorldWizards.core.entity.gameObject.utils
{
    public static class WWWallsHelper
    {
        public static WWWalls GetRotatedWWWalls(WWResourceMetaData metaData, int rotation)
        {
            int yRotation = rotation % 360 + (rotation < 0 ? 360 : 0);

            // rotation should only be 1 of 4 discrete values, 0, 90, 180, and 270
            Assert.IsTrue(yRotation == 0 || yRotation == 90 || yRotation == 180 || yRotation == 270 ||
                          yRotation == 360);

            bool north;
            bool east;
            bool south;
            bool west;

            if (yRotation == 0 || yRotation == 360)
            {
                north = metaData.wwTileMetaData.wwWallMetaData.north;
                east = metaData.wwTileMetaData.wwWallMetaData.east;
                south = metaData.wwTileMetaData.wwWallMetaData.south;
                west = metaData.wwTileMetaData.wwWallMetaData.west;
            }
            else if (yRotation == 90)
            {
                north = metaData.wwTileMetaData.wwWallMetaData.west;
                east = metaData.wwTileMetaData.wwWallMetaData.north;
                south = metaData.wwTileMetaData.wwWallMetaData.east;
                west = metaData.wwTileMetaData.wwWallMetaData.south;
            }
            else if (yRotation == 180)
            {
                north = metaData.wwTileMetaData.wwWallMetaData.south;
                east = metaData.wwTileMetaData.wwWallMetaData.west;
                south = metaData.wwTileMetaData.wwWallMetaData.north;
                west = metaData.wwTileMetaData.wwWallMetaData.east;
            }
            else // (yRotation == 270)
            {
                north = metaData.wwTileMetaData.wwWallMetaData.east;
                east = metaData.wwTileMetaData.wwWallMetaData.south;
                south = metaData.wwTileMetaData.wwWallMetaData.west;
                west = metaData.wwTileMetaData.wwWallMetaData.north;
            }

            bool top = metaData.wwTileMetaData.wwWallMetaData.top;
            bool bottom = metaData.wwTileMetaData.wwWallMetaData.bottom;
            var rotatedMetaData = new WWWallMetaData(north, east, south, west, top, bottom);
            WWWalls walls = rotatedMetaData.GetWallsEnum();

            return walls;
        }
    }
}