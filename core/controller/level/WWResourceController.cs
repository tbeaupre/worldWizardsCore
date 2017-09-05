using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using worldWizards.core.entity.gameObject;

namespace worldWizards.core.controller.level
{
    class WWResourceController
    {
        #region Singleton
        private static WWResourceController instance;

        private WWResourceController()
        {
            resources = new Dictionary<string, WWResource>();
        }

        public static WWResourceController GetInstance()
        {
            if (instance == null)
            {
                instance = new WWResourceController();
            }
            return instance;
        }
        #endregion

        Dictionary<string, WWResource> resources;

        public void LoadResource(string path, string tag)
        {
            if (resources.ContainsKey(tag))
            {
                Debug.Log("tag: " + tag + " has already been used.");
            }
            else
            {
                resources.Add(tag, new WWResource(path));
            }
        }

        public WWResource GetResource(string tag)
        {
            WWResource resource;
            if (resources.TryGetValue(tag, out resource))
            {
                return resource;
            }
            else
            {
                Debug.Log("A resource with this tag has not been loaded.");
                return null;
            }
        }
    }
}
