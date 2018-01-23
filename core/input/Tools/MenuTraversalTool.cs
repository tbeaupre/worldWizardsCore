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
    /**
     * VR menu traversal with controller.
     * Gives controller a laser pointer that is able to interact with WWButtons.
     */
    public class MenuTraversalTool : Tool
    {
        private SteamVR_LaserPointer laserPointer;
        private GameObject assetBundleMenu;

        void Awake()
        {
            base.Awake();
            assetBundleMenu = ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().GetMenuReference("AssetBundlesMenu");
        }

        private void OnEnable()
        {
            laserPointer = GetComponent<SteamVR_LaserPointer>();
            laserPointer.PointerIn -= HandlePointerIn;
            laserPointer.PointerIn += HandlePointerIn;
            laserPointer.PointerOut -= HandlePointerOut;
            laserPointer.PointerOut += HandlePointerOut;

            laserPointer.active = true;
        }

        public override void OnTriggerUnclick()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }

            base.OnTriggerUnclick();
        }

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
