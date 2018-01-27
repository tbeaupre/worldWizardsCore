using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.controller.resources;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.file.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    public class SaveLoadSceneGraph : MonoBehaviour
    {
        public void Save()
        {
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Save(FileIO.testPath);
        }

        public void Load()
        {
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Load(FileIO.testPath);
        }

        public void DeleteObjects()
        {
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().ClearAll();
        }
    }
}