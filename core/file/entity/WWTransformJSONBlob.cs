using System;
using Newtonsoft.Json;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.file.entity
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// JSON Blob structure for WWTransforms
    /// </summary>
    [Serializable]
    public class WWTransformJSONBlob
    {
        public CoordinateJSONBlob coordinateJSONBlob;
        public int rotation;
        
        /// <summary>
        /// Constructor used by the JSON parser.
        /// </summary>
        [JsonConstructor]
        public WWTransformJSONBlob()
        {
        }

        public WWTransformJSONBlob(WWTransform wwTransform)
        {
            coordinateJSONBlob = new CoordinateJSONBlob(wwTransform.coordinate);
            rotation = wwTransform.rotation;
        }
    }
}