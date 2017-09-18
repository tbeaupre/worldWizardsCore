using System;
using worldWizards.core.entity.coordinate;
using UnityEngine;
using System.Collections.Generic;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;

namespace worldWizards.core.controller.level.utils
{
	public class TerrainGenerator
	{
		static string resourceTag = "white";
		public static List<Coordinate> CreateTerrainFromImage(SceneGraphController sceneGraphController, Texture2D heightmap){
			List<Coordinate> coordinates = new List<Coordinate>();
			int maxHeight = 10;
			for(int x = 0; x < heightmap.width; x++){
				for (int y = 0; y < heightmap.height; y++) {
					int height = (int)(heightmap.GetPixel (x, y).r * maxHeight);
					Coordinate c = new Coordinate(x,height,y);
					coordinates.Add (c);

					WWObjectData parentData = WWObjectFactory.MockCreate(c, resourceTag);
					WWObject parentObj = WWObjectFactory.Instantiate(parentData);
					sceneGraphController.Add(parentObj);

					// prop
					IntVector3 intVector3 = new IntVector3(x,height+1,y);
					Vector3 offset = new Vector3(UnityEngine.Random.Range(-1,1), 0,UnityEngine.Random.Range(-1,1));
					Coordinate propCoordinate = new Coordinate(intVector3, offset);
					WWObjectData propData = WWObjectFactory.MockCreateProp(propCoordinate, resourceTag);
					WWObject propObj = WWObjectFactory.Instantiate(propData);
					sceneGraphController.Add(propObj);

					List<WWObject> propChildren = new List<WWObject> ();
					propChildren.Add (propObj);
					parentObj.AddChildren (propChildren);


					while (height > 0) {
						height--;
						c = new Coordinate(x,height,y);
						coordinates.Add (c);

						WWObjectData childData = WWObjectFactory.MockCreate(c, resourceTag);

						WWObject childObj = WWObjectFactory.Instantiate(childData);
						sceneGraphController.Add(childObj);

						List<WWObject> children = new List<WWObject> ();
						children.Add (childObj);
						parentObj.AddChildren (children);
						parentObj = childObj;

					}
				}	
			}
			return coordinates;
		}


		public static void SetupRelationships (SceneGraphController sceneGraphController){
			



		}


	}
}

