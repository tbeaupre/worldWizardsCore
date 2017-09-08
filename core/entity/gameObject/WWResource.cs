using worldWizards.core.controller.level;
using UnityEngine;

namespace worldWizards.core.entity.gameObject
{
    public class WWResource
    {

        string assetBundleTag = null;
        // Set at time of use.
        GameObject prefab = null;
        WWResourceMetaData metaData = null;
		public string path {get;}

        // Flag for single load upon use.
        bool loaded = false;

        public WWResource(string assetBundleTag, string path)
        {
            this.assetBundleTag = assetBundleTag;
            this.path = path;
        }

        public GameObject GetPrefab()
        {
            if (!loaded)
            {
                Load();
            }
            return prefab;
        }

        public WWResourceMetaData GetMetaData()
        {
            if (!loaded)
            {
                Load();
            }
            return metaData;
        }

        private void Load()
        {
            LoadPrefab();
            LoadMetaData();
            loaded = true;
        }

        private void LoadPrefab()
        {
            if (assetBundleTag != null)
            {
                AssetBundle assetBundle = WWAssetBundleController.GetAssetBundle(assetBundleTag);
                prefab = assetBundle.LoadAsset(path) as GameObject;
            }
            else
            {
                prefab = Resources.Load(path) as GameObject;
            }
        }

        private void LoadMetaData()
        {
            if (prefab != null)
            {
                metaData = prefab.GetComponent<WWResourceMetaData>();
            }
        }
    }
}
