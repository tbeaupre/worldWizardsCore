using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.gameObject.resource;

namespace WorldWizards.core.controller.level
{
    public static class WWResourceController
    {
        public static Dictionary<string, WWResource> bundles = new Dictionary<string, WWResource>();

        /// <summary>
        ///     Gets the resource keys by asset bundle tag.
        /// </summary>
        /// <returns>The resource keys by asset bundle.</returns>
        /// <param name="assetBundleTag">Asset bundle tag.</param>
        public static List<string> GetResourceKeysByAssetBundle(string assetBundleTag)
        {
            Debug.Log(bundles.Keys);
            var filteredKeys = new List<string>();
            foreach (KeyValuePair<string, WWResource> kvp in bundles)
                if (kvp.Value.assetBundleTag.Equals(assetBundleTag))
                {
                    filteredKeys.Add(kvp.Key);
                }
            return filteredKeys;
        }

        public static List<string> GetResourceKeysByAssetBundleFiltered(string assetBundleTag, WWType type)
        {
            var filteredKeys = new List<string>();
            foreach (KeyValuePair<string, WWResource> kvp in bundles)
                if (kvp.Value.assetBundleTag.Equals(assetBundleTag))
                {
                    if (kvp.Value.GetMetaData().wwObjectMetaData.type.Equals(type))
                    {
                        filteredKeys.Add(kvp.Key);
                    }
                }
            return filteredKeys;
        }

        public static void LoadResource(string tag, string assetBundleTag, string path)
        {
            if (bundles.ContainsKey(tag))
            {
                Debug.Log("resourceTag: " + tag + " has already been used.");
            }
            else
            {
                var resource = new WWResource(assetBundleTag, path);
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
            Debug.Log("A resource with the tag: " + tag + " has not been loaded.");
            return new WWResource(null, null);
        }
    }
}