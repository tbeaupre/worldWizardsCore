using System.Collections.Generic;
using UnityEngine;
using worldWizards.core.entity.gameObject;
using System.IO;

namespace worldWizards.core.controller.level
{
    public class ResourceLoader
    {
        private static List<string> FindAssetBundlePaths()
        {
            List<string> results = new List<string>();

            string assetBundlesDirPath = Application.dataPath + "/../AssetBundles/";
            DirectoryInfo assetBundlesDirInfo = new DirectoryInfo(assetBundlesDirPath);

            FileInfo[] manifestList = assetBundlesDirInfo.GetFiles("*.manifest");

            foreach (FileInfo manifest in manifestList)
            {
                StreamReader sr = manifest.OpenText();
                string s = "";
                if ((s = sr.ReadToEnd()) != null)
                {
                    if (s.Contains(".prefab"))
                    {
                        string assetBundlePath = Path.ChangeExtension(manifest.FullName, null);
                        results.Add(assetBundlePath);
                    }
                }
            }

            return results;
        }

        public static void LoadResources()
        {
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
                        UnityEngine.GameObject obj = assetBundle.LoadAsset(assetName) as GameObject;
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
