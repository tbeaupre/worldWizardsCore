using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.gameObject.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    public class TerrainGenerator
    {
        public static List<Coordinate> CreateTerrainFromImage(Texture2D heightmap)
        {
            var coordinates = new List<Coordinate>();
            var maxHeight = 10;
            for (var x = 0; x < heightmap.width; x++)
            for (var y = 0; y < heightmap.height; y++)
            {
                var height = (int) (heightmap.GetPixel(x, y).r * maxHeight);
                var c = new Coordinate(x, height, y);
                coordinates.Add(c);

                WWObjectData parentData = WWObjectFactory.CreateNew(c, "white");
                WWObject parentObj = WWObjectFactory.Instantiate(parentData);
                ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(parentObj);
                

                //					// prop
                //					IntVector3 intVector3 = new IntVector3(x,height+1,y);
                //					Vector3 offset = new Vector3(UnityEngine.Random.Range(-1,1), 0,UnityEngine.Random.Range(-1,1));
                //					Coordinate propCoordinate = new Coordinate(intVector3, offset, 0);
                //					WWObjectData propData = WWObjectFactory.MockCreateProp(propCoordinate, "tree");
                //					WWObject propObj = WWObjectFactory.Instantiate(propData);
                //					sceneGraphController.Add(propObj);
                //
                //					List<WWObject> propChildren = new List<WWObject> ();
                //					propChildren.Add (propObj);
                //					parentObj.AddChildren (propChildren);

                while (height > 0)
                {
                    height--;
                    c = new Coordinate(x, height, y);
                    coordinates.Add(c);

                    WWObjectData childData = WWObjectFactory.CreateNew(c, "white");

                    WWObject childObj = WWObjectFactory.Instantiate(childData);
                    ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(childObj);

                    var children = new List<WWObject>();
                    children.Add(childObj);
                    parentObj.AddChildren(children);
                    parentObj = childObj;
                }
            }
            return coordinates;
        }
    }
}