using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.gameObject.resource;
using WorldWizards.core.entity.gameObject.utils;

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

        public List<WWObjectDataMemento> GetMementoObjects()
        {
            var objectstoSave = new List<WWObjectDataMemento>();
            Debug.Log(GetCount());
            foreach (var entry in objects)
            {
                var memento = new WWObjectDataMemento(entry.Value.objectData);
                objectstoSave.Add(memento);
            }
            return objectstoSave;
        }
        
        
        public List<WWObject> GetObjectsInCoordinateIndex(Coordinate coordinate)
        {
            var guids = coordinates[coordinate.index];
            
            var result = new List<WWObject>();

            foreach (var guid in guids)
            {
                result.Add(objects[guid]);
            }
            return result;            
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
                var objectsAtCoord = Get(wwObject.GetCoordinate());
                WWWalls existingWalls = 0;
                foreach (var obj in objectsAtCoord)
                {
                    if (obj.resourceMetaData.type.Equals(WWType.Tile))
                    {
                        var walls = WWWallsHelper.getRotatedWWWalls(obj.resourceMetaData, obj.GetCoordinate());
                        existingWalls = existingWalls | walls;
                    }
                }
                var newWalls = WWWallsHelper.getRotatedWWWalls(wwObject.resourceMetaData, wwObject.GetCoordinate());
                var doesCollide = Convert.ToBoolean(newWalls & existingWalls); // should be 0 or False if no collision
                return doesCollide;
            }
            return false;
        }

        public bool Add(WWObject wwObject)
        {
            var coord = wwObject.GetCoordinate();
            var guid = wwObject.GetId();

            if (Collides(wwObject) && wwObject.resourceMetaData.type.Equals(WWType.Tile))
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
                
                foreach (var kvp in coordinates)
                {
                    if (kvp.Value.Contains(id))
                    {
                        kvp.Value.Remove(id);
                    }
                }
            }
            return removedObject;
        }
        
        public WWObject Get(Guid id)
        {
            WWObject objectToGet;
            objects.TryGetValue(id, out objectToGet);
            if (!objectToGet) Debug.LogError("SceneGraph : Failed to get Object with Guid " + id);
            return objectToGet;
        }

        public List<WWObject> Get(Coordinate coord)
        {
            var result = new List<WWObject>();
            if (coordinates.ContainsKey(coord.index))
            {
                var guids = coordinates[coord.index];
                foreach (var guid in guids)
                {
                    result.Add(objects[guid]);
                }
            }
            return result;
        }

        
    }
}