using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.manager
{
    /// <summary>
    /// The Scene Graph is responsible for maintaining all of the World Wizard Objects in a given scene.
    /// </summary>
    public interface SceneGraphManager: Manager
    {
        /// <summary>
        /// Determine how many WWObjects are in the scene graph
        /// </summary>
        /// <returns>The number if WWObjects in the scene graph.</returns>
        int SceneSize();

        /// <summary>
        /// Attempts to add a WWObject to th Scene Graph
        /// </summary>
        /// <param name="wwObject"> the object to add.</param>
        /// <returns>True if the object was successfully added, false if the object failed to be added.</returns>
        bool Add(WWObject wwObject);

        /// <summary>
        /// Hides the visibility of objects above the input coordinate height index.
        /// </summary>
        /// <param name="height">The height at which to hide objects above.</param>
        void HideObjectsAbove(int height);

        /// <summary>
        /// Returns the collision walls at a the given coordinate index.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        WWWalls GetWallsAtCoordinate(Coordinate coordinate);

        /// <summary>
        ///  Gets the objects in the Scene Graph at the given coordinate index space.
        /// </summary>
        /// <returns>the objects in the Scene Graph in the given coordinate index space.</returns>
        /// <param name="coordinate">The coordinate to space to get.</param>
        List<WWObject> GetObjectsInCoordinateIndex(Coordinate coordinate);

        /// <summary>
        /// Attempts to add a door to the Scene Graph. 
        /// </summary>
        /// <param name="door">The Door to place.</param>
        /// <param name="tile">The Tile to look for Door Holders within</param>
        /// <param name="hitPoint">The hit point will be used to select the nearest door Holder within the Tile.</param>
        /// <returns>True if the door fit the doorHolder, false if the door was unable to be placed.</returns>
        bool AddDoor(Door door, Tile tile, Vector3 hitPoint);

        /// <summary>
        /// Changes the scale of all objects in the SceneGraph.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        void ChangeScale(float scale);

        /// <summary>
        /// Removes an object from the Scene Graph and then deletes it from the Unity Scene.
        /// </summary>
        /// <param name="id">The id of the object to delete.</param>
        void Delete(Guid id);

        /// <summary>
        /// Removes an object from the Scene Graph. The object is note deleted from the Unity Scene.
        /// </summary>
        /// <param name="id">The id of the object to remove.</param>
        void Remove(Guid id);

        /// <summary>
        /// Loads the Scene Graph from a file.
        /// </summary>
        /// <param name="filePath">The file path of where to load the Scene Graph from.</param>
        void Load(string filePath);

        /// <summary>
        /// Saves the Scene Graph to file.
        /// </summary>
        /// <param name="filePath">The file path of where to save the Scene Graph.</param>
        void Save(string filePath);

        /// <summary>
        /// Clears all the objects in the scene graph.
        /// </summary>
        void ClearAll();
    }
}