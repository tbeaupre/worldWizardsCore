using NUnit.Framework;
using UnityEngine;
using WorldWizards.core.entity.coordinate.utils;

namespace WorldWizards.core.UnitTests.Editor
{
    [TestFixture]
    /// <summary>
    /// CoordinateHelper tests.
    /// </summary>
    internal class CoordinateHelperTest
    {
        [Test]
        public static void CoordinateConversionWithNoOffset()
        {
            var position = new Vector3(3.0f, 0.0f, 3.9f) * CoordinateHelper.baseTileLength *
                                          CoordinateHelper.tileLengthScale;
            var wwCoord = CoordinateHelper.UnityCoordToWWCoord(position, 0);
            var decodedPostion = CoordinateHelper.WWCoordToUnityCoord(wwCoord);
            Assert.True(position.Equals(decodedPostion));

        }
    }
}