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
            var keys = _sceneDictionary.GetAllGuids();
            foreach (var key in keys) Delete(key);
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
        
        public void HideObjectsAbove(int height)
        {
            var objects = _sceneDictionary.GetObjectsAbove(height);
            foreach (var obj in objects)
            {
                var meshRenders = obj.GetComponentsInChildren<MeshRenderer>();
                var skinnedRenders = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (var mesh in meshRenders)
                {
                    mesh.enabled = false;
                }
                foreach (var skin in skinnedRenders)
                {
                    skin.enabled = false;
                }
            }
            
            var objectsAtAndBelow = _sceneDictionary.GetObjectsAtAndBelow(height);
            foreach (var obj in objectsAtAndBelow)
            {
                var meshRenders = obj.GetComponentsInChildren<MeshRenderer>();
                var skinnedRenders = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (var mesh in meshRenders)
                {
                    mesh.enabled = true;
                }
                foreach (var skin in skinnedRenders)
                {
                    skin.enabled = true;
                }
            }
        }


        public WWWalls GetWallsAtCoordinate(Coordinate coordinate)
        {
            var objects = GetObjectsInCoordinateIndex(coordinate);
            WWWalls walls = 0;
            foreach (var obj in objects)
            {
                walls = walls | obj.GetWallsWRotationApplied();
            }
            return walls;
        }

        public void Delete(Guid id)
        {
            if (_sceneDictionary.ContainsGuid(id))
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
            return _sceneDictionary.Get(id);
        }

        public void Save()
        {
            var objectsToSave = _sceneDictionary.GetMementoObjects();
            var json = JsonConvert.SerializeObject(objectsToSave);
            FileIO.SaveJSONToFile(json, FileIO.testPath);
        }

        public void Load()
        {
            var json = FileIO.LoadJsonFromFile(FileIO.testPath);

            var objectsToRestore = JsonConvert.DeserializeObject<List<WWObjectJSONBlob>>(json);
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
                    childrenToRestore.Add(childObject);
                }
                root.AddChildren(childrenToRestore);
            }
        }
    }
}