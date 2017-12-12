using System;
using Newtonsoft.Json;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.file.entity
{
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

        [JsonConstructor]
        public CoordinateJSONBlob()
        {
        }

        public CoordinateJSONBlob(Coordinate c)
        {
            indexX = c.Index.x;
            indexY = c.Index.y;
            indexZ = c.Index.z;
            offsetX = c.Offset.x;
            offsetY = c.Offset.y;
            offsetZ = c.Offset.z;
            rotation = c.Rotation;
        }
    }
}