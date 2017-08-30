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
        public static float tileLength = 50; // Temporary until we make some kind of constant

        public static Coordinate convertUnityCoordinateToWWCoordinate(Vector3 coordinate){
            return new Coordinate(new IntVector3(coordinate / tileLength));
        }

        public static Vector3 convertWWCoordinateToUnityCoordinate(Coordinate coordinate) {
            return new Vector3(coordinate.index.x, coordinate.index.y, coordinate.index.z) * tileLength;
        }
    }
}
