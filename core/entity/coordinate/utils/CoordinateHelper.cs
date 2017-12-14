using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.coordinate.utils
{
    /// <summary>
    ///     A utility that converts the Unity Coordinate System to World
    ///     Wizards Coordinate System.
    /// </summary>
    public static class CoordinateHelper
    {
        public static float baseTileLength = 10f; // the base size of what a tile should be
        public static float tileLengthScale = 1f; // how much to scale up the base size

        /// <summary>
        ///     Gets the tile scale. Takes into account original scale of tiles.
        /// </summary>
        /// <returns>The tile scale.</returns>
        public static float GetTileScale()
        {
            return baseTileLength * tileLengthScale;
        }

        public static Vector3 GetTileCenter(Vector3 position)
        {
            Coordinate coord = UnityCoordToWWCoord(position, 0);
            coord.SnapToGrid();
            return WWCoordToUnityCoord(coord);
        }

        public static Coordinate UnityCoordToWWCoord(Vector3 position, int rotation)
        {
            Vector3 scaled = position / GetTileScale();
            Vector3 fraction = scaled - new Vector3(Mathf.Floor(scaled.x), Mathf.Floor(scaled.y), Mathf.Floor(scaled.z));

            // Convert the coordinate origin to be in the center of tile. Origin is in the middle of the Tile Cube.
            fraction -= new Vector3(0.5f, 0.5f, 0.5f); // offset to center
            fraction *= 2f; // make range (-1, 1)

            var offset = new Vector3(fraction.x, fraction.y, fraction.z);
            return new Coordinate(new IntVector3(scaled), offset, rotation);
        }

        public static Vector3 WWCoordToUnityCoord(Coordinate coordinate)
        {
            // Move origin to bottom left corner.
            float offsetX = coordinate.Offset.x / 2 + 0.5f;
            float offsetY = coordinate.Offset.y / 2 + 0.5f;
            float offsetZ = coordinate.Offset.z / 2 + 0.5f;
            var offset = new Vector3(offsetX, offsetY, offsetZ);
            Vector3 index = new Vector3(coordinate.Index.x, coordinate.Index.y, coordinate.Index.z);
            return (index + offset) * GetTileScale();
        }
    }
}