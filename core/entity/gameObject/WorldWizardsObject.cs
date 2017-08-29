using UnityEngine;
using System;
using System.Collections.Generic;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

namespace worldWizards.core.entity.gameObject
{
    /// <summary>
    /// The WorldWizardsObject is base class for all World Wizards objects.
    /// WorldWizardsObject extends MonoBehavior so it has to be attached to a GameObject.
    /// </summary>
    public abstract class WorldWizardsObject : MonoBehaviour
    {
        private Guid id;
        private WorldWizardsType worldWizardType;
        private MetaData metaData;
        private Coordinate coordinate;

        private WorldWizardsObject parent;
        private List<WorldWizardsObject> children;

        public virtual void Init (Guid id, WorldWizardsType worldWizardType, MetaData metaData, Coordinate coordinate, WorldWizardsObject parent, List<WorldWizardsObject> children) {
            this.id = id;
            this.worldWizardType = worldWizardType;
            this.metaData = metaData;
            this.coordinate = coordinate;
            this.parent = parent;
            this.children = children;
        }

        public Guid GetId() {
            return id;
        }

        public void SetCoordinate(Coordinate coordinate) {
        }

        public WorldWizardsObject GetOldestParent() {
            return null;
        }

        /// <summary>
        /// Promote this World Wizard Object to not have a parent.
        /// </summary>
        public void Unparent() {
        }

        public void RemoveChildren(List<WorldWizardsObject> children) {
        }

        public void AddChildren(List<WorldWizardsObject> children) {
        }

        /// <summary>
        /// Delete this WorldWizardsObject and all of its descendents.
        /// </summary>
        public void Delete() {
        }

        public List<WorldWizardsObject> GetChildren() {
            return children;
        }

        public WorldWizardsObject GetParent() {
            return parent;
        }

        public List<WorldWizardsObject> GetAllDescendents()
        {
            return null;
        }
    }
}
