using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.file.entity;
using Object = UnityEngine.Object;

namespace WorldWizards.core.entity.level
{
    /// <summary>
    ///     The Scene Graph is the data structure that holds all the World
    ///     Wizards Objects in the current level.
    /// </summary>
    public class SceneGraph
    {
        private readonly SceneDictionary _sceneDictionary;

        public SceneGraph()
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
            return _sceneDictionary.Add(worldWizardsObject);
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


        public Dictionary<IntVector3, WWWalls>  SelectPerimeter(WWObject wwObject)
        {
            Dictionary<IntVector3, WWWalls> wallsToPlace = new Dictionary<IntVector3, WWWalls>();
            List<IntVector3> visited = new List<IntVector3>();
            IntVector3 curIndex = wwObject.GetCoordinate().index;
            SelectPerimeter(wallsToPlace, visited, curIndex);
            return wallsToPlace;
        }

        private void UpdateWallsDict( List<WWObject> objects, IntVector3 curIndex, WWWalls direction, Dictionary<IntVector3,
            WWWalls> wallsToPlace, List<IntVector3> visited, IntVector3 origIndex)
        {
            Debug.Log("UpdateWallsDict called");
            Debug.Log("Objects count " + objects.Count);

            // TODO only consider the floor type tiles in Count
            if (objects.Count == 0)
            {
                Debug.Log("Adding to walls to place");
                if (wallsToPlace.ContainsKey(origIndex))
                {
                    wallsToPlace[origIndex] =  direction | wallsToPlace[origIndex];
                }
                else
                {
                    wallsToPlace.Add(origIndex, direction);
                }
            }
            else // we need to search further for perimeter
            {
                SelectPerimeter(wallsToPlace, visited, curIndex);
            }
        }

        private void SelectPerimeter(Dictionary<IntVector3, WWWalls> wallsToPlace, List<IntVector3> visited, IntVector3 curIndex)
        {
            Debug.Log("SelectPerimeter called.");
            IntVector3 northIndex = new IntVector3(curIndex.x, curIndex.y, curIndex.z + 1);
            IntVector3 eastIndex = new IntVector3(curIndex.x + 1, curIndex.y, curIndex.z);
            IntVector3 southIndex = new IntVector3(curIndex.x, curIndex.y, curIndex.z - 1);
            IntVector3 westIndex = new IntVector3(curIndex.x - 1, curIndex.y, curIndex.z);
            
            visited.Add(curIndex);

            if (!visited.Contains(northIndex))
            {
                List<WWObject> northObjects = _sceneDictionary.GetObjects(northIndex);
                UpdateWallsDict(northObjects, northIndex, WWWalls.North, wallsToPlace, visited, curIndex);
            }
            if (!visited.Contains(eastIndex))
            {
                List<WWObject> eastObjects = _sceneDictionary.GetObjects(eastIndex);
                UpdateWallsDict(eastObjects, eastIndex, WWWalls.East, wallsToPlace, visited, curIndex);
            }
            if (!visited.Contains(southIndex))
            {
                List<WWObject> southObjects = _sceneDictionary.GetObjects(southIndex);
                UpdateWallsDict(southObjects, southIndex, WWWalls.South, wallsToPlace, visited, curIndex);
            }
            if (!visited.Contains(westIndex))
            {
                List<WWObject> westObjectsList = _sceneDictionary.GetObjects(westIndex);
                UpdateWallsDict(westObjectsList, westIndex, WWWalls.West, wallsToPlace, visited, curIndex);
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