using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldWizards.core.controller.level.utils;
using worldWizardsCore.core.controller.level;
using worldWizards.core.entity.gameObject;
using UnityEngine;


namespace Assets.worldWizardsCore
{
    class LoadWWResources : MonoBehaviour
    {
        SceneGraphController sceneGraphController;

        void Awake ()
        {
            sceneGraphController = FindObjectOfType<SceneGraphController>();

            WorldWizardsObject obj = Resources.Load<WorldWizardsObject>("tileTemp");
            Debug.Log(obj);
            sceneGraphController.Add(obj);
        }
    }
}
