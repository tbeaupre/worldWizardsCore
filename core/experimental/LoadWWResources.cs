using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.coordinate;
using UnityEngine;


namespace worldWizards.core.experimental
{
    class LoadWWResources : MonoBehaviour
    {
        SceneGraphController sceneGraphController;

        void Start ()
        {
            sceneGraphController = FindObjectOfType<SceneGraphController>();

            // Load AssetBundles.
            //WWAssetBundleController.LoadAssetBundle("testBundle", Application.dataPath + "/../AssetBundles/Windows/test");
            ResourceLoader.LoadResources(new string[] { Application.dataPath + "/../AssetBundles/Windows/test" });

            // Load Resources.
            WWResourceController.LoadResource("white", null, "whiteCube");
            WWResourceController.LoadResource("black", null, "blackCube");
            WWResourceController.LoadResource("blue", Application.dataPath + "/../AssetBundles/Windows/test", "blueCube");

            for (int i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.MockCreate(new Coordinate(i, i, i), "white");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (int i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.MockCreate(new Coordinate(i, i + 1, i), "black");
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
