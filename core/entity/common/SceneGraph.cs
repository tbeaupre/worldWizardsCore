using System.Collections.Generic;

namespace worldWizards.core.entity.common
{
    public class SceneGraph
    {
        Dictionary<string, WorldWizardsObject> objects; // data structure

        public WorldWizardsObject getObject(string id) {
            return null;
        }

        public List<WorldWizardsObject> getAdjacentTiles(List<string> selection) {
            return null;
        }

        public List<WorldWizardsObject> getPropsContainedInTiles(List<string> selection)
        {
            return null;
        }

        /// <summary>
        /// Return a list of groups that are 
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        public List<WorldWizardsObject> getGroup(List<string> selection)
        {
            return null;
        }

    }
}
