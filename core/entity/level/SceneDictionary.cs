using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using UnityEngine;

namespace WorldWizards.core.entity.level
{
    public class SceneDictionary
    {
        private readonly Dictionary<Coordinate, List<Guid>> coordinates;
        private readonly Dictionary<Guid, WWObject> objects;

        public SceneDictionary()
        {
            objects = new Dictionary<Guid, WWObject>();
            coordinates = new Dictionary<Coordinate, List<Guid>>();
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
            var guids = coordinates[coordinate];
            
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

        public void Add(WWObject wwObject)
        {
            var coord = wwObject.GetCoordinate();
            var guid = wwObject.GetId();
            
            Assert.IsNotNull(coord);
            
            if (coordinates.ContainsKey(coord))
            {
                coordinates[coord].Add(guid);
            }
            else
            {
                var guidList = new List<Guid>();
                guidList.Add(guid);
                coordinates.Add(coord, guidList);
            }
            objects.Add(wwObject.GetId(), wwObject);
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
            if (coordinates.ContainsKey(coord))
            {
                var guids = coordinates[coord];
                foreach (var guid in guids)
                {
                    result.Add(objects[guid]);
                }
            }
            return result;
        }

        
    }
}