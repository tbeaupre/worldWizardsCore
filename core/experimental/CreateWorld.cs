using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    public class CreateWorld : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            WWResourceController.LoadResource("white", null, "whiteCube");
            WWResourceController.LoadResource("tree", null, "treeProp");

            var imagePath = "Heightmaps/MazeHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);

            var terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(heightmap);
        }
    }
}