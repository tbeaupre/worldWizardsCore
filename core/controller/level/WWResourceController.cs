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
            assetBundles = new Dictionary<string, AssetBundle>();
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

        Dictionary<string, AssetBundle> assetBundles;
        Dictionary<string, WWResource> resources;

        public void LoadBundle(string assetBundlePath, string bundleTag)
        {
            if (assetBundles.ContainsKey(bundleTag))
            {
                Debug.Log("bundleTag: " + bundleTag + " has already been used.");
            }
            else
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
                if (assetBundle == null)
                {
                    Debug.Log("Unable to load Asset Bundle");
                }
                else
                {
                    assetBundles.Add(bundleTag, assetBundle);
                }
            }
        }

        public void LoadResource(string bundleTag, string name, string tag)
        {
            AssetBundle assetBundle = GetAssetBundle(bundleTag);
            AddResource(tag, new WWResource(assetBundle, name));
        }

        public void LoadResource(string path, string tag)
        {
            AddResource(tag, new WWResource(path));
        }

        public void AddResource(string tag, WWResource resource)
        {
            if (resources.ContainsKey(tag))
            {
                Debug.Log("tag: " + tag + " has already been used.");
            }
            else
            {
                resources.Add(tag, resource);
            }
        }

        public AssetBundle GetAssetBundle(string tag)
        {
            AssetBundle assetBundle;
            if (assetBundles.TryGetValue(tag, out assetBundle))
            {
                return assetBundle;
            }
            else
            {
                Debug.Log("An asset bundle with this tag has not been loaded.");
                return null;
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
