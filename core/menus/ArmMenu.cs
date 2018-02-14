using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.input.Tools;
using WorldWizards.core.input.VRControls;
using WorldWizards.SteamVR.Scripts;

namespace WorldWizards.core.menus
{
    /// <summary>
    ///     ArmMenu is a VR-only WWMenu that sits on the player's arm and allows them to quickly change controller tools.
    ///     Sets up positioning based on left controller position & rotation
    /// </summary>
    
    public class ArmMenu : WWMenu
    { 
        private GameObject popupArmMenu;
        private GameObject controller;
        public Button objPlaceButton, objEditButton;
        
        private Color normalColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        private Color pressedColor = new Color32(0x2E, 0xFF, 0x41, 0xFF);

        private void Start ()
        {
            Debug.Log("ArmMenu Start");

            if (UnityEngine.XR.XRDevice.isPresent)
            {
                controller = FindObjectOfType<SteamVR_ControllerManager>().left;
                Setup();
            }
            
            foreach (Button b in GetComponentsInChildren<Button>())
            {
                switch (b.name)
                {
                    case "ObjPlaceButton":
                        objPlaceButton = b;
                        Debug.Log("Placement button found");
                        break;
                    case "ObjEditButton":
                        objEditButton = b;
                        Debug.Log("Edit button found");
                        break;
                }
            }
        }

        protected override void Setup()
        {
            // Get list of all buttons on this menu.
            //allButtons = new List<Button>(gameObject.GetComponents<Button>());
            
            // Put arm menu in the right place and parent it to the controller
            transform.rotation = controller.transform.rotation;
            transform.position = controller.transform.position;
            transform.Rotate(90, 0, 0);
            transform.position = new Vector3(transform.position.x, 
                                             transform.position.y + 0.05f,
                                             transform.position.z - 0.35f);
            transform.parent = controller.transform;
        }
        
        
        /**
         * Called when the Object Placement button is hit.
         * Changes the tool on the controller to CreateObjectTool.
         */
        public void OnClickObjectPlacement()
        {
            Debug.Log("Object Placement Tool");
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
            Debug.Log("Object Edit Tool");
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
