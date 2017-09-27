using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.coordinate;
using worldWizards.core.entity.coordinate.utils;
using worldWizards.core.entity.gameObject;

namespace worldWizards.core.unitTests{
	
	[TestFixture]
	/// <summary>
	/// Scene graph controller test.
	/// </summary>
	internal class CoordinateHelperTest {
		

		[SetUp]
		public static void Setup(){
			
		}

		[TearDown]
		public static void TearDown(){
	
		}
			
		[Test]
		public static void SceneGraphNotNull() {
			Vector3 unityCoord = new Vector3(3.0f,0.0f,3.9f) * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
			Coordinate wwCoord = CoordinateHelper.convertUnityCoordinateToWWCoordinate (unityCoord);
			Debug.Log (wwCoord.index);
			Assert.True (true);
		}

	}
}