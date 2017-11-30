using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WorldWizards.core.entity.gameObject.resource;

namespace WorldWizards.core.controller.level
{
    public class ResourceLoader
    {
        private static bool loaded = false;
        
        public static List<string> FindAssetBundlePaths()
        {
            var results = new List<string>();

            string assetBundlesDirPath = Application.dataPath + "/../AssetBundles/";
            var assetBundlesDirInfo = new DirectoryInfo(assetBundlesDirPath);

            FileInfo[] manifestList = assetBundlesDirInfo.GetFiles("*.manifest");

            foreach (FileInfo manifest in manifestList)
            {
                StreamReader sr = manifest.OpenText();
                var s = "";
                if ((s = sr.ReadToEnd()) != null)
                {
                    if (s.Contains(".prefab"))
                    {
                        string assetBundlePath = Path.ChangeExtension(manifest.FullName, null);
                        results.Add(assetBundlePath);
                    }
                }
                sr.Close();
            }

            return results;
        }

        public static void LoadResources()
        {
            if (loaded) return; // Check if resources have already been loaded.

            loaded = true;
            foreach (string assetBundlePath in FindAssetBundlePaths())
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
                if (assetBundle == null)
                {
                    Debug.Log("Unable to load Asset Bundle");
                }
                else
                {
                    string[] allAssetNames = assetBundle.GetAllAssetNames();
                    foreach (string assetName in allAssetNames)
                    {
                        var obj = assetBundle.LoadAsset(assetName) as GameObject;
                        if (obj != null && obj.GetComponent<WWResourceMetaData>() != null)
                        {
                            string assetBundleTag = Path.GetFileName(assetBundlePath);
                            string tag = assetBundleTag + "_" + Path.GetFileNameWithoutExtension(assetName);
                            WWResourceController.LoadResource(tag, assetBundleTag, assetName);
                        }
                    }
                    assetBundle.Unload(true);
                    WWAssetBundleController.LoadAssetBundle(Path.GetFileName(assetBundlePath), assetBundlePath);
                }
            }
        }
    }
}