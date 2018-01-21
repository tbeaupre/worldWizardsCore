using System;
using Newtonsoft.Json;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.file.entity
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// JSON blobl structure for coordinates.
    /// </summary>
    [Serializable]
    public class CoordinateJSONBlob
    {
        public int indexX;
        public int indexY;
        public int indexZ;
        public float offsetX;
        public float offsetY;
        public float offsetZ;
        public int rotation;

        /// <summary>
        /// Constructor used by the JSON parser.
        /// </summary>
        [JsonConstructor]
        public CoordinateJSONBlob()
        {
        }

        public CoordinateJSONBlob(Coordinate c)
        {
            indexX = c.Index.x;
            indexY = c.Index.y;
            indexZ = c.Index.z;
            offsetX = c.GetOffset().x;
            offsetY = c.GetOffset().y;
            offsetZ = c.GetOffset().z;
            rotation = c.Rotation;
        }
    }
}