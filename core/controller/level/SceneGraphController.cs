using System;
using UnityEngine;
using worldWizards.core.entity.level;
using worldWizards.core.entity.gameObject;

namespace worldWizardsCore.core.controller.level
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneGraphController : MonoBehaviour
    {
        private SceneGraph sceneGraph;

        public void Awake()
        {
            sceneGraph = new SceneGraph();
        }

        public void Add(WWObject worldWizardsObject)
        {
            sceneGraph.Add(worldWizardsObject);
        }

        public WWObject Remove(Guid id)
        {
            return sceneGraph.Remove(id);
        }
    }
}
