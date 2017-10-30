using UnityEngine;
using WorldWizards.core.controller.level;

namespace WorldWizards.core.entity.gameObject.resource
{
    public class WWResource
    {
        // Flag for single load upon use.
        private bool loaded;

        private WWResourceMetaData metaData;

        // Set at time of use.
        private GameObject prefab;

        public WWResource(string assetBundleTag, string path)
        {
            prefab = null;
            metaData = null;
            this.assetBundleTag = assetBundleTag;
            this.path = path;
        }

        public string assetBundleTag { get; private set; }
        public string path { get; private set; }

        public GameObject GetPrefab()
        {
            if (!loaded)
                Load();
            return prefab;
        }

        public WWResourceMetaData GetMetaData()
        {
            if (!loaded)
                Load();
            return metaData;
        }

        private void Load()
        {
            LoadPrefab();

            if (prefab != null)
                LoadMetaData();

            loaded = true;
        }

        private void LoadPrefab()
        {
            if (assetBundleTag != null)
            {
                var assetBundle = WWAssetBundleController.GetAssetBundle(assetBundleTag);
                if (assetBundle != null)
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
                metaData = prefab.GetComponent<WWResourceMetaData>();
        }
    }
}