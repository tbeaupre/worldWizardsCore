using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.file.entity
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// JSON Blob structure for WWObjects
    /// </summary>
    [Serializable]
    public class WWObjectJSONBlob
    {
        public List<Guid> children;
        public WWTransformJSONBlob wwTransform;
        public Guid id;
        public Guid parent;
        public string resourceTag;
        public WWType type;

        /// <summary>
        /// Constructor used by the JSON parser.
        /// </summary>
        [JsonConstructor]
        public WWObjectJSONBlob()
        {
        }

        /// <summary>
        /// Constructor that takes in the state of a WWObjectData and constructs a blob.
        /// </summary>
        /// <param name="wwObjectData"></param>
        public WWObjectJSONBlob(WWObjectData wwObjectData)
        {
            id = wwObjectData.id;
            wwTransform = new WWTransformJSONBlob(wwObjectData.wwTransform);
            resourceTag = wwObjectData.resourceTag;
            if (wwObjectData.parent != null)
            {
                parent = wwObjectData.parent.id;
            }
            children = new List<Guid>();
            foreach (WWObjectData child in wwObjectData.children) children.Add(child.id);
        }
    }
}