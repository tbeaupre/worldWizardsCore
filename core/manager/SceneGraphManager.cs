using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.manager
{
    /// <summary>
    /// </summary>
    public interface SceneGraphManager: Manager
    {
        /// <summary>
        ///     Determine how many WWObjects are in the scene graph
        /// </summary>
        /// <returns>The number if WWObjects in the scene graph.</returns>
        int SceneSize();

        /// <summary>
        /// Attempts to add a WWObject to th Scene Graph
        /// </summary>
        /// <param name="wwObject"> the object to Add.</param>
        /// <returns>True if the object was successfully added  </returns>
        bool Add(WWObject wwObject);

        /// <summary>
        /// Hides the visibility of objects above the input coordinate height index.
        /// </summary>
        /// <param name="height">The height</param>
        void HideObjectsAbove(int height);

        /// <summary>
        /// Returns the collision walls at a the given coordiante index.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        WWWalls GetWallsAtCoordinate(Coordinate coordinate);

        /// <summary>
        ///     Gets the objects in the SceneGraph in the given coordinate index space.
        /// </summary>
        /// <returns>the objects in the SceneGraph in the given coordinate index space.</returns>
        /// <param name="coordinate">The coordinate to space to get.</param>
        List<WWObject> GetObjectsInCoordinateIndex(Coordinate coordinate);

        /// <summary>
        /// Attempts to add a door to the scene graph. 
        /// </summary>
        /// <param name="door">The door to place</param>
        /// <param name="tile">The Tile to look for door holders within</param>
        /// <param name="hitPoint">The hit point will be used to select the nearest doorHolder within the Tile</param>
        /// <returns>True if the door fit the doorHolder, false if the door was unable to be placed.</returns>
        bool AddDoor(Door door, Tile tile, Vector3 hitPoint);

        /// <summary>
        /// Changes the scale of all objects in the SceneGraph
        /// </summary>
        /// <param name="scale"></param>
        void ChangeScale(float scale);

        /// <summary>
        /// Deletes an object
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        void Remove(Guid id);

        void Load();

        void Save();

        /// <summary>
        ///     Clears all the objects in the scene graph.
        /// </summary>
        void ClearAll();
    }
}