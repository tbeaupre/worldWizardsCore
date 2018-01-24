using UnityEngine;
using WorldWizards.core.controller.resources;
using WorldWizards.core.entity.gameObject.resource.metaData;

namespace WorldWizards.core.entity.gameObject.resource
{
    public class WWResource
    {
        // Flag for single load upon use.
        private bool loaded;

        private WWResourceMetadata _metadata;

        // Set at time of use.
        private GameObject prefab;

        public WWResource(string assetBundleTag, string path)
        {
            prefab = null;
            _metadata = null;
            this.assetBundleTag = assetBundleTag;
            this.path = path;
        }

        public string assetBundleTag { get; private set; }
        public string path { get; private set; }

        public GameObject GetPrefab()
        {
            if (!loaded)
            {
                Load();
            }
            return prefab;
        }

        public WWResourceMetadata GetMetaData()
        {
            if (!loaded)
            {
                Load();
            }
            return _metadata;
        }

        private void Load()
        {
            LoadPrefab();

            if (prefab != null)
            {
                LoadMetaData();
            }

            loaded = true;
        }

        private void LoadPrefab()
        {
            if (assetBundleTag != null)
            {
                AssetBundle assetBundle = WWAssetBundleController.GetAssetBundle(assetBundleTag);
                if (assetBundle != null)
                {
                    prefab = assetBundle.LoadAsset(path) as GameObject;
                }
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
                _metadata = prefab.GetComponent<WWResourceMetadata>();
            }
        }
    }
}