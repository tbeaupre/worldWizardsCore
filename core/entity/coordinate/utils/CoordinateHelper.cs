using UnityEngine;
using worldWizards.core.entity.common;

namespace worldWizards.core.entity.coordinate.utils
{
    /// <summary>
    /// A utility that converts the Unity Coordinate System to World
    /// Wizards Coordinate System.
    /// </summary>
    public static class CoordinateHelper
    {
        public static float tileLength = 5; // Temporary until we make some kind of constant

        public static Coordinate convertUnityCoordinateToWWCoordinate(Vector3 coordinate){
            return new Coordinate(new IntVector3(coordinate / tileLength));
        }

        public static Vector3 convertWWCoordinateToUnityCoordinate(Coordinate coordinate) {
			float offsetX = coordinate.offset.x * tileLength * 0.5f;
			float offsetY = coordinate.offset.y * tileLength * 0.5f;
			float offsetZ = coordinate.offset.z * tileLength * 0.5f;
			Vector3 offset = new Vector3 (offsetX,offsetY,offsetZ);
			Vector3 c = new Vector3(coordinate.index.x, coordinate.index.y, coordinate.index.z) * tileLength;
			return c + offset;
        }
    }
}
