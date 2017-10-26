using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.coordinate;
using worldWizards.core.entity.gameObject;

namespace worldWizards.core.unitTests{
	
	[TestFixture]
	/// <summary>
	/// Scene graph controller test.
	/// </summary>
	internal class SceneGraphControllerTest {
		static GameObject root;
		static SceneGraphController sceneGraphController;

		[SetUp]
		public static void Setup(){
			root = new GameObject ();
			sceneGraphController = root.AddComponent<SceneGraphController>();
			sceneGraphController.Awake ();
			WWResourceController.LoadResource("white", null, "whiteCube");
			WWResourceController.LoadResource("tree", null, "treeProp");
		}

		[TearDown]
		public static void TearDown(){
			// remove everything from the SceneGraph
			sceneGraphController.ClearAll ();
			Assert.AreEqual (0, sceneGraphController.sceneGraph.SceneSize());
		}
			
		[Test]
		public static void SceneGraphNotNull() {
			// Use the Assert class to test conditions
			Assert.IsNotNull (sceneGraphController.sceneGraph);
		}
			
		[Test]
		public static void AddObjectToSceneGraph() {
			Assert.AreEqual (0, sceneGraphController.sceneGraph.SceneSize());
			Coordinate coordinate = new Coordinate (0,0,0);
			WWObjectData wwObjectData = WWObjectFactory.CreateNew (coordinate, "white");
			WWObject wwObject = WWObjectFactory.Instantiate(wwObjectData);
			sceneGraphController.Add (wwObject);
			Assert.AreEqual (1, sceneGraphController.sceneGraph.SceneSize());
		}

		[Test]
		/// <summary>
		/// Saves and Loads a large scene with child parent relationships and ensures
		/// that the same number of objects are loaded as saved.
		/// </summary>
		public static void SaveLoadSceneGraph() {
			CreateMaze ();
			sceneGraphController.Save ();
			int objectCountBeforeSave = sceneGraphController.SceneSize ();
			sceneGraphController.ClearAll ();
			sceneGraphController.Load ();
			int objectCountAfterSave = sceneGraphController.SceneSize ();
			Assert.AreEqual (objectCountAfterSave, objectCountBeforeSave);
		}
			

		private static void CreateMaze(){
			string imagePath = "Heightmaps/MazeHeightmap";
			Texture2D heightmap = Resources.Load<Texture2D> (imagePath);
	
			List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage (sceneGraphController, heightmap);
		}
	
		private void CreateTerrain(){
			string imagePath = "Heightmaps/TerrainHeightmap";
			Texture2D heightmap = Resources.Load<Texture2D> (imagePath);
			List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage (sceneGraphController, heightmap);
		}
	
		private void DeleteObjects(){
			sceneGraphController.ClearAll ();
		}


	}
}