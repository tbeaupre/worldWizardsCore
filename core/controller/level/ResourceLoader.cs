using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using worldWizards.core.entity.gameObject;

namespace worldWizards.core.controller.level
{
    public class ResourceLoader
    {
        public static void LoadResources(string[] assetBundlePaths)
        {
            foreach (string assetBundlePath in assetBundlePaths)
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
                            WWResourceController.LoadResource(assetBundlePath + assetName, assetBundlePath, assetName);
                        }
                    }
                    assetBundle.Unload(true);
                    WWAssetBundleController.LoadAssetBundle(assetBundlePath, assetBundlePath);
                }
            }
        }
    }
}
