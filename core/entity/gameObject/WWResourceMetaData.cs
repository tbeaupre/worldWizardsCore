using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using worldWizards.core.controller.level;

namespace worldWizards.core.entity.gameObject
{
    [System.Serializable]
    public class WWResourceMetaData
    {
        public string assetBundleTag = null;
        public string path = "";
        public WWWalls wallBarriers = WWWalls.Bottom;

        public UnityEngine.Object GetObject()
        {
            UnityEngine.Object prefab;
            if (assetBundleTag != null)
            {
                AssetBundle assetBundle = WWAssetBundleController.GetAssetBundle(assetBundleTag);
                prefab = assetBundle.LoadAsset(path);
            }
            else
            {
                prefab = Resources.Load(path);
            }
            return prefab;
        }
    }
}
