using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    internal class LoadWWResources : MonoBehaviour
    {
        private void Start()
        {
            CoordinateHelper.baseTileLength = 1;

            // Load Resources from AssetBundles.
            ResourceLoader.LoadResources();

            for (var i = 0; i < 5; i++)
            {
                var objData = WWObjectFactory.CreateNew(new Coordinate(i, i, i), "defaultWhiteCube");
                var go = WWObjectFactory.Instantiate(objData);
                ManagerRegistry.Instance.sceneGraphImpl.Add(go);
            }

            for (var i = 0; i < 5; i++)
            {
                var objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 1, i), "test_blackcube");
                var go = WWObjectFactory.Instantiate(objData);
                ManagerRegistry.Instance.sceneGraphImpl.Add(go);
            }

            for (var i = 0; i < 5; i++)
            {
                var objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 2, i), "test_bluecube");
                var go = WWObjectFactory.Instantiate(objData);
                ManagerRegistry.Instance.sceneGraphImpl.Add(go);
            }
        }
    }
}