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
		public SceneGraph sceneGraph { 
			get; 
			private set;
		}

        public void Awake()
        {
            sceneGraph = new SceneGraph();
        }
			
		public int SceneSize(){
			return this.sceneGraph.SceneSize ();
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
