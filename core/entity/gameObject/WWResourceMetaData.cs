using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace worldWizards.core.entity.gameObject
{
    [System.Serializable]
    public class WWResourceMetaData
    {
        public AssetBundle assetBundle = null;
        public string path = "";
        public WWWalls wallBarriers = WWWalls.Bottom;

        public UnityEngine.Object GetObject()
        {
            UnityEngine.Object prefab;
            if (assetBundle)
            {
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
