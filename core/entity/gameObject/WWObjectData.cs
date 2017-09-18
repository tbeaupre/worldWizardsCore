using System;
using System.Collections.Generic;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

namespace worldWizards.core.entity.gameObject
{
    public class WWObjectData
    {
        public Guid id { get; }
        private MetaData metaData;
        public Coordinate coordinate { get; }

        private WWObject parent;
        private List<WWObject> children;

        public string resourceTag { get; }

        public WWObjectData(Guid id, MetaData metaData, Coordinate coordinate,
            WWObject parent, List<WWObject> children, string resourceTag)
        {
            this.id = id;
            this.metaData = metaData;
            this.coordinate = coordinate;
            this.parent = parent;
            this.children = children;
            this.resourceTag = resourceTag;
        }
    }
}
