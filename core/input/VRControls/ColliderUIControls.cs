using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.input.Tools;
using WorldWizards.core.manager;
using WorldWizards.core.menus;
using WorldWizards.SteamVR.Extras;

namespace WorldWizards.core.input.VRControls
{
    /**
     * ColliderUIControls is attached to the right hand controller to allow collision with the Arm Menu.
     * 
     */
    public class ColliderUIControls : MonoBehaviour
    {

        public GameObject popupArmMenu;
        public GameObject armMenu;
        private SteamVR_TrackedController controller;
        public Button objPlaceButton;
        public Button objEditButton;
        private Color normalColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        private Color pressedColor = new Color32(0x2E, 0xFF, 0x41, 0xFF);

        void Start()
        {
            controller = GetComponent<SteamVR_TrackedController>();
        }
    
        /**
         * Checks for collision with Arm Menu.
         * 
         */
        void OnTriggerEnter (Collider col)
        {
            Debug.Log("Collision");

            if (popupArmMenu == null)
            {
                popupArmMenu = GameObject.Find("PopupArmMenu");
            }

            if (armMenu == null)
            {
                armMenu = GameObject.Find("ArmMenu(Clone)");

                if (armMenu != null)
                {
                    Debug.Log("ArmMenu found");
                    objEditButton = armMenu.GetComponent<ArmMenu>().objEditButton;
                    objPlaceButton = armMenu.GetComponent<ArmMenu>().objPlaceButton;
                }

            }

            switch (col.gameObject.name)
            {
                case "OpenPopupButton":
                    if (popupArmMenu.activeSelf)
                    {
                        Debug.Log("Popup Menu inactive");
                        popupArmMenu.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Popup Menu active");
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
        
        /**
         * Called when the Object Placement button is hit.
         * Changes the tool on the controller to CreateObjectTool.
         */
        public void OnClickObjectPlacement()
        {
            controller.GetComponent<VRListener>().ChangeTool(typeof(CreateObjectTool));
            
            var objPlaceColors = objPlaceButton.colors;
            objPlaceColors.normalColor = pressedColor;
            objPlaceButton.colors = objPlaceColors;
                
            var objEditColors = objEditButton.colors;
            objEditColors.normalColor = normalColor;
            objEditButton.colors = objEditColors;
        }

        /**
         * Called when the Object Edit button is hit.
         * Changes the tool on the controller to EditObjectTool.
         */
        public void OnClickObjectEdit()
        {
            controller.GetComponent<VRListener>().ChangeTool(typeof(EditObjectTool));
            
            var objEditColors = objEditButton.colors;
            objEditColors.normalColor = pressedColor;
            objEditButton.colors = objEditColors;
            
            var objPlaceColors = objPlaceButton.colors;
            objPlaceColors.normalColor = normalColor;
            objPlaceButton.colors = objPlaceColors;
        }
    }
}
