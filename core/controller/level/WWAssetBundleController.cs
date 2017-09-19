using System.Collections.Generic;
using UnityEngine;

namespace worldWizards.core.controller.level
{
    public static class WWAssetBundleController
    {
        static Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

        public static void LoadAssetBundle(string tag, string path)
        {
            if (bundles.ContainsKey(tag))
            {
                Debug.Log("bundleTag: " + tag + " has already been used.");
            }
            else
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
                if (assetBundle == null)
                {
                    Debug.Log("Unable to load Asset Bundle");
                }
                else
                {
                    bundles.Add(tag, assetBundle);
                }
            }
        }

        public static AssetBundle GetAssetBundle(string tag)
        {
            AssetBundle assetBundle;
            if (bundles.TryGetValue(tag, out assetBundle))
            {
                return assetBundle;
            }
            else
            {
                Debug.Log("An asset bundle with this tag has not been loaded.");
                return null;
            }
        }
    }
}
