using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WorldWizards.core.entity.gameObject.resource;

namespace WorldWizards.core.controller.level
{
    public class ResourceLoader
    {
        public static List<string> FindAssetBundlePaths()
        {
            var results = new List<string>();

            var assetBundlesDirPath = Application.dataPath + "/../AssetBundles/";
            var assetBundlesDirInfo = new DirectoryInfo(assetBundlesDirPath);

            var manifestList = assetBundlesDirInfo.GetFiles("*.manifest");

            foreach (var manifest in manifestList)
            {
                var sr = manifest.OpenText();
                var s = "";
                if ((s = sr.ReadToEnd()) != null)
                    if (s.Contains(".prefab"))
                    {
                        var assetBundlePath = Path.ChangeExtension(manifest.FullName, null);
                        results.Add(assetBundlePath);
                    }
                sr.Close();
            }

            return results;
        }

        public static void LoadResources()
        {
            foreach (var assetBundlePath in FindAssetBundlePaths())
            {
                var assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
                if (assetBundle == null)
                {
                    Debug.Log("Unable to load Asset Bundle");
                }
                else
                {
                    var allAssetNames = assetBundle.GetAllAssetNames();
                    foreach (var assetName in allAssetNames)
                    {
                        var obj = assetBundle.LoadAsset(assetName) as GameObject;
                        if (obj != null && obj.GetComponent<WWResourceMetaData>() != null)
                        {
                            var assetBundleTag = Path.GetFileName(assetBundlePath);
                            var tag = assetBundleTag + "_" + Path.GetFileNameWithoutExtension(assetName);
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