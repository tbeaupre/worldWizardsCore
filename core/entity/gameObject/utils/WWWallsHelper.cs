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
                north = metaData.wwCollisions.north;
                east = metaData.wwCollisions.east;
                south = metaData.wwCollisions.south;
                west = metaData.wwCollisions.west;
            }
            else if (yRotation == 90)
            {
                north = metaData.wwCollisions.west;
                east = metaData.wwCollisions.north;
                south = metaData.wwCollisions.east;
                west = metaData.wwCollisions.south;
            }
            else if (yRotation == 180)
            {
                north = metaData.wwCollisions.south;
                east = metaData.wwCollisions.west;
                south = metaData.wwCollisions.north;
                west = metaData.wwCollisions.east;
            }
            else // (yRotation == 270)
            {
                north = metaData.wwCollisions.east;
                east = metaData.wwCollisions.south;
                south = metaData.wwCollisions.west;
                west = metaData.wwCollisions.north;
            }

            bool top = metaData.wwCollisions.top;
            bool bottom = metaData.wwCollisions.bottom;
            var rotatedMetaData = new WWCollisions(north, east, south, west, top, bottom);
            WWWalls walls = rotatedMetaData.GetWallsEnum();

            return walls;
        }
    }
}