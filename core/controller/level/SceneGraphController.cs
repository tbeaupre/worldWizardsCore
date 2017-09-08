using System;
using UnityEngine;
using worldWizards.core.entity.level;
using worldWizards.core.entity.gameObject;

namespace worldWizards.core.controller.level
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

        public void Add(WWObject wwObject)
        {
            if (wwObject != null)
            {
                sceneGraph.Add(wwObject);
            }
        }

        public void Delete(Guid id)
        {
            sceneGraph.Delete(id);
        }

		public void DeleteDescending(Guid id)
		{
			sceneGraph.DeleteDescending(id);
		}
			
		public void Load(){
			sceneGraph.Load ();
		}

		public void Save(){
			sceneGraph.Save ();
		}

		public void ClearAll(){
			sceneGraph.ClearAll ();
		}


    }
}
