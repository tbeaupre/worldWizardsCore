using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.coordinate;
using UnityEngine;


namespace worldWizards.core.experimental
{
    class BuildFromTestBundle : MonoBehaviour
    {
        SceneGraphController sceneGraphController;

        void Start ()
        {
            sceneGraphController = FindObjectOfType<SceneGraphController>();

            // Load AssetBundles.
			WWAssetBundleController.LoadAssetBundle("ww_basic_assets", Application.dataPath + "/../AssetBundles/OSX/ww_basic_assets");
			WWAssetBundleController.LoadAssetBundle("testBundle", Application.dataPath + "/../AssetBundles/OSX/test");
			WWResourceController.LoadResource("blue", "testBundle", "blueCube");


            // Load Resources.
//            WWResourceController.LoadResource("white", null, "whiteCube");
//            WWResourceController.LoadResource("black", null, "blackCube");
			WWResourceController.LoadResource("Tile_FloorBrick", "ww_basic_assets", "Tile_FloorBrick");
			WWResourceController.LoadResource("Tile_Grass", "ww_basic_assets", "Tile_Grass");
			WWResourceController.LoadResource("Tile_Arch", "ww_basic_assets", "Tile_Arch");


            for (int i = 0; i < 5; i++)
            {
				WWObjectData objData = WWObjectFactory.MockCreate(new Coordinate(i, i, i), "Tile_Grass");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (int i = 0; i < 5; i++)
            {
				WWObjectData objData = WWObjectFactory.MockCreate(new Coordinate(i, i + 1, i), "Tile_Arch");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (int i = 0; i < 5; i++)
            {
				WWObjectData objData = WWObjectFactory.MockCreate(new Coordinate(i, i + 2, i), "Tile_FloorBrick");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

			for (int i = 0; i < 5; i++)
			{
				WWObjectData objData = WWObjectFactory.MockCreate(new Coordinate(i, i + 2, i), "blue");
				WWObject go = WWObjectFactory.Instantiate(objData);
				sceneGraphController.Add(go);
			}
        }
    }
}
