using System.Collections.Generic;

namespace worldWizards.core.entity.common
{
    public class WorldWizardsObject
    {
        private readonly string id;
        private Coordinate coordinate;

        private readonly WorldWizardsType worldWizardType;
        private readonly MetaData metaData;

        private WorldWizardsObject parent;
        private List<WorldWizardsObject> children;

        public string getId() {
            return id;
        }

        public void set(){
        }

        public WorldWizardsObject getOldestParent() {
            return null;
        }

        public void unparent(WorldWizardsObject parent) {
        }

        public void removeChildren(List<WorldWizardsObject> children){
        }

        public void addChildren(List<WorldWizardsObject> children) {
        }


        /// <summary>
        /// Delete this WorldWizardsObject and all of its children
        /// </summary>
        public void delete() {
        }


        public List<WorldWizardsObject> getChildren() {
            return children;
        }

        public WorldWizardsObject getParent() {
            return parent;
        }
    }
}
