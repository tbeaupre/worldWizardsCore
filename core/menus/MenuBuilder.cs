using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.controller.level;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.gameObject.resource;

namespace worldWizardsCore.core.menus
{
    /**
     * MenuBuilder adds a button for each Asset Bundle to the AssetBundlesMenu at runtime.
     * NOTE: Will be refactored to allow generic use among all menus to add buttons at runtime
     */
    // TODO: Convert this to generic menu builder for all menus to use
    public class MenuBuilder : MonoBehaviour
    {

        private GameObject canvas;
        public GameObject panel;
        private Vector3 nextPosition;
        private List<string> assetBundleTags;
        private List<Button> buttons;
        private Button buttonPrefab;

        private List<string> possibleTiles;
        private string currentAssetBundle;
        private WWType filterType;
        private bool doFilter;

        // Use this for initialization
        void Awake ()
        {
            canvas = GameObject.Find("AssetBundlesMenu");
            buttonPrefab = (Button)Resources.Load("Prefabs/Buttons/AssetBundleButton", typeof(Button));

            doFilter = false;
            buttons = new List<Button>();

            BuildMenu();
        }

        // TODO: Use this to build menu instead of Awake()
        /*void OnEnable()
        {
            // TODO: Delete all buttons from last time?
            // Or, find a way to keep track of if the asset bundle list changed and only rebuild if necessary
            BuildMenu();
        }*/

        public void BuildMenu()
        {
            assetBundleTags = new List<string>(WWAssetBundleController.GetAllAssetBundles().Keys);
            //assetBundleTags = new List<string>();
            //assetBundleTags.Add("castle");
            //assetBundleTags.Add("dungeon");
            //assetBundleTags.Add("forest");
            //assetBundleTags.Add("shipwreck");
            //assetBundleTags.Add("ocean");
            
            foreach (string bundle in assetBundleTags)
            {
                AddButton(bundle);
            }
        }

        /**
         * Instantiates a button and adds it to the list of all buttons for this menu
         * 
         * @param bundleTag the string representing the asset bundle tag to be put on the button
         */
        private void AddButton(string bundleTag)
        {
            Button button = Instantiate(buttonPrefab);
            Text text = button.GetComponentInChildren<Text>();
            text.text = bundleTag;

            button.transform.SetParent(panel.transform, false);
            button.transform.localScale = new Vector3(1, 1, 1);
            
            button.GetComponent<WWButton>().SetMetadata(bundleTag);

            button.onClick.AddListener(() => { OnClickBundle(button.GetComponent<WWButton>().GetMetaData()); });
            
            buttons.Add(button);
        }

        // TODO: Put all below functions into AssetBundleMenu : WWMenu
        // Probably not the best place to save this info...
        public List<string> GetPossibleTiles()
        {
            return possibleTiles;
        }
        
        // TODO: Get info from button
        private void OnClickBundle(string tag)
        {
            currentAssetBundle = tag;
            Debug.Log("Asset Bundle clicked: " + tag);
        }

        public void OnClickTileFilter()
        {
            doFilter = true;
            filterType = WWType.Tile;
            Debug.Log("Tile Filter");
        }

        public void OnClickPropFilter()
        {
            doFilter = true;
            filterType = WWType.Prop;
            Debug.Log("Prop Filter");
        }

        public void OnClickInteractableFilter()
        {
            doFilter = true;
            filterType = WWType.Interactable;
            Debug.Log("Interactable Filter");
        }

        public void GetFilteredPossibleTilesKeys()
        {
            if (doFilter)
            {
                possibleTiles = WWResourceController.GetResourceKeysByAssetBundleFiltered(currentAssetBundle, filterType);
            }
            else
            {
                possibleTiles = WWResourceController.GetResourceKeysByAssetBundle(currentAssetBundle);
            }
        }

    }
}
