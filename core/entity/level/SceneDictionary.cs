using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.gameObject.utils;
using WorldWizards.core.file.entity;

namespace WorldWizards.core.entity.level
{
    public class SceneDictionary
    {
        private readonly Dictionary<IntVector3, List<Guid>> coordinates;
        private readonly Dictionary<Guid, WWObject> objects;

        public SceneDictionary()
        {
            objects = new Dictionary<Guid, WWObject>();
            coordinates = new Dictionary<IntVector3, List<Guid>>();
        }

        public List<WWObject> GetAllObjects()
        {
            return new List<WWObject>(objects.Values);
        }

        public List<WWObject> GetObjectsAbove(int height)
        {
            var result = new List<WWObject>();
            foreach (KeyValuePair<IntVector3, List<Guid>> kvp in coordinates)
                if (kvp.Key.y > height)
                {
                    foreach (Guid g in coordinates[kvp.Key])
                        result.Add(objects[g]);
                }
            return result;
        }

        public List<WWObject> GetObjectsAtAndBelow(int height)
        {
            var result = new List<WWObject>();
            foreach (KeyValuePair<IntVector3, List<Guid>> kvp in coordinates)
                if (kvp.Key.y <= height)
                {
                    foreach (Guid g in coordinates[kvp.Key])
                        result.Add(objects[g]);
                }
            return result;
        }


        public List<WWObjectJSONBlob> GetMementoObjects()
        {
            var objectstoSave = new List<WWObjectJSONBlob>();
            Debug.Log(GetCount());
            foreach (KeyValuePair<Guid, WWObject> entry in objects)
            {
                var blob = new WWObjectJSONBlob(entry.Value.objectData);
                objectstoSave.Add(blob);
            }
            return objectstoSave;
        }

        public bool ContainsGuid(Guid id)
        {
            return objects.ContainsKey(id);
        }

        public int GetCount()
        {
            return objects.Count;
        }

        private bool Collides(WWObject wwObject)
        {
            if (coordinates.ContainsKey(wwObject.GetCoordinate().index))
            {
                List<WWObject> objectsAtCoord = GetObjects(wwObject.GetCoordinate());
                WWWalls existingWalls = 0;
                foreach (WWObject obj in objectsAtCoord)
                    if (obj.resourceMetaData.wwObjectMetaData.type.Equals(WWType.Tile))
                    {
                        WWWalls walls =
                            WWWallsHelper.GetRotatedWWWalls(obj.resourceMetaData, obj.GetCoordinate().rotation);
                        existingWalls = existingWalls | walls;
                    }
                WWWalls newWalls =
                    WWWallsHelper.GetRotatedWWWalls(wwObject.resourceMetaData, wwObject.GetCoordinate().rotation);
                bool doesCollide = Convert.ToBoolean(newWalls & existingWalls); // should be 0 or False if no collision
                return doesCollide;
            }
            return false;
        }

        public bool Add(WWObject wwObject)
        {
            Coordinate coord = wwObject.GetCoordinate();
            Guid guid = wwObject.GetId();

            if (Collides(wwObject) && wwObject.resourceMetaData.wwObjectMetaData.type.Equals(WWType.Tile))
            {
                Debug.Log("Tile collides with existing tiles. Preventing placement of new tile.");
                return false;
            }
            if (coordinates.ContainsKey(coord.index))
            {
                Debug.Log("Updating Guid list.");
                coordinates[coord.index].Add(guid);
            }
            else
            {
                Debug.Log("Creating new Guid list.");
                var guidList = new List<Guid>();
                guidList.Add(guid);
                coordinates.Add(coord.index, guidList);
            }
            objects.Add(wwObject.GetId(), wwObject);
            return true;
        }


        public List<Guid> GetAllGuids()
        {
            return new List<Guid>(objects.Keys);
        }

        public WWObject Remove(Guid id)
        {
            WWObject removedObject;
            objects.TryGetValue(id, out removedObject);
            if (removedObject)
            {
                objects.Remove(id);
                // now we have to remove the the id from the coordinate to List<Guid> Dictionary

                foreach (KeyValuePair<IntVector3, List<Guid>> kvp in coordinates)
                    if (kvp.Value.Contains(id))
                    {
                        kvp.Value.Remove(id);
                    }
            }
            return removedObject;
        }

        public WWObject Get(Guid id)
        {
            WWObject objectToGet;
            objects.TryGetValue(id, out objectToGet);
            if (!objectToGet)
            {
                Debug.LogError("SceneGraph : Failed to get Object with Guid " + id);
            }
            return objectToGet;
        }

        public List<WWObject> GetObjects(Coordinate coord)
        {
            return GetObjects(coord.index);
        }

        public List<WWObject> GetObjects(IntVector3 index)
        {
            var result = new List<WWObject>();
            if (coordinates.ContainsKey(index))
            {
                List<Guid> guids = coordinates[index];
                foreach (Guid guid in guids)
                    result.Add(objects[guid]);
            }
            return result;
        }

        public List<WWObject> GetPossibleTiles()
        {
            return null;
        }

        public List<WWObject> RankObjectsToPlace()
        {
            return null;
        }
    }
}