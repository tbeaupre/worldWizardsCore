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
using Object = UnityEngine.Object;

namespace WorldWizards.core.entity.level.utils
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// 
    /// </summary>
    public static class BuilderAlgorithms
    {
        public static void BuildPerimeterWalls(string resourceTag, WWObject wwObject)
        {
            IntVector3 curIndex = wwObject.GetCoordinate().Index;
            Dictionary<IntVector3, WWWalls> walls = SelectPerimeter(curIndex);
            foreach (KeyValuePair<IntVector3, WWWalls> wall in walls)
            {
                if (Convert.ToBoolean(WWWalls.North & wall.Value))
                {
                    BuildWalls(wall.Key, WWWalls.North, resourceTag);
                }
                if (Convert.ToBoolean(WWWalls.East & wall.Value))
                {
                    BuildWalls(wall.Key, WWWalls.East, resourceTag);
                }
                if (Convert.ToBoolean(WWWalls.South & wall.Value))
                {
                    BuildWalls(wall.Key, WWWalls.South, resourceTag);
                }
                if (Convert.ToBoolean(WWWalls.West & wall.Value))
                {
                    BuildWalls(wall.Key, WWWalls.West, resourceTag);
                }
            }
        }

        private static void BuildWalls(IntVector3 coordIndex, WWWalls wallOpening, string resourceTag)
        {
            WWWalls wallsToFit = ~ wallOpening |
                                 ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().GetWallsAtCoordinate(
                                     new Coordinate(coordIndex));
            WWResource resource = WWResourceController.GetResource(resourceTag);
            WWResourceMetadata resourceMetadata = resource.GetMetaData();

            for (var r = 0; r < 360; r += 90)
            {
                WWWalls newWalls = WWWallsHelper.GetRotatedWWWalls(resourceMetadata, r);
                bool doesCollide = Convert.ToBoolean(newWalls & wallsToFit); // should be 0 or False if no collision
                if (!doesCollide)
                {
                    PlaceWallObject(coordIndex, r, resourceTag);
                    break;
                }
            }
        }

        private static void PlaceWallObject(IntVector3 coordIndex, int rotation, string resourceTag)
        {
            var coordinate = new Coordinate(coordIndex, rotation);
            WWObjectData objData = WWObjectFactory.CreateNew(coordinate, resourceTag);
            // TODO refactor to only instantiate object if it can fit by looking at ResourceMetaData
            // but, currently this is only called when a fit is possible
            WWObject go = WWObjectFactory.Instantiate(objData);
            if (! ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(go))
            {
                Debug.Log("Could not place wall because of collision, deleting temp");
                Object.Destroy(go.gameObject);
            }
        }

        public static List<int> GetPossibleRotations(Vector3 position, string resourceTag)
        {
            var result = new List<int>();
            Coordinate coordinate = CoordinateHelper.UnityCoordToWWCoord(position, 0);
            WWWalls wallsToFit =  ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().GetWallsAtCoordinate(coordinate);

            // check to see if any of the 4 possible rotations would fit given resource's walls            
            WWResource resource = WWResourceController.GetResource(resourceTag);
            WWResourceMetadata resourceMetadata = resource.GetMetaData();

            for (var r = 0; r < 360; r += 90)
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

        public static Dictionary<IntVector3, WWWalls> SelectPerimeter(IntVector3 curIndex)
        {
            var wallsToPlace = new Dictionary<IntVector3, WWWalls>();
            var visited = new List<IntVector3>();
            GetPerimeterWalls(wallsToPlace, visited, curIndex, ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>());
            return wallsToPlace;
        }

        private static void GetPerimeterWalls(Dictionary<IntVector3, WWWalls> wallsToPlace, List<IntVector3> visited,
            IntVector3 curIndex, SceneGraphManager sceneGraphManager)
        {
            var northIndex = new IntVector3(curIndex.x, curIndex.y, curIndex.z + 1);
            var eastIndex = new IntVector3(curIndex.x + 1, curIndex.y, curIndex.z);
            var southIndex = new IntVector3(curIndex.x, curIndex.y, curIndex.z - 1);
            var westIndex = new IntVector3(curIndex.x - 1, curIndex.y, curIndex.z);

            visited.Add(curIndex);

            if (!visited.Contains(northIndex))
            {
                List<WWObject> northObjects = sceneGraphManager.GetObjectsInCoordinateIndex(new Coordinate(northIndex));
                GetPerimeterWallsHelper(northObjects, northIndex, WWWalls.North, wallsToPlace, visited, curIndex,
                    sceneGraphManager);
            }
            if (!visited.Contains(eastIndex))
            {
                List<WWObject> eastObjects = sceneGraphManager.GetObjectsInCoordinateIndex(new Coordinate(eastIndex));
                GetPerimeterWallsHelper(eastObjects, eastIndex, WWWalls.East, wallsToPlace, visited, curIndex,
                    sceneGraphManager);
            }
            if (!visited.Contains(southIndex))
            {
                List<WWObject> southObjects = sceneGraphManager.GetObjectsInCoordinateIndex(new Coordinate(southIndex));
                GetPerimeterWallsHelper(southObjects, southIndex, WWWalls.South, wallsToPlace, visited, curIndex,
                    sceneGraphManager);
            }
            if (!visited.Contains(westIndex))
            {
                List<WWObject> westObjectsList =
                    sceneGraphManager.GetObjectsInCoordinateIndex(new Coordinate(westIndex));
                GetPerimeterWallsHelper(westObjectsList, westIndex, WWWalls.West, wallsToPlace, visited, curIndex,
                    sceneGraphManager);
            }
        }

        private static void GetPerimeterWallsHelper(List<WWObject> objects, IntVector3 curIndex, WWWalls direction,
            Dictionary<IntVector3,
                WWWalls> wallsToPlace, List<IntVector3> visited, IntVector3 origIndex,
            SceneGraphManager sceneGraphManager)
        {
            // TODO only consider the floor type tiles in Count
            //            if (Convert.ToBoolean(~objects))
            if (objects.Count == 0)
            {
                if (wallsToPlace.ContainsKey(origIndex))
                {
                    wallsToPlace[origIndex] = direction | wallsToPlace[origIndex];
                }
                else
                {
                    wallsToPlace.Add(origIndex, direction);
                }
            }
            else // search further for perimeter
            {
                GetPerimeterWalls(wallsToPlace, visited, curIndex, sceneGraphManager);
            }
        }
    }
}