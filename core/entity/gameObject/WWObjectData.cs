using System;
using System.Collections.Generic;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

namespace worldWizards.core.entity.gameObject
{
    public class WWObjectData
    {
        public Guid id { get; }
        private WWType type;
        private MetaData metaData;
        public Coordinate coordinate { get; }
        public WWResource resource { get; }

        private WWObject parent;
        private List<WWObject> children;

        public WWObjectData(Guid id, WWType type, MetaData metaData, Coordinate coordinate,
            WWResource resource, WWObject parent, List<WWObject> children)
        {
            this.id = id;
            this.type = type;
            this.metaData = metaData;
            this.coordinate = coordinate;
            this.resource = resource;
            this.parent = parent;
            this.children = children;
        }
    }
}
