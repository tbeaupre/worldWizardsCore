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
            ResourceLoader.LoadResources();

            for (int i = 0; i < 5; i++)
            {
				WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i, i), "ww_basic_assets_Tile_Grass");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (int i = 0; i < 5; i++)
            {
				WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 1, i), "ww_basic_assets_Tile_Arch");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (int i = 0; i < 5; i++)
            {
				WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 2, i), "ww_basic_assets_Tile_FloorBrick");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

			for (int i = 0; i < 5; i++)
			{
				WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 2, i), "ww_basic_assets_blueCube");
				WWObject go = WWObjectFactory.Instantiate(objData);
				sceneGraphController.Add(go);
			}
        }
    }
}
