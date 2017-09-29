using System.Collections;
using System.Collections.Generic;
using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.coordinate;
using UnityEngine;



namespace worldWizards.core.experimental { 
public class CreateWorld : MonoBehaviour {

    // Use this for initialization
    void Start() {

            SceneGraphController sceneGraphController = FindObjectOfType<SceneGraphController>();
            WWResourceController.LoadResource("white", null, "whiteCube");
            WWResourceController.LoadResource("tree", null, "treeProp");

            string imagePath = "Heightmaps/MazeHeightmap";
            Texture2D heightmap = Resources.Load<Texture2D>(imagePath);

            List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(sceneGraphController, heightmap);

        }


}
}