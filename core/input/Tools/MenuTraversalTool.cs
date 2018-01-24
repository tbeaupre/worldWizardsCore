using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using worldWizards.core.input.Tools;
using worldWizards.core.input.VRControls;
using worldWizardsCore.core.manager;
using worldWizardsCore.core.menus;
using WorldWizards.core.manager;

namespace worldWizardsCore.core.input.Tools
{

    /// <summary>
    ///     VR menu traversal with controller
    ///     Gives controller a laser pointer that is able to interact with WWButtons.
    /// </summary>
    
    public class MenuTraversalTool : Tool
    {
        private SteamVR_LaserPointer laserPointer;
        private GameObject assetBundleMenu;

        void Awake()
        {
            base.Awake();
            assetBundleMenu = ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().GetMenuReference("AssetBundlesMenu");
        }

        /// <summary>
        ///     Set up the laser pointer.
        /// </summary>
        private void OnEnable()
        {
            laserPointer = GetComponent<SteamVR_LaserPointer>();
            laserPointer.PointerIn -= HandlePointerIn;
            laserPointer.PointerIn += HandlePointerIn;
            laserPointer.PointerOut -= HandlePointerOut;
            laserPointer.PointerOut += HandlePointerOut;

            laserPointer.active = true;
        }

        /// <summary>
        ///     Handle click event with WWButton.
        /// </summary>
        public override void OnTriggerUnclick()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }

            base.OnTriggerUnclick();
        }

        /// <summary>
        ///     Gets rid of Asset Bundle Menu on menu button click.
        ///     Changes tool on controller to whatever was being used before the menu opened.
        /// </summary>
        public override void OnMenuUnclick()
        {
            if (assetBundleMenu.activeSelf)
            {   
                assetBundleMenu.SetActive(false);
                
                // Go back to tool we were using before
                SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
                controllerManager.right.GetComponent<VRListener>().ChangeToPreviousTool();
            }
            base.OnMenuUnclick();
        }

        /// <summary>
        ///     Handles interacting with WWButtons with the laser pointer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlePointerIn(object sender, PointerEventArgs e)
        {
            var button = e.target.GetComponent<Button>();
            if (button != null)
            {
                button.Select();
                Debug.Log("HandlePointerIn", e.target.gameObject);
            }
        }

        private void HandlePointerOut(object sender, PointerEventArgs e)
        {
            var button = e.target.GetComponent<Button>();
            if (button != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("HandlePointerOut", e.target.gameObject);
            }
        }
    }
}
