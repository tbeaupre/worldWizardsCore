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
        private WWObjectData objectData;

        public virtual void Init (Guid id, WorldWizardsType worldWizardType, MetaData metaData, Coordinate coordinate,
            WWResource resource, WorldWizardsObject parent, List<WorldWizardsObject> children)
        {
            this.objectData = new WWObjectData(id, worldWizardType, metaData, coordinate, resource, parent, children);
        }

        public virtual void Init(WWObjectData objectData)
        {
            this.objectData = objectData;
        }

        public Guid GetId() {
            return objectData.id;
        }

        public Coordinate GetCoordinate() {
            return objectData.coordinate;
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
            return null;// children;
        }

        public WorldWizardsObject GetParent() {
            return null;// parent;
        }

        public List<WorldWizardsObject> GetAllDescendents()
        {
            return null;
        }
    }
}
