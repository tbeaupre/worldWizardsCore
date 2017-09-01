using System.Collections.Generic;
using System;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using UnityEngine;

namespace worldWizards.core.entity.level
{
    /// <summary>
    /// The Scene Graph is the data structure that holds all the World
    /// Wizards Objects in the current level.
    /// </summary>
    public class SceneGraph
    {
		private Dictionary<Guid, WWObject> objects;

        public SceneGraph ()
        {
            objects = new Dictionary<Guid, WWObject>();
        }

        public void Add(WWObject worldWizardsObject)
        {
            objects.Add(worldWizardsObject.GetId(), worldWizardsObject);
        }

        /// <summary>
        /// Remove an object from the scene graph.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns removed object, which may be null.</returns>
        public WWObject Remove(Guid id)
        {
            WWObject removedObject;
            objects.TryGetValue (id, out removedObject);
            if (removedObject) {
                objects.Remove(id);
            }
            return removedObject;
        }

        public WWObject Get(string id)
        {
            return null;
        }

        public List<WWObject> GetAllOfType(WWType type)
        {
            return null;
        }

        public List<WWObject> GetAllOfMetaData(MetaData metaData)
        {
            return null;
        }

        public List<WWObject> GetAdjacentTiles(List<Guid> selection)
        {
            return null;
        }

        public List<WWObject> GetPropsContainedInTiles(List<Guid> selection)
        {
            return null;
        }

        public WWObject GetRootParent(List<Guid> selection)
        {
            return null;
        }

        public List<WWObject> GetAllChildren(List<Guid> selection)
        {
            return null;
        }

        // TODO: Does it return itself? or just its siblings?
        public List<WWObject> GetAllSiblings(List<Guid> selection)
        {
            return null;
        }
    }
}
