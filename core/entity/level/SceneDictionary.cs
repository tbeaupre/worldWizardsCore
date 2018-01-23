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
    /// </summary>
    public class SceneDictionary
    {
        private readonly Dictionary<IntVector3, List<Guid>> coordinates;
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
        /// <returns></returns>
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
            foreach (KeyValuePair<IntVector3, List<Guid>> kvp in coordinates)
                if (kvp.Key.y > height)
                {
                    foreach (Guid g in coordinates[kvp.Key])
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
            foreach (KeyValuePair<Guid, WWObject> entry in objects)
            {
                var blob = new WWObjectJSONBlob(entry.Value.objectData);
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
        /// by this data structure. This is private method and it safe to assume that the  WWObject being tested
        /// for collision has not yet been added to this data structure.
        /// </summary>
        /// <param name="wwObject">The object to test for collision.</param>
        /// <returns>True if the WWObject can fit without colliding given its current coordinate.</returns>
        private bool Collides(WWObject wwObject)
        {
            if (coordinates.ContainsKey(wwObject.GetCoordinate().Index))
            {
                List<WWObject> objectsAtCoord = GetObjects(wwObject.GetCoordinate());
                WWWalls existingWalls = 0;
                foreach (WWObject obj in objectsAtCoord)
                    if (obj.ResourceMetadata.wwObjectMetadata.type.Equals(WWType.Tile))
                    {
                        WWWalls walls =
                            WWWallsHelper.GetRotatedWWWalls(obj.ResourceMetadata, obj.GetCoordinate().Rotation);
                        existingWalls = existingWalls | walls;
                    }
                WWWalls newWalls =
                    WWWallsHelper.GetRotatedWWWalls(wwObject.ResourceMetadata, wwObject.GetCoordinate().Rotation);
                bool doesCollide = Convert.ToBoolean(newWalls & existingWalls); // should be 0 or False if no collision
                return doesCollide;
            }
            return false;
        }

        /// <summary>
        /// Attempt to Add a WWObject to the data structure.
        /// </summary>
        /// <param name="wwObject">The object to Add.</param>
        /// <returns>True if the object can be Added to the data strucutre.</returns>
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
                coordinates[coord.Index].Add(guid);
            }
            else
            {
                var guidList = new List<Guid>();
                guidList.Add(guid);
                coordinates.Add(coord.Index, guidList);
            }
            objects.Add(wwObject.GetId(), wwObject);
            return true;
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
                // now we have to remove the the id from the coordinate to List<Guid> Dictionary

                foreach (KeyValuePair<IntVector3, List<Guid>> kvp in coordinates)
                    if (kvp.Value.Contains(id))
                    {
                        kvp.Value.Remove(id);
                    }
            }
            return removedObject;
        }

        /// <summary>
        /// Get a WWObject.
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
                List<Guid> guids = coordinates[index];
                foreach (Guid guid in guids)
                    result.Add(objects[guid]);
            }
            return result;
        }

        public List<WWObject> GetPossibleTiles()
        {
            // not implemented yet
            return null;
        }

        public List<WWObject> RankObjectsToPlace()
        {
            // not implemented yet
            return null;
        }
    }
}