using System;
using UnityEngine;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.level;

namespace WorldWizards.core.controller.level
{
	/// <summary>
	/// </summary>
	public class SceneGraphController : MonoBehaviour
    {
        public SceneGraph sceneGraph { get; private set; }

        public void Awake()
        {
            sceneGraph = new SceneGraph();
        }

        public int SceneSize()
        {
            return sceneGraph.SceneSize();
        }

        public void Add(WWObject wwObject)
        {
            if (wwObject != null)
                sceneGraph.Add(wwObject);
        }

        public void Delete(Guid id)
        {
            sceneGraph.Delete(id);
        }

        public void Load()
        {
            sceneGraph.Load();
        }

        public void Save()
        {
            sceneGraph.Save();
        }

        public void ClearAll()
        {
            sceneGraph.ClearAll();
        }
    }
}