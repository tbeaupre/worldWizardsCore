using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldWizards.core.controller.level.utils;
using worldWizardsCore.core.controller.level;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using UnityEngine;


namespace Assets.worldWizardsCore
{
    class LoadWWResources : MonoBehaviour
    {
        SceneGraphController sceneGraphController;

        void Start ()
        {
            sceneGraphController = FindObjectOfType<SceneGraphController>();

            WorldWizardsObject obj = Resources.Load<WorldWizardsObject>("tileTemp");
            obj.Init(Guid.NewGuid(), WorldWizardsType.Tile, null, null, null, null);
            Debug.Log(obj);
            sceneGraphController.Add(obj);
        }
    }
}
