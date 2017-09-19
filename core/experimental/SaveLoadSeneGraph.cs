using System.Collections.Generic;
using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.coordinate;
using UnityEngine;

namespace worldWizards.core.experimental
{
	public class SaveLoadSeneGraph : MonoBehaviour
	{
		SceneGraphController sceneGraphController;

		void Start ()
		{
			sceneGraphController = FindObjectOfType<SceneGraphController>();
		}
			
		public void Save(){
			sceneGraphController.Save ();
		}

		public void Load(){
			sceneGraphController.Load ();
		}

		public void CreateMaze(){
			string imagePath = "Heightmaps/MazeHeightmap";
			Texture2D heightmap = Resources.Load<Texture2D> (imagePath);

			List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage (sceneGraphController, heightmap);
		}

		public void CreateTerrain(){
			string imagePath = "Heightmaps/TerrainHeightmap";
			Texture2D heightmap = Resources.Load<Texture2D> (imagePath);
			List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage (sceneGraphController, heightmap);
		}

		public void DeleteObjects(){
			sceneGraphController.ClearAll ();
		}
	}
}
