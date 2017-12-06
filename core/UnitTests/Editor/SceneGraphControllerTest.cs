using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace WorldWizards.core.UnitTests.Editor
{
    [TestFixture]
    /// <summary>
    /// Scene graph controller test.
    /// </summary>
    internal class SceneGraphControllerTest
    {
        [SetUp]
        public static void Setup()
        {
            WWResourceController.LoadResource("white", null, "whiteCube");
            WWResourceController.LoadResource("tree", null, "treeProp");
        }

        [TearDown]
        public static void TearDown()
        {
            // remove everything from the SceneGraph
            ManagerRegistry.Instance.sceneGraphManager.ClearAll();
            Assert.AreEqual(0, ManagerRegistry.Instance.sceneGraphManager.SceneSize());
        }

        private static GameObject root;


        private static void CreateMaze()
        {
            var imagePath = "Heightmaps/MazeHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);

            List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(heightmap);
        }

        private void CreateTerrain()
        {
            var imagePath = "Heightmaps/TerrainHeightmap";
            var heightmap = Resources.Load<Texture2D>(imagePath);
            List<Coordinate> terrainCoordinates = TerrainGenerator.CreateTerrainFromImage(heightmap);
        }

        private void DeleteObjects()
        {
            ManagerRegistry.Instance.sceneGraphManager.ClearAll();
        }

        [Test]
        public static void AddObjectToSceneGraph()
        {
            Assert.AreEqual(0, ManagerRegistry.Instance.sceneGraphManager.SceneSize());
            var coordinate = new Coordinate(0, 0, 0);
            WWObjectData wwObjectData = WWObjectFactory.CreateNew(coordinate, "white");
            WWObject wwObject = WWObjectFactory.Instantiate(wwObjectData);
            ManagerRegistry.Instance.sceneGraphManager.Add(wwObject);
            Assert.AreEqual(1, ManagerRegistry.Instance.sceneGraphManager.SceneSize());
        }

        [Test]
        /// <summary>
        /// Saves and Loads a large scene with child parent relationships and ensures
        /// that the same number of objects are loaded as saved.
        /// </summary>
        public static void SaveLoadSceneGraph()
        {
            CreateMaze();
            ManagerRegistry.Instance.sceneGraphManager.Save();
            int objectCountBeforeSave = ManagerRegistry.Instance.sceneGraphManager.SceneSize();
            ManagerRegistry.Instance.sceneGraphManager.ClearAll();
            ManagerRegistry.Instance.sceneGraphManager.Load();
            int objectCountAfterSave = ManagerRegistry.Instance.sceneGraphManager.SceneSize();
            Assert.AreEqual(objectCountAfterSave, objectCountBeforeSave);
        }

        [Test]
        public static void SceneGraphNotNull()
        {
            // Use the Assert class to test conditions
            Assert.IsNotNull(ManagerRegistry.Instance.sceneGraphManager);
        }
    }
}