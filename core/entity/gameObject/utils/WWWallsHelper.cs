using UnityEngine.Assertions;
using WorldWizards.core.entity.gameObject.resource.metaData;

namespace WorldWizards.core.entity.gameObject.utils
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Helper class for getting the Walls of a Resource's metadata after applying rotation
    /// </summary>
    public static class WWWallsHelper
    {
        public static WWWalls GetRotatedWWWalls(WWResourceMetaData metadata, int rotation)
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
                north = metadata.wwTileMetaData.wwWallMetaData.north;
                east = metadata.wwTileMetaData.wwWallMetaData.east;
                south = metadata.wwTileMetaData.wwWallMetaData.south;
                west = metadata.wwTileMetaData.wwWallMetaData.west;
            }
            else if (yRotation == 90)
            {
                north = metadata.wwTileMetaData.wwWallMetaData.west;
                east = metadata.wwTileMetaData.wwWallMetaData.north;
                south = metadata.wwTileMetaData.wwWallMetaData.east;
                west = metadata.wwTileMetaData.wwWallMetaData.south;
            }
            else if (yRotation == 180)
            {
                north = metadata.wwTileMetaData.wwWallMetaData.south;
                east = metadata.wwTileMetaData.wwWallMetaData.west;
                south = metadata.wwTileMetaData.wwWallMetaData.north;
                west = metadata.wwTileMetaData.wwWallMetaData.east;
            }
            else // (yRotation == 270)
            {
                north = metadata.wwTileMetaData.wwWallMetaData.east;
                east = metadata.wwTileMetaData.wwWallMetaData.south;
                south = metadata.wwTileMetaData.wwWallMetaData.west;
                west = metadata.wwTileMetaData.wwWallMetaData.north;
            }

            bool top = metadata.wwTileMetaData.wwWallMetaData.top;
            bool bottom = metadata.wwTileMetaData.wwWallMetaData.bottom;
            var rotatedMetaData = new WWWallMetaData(north, east, south, west, top, bottom);
            WWWalls walls = rotatedMetaData.GetWallsEnum();

            return walls;
        }
    }
}