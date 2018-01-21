using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.controller.resources;
using WorldWizards.core.entity.coordinate;

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

            List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(heightmap);
        }
    }
}