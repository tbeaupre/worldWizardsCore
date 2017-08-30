using System;
using System.Collections.Generic;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

namespace worldWizards.core.entity.gameObject
{
    public class WWObjectData
    {
        public Guid id { get; }
        private WorldWizardsType worldWizardType;
        private MetaData metaData;
        public Coordinate coordinate { get; }
        public WWResource resource { get; }

        private WorldWizardsObject parent;
        private List<WorldWizardsObject> children;

        public WWObjectData(Guid id, WorldWizardsType worldWizardType, MetaData metaData, Coordinate coordinate,
            WWResource resource, WorldWizardsObject parent, List<WorldWizardsObject> children)
        {
            this.id = id;
            this.worldWizardType = worldWizardType;
            this.metaData = metaData;
            this.coordinate = coordinate;
            this.resource = resource;
            this.parent = parent;
            this.children = children;
        }
    }
}
