using worldWizards.core.controller.level;
using UnityEngine;

namespace worldWizards.core.entity.gameObject
{
    public class WWResource
    {

		public string assetBundleTag { get; }
        public string path { get; }
        // Set at time of use.
		private GameObject prefab;
		private WWResourceMetaData metaData;

        // Flag for single load upon use.
        bool loaded = false;

        public WWResource(string assetBundleTag, string path)
        {
			this.prefab = null;
			this.metaData = null;
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
                metaData = prefab.GetComponent<WWResourceMetaData>();
            }
        }
    }
}
