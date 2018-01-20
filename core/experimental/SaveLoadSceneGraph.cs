using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.file.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    public class SaveLoadSceneGraph : MonoBehaviour
    {
        private void Start()
        {
            WWResourceController.LoadResource("white", null, "whiteCube");
            WWResourceController.LoadResource("tree", null, "treeProp");
        }

        public void Save()
        {
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Save(FileIO.testPath);
        }

        public void Load()
        {
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Load(FileIO.testPath);
        }

        public void CreateMaze()
        {
            var imagePath = "Heightmaps/MazeHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);

            List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(heightmap);
        }

        public void CreateTerrain()
        {
            var imagePath = "Heightmaps/TerrainHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);
            List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(heightmap);
        }

        public void DeleteObjects()
        {
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().ClearAll();
        }
    }
}