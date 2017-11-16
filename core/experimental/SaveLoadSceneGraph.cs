using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
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
            ManagerRegistry.Instance.sceneGraphImpl.Save();
        }

        public void Load()
        {
            ManagerRegistry.Instance.sceneGraphImpl.Load();
        }

        public void CreateMaze()
        {
            var imagePath = "Heightmaps/MazeHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);

            var terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(heightmap);
        }

        public void CreateTerrain()
        {
            var imagePath = "Heightmaps/TerrainHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);
            var terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(heightmap);
        }

        public void DeleteObjects()
        {
            ManagerRegistry.Instance.sceneGraphImpl.ClearAll();
        }
    }
}