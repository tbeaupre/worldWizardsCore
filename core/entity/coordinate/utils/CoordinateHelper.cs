using UnityEngine;

namespace WorldWizards.core.entity.coordinate.utils
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// A utility that converts between the Unity Coordinate System and World
    /// Wizards Coordinate System.
    /// </summary>
    public static class CoordinateHelper
    {
        // TODO baseTileLength can be replaced as all WWResourceMetadatas will have their own scale.
        public static float baseTileLength = 10f; // the base size of what a tile should be
        public static float tileLengthScale = 1f; // how much to scale up the base size

        /// <summary>
        /// Gets the tile scale. Takes into account original scale of tiles (baseTileLength) and a multiplier (tileLengthScale).
        /// </summary>
        /// <returns>The tile scale.</returns>
        public static float GetTileScale()
        {
            return baseTileLength * tileLengthScale;
        }

//        public static Vector3 GetTileCenter(Vector3 position)
//        {
//            Coordinate coord = UnityCoordToWWCoord(position, 0);
//            coord.SnapToGrid();
//            return WWCoordToUnityCoord(coord);
//        }

        /// <see cref="UnityCoordToWWCoord(Vector3, int)"/>
        public static Coordinate UnityCoordToWWCoord(Vector3 position)
        {
            return UnityCoordToWWCoord(position, 0);
        }



        public static Vector3 GetOffset(Coordinate coordinate)
        {
            float offsetX = coordinate.GetOffset().x / 2 + 0.5f;
            float offsetY = coordinate.GetOffset().y / 2 + 0.5f;
            float offsetZ = coordinate.GetOffset().z / 2 + 0.5f;
            return new Vector3(offsetX,offsetY, offsetZ);
        }

        /// <summary>
        /// Convert a Unity Space position to a WWCoordinate.
        /// </summary>
        /// <param name="position">The Unity position.</param>
        /// <param name="rotation">The desired rotation for the resulting coordinate.</param>
        /// <returns></returns>
        public static Coordinate UnityCoordToWWCoord(Vector3 position, int rotation)
        {
            Vector3 scaled = position / GetTileScale();
            // the fraction is between [0,1] as it is the numbers after the decimal place of the scaled position
            Vector3 fraction = scaled - new Vector3(Mathf.Floor(scaled.x), Mathf.Floor(scaled.y), Mathf.Floor(scaled.z));

            // Convert the coordinate origin to be in the center of tile. Origin is in the middle of the Tile Cube.
            fraction -= new Vector3(0.5f, 0.5f, 0.5f); // offset to center
            fraction *= 2f; // make range (-1, 1)

            var offset = new Vector3(fraction.x, fraction.y, fraction.z);
            return new Coordinate(new IntVector3(scaled), offset, rotation);
        }

        /// <summary>
        /// Convert a WWCoordinate to a Unity Space position.
        /// </summary>
        /// <param name="coordinate">The coordinate to convert.</param>
        /// <returns>The coordinate as a Unity Space position.</returns>
        public static Vector3 WWCoordToUnityCoord(Coordinate coordinate)
        {
            // Move origin to bottom left corner.
            float offsetX = coordinate.GetOffset().x / 2 + 0.5f;
            float offsetY = coordinate.GetOffset().y / 2 + 0.5f;
            float offsetZ = coordinate.GetOffset().z / 2 + 0.5f;

            var offset = GetOffset(coordinate);
            Vector3 index = new Vector3(coordinate.Index.x, coordinate.Index.y, coordinate.Index.z);
            return (index + offset) * GetTileScale();
        }
    }
}