using UnityEngine.Assertions;
using WorldWizards.core.entity.gameObject.resource;

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
                north = metaData.wwTileMetaData.wwOccupiedWalls.north;
                east = metaData.wwTileMetaData.wwOccupiedWalls.east;
                south = metaData.wwTileMetaData.wwOccupiedWalls.south;
                west = metaData.wwTileMetaData.wwOccupiedWalls.west;
            }
            else if (yRotation == 90)
            {
                north = metaData.wwTileMetaData.wwOccupiedWalls.west;
                east = metaData.wwTileMetaData.wwOccupiedWalls.north;
                south = metaData.wwTileMetaData.wwOccupiedWalls.east;
                west = metaData.wwTileMetaData.wwOccupiedWalls.south;
            }
            else if (yRotation == 180)
            {
                north = metaData.wwTileMetaData.wwOccupiedWalls.south;
                east = metaData.wwTileMetaData.wwOccupiedWalls.west;
                south = metaData.wwTileMetaData.wwOccupiedWalls.north;
                west = metaData.wwTileMetaData.wwOccupiedWalls.east;
            }
            else // (yRotation == 270)
            {
                north = metaData.wwTileMetaData.wwOccupiedWalls.east;
                east = metaData.wwTileMetaData.wwOccupiedWalls.south;
                south = metaData.wwTileMetaData.wwOccupiedWalls.west;
                west = metaData.wwTileMetaData.wwOccupiedWalls.north;
            }

            bool top = metaData.wwTileMetaData.wwOccupiedWalls.top;
            bool bottom = metaData.wwTileMetaData.wwOccupiedWalls.bottom;
            var rotatedMetaData = new WWOccupiedWalls(north, east, south, west, top, bottom);
            WWWalls walls = rotatedMetaData.GetWallsEnum();

            return walls;
        }
    }
}