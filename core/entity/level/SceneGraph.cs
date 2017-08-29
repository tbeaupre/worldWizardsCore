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

        public SceneGraph ()
        {
            worldWizardsObjects = new Dictionary<Guid, WorldWizardsObject>();
        }

        public void Add(WorldWizardsObject worldWizardsObject)
        {
            worldWizardsObjects.Add(worldWizardsObject.GetId(), worldWizardsObject);
        }

        /// <summary>
        /// Remove an object from the scene graph.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns removed object, which may be null.</returns>
        public WorldWizardsObject Remove(Guid id)
        {
            WorldWizardsObject removedObject;
            worldWizardsObjects.TryGetValue (id, out removedObject);
            if (removedObject) {
                worldWizardsObjects.Remove(id);
            }
            return removedObject;
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

        // TODO: Does it return itself? or just its siblings?
        public List<WorldWizardsObject> GetAllSiblings(List<Guid> selection)
        {
            return null;
        }
    }
}
