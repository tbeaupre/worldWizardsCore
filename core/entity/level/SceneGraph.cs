using System.Collections.Generic;
using System;
using System.Collections;

using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using worldWizards.core.entity.utils;
using worldWizards.core.controller.level.utils;

using Newtonsoft.Json;
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


		/// <summary>
		/// Clears all the objects in the scene graph.
		/// </summary>
		public void ClearAll(){
			var keys = new List<Guid>(objects.Keys);
			foreach (var key in keys) {
				Delete (key);
			}
		}
			
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
        private WWObject Remove(Guid id)
        {
            WWObject removedObject;
            objects.TryGetValue (id, out removedObject);
            if (removedObject) {
                objects.Remove(id);
            }
            return removedObject;
        }

		public void Delete(Guid id){
			WWObject objectToDestroy = Remove (id);
			GameObject.Destroy (objectToDestroy.gameObject);
		}


		public void DeleteDescending(Guid id){
			WWObject rootObject = Get (id);
			List<WWObjectData> objectsToDelete = rootObject.GetAllDescendents();
			// include the root
			objectsToDelete.Add(rootObject.objectData);
			foreach (WWObjectData objectToDelete in objectsToDelete) {
				WWObject objectToDestroy = Remove (objectToDelete.id);
					// TODO remove this null check by ensure no dangling children
					if (objectToDestroy){
						GameObject.Destroy (objectToDestroy.gameObject);
					}
			}
		}

		public WWObject Get(Guid id)
        {
			WWObject objectToGet;
			objects.TryGetValue (id, out objectToGet);
			if (!objectToGet) {
				Debug.LogError ("SceneGraph : Failed to get Object with Guid " + id.ToString());
			}
			return objectToGet;
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

//        public List<WWObject> GetAllChildren(List<Guid> selection)
//        {
//            return null;
//        }

        // TODO: Does it return itself? or just its siblings?
        public List<WWObject> GetAllSiblings(List<Guid> selection)
        {
            return null;
        }

		public void Save(){
			List<WWObjectDataMemento> objectstoSave = new List<WWObjectDataMemento> ();
			Debug.Log (objects.Count);
			foreach(KeyValuePair<Guid, WWObject> entry in objects)
			{
				WWObjectDataMemento memento = new WWObjectDataMemento (entry.Value.objectData);
				objectstoSave.Add (memento);
			}

			string json = JsonConvert.SerializeObject (objectstoSave);
			FileIO.SaveJSONToFile (json, FileIO.testPath);
		}
			
		public void Load(){
			string json = FileIO.LoadJsonFromFile (FileIO.testPath);

			List<WWObjectDataMemento> objectsToRestore = JsonConvert.DeserializeObject<List<WWObjectDataMemento>> (json);
			Debug.Log (String.Format ("Loaded {0} objects from file", objectsToRestore.Count));

			foreach (WWObjectDataMemento obj in objectsToRestore) {
				WWObjectData objectData = new WWObjectData (obj);
				WWObject go = WWObjectFactory.Instantiate(objectData);
                Add(go);
			}

			// re-link children since all the objects have been instantiated in game world
			foreach (WWObjectDataMemento obj in objectsToRestore) {
				WWObject root = Get (obj.id);
				List<WWObject> childrenToRestore = new List<WWObject> ();
				foreach (Guid childID in obj.children) {
					WWObject childObject = Get (childID);
					if (childObject) { // TODO remove the null check by ensuring no dangling children
						childrenToRestore.Add (childObject);
					}
				}
				root.AddChildren (childrenToRestore);
			}

		}
			
    }

}
