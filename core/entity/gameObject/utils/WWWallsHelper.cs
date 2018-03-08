using UnityEngine;
using WorldWizards.core.entity.gameObject.resource.metaData;

namespace WorldWizards.core.entity.gameObject.utils
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Helper class for getting the Walls of a Resource's metadata after applying rotation
    /// </summary>
    public static class WWWallsHelper
    {
        public static WWWalls GetRotatedWWWalls(WWResourceMetadata metadata, int rotation)
        {
            int yRotation = rotation % 360 + (rotation < 0 ? 360 : 0);

            // rotation should only be 1 of 4 discrete values, 0, 90, 180, and 270
            bool isvalidRotation = yRotation == 0 || yRotation == 90 || yRotation == 180 || yRotation == 270 ||
                                   yRotation == 360;
            if (!isvalidRotation)
            {
                Debug.LogError(string.Format("WWWallsHelper : {0} is an invalid rotation.", yRotation));
            }

            bool north;
            bool east;
            bool south;
            bool west;

            if (yRotation == 0 || yRotation == 360)
            {
                north = metadata.wwTileMetadata.wwWallMetadata.north;
                east = metadata.wwTileMetadata.wwWallMetadata.east;
                south = metadata.wwTileMetadata.wwWallMetadata.south;
                west = metadata.wwTileMetadata.wwWallMetadata.west;
            }
            else if (yRotation == 90)
            {
                north = metadata.wwTileMetadata.wwWallMetadata.west;
                east = metadata.wwTileMetadata.wwWallMetadata.north;
                south = metadata.wwTileMetadata.wwWallMetadata.east;
                west = metadata.wwTileMetadata.wwWallMetadata.south;
            }
            else if (yRotation == 180)
            {
                north = metadata.wwTileMetadata.wwWallMetadata.south;
                east = metadata.wwTileMetadata.wwWallMetadata.west;
                south = metadata.wwTileMetadata.wwWallMetadata.north;
                west = metadata.wwTileMetadata.wwWallMetadata.east;
            }
            else // (yRotation == 270)
            {
                north = metadata.wwTileMetadata.wwWallMetadata.east;
                east = metadata.wwTileMetadata.wwWallMetadata.south;
                south = metadata.wwTileMetadata.wwWallMetadata.west;
                west = metadata.wwTileMetadata.wwWallMetadata.north;
            }

            bool top = metadata.wwTileMetadata.wwWallMetadata.top;
            bool bottom = metadata.wwTileMetadata.wwWallMetadata.bottom;
            var rotatedMetaData = new WWWallMetadata(north, east, south, west, top, bottom);
            WWWalls walls = rotatedMetaData.GetWallsEnum();

            return walls;
        }
    }
}