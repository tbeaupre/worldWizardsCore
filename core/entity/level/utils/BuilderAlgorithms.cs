using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.controller.resources;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.gameObject.resource;
using WorldWizards.core.entity.gameObject.resource.metaData;
using WorldWizards.core.entity.gameObject.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.entity.level.utils
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// A collection of helpful algorithms that speed up building levels in World Wizards.
    /// </summary>
    public static class BuilderAlgorithms
    {
        /// <summary>
        /// Building a perimeter wall around a large floor or dungeon map is time consuming, especially
        /// when wall pieces have to constantly be rotated into place due to World Wizard's
        /// slightly elaborate tile system.
        /// Build Perimeter Walls assists the player by searching for the perimeter of all contiguous
        /// tiles. Once the perimeter has been found, the walls are erected and added to the
        /// current Scene Graph.
        /// </summary>
        /// <param name="resourceTag">the resource to place around the perimeter</param>
        /// <param name="wwObject">the floor tile to start the search from</param>
        public static void BuildPerimeterWalls(string resourceTag, WWObject wwObject)
        {
            IntVector3 curIndex = wwObject.GetCoordinate().Index;
            Dictionary<IntVector3, WWWalls> coordToWalls = SelectPerimeter(curIndex);
            foreach (KeyValuePair<IntVector3, WWWalls> coordToWall in coordToWalls)
            {
                // Note: WWWalls is a bitmask, an there may be multiple perimeter walls
                // for a given coordinate index. Hence why these are if statements and not if elses.
                // if the perimeter wall is on the north side...
                if (Convert.ToBoolean(WWWalls.North & coordToWall.Value))
                {
                    TryToPlaceWall(coordToWall.Key, WWWalls.North, resourceTag);
                }
                // if the perimeter wall is on the east side...
                if (Convert.ToBoolean(WWWalls.East & coordToWall.Value))
                {
                    TryToPlaceWall(coordToWall.Key, WWWalls.East, resourceTag);
                }
                // if the perimeter wall is on the south side...
                if (Convert.ToBoolean(WWWalls.South & coordToWall.Value))
                {
                    TryToPlaceWall(coordToWall.Key, WWWalls.South, resourceTag);
                }
                // if the perimeter wall is on the west side...
                if (Convert.ToBoolean(WWWalls.West & coordToWall.Value))
                {
                    TryToPlaceWall(coordToWall.Key, WWWalls.West, resourceTag);
                }
            }
        }

        /// <summary>
        /// Loads from the resourceTag a WWObject, then attempts to find a valid rotation for the WWObject,
        /// such that it fits with existing Objects at the specified Coordinate Index, and finally places and
        /// adds the WWObject wall to the active Scene Graph.
        /// </summary>
        /// <param name="coordIndex">The coordinate index for this perimeter wall</param>
        /// <param name="perimeterWallOpening">The direction for the perimeter wall</param>
        /// <param name="resourceTag">The resource tag for of the WWObject to place as the perimeter wall.</param>
        private static void TryToPlaceWall(IntVector3 coordIndex, WWWalls perimeterWallOpening, string resourceTag)
        {
            // the collisions that need to be taken into account to see if the WWObject belonging to resourceTag can fit
            WWWalls wallsToFit = ~ perimeterWallOpening |
                                 ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().
                                     GetWallsAtCoordinate(new Coordinate(coordIndex));
            WWResource resource = WWResourceController.GetResource(resourceTag);
            WWResourceMetadata resourceMetadata = resource.GetMetaData();
            // Try possible rotations for the resource, and place the resource at the first rotation that fits.
            for (var r = 0; r < 360; r += 90) // only rotations of 90 degrees are valid
            {
                WWWalls wallToPlace = WWWallsHelper.GetRotatedWWWalls(resourceMetadata, r);
                bool doesCollide = Convert.ToBoolean(wallToPlace & wallsToFit); // should be 0 or False if no collision
                if (!doesCollide)
                {
                    PlaceWallObject(coordIndex, r, resourceTag); // add the wall to the active Scene Graph
                    return; // a valid rotation was found for the WWObject, no need to try further rotations
                }
            }
        }

        /// <summary>
        /// Instantiates the WwObject wall belonging to the resource and adds the wall to the active
        /// Scene Graph.
        /// </summary>
        /// <param name="coordIndex">The coordinate index for this perimeter wall.</param>
        /// <param name="rotation">The valid rotation for this perimeter.</param>
        /// <param name="resourceTag">The resourceTag from which to load the WWObject from.</param>
        private static void PlaceWallObject(IntVector3 coordIndex, int rotation, string resourceTag)
        {
            var coordinate = new Coordinate(coordIndex);
            var wwTransform = new WWTransform(coordinate, rotation);
            WWObjectData objData = WWObjectFactory.CreateNew(wwTransform, resourceTag);
            WWObject go = WWObjectFactory.Instantiate(objData);
            if (! ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(go))
            {
                // this error should never happen as TryToPlaceWall checks for collision before calling this function
                Debug.LogError("Could not place wall because of collision, deleting temp");
                UnityEngine.Object.Destroy(go.gameObject);
            }
        }

        /// <summary>
        /// Searches for perimeter walls that are contiguous from the given coordinate index.
        /// </summary>
        /// <param name="curIndex">The coordinate index to start the contiguous search from.</param>
        /// <returns>A map from coordinate index to a bitmask of perimter walls </returns>
        private static Dictionary<IntVector3, WWWalls> SelectPerimeter(IntVector3 curIndex)
        {
            // this is the result and will be threaded through and mutated by GetPerimeterWalls
            var wallsToPlace = new Dictionary<IntVector3, WWWalls>();
            GetPerimeterWalls(wallsToPlace, new List<IntVector3>(), curIndex, ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>());
            return wallsToPlace;
        }

        /// <summary>
        /// Builds up the wallsToPlace map. Mutually recursive call with GetPerimeterWallsHelper function.
        /// </summary>
        /// <param name="wallsToPlace">The current map of index to perimeter walls. Will be mutated.</param>
        /// <param name="visited">The current list of coordinate indices already visited. Will be mutated.</param>
        /// <param name="curIndex">The coordinate index to contiguously search from.</param>
        /// <param name="sceneGraphManager">The ative Scene Graph</param>
        private static void GetPerimeterWalls(Dictionary<IntVector3, WWWalls> wallsToPlace, List<IntVector3> visited,
            IntVector3 curIndex, SceneGraphManager sceneGraphManager)
        {
            // check if there are contiguous tiles in all four directions from the current coordinate index.
            var northIndex = new IntVector3(curIndex.x, curIndex.y, curIndex.z + 1);
            var eastIndex = new IntVector3(curIndex.x + 1, curIndex.y, curIndex.z);
            var southIndex = new IntVector3(curIndex.x, curIndex.y, curIndex.z - 1);
            var westIndex = new IntVector3(curIndex.x - 1, curIndex.y, curIndex.z);
            
            visited.Add(curIndex); // mark this index as visited.

            if (!visited.Contains(northIndex)) // if we have not visited the coordinate index one to the north...
            {
                GetPerimeterWallsHelper(northIndex, WWWalls.North, wallsToPlace, visited, curIndex,
                    sceneGraphManager);
            }
            if (!visited.Contains(eastIndex)) // if we have not visited the coordinate index one to the east...
            {
                GetPerimeterWallsHelper(eastIndex, WWWalls.East, wallsToPlace, visited, curIndex,
                    sceneGraphManager);
            }
            if (!visited.Contains(southIndex)) // if we have not visited the coordinate index one to the south...
            {
                GetPerimeterWallsHelper(southIndex, WWWalls.South, wallsToPlace, visited, curIndex,
                    sceneGraphManager);
            }
            if (!visited.Contains(westIndex)) // if we have not visited the coordinate index one to the west...
            {
                GetPerimeterWallsHelper(westIndex, WWWalls.West, wallsToPlace, visited, curIndex,
                    sceneGraphManager);
            }
        }

        /// <summary>
        /// This helper function checks to see if there are any object in curIndex cooridnate index. If there are none,
        /// the origIndex must have had a perimeter wall and the wallsToPlace map is acoordingly updated. However, if
        /// there were objects at the curIndex coordinate index, the search continues on and a mutually recursive
        /// call back to GetPerimeterWalls is made.
        /// </summary>
        /// <param name="curIndex">The current coordinate index.</param>
        /// <param name="direction">The direction of the perimeter wall to test for.</param>
        /// <param name="wallsToPlace">The current threaded map of perimeter walls to place.</param>
        /// <param name="visited">The current list of coordinate Indices already visited.</param>
        /// <param name="origIndex"> The coordinate index one step before this, where the perimeter wall will be placed.</param>
        /// <param name="sceneGraphManager"> The active Scene Graph.</param>
        private static void GetPerimeterWallsHelper(IntVector3 curIndex, WWWalls direction,
            Dictionary<IntVector3,WWWalls> wallsToPlace, List<IntVector3> visited, IntVector3 origIndex,
            SceneGraphManager sceneGraphManager)
        {
            // TODO potentially filter out non Floor tiles, and Props.
            List<WWObject> objects = sceneGraphManager.GetObjectsInCoordinateIndex(new Coordinate(curIndex));
            if (objects.Count == 0) // empty coordinate index, must have originated from an coordinate index with a perimeter
            {
                if (wallsToPlace.ContainsKey(origIndex))
                {
                    // append by OR to existing perimeter walls at the coordinate index
                    wallsToPlace[origIndex] = direction | wallsToPlace[origIndex];
                }
                else
                {
                    // add a new entry into wallsToPlace
                    wallsToPlace.Add(origIndex, direction);
                }
            }
            else // search further for a perimeter
            {
                GetPerimeterWalls(wallsToPlace, visited, curIndex, sceneGraphManager);
            }
        }
        
        /// <summary>
        /// Given a resourceTag, loads the WWObject belonging to the resource and returns a list of all possible
        /// valid rotations for the WWObject that do not collide with existing WWObjects at the given position
        /// in the active Scene Graph. Useful for assisting the player with auto rotation of tiles.
        /// </summary>
        /// <param name="position">The Unity Space position to convert to Coordinate Index to determine valid rotations in.</param>
        /// <param name="resourceTag">The resourceTag belonging to the desired WWObject.</param>
        /// <returns></returns>
        public static List<int> GetPossibleRotations(Vector3 position, string resourceTag)
        {
            var result = new List<int>(); // the resulting list of possible rotations, currently empty
            Coordinate coordinate = CoordinateHelper.UnityCoordToWWCoord(position);
            WWWalls wallsToFit =  ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().GetWallsAtCoordinate(coordinate);

            // check to see if any of the 4 possible rotations would fit given resource's walls            
            WWResource resource = WWResourceController.GetResource(resourceTag);
            WWResourceMetadata resourceMetadata = resource.GetMetaData();

            for (var r = 0; r < 360; r += 90) // only rotations of 90 degrees are valid
            {
                WWWalls newWalls = WWWallsHelper.GetRotatedWWWalls(resourceMetadata, r);
                bool doesCollide = Convert.ToBoolean(newWalls & wallsToFit); // should be 0 or False if no collision
                if (!doesCollide)
                {
                    result.Add(r);
                }
            }
            return result;
        }
    }
}