using System.Collections.Generic;
using worldWizards.core.entity.gameObject;
using UnityEngine;

namespace worldWizards.core.controller.level
{
    public static class WWResourceController
    {
        public static Dictionary<string, WWResource> bundles = new Dictionary<string, WWResource>();

        public static void LoadResource(string tag, string assetBundleTag, string path)
        {
            if (bundles.ContainsKey(tag))
            {
                Debug.Log("resourceTag: " + tag + " has already been used.");
            }
            else
            {
                WWResource resource = new WWResource(assetBundleTag, path);
                bundles.Add(tag, resource);
            }
        }

        public static WWResource GetResource(string tag)
        {
            WWResource resource;
            if (bundles.TryGetValue(tag, out resource))
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
