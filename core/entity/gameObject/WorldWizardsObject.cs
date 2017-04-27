using UnityEngine;
using System.Collections.Generic;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

namespace worldWizards.core.entity.gameObject
{
    /// <summary>
    /// The WorldWizardsObject is base class for all World Wizards objects.
    /// WorldWizardsObject extends MonoBehavior so has to be attached to a GameObject.
    /// </summary>
    public class WorldWizardsObject : MonoBehaviour
    {
        private readonly string id;
        private readonly WorldWizardsType worldWizardType;
        private readonly MetaData metaData;
        private Coordinate coordinate;

        private WorldWizardsObject parent;
        private List<WorldWizardsObject> children;

        public string GetId() {
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

        public void RemoveChildren(List<WorldWizardsObject> children){
        }

        public void AddChildren(List<WorldWizardsObject> children) {
        }

        /// <summary>
        /// Delete this WorldWizardsObject and all of its descendents
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
