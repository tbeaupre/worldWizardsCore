using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
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

        public bool Add(WWObject wwObject)
        {
            if (wwObject != null)
                return sceneGraph.Add(wwObject);
            
            return false;
        }

        public void HideObjectsAbove(int height)
        {
            sceneGraph.HideObjectsAbove(height);
        }

        public WWWalls GetWallsAtCoordinate(Coordinate coordinate)
        {
            return sceneGraph.GetWallsAtCoordinate(coordinate);
        }

        public List<WWObject> GetObjectsInCoordinateIndex(Coordinate coordinate)
        {
            return sceneGraph.GetObjectsInCoordinateIndex(coordinate);
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