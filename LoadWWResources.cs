using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizardsCore.core.controller.level;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;
using UnityEngine;


namespace Assets.worldWizardsCore
{
    class LoadWWResources : MonoBehaviour
    {
        SceneGraphController sceneGraphController;

        void Start ()
        {
            sceneGraphController = FindObjectOfType<SceneGraphController>();

            // Load AssetBundles.
            WWAssetBundleController.LoadAssetBundle("testBundle", Application.dataPath + "/../AssetBundles/Windows/test");

            // Load Resources.
            WWResourceController.LoadResource("white", null, "whiteCube");
            WWResourceController.LoadResource("black", null, "blackCube");
            WWResourceController.LoadResource("blue", "testBundle", "blueCube");

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
