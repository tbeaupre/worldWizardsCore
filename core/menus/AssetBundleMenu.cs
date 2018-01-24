using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.controller.resources;
using WorldWizards.core.entity.common;
using WorldWizards.core.manager;

namespace WorldWizards.core.menus
{
    /// <summary>
    ///     Sets up and handles input to AssetBundleMenu
    /// </summary>
    public class AssetBundleMenu : WWMenu
    {
        private List<string> assetBundleTags;
        private Button buttonPrefab;
        private GameObject panel;

        private string currentAssetBundle;
        private WWType filterType;
        private bool doFilter;
        
        private void Start()
        {
            Debug.Log("AssetBundleMenu Start");
            Setup();
        }
        
        protected override void Setup()
        {
            base.Setup();

            // Default value
            currentAssetBundle = "ww_basic_assets";
            
            buttonPrefab = (Button)Resources.Load("Prefabs/Buttons/AssetBundleButton", typeof(Button));
            assetBundleTags = new List<string>(WWAssetBundleController.GetAllAssetBundles().Keys);
            
            panel = GameObject.FindWithTag("UIPanel");
            
            WWMenuBuilder.BuildMenu(buttonPrefab, assetBundleTags, panel, this);
        }

        /// <summary>
        ///     Adds buttons to the menus list of all of its buttons.
        ///     Adds click listener to button.
        /// </summary>
        /// <param name="button">The button to be added to the menu</param>
        public override void AddButton(Button button)
        {
            base.AddButton(button);
            button.onClick.AddListener(() => { OnClickBundle(button.GetComponent<WWButton>().GetMetaData()); });
        }
        
        /// <summary>
        ///     Click handler for asset bundle buttons
        /// </summary>
        /// <param name="tag">The tag</param>
        private void OnClickBundle(string tag)
        {
            currentAssetBundle = tag;
            Debug.Log("Asset Bundle clicked: " + tag);
        }

        /// <summary>
        ///     Click handler for tile filtering
        /// </summary>
        public void OnClickTileFilter()
        {
            if (doFilter && filterType == WWType.Tile)
            {
                doFilter = false;
            }
            else
            {
                doFilter = true;
                filterType = WWType.Tile;
            }
            Debug.Log("Tile Filter");
        }

        /// <summary>
        ///     Click function for prop filtering
        /// </summary>
        public void OnClickPropFilter()
        {
            if (doFilter && filterType == WWType.Prop)
            {
                doFilter = false;
            }
            else
            {
                doFilter = true;
                filterType = WWType.Prop;
            }
            Debug.Log("Prop Filter");
        }

        /// <summary>
        ///     Click handler for interactable filtering
        /// </summary>
        public void OnClickInteractableFilter()
        {
            if (doFilter && filterType == WWType.Interactable)
            {
                doFilter = false;
            }
            else
            {
                doFilter = true;
                filterType = WWType.Interactable;
            }
            Debug.Log("Interactable Filter");
        }

        /// <summary>
        ///     Give the WWObjectGunManager the settings from this menu
        /// </summary>
        public void OnDisable()
        { 
            if (ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>() != null)
            {
                Debug.Log("doFilter: " + doFilter + ", filterType: " + filterType + ", currentAssetBundle: " + currentAssetBundle);
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetDoFilter(doFilter);
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetFilterType(filterType);
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetCurrentAssetBundle(currentAssetBundle);
            }
        }
    }
}