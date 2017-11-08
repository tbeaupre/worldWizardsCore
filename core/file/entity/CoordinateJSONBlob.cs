using System;
using Newtonsoft.Json;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.file.entity
{
    [Serializable]
    public class CoordinateJSONBlob
    {
        [JsonConstructor]
        public CoordinateJSONBlob()
        {
        } 
        public int indexX;
        public int indexY;
        public int indexZ;
        public float offsetX;
        public float offsetY;
        public float offsetZ;
        public int rotation;
        
        public CoordinateJSONBlob(Coordinate c)
        {
            this.indexX = c.index.x;
            this.indexY = c.index.y;
            this.indexZ = c.index.z;
            this.offsetX = c.offset.x;
            this.offsetY = c.offset.y;
            this.offsetZ = c.offset.z;
            this.rotation = c.rotation;
        }  
    }
}