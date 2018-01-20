using UnityEngine;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    internal class LoadWWResources : MonoBehaviour
    {
        private void Start()
        {
            CoordinateHelper.baseTileLength = 1;

            for (var i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i, i), "defaultWhiteCube");
                WWObject go = WWObjectFactory.Instantiate(objData);
                ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(go);
            }

            for (var i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 1, i), "test_blackcube");
                WWObject go = WWObjectFactory.Instantiate(objData);
                ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(go);
            }

            for (var i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 2, i), "test_bluecube");
                WWObject go = WWObjectFactory.Instantiate(objData);
                ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(go);
            }
        }
    }
}