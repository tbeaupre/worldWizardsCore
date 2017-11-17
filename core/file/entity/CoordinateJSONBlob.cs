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
            indexX = c.index.x;
            indexY = c.index.y;
            indexZ = c.index.z;
            offsetX = c.offset.x;
            offsetY = c.offset.y;
            offsetZ = c.offset.z;
            rotation = c.rotation;
        }
    }
}