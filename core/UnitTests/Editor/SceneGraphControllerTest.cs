using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace worldWizards.core.unitTests{
	
	[TestFixture]
	/// <summary>
	/// Scene graph controller test.
	/// </summary>
	internal class SceneGraphControllerTest {
		static GameObject root;

		[SetUp]
		public static void Setup(){
			WWResourceController.LoadResource("white", null, "whiteCube");
			WWResourceController.LoadResource("tree", null, "treeProp");
		}

		[TearDown]
		public static void TearDown(){
			// remove everything from the SceneGraph
		    ManagerRegistry.Instance.sceneGraphImpl.ClearAll ();
			Assert.AreEqual (0,  ManagerRegistry.Instance.sceneGraphImpl.SceneSize());
		}
			
		[Test]
		public static void SceneGraphNotNull() {
			// Use the Assert class to test conditions
			Assert.IsNotNull ( ManagerRegistry.Instance.sceneGraphImpl);
		}
			
		[Test]
		public static void AddObjectToSceneGraph() {
			Assert.AreEqual (0,  ManagerRegistry.Instance.sceneGraphImpl.SceneSize());
			Coordinate coordinate = new Coordinate (0,0,0);
			WWObjectData wwObjectData = WWObjectFactory.CreateNew (coordinate, "white");
			WWObject wwObject = WWObjectFactory.Instantiate(wwObjectData);
		    ManagerRegistry.Instance.sceneGraphImpl.Add (wwObject);
			Assert.AreEqual (1,  ManagerRegistry.Instance.sceneGraphImpl.SceneSize());
		}

		[Test]
		/// <summary>
		/// Saves and Loads a large scene with child parent relationships and ensures
		/// that the same number of objects are loaded as saved.
		/// </summary>
		public static void SaveLoadSceneGraph() {
			CreateMaze ();
		    ManagerRegistry.Instance.sceneGraphImpl.Save ();
			int objectCountBeforeSave =  ManagerRegistry.Instance.sceneGraphImpl.SceneSize ();
		    ManagerRegistry.Instance.sceneGraphImpl.ClearAll ();
		    ManagerRegistry.Instance.sceneGraphImpl.Load ();
			int objectCountAfterSave =  ManagerRegistry.Instance.sceneGraphImpl.SceneSize ();
			Assert.AreEqual (objectCountAfterSave, objectCountBeforeSave);
		}
			

		private static void CreateMaze(){
			string imagePath = "Heightmaps/MazeHeightmap";
			Texture2D heightmap = Resources.Load<Texture2D> (imagePath);
	
			List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage (heightmap);
		}
	
		private void CreateTerrain(){
			string imagePath = "Heightmaps/TerrainHeightmap";
			Texture2D heightmap = Resources.Load<Texture2D> (imagePath);
			List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage (heightmap);
		}
	
		private void DeleteObjects(){
		    ManagerRegistry.Instance.sceneGraphImpl.ClearAll ();
		}


	}
}