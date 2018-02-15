using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WorldWizards.core.controller.builder;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.gameObject.resource.metaData;
using WorldWizards.core.entity.gameObject.utils;
using WorldWizards.core.entity.level;
using WorldWizards.core.file.entity;
using WorldWizards.core.file.utils;
using Object = UnityEngine.Object;

namespace WorldWizards.core.manager
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    ///     The Scene Graph is the data structure that holds all the World
    ///     Wizards Objects in the current level.
    /// </summary>
    public class SceneGraphManagerImpl : SceneGraphManager
    {
        /// <summary>
        /// The data structure used by this implementation.
        /// </summary>
        private readonly SceneDictionary sceneDictionary;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public SceneGraphManagerImpl()
        {
            sceneDictionary = new SceneDictionary();
        }
        
        /// <see cref="SceneGraphManager.SceneSize"/>
        public int SceneSize()
        {
            return sceneDictionary.GetCount();
        }

        /// <see cref="SceneGraphManager.ClearAll"/>
        public void ClearAll()
        {
            List<Guid> keys = sceneDictionary.GetAllGuids();
            foreach (Guid key in keys) Delete(key);
        }

        /// <see cref="SceneGraphManager.GetObjectsInCoordinateIndex"/>
        public List<WWObject> GetObjectsInCoordinateIndex(Coordinate coordinate)
        {
            return sceneDictionary.GetObjects(coordinate);
        }

        /// <see cref="SceneGraphManager.Add"/>
        public bool Add(WWObject worldWizardsObject)
        {
            if (worldWizardsObject != null)
            {
                return sceneDictionary.Add(worldWizardsObject);
            }
            return false;
        }

        /// <see cref="SceneGraphManager.HideObjectsAbove"/>
        public void HideObjectsAbove(int height)
        {
            List<WWObject> objects = sceneDictionary.GetObjectsAbove(height);
            foreach (WWObject obj in objects)
                obj.tileFader.Off();

            List<WWObject> objectsAtAndBelow = sceneDictionary.GetObjectsAtAndBelow(height);
            foreach (WWObject obj in objectsAtAndBelow)
                obj.tileFader.On();
        }

        /// <see cref="SceneGraphManager.ChangeScale"/>
        public void ChangeScale(float scale)
        {
            List<WWObject> allObjects = sceneDictionary.GetAllObjects();
            foreach (WWObject obj in allObjects)
            {

                var newPos = CoordinateHelper.WWCoordToUnityCoord(obj.GetCoordinate());

                obj.transform.position = newPos;
                obj.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;
            }
            var gridController = Object.FindObjectOfType<GridController>();
            gridController.RefreshGrid();
        }

        /// <see cref="SceneGraphManager.GetWallsAtCoordinate"/>
        public WWWalls GetWallsAtCoordinate(Coordinate coordinate)
        {
            List<WWObject> objects = GetObjectsInCoordinateIndex(coordinate);
            WWWalls walls = 0;
            foreach (WWObject obj in objects)
                walls = walls | obj.GetWallsWRotationApplied();
            return walls;
        }

        /// <see cref="SceneGraphManager.Delete"/>
        public void Delete(Guid id)
        {
            Remove(id, true);
        }

        /// <see cref="SceneGraphManager.Remove"/>
        public void Remove(Guid id)
        {
            Remove(id, false);
        }

        private void Remove(Guid id, bool delete)
        {
            if (sceneDictionary.ContainsGuid(id))
            {
                WWObject rootObject = Get(id);

                // remove child from parent if there is a parent
                WWObjectData parent = rootObject.GetParent();
                if (parent != null)
                {
                    WWObject parentObject = Get(parent.id);
                    parentObject.RemoveChild(rootObject);
                }
                
                List<WWObjectData> objectDescendents = rootObject.GetAllDescendents();
                // include the root
                objectDescendents.Add(rootObject.objectData);

                foreach (WWObjectData objectToDelete in objectDescendents)
                {
                    WWObject objectToDestroy = RemoveObject(objectToDelete.id);
                    if (delete)
                    {
                        Destroy(objectToDestroy);
                    }
                }
            }
        }

        /// <see cref="SceneGraphManager.Save"/>
        public void Save(string filePath)
        {
            List<WWObjectJSONBlob> objectsToSave = sceneDictionary.GetObjectsAsJSONBlobs();
            string json = JsonConvert.SerializeObject(objectsToSave);
            FileIO.SaveJsonToFile(json, filePath);
        }

        /// <see cref="SceneGraphManager.Load"/>
        public void Load(string filePath)
        {
            string json = FileIO.LoadJsonFromFile(filePath);

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

        /// <see cref="SceneGraphManager.AddDoor"/>
        public bool AddDoor(Door door, Tile tile, Vector3 hitPoint)
        {
            float doorWidth = door.GetWidth();
            float doorHeight = door.GetHeight();
            List<WWDoorHolderMetadata> doorHolders = tile.GetDoorHolders();
            // TODO, use the DoorHolder that is closest to the hitPoint
            // TODO handle the posibility that a Tile has mutliple Door Holders
            if (doorHolders.Count > 0)
            {
                WWDoorHolderMetadata doorHolder = doorHolders[0];
                float holderWidth = doorHolder.width;
                float holderHeight = doorHolder.height;

                float doorRatio = doorWidth / doorHeight;
                float holderRatio = holderWidth / holderHeight;

                float diff = Math.Abs(holderRatio - doorRatio);
                var threshold = 0.2f;
                if (diff < threshold)
                {
                    // TODO scale up to match door to tile if necessary
                    // TODO handle the rotation
                    // TODO handle collision for existing doors

                    var holderRot = tile.GetCoordinate().Rotation;                    
//                    var config = new WWDoorHolderConfiguration(tile);
                    var rotatedOffset = RotatePointAroundPivot(doorHolder.pivot,
                        Vector3.zero, 
                        new Vector3(0, holderRot, 0));
                    
                    var coord = new Coordinate(tile.GetCoordinate().Index, rotatedOffset, holderRot);
                    door.SetPosition(coord);
                    sceneDictionary.Add(door);
                    return true;
                }
            }
            return false;
        }
        
        private static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }


        public List<Collider> GetAllColliders(WWType wwType)
        {
            return sceneDictionary.GetColliders(wwType);
        }
        
        public List<Collider> GetAllColliders()
        {
            return sceneDictionary.GetAllColliders();
        }

        private WWObject RemoveObject(Guid id)
        {
            return sceneDictionary.Remove(id);
        }

        private void Destroy(WWObject objectToDestroy)
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(objectToDestroy.gameObject);
#else
			GameObject.Destroy (objectToDestroy.gameObject);
			#endif
        }

        private WWObject Get(Guid id)
        {
            return sceneDictionary.Get(id);
        }

        /// <see cref="SceneGraphManager.DoesNotCollide"/>
        public bool DoesNotCollide(List<WWObject> wwObjects)
        {
            return sceneDictionary.DoesNotCollide(wwObjects);
        }
    }
}