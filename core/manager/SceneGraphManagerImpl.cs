using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WorldWizards.core.controller.builder;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.gameObject.resource.metaData;
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
                obj.tileFader.Off();

            List<WWObject> objectsAtAndBelow = _sceneDictionary.GetObjectsAtAndBelow(height);
            foreach (WWObject obj in objectsAtAndBelow)
                obj.tileFader.On();
        }

        public void ChangeScale(float scale)
        {
            List<WWObject> allObjects = _sceneDictionary.GetAllObjects();
            foreach (WWObject obj in allObjects)
            {
                obj.transform.position = CoordinateHelper.convertWWCoordinateToUnityCoordinate(obj.GetCoordinate());
                obj.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;
            }
            var gridController = Object.FindObjectOfType<GridController>();
            gridController.MoveGrid();
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

        public bool AddDoor(Door door, Tile holder, Vector3 hitPoint)
        {
            //            Vector3 doorPivot = door.GetPivot();
            //            Vector3 doorFacingDirection = door.GetFacingDirection();
            float doorWidth = door.GetWidth();
            float doorHeight = door.GetHeight();
            List<WWDoorHolderMetaData> doorHolders = holder.GetDoorHolders();
            // TODO, use the DoorHolder that is closest to the hitPoint
            if (doorHolders.Count > 0)
            {
                WWDoorHolderMetaData doorHolder = doorHolders[0];
                float holderWidth = doorHolder.width;
                float holderHeight = doorHolder.height;

                float doorRatio = doorWidth / doorHeight;
                float holderRatio = holderWidth / holderHeight;

                float diff = Math.Abs(holderRatio - doorRatio);
                Debug.Log("diff " + diff);
                if (diff < 0.2f)
                {
                    // TODO scale up to match door to holder if necessary
                    // TODO handle the rotation
                    // TODO handle collision for existing doors

                    var holderRot = holder.GetCoordinate().rotation;
                    var config = new WWDoorHolderConfiguration(holder);
                    
                    var rotatedOffset = RotatePointAroundPivot(doorHolder.pivot,
                        Vector3.zero, 
                        new Vector3(0, holderRot, 0));
                    
                    var coord = new Coordinate(holder.GetCoordinate().index, rotatedOffset, holderRot); //doorHolder.pivot
                    door.SetPosition(coord);
                    _sceneDictionary.Add(door);
                    return true;
                }
            }
            return false;
        }
        
        
        private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
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