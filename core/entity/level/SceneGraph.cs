using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using Object = UnityEngine.Object;

namespace WorldWizards.core.entity.level
{
	/// <summary>
	///     The Scene Graph is the data structure that holds all the World
	///     Wizards Objects in the current level.
	/// </summary>
	public class SceneGraph
    {
        private readonly Dictionary<Guid, WWObject> objects;


        public SceneGraph()
        {
            objects = new Dictionary<Guid, WWObject>();
        }

	    /// <summary>
	    ///     Determine how many WWObjects are in the scene graph
	    /// </summary>
	    /// <returns>The number if WWObjects in the scene graph.</returns>
	    public int SceneSize()
        {
            return objects.Count;
        }

	    /// <summary>
	    ///     Clears all the objects in the scene graph.
	    /// </summary>
	    public void ClearAll()
        {
            var keys = new List<Guid>(objects.Keys);
            foreach (var key in keys) Delete(key);
        }

	    /// <summary>
	    ///     Gets the objects in the SceneGraph in the given coordinate index space.
	    /// </summary>
	    /// <returns>the objects in the SceneGraph in the given coordinate index space.</returns>
	    /// <param name="coordinate">The coordinate to space to get.</param>
	    public List<WWObject> GetObjectsInCoordinateIndex(Coordinate coordinate)
        {
            var result = new List<WWObject>();
            foreach (var wwObject in objects.Values)
                if (coordinate.index.Equals(wwObject.GetCoordinate().index)) result.Add(wwObject);
            return result;
        }

        public void Add(WWObject worldWizardsObject)
        {
            //			List<WWObject> collidingObjects =  GetObjectsInCoordinateIndex (worldWizardsObject.objectData.coordinate);
            //			if (collidingObjects.Count == 0) {
            //				objects.Add (worldWizardsObject.GetId (), worldWizardsObject);
            //			} else {
            //				Destroy (worldWizardsObject);
            //			}
            objects.Add(worldWizardsObject.GetId(), worldWizardsObject);
        }

	    /// <summary>
	    ///     Remove an object from the scene graph.
	    /// </summary>
	    /// <param name="id"></param>
	    /// <returns>returns removed object, which may be null.</returns>
	    private WWObject Remove(Guid id)
        {
            WWObject removedObject;
            objects.TryGetValue(id, out removedObject);
            if (removedObject) objects.Remove(id);
            return removedObject;
        }

        //		public void Delete(Guid id){
        //			// check if id exists in the sceneGraph
        //			if (objects.ContainsKey (id)) {
        //				WWObject objectToDestroy = Remove (id);
        //				Destroy (objectToDestroy);
        //			}
        //		}

        private void Destroy(WWObject objectToDestroy)
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(objectToDestroy.gameObject);
#else
			GameObject.Destroy (objectToDestroy.gameObject);
			#endif
        }


        public void Delete(Guid id)
        {
            if (objects.ContainsKey(id))
            {
                var rootObject = Get(id);

                // remove child from parent if there is a parent
                var parent = rootObject.GetParent();
                if (parent != null)
                {
                    var parentObject = Get(parent.id);
                    parentObject.RemoveChild(rootObject);
                }

                var objectsToDelete = rootObject.GetAllDescendents();
                // include the root
                objectsToDelete.Add(rootObject.objectData);
                foreach (var objectToDelete in objectsToDelete)
                {
                    var objectToDestroy = Remove(objectToDelete.id);
                    Destroy(objectToDestroy);
                }
            }
        }

        public WWObject Get(Guid id)
        {
            WWObject objectToGet;
            objects.TryGetValue(id, out objectToGet);
            if (!objectToGet) Debug.LogError("SceneGraph : Failed to get Object with Guid " + id);
            return objectToGet;
        }


        //        public List<WWObject> GetAllOfType(WWType type)
        //        {
        //            return null;
        //        }
        //
        //        public List<WWObject> GetAllOfMetaData(MetaData metaData)
        //        {
        //            return null;
        //        }
        //
        //        public List<WWObject> GetAdjacentTiles(List<Guid> selection)
        //        {
        //            return null;
        //        }
        //
        //        public List<WWObject> GetPropsContainedInTiles(List<Guid> selection)
        //        {
        //            return null;
        //        }

        //        public WWObject GetRootParent(List<Guid> selection)
        //        {
        //            return null;
        //        }

        //        public List<WWObject> GetAllChildren(List<Guid> selection)
        //        {
        //            return null;
        //        }

        //        // TODO: Does it return itself? or just its siblings?
        //        public List<WWObject> GetAllSiblings(List<Guid> selection)
        //        {
        //            return null;
        //        }

        public void Save()
        {
            var objectstoSave = new List<WWObjectDataMemento>();
            Debug.Log(objects.Count);
            foreach (var entry in objects)
            {
                var memento = new WWObjectDataMemento(entry.Value.objectData);
                objectstoSave.Add(memento);
            }

            var json = JsonConvert.SerializeObject(objectstoSave);
            FileIO.SaveJSONToFile(json, FileIO.testPath);
        }

        public void Load()
        {
            var json = FileIO.LoadJsonFromFile(FileIO.testPath);

            var objectsToRestore = JsonConvert.DeserializeObject<List<WWObjectDataMemento>>(json);
            Debug.Log(string.Format("Loaded {0} objects from file", objectsToRestore.Count));

            foreach (var obj in objectsToRestore)
            {
                var objectData = new WWObjectData(obj);
                var go = WWObjectFactory.Instantiate(objectData);
                Add(go);
            }

            // re-link children since all the objects have been instantiated in game world
            foreach (var obj in objectsToRestore)
            {
                var root = Get(obj.id);
                var childrenToRestore = new List<WWObject>();
                foreach (var childID in obj.children)
                {
                    var childObject = Get(childID);
                    //					if (childObject) { // TODO remove the null check by ensuring no dangling children
                    childrenToRestore.Add(childObject);
                    //					}
                }
                root.AddChildren(childrenToRestore);
            }
        }
    }
}