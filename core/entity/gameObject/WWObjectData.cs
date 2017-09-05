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
        public string resourceTag { get; }

        private WWObject parent;
        private List<WWObject> children;

        public WWObjectData(Guid id, WWType type, MetaData metaData, Coordinate coordinate,
            string resourceTag, WWObject parent, List<WWObject> children)
        {
            this.id = id;
            this.type = type;
            this.metaData = metaData;
            this.coordinate = coordinate;
            this.resourceTag = resourceTag;
            this.parent = parent;
            this.children = children;
        }

        // TODO: Find more elegant solution to this.
        public Type GetWWType()
        {
            switch (type)
            {
                case WWType.Interactable:
                    return typeof(Interactable);
                case WWType.Prop:
                    return typeof(Prop);
                case WWType.Tile:
                    return typeof(Tile);
                default:
                    return typeof(Tile);
            }
        }
    }
}
