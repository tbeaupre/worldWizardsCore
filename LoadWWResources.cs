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

            WWResourceController.GetInstance().LoadResource("tileTemp", "white");
            WWResourceController.GetInstance().LoadResource("secondTemp", "black");

            for (int i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.MockCreate(new Coordinate(i, i, i), "white");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }

            for (int i = 0; i < 5; i++)
            {
                WWObjectData objData = WWObjectFactory.MockCreate(new Coordinate(i, i+1, i), "black");
                WWObject go = WWObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }
        }
    }
}
