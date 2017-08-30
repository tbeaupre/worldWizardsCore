using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldWizards.core.controller.level.utils;
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

            for (int i = 0; i < 5; i++)
            {
                WWObjectData objData = WorldWizardObjectFactory.MockCreate(new Coordinate(i, i, i));
                WorldWizardsObject go = WorldWizardObjectFactory.Instantiate(objData);
                sceneGraphController.Add(go);
            }
        }
    }
}
