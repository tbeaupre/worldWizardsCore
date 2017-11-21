using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.level;
using WorldWizards.core.file.entity;
using Object = UnityEngine.Object;

namespace WorldWizards.core.manager
{
    /// <summary>
    ///     The Scene Graph is the data structure that holds all the World
    ///     Wizards Objects in the current level.
    /// </summary>
    public class SceneGraphManagerImpl : SceneGraphManager
    {
        private readonly SceneDictionary _sceneDictionary;

        public SceneGraphManagerImpl()
        {
            _sceneDictionary = new SceneDictionary();
        }

        /// <summary>
        ///     Determine how many WWObjects are in the scene graph
        /// </summary>
        /// <returns>The number if WWObjects in the scene graph.</returns>
        public int SceneSize()
        {
            return _sceneDictionary.GetCount();
        }

        /// <summary>
        ///     Clears all the objects in the scene graph.
        /// </summary>
        public void ClearAll()
        {
            List<Guid> keys = _sceneDictionary.GetAllGuids();
            foreach (Guid key in keys) Delete(key);
        }

        /// <summary>
        ///     Gets the objects in the SceneGraph in the given coordinate index space.
        /// </summary>
        /// <returns>the objects in the SceneGraph in the given coordinate index space.</returns>
        /// <param name="coordinate">The coordinate to space to get.</param>
        public List<WWObject> GetObjectsInCoordinateIndex(Coordinate coordinate)
        {
            return _sceneDictionary.GetObjects(coordinate);
        }

        public bool Add(WWObject worldWizardsObject)
        {
            if (worldWizardsObject != null)
            {
                return _sceneDictionary.Add(worldWizardsObject);
            }
            return false;
        }

        public void HideObjectsAbove(int height)
        {
            List<WWObject> objects = _sceneDictionary.GetObjectsAbove(height);
            foreach (WWObject obj in objects)
            {
                MeshRenderer[] meshRenders = obj.GetComponentsInChildren<MeshRenderer>();
                SkinnedMeshRenderer[] skinnedRenders = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (MeshRenderer mesh in meshRenders)
                    mesh.enabled = false;
                foreach (SkinnedMeshRenderer skin in skinnedRenders)
                    skin.enabled = false;
            }

            List<WWObject> objectsAtAndBelow = _sceneDictionary.GetObjectsAtAndBelow(height);
            foreach (WWObject obj in objectsAtAndBelow)
            {
                MeshRenderer[] meshRenders = obj.GetComponentsInChildren<MeshRenderer>();
                SkinnedMeshRenderer[] skinnedRenders = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (MeshRenderer mesh in meshRenders)
                    mesh.enabled = true;
                foreach (SkinnedMeshRenderer skin in skinnedRenders)
                    skin.enabled = true;
            }
        }


        public WWWalls GetWallsAtCoordinate(Coordinate coordinate)
        {
            List<WWObject> objects = GetObjectsInCoordinateIndex(coordinate);
            WWWalls walls = 0;
            foreach (WWObject obj in objects)
                walls = walls | obj.GetWallsWRotationApplied();
            return walls;
        }

        public void Delete(Guid id)
        {
            if (_sceneDictionary.ContainsGuid(id))
            {
                WWObject rootObject = Get(id);

                // remove child from parent if there is a parent
                WWObjectData parent = rootObject.GetParent();
                if (parent != null)
                {
                    WWObject parentObject = Get(parent.id);
                    parentObject.RemoveChild(rootObject);
                }

                List<WWObjectData> objectsToDelete = rootObject.GetAllDescendents();
                // include the root
                objectsToDelete.Add(rootObject.objectData);
                foreach (WWObjectData objectToDelete in objectsToDelete)
                {
                    WWObject objectToDestroy = Remove(objectToDelete.id);
                    Destroy(objectToDestroy);
                }
            }
        }

        public void Save()
        {
            List<WWObjectJSONBlob> objectsToSave = _sceneDictionary.GetMementoObjects();
            string json = JsonConvert.SerializeObject(objectsToSave);
            FileIO.SaveJSONToFile(json, FileIO.testPath);
        }

        public void Load()
        {
            string json = FileIO.LoadJsonFromFile(FileIO.testPath);

            var objectsToRestore = JsonConvert.DeserializeObject<List<WWObjectJSONBlob>>(json);
            Debug.Log(string.Format("Loaded {0} objects from file", objectsToRestore.Count));

            foreach (WWObjectJSONBlob obj in objectsToRestore)
            {
                var objectData = new WWObjectData(obj);
                WWObject go = WWObjectFactory.Instantiate(objectData);
                Add(go);
            }

            // re-link children since all the objects have been instantiated in game world
            foreach (WWObjectJSONBlob obj in objectsToRestore)
            {
                WWObject root = Get(obj.id);
                var childrenToRestore = new List<WWObject>();
                foreach (Guid childID in obj.children)
                {
                    WWObject childObject = Get(childID);
                    childrenToRestore.Add(childObject);
                }
                root.AddChildren(childrenToRestore);
            }
        }

        /// <summary>
        ///     Remove an object from the scene graph.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns removed object, which may be null.</returns>
        private WWObject Remove(Guid id)
        {
            return _sceneDictionary.Remove(id);
        }

        private void Destroy(WWObject objectToDestroy)
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(objectToDestroy.gameObject);
#else
			GameObject.Destroy (objectToDestroy.gameObject);
			#endif
        }

        public WWObject Get(Guid id)
        {
            return _sceneDictionary.Get(id);
        }
    }
}