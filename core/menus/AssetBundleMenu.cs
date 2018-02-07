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
        
        void Start()
        {
            Debug.Log("AssetBundleMenu Start");
            Setup();
        }

        void Awake()
        {
            inFrontOfCamera = true;
            Debug.Log("AssetBundleMenu Awake: inFrontOfCamera = " + inFrontOfCamera);
        }
        
        protected override void Setup()
        {
            base.Setup();

            inFrontOfCamera = true;
            
            Debug.Log("AssetBundleMenu Start: inFrontOfCamera = " + inFrontOfCamera);
            
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
        /// <param name="assetBundleTag">The tag for the asset bundle</param>
        private void OnClickBundle(string assetBundleTag)
        {
            ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(assetBundleTag);
        }

        /// <summary>
        ///     Click handler for tile filtering
        /// </summary>
        public void OnClickTileFilter()
        {
            if (ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetDoFilter() && 
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetFilterType() == WWType.Tile)
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(false, WWType.Tile);
            }
            else
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(true, WWType.Tile);
            }
        }

        /// <summary>
        ///     Click function for prop filtering
        /// </summary>
        public void OnClickPropFilter()
        {
            if (ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetDoFilter() && 
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetFilterType() == WWType.Prop)
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(false, WWType.Prop);
            }
            else
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(true, WWType.Prop);
            }
        }

        /// <summary>
        ///     Click handler for interactable filtering
        /// </summary>
        public void OnClickInteractableFilter()
        {
            if (ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetDoFilter() && 
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetFilterType() == WWType.Interactable)
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(false, WWType.Interactable);
            }
            else
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(true, WWType.Interactable);
            }
        }
    }
}