using NUnit.Framework;
using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;

namespace WorldWizards.core.UnitTests.Editor
{
    [TestFixture]
    /// <summary>
    /// CoordinateHelper tests.
    /// </summary>
    internal class CoordinateHelperTest
    {
        private static readonly float delta = .00001f;
        [Test]
        public static void CoordinateConversionPerservesUnitySpace()
        {
            var position = new Vector3(3.333f, 0.94444f, 3.1238746f) * CoordinateHelper.baseTileLength *
                                          CoordinateHelper.tileLengthScale;
            var wwCoord = CoordinateHelper.UnityCoordToWWCoord(position, 0);
            var decodedPostion = CoordinateHelper.WWCoordToUnityCoord(wwCoord);
            Debug.Log(string.Format(" {0} , {1}", position.ToString(), decodedPostion.ToString()));
            Assert.True(position.Equals(decodedPostion));
        }
        
        [Test]
        public static void CoordinateConversionPerservesWorldWizardSpace()
        {
            var wwwCoord = new Coordinate(new IntVector3(1,2,3), new Vector3(.33f, .9f, 0), 0);
            var position = CoordinateHelper.WWCoordToUnityCoord(wwwCoord);
            var decodedWWCoord = CoordinateHelper.UnityCoordToWWCoord(position, wwwCoord.Rotation);
            Debug.Log(string.Format("{0} , {1}", wwwCoord.ToString(), decodedWWCoord.ToString()));
            Assert.AreEqual(wwwCoord.Index, decodedWWCoord.Index);
            Assert.AreEqual(wwwCoord.Offset.x, decodedWWCoord.Offset.x, delta);
            Assert.AreEqual(wwwCoord.Offset.y, decodedWWCoord.Offset.y, delta);
            Assert.AreEqual(wwwCoord.Offset.z, decodedWWCoord.Offset.z, delta);
            Assert.AreEqual(wwwCoord.Rotation, decodedWWCoord.Rotation);
        }
        
          
        [Test]
        public static void CoordinateConversionPerservesWorldWizardSpaceWithNegOffset()
        {
            var wwwCoord = new Coordinate(new IntVector3(1,2,3), new Vector3(-.33f, -1f, 0), 0);
            var position = CoordinateHelper.WWCoordToUnityCoord(wwwCoord);
            var decodedWWCoord = CoordinateHelper.UnityCoordToWWCoord(position, wwwCoord.Rotation);
            Debug.Log(string.Format("{0} , {1}", wwwCoord.ToString(), decodedWWCoord.ToString()));
            Assert.AreEqual(wwwCoord.Index, decodedWWCoord.Index);
            Assert.AreEqual(wwwCoord.Offset.x, decodedWWCoord.Offset.x, delta);
            Assert.AreEqual(wwwCoord.Offset.y, decodedWWCoord.Offset.y, delta);
            Assert.AreEqual(wwwCoord.Offset.z, decodedWWCoord.Offset.z, delta);
            Assert.AreEqual(wwwCoord.Rotation, decodedWWCoord.Rotation);
        }
        
    }
}