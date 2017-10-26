using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.coordinate;
using worldWizards.core.entity.coordinate.utils;
using UnityEngine;


namespace worldWizards.core.experimental
{
    class LoadWWResources : MonoBehaviour
    {
        SceneGraphController sceneGraphController;

        void Start ()
        {
            sceneGraphController = FindObjectOfType<SceneGraphController>();

            CoordinateHelper.baseTileLength = 1;

            // Load Resources from AssetBundles.
            ResourceLoader.LoadResources();

            for (int i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i, i), "defaultWhiteCube");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (int i = 0; i < 5; i++)
            {
				WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 1, i), "test_blackcube");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (int i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.CreateNew(new Coordinate(i, i + 2, i), "test_bluecube");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }
        }
    }
}
