using UnityEngine;
using UnityEngine.UI;
using worldWizards.core.input.Tools;
using worldWizards.core.input.VRControls;
using WorldWizards.core.manager;

namespace worldWizardsCore.core.input.VRControls
{
    public class ColliderUIControls : MonoBehaviour {
    
        public GameObject popupArmMenu;
        private SteamVR_TrackedController controller;
        public Button objPlaceButton;
        public Button objEditButton;

        void Awake()
        {
            if (ManagerRegistry.Instance.menuManager.GetMenuExists("PopupArmMenu"))
            {
                popupArmMenu = ManagerRegistry.Instance.menuManager.GetMenuReference("PopupArmMenu");
            }
            
            controller = GetComponent<SteamVR_TrackedController>();

        }
    
        void OnTriggerEnter (Collider col)
        {
            Debug.Log("Collision");

            switch (col.gameObject.name)
            {
                case "OpenPopupButton":
                    if (popupArmMenu.activeSelf)
                    {
                        popupArmMenu.SetActive(false);
                    }
                    else
                    {
                        popupArmMenu.SetActive(true);
                    }
                    break;
                case "ObjPlaceButton":
                    OnClickObjectPlacement();
                    break;
                case "ObjEditButton":
                    OnClickObjectEdit();
                    break;
            }
        }

        public void OnClickObjectPlacement()
        {
            controller.GetComponent<VRListener>().ChangeTool(typeof(CreateObjectTool));
            objPlaceButton.Select();
        }

        public void OnClickObjectEdit()
        {
            controller.GetComponent<VRListener>().ChangeTool(typeof(EditObjectTool));
            objEditButton.Select();
        }
    }
}
