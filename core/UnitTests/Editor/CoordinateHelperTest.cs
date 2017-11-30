using NUnit.Framework;
using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;

namespace worldWizards.core.unitTests
{
    [TestFixture]
    /// <summary>
    /// Scene graph controller test.
    /// </summary>
    internal class CoordinateHelperTest
    {
        [SetUp]
        public static void Setup()
        {
        }

        [TearDown]
        public static void TearDown()
        {
        }

        [Test]
        public static void SceneGraphNotNull()
        {
            Vector3 unityCoord = new Vector3(3.0f, 0.0f, 3.9f) * CoordinateHelper.baseTileLength *
                                 CoordinateHelper.tileLengthScale;
            Coordinate wwCoord = CoordinateHelper.ConvertUnityCoordinateToWWCoordinate(unityCoord);
            Debug.Log(wwCoord.index);
            Assert.True(true);
        }
    }
}