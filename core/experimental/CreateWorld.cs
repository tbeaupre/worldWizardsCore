using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;

namespace WorldWizards.core.experimental
{
    public class CreateWorld : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            var sceneGraphController = FindObjectOfType<SceneGraphController>();
            WWResourceController.LoadResource("white", null, "whiteCube");
            WWResourceController.LoadResource("tree", null, "treeProp");

            var imagePath = "Heightmaps/MazeHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);

            var terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(sceneGraphController, heightmap);
        }
    }
}