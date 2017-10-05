using System.Collections.Generic;
using worldWizards.core.entity.gameObject;
using UnityEngine;

namespace worldWizards.core.controller.level
{
    public static class WWResourceController
    {
        public static Dictionary<string, WWResource> bundles = new Dictionary<string, WWResource>();

		/// <summary>
		/// Gets the resource keys by asset bundle tag.
		/// </summary>
		/// <returns>The resource keys by asset bundle.</returns>
		/// <param name="assetBundleTag">Asset bundle tag.</param>
		public static List<string> GetResourceKeysByAssetBundle(string assetBundleTag){
			List<string> filteredKeys = new List<string> ();
			foreach (KeyValuePair<string,WWResource> kvp in bundles) {
				if (kvp.Value.assetBundleTag.Equals(assetBundleTag)){
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
                Debug.Log("A resource with the tag: " + tag + " has not been loaded.");
                return new WWResource(null, null);
            }
        }
    }
}
