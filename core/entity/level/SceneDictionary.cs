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
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// A data structure that efficiently maintains the World Wizard Objects for a Scene.
    /// WWObjects are indexed by both Guid and Coordinate Index, allowing for constant time lookups
    /// for either method of index.
    /// 
    /// Trade off some space for fast look up. Maintain in parallel
    /// a map from Coordinate Index to a list of all Guids at that Coordinate Index.
    /// Then maintain a map from Guid to the WWObject. This allows querying WWObjects by
    /// coordinate index as well as the Guid in constant time.
    /// </summary>
    public class SceneDictionary
    {
        // a map from Coordinate Index to a list of Guids that are at that index.
        private readonly Dictionary<IntVector3, List<Guid>> coordinates;
        // a map from Guid to the actual WWObject itself.
        private readonly Dictionary<Guid, WWObject> objects;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SceneDictionary()
        {
            objects = new Dictionary<Guid, WWObject>();
            coordinates = new Dictionary<IntVector3, List<Guid>>();
        }

        /// <summary>
        /// Get all WWObjects stored in the data structure.
        /// </summary>
        /// <returns>all the objects stored in the data structure.</returns>
        public List<WWObject> GetAllObjects()
        {
            return new List<WWObject>(objects.Values);
        }

        /// <summary>
        /// Get all WWObjects that have a Coordinate Index height greater
        /// than the given height.
        /// </summary>
        /// <param name="height">The height to compare against</param>
        /// <returns></returns>
        public List<WWObject> GetObjectsAbove(int height)
        {
            var result = new List<WWObject>();
            foreach (var indexToGuidList in coordinates)
                if (indexToGuidList.Key.y > height)
                {
                    foreach (Guid g in coordinates[indexToGuidList.Key])
                        result.Add(objects[g]);
                }
            return result;
        }

        /// <summary>
        /// Get all WWObjects that have a Coordinate Index height greater
        /// than the given height.
        /// </summary>
        /// <param name="height">The height to compare against</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all WWObjects in the scene and create a list of Serializable JSON Blobs.
        /// </summary>
        /// <returns>a list of WWObjectJSONBlobs for all objects maintained by the data structure.</returns>
        public List<WWObjectJSONBlob> GetObjectsAsJSONBlobs()
        {
            var objectstoSave = new List<WWObjectJSONBlob>();
            Debug.Log(GetCount());
            foreach (KeyValuePair<Guid, WWObject> guidToObject in objects)
            {
                var blob = new WWObjectJSONBlob(guidToObject.Value.objectData);
                objectstoSave.Add(blob);
            }
            return objectstoSave;
        }

        /// <summary>
        /// Determine whether the data structure contains a WWObject of the given Guid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if the object exists or false if it does not.</returns>
        public bool ContainsGuid(Guid id)
        {
            return objects.ContainsKey(id);
        }

        /// <summary>
        /// Get the WWObject count maintained by the data structure.
        /// </summary>
        /// <returns>The number of WWObjects being maintained.</returns>
        public int GetCount()
        {
            return objects.Count;
        }

        /// <summary>
        /// Consumes a WWObject and determines whether this object can fit at its current coordinate,
        /// taking into consideration rotation, or if it collides with other WWObjects being maintained
        /// by this data structure. This is private method and it safe to assume that the WWObject being tested
        /// for collision has not yet been added to this data structure. Note: it would be easy to do a Guid
        /// comparison before checking collision if the use case ever arises in the future.
        /// </summary>
        /// <param name="wwObject">The object to test for collision.</param>
        /// <returns>True if the WWObject can fit without colliding given its current coordinate.</returns>
        private bool Collides(WWObject wwObject)
        {
            if (coordinates.ContainsKey(wwObject.GetCoordinate().Index)) // any objects at coordinate?
            {
                List<WWObject> objectsAtCoord = GetObjects(wwObject.GetCoordinate());
                WWWalls totalWalls = 0; // this is a bit mask which will keep the running total of wall collisions
                foreach (WWObject obj in objectsAtCoord) // only need to do collision checking at the coordinate index
                if (obj.ResourceMetadata.wwObjectMetadata.type.Equals(WWType.Tile)) // ignore non Tile types
                {
                    // get the walls of the WWObject after applying its rotation transformation
                    WWWalls walls = WWWallsHelper.GetRotatedWWWalls(obj.ResourceMetadata, obj.GetRotation());
                    totalWalls = totalWalls | walls; // OR the walls with the running sum stored in totalWalls
                }
                // now get the walls for the object that is being collision checked for
                WWWalls newWalls = WWWallsHelper.GetRotatedWWWalls(wwObject.ResourceMetadata, wwObject.GetRotation());
                bool doesCollide = Convert.ToBoolean(newWalls & totalWalls); // 0 or False if no collision
                return doesCollide;
            }
            return false; // if there are no objects at the index, obviously there are no collisions
        }
        
        /// <summary>
        /// Consumes a list of WWObjects and determines if any of them collide with existing objects.
        /// </summary>
        /// <param name="wwObjects"> The objects to check for fitting in with existing objects.</param>
        /// <returns>True if there are no collisions detected. False otherwise.</returns>
        public bool DoesNotCollide(List<WWObject> wwObjects)
        {
            foreach (var wwObject in wwObjects)
            {
                if (Collides(wwObject))
                {
                    return false;
                }
            }
            return true; // nothing collides
        }

        /// <summary>
        /// Attempt to Add a WWObject to the data structure.
        /// </summary>
        /// <param name="wwObject">The object to Add.</param>
        /// <returns>True if the object can be Added to the data structure.</returns>
        public bool Add(WWObject wwObject)
        {
            Coordinate coord = wwObject.GetCoordinate();
            Guid guid = wwObject.GetId();

            if (Collides(wwObject) && wwObject.ResourceMetadata.wwObjectMetadata.type.Equals(WWType.Tile))
            {
                Debug.Log("Tile collides with existing tiles. Preventing placement of new tile.");
                return false;
            }
            if (coordinates.ContainsKey(coord.Index))
            {
                coordinates[coord.Index].Add(guid); // append the existing list of Guids for this index
            }
            else // create new entry in the map
            {
                var guidList = new List<Guid>();
                guidList.Add(guid);
                coordinates.Add(coord.Index, guidList);
            }
            objects.Add(wwObject.GetId(), wwObject);
            return true; // sucessfully added the object to the data structure
        }

        /// <summary>
        /// Get the Guids of all WWObjects managed by the data structure.
        /// </summary>
        /// <returns>A list of all Guids managed by the data structure.</returns>
        public List<Guid> GetAllGuids()
        {
            return new List<Guid>(objects.Keys);
        }

        /// <summary>
        /// Attempts to remove a WWObject from the data structure if it exists.
        /// </summary>
        /// <param name="id">The Guid of the object to remove.</param>
        /// <returns>The WwObject that was removed. Null if the object does not exist.</returns>
        public WWObject Remove(Guid id)
        {
            WWObject removedObject;
            objects.TryGetValue(id, out removedObject);
            if (removedObject)
            {
                objects.Remove(id);
                // have to also remove the the id from the coordinate to List<Guid> Dictionary
                foreach (KeyValuePair<IntVector3, List<Guid>> kvp in coordinates)
                if (kvp.Value.Contains(id))
                {
                    kvp.Value.Remove(id);
                }
            }
            return removedObject;
        }

        /// <summary>
        /// Get a WWObject by Guid.
        /// </summary>
        /// <param name="id">The Guid of the WWObject to get.</param>
        /// <returns>The WWObject of the provided Guid.</returns>
        public WWObject Get(Guid id)
        {
            WWObject objectToGet;
            objects.TryGetValue(id, out objectToGet);
            if (!objectToGet)
            {
                Debug.LogError(string.Format(
                    "SceneGraph : Failed to get Object with Guid {0}. This SHOULD NOT happen.", id));
            }
            return objectToGet;
        }

        /// <summary>
        /// Get all WWObjects at the given Coordinate.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns>a list of WWObjects at the given Coordiante.</returns>
        public List<WWObject> GetObjects(Coordinate coord)
        {
            return GetObjects(coord.Index);
        }

        /// <summary>
        /// Get a list of all WWObjects at the given coordinate index.
        /// </summary>
        /// <param name="index">The coordinate index to look at.</param>
        /// <returns>A list of all WWObjects at the given coordiante index.</returns>
        public List<WWObject> GetObjects(IntVector3 index)
        {
            var result = new List<WWObject>();
            if (coordinates.ContainsKey(index))
            {
                var guids = coordinates[index];
                foreach (var guid in guids)
                {
                    result.Add(objects[guid]);
                }
            }
            return result;
        }

        /// <summary>
        /// Get all the colliders maintained by the data structure, filtered by a WWType.
        /// Useful for custom raycasting against a list of WWObjects filtered by type.
        /// </summary>
        /// <param name="wwType">the type to filter by</param>
        /// <returns>Colliders filtered by WWType</returns>
        public List<Collider> GetColliders(WWType wwType)
        {
            var result = new List<Collider>();
            foreach (var wwObject in objects.Values)
            {
                if (wwObject.ResourceMetadata.wwObjectMetadata.type == wwType)
                {
                    var colliders = wwObject.GetComponentsInChildren<Collider>();
                    foreach (var c  in colliders)
                    {
                        result.Add(c);
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// Get all the colliders maintained by the data structure.
        /// Useful for custom raycasting against a list of WWObjects.
        /// </summary>
        /// <returns>All colliders of all the WWObjects being maintained</returns>
        public List<Collider> GetAllColliders()
        {
            var result = new List<Collider>();
            foreach (var wwObject in objects.Values)
            {
                var colliders = wwObject.GetComponentsInChildren<Collider>();
                foreach (var c  in colliders)
                {
                    result.Add(c);
                }
            }
            return result;
        }
    }
}