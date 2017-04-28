using System.Collections.Generic;
using System;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;

namespace worldWizards.core.entity.level
{
    /// <summary>
    /// The Scene Graph is the data structure that holds all the World
    /// Wizards Objects in the current level.
    /// </summary>
    public class SceneGraph
    {
		private Dictionary<Guid, WorldWizardsObject> worldWizardsObjects;

        private void Add(WorldWizardsObject worldWizardsObject)
        {

        }

        private WorldWizardsObject Remove(Guid id)
        {
            return null;
        }

        public WorldWizardsObject Get(string id)
        {
            return null;
        }

        public List<WorldWizardsObject> GetAllOfType(WorldWizardsType type)
        {
            return null;
        }

        public List<WorldWizardsObject> GetAllOfMetaData(MetaData metaData)
        {
            return null;
        }

        public List<WorldWizardsObject> GetAdjacentTiles(List<Guid> selection)
        {
            return null;
        }

        public List<WorldWizardsObject> GetPropsContainedInTiles(List<Guid> selection)
        {
            return null;
        }

        public WorldWizardsObject GetRootParent(List<Guid> selection)
        {
            return null;
        }

        public List<WorldWizardsObject> GetAllChildren(List<Guid> selection)
        {
            return null;
        }

        public List<WorldWizardsObject> GetAllSiblings(List<Guid> selection)
        {
            return null;
        }
    }
}
