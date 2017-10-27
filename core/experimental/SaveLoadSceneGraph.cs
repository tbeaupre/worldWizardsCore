using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;

namespace WorldWizards.core.experimental
{
    public class SaveLoadSceneGraph : MonoBehaviour
    {
        private SceneGraphController sceneGraphController;

        private void Start()
        {
            sceneGraphController = FindObjectOfType<SceneGraphController>();
            WWResourceController.LoadResource("white", null, "whiteCube");
            WWResourceController.LoadResource("tree", null, "treeProp");
        }

        public void Save()
        {
            sceneGraphController.Save();
        }

        public void Load()
        {
            sceneGraphController.Load();
        }

        public void CreateMaze()
        {
            var imagePath = "Heightmaps/MazeHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);

            var terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(sceneGraphController, heightmap);
        }

        public void CreateTerrain()
        {
            var imagePath = "Heightmaps/TerrainHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);
            var terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(sceneGraphController, heightmap);
        }

        public void DeleteObjects()
        {
            sceneGraphController.ClearAll();
        }
    }
}