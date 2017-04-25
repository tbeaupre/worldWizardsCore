using UnityEngine;
using worldWizards.core.entity.common;

namespace worldWizards.core.entity.utils
{
    /// <summary>
    /// A utility that converts the Unity Coordinate System to World
    /// Wizards Coordinate System.
    /// </summary>
    public static class CoordinateHelper
    {
        public static Coordinate convertUnityCoordinateToWWCoordinate(
            Vector3 coordinate, float tileLength){
            // TODO
            return null;
        }

        public static Vector3 convertWWCoordinateToUnityCoordinate(
            Coordinate coordinate, float tileLength) {
            // TODO
            return Vector3.zero;
        }
    }
}
