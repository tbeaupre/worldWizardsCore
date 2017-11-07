using UnityEngine;
using UnityEngine.Assertions;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject.resource;

namespace WorldWizards.core.entity.gameObject.utils
{
    public class WWWallsHelper
    {
        public static WWWalls getRotatedWWWalls(WWResourceMetaData metaData, Coordinate coordinate)
        {
            var yRotation = coordinate.rotation % 360 + (coordinate.rotation < 0 ? 360 : 0);

            // rotation should only be 1 of 4 discrete values, 0, 90, 180, and 270
//            Debug.Log(yRotation);
            Assert.IsTrue(yRotation == 0 || yRotation == 90 || yRotation == 180 || yRotation == 270 ||
                          yRotation == 360);

            bool north;
            bool east;
            bool south;
            bool west;

            if (yRotation == 0 || yRotation == 360)
            {
                north = metaData.north;
                east = metaData.east;
                south = metaData.south;
                west = metaData.west;
            }
            else if (yRotation == 90)
            {
                north = metaData.west;
                east = metaData.north;
                south = metaData.east;
                west = metaData.south;
            }
            else if (yRotation == 180)
            {
                north = metaData.south;
                east = metaData.west;
                south = metaData.north;
                west = metaData.east;
            }
            else // (yRotation == 270)
            {
                north = metaData.east;
                east = metaData.south;
                south = metaData.west;
                west = metaData.north;
            }

            var top = metaData.top;
            var bottom = metaData.bottom;

            var rotatedMetaData = new WWResourceMetaData(north, east, south, west, top, bottom, metaData.type);

            var walls = rotatedMetaData.GetWallsEnum();

            return walls;
        }
    }
}