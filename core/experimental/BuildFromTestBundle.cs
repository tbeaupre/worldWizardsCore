using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.experimental
{
    internal class BuildFromTestBundle : MonoBehaviour
    {
        private SceneGraphController sceneGraphController;

        private void Start()
        {
            sceneGraphController = FindObjectOfType<SceneGraphController>();
            ResourceLoader.LoadResources();

            for (var i = 0; i < 5; i++)
            {
                var objData = WWObjectFactory.CreateNew(new Coordinate(i, i, i), "ww_basic_assets_Tile_Grass");
                var go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (var i = 0; i < 5; i++)
            {
                var objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 1, i), "ww_basic_assets_Tile_Arch");
                var go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (var i = 0; i < 5; i++)
            {
                var objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 2, i), "ww_basic_assets_Tile_FloorBrick");
                var go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (var i = 0; i < 5; i++)
            {
                var objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 2, i), "ww_basic_assets_blueCube");
                var go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }
        }
    }
}