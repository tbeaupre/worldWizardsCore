using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.file.entity
{
    [Serializable]
    public class WWObjectJSONBlob
    {
        public List<Guid> children;
        public CoordinateJSONBlob coordinate;
        public Guid id;
        public Guid parent;
        public string resourceTag;
        public WWType type;

        [JsonConstructor]
        public WWObjectJSONBlob()
        {
        }

        public WWObjectJSONBlob(WWObjectData state)
        {
            id = state.id;
            ;
            coordinate = new CoordinateJSONBlob(state.coordinate);
            resourceTag = state.resourceTag;
            if (state.parent != null)
            {
                parent = state.parent.id;
            }
            children = new List<Guid>();
            foreach (WWObjectData child in state.children) children.Add(child.id);
        }
    }
}