using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WorldWizards.core.controller.resources;
using WorldWizards.core.entity.common;
using WorldWizards.core.file.utils;
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
        // TODO: Make color changing not stupid after Alphafest
        public Button propFilter, tileFilter, interactableFilter;
        private Color normalColor = new Color32(0x5D, 0x7F, 0xFD, 0xFF);
        private Color pressedColor = new Color32(0x2E, 0xFF, 0x41, 0xFF);
        
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
            
            // TODO: Turn this back on after Alphafest
            //WWMenuBuilder.BuildMenu(buttonPrefab, assetBundleTags, panel, this);
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
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(false, WWType.None);
                var tileFilterColors = tileFilter.colors;
                tileFilterColors.normalColor = normalColor;
                tileFilter.colors = tileFilterColors;
            }
            else
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(true, WWType.Tile);
                var tileFilterColors = tileFilter.colors;
                tileFilterColors.normalColor = pressedColor;
                tileFilter.colors = tileFilterColors;
                
                var propFilterColors = propFilter.colors;
                propFilterColors.normalColor = normalColor;
                propFilter.colors = propFilterColors;
            }
            
            EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        ///     Click function for prop filtering
        /// </summary>
        public void OnClickPropFilter()
        {
            if (ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetDoFilter() && 
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetFilterType() == WWType.Prop)
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(false, WWType.None);
                var propFilterColors = propFilter.colors;
                propFilterColors.normalColor = normalColor;
                propFilter.colors = propFilterColors;
            }
            else
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(true, WWType.Prop);
                var propFilterColors = propFilter.colors;
                propFilterColors.normalColor = pressedColor;
                propFilter.colors = propFilterColors;
                
                var tileFilterColors = tileFilter.colors;
                tileFilterColors.normalColor = normalColor;
                tileFilter.colors = tileFilterColors;
            }
            
            EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        ///     Click handler for interactable filtering
        /// </summary>
        public void OnClickInteractableFilter()
        {
            if (ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetDoFilter() && 
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetFilterType() == WWType.Interactable)
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(false, WWType.None);
            }
            else
            {
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().SetPossibleObjectKeys(true, WWType.Interactable);
            }
        }
        
        // TODO: Delete/move after Alphafest
        public void Load()
        {
            DeleteObjects();
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Load(FileIO.testPath);
        }

        public void DeleteObjects()
        {
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().ClearAll();
        }

        public void ShowControls()
        {
            Debug.Log("Controls");
        }
    }
}